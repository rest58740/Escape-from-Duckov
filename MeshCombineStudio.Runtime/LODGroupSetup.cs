using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000012 RID: 18
	public class LODGroupSetup : MonoBehaviour
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00003442 File Offset: 0x00001642
		public void Init(MeshCombiner meshCombiner, int lodGroupParentIndex)
		{
			this.meshCombiner = meshCombiner;
			this.lodGroupParentIndex = lodGroupParentIndex;
			this.lodCount = lodGroupParentIndex + 1;
			if (this.lodGroup == null)
			{
				this.lodGroup = base.gameObject.AddComponent<LODGroup>();
			}
			this.GetSetup();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003480 File Offset: 0x00001680
		private void GetSetup()
		{
			LOD[] array = new LOD[this.lodGroupParentIndex + 1];
			MeshCombiner.LODGroupSettings lodgroupSettings = this.meshCombiner.lodGroupsSettings[this.lodGroupParentIndex];
			this.lodGroup.animateCrossFading = lodgroupSettings.animateCrossFading;
			this.lodGroup.fadeMode = lodgroupSettings.fadeMode;
			for (int i = 0; i < array.Length; i++)
			{
				MeshCombiner.LODSettings lodsettings = lodgroupSettings.lodSettings[i];
				array[i] = default(LOD);
				array[i].screenRelativeTransitionHeight = lodsettings.screenRelativeTransitionHeight;
				array[i].fadeTransitionWidth = lodsettings.fadeTransitionWidth;
			}
			this.lodGroup.SetLODs(array);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003524 File Offset: 0x00001724
		public void ApplySetup()
		{
			LOD[] lods = this.lodGroup.GetLODs();
			if (this.lodGroups == null)
			{
				this.lodGroups = base.GetComponentsInChildren<LODGroup>();
			}
			if (lods.Length != this.lodCount)
			{
				return;
			}
			bool flag = false;
			if (this.lodGroupParentIndex == 0)
			{
				if (lods[0].screenRelativeTransitionHeight != 0f)
				{
					if (this.lodGroups == null || this.lodGroups.Length == 1)
					{
						this.AddLODGroupsToChildren();
					}
				}
				else
				{
					if (this.lodGroup != null && this.lodGroups.Length != 1)
					{
						this.RemoveLODGroupFromChildren();
					}
					flag = true;
				}
			}
			if (this.meshCombiner != null)
			{
				for (int i = 0; i < lods.Length; i++)
				{
					this.meshCombiner.lodGroupsSettings[this.lodGroupParentIndex].lodSettings[i].screenRelativeTransitionHeight = lods[i].screenRelativeTransitionHeight;
				}
			}
			if (flag)
			{
				return;
			}
			for (int j = 0; j < this.lodGroups.Length; j++)
			{
				LOD[] lods2 = this.lodGroups[j].GetLODs();
				this.lodGroups[j].animateCrossFading = this.lodGroup.animateCrossFading;
				this.lodGroups[j].fadeMode = this.lodGroup.fadeMode;
				for (int k = 0; k < lods2.Length; k++)
				{
					if (k < lods2.Length && k < lods.Length)
					{
						lods2[k].screenRelativeTransitionHeight = lods[k].screenRelativeTransitionHeight;
						lods2[k].fadeTransitionWidth = lods[k].fadeTransitionWidth;
					}
				}
				if (lods2.Length == lods.Length)
				{
					try
					{
						this.lodGroups[j].SetLODs(lods2);
					}
					catch (Exception ex)
					{
						Debug.Log("ApplySetup error in " + this.lodGroups[j].name + ": " + ex.Message);
					}
				}
			}
			if (this.meshCombiner != null)
			{
				this.lodGroup.size = (float)this.meshCombiner.cellSize;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000372C File Offset: 0x0000192C
		public void AddLODGroupsToChildren()
		{
			Transform transform = base.transform;
			List<LODGroup> list = new List<LODGroup>();
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				LODGroup lodgroup = child.GetComponent<LODGroup>();
				if (lodgroup == null)
				{
					lodgroup = child.gameObject.AddComponent<LODGroup>();
					LOD[] array = new LOD[1];
					LOD[] array2 = array;
					int num = 0;
					float screenRelativeTransitionHeight = 0f;
					Renderer[] componentsInChildren = child.GetComponentsInChildren<MeshRenderer>();
					array2[num] = new LOD(screenRelativeTransitionHeight, componentsInChildren);
					lodgroup.SetLODs(array);
				}
				list.Add(lodgroup);
			}
			this.lodGroups = list.ToArray();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000037C0 File Offset: 0x000019C0
		public void RemoveLODGroupFromChildren()
		{
			Transform transform = base.transform;
			for (int i = 0; i < transform.childCount; i++)
			{
				LODGroup component = transform.GetChild(i).GetComponent<LODGroup>();
				if (component != null)
				{
					UnityEngine.Object.DestroyImmediate(component);
				}
			}
			this.lodGroups = null;
		}

		// Token: 0x04000033 RID: 51
		public MeshCombiner meshCombiner;

		// Token: 0x04000034 RID: 52
		public LODGroup lodGroup;

		// Token: 0x04000035 RID: 53
		public int lodGroupParentIndex;

		// Token: 0x04000036 RID: 54
		public int lodCount;

		// Token: 0x04000037 RID: 55
		private LODGroup[] lodGroups;
	}
}
