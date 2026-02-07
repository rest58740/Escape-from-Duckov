using System;
using TMPro;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class DPSDisplayer : MonoBehaviour
{
	// Token: 0x06000BDC RID: 3036 RVA: 0x00032AB5 File Offset: 0x00030CB5
	private void Awake()
	{
		Health.OnHurt += this.OnHurt;
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x00032AC8 File Offset: 0x00030CC8
	private void Update()
	{
		if (Time.time - this.lastTimeMarker > 3f)
		{
			this.empty = true;
			this.totalDamage = 0f;
			this.RefreshDisplay();
		}
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x00032AF5 File Offset: 0x00030CF5
	private void OnDestroy()
	{
		Health.OnHurt -= this.OnHurt;
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x00032B08 File Offset: 0x00030D08
	private void OnHurt(Health health, DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		this.totalDamage += dmgInfo.finalDamage;
		if (this.empty)
		{
			this.firstTimeMarker = Time.time;
			this.lastTimeMarker = Time.time;
			this.empty = false;
			return;
		}
		this.lastTimeMarker = Time.time;
		this.RefreshDisplay();
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x00032B7C File Offset: 0x00030D7C
	private void RefreshDisplay()
	{
		float num = this.CalculateDPS();
		this.dpsText.text = num.ToString("00000");
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x00032BA8 File Offset: 0x00030DA8
	private float CalculateDPS()
	{
		if (this.empty)
		{
			return 0f;
		}
		float num = this.lastTimeMarker - this.firstTimeMarker;
		if (num <= 0f)
		{
			return 0f;
		}
		return this.totalDamage / num;
	}

	// Token: 0x04000A29 RID: 2601
	[SerializeField]
	private TextMeshPro dpsText;

	// Token: 0x04000A2A RID: 2602
	private bool empty;

	// Token: 0x04000A2B RID: 2603
	private float totalDamage;

	// Token: 0x04000A2C RID: 2604
	private float firstTimeMarker;

	// Token: 0x04000A2D RID: 2605
	private float lastTimeMarker;
}
