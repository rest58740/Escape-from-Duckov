using System;
using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
	// Token: 0x0200000C RID: 12
	public class KTransformChain
	{
		// Token: 0x06000013 RID: 19 RVA: 0x0000247C File Offset: 0x0000067C
		public void CacheTransforms(ESpaceType targetSpace, Transform root = null)
		{
			this.cachedTransforms.Clear();
			this.spaceType = targetSpace;
			foreach (Transform transform in this.transformChain)
			{
				KTransform item = default(KTransform);
				if (targetSpace == ESpaceType.WorldSpace)
				{
					item.position = transform.position;
					item.rotation = transform.rotation;
				}
				else if (targetSpace == ESpaceType.ComponentSpace)
				{
					if (root == null)
					{
						root = transform.root;
					}
					item.position = root.InverseTransformPoint(transform.position);
					item.rotation = Quaternion.Inverse(root.rotation) * transform.rotation;
				}
				else
				{
					item.position = transform.localPosition;
					item.rotation = transform.localRotation;
				}
				this.cachedTransforms.Add(item);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002578 File Offset: 0x00000778
		public void BlendTransforms(float weight)
		{
			int count = this.transformChain.Count;
			for (int i = 0; i < count; i++)
			{
				Transform transform = this.transformChain[i];
				KTransform pose = this.cachedTransforms[i];
				KPose kpose = new KPose
				{
					modifyMode = EModifyMode.Replace,
					pose = pose,
					space = this.spaceType
				};
				KAnimationMath.ModifyTransform(transform.root, transform, kpose, weight);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025F0 File Offset: 0x000007F0
		public float GetLength(Transform root = null)
		{
			float num = 0f;
			int count = this.transformChain.Count;
			if (count > 0 && root == null)
			{
				root = this.transformChain[0];
			}
			for (int i = 0; i < count; i++)
			{
				Transform transform = this.transformChain[i];
				if (count == 1)
				{
					num = root.InverseTransformPoint(transform.position).magnitude;
				}
				if (i > 0)
				{
					num += (transform.position - this.transformChain[i - 1].position).magnitude;
				}
			}
			return num;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000268C File Offset: 0x0000088C
		public bool IsValid()
		{
			return this.transformChain != null && this.cachedTransforms != null && this.transformChain.Count != 0;
		}

		// Token: 0x0400001C RID: 28
		public List<Transform> transformChain = new List<Transform>();

		// Token: 0x0400001D RID: 29
		public List<KTransform> cachedTransforms = new List<KTransform>();

		// Token: 0x0400001E RID: 30
		public ESpaceType spaceType;
	}
}
