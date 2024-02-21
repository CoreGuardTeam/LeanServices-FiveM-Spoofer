using System;
using Hollow.Other;

namespace Hollow.Tasks
{
	// Token: 0x02000014 RID: 20
	internal class check
	{
		// Token: 0x060000AF RID: 175 RVA: 0x0000E718 File Offset: 0x0000C918
		public static bool serials()
		{
			bool flag;
			try
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Disk Drive");
				Console.ResetColor();
				Natives.GetWMICProperty("Win32_DiskDrive", "Model");
				Natives.GetWMICProperty("Win32_DiskDrive", "SerialNumber");
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("CPU");
				Console.ResetColor();
				Natives.GetWMICProperty("Win32_Processor", "SerialNumber");
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("BIOS");
				Console.ResetColor();
				Natives.GetWMICProperty("Win32_BIOS", "SerialNumber");
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Motherboard");
				Console.ResetColor();
				Natives.GetWMICProperty("Win32_BaseBoard", "SerialNumber");
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("smBIOS UUID");
				Console.ResetColor();
				Natives.GetWMICProperty("Win32_ComputerSystemProduct", "UUID");
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}
	}
}
