using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
	// Token: 0x02000003 RID: 3
	[AddComponentMenu("DOTween/DOTween Animation")]
	public class DOTweenAnimation : ABSAnimationComponent
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		// (remove) Token: 0x06000004 RID: 4 RVA: 0x000020F4 File Offset: 0x000002F4
		public static event Action<DOTweenAnimation> OnReset;

		// Token: 0x06000005 RID: 5 RVA: 0x00002127 File Offset: 0x00000327
		private static void Dispatch_OnReset(DOTweenAnimation anim)
		{
			if (DOTweenAnimation.OnReset != null)
			{
				DOTweenAnimation.OnReset(anim);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000213B File Offset: 0x0000033B
		private void Awake()
		{
			if (!this.isActive || !this.autoGenerate)
			{
				return;
			}
			if (this.animationType != DOTweenAnimation.AnimationType.Move || !this.useTargetAsV3)
			{
				this.CreateTween(false, this.autoPlay);
				this._tweenAutoGenerationCalled = true;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002173 File Offset: 0x00000373
		private void Start()
		{
			if (this._tweenAutoGenerationCalled || !this.isActive || !this.autoGenerate)
			{
				return;
			}
			this.CreateTween(false, this.autoPlay);
			this._tweenAutoGenerationCalled = true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021A2 File Offset: 0x000003A2
		private void Reset()
		{
			DOTweenAnimation.Dispatch_OnReset(this);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021AA File Offset: 0x000003AA
		private void OnDestroy()
		{
			if (this.tween != null && this.tween.active)
			{
				TweenExtensions.Kill(this.tween, false);
			}
			this.tween = null;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021D4 File Offset: 0x000003D4
		public void RewindThenRecreateTween()
		{
			if (this.tween != null && this.tween.active)
			{
				TweenExtensions.Rewind(this.tween, true);
			}
			this.CreateTween(true, false);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021FF File Offset: 0x000003FF
		public void RewindThenRecreateTweenAndPlay()
		{
			if (this.tween != null && this.tween.active)
			{
				TweenExtensions.Rewind(this.tween, true);
			}
			this.CreateTween(true, true);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000222A File Offset: 0x0000042A
		public void RecreateTween()
		{
			this.CreateTween(true, false);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002234 File Offset: 0x00000434
		public void RecreateTweenAndPlay()
		{
			this.CreateTween(true, true);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002240 File Offset: 0x00000440
		public void CreateTween(bool regenerateIfExists = false, bool andPlay = true)
		{
			if (!this.isValid)
			{
				if (regenerateIfExists)
				{
					Debug.LogWarning(string.Format("{0} :: This DOTweenAnimation isn't valid and its tween won't be created", base.gameObject.name), base.gameObject);
				}
				return;
			}
			if (this.tween != null)
			{
				if (this.tween.active)
				{
					if (!regenerateIfExists)
					{
						return;
					}
					TweenExtensions.Kill(this.tween, false);
				}
				this.tween = null;
			}
			GameObject tweenGO = this.GetTweenGO();
			if (this.target == null || tweenGO == null)
			{
				if (this.targetIsSelf && this.target == null)
				{
					Debug.LogWarning(string.Format("{0} :: This DOTweenAnimation's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", base.gameObject.name), base.gameObject);
					return;
				}
				Debug.LogWarning(string.Format("{0} :: This DOTweenAnimation's target/GameObject is unset: the tween will not be created.", base.gameObject.name), base.gameObject);
				return;
			}
			else
			{
				if (this.forcedTargetType != DOTweenAnimation.TargetType.Unset)
				{
					this.targetType = this.forcedTargetType;
				}
				if (this.targetType == DOTweenAnimation.TargetType.Unset)
				{
					this.targetType = DOTweenAnimation.TypeToDOTargetType(this.target.GetType());
				}
				switch (this.animationType)
				{
				case DOTweenAnimation.AnimationType.Move:
					if (this.useTargetAsV3)
					{
						this.isRelative = false;
						if (this.endValueTransform == null)
						{
							Debug.LogWarning(string.Format("{0} :: This tween's TO target is NULL, a Vector3 of (0,0,0) will be used instead", base.gameObject.name), base.gameObject);
							this.endValueV3 = Vector3.zero;
						}
						else if (this.targetType == DOTweenAnimation.TargetType.RectTransform)
						{
							RectTransform rectTransform = this.endValueTransform as RectTransform;
							if (rectTransform == null)
							{
								Debug.LogWarning(string.Format("{0} :: This tween's TO target should be a RectTransform, a Vector3 of (0,0,0) will be used instead", base.gameObject.name), base.gameObject);
								this.endValueV3 = Vector3.zero;
							}
							else
							{
								RectTransform rectTransform2 = this.target as RectTransform;
								if (rectTransform2 == null)
								{
									Debug.LogWarning(string.Format("{0} :: This tween's target and TO target are not of the same type. Please reassign the values", base.gameObject.name), base.gameObject);
								}
								else
								{
									this.endValueV3 = DOTweenModuleUI.Utils.SwitchToRectTransform(rectTransform, rectTransform2);
								}
							}
						}
						else
						{
							this.endValueV3 = this.endValueTransform.position;
						}
					}
					switch (this.targetType)
					{
					case DOTweenAnimation.TargetType.RectTransform:
						this.tween = DOTweenModuleUI.DOAnchorPos3D((RectTransform)this.target, this.endValueV3, this.duration, this.optionalBool0);
						break;
					case DOTweenAnimation.TargetType.Rigidbody:
						this.tween = DOTweenModulePhysics.DOMove((Rigidbody)this.target, this.endValueV3, this.duration, this.optionalBool0);
						break;
					case DOTweenAnimation.TargetType.Rigidbody2D:
						this.tween = DOTweenModulePhysics2D.DOMove((Rigidbody2D)this.target, this.endValueV3, this.duration, this.optionalBool0);
						break;
					case DOTweenAnimation.TargetType.Transform:
						this.tween = ShortcutExtensions.DOMove((Transform)this.target, this.endValueV3, this.duration, this.optionalBool0);
						break;
					}
					break;
				case DOTweenAnimation.AnimationType.LocalMove:
					this.tween = ShortcutExtensions.DOLocalMove(tweenGO.transform, this.endValueV3, this.duration, this.optionalBool0);
					break;
				case DOTweenAnimation.AnimationType.Rotate:
					switch (this.targetType)
					{
					case DOTweenAnimation.TargetType.Rigidbody:
						this.tween = DOTweenModulePhysics.DORotate((Rigidbody)this.target, this.endValueV3, this.duration, this.optionalRotationMode);
						break;
					case DOTweenAnimation.TargetType.Rigidbody2D:
						this.tween = DOTweenModulePhysics2D.DORotate((Rigidbody2D)this.target, this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Transform:
						this.tween = ShortcutExtensions.DORotate((Transform)this.target, this.endValueV3, this.duration, this.optionalRotationMode);
						break;
					}
					break;
				case DOTweenAnimation.AnimationType.LocalRotate:
					this.tween = ShortcutExtensions.DOLocalRotate(tweenGO.transform, this.endValueV3, this.duration, this.optionalRotationMode);
					break;
				case DOTweenAnimation.AnimationType.Scale:
				{
					DOTweenAnimation.TargetType targetType = this.targetType;
					this.tween = ShortcutExtensions.DOScale(tweenGO.transform, this.optionalBool0 ? new Vector3(this.endValueFloat, this.endValueFloat, this.endValueFloat) : this.endValueV3, this.duration);
					break;
				}
				case DOTweenAnimation.AnimationType.Color:
					this.isRelative = false;
					switch (this.targetType)
					{
					case DOTweenAnimation.TargetType.Image:
						this.tween = DOTweenModuleUI.DOColor((Graphic)this.target, this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.Light:
						this.tween = ShortcutExtensions.DOColor((Light)this.target, this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.Renderer:
						this.tween = ShortcutExtensions.DOColor(((Renderer)this.target).material, this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.SpriteRenderer:
						this.tween = DOTweenModuleSprite.DOColor((SpriteRenderer)this.target, this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.Text:
						this.tween = DOTweenModuleUI.DOColor((Text)this.target, this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.TextMeshPro:
						this.tween = ((TextMeshPro)this.target).DOColor(this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.TextMeshProUGUI:
						this.tween = ((TextMeshProUGUI)this.target).DOColor(this.endValueColor, this.duration);
						break;
					}
					break;
				case DOTweenAnimation.AnimationType.Fade:
					this.isRelative = false;
					switch (this.targetType)
					{
					case DOTweenAnimation.TargetType.CanvasGroup:
						this.tween = DOTweenModuleUI.DOFade((CanvasGroup)this.target, this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Image:
						this.tween = DOTweenModuleUI.DOFade((Graphic)this.target, this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Light:
						this.tween = ShortcutExtensions.DOIntensity((Light)this.target, this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Renderer:
						this.tween = ShortcutExtensions.DOFade(((Renderer)this.target).material, this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.SpriteRenderer:
						this.tween = DOTweenModuleSprite.DOFade((SpriteRenderer)this.target, this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Text:
						this.tween = DOTweenModuleUI.DOFade((Text)this.target, this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.TextMeshPro:
						this.tween = ((TextMeshPro)this.target).DOFade(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.TextMeshProUGUI:
						this.tween = ((TextMeshProUGUI)this.target).DOFade(this.endValueFloat, this.duration);
						break;
					}
					break;
				case DOTweenAnimation.AnimationType.Text:
				{
					if (this.targetType == DOTweenAnimation.TargetType.Text)
					{
						this.tween = DOTweenModuleUI.DOText((Text)this.target, this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
					}
					DOTweenAnimation.TargetType targetType2 = this.targetType;
					if (targetType2 != DOTweenAnimation.TargetType.TextMeshPro)
					{
						if (targetType2 == DOTweenAnimation.TargetType.TextMeshProUGUI)
						{
							this.tween = ((TextMeshProUGUI)this.target).DOText(this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
						}
					}
					else
					{
						this.tween = ((TextMeshPro)this.target).DOText(this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
					}
					break;
				}
				case DOTweenAnimation.AnimationType.PunchPosition:
				{
					DOTweenAnimation.TargetType targetType2 = this.targetType;
					if (targetType2 != DOTweenAnimation.TargetType.RectTransform)
					{
						if (targetType2 == DOTweenAnimation.TargetType.Transform)
						{
							this.tween = ShortcutExtensions.DOPunchPosition((Transform)this.target, this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
						}
					}
					else
					{
						this.tween = DOTweenModuleUI.DOPunchAnchorPos((RectTransform)this.target, this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
					}
					break;
				}
				case DOTweenAnimation.AnimationType.PunchRotation:
					this.tween = ShortcutExtensions.DOPunchRotation(tweenGO.transform, this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
					break;
				case DOTweenAnimation.AnimationType.PunchScale:
					this.tween = ShortcutExtensions.DOPunchScale(tweenGO.transform, this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
					break;
				case DOTweenAnimation.AnimationType.ShakePosition:
				{
					DOTweenAnimation.TargetType targetType2 = this.targetType;
					if (targetType2 != DOTweenAnimation.TargetType.RectTransform)
					{
						if (targetType2 == DOTweenAnimation.TargetType.Transform)
						{
							this.tween = ShortcutExtensions.DOShakePosition((Transform)this.target, this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0, this.optionalBool1);
						}
					}
					else
					{
						this.tween = DOTweenModuleUI.DOShakeAnchorPos((RectTransform)this.target, this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0, this.optionalBool1);
					}
					break;
				}
				case DOTweenAnimation.AnimationType.ShakeRotation:
					this.tween = ShortcutExtensions.DOShakeRotation(tweenGO.transform, this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool1);
					break;
				case DOTweenAnimation.AnimationType.ShakeScale:
					this.tween = ShortcutExtensions.DOShakeScale(tweenGO.transform, this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool1);
					break;
				case DOTweenAnimation.AnimationType.CameraAspect:
					this.tween = ShortcutExtensions.DOAspect((Camera)this.target, this.endValueFloat, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraBackgroundColor:
					this.tween = ShortcutExtensions.DOColor((Camera)this.target, this.endValueColor, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraFieldOfView:
					this.tween = ShortcutExtensions.DOFieldOfView((Camera)this.target, this.endValueFloat, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraOrthoSize:
					this.tween = ShortcutExtensions.DOOrthoSize((Camera)this.target, this.endValueFloat, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraPixelRect:
					this.tween = ShortcutExtensions.DOPixelRect((Camera)this.target, this.endValueRect, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraRect:
					this.tween = ShortcutExtensions.DORect((Camera)this.target, this.endValueRect, this.duration);
					break;
				case DOTweenAnimation.AnimationType.UIWidthHeight:
					this.tween = DOTweenModuleUI.DOSizeDelta((RectTransform)this.target, this.optionalBool0 ? new Vector2(this.endValueFloat, this.endValueFloat) : this.endValueV2, this.duration, false);
					break;
				}
				if (this.tween == null)
				{
					return;
				}
				if (this.isFrom)
				{
					TweenSettingsExtensions.From<Tweener>((Tweener)this.tween, this.isRelative);
				}
				else
				{
					TweenSettingsExtensions.SetRelative<Tween>(this.tween, this.isRelative);
				}
				GameObject tweenTarget = this.GetTweenTarget();
				TweenSettingsExtensions.OnKill<Tween>(TweenSettingsExtensions.SetAutoKill<Tween>(TweenSettingsExtensions.SetLoops<Tween>(TweenSettingsExtensions.SetDelay<Tween>(TweenSettingsExtensions.SetTarget<Tween>(this.tween, tweenTarget), this.delay), this.loops, this.loopType), this.autoKill), delegate()
				{
					this.tween = null;
				});
				if (this.isSpeedBased)
				{
					TweenSettingsExtensions.SetSpeedBased<Tween>(this.tween);
				}
				if (this.easeType == 37)
				{
					TweenSettingsExtensions.SetEase<Tween>(this.tween, this.easeCurve);
				}
				else
				{
					TweenSettingsExtensions.SetEase<Tween>(this.tween, this.easeType);
				}
				if (!string.IsNullOrEmpty(this.id))
				{
					TweenSettingsExtensions.SetId<Tween>(this.tween, this.id);
				}
				TweenSettingsExtensions.SetUpdate<Tween>(this.tween, this.isIndependentUpdate);
				if (this.hasOnStart)
				{
					if (this.onStart != null)
					{
						TweenSettingsExtensions.OnStart<Tween>(this.tween, new TweenCallback(this.onStart.Invoke));
					}
				}
				else
				{
					this.onStart = null;
				}
				if (this.hasOnPlay)
				{
					if (this.onPlay != null)
					{
						TweenSettingsExtensions.OnPlay<Tween>(this.tween, new TweenCallback(this.onPlay.Invoke));
					}
				}
				else
				{
					this.onPlay = null;
				}
				if (this.hasOnUpdate)
				{
					if (this.onUpdate != null)
					{
						TweenSettingsExtensions.OnUpdate<Tween>(this.tween, new TweenCallback(this.onUpdate.Invoke));
					}
				}
				else
				{
					this.onUpdate = null;
				}
				if (this.hasOnStepComplete)
				{
					if (this.onStepComplete != null)
					{
						TweenSettingsExtensions.OnStepComplete<Tween>(this.tween, new TweenCallback(this.onStepComplete.Invoke));
					}
				}
				else
				{
					this.onStepComplete = null;
				}
				if (this.hasOnComplete)
				{
					if (this.onComplete != null)
					{
						TweenSettingsExtensions.OnComplete<Tween>(this.tween, new TweenCallback(this.onComplete.Invoke));
					}
				}
				else
				{
					this.onComplete = null;
				}
				if (this.hasOnRewind)
				{
					if (this.onRewind != null)
					{
						TweenSettingsExtensions.OnRewind<Tween>(this.tween, new TweenCallback(this.onRewind.Invoke));
					}
				}
				else
				{
					this.onRewind = null;
				}
				if (andPlay)
				{
					TweenExtensions.Play<Tween>(this.tween);
				}
				else
				{
					TweenExtensions.Pause<Tween>(this.tween);
				}
				if (this.hasOnTweenCreated && this.onTweenCreated != null)
				{
					this.onTweenCreated.Invoke();
				}
				return;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003024 File Offset: 0x00001224
		public List<Tween> GetTweens()
		{
			List<Tween> list = new List<Tween>();
			foreach (DOTweenAnimation dotweenAnimation in base.GetComponents<DOTweenAnimation>())
			{
				if (dotweenAnimation.tween != null && dotweenAnimation.tween.active)
				{
					list.Add(dotweenAnimation.tween);
				}
			}
			return list;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003074 File Offset: 0x00001274
		public void SetAnimationTarget(Component tweenTarget, bool useTweenTargetGameObjectForGroupOperations = true)
		{
			if (DOTweenAnimation.TypeToDOTargetType(this.target.GetType()) != this.targetType)
			{
				Debug.LogError("DOTweenAnimation ► SetAnimationTarget: the new target is of a different type from the one set in the Inspector");
				return;
			}
			this.target = tweenTarget;
			this.targetGO = this.target.gameObject;
			this.tweenTargetIsTargetGO = useTweenTargetGameObjectForGroupOperations;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000030C3 File Offset: 0x000012C3
		public override void DOPlay()
		{
			DOTween.Play(this.GetTweenTarget());
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000030D1 File Offset: 0x000012D1
		public override void DOPlayBackwards()
		{
			DOTween.PlayBackwards(this.GetTweenTarget());
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000030DF File Offset: 0x000012DF
		public override void DOPlayForward()
		{
			DOTween.PlayForward(this.GetTweenTarget());
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000030ED File Offset: 0x000012ED
		public override void DOPause()
		{
			DOTween.Pause(this.GetTweenTarget());
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000030FB File Offset: 0x000012FB
		public override void DOTogglePause()
		{
			DOTween.TogglePause(this.GetTweenTarget());
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000310C File Offset: 0x0000130C
		public override void DORewind()
		{
			this._playCount = -1;
			DOTweenAnimation[] components = base.gameObject.GetComponents<DOTweenAnimation>();
			for (int i = components.Length - 1; i > -1; i--)
			{
				Tween tween = components[i].tween;
				if (tween != null && TweenExtensions.IsInitialized(tween))
				{
					TweenExtensions.Rewind(components[i].tween, true);
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000315E File Offset: 0x0000135E
		public override void DORestart()
		{
			this.DORestart(false);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003168 File Offset: 0x00001368
		public override void DORestart(bool fromHere)
		{
			this._playCount = -1;
			if (this.tween == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(this.tween);
				}
				return;
			}
			if (fromHere && this.isRelative)
			{
				this.ReEvaluateRelativeTween();
			}
			DOTween.Restart(this.GetTweenTarget(), true, -1f);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000031BB File Offset: 0x000013BB
		public override void DOComplete()
		{
			DOTween.Complete(this.GetTweenTarget(), false);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000031CA File Offset: 0x000013CA
		public override void DOKill()
		{
			DOTween.Kill(this.GetTweenTarget(), false);
			this.tween = null;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000031E0 File Offset: 0x000013E0
		public void DOPlayById(string id)
		{
			DOTween.Play(this.GetTweenTarget(), id);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000031EF File Offset: 0x000013EF
		public void DOPlayAllById(string id)
		{
			DOTween.Play(id);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000031F8 File Offset: 0x000013F8
		public void DOPauseAllById(string id)
		{
			DOTween.Pause(id);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003201 File Offset: 0x00001401
		public void DOPlayBackwardsById(string id)
		{
			DOTween.PlayBackwards(this.GetTweenTarget(), id);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003210 File Offset: 0x00001410
		public void DOPlayBackwardsAllById(string id)
		{
			DOTween.PlayBackwards(id);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003219 File Offset: 0x00001419
		public void DOPlayForwardById(string id)
		{
			DOTween.PlayForward(this.GetTweenTarget(), id);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003228 File Offset: 0x00001428
		public void DOPlayForwardAllById(string id)
		{
			DOTween.PlayForward(id);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003234 File Offset: 0x00001434
		public void DOPlayNext()
		{
			DOTweenAnimation[] components = base.GetComponents<DOTweenAnimation>();
			while (this._playCount < components.Length - 1)
			{
				this._playCount++;
				DOTweenAnimation dotweenAnimation = components[this._playCount];
				if (dotweenAnimation != null && dotweenAnimation.tween != null && dotweenAnimation.tween.active && !TweenExtensions.IsPlaying(dotweenAnimation.tween) && !TweenExtensions.IsComplete(dotweenAnimation.tween))
				{
					TweenExtensions.Play<Tween>(dotweenAnimation.tween);
					return;
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000032B3 File Offset: 0x000014B3
		public void DORewindAndPlayNext()
		{
			this._playCount = -1;
			DOTween.Rewind(this.GetTweenTarget(), true);
			this.DOPlayNext();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000032CF File Offset: 0x000014CF
		public void DORewindAllById(string id)
		{
			this._playCount = -1;
			DOTween.Rewind(id, true);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000032E0 File Offset: 0x000014E0
		public void DORestartById(string id)
		{
			this._playCount = -1;
			DOTween.Restart(this.GetTweenTarget(), id, true, -1f);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000032FC File Offset: 0x000014FC
		public void DORestartAllById(string id)
		{
			this._playCount = -1;
			DOTween.Restart(id, true, -1f);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003312 File Offset: 0x00001512
		public void DOKillById(string id)
		{
			DOTween.Kill(this.GetTweenTarget(), id, false);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003322 File Offset: 0x00001522
		public void DOKillAllById(string id)
		{
			DOTween.Kill(id, false);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000332C File Offset: 0x0000152C
		public static DOTweenAnimation.TargetType TypeToDOTargetType(Type t)
		{
			string text = t.ToString();
			int num = text.LastIndexOf(".");
			if (num != -1)
			{
				text = text.Substring(num + 1);
			}
			if (text.IndexOf("Renderer") != -1 && text != "SpriteRenderer")
			{
				text = "Renderer";
			}
			if (text == "RawImage" || text == "Graphic")
			{
				text = "Image";
			}
			return (DOTweenAnimation.TargetType)Enum.Parse(typeof(DOTweenAnimation.TargetType), text);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000033B0 File Offset: 0x000015B0
		public Tween CreateEditorPreview()
		{
			if (Application.isPlaying)
			{
				return null;
			}
			this.CreateTween(true, this.autoPlay);
			return this.tween;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000033CE File Offset: 0x000015CE
		private GameObject GetTweenGO()
		{
			if (!this.targetIsSelf)
			{
				return this.targetGO;
			}
			return base.gameObject;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000033E5 File Offset: 0x000015E5
		private GameObject GetTweenTarget()
		{
			if (!this.targetIsSelf && this.tweenTargetIsTargetGO)
			{
				return this.targetGO;
			}
			return base.gameObject;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003404 File Offset: 0x00001604
		private void ReEvaluateRelativeTween()
		{
			GameObject tweenGO = this.GetTweenGO();
			if (tweenGO == null)
			{
				Debug.LogWarning(string.Format("{0} :: This DOTweenAnimation's target/GameObject is unset: the tween will not be created.", base.gameObject.name), base.gameObject);
				return;
			}
			if (this.animationType == DOTweenAnimation.AnimationType.Move)
			{
				((Tweener)this.tween).ChangeEndValue(tweenGO.transform.position + this.endValueV3, true);
				return;
			}
			if (this.animationType == DOTweenAnimation.AnimationType.LocalMove)
			{
				((Tweener)this.tween).ChangeEndValue(tweenGO.transform.localPosition + this.endValueV3, true);
			}
		}

		// Token: 0x04000002 RID: 2
		public bool targetIsSelf = true;

		// Token: 0x04000003 RID: 3
		public GameObject targetGO;

		// Token: 0x04000004 RID: 4
		public bool tweenTargetIsTargetGO = true;

		// Token: 0x04000005 RID: 5
		public float delay;

		// Token: 0x04000006 RID: 6
		public float duration = 1f;

		// Token: 0x04000007 RID: 7
		public Ease easeType = 6;

		// Token: 0x04000008 RID: 8
		public AnimationCurve easeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000009 RID: 9
		public LoopType loopType;

		// Token: 0x0400000A RID: 10
		public int loops = 1;

		// Token: 0x0400000B RID: 11
		public string id = "";

		// Token: 0x0400000C RID: 12
		public bool isRelative;

		// Token: 0x0400000D RID: 13
		public bool isFrom;

		// Token: 0x0400000E RID: 14
		public bool isIndependentUpdate;

		// Token: 0x0400000F RID: 15
		public bool autoKill = true;

		// Token: 0x04000010 RID: 16
		public bool autoGenerate = true;

		// Token: 0x04000011 RID: 17
		public bool isActive = true;

		// Token: 0x04000012 RID: 18
		public bool isValid;

		// Token: 0x04000013 RID: 19
		public Component target;

		// Token: 0x04000014 RID: 20
		public DOTweenAnimation.AnimationType animationType;

		// Token: 0x04000015 RID: 21
		public DOTweenAnimation.TargetType targetType;

		// Token: 0x04000016 RID: 22
		public DOTweenAnimation.TargetType forcedTargetType;

		// Token: 0x04000017 RID: 23
		public bool autoPlay = true;

		// Token: 0x04000018 RID: 24
		public bool useTargetAsV3;

		// Token: 0x04000019 RID: 25
		public float endValueFloat;

		// Token: 0x0400001A RID: 26
		public Vector3 endValueV3;

		// Token: 0x0400001B RID: 27
		public Vector2 endValueV2;

		// Token: 0x0400001C RID: 28
		public Color endValueColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400001D RID: 29
		public string endValueString = "";

		// Token: 0x0400001E RID: 30
		public Rect endValueRect = new Rect(0f, 0f, 0f, 0f);

		// Token: 0x0400001F RID: 31
		public Transform endValueTransform;

		// Token: 0x04000020 RID: 32
		public bool optionalBool0;

		// Token: 0x04000021 RID: 33
		public bool optionalBool1;

		// Token: 0x04000022 RID: 34
		public float optionalFloat0;

		// Token: 0x04000023 RID: 35
		public int optionalInt0;

		// Token: 0x04000024 RID: 36
		public RotateMode optionalRotationMode;

		// Token: 0x04000025 RID: 37
		public ScrambleMode optionalScrambleMode;

		// Token: 0x04000026 RID: 38
		public string optionalString;

		// Token: 0x04000027 RID: 39
		private bool _tweenAutoGenerationCalled;

		// Token: 0x04000028 RID: 40
		private int _playCount = -1;

		// Token: 0x0200000B RID: 11
		public enum AnimationType
		{
			// Token: 0x0400003A RID: 58
			None,
			// Token: 0x0400003B RID: 59
			Move,
			// Token: 0x0400003C RID: 60
			LocalMove,
			// Token: 0x0400003D RID: 61
			Rotate,
			// Token: 0x0400003E RID: 62
			LocalRotate,
			// Token: 0x0400003F RID: 63
			Scale,
			// Token: 0x04000040 RID: 64
			Color,
			// Token: 0x04000041 RID: 65
			Fade,
			// Token: 0x04000042 RID: 66
			Text,
			// Token: 0x04000043 RID: 67
			PunchPosition,
			// Token: 0x04000044 RID: 68
			PunchRotation,
			// Token: 0x04000045 RID: 69
			PunchScale,
			// Token: 0x04000046 RID: 70
			ShakePosition,
			// Token: 0x04000047 RID: 71
			ShakeRotation,
			// Token: 0x04000048 RID: 72
			ShakeScale,
			// Token: 0x04000049 RID: 73
			CameraAspect,
			// Token: 0x0400004A RID: 74
			CameraBackgroundColor,
			// Token: 0x0400004B RID: 75
			CameraFieldOfView,
			// Token: 0x0400004C RID: 76
			CameraOrthoSize,
			// Token: 0x0400004D RID: 77
			CameraPixelRect,
			// Token: 0x0400004E RID: 78
			CameraRect,
			// Token: 0x0400004F RID: 79
			UIWidthHeight
		}

		// Token: 0x0200000C RID: 12
		public enum TargetType
		{
			// Token: 0x04000051 RID: 81
			Unset,
			// Token: 0x04000052 RID: 82
			Camera,
			// Token: 0x04000053 RID: 83
			CanvasGroup,
			// Token: 0x04000054 RID: 84
			Image,
			// Token: 0x04000055 RID: 85
			Light,
			// Token: 0x04000056 RID: 86
			RectTransform,
			// Token: 0x04000057 RID: 87
			Renderer,
			// Token: 0x04000058 RID: 88
			SpriteRenderer,
			// Token: 0x04000059 RID: 89
			Rigidbody,
			// Token: 0x0400005A RID: 90
			Rigidbody2D,
			// Token: 0x0400005B RID: 91
			Text,
			// Token: 0x0400005C RID: 92
			Transform,
			// Token: 0x0400005D RID: 93
			tk2dBaseSprite,
			// Token: 0x0400005E RID: 94
			tk2dTextMesh,
			// Token: 0x0400005F RID: 95
			TextMeshPro,
			// Token: 0x04000060 RID: 96
			TextMeshProUGUI
		}
	}
}
