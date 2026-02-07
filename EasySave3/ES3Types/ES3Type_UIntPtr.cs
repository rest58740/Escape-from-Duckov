using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000051 RID: 81
	[Preserve]
	public class ES3Type_UIntPtr : ES3Type
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000A202 File Offset: 0x00008402
		public ES3Type_UIntPtr() : base(typeof(UIntPtr))
		{
			this.isPrimitive = true;
			ES3Type_UIntPtr.Instance = this;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000A221 File Offset: 0x00008421
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((ulong)obj);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000A22F File Offset: 0x0000842F
		public override object Read<T>(ES3Reader reader)
		{
			return reader.Read_ulong();
		}

		// Token: 0x04000096 RID: 150
		public static ES3Type Instance;
	}
}
