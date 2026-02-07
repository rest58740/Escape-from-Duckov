using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000051 RID: 81
	[DefaultExecutionOrder(-99999999)]
	[ExecuteInEditMode]
	public abstract class MCS_RemoveTris : MonoBehaviour
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x0000EDB9 File Offset: 0x0000CFB9
		private void Awake()
		{
			this.Register(true);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000EDC2 File Offset: 0x0000CFC2
		private void OnEnable()
		{
			this.Register(false);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000EDCC File Offset: 0x0000CFCC
		private void Register(bool first)
		{
			if (this.hasRegistered)
			{
				return;
			}
			if (first)
			{
				if (MeshCombiner.instances.Count == 0)
				{
					return;
				}
				for (int i = 0; i < MeshCombiner.instances.Count; i++)
				{
					this.Init(MeshCombiner.instances[i]);
				}
			}
			else
			{
				MeshCombiner.onInit = (MeshCombiner.EventMethod)Delegate.Combine(MeshCombiner.onInit, new MeshCombiner.EventMethod(this.Init));
			}
			this.hasRegistered = true;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000EE41 File Offset: 0x0000D041
		private void Init(MeshCombiner meshCombiner)
		{
			meshCombiner.onCombiningStart += this.OnCombine;
			meshCombiner.onCombiningAbort += this.OnCombineReady;
			meshCombiner.onCombiningReady += this.OnCombineReady;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000EE79 File Offset: 0x0000D079
		private void OnDisable()
		{
			this.Unregister();
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000EE81 File Offset: 0x0000D081
		private void OnDestroy()
		{
			this.Unregister();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000EE8C File Offset: 0x0000D08C
		private void Unregister()
		{
			if (!this.hasRegistered)
			{
				return;
			}
			this.hasRegistered = false;
			this.OnCombineReady(null);
			MeshCombiner.onInit = (MeshCombiner.EventMethod)Delegate.Remove(MeshCombiner.onInit, new MeshCombiner.EventMethod(this.Init));
			for (int i = 0; i < MeshCombiner.instances.Count; i++)
			{
				MeshCombiner meshCombiner = MeshCombiner.instances[i];
				meshCombiner.onCombiningStart -= this.OnCombine;
				meshCombiner.onCombiningAbort -= this.OnCombineReady;
				meshCombiner.onCombiningReady -= this.OnCombineReady;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000EF28 File Offset: 0x0000D128
		private void OnCombine(MeshCombiner meshCombiner)
		{
			if (this.gos.Count > 0)
			{
				this.OnCombineReady(null);
			}
			int firstLayerInLayerMask;
			if (this is MCS_RemoveTrisBelowSurface)
			{
				firstLayerInLayerMask = Methods.GetFirstLayerInLayerMask(meshCombiner.surfaceLayerMask);
			}
			else
			{
				firstLayerInLayerMask = Methods.GetFirstLayerInLayerMask(meshCombiner.overlapLayerMask);
			}
			if (firstLayerInLayerMask == -1)
			{
				return;
			}
			Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				GameObject gameObject = componentsInChildren[i].gameObject;
				this.gos.Add(new GameObjectLayer(gameObject));
				gameObject.layer = firstLayerInLayerMask;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000EFB4 File Offset: 0x0000D1B4
		private void OnCombineReady(MeshCombiner meshCombiner)
		{
			foreach (GameObjectLayer gameObjectLayer in this.gos)
			{
				gameObjectLayer.RestoreLayer();
			}
			this.gos.Clear();
		}

		// Token: 0x04000219 RID: 537
		private HashSet<GameObjectLayer> gos = new HashSet<GameObjectLayer>();

		// Token: 0x0400021A RID: 538
		private bool hasRegistered;
	}
}
