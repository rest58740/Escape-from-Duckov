using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C0 RID: 960
	public class HealthBar : MonoBehaviour, IPoolable
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x00078307 File Offset: 0x00076507
		// (set) Token: 0x0600225B RID: 8795 RVA: 0x0007830F File Offset: 0x0007650F
		public Health target { get; private set; }

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x00078318 File Offset: 0x00076518
		private PrefabPool<HealthBar_DamageBar> DamageBarPool
		{
			get
			{
				if (this._damageBarPool == null)
				{
					this._damageBarPool = new PrefabPool<HealthBar_DamageBar>(this.damageBarTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._damageBarPool;
			}
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00078351 File Offset: 0x00076551
		public void NotifyPooled()
		{
			this.pooled = true;
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x0007835A File Offset: 0x0007655A
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
			this.pooled = false;
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x00078370 File Offset: 0x00076570
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x00078383 File Offset: 0x00076583
		private void OnDestroy()
		{
			this.UnregisterEvents();
			Image image = this.followFill;
			if (image != null)
			{
				ShortcutExtensions.DOKill(image, false);
			}
			Image image2 = this.hurtBlink;
			if (image2 == null)
			{
				return;
			}
			ShortcutExtensions.DOKill(image2, false);
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000783B0 File Offset: 0x000765B0
		private void LateUpdate()
		{
			if (this.target == null || !this.target.isActiveAndEnabled || this.target.Hidden)
			{
				this.Release();
				return;
			}
			this.UpdatePosition();
			bool flag = this.CheckInFrame();
			if (flag && !this.fadeGroup.IsShown)
			{
				this.fadeGroup.SkipShow();
				return;
			}
			if (!flag && this.fadeGroup.IsShown)
			{
				this.fadeGroup.SkipHide();
			}
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x00078430 File Offset: 0x00076630
		private bool CheckInFrame()
		{
			if (this.target == null)
			{
				return false;
			}
			Camera main = Camera.main;
			return Vector3.Dot(this.target.transform.position - main.transform.position, main.transform.forward) > 0f;
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x0007848D File Offset: 0x0007668D
		private void UpdateFrame()
		{
			if (this.CheckInFrame())
			{
				this.lastTimeInFrame = Time.unscaledTime;
			}
			if (Time.unscaledTime - this.lastTimeInFrame > this.releaseAfterOutOfFrame)
			{
				this.Release();
			}
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000784BC File Offset: 0x000766BC
		private void UpdatePosition()
		{
			Vector3 vector = this.target.transform.position + this.displayOffset;
			Vector3 position = Camera.main.WorldToScreenPoint(vector);
			position.y += this.screenYOffset * (float)Screen.height;
			base.transform.position = position;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x00078518 File Offset: 0x00076718
		public void Setup(Health target, DamageInfo? damage = null, Action releaseAction = null)
		{
			this.releaseAction = releaseAction;
			this.UnregisterEvents();
			if (target == null)
			{
				this.Release();
				return;
			}
			if (target.IsDead)
			{
				this.Release();
				return;
			}
			this.background.SetActive(true);
			this.deathIndicator.SetActive(false);
			this.fill.gameObject.SetActive(true);
			this.followFill.gameObject.SetActive(true);
			this.target = target;
			this.RefreshOffset();
			this.RegisterEvents();
			this.Refresh();
			this.lastTimeInFrame = Time.unscaledTime;
			this.damageBarTemplate.gameObject.SetActive(false);
			if (damage != null)
			{
				this.OnTargetHurt(damage.Value);
			}
			this.UpdatePosition();
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000785DC File Offset: 0x000767DC
		public void RefreshOffset()
		{
			if (!this.target)
			{
				return;
			}
			this.displayOffset = Vector3.up * 1.5f;
			CharacterMainControl characterMainControl = this.target.TryGetCharacter();
			if (characterMainControl && characterMainControl.characterModel)
			{
				Transform helmatSocket = characterMainControl.characterModel.HelmatSocket;
				if (helmatSocket)
				{
					this.displayOffset = Vector3.up * (Vector3.Distance(characterMainControl.transform.position, helmatSocket.position) + 0.5f);
				}
			}
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x00078670 File Offset: 0x00076870
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.RefreshCharacterIcon();
			this.target.OnMaxHealthChange.AddListener(new UnityAction<Health>(this.OnTargetMaxHealthChange));
			this.target.OnHealthChange.AddListener(new UnityAction<Health>(this.OnTargetHealthChange));
			this.target.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnTargetHurt));
			this.target.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnTargetDead));
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00078704 File Offset: 0x00076904
		private void RefreshCharacterIcon()
		{
			if (!this.target)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			CharacterMainControl characterMainControl = this.target.TryGetCharacter();
			if (!characterMainControl)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
			if (!characterPreset)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			Sprite characterIcon = characterPreset.GetCharacterIcon();
			if (!characterIcon)
			{
				this.levelIcon.gameObject.SetActive(false);
			}
			else
			{
				this.levelIcon.sprite = characterIcon;
				this.levelIcon.gameObject.SetActive(true);
			}
			if (!characterPreset.showName)
			{
				this.nameText.gameObject.SetActive(false);
				return;
			}
			this.nameText.text = characterPreset.DisplayName;
			this.nameText.gameObject.SetActive(true);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x00078828 File Offset: 0x00076A28
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnMaxHealthChange.RemoveListener(new UnityAction<Health>(this.OnTargetMaxHealthChange));
			this.target.OnHealthChange.RemoveListener(new UnityAction<Health>(this.OnTargetHealthChange));
			this.target.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTargetHurt));
			this.target.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTargetDead));
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000788B4 File Offset: 0x00076AB4
		private void OnTargetMaxHealthChange(Health obj)
		{
			this.Refresh();
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000788BC File Offset: 0x00076ABC
		private void OnTargetHealthChange(Health obj)
		{
			this.Refresh();
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000788C4 File Offset: 0x00076AC4
		private void OnTargetHurt(DamageInfo damage)
		{
			Color blinkEndColor = this.blinkColor;
			blinkEndColor.a = 0f;
			if (this.hurtBlink != null)
			{
				TweenSettingsExtensions.OnKill<TweenerCore<Color, Color, ColorOptions>>(TweenSettingsExtensions.From<TweenerCore<Color, Color, ColorOptions>>(DOTweenModuleUI.DOColor(this.hurtBlink, this.blinkColor, this.blinkDuration)), delegate()
				{
					if (this.hurtBlink != null)
					{
						this.hurtBlink.color = blinkEndColor;
					}
				});
			}
			UnityEvent unityEvent = this.onHurt;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.ShowDamageBar(damage.finalDamage);
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00078954 File Offset: 0x00076B54
		private void OnTargetDead(DamageInfo damage)
		{
			this.UnregisterEvents();
			UnityEvent unityEvent = this.onDead;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			if (!damage.toDamageReceiver || !damage.toDamageReceiver.health)
			{
				return;
			}
			this.DeathTask(damage.toDamageReceiver.health).Forget();
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000789B0 File Offset: 0x00076BB0
		internal void Release()
		{
			if (!this.pooled)
			{
				return;
			}
			if (this.target != null && this.target.IsMainCharacterHealth && !this.target.IsDead && this.target.gameObject.activeInHierarchy)
			{
				return;
			}
			this.UnregisterEvents();
			this.target != null;
			this.target = null;
			Action action = this.releaseAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x00078A2C File Offset: 0x00076C2C
		private void Refresh()
		{
			float currentHealth = this.target.CurrentHealth;
			float maxHealth = this.target.MaxHealth;
			float num = 0f;
			if (maxHealth > 0f)
			{
				num = currentHealth / maxHealth;
			}
			this.fill.fillAmount = num;
			this.fill.color = this.colorOverAmount.Evaluate(num);
			if (this.followFill != null)
			{
				ShortcutExtensions.DOKill(this.followFill, false);
				DOTweenModuleUI.DOFillAmount(this.followFill, num, this.followFillDuration);
			}
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00078AB8 File Offset: 0x00076CB8
		private void ShowDamageBar(float damageAmount)
		{
			float num = Mathf.Clamp01(damageAmount / this.target.MaxHealth);
			float num2 = Mathf.Clamp01(this.target.CurrentHealth / this.target.MaxHealth);
			float width = this.fill.rectTransform.rect.width;
			float damageBarWidth = width * num;
			float damageBarPostion = width * num2;
			HealthBar_DamageBar damageBar = this.DamageBarPool.Get(null);
			damageBar.Animate(damageBarPostion, damageBarWidth, delegate
			{
				this.DamageBarPool.Release(damageBar);
			}).Forget();
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x00078B58 File Offset: 0x00076D58
		private UniTask DeathTask(Health health)
		{
			HealthBar.<DeathTask>d__52 <DeathTask>d__;
			<DeathTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DeathTask>d__.<>4__this = this;
			<DeathTask>d__.health = health;
			<DeathTask>d__.<>1__state = -1;
			<DeathTask>d__.<>t__builder.Start<HealthBar.<DeathTask>d__52>(ref <DeathTask>d__);
			return <DeathTask>d__.<>t__builder.Task;
		}

		// Token: 0x0400174D RID: 5965
		private RectTransform rectTransform;

		// Token: 0x0400174E RID: 5966
		[SerializeField]
		private GameObject background;

		// Token: 0x0400174F RID: 5967
		[SerializeField]
		private Image fill;

		// Token: 0x04001750 RID: 5968
		[SerializeField]
		private Image followFill;

		// Token: 0x04001751 RID: 5969
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001752 RID: 5970
		[SerializeField]
		private GameObject deathIndicator;

		// Token: 0x04001753 RID: 5971
		[SerializeField]
		private PunchReceiver deathIndicatorPunchReceiver;

		// Token: 0x04001754 RID: 5972
		[SerializeField]
		private Image hurtBlink;

		// Token: 0x04001755 RID: 5973
		[SerializeField]
		private HealthBar_DamageBar damageBarTemplate;

		// Token: 0x04001756 RID: 5974
		[SerializeField]
		private Gradient colorOverAmount = new Gradient();

		// Token: 0x04001757 RID: 5975
		[SerializeField]
		private float followFillDuration = 0.5f;

		// Token: 0x04001758 RID: 5976
		[SerializeField]
		private float blinkDuration = 0.1f;

		// Token: 0x04001759 RID: 5977
		[SerializeField]
		private Color blinkColor = Color.white;

		// Token: 0x0400175A RID: 5978
		private Vector3 displayOffset = Vector3.zero;

		// Token: 0x0400175B RID: 5979
		[SerializeField]
		private float releaseAfterOutOfFrame = 1f;

		// Token: 0x0400175C RID: 5980
		[SerializeField]
		private float disappearDelay = 0.2f;

		// Token: 0x0400175D RID: 5981
		[SerializeField]
		private Image levelIcon;

		// Token: 0x0400175E RID: 5982
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x0400175F RID: 5983
		[SerializeField]
		private UnityEvent onHurt;

		// Token: 0x04001760 RID: 5984
		[SerializeField]
		private UnityEvent onDead;

		// Token: 0x04001762 RID: 5986
		private Action releaseAction;

		// Token: 0x04001763 RID: 5987
		private float lastTimeInFrame = float.MinValue;

		// Token: 0x04001764 RID: 5988
		private float screenYOffset = 0.02f;

		// Token: 0x04001765 RID: 5989
		private PrefabPool<HealthBar_DamageBar> _damageBarPool;

		// Token: 0x04001766 RID: 5990
		private bool pooled;

		// Token: 0x04001767 RID: 5991
		private Vector3[] cornersBuffer = new Vector3[4];
	}
}
