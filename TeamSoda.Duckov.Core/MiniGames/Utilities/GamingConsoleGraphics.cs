using System;
using Cysharp.Threading.Tasks;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x02000296 RID: 662
	public class GamingConsoleGraphics : MonoBehaviour
	{
		// Token: 0x06001579 RID: 5497 RVA: 0x00050138 File Offset: 0x0004E338
		private void Awake()
		{
			this.master.onContentChanged += this.OnContentChanged;
			this.master.OnAfterAnimateIn += this.OnAfterAnimateIn;
			this.master.OnBeforeAnimateOut += this.OnBeforeAnimateOut;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0005018A File Offset: 0x0004E38A
		private void Start()
		{
			this.dirty = true;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00050194 File Offset: 0x0004E394
		private void OnContentChanged(GamingConsole console)
		{
			if (console.Monitor != this._cachedMonitor)
			{
				this.OnMonitorChanged();
			}
			if (console.Console != this._cachedConsole)
			{
				this.OnConsoleChanged();
			}
			if (console.Cartridge != this._cachedCartridge)
			{
				this.OnCatridgeChanged();
			}
			this.dirty = true;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x000501F3 File Offset: 0x0004E3F3
		private void Update()
		{
			if (this.dirty)
			{
				this.RefreshDisplays();
				this.dirty = false;
			}
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0005020C File Offset: 0x0004E40C
		private void RefreshDisplays()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this._cachedMonitor = this.master.Monitor;
			this._cachedConsole = this.master.Console;
			this._cachedCartridge = this.master.Cartridge;
			if (this.monitorGraphic)
			{
				Object.Destroy(this.monitorGraphic.gameObject);
			}
			if (this.consoleGraphic)
			{
				Object.Destroy(this.consoleGraphic.gameObject);
			}
			if (this._cachedMonitor && !this._cachedMonitor.IsBeingDestroyed)
			{
				this.monitorGraphic = ItemGraphicInfo.CreateAGraphic(this._cachedMonitor, this.monitorRoot);
			}
			if (this._cachedConsole && !this._cachedConsole.IsBeingDestroyed)
			{
				this.consoleGraphic = ItemGraphicInfo.CreateAGraphic(this._cachedConsole, this.consoleRoot);
				if (this.consoleGraphic != null)
				{
					this.pickupAnimation = this.consoleGraphic.GetComponent<ControllerPickupAnimation>();
					this.controllerAnimator = this.consoleGraphic.GetComponentInChildren<ControllerAnimator>();
				}
				else
				{
					this.pickupAnimation = null;
					this.controllerAnimator = null;
				}
				if (this.controllerAnimator != null)
				{
					this.controllerAnimator.SetConsole(this.master);
				}
			}
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00050353 File Offset: 0x0004E553
		private void OnCatridgeChanged()
		{
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00050355 File Offset: 0x0004E555
		private void OnConsoleChanged()
		{
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00050357 File Offset: 0x0004E557
		private void OnMonitorChanged()
		{
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00050359 File Offset: 0x0004E559
		private void OnDestroy()
		{
			this.isBeingDestroyed = true;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00050362 File Offset: 0x0004E562
		private void OnBeforeAnimateOut(GamingConsole console)
		{
			if (this.pickupAnimation == null)
			{
				return;
			}
			this.pickupAnimation.PutDown().Forget();
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00050383 File Offset: 0x0004E583
		private void OnAfterAnimateIn(GamingConsole console)
		{
			if (this.pickupAnimation == null)
			{
				return;
			}
			this.pickupAnimation.PickUp(this.playingControllerPosition).Forget();
		}

		// Token: 0x04000FC1 RID: 4033
		[SerializeField]
		private GamingConsole master;

		// Token: 0x04000FC2 RID: 4034
		[SerializeField]
		private Transform monitorRoot;

		// Token: 0x04000FC3 RID: 4035
		[SerializeField]
		private Transform consoleRoot;

		// Token: 0x04000FC4 RID: 4036
		[SerializeField]
		private Transform playingControllerPosition;

		// Token: 0x04000FC5 RID: 4037
		private Transform cartridgeRoot;

		// Token: 0x04000FC6 RID: 4038
		private Item _cachedMonitor;

		// Token: 0x04000FC7 RID: 4039
		private Item _cachedConsole;

		// Token: 0x04000FC8 RID: 4040
		private Item _cachedCartridge;

		// Token: 0x04000FC9 RID: 4041
		private ItemGraphicInfo monitorGraphic;

		// Token: 0x04000FCA RID: 4042
		private ItemGraphicInfo consoleGraphic;

		// Token: 0x04000FCB RID: 4043
		private ControllerPickupAnimation pickupAnimation;

		// Token: 0x04000FCC RID: 4044
		private ControllerAnimator controllerAnimator;

		// Token: 0x04000FCD RID: 4045
		private bool dirty;

		// Token: 0x04000FCE RID: 4046
		private bool isBeingDestroyed;
	}
}
