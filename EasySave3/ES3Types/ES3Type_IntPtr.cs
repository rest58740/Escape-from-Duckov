using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000045 RID: 69
	[Preserve]
	public class ES3Type_IntPtr : ES3Type
	{
		// Token: 0x06000296 RID: 662 RVA: 0x00009FC1 File Offset: 0x000081C1
		public ES3Type_IntPtr() : base(typeof(IntPtr))
		{
			this.isPrimitive = true;
			ES3Type_IntPtr.Instance = this;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00009FE0 File Offset: 0x000081E0
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((long)((IntPtr)obj));
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00009FF3 File Offset: 0x000081F3
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)((IntPtr)reader.Read_long()));
		}

		// Token: 0x0400008A RID: 138
		public static ES3Type Instance;
	}
}
