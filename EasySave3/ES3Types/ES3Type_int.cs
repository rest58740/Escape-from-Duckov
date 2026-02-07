using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000043 RID: 67
	[Preserve]
	public class ES3Type_int : ES3Type
	{
		// Token: 0x06000292 RID: 658 RVA: 0x00009F60 File Offset: 0x00008160
		public ES3Type_int() : base(typeof(int))
		{
			this.isPrimitive = true;
			ES3Type_int.Instance = this;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00009F7F File Offset: 0x0000817F
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((int)obj);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00009F8D File Offset: 0x0000818D
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_int());
		}

		// Token: 0x04000088 RID: 136
		public static ES3Type Instance;
	}
}
