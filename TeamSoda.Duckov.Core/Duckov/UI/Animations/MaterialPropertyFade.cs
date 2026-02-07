using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003EF RID: 1007
	public class MaterialPropertyFade : FadeElement
	{
		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x000808EC File Offset: 0x0007EAEC
		// (set) Token: 0x060024A5 RID: 9381 RVA: 0x000808F4 File Offset: 0x0007EAF4
		public AnimationCurve ShowCurve
		{
			get
			{
				return this.showCurve;
			}
			set
			{
				this.showCurve = value;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060024A6 RID: 9382 RVA: 0x000808FD File Offset: 0x0007EAFD
		// (set) Token: 0x060024A7 RID: 9383 RVA: 0x00080905 File Offset: 0x0007EB05
		public AnimationCurve HideCurve
		{
			get
			{
				return this.hideCurve;
			}
			set
			{
				this.hideCurve = value;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060024A8 RID: 9384 RVA: 0x00080910 File Offset: 0x0007EB10
		private Material Material
		{
			get
			{
				if (this._material == null && this.renderer != null)
				{
					this._material = Object.Instantiate<Material>(this.renderer.material);
					this.renderer.material = this._material;
				}
				return this._material;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060024A9 RID: 9385 RVA: 0x00080966 File Offset: 0x0007EB66
		// (set) Token: 0x060024AA RID: 9386 RVA: 0x0008096E File Offset: 0x0007EB6E
		public float Duration
		{
			get
			{
				return this.duration;
			}
			internal set
			{
				this.duration = value;
			}
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x00080977 File Offset: 0x0007EB77
		private void Awake()
		{
			if (this.renderer == null)
			{
				this.renderer = base.GetComponent<Image>();
			}
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x00080993 File Offset: 0x0007EB93
		private void OnDestroy()
		{
			if (this._material)
			{
				Object.Destroy(this._material);
			}
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000809B0 File Offset: 0x0007EBB0
		protected override UniTask HideTask(int token)
		{
			MaterialPropertyFade.<HideTask>d__20 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.token = token;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<MaterialPropertyFade.<HideTask>d__20>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000809FB File Offset: 0x0007EBFB
		protected override void OnSkipHide()
		{
			if (this.Material == null)
			{
				return;
			}
			this.Material.SetFloat(this.propertyName, this.propertyRange.x);
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x00080A28 File Offset: 0x0007EC28
		protected override void OnSkipShow()
		{
			if (this.Material == null)
			{
				return;
			}
			this.Material.SetFloat(this.propertyName, this.propertyRange.y);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x00080A58 File Offset: 0x0007EC58
		protected override UniTask ShowTask(int token)
		{
			MaterialPropertyFade.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.token = token;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<MaterialPropertyFade.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x00080B20 File Offset: 0x0007ED20
		[CompilerGenerated]
		internal static float <HideTask>g__TimeSinceFadeBegun|20_0(ref MaterialPropertyFade.<>c__DisplayClass20_0 A_0)
		{
			return Time.unscaledTime - A_0.timeWhenFadeBegun;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x00080B2E File Offset: 0x0007ED2E
		[CompilerGenerated]
		internal static float <ShowTask>g__TimeSinceFadeBegun|23_0(ref MaterialPropertyFade.<>c__DisplayClass23_0 A_0)
		{
			return Time.unscaledTime - A_0.timeWhenFadeBegun;
		}

		// Token: 0x040018E5 RID: 6373
		[SerializeField]
		private Image renderer;

		// Token: 0x040018E6 RID: 6374
		[SerializeField]
		private string propertyName = "t";

		// Token: 0x040018E7 RID: 6375
		[SerializeField]
		private Vector2 propertyRange = new Vector2(0f, 1f);

		// Token: 0x040018E8 RID: 6376
		[SerializeField]
		private float duration = 0.5f;

		// Token: 0x040018E9 RID: 6377
		[SerializeField]
		private AnimationCurve showCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040018EA RID: 6378
		[SerializeField]
		private AnimationCurve hideCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040018EB RID: 6379
		private Material _material;
	}
}
