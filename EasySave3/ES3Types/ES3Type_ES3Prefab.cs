using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000018 RID: 24
	[Preserve]
	public class ES3Type_ES3Prefab : ES3Type
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x00006D3E File Offset: 0x00004F3E
		public ES3Type_ES3Prefab() : base(typeof(ES3Prefab))
		{
			ES3Type_ES3Prefab.Instance = this;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006D56 File Offset: 0x00004F56
		public override void Write(object obj, ES3Writer writer)
		{
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00006D58 File Offset: 0x00004F58
		public override object Read<T>(ES3Reader reader)
		{
			return null;
		}

		// Token: 0x04000052 RID: 82
		public static ES3Type Instance;
	}
}
