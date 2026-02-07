using System;
using Duckov.Buffs;

namespace ItemStatsSystem
{
	// Token: 0x02000232 RID: 562
	[MenuPath("Buff/On Buff Layer Changed")]
	public class OnBuffLayerChanged : EffectTrigger
	{
		// Token: 0x06001189 RID: 4489 RVA: 0x00044983 File Offset: 0x00042B83
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0004498B File Offset: 0x00042B8B
		protected override void OnDisable()
		{
			base.OnDisable();
			this.UnregisterEvents();
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00044999 File Offset: 0x00042B99
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			this.RegisterEvents();
			this.OnBufflayerChanged();
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x000449A7 File Offset: 0x00042BA7
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			this.buff.OnLayerChangedEvent += this.OnBufflayerChanged;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x000449C6 File Offset: 0x00042BC6
		private void UnregisterEvents()
		{
			this.buff.OnLayerChangedEvent -= this.OnBufflayerChanged;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x000449DF File Offset: 0x00042BDF
		private void OnBufflayerChanged()
		{
			if (!this.needMaxLayers)
			{
				base.Trigger(true);
			}
			if (this.buff.CurrentLayers >= this.buff.MaxLayers)
			{
				base.Trigger(true);
			}
		}

		// Token: 0x04000DC6 RID: 3526
		public Buff buff;

		// Token: 0x04000DC7 RID: 3527
		public bool needMaxLayers;
	}
}
