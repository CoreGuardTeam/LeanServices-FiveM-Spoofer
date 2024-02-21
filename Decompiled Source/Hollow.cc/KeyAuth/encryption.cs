using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace KeyAuth
{
	// Token: 0x0200000D RID: 13
	public static class encryption
	{
		// Token: 0x06000086 RID: 134 RVA: 0x0000D9C0 File Offset: 0x0000BBC0
		public static string HashHMAC(string enckey, string resp)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(enckey);
			byte[] bytes2 = Encoding.ASCII.GetBytes(resp);
			return encryption.byte_arr_to_str(new HMACSHA256(bytes).ComputeHash(bytes2));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000D9FC File Offset: 0x0000BBFC
		public static string byte_arr_to_str(byte[] ba)
		{
			StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000DA4C File Offset: 0x0000BC4C
		public static byte[] str_to_byte_arr(string hex)
		{
			byte[] array2;
			try
			{
				int length = hex.Length;
				byte[] array = new byte[length / 2];
				for (int i = 0; i < length; i += 2)
				{
					array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
				}
				array2 = array;
			}
			catch
			{
				api.error("The session has ended, open program again.");
				Environment.Exit(0);
				array2 = null;
			}
			return array2;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000DAC8 File Offset: 0x0000BCC8
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool CheckStringsFixedTime(string str1, string str2)
		{
			bool flag = str1.Length != str2.Length;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				int num = 0;
				for (int i = 0; i < str1.Length; i++)
				{
					num |= (int)(str1[i] ^ str2[i]);
				}
				flag2 = num == 0;
			}
			return flag2;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000DB2C File Offset: 0x0000BD2C
		public static string iv_key()
		{
			return Guid.NewGuid().ToString().Substring(0, 16);
		}
	}
}
