using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
	// Token: 0x0200000D RID: 13
	public class KVirtualElement : MonoBehaviour
	{
		// Token: 0x06000018 RID: 24 RVA: 0x000026CE File Offset: 0x000008CE
		public void Animate()
		{
			base.transform.position = this.targetBone.position;
			base.transform.rotation = this.targetBone.rotation;
		}

		// Token: 0x0400001F RID: 31
		public Transform targetBone;
	}
}
