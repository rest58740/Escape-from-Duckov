using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class BowAnimation : MonoBehaviour
{
	// Token: 0x06000BC2 RID: 3010 RVA: 0x0003254C File Offset: 0x0003074C
	private void Start()
	{
		if (this.gunAgent != null)
		{
			this.gunAgent.OnShootEvent += this.OnShoot;
			this.gunAgent.OnLoadedEvent += this.OnLoaded;
			if (this.gunAgent.BulletCount > 0)
			{
				this.OnLoaded();
			}
		}
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x000325A9 File Offset: 0x000307A9
	private void OnDestroy()
	{
		if (this.gunAgent != null)
		{
			this.gunAgent.OnShootEvent -= this.OnShoot;
			this.gunAgent.OnLoadedEvent -= this.OnLoaded;
		}
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x000325E7 File Offset: 0x000307E7
	private void OnShoot()
	{
		this.animator.SetTrigger("Shoot");
		if (this.gunAgent.BulletCount <= 0)
		{
			this.animator.SetBool("Loaded", false);
		}
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00032618 File Offset: 0x00030818
	private void OnLoaded()
	{
		this.animator.SetBool("Loaded", true);
	}

	// Token: 0x04000A11 RID: 2577
	public ItemAgent_Gun gunAgent;

	// Token: 0x04000A12 RID: 2578
	public Animator animator;

	// Token: 0x04000A13 RID: 2579
	private int hash_Loaded = "Loaded".GetHashCode();

	// Token: 0x04000A14 RID: 2580
	private int hash_Aiming = "Aiming".GetHashCode();

	// Token: 0x04000A15 RID: 2581
	private int hash_Shoot = "Shoot".GetHashCode();
}
