using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200000F RID: 15
	public class SelectOriginal : MonoBehaviour
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00003204 File Offset: 0x00001404
		private void Update()
		{
			Ray ray = this.mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				this.Deselect();
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit))
			{
				Transform transform = raycastHit.transform;
				MeshRenderer component = transform.GetComponent<MeshRenderer>();
				if (component != null)
				{
					if (component == this.oldMr)
					{
						return;
					}
					this.Deselect();
					this.oldMr = component;
					if (this.meshCombiner.searchOptions.objectCenter == MeshCombiner.ObjectCenter.BoundsCenter)
					{
						this.oldPos = this.oldMr.bounds.center;
					}
					else
					{
						this.oldPos = transform.position;
					}
					this.oldMat = this.oldMr.sharedMaterial;
					this.SelectOrDeselect(this.oldPos, this.oldMr, this.matSelect, true);
					return;
				}
			}
			else
			{
				this.Deselect();
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000032DD File Offset: 0x000014DD
		private void Deselect()
		{
			if (this.oldMr != null)
			{
				this.SelectOrDeselect(this.oldPos, this.oldMr, this.oldMat, false);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003308 File Offset: 0x00001508
		private void SelectOrDeselect(Vector3 position, MeshRenderer mr, Material mat, bool select)
		{
			ObjectOctree.Cell octree = this.meshCombiner.octree;
			if (octree == null)
			{
				return;
			}
			ObjectOctree.MaxCell cell = octree.GetCell(position);
			if (cell == null)
			{
				return;
			}
			mr.sharedMaterial = mat;
			if (this.meshCombiner.activeOriginal)
			{
				return;
			}
			foreach (ObjectOctree.LODParent lodparent in cell.lodParents)
			{
				if (lodparent != null)
				{
					foreach (ObjectOctree.LODLevel lodlevel in lodparent.lodLevels)
					{
						if (lodlevel != null)
						{
							Methods.SetMeshRenderersActive(lodlevel.newMeshRenderers, !select);
							Methods.SetCachedGOSActive(lodlevel.cachedGOs, select);
						}
					}
				}
			}
		}

		// Token: 0x0400002A RID: 42
		public MeshCombiner meshCombiner;

		// Token: 0x0400002B RID: 43
		public Camera mainCamera;

		// Token: 0x0400002C RID: 44
		public Material matSelect;

		// Token: 0x0400002D RID: 45
		private Material oldMat;

		// Token: 0x0400002E RID: 46
		private Vector3 oldPos;

		// Token: 0x0400002F RID: 47
		private MeshRenderer oldMr;
	}
}
