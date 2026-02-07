using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class OcclusionFadeObject : MonoBehaviour
{
	// Token: 0x06000C1D RID: 3101 RVA: 0x00033DE7 File Offset: 0x00031FE7
	private void Collect()
	{
		this.CollectTriggers();
		this.CollectRenderers();
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00033DF8 File Offset: 0x00031FF8
	private void CollectTriggers()
	{
		this.triggers = new OcclusionFadeTrigger[0];
		this.triggers = base.GetComponentsInChildren<OcclusionFadeTrigger>();
		if (this.triggers.Length != 0)
		{
			foreach (OcclusionFadeTrigger occlusionFadeTrigger in this.triggers)
			{
				occlusionFadeTrigger.parent = this;
				Collider[] componentsInChildren = occlusionFadeTrigger.GetComponentsInChildren<Collider>(true);
				if (componentsInChildren.Length != 0)
				{
					Collider[] array2 = componentsInChildren;
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j].isTrigger = true;
					}
				}
			}
		}
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00033E70 File Offset: 0x00032070
	private void CollectRenderers()
	{
		this.topTransform = this.FindFirst(base.transform, this.topName);
		if (this.topTransform == null)
		{
			return;
		}
		this.renderers = this.topTransform.GetComponentsInChildren<Renderer>(true);
		this.originMaterials.Clear();
		foreach (Renderer renderer in this.renderers)
		{
			this.originMaterials.AddRange(renderer.sharedMaterials);
		}
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00033EEB File Offset: 0x000320EB
	public void OnEnter()
	{
		this.enterCounter++;
		this.Refresh();
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00033F01 File Offset: 0x00032101
	public void OnLeave()
	{
		this.enterCounter--;
		this.Refresh();
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00033F18 File Offset: 0x00032118
	private void Refresh()
	{
		this.SyncEnable();
		if (!this.triggerEnabled)
		{
			this.hiding = false;
			this.Sync();
			return;
		}
		if (this.enterCounter > 0 && !this.hiding)
		{
			this.hiding = true;
			this.Sync();
			return;
		}
		if (this.enterCounter <= 0 && this.hiding)
		{
			this.hiding = false;
			this.Sync();
		}
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00033F7E File Offset: 0x0003217E
	private void OnEnable()
	{
		this.SyncEnable();
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00033F86 File Offset: 0x00032186
	private void OnDisable()
	{
		this.SyncEnable();
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00033F90 File Offset: 0x00032190
	private void SyncEnable()
	{
		if (this.triggerEnabled != base.enabled)
		{
			OcclusionFadeTrigger[] array = this.triggers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(base.enabled);
			}
			this.triggerEnabled = base.enabled;
		}
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x00033FE0 File Offset: 0x000321E0
	private void Sync()
	{
		this.SyncEnable();
		OcclusionFadeTypes occlusionFadeTypes = this.fadeType;
		if (occlusionFadeTypes != OcclusionFadeTypes.Fade)
		{
			if (occlusionFadeTypes != OcclusionFadeTypes.ShadowOnly)
			{
				return;
			}
			if (this.hiding)
			{
				foreach (Renderer renderer in this.renderers)
				{
					if (!(renderer == null))
					{
						renderer.shadowCastingMode = 3;
					}
				}
				return;
			}
			foreach (Renderer renderer2 in this.renderers)
			{
				if (!(renderer2 == null))
				{
					renderer2.shadowCastingMode = 1;
				}
			}
			return;
		}
		else
		{
			if (this.tempMaterials == null)
			{
				this.tempMaterials = new List<Material>();
			}
			if (this.hiding)
			{
				int num = 0;
				foreach (Renderer renderer3 in this.renderers)
				{
					if (!(renderer3 == null))
					{
						this.tempMaterials.Clear();
						for (int j = 0; j < renderer3.materials.Length; j++)
						{
							Material mat = this.originMaterials[num];
							Material maskedMaterial = OcclusionFadeManager.Instance.GetMaskedMaterial(mat);
							this.tempMaterials.Add(maskedMaterial);
							num++;
						}
						renderer3.SetSharedMaterials(this.tempMaterials);
					}
				}
				return;
			}
			int num2 = 0;
			foreach (Renderer renderer4 in this.renderers)
			{
				if (!(renderer4 == null))
				{
					this.tempMaterials.Clear();
					for (int k = 0; k < renderer4.materials.Length; k++)
					{
						this.tempMaterials.Add(this.originMaterials[num2]);
						num2++;
					}
					renderer4.SetSharedMaterials(this.tempMaterials);
				}
			}
			return;
		}
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x00034180 File Offset: 0x00032380
	private void Hide()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			Renderer renderer = this.renderers[i];
			if (renderer != null)
			{
				renderer.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x000341BC File Offset: 0x000323BC
	private void Show()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			Renderer renderer = this.renderers[i];
			if (renderer != null)
			{
				renderer.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x000341F8 File Offset: 0x000323F8
	private Transform FindFirst(Transform root, string checkName)
	{
		for (int i = 0; i < root.childCount; i++)
		{
			Transform child = root.GetChild(i);
			if (child.name == checkName)
			{
				return child;
			}
			if (child.childCount > 0)
			{
				Transform transform = this.FindFirst(child, checkName);
				if (transform != null)
				{
					return transform;
				}
			}
		}
		return null;
	}

	// Token: 0x04000A79 RID: 2681
	public OcclusionFadeTypes fadeType;

	// Token: 0x04000A7A RID: 2682
	public string topName = "Fade";

	// Token: 0x04000A7B RID: 2683
	public OcclusionFadeTrigger[] triggers;

	// Token: 0x04000A7C RID: 2684
	public Renderer[] renderers;

	// Token: 0x04000A7D RID: 2685
	public List<Material> originMaterials;

	// Token: 0x04000A7E RID: 2686
	private List<Material> tempMaterials;

	// Token: 0x04000A7F RID: 2687
	private Transform topTransform;

	// Token: 0x04000A80 RID: 2688
	private int enterCounter;

	// Token: 0x04000A81 RID: 2689
	private bool hiding;

	// Token: 0x04000A82 RID: 2690
	private bool triggerEnabled = true;
}
