using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000047 RID: 71
	[Preserve]
	public class ES3Type_long : ES3Type
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0000A02C File Offset: 0x0000822C
		public ES3Type_long() : base(typeof(long))
		{
			this.isPrimitive = true;
			ES3Type_long.Instance = this;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000A04B File Offset: 0x0000824B
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((long)obj);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000A059 File Offset: 0x00008259
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_long());
		}

		// Token: 0x0400008C RID: 140
		public static ES3Type Instance;
	}
}
