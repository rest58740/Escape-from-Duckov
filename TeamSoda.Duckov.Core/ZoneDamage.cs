using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000BA RID: 186
[RequireComponent(typeof(Zone))]
public class ZoneDamage : MonoBehaviour
{
	// Token: 0x06000627 RID: 1575 RVA: 0x0001B8BD File Offset: 0x00019ABD
	private void Start()
	{
		if (this.zone == null)
		{
			this.zone = base.GetComponent<Zone>();
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0001B8DC File Offset: 0x00019ADC
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer > this.timeSpace)
		{
			this.timer %= this.timeSpace;
			this.Damage();
		}
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0001B92C File Offset: 0x00019B2C
	private void Damage()
	{
		foreach (Health health in this.zone.Healths)
		{
			CharacterMainControl characterMainControl = health.TryGetCharacter();
			if (!(characterMainControl == null))
			{
				if (this.checkGasMask && characterMainControl.HasGasMask)
				{
					Item faceMaskItem = characterMainControl.GetFaceMaskItem();
					if (faceMaskItem && faceMaskItem.GetStat(this.hasMaskHash) != null)
					{
						faceMaskItem.Durability -= 0.1f * this.timeSpace;
					}
				}
				else if ((!this.checkElecProtection || characterMainControl.CharacterItem.GetStat(this.elecProtectionHash).Value <= 0.99f) && (!this.checkFireProtection || characterMainControl.CharacterItem.GetStat(this.fireProtectionHash).Value <= 0.99f))
				{
					this.damageInfo.fromCharacter = null;
					this.damageInfo.damagePoint = health.transform.position + Vector3.up * 0.5f;
					this.damageInfo.damageNormal = Vector3.up;
					health.Hurt(this.damageInfo);
				}
			}
		}
	}

	// Token: 0x040005B9 RID: 1465
	public Zone zone;

	// Token: 0x040005BA RID: 1466
	public float timeSpace = 0.5f;

	// Token: 0x040005BB RID: 1467
	private float timer;

	// Token: 0x040005BC RID: 1468
	public DamageInfo damageInfo;

	// Token: 0x040005BD RID: 1469
	public bool checkGasMask;

	// Token: 0x040005BE RID: 1470
	public bool checkElecProtection;

	// Token: 0x040005BF RID: 1471
	public bool checkFireProtection;

	// Token: 0x040005C0 RID: 1472
	private int hasMaskHash = "GasMask".GetHashCode();

	// Token: 0x040005C1 RID: 1473
	private int elecProtectionHash = "ElecProtection".GetHashCode();

	// Token: 0x040005C2 RID: 1474
	private int fireProtectionHash = "FireProtection".GetHashCode();
}
