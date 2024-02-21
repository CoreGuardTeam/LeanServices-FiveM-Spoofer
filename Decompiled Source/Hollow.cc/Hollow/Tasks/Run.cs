using System;
using System.Diagnostics;

namespace Hollow.Tasks
{
	// Token: 0x02000015 RID: 21
	internal class Run
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x0000E840 File Offset: 0x0000CA40
		public static void CMD(string cmd, bool showoutput = false)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = "/c " + cmd;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			string text = process.StandardOutput.ReadToEnd();
			if (showoutput)
			{
				Console.WriteLine(text);
			}
			process.WaitForExit();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000C0DA File Offset: 0x0000A2DA
		public static void resetwinmgmt()
		{
			Run.CMD("net stop winmgmt /y && net start winmgmt /y && sc stop winmgmt && sc start winmgmt", false);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000E8CC File Offset: 0x0000CACC
		public static void runpdrv()
		{
			Run.CMD("C:\\Nexus\\zhjers.exe /SU auto", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /SS \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /SV \"1.0\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CSK \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CM  \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /SP \"MS-7D22\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /SM \"Micro-Star International Co., Ltd.\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /SK \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /SF \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /BM \"Micro-Star International Co., Ltd.\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /BP \"H510M-A PRO (MS-7D22)\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /BV \"1.0\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /BT \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /BLC \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /PSN \"To Be Filled By O.E.M.\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /PAT \"To Be Filled By O.E.M.\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /PPN \"To Be Filled By O.E.M.\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CSK \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CS \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CV \"1.0\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CM \"Micro-Star International Co., Ltd.\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CA \"Default string\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CO \"0000 0000h\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /CT \"03h\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /IV \"3.80\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /IVN \"American Megatrends International, LLC.\"", false);
			Run.CMD("C:\\Nexus\\zhjers.exe /BS \"%random%%random%%random%%random%%random%\"", false);
		}
	}
}
