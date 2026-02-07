using System;
using System.Collections.Generic;
using Duckov;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200007A RID: 122
public class HitMarker : MonoBehaviour
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x060004B2 RID: 1202 RVA: 0x000158EC File Offset: 0x00013AEC
	// (remove) Token: 0x060004B3 RID: 1203 RVA: 0x00015920 File Offset: 0x00013B20
	public static event Action OnHitMarker;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x060004B4 RID: 1204 RVA: 0x00015954 File Offset: 0x00013B54
	// (remove) Token: 0x060004B5 RID: 1205 RVA: 0x00015988 File Offset: 0x00013B88
	public static event Action OnKillMarker;

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060004B6 RID: 1206 RVA: 0x000159BC File Offset: 0x00013BBC
	private Camera MainCam
	{
		get
		{
			if (!this._cam)
			{
				if (LevelManager.Instance == null)
				{
					return null;
				}
				if (LevelManager.Instance.GameCamera == null)
				{
					return null;
				}
				this._cam = LevelManager.Instance.GameCamera.renderCamera;
			}
			return this._cam;
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00015A14 File Offset: 0x00013C14
	private void Awake()
	{
		Health.OnHurt += this.OnHealthHitEvent;
		Health.OnDead += this.OnHealthKillEvent;
		HealthSimpleBase.OnSimpleHealthHit += this.OnSimpleHealthHit;
		HealthSimpleBase.OnSimpleHealthDead += this.OnSimpleHealthKill;
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00015A68 File Offset: 0x00013C68
	private void OnDestroy()
	{
		Health.OnHurt -= this.OnHealthHitEvent;
		Health.OnDead -= this.OnHealthKillEvent;
		HealthSimpleBase.OnSimpleHealthHit -= this.OnSimpleHealthHit;
		HealthSimpleBase.OnSimpleHealthDead -= this.OnSimpleHealthKill;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00015AB9 File Offset: 0x00013CB9
	private void OnHealthHitEvent(Health _health, DamageInfo dmgInfo)
	{
		if (dmgInfo.isFromBuffOrEffect)
		{
			return;
		}
		if (dmgInfo.damageValue <= 1.01f)
		{
			return;
		}
		this.OnHit(dmgInfo);
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00015ADC File Offset: 0x00013CDC
	private void OnHit(DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		if (dmgInfo.toDamageReceiver && dmgInfo.toDamageReceiver.IsMainCharacter)
		{
			return;
		}
		bool flag = (float)dmgInfo.crit > 0f;
		Vector3 vector = this.MainCam.WorldToScreenPoint(dmgInfo.damagePoint);
		Vector2 vector2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, vector, null, ref vector2);
		base.transform.localPosition = Vector3.ClampMagnitude(vector2, 10f);
		ItemAgent_Gun gun = CharacterMainControl.Main.GetGun();
		if (gun != null)
		{
			this.scatterOnHit = gun.CurrentScatter;
		}
		int num = flag ? (this.hitMarkerIndex ? this.critHash1 : this.critHash2) : (this.hitMarkerIndex ? this.hitHash1 : this.hitHash2);
		int shortNameHash = this.animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
		if (shortNameHash != this.killHash && shortNameHash != this.killCritHash)
		{
			this.hitMarkerIndex = !this.hitMarkerIndex;
			this.animator.CrossFade(num, 0.02f);
		}
		Action onHitMarker = HitMarker.OnHitMarker;
		if (onHitMarker != null)
		{
			onHitMarker();
		}
		if (!dmgInfo.toDamageReceiver || !dmgInfo.toDamageReceiver.useSimpleHealth)
		{
			AudioManager.PostHitMarker(flag);
		}
		UnityEvent unityEvent = this.hitEvent;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x00015C59 File Offset: 0x00013E59
	private void OnHealthKillEvent(Health _health, DamageInfo dmgInfo)
	{
		this.OnKill(dmgInfo);
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00015C64 File Offset: 0x00013E64
	private void OnKill(DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		if (dmgInfo.toDamageReceiver && dmgInfo.toDamageReceiver.IsMainCharacter)
		{
			return;
		}
		bool flag = (float)dmgInfo.crit > 0f;
		int num = flag ? this.killCritHash : this.killHash;
		this.animator.CrossFade(num, 0.02f);
		if (!dmgInfo.toDamageReceiver || !dmgInfo.toDamageReceiver.useSimpleHealth)
		{
			AudioManager.PostKillMarker(flag);
		}
		Action onKillMarker = HitMarker.OnKillMarker;
		if (onKillMarker != null)
		{
			onKillMarker();
		}
		UnityEvent unityEvent = this.killEvent;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00015D19 File Offset: 0x00013F19
	private void OnSimpleHealthHit(HealthSimpleBase health, DamageInfo dmgInfo)
	{
		if (dmgInfo.damageValue <= 1.01f)
		{
			return;
		}
		this.OnHit(dmgInfo);
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00015D30 File Offset: 0x00013F30
	private void OnSimpleHealthKill(HealthSimpleBase health, DamageInfo dmgInfo)
	{
		this.OnKill(dmgInfo);
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00015D3C File Offset: 0x00013F3C
	private void LateUpdate()
	{
		foreach (RectTransform rectTransform in this.hitMarkerImages)
		{
			rectTransform.anchoredPosition += rectTransform.anchoredPosition.normalized * this.scatterOnHit * 3f;
		}
	}

	// Token: 0x040003F4 RID: 1012
	public UnityEvent hitEvent;

	// Token: 0x040003F5 RID: 1013
	public UnityEvent killEvent;

	// Token: 0x040003F8 RID: 1016
	public Animator animator;

	// Token: 0x040003F9 RID: 1017
	private readonly int hitHash1 = Animator.StringToHash("HitMarkerHit1");

	// Token: 0x040003FA RID: 1018
	private readonly int hitHash2 = Animator.StringToHash("HitMarkerHit2");

	// Token: 0x040003FB RID: 1019
	private readonly int critHash1 = Animator.StringToHash("HitMarkerCrit1");

	// Token: 0x040003FC RID: 1020
	private readonly int critHash2 = Animator.StringToHash("HitMarkerCrit2");

	// Token: 0x040003FD RID: 1021
	private bool hitMarkerIndex;

	// Token: 0x040003FE RID: 1022
	private readonly int killHash = Animator.StringToHash("HitMarkerKill");

	// Token: 0x040003FF RID: 1023
	private readonly int killCritHash = Animator.StringToHash("HitMarkerKillCrit");

	// Token: 0x04000400 RID: 1024
	public List<RectTransform> hitMarkerImages;

	// Token: 0x04000401 RID: 1025
	private float scatterOnHit;

	// Token: 0x04000402 RID: 1026
	private Camera _cam;
}
