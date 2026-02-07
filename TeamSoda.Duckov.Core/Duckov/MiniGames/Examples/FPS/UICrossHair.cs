using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002DF RID: 735
	public class UICrossHair : MiniGameBehaviour
	{
		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001778 RID: 6008 RVA: 0x000567B4 File Offset: 0x000549B4
		private float ScatterAngle
		{
			get
			{
				if (this.gunControl)
				{
					return this.gunControl.ScatterAngle;
				}
				return 0f;
			}
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000567D4 File Offset: 0x000549D4
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000567F0 File Offset: 0x000549F0
		protected override void OnUpdate(float deltaTime)
		{
			float scatterAngle = this.ScatterAngle;
			float fieldOfView = base.Game.Camera.fieldOfView;
			float y = this.canvasRectTransform.sizeDelta.y;
			float num = scatterAngle / fieldOfView;
			float num2 = (float)(Mathf.FloorToInt(y * num / 2f) * 2 + 1);
			this.rectTransform.sizeDelta = num2 * Vector2.one;
		}

		// Token: 0x04001128 RID: 4392
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001129 RID: 4393
		[SerializeField]
		private RectTransform canvasRectTransform;

		// Token: 0x0400112A RID: 4394
		[SerializeField]
		private FPSGunControl gunControl;
	}
}
