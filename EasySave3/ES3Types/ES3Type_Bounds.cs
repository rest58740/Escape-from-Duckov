using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000079 RID: 121
	[Preserve]
	[ES3Properties(new string[]
	{
		"center",
		"size"
	})]
	public class ES3Type_Bounds : ES3Type
	{
		// Token: 0x06000315 RID: 789 RVA: 0x0000F2D0 File Offset: 0x0000D4D0
		public ES3Type_Bounds() : base(typeof(Bounds))
		{
			ES3Type_Bounds.Instance = this;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000F2E8 File Offset: 0x0000D4E8
		public override void Write(object obj, ES3Writer writer)
		{
			Bounds bounds = (Bounds)obj;
			writer.WriteProperty("center", bounds.center, ES3Type_Vector3.Instance);
			writer.WriteProperty("size", bounds.size, ES3Type_Vector3.Instance);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000F334 File Offset: 0x0000D534
		public override object Read<T>(ES3Reader reader)
		{
			return new Bounds(reader.ReadProperty<Vector3>(ES3Type_Vector3.Instance), reader.ReadProperty<Vector3>(ES3Type_Vector3.Instance));
		}

		// Token: 0x040000B9 RID: 185
		public static ES3Type Instance;
	}
}
