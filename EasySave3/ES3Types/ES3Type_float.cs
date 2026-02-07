using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000041 RID: 65
	[Preserve]
	public class ES3Type_float : ES3Type
	{
		// Token: 0x0600028E RID: 654 RVA: 0x00009EFF File Offset: 0x000080FF
		public ES3Type_float() : base(typeof(float))
		{
			this.isPrimitive = true;
			ES3Type_float.Instance = this;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00009F1E File Offset: 0x0000811E
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((float)obj);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00009F2C File Offset: 0x0000812C
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_float());
		}

		// Token: 0x04000086 RID: 134
		public static ES3Type Instance;
	}
}
