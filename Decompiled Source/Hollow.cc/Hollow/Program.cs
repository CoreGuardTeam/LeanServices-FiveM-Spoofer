using System;
using System.IO;
using System.Threading.Tasks;
using Hollow.Files;
using Hollow.Other;
using Hollow.Tasks;
using Hollow.Threads;
using KeyAuth;
using PhantomSolutions.Files;

namespace Hollow
{
	// Token: 0x0200000F RID: 15
	internal class Program
	{
		// Token: 0x0600008F RID: 143 RVA: 0x0000DC54 File Offset: 0x0000BE54
		private static void Main(string[] args)
		{
			Program.KeyAuthApp.init();
			index.init();
			Natives.SetLayeredWindowAttributes(Natives.currenthandle(), 0U, 200, 2U);
			Console.Title = "Lean.cc";
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("This application was cracked by netENGINE!");
			Console.WriteLine("                                                                         ");
			Console.WriteLine("                                                                         ");
			Console.WriteLine("      LEAN SPOOFER READY FOR REMOVE FIVEM BANS PERMANENTLY               ");
			Console.WriteLine("                                                                         ");
			Console.WriteLine("      if you need help join my discord & open ticket:                    ");
			Console.WriteLine("                                                                         ");
			Console.WriteLine("      discord.gg/leaservices                                             ");
			Console.WriteLine("                                                                         ");
			Console.WriteLine("                                                                         ");
			Console.WriteLine("Bypass Login: Success");
			Console.WriteLine("Cracked by netENGINE Team!");
			Console.Beep(400, 400);
			Task.Delay(1000).Wait();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Enter help for commands");
			for (;;)
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write("Leanservices.cc ");
				Console.ResetColor();
				string text = Console.ReadLine();
				string text2 = text;
				if (!text2.StartsWith("help"))
				{
					if (!text2.StartsWith("perm"))
					{
						if (!text2.StartsWith("clean"))
						{
							if (!text2.StartsWith("serial"))
							{
								if (!text2.StartsWith("check"))
								{
									Console.WriteLine(text + " is not a valid command");
									goto IL_2B1;
								}
								if (!check.serials())
								{
									Console.WriteLine("Something went wrong...");
									goto IL_2B1;
								}
								goto IL_2B1;
							}
							else
							{
								if (!check.serials())
								{
									Console.WriteLine("Something went wrong...");
									goto IL_2B1;
								}
								goto IL_2B1;
							}
						}
						else
						{
							try
							{
								if (!Directory.Exists("C:\\Malaia.cc"))
								{
									Directory.CreateDirectory("C:\\Malaia.cc");
								}
								File.WriteAllBytes("C:\\Malaia.cc\\AppleCleaner.exe", applecleaner_2.rawData);
								Run.CMD("start C:\\Malaia.cc\\AppleCleaner.exe", false);
								File.Delete("C:\\Malaia.cc\\AppleCleaner.exe");
								goto IL_2B1;
							}
							catch
							{
								goto IL_2B1;
							}
						}
					}
					try
					{
						Natives.BlockInput(true);
						if (!Directory.Exists("C:\\Malaia.cc"))
						{
							Directory.CreateDirectory("C:\\Malaia.cc");
						}
						File.WriteAllBytes("C:\\Malaia.cc\\zhjers.exe", PLoader.rawData);
						File.WriteAllBytes("C:\\Malaia.cc\\AMIFLDRV64.SYS", PDrv1.rawData);
						File.WriteAllBytes("C:\\Malaia.cc\\dvlwwwdrv64.sys", PDrv2.rawData);
						Run.runpdrv();
						Run.resetwinmgmt();
						File.Delete("C:\\Malaia.cc\\zhjers.exe");
						File.Delete("C:\\Malaia.cc\\AMIFLDRV64.SYS");
						File.Delete("C:\\Malaia.cc\\dvlwwwdrv64.sys");
						if (File.Exists("C:\\Nexus\\AppleCleaner.exe"))
						{
							File.Delete("C:\\Malaia.cc\\AppleCleaner.exe");
						}
						Natives.BlockInput(false);
						Console.ForegroundColor = ConsoleColor.Green;
						Console.Write("[+]");
						Console.ResetColor();
						Console.WriteLine("Successfuly Spoofed");
						goto IL_2B1;
					}
					catch (Exception)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write("[-]");
						Console.ResetColor();
						Console.WriteLine(" Something went wrong...");
						goto IL_2B1;
					}
					goto IL_2A7;
				}
				goto IL_2A7;
				IL_2B1:
				Console.WriteLine();
				continue;
				IL_2A7:
				Console.WriteLine("lean.cc CMDS\nperm - Perm spoof\ncheck - check serials | aliases: serials, serial\nclean - AppleCleaner");
				goto IL_2B1;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000C084 File Offset: 0x0000A284
		// (set) Token: 0x06000091 RID: 145 RVA: 0x0000C08B File Offset: 0x0000A28B
		public static object Consolecolor { get; private set; }

		// Token: 0x0400003C RID: 60
		public static api KeyAuthApp = new api("Lean swoofer", "vsRiSOvKdL", "40c9466580bd49afc68d0f9d7a0338cb390f9e968e63532b389e93b661433ad9", "1.1");
	}
}
