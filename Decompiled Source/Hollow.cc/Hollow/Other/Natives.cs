using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace Hollow.Other
{
	// Token: 0x02000016 RID: 22
	internal class Natives
	{
		// Token: 0x060000B5 RID: 181
		[DllImport("user32.dll")]
		public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

		// Token: 0x060000B6 RID: 182
		[DllImport("user32.dll")]
		public static extern bool BlockInput(bool fkk);

		// Token: 0x060000B7 RID: 183 RVA: 0x0000EA20 File Offset: 0x0000CC20
		public static IntPtr currenthandle()
		{
			return Process.GetCurrentProcess().MainWindowHandle;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000EA3C File Offset: 0x0000CC3C
		public static void GetWMICProperty(string className, string propertyName)
		{
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * FROM " + className).Get())
			{
				Console.WriteLine(((ManagementObject)managementBaseObject)[propertyName]);
			}
		}
	}
}
