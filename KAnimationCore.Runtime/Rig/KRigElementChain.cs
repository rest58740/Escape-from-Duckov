using System;
using System.Collections.Generic;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class KRigElementChain
	{
		// Token: 0x04000019 RID: 25
		public string chainName;

		// Token: 0x0400001A RID: 26
		[HideInInspector]
		public List<KRigElement> elementChain = new List<KRigElement>();

		// Token: 0x0400001B RID: 27
		[HideInInspector]
		public bool isStandalone;
	}
}
