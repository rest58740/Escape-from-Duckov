using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x020003E0 RID: 992
	[ExecuteInEditMode]
	public class RectTransformChangeEventEmitter : UIBehaviour
	{
		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x06002435 RID: 9269 RVA: 0x0007F32C File Offset: 0x0007D52C
		// (remove) Token: 0x06002436 RID: 9270 RVA: 0x0007F364 File Offset: 0x0007D564
		public event Action<RectTransform> OnRectTransformChange;

		// Token: 0x06002437 RID: 9271 RVA: 0x0007F399 File Offset: 0x0007D599
		private void SetDirty()
		{
			Action<RectTransform> onRectTransformChange = this.OnRectTransformChange;
			if (onRectTransformChange == null)
			{
				return;
			}
			onRectTransformChange(base.transform as RectTransform);
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0007F3B6 File Offset: 0x0007D5B6
		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0007F3BE File Offset: 0x0007D5BE
		protected override void OnEnable()
		{
			this.SetDirty();
		}
	}
}
