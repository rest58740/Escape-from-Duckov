using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x0200004B RID: 75
	public class FeaturesNotSupportedMessage : MonoBehaviour
	{
		// Token: 0x060002DC RID: 732 RVA: 0x0000B8B1 File Offset: 0x00009AB1
		private void Start()
		{
			if (!Noise3D.isSupported)
			{
				Debug.LogWarning(Noise3D.isNotSupportedString);
			}
		}
	}
}
