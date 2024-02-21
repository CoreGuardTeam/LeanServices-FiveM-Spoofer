using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace KeyAuth
{
	// Token: 0x0200000E RID: 14
	public class json_wrapper
	{
		// Token: 0x0600008B RID: 139 RVA: 0x0000DB5C File Offset: 0x0000BD5C
		public static bool is_serializable(Type to_check)
		{
			return to_check.IsSerializable || to_check.IsDefined(typeof(DataContractAttribute), true);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000DB8C File Offset: 0x0000BD8C
		public json_wrapper(object obj_to_work_with)
		{
			this.current_object = obj_to_work_with;
			Type type = this.current_object.GetType();
			this.serializer = new DataContractJsonSerializer(type);
			bool flag = !json_wrapper.is_serializable(type);
			if (flag)
			{
				throw new Exception(string.Format("the object {0} isn't a serializable", this.current_object));
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000DBE4 File Offset: 0x0000BDE4
		public object string_to_object(string json)
		{
			object obj;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(json)))
			{
				obj = this.serializer.ReadObject(memoryStream);
			}
			return obj;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000DC34 File Offset: 0x0000BE34
		public T string_to_generic<T>(string json)
		{
			return (T)((object)this.string_to_object(json));
		}

		// Token: 0x0400003A RID: 58
		private DataContractJsonSerializer serializer;

		// Token: 0x0400003B RID: 59
		private object current_object;
	}
}
