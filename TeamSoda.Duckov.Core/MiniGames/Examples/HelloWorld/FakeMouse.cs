using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.HelloWorld
{
	// Token: 0x020002DD RID: 733
	public class FakeMouse : MiniGameBehaviour
	{
		// Token: 0x0600176F RID: 5999 RVA: 0x000565F1 File Offset: 0x000547F1
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.parentRectTransform = (base.transform.parent as RectTransform);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0005661C File Offset: 0x0005481C
		protected override void OnUpdate(float deltaTime)
		{
			Vector3 vector = this.rectTransform.localPosition;
			vector += base.Game.GetAxis(1) * this.sensitivity;
			Rect rect = this.parentRectTransform.rect;
			vector.x = Mathf.Clamp(vector.x, rect.xMin, rect.xMax);
			vector.y = Mathf.Clamp(vector.y, rect.yMin, rect.yMax);
			this.rectTransform.localPosition = vector;
		}

		// Token: 0x04001121 RID: 4385
		[SerializeField]
		private float sensitivity = 1f;

		// Token: 0x04001122 RID: 4386
		private RectTransform rectTransform;

		// Token: 0x04001123 RID: 4387
		private RectTransform parentRectTransform;
	}
}
