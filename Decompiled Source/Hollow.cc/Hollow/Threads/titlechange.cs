using System;

namespace Hollow.Threads
{
	// Token: 0x02000013 RID: 19
	internal class titlechange
	{
		// Token: 0x060000AC RID: 172 RVA: 0x0000C0BB File Offset: 0x0000A2BB
		public static void changetitle_tick()
		{
			for (;;)
			{
				Console.Title = "Leanservices.cc | " + titlechange.randomString(20);
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000E6C0 File Offset: 0x0000C8C0
		private static string randomString(int length)
		{
			Random random = new Random();
			char[] array = new char[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+=[]{}|;:,.<>?"[random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+=[]{}|;:,.<>?".Length)];
			}
			return new string(array);
		}
	}
}
