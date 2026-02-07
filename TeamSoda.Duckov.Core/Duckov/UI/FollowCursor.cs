using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x020003DF RID: 991
	public class FollowCursor : MonoBehaviour
	{
		// Token: 0x06002432 RID: 9266 RVA: 0x0007F2B7 File Offset: 0x0007D4B7
		private void Awake()
		{
			this.parentRectTransform = (base.transform.parent as RectTransform);
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0007F2E0 File Offset: 0x0007D4E0
		private unsafe void Update()
		{
			Vector2 vector = *Mouse.current.position.value;
			Vector2 vector2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentRectTransform, vector, null, ref vector2);
			this.rectTransform.localPosition = vector2;
		}

		// Token: 0x0400189E RID: 6302
		private RectTransform parentRectTransform;

		// Token: 0x0400189F RID: 6303
		private RectTransform rectTransform;
	}
}
