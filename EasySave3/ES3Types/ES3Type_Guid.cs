using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000092 RID: 146
	[Preserve]
	[ES3Properties(new string[]
	{
		"value"
	})]
	public class ES3Type_Guid : ES3Type
	{
		// Token: 0x0600035F RID: 863 RVA: 0x000112CD File Offset: 0x0000F4CD
		public ES3Type_Guid() : base(typeof(Guid))
		{
			ES3Type_Guid.Instance = this;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000112E8 File Offset: 0x0000F4E8
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WriteProperty("value", ((Guid)obj).ToString(), ES3Type_string.Instance);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00011319 File Offset: 0x0000F519
		public override object Read<T>(ES3Reader reader)
		{
			return Guid.Parse(reader.ReadProperty<string>(ES3Type_string.Instance));
		}

		// Token: 0x040000D5 RID: 213
		public static ES3Type Instance;
	}
}
