using System;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class MoveRing : MonoBehaviour
{
	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000C00 RID: 3072 RVA: 0x000333C7 File Offset: 0x000315C7
	private CharacterMainControl character
	{
		get
		{
			return this.inputManager.characterMainControl;
		}
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x000333D4 File Offset: 0x000315D4
	public void SetThreshold(float threshold)
	{
		this.runThreshold = threshold;
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x000333E0 File Offset: 0x000315E0
	public void LateUpdate()
	{
		if (!this.inputManager)
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.inputManager = LevelManager.Instance.InputManager;
			return;
		}
		else
		{
			if (!this.character)
			{
				this.SetMove(Vector3.zero, 0f);
				return;
			}
			base.transform.position = this.character.transform.position + Vector3.up * 0.02f;
			this.SetThreshold(this.inputManager.runThreshold);
			this.SetMove(this.inputManager.WorldMoveInput.normalized, this.inputManager.WorldMoveInput.magnitude);
			this.SetRunning(this.character.Running);
			if (this.ring.enabled != this.character.gameObject.activeInHierarchy)
			{
				this.ring.enabled = this.character.gameObject.activeInHierarchy;
			}
			return;
		}
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x000334EC File Offset: 0x000316EC
	public void SetMove(Vector3 direction, float value)
	{
		if (this.ringMat)
		{
			this.ringMat.SetVector("_Direction", direction);
			this.ringMat.SetFloat("_Distance", value);
			this.ringMat.SetFloat("_Threshold", this.runThreshold);
			return;
		}
		if (!this.ring)
		{
			return;
		}
		this.ringMat = this.ring.material;
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x00033563 File Offset: 0x00031763
	public void SetRunning(bool running)
	{
		this.ringMat.SetFloat("_Running", (float)(running ? 1 : 0));
	}

	// Token: 0x04000A4D RID: 2637
	public Renderer ring;

	// Token: 0x04000A4E RID: 2638
	public float runThreshold;

	// Token: 0x04000A4F RID: 2639
	private Material ringMat;

	// Token: 0x04000A50 RID: 2640
	private InputManager inputManager;
}
