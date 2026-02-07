using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x02000191 RID: 401
public class PlayerHurtVisual : MonoBehaviour
{
	// Token: 0x06000C2F RID: 3119 RVA: 0x000342A0 File Offset: 0x000324A0
	private void Update()
	{
		if (!this.inited)
		{
			this.TryInit();
			return;
		}
		this.value = Mathf.MoveTowards(this.value, 0f, Time.deltaTime * this.speed);
		if (this.volume.weight != this.value)
		{
			this.volume.weight = this.value;
		}
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00034304 File Offset: 0x00032504
	private void TryInit()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (!main)
		{
			return;
		}
		this.mainCharacterHealth = main.Health;
		if (!this.mainCharacterHealth)
		{
			return;
		}
		this.mainCharacterHealth.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
		this.inited = true;
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00034365 File Offset: 0x00032565
	private void OnDestroy()
	{
		if (this.mainCharacterHealth)
		{
			this.mainCharacterHealth.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
		}
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00034390 File Offset: 0x00032590
	private void OnHurt(DamageInfo dmgInfo)
	{
		if (dmgInfo.damageValue < 1.5f)
		{
			return;
		}
		if (!this.mainCharacterHealth || !PlayerHurtVisual.hurtVisualOn)
		{
			this.value = 0f;
		}
		else if (this.mainCharacterHealth.CurrentHealth / this.mainCharacterHealth.MaxHealth > 0.5f)
		{
			this.value = 0.5f;
		}
		else
		{
			this.value = 1f;
		}
		if (this.volume.weight != this.value)
		{
			this.volume.weight = this.value;
		}
	}

	// Token: 0x04000A84 RID: 2692
	[SerializeField]
	private Volume volume;

	// Token: 0x04000A85 RID: 2693
	[SerializeField]
	private float speed = 4f;

	// Token: 0x04000A86 RID: 2694
	private Health mainCharacterHealth;

	// Token: 0x04000A87 RID: 2695
	private bool inited;

	// Token: 0x04000A88 RID: 2696
	public static bool hurtVisualOn = true;

	// Token: 0x04000A89 RID: 2697
	private float value;
}
