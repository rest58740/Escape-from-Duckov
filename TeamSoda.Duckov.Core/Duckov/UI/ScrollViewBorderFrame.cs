using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003E1 RID: 993
	public class ScrollViewBorderFrame : MonoBehaviour
	{
		// Token: 0x0600243B RID: 9275 RVA: 0x0007F3CE File Offset: 0x0007D5CE
		private void OnEnable()
		{
			this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.Refresh));
			UniTask.Void(delegate()
			{
				ScrollViewBorderFrame.<<OnEnable>b__8_0>d <<OnEnable>b__8_0>d;
				<<OnEnable>b__8_0>d.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<<OnEnable>b__8_0>d.<>4__this = this;
				<<OnEnable>b__8_0>d.<>1__state = -1;
				<<OnEnable>b__8_0>d.<>t__builder.Start<ScrollViewBorderFrame.<<OnEnable>b__8_0>d>(ref <<OnEnable>b__8_0>d);
				return <<OnEnable>b__8_0>d.<>t__builder.Task;
			});
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0007F3FD File Offset: 0x0007D5FD
		private void OnDisable()
		{
			this.scrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.Refresh));
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0007F41B File Offset: 0x0007D61B
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0007F424 File Offset: 0x0007D624
		private void Refresh(Vector2 scrollPos)
		{
			RectTransform viewport = this.scrollRect.viewport;
			RectTransform content = this.scrollRect.content;
			Rect rect = viewport.rect;
			Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, content);
			float num = bounds.max.y - rect.max.y + this.extendOffset;
			float num2 = rect.min.y - bounds.min.y + this.extendOffset;
			float num3 = rect.min.x - bounds.min.x + this.extendOffset;
			float num4 = bounds.max.x - rect.max.x + this.extendOffset;
			float alpha = Mathf.Lerp(0f, this.maxAlpha, num / this.extendThreshold);
			float alpha2 = Mathf.Lerp(0f, this.maxAlpha, num2 / this.extendThreshold);
			float alpha3 = Mathf.Lerp(0f, this.maxAlpha, num3 / this.extendThreshold);
			float alpha4 = Mathf.Lerp(0f, this.maxAlpha, num4 / this.extendThreshold);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.upGraphic, alpha);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.downGraphic, alpha2);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.leftGraphic, alpha3);
			ScrollViewBorderFrame.<Refresh>g__SetAlpha|11_0(this.rightGraphic, alpha4);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0007F57C File Offset: 0x0007D77C
		private void Refresh()
		{
			if (this.scrollRect == null)
			{
				return;
			}
			this.Refresh(this.scrollRect.normalizedPosition);
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0007F600 File Offset: 0x0007D800
		[CompilerGenerated]
		internal static void <Refresh>g__SetAlpha|11_0(Graphic graphic, float alpha)
		{
			if (graphic == null)
			{
				return;
			}
			Color color = graphic.color;
			color.a = alpha;
			graphic.color = color;
		}

		// Token: 0x040018A1 RID: 6305
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x040018A2 RID: 6306
		[Range(0f, 1f)]
		[SerializeField]
		private float maxAlpha = 1f;

		// Token: 0x040018A3 RID: 6307
		[SerializeField]
		private float extendThreshold = 10f;

		// Token: 0x040018A4 RID: 6308
		[SerializeField]
		private float extendOffset;

		// Token: 0x040018A5 RID: 6309
		[SerializeField]
		private Graphic upGraphic;

		// Token: 0x040018A6 RID: 6310
		[SerializeField]
		private Graphic downGraphic;

		// Token: 0x040018A7 RID: 6311
		[SerializeField]
		private Graphic leftGraphic;

		// Token: 0x040018A8 RID: 6312
		[SerializeField]
		private Graphic rightGraphic;
	}
}
