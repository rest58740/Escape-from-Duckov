using System;
using UnityEngine.Events;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003EA RID: 1002
	public class BarDisplayController_HP : BarDisplayController
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002481 RID: 9345 RVA: 0x0008032E File Offset: 0x0007E52E
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.Health.CurrentHealth;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002482 RID: 9346 RVA: 0x00080354 File Offset: 0x0007E554
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.Health.MaxHealth;
			}
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x0008037A File Offset: 0x0007E57A
		private void OnEnable()
		{
			base.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x00080388 File Offset: 0x0007E588
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x00080390 File Offset: 0x0007E590
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.Health.OnHealthChange.AddListener(new UnityAction<Health>(this.OnHealthChange));
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000803C2 File Offset: 0x0007E5C2
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.Health.OnHealthChange.RemoveListener(new UnityAction<Health>(this.OnHealthChange));
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x000803F4 File Offset: 0x0007E5F4
		private CharacterMainControl Target
		{
			get
			{
				if (this._target == null)
				{
					this._target = CharacterMainControl.Main;
				}
				return this._target;
			}
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x00080415 File Offset: 0x0007E615
		private void OnHealthChange(Health health)
		{
			base.Refresh();
		}

		// Token: 0x040018D5 RID: 6357
		private CharacterMainControl _target;
	}
}
