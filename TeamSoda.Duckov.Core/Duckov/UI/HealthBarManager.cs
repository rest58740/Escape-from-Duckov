using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C1 RID: 961
	public class HealthBarManager : MonoBehaviour
	{
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002273 RID: 8819 RVA: 0x00078C26 File Offset: 0x00076E26
		public static HealthBarManager Instance
		{
			get
			{
				return HealthBarManager._instance;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x00078C30 File Offset: 0x00076E30
		private PrefabPool<HealthBar> PrefabPool
		{
			get
			{
				if (this._prefabPool == null)
				{
					this._prefabPool = new PrefabPool<HealthBar>(this.healthBarPrefab, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this._prefabPool;
			}
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00078C6E File Offset: 0x00076E6E
		private void Awake()
		{
			if (HealthBarManager._instance == null)
			{
				HealthBarManager._instance = this;
			}
			this.UnregisterStaticEvents();
			this.RegisterStaticEvents();
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x00078C8F File Offset: 0x00076E8F
		private void OnDestroy()
		{
			this.UnregisterStaticEvents();
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x00078C97 File Offset: 0x00076E97
		private void RegisterStaticEvents()
		{
			Health.OnRequestHealthBar += this.Health_OnRequestHealthBar;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00078CAA File Offset: 0x00076EAA
		private void UnregisterStaticEvents()
		{
			Health.OnRequestHealthBar -= this.Health_OnRequestHealthBar;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00078CC0 File Offset: 0x00076EC0
		private HealthBar GetActiveHealthBar(Health health)
		{
			if (health == null)
			{
				return null;
			}
			return this.PrefabPool.ActiveEntries.FirstOrDefault((HealthBar e) => e.target == health);
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00078D08 File Offset: 0x00076F08
		private HealthBar CreateHealthBarFor(Health health, DamageInfo? damage = null)
		{
			if (health == null)
			{
				return null;
			}
			if (this.PrefabPool.ActiveEntries.FirstOrDefault((HealthBar e) => e.target == health))
			{
				Debug.Log("Health bar for " + health.name + " already exists");
				return null;
			}
			HealthBar newBar = this.PrefabPool.Get(null);
			newBar.Setup(health, damage, delegate
			{
				this.PrefabPool.Release(newBar);
			});
			return newBar;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00078DB4 File Offset: 0x00076FB4
		private void Health_OnRequestHealthBar(Health health)
		{
			HealthBar activeHealthBar = this.GetActiveHealthBar(health);
			if (activeHealthBar != null)
			{
				activeHealthBar.RefreshOffset();
				return;
			}
			this.CreateHealthBarFor(health, null);
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x00078DEA File Offset: 0x00076FEA
		public static void RequestHealthBar(Health health, DamageInfo? damage = null)
		{
			if (HealthBarManager.Instance == null)
			{
				return;
			}
			HealthBarManager.Instance.CreateHealthBarFor(health, damage);
		}

		// Token: 0x04001768 RID: 5992
		private static HealthBarManager _instance;

		// Token: 0x04001769 RID: 5993
		[SerializeField]
		public HealthBar healthBarPrefab;

		// Token: 0x0400176A RID: 5994
		private PrefabPool<HealthBar> _prefabPool;
	}
}
