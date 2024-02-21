using System;
using System.Threading;

namespace Hollow.Threads
{
	// Token: 0x02000012 RID: 18
	internal class index
	{
		// Token: 0x060000AA RID: 170 RVA: 0x0000E66C File Offset: 0x0000C86C
		public static void init()
		{
			new Thread(new ThreadStart(titlechange.changetitle_tick)).Start();
			new Thread(new ThreadStart(blacklisted_programs.KillBlackPrograms)).Start();
			new Thread(new ThreadStart(blacklisted_programs.MemoryDumpProtection)).Start();
		}
	}
}
