using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public struct KRigElement
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000244F File Offset: 0x0000064F
		public KRigElement(int index = -1, string name = "None", bool isVirtual = false)
		{
			this.index = index;
			this.name = name;
			this.isVirtual = isVirtual;
		}

		// Token: 0x04000016 RID: 22
		public string name;

		// Token: 0x04000017 RID: 23
		[HideInInspector]
		public int index;

		// Token: 0x04000018 RID: 24
		public bool isVirtual;
	}
}
