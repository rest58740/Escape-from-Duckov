using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000049 RID: 73
	[Preserve]
	public class ES3Type_sbyte : ES3Type
	{
		// Token: 0x0600029E RID: 670 RVA: 0x0000A08D File Offset: 0x0000828D
		public ES3Type_sbyte() : base(typeof(sbyte))
		{
			this.isPrimitive = true;
			ES3Type_sbyte.Instance = this;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000A0AC File Offset: 0x000082AC
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((sbyte)obj);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000A0BA File Offset: 0x000082BA
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_sbyte());
		}

		// Token: 0x0400008E RID: 142
		public static ES3Type Instance;
	}
}
