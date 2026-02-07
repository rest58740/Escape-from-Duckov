using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x02000299 RID: 665
	public class SnakePartDisplay : MiniGameBehaviour
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x0005135D File Offset: 0x0004F55D
		// (set) Token: 0x060015BD RID: 5565 RVA: 0x00051365 File Offset: 0x0004F565
		public SnakeDisplay Master { get; private set; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x0005136E File Offset: 0x0004F56E
		// (set) Token: 0x060015BF RID: 5567 RVA: 0x00051376 File Offset: 0x0004F576
		public SnakeForce.Part Target { get; private set; }

		// Token: 0x060015C0 RID: 5568 RVA: 0x00051380 File Offset: 0x0004F580
		internal void Setup(SnakeDisplay master, SnakeForce.Part part)
		{
			if (this.Target != null)
			{
				this.Target.OnMove -= this.OnTargetMove;
			}
			this.Master = master;
			this.Target = part;
			this.cachedCoord = this.Target.coord;
			base.transform.localPosition = this.Master.GetPosition(this.cachedCoord);
			this.Target.OnMove += this.OnTargetMove;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x00051400 File Offset: 0x0004F600
		private void OnTargetMove(SnakeForce.Part part)
		{
			if (!base.enabled)
			{
				return;
			}
			int sqrMagnitude = (this.Target.coord - this.cachedCoord).sqrMagnitude;
			this.cachedCoord = this.Target.coord;
			Vector3 position = this.Master.GetPosition(this.cachedCoord);
			this.DoMove(position);
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0005145F File Offset: 0x0004F65F
		private void DoMove(Vector3 vector3)
		{
			base.transform.localPosition = vector3;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00051470 File Offset: 0x0004F670
		internal void Punch()
		{
			ShortcutExtensions.DOKill(base.transform, true);
			base.transform.localScale = Vector3.one;
			ShortcutExtensions.DOPunchScale(base.transform, Vector3.one * 1.1f, 0.2f, 4, 1f);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x000514C0 File Offset: 0x0004F6C0
		internal void PunchColor(Color color)
		{
			ShortcutExtensions.DOKill(this.image, false);
			this.image.color = color;
			DOTweenModuleUI.DOColor(this.image, Color.white, 0.4f);
		}

		// Token: 0x04000FFF RID: 4095
		[SerializeField]
		private Image image;

		// Token: 0x04001000 RID: 4096
		private Vector2Int cachedCoord;
	}
}
