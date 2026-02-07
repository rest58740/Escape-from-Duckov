using System;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class DarkRoomFade : MonoBehaviour
{
	// Token: 0x06000BD4 RID: 3028 RVA: 0x00032908 File Offset: 0x00030B08
	public void StartFade()
	{
		this.started = true;
		base.enabled = true;
		this.startPos = CharacterMainControl.Main.transform.position;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x0003292D File Offset: 0x00030B2D
	private void Awake()
	{
		this.range = 0f;
		this.UpdateMaterial();
		if (!this.started)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x00032950 File Offset: 0x00030B50
	private void Update()
	{
		if (!this.started)
		{
			base.enabled = false;
		}
		this.range += this.speed * Time.deltaTime;
		this.UpdateMaterial();
		if (this.range > this.maxRange)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x000329A0 File Offset: 0x00030BA0
	private void UpdateMaterial()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		materialPropertyBlock.SetFloat("_Range", this.range);
		materialPropertyBlock.SetVector("_CenterPos", this.startPos);
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPropertyBlock(materialPropertyBlock);
		}
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x000329F8 File Offset: 0x00030BF8
	private void Collect()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sharedMaterial.SetFloat("_Range", 0f);
		}
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00032A40 File Offset: 0x00030C40
	public void SetRenderers(bool enable)
	{
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = enable;
		}
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00032A6C File Offset: 0x00030C6C
	public static void SetRenderersEnable(bool enable)
	{
		DarkRoomFade[] array = Object.FindObjectsByType<DarkRoomFade>(0);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetRenderers(enable);
		}
	}

	// Token: 0x04000A23 RID: 2595
	public float maxRange = 100f;

	// Token: 0x04000A24 RID: 2596
	public float speed = 20f;

	// Token: 0x04000A25 RID: 2597
	public Renderer[] renderers;

	// Token: 0x04000A26 RID: 2598
	private Vector3 startPos;

	// Token: 0x04000A27 RID: 2599
	private float range;

	// Token: 0x04000A28 RID: 2600
	private bool started;
}
