using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003DE RID: 990
	public class WeightBarComplex : MonoBehaviour
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x0007EE02 File Offset: 0x0007D002
		private CharacterMainControl Target
		{
			get
			{
				if (!this.target)
				{
					LevelManager instance = LevelManager.Instance;
					this.target = ((instance != null) ? instance.MainCharacter : null);
				}
				return this.target;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x0007EE2E File Offset: 0x0007D02E
		private float LightPercentage
		{
			get
			{
				return 0.25f;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0007EE35 File Offset: 0x0007D035
		private float SuperHeavyPercentage
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x0007EE3C File Offset: 0x0007D03C
		private float MaxWeight
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.MaxWeight;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x0007EE60 File Offset: 0x0007D060
		private float BarWidth
		{
			get
			{
				if (this.barArea == null)
				{
					return 0f;
				}
				return this.barArea.rect.width;
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x0007EE94 File Offset: 0x0007D094
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			if (this.Target)
			{
				this.Target.CharacterItem.onChildChanged += this.OnTargetChildChanged;
			}
			this.RefreshMarkPositions();
			this.ResetMainBar();
			this.Animate().Forget();
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x0007EEF2 File Offset: 0x0007D0F2
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			if (this.Target)
			{
				this.Target.CharacterItem.onChildChanged -= this.OnTargetChildChanged;
			}
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x0007EF30 File Offset: 0x0007D130
		private void RefreshMarkPositions()
		{
			if (this.lightMark == null)
			{
				return;
			}
			if (this.superHeavyMark == null)
			{
				return;
			}
			float num = this.BarWidth * this.LightPercentage;
			float num2 = this.BarWidth * this.SuperHeavyPercentage;
			this.lightMark.anchoredPosition = Vector2.right * num;
			this.superHeavyMark.anchoredPosition = Vector2.right * num2;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0007EFA4 File Offset: 0x0007D1A4
		private void RefreshMarkStatus()
		{
			float num = 0f;
			if (this.MaxWeight > 0f)
			{
				num = this.Target.CharacterItem.TotalWeight / this.MaxWeight;
			}
			this.lightMarkToggle.SetToggle(num > this.LightPercentage);
			this.superHeavyMarkToggle.SetToggle(num > this.SuperHeavyPercentage);
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0007F004 File Offset: 0x0007D204
		private void OnTargetChildChanged(Item item)
		{
			this.Animate().Forget();
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x0007F011 File Offset: 0x0007D211
		private void OnItemSelectionChanged()
		{
			this.Animate().Forget();
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0007F020 File Offset: 0x0007D220
		private UniTask Animate()
		{
			WeightBarComplex.<Animate>d__33 <Animate>d__;
			<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Animate>d__.<>4__this = this;
			<Animate>d__.<>1__state = -1;
			<Animate>d__.<>t__builder.Start<WeightBarComplex.<Animate>d__33>(ref <Animate>d__);
			return <Animate>d__.<>t__builder.Task;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0007F064 File Offset: 0x0007D264
		private void ResetChangeBars()
		{
			ShortcutExtensions.DOKill(this.positiveBar, false);
			ShortcutExtensions.DOKill(this.negativeBar, false);
			this.positiveBar.sizeDelta = new Vector2(this.positiveBar.sizeDelta.x, 0f);
			this.negativeBar.sizeDelta = new Vector2(this.negativeBar.sizeDelta.x, 0f);
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x0007F0D5 File Offset: 0x0007D2D5
		private void ResetMainBar()
		{
			ShortcutExtensions.DOKill(this.mainBar, false);
			this.mainBar.sizeDelta = new Vector2(this.mainBar.sizeDelta.x, 0f);
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x0007F10C File Offset: 0x0007D30C
		private UniTask AnimateMainBar(int token)
		{
			WeightBarComplex.<AnimateMainBar>d__37 <AnimateMainBar>d__;
			<AnimateMainBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateMainBar>d__.<>4__this = this;
			<AnimateMainBar>d__.token = token;
			<AnimateMainBar>d__.<>1__state = -1;
			<AnimateMainBar>d__.<>t__builder.Start<WeightBarComplex.<AnimateMainBar>d__37>(ref <AnimateMainBar>d__);
			return <AnimateMainBar>d__.<>t__builder.Task;
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0007F158 File Offset: 0x0007D358
		private UniTask AnimatePositiveBar(int token)
		{
			WeightBarComplex.<AnimatePositiveBar>d__38 <AnimatePositiveBar>d__;
			<AnimatePositiveBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimatePositiveBar>d__.<>4__this = this;
			<AnimatePositiveBar>d__.token = token;
			<AnimatePositiveBar>d__.<>1__state = -1;
			<AnimatePositiveBar>d__.<>t__builder.Start<WeightBarComplex.<AnimatePositiveBar>d__38>(ref <AnimatePositiveBar>d__);
			return <AnimatePositiveBar>d__.<>t__builder.Task;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x0007F1A4 File Offset: 0x0007D3A4
		private UniTask AnimateNegativeBar(int token)
		{
			WeightBarComplex.<AnimateNegativeBar>d__39 <AnimateNegativeBar>d__;
			<AnimateNegativeBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateNegativeBar>d__.<>4__this = this;
			<AnimateNegativeBar>d__.token = token;
			<AnimateNegativeBar>d__.<>1__state = -1;
			<AnimateNegativeBar>d__.<>t__builder.Start<WeightBarComplex.<AnimateNegativeBar>d__39>(ref <AnimateNegativeBar>d__);
			return <AnimateNegativeBar>d__.<>t__builder.Task;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0007F1EF File Offset: 0x0007D3EF
		private void SetupInvalid()
		{
			WeightBarComplex.SetSizeDeltaY(this.mainBar, 0f);
			WeightBarComplex.SetSizeDeltaY(this.positiveBar, 0f);
			WeightBarComplex.SetSizeDeltaY(this.negativeBar, 0f);
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0007F224 File Offset: 0x0007D424
		private static void SetSizeDeltaY(RectTransform rectTransform, float sizeDelta)
		{
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			sizeDelta2.y = sizeDelta;
			rectTransform.sizeDelta = sizeDelta2;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x0007F247 File Offset: 0x0007D447
		private static float GetSizeDeltaY(RectTransform rectTransform)
		{
			return rectTransform.sizeDelta.y;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0007F254 File Offset: 0x0007D454
		private float WeightToRectHeight(float weight)
		{
			if (this.MaxWeight <= 0f)
			{
				return 0f;
			}
			float num = weight / this.MaxWeight;
			return this.BarWidth * num;
		}

		// Token: 0x0400188C RID: 6284
		[SerializeField]
		private CharacterMainControl target;

		// Token: 0x0400188D RID: 6285
		[SerializeField]
		private RectTransform barArea;

		// Token: 0x0400188E RID: 6286
		[SerializeField]
		private RectTransform mainBar;

		// Token: 0x0400188F RID: 6287
		[SerializeField]
		private Graphic mainBarGraphic;

		// Token: 0x04001890 RID: 6288
		[SerializeField]
		private RectTransform positiveBar;

		// Token: 0x04001891 RID: 6289
		[SerializeField]
		private RectTransform negativeBar;

		// Token: 0x04001892 RID: 6290
		[SerializeField]
		private RectTransform lightMark;

		// Token: 0x04001893 RID: 6291
		[SerializeField]
		private RectTransform superHeavyMark;

		// Token: 0x04001894 RID: 6292
		[SerializeField]
		private ToggleAnimation lightMarkToggle;

		// Token: 0x04001895 RID: 6293
		[SerializeField]
		private ToggleAnimation superHeavyMarkToggle;

		// Token: 0x04001896 RID: 6294
		[SerializeField]
		private Color superLightColor;

		// Token: 0x04001897 RID: 6295
		[SerializeField]
		private Color lightColor;

		// Token: 0x04001898 RID: 6296
		[SerializeField]
		private Color superHeavyColor;

		// Token: 0x04001899 RID: 6297
		[SerializeField]
		private Color overweightColor;

		// Token: 0x0400189A RID: 6298
		[SerializeField]
		private float animateDuration = 0.1f;

		// Token: 0x0400189B RID: 6299
		[SerializeField]
		private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400189C RID: 6300
		private float targetRealBarTop;

		// Token: 0x0400189D RID: 6301
		private int currentToken;
	}
}
