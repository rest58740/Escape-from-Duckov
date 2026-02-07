using System;
using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
	// Token: 0x02000009 RID: 9
	public class KRigComponent : MonoBehaviour
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000021C8 File Offset: 0x000003C8
		public void Initialize()
		{
			this._virtualElements = new List<KVirtualElement>();
			foreach (KVirtualElement item in base.GetComponentsInChildren<KVirtualElement>())
			{
				this._virtualElements.Add(item);
			}
			this._hierarchyMap = new Dictionary<string, int>();
			int count = this.hierarchy.Count;
			for (int j = 0; j < count; j++)
			{
				this._hierarchyMap.TryAdd(this.hierarchy[j].name, j);
			}
			this._cachedHierarchyPose = new List<KTransform>();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002258 File Offset: 0x00000458
		public void AnimateVirtualElements()
		{
			foreach (KVirtualElement kvirtualElement in this._virtualElements)
			{
				kvirtualElement.Animate();
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022A8 File Offset: 0x000004A8
		public Transform[] GetRigTransforms()
		{
			return this.hierarchy.ToArray();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022B8 File Offset: 0x000004B8
		public Transform GetRigTransform(KRigElement rigElement)
		{
			int num = rigElement.index;
			if (num < 0 || num > this.hierarchy.Count - 1)
			{
				num = this._hierarchyMap[rigElement.name];
			}
			if (num < 0 || num > this.hierarchy.Count - 1)
			{
				return null;
			}
			return this.hierarchy[num].transform;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000231C File Offset: 0x0000051C
		public Transform GetRigTransform(string elementName)
		{
			int index;
			if (this._hierarchyMap.TryGetValue(elementName, out index))
			{
				return this.hierarchy[index];
			}
			return null;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002347 File Offset: 0x00000547
		public Transform GetRigTransform(int elementIndex)
		{
			if (elementIndex < 0 || elementIndex > this.hierarchy.Count - 1)
			{
				return null;
			}
			return this.hierarchy[elementIndex].transform;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002370 File Offset: 0x00000570
		public void CacheHierarchyPose()
		{
			this._cachedHierarchyPose.Clear();
			foreach (Transform t in this.hierarchy)
			{
				this._cachedHierarchyPose.Add(new KTransform(t, false));
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000023DC File Offset: 0x000005DC
		public void ApplyHierarchyCachedPose()
		{
			int count = this.hierarchy.Count;
			for (int i = 0; i < count; i++)
			{
				KTransform ktransform = this._cachedHierarchyPose[i];
				this.hierarchy[i].localPosition = ktransform.position;
				this.hierarchy[i].localRotation = ktransform.rotation;
			}
		}

		// Token: 0x04000012 RID: 18
		[SerializeField]
		private List<Transform> hierarchy = new List<Transform>();

		// Token: 0x04000013 RID: 19
		private List<KVirtualElement> _virtualElements;

		// Token: 0x04000014 RID: 20
		private Dictionary<string, int> _hierarchyMap;

		// Token: 0x04000015 RID: 21
		private List<KTransform> _cachedHierarchyPose;
	}
}
