using System;
using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Input;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
	// Token: 0x02000008 RID: 8
	[CreateAssetMenu(fileName = "NewRig", menuName = "KINEMATION/Rig")]
	public class KRig : ScriptableObject
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020C0 File Offset: 0x000002C0
		public KRigElementChain GetElementChainByName(string chainName)
		{
			return this.rigElementChains.Find((KRigElementChain item) => item.chainName.Equals(chainName));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F4 File Offset: 0x000002F4
		public KTransformChain GetPopulatedChain(string chainName, KRigComponent rigComponent)
		{
			KTransformChain ktransformChain = new KTransformChain();
			KRigElementChain elementChainByName = this.GetElementChainByName(chainName);
			if (elementChainByName == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Rig `",
					base.name,
					"`: `",
					chainName,
					"` chain not found!"
				}));
				return null;
			}
			foreach (KRigElement rigElement in elementChainByName.elementChain)
			{
				ktransformChain.transformChain.Add(rigComponent.GetRigTransform(rigElement));
			}
			return ktransformChain;
		}

		// Token: 0x0400000D RID: 13
		public RuntimeAnimatorController targetAnimator;

		// Token: 0x0400000E RID: 14
		public UserInputConfig inputConfig;

		// Token: 0x0400000F RID: 15
		public List<KRigElement> rigHierarchy = new List<KRigElement>();

		// Token: 0x04000010 RID: 16
		public List<KRigElementChain> rigElementChains = new List<KRigElementChain>();

		// Token: 0x04000011 RID: 17
		public List<string> rigCurves = new List<string>();
	}
}
