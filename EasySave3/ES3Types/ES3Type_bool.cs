using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000031 RID: 49
	[Preserve]
	public class ES3Type_bool : ES3Type
	{
		// Token: 0x06000269 RID: 617 RVA: 0x000096D2 File Offset: 0x000078D2
		public ES3Type_bool() : base(typeof(bool))
		{
			this.isPrimitive = true;
			ES3Type_bool.Instance = this;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000096F1 File Offset: 0x000078F1
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((bool)obj);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000096FF File Offset: 0x000078FF
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_bool());
		}

		// Token: 0x04000075 RID: 117
		public static ES3Type Instance;
	}
}
