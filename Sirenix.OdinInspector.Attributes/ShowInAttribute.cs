using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000063 RID: 99
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class ShowInAttribute : Attribute
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00003830 File Offset: 0x00001A30
		public ShowInAttribute(PrefabKind prefabKind)
		{
			this.PrefabKind = prefabKind;
		}

		// Token: 0x0400010D RID: 269
		public PrefabKind PrefabKind;
	}
}
