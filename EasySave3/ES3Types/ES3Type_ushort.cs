using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000055 RID: 85
	[Preserve]
	public class ES3Type_ushort : ES3Type
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x0000A2BA File Offset: 0x000084BA
		public ES3Type_ushort() : base(typeof(ushort))
		{
			this.isPrimitive = true;
			ES3Type_ushort.Instance = this;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000A2D9 File Offset: 0x000084D9
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((ushort)obj);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000A2E7 File Offset: 0x000084E7
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_ushort());
		}

		// Token: 0x0400009A RID: 154
		public static ES3Type Instance;
	}
}
