using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003CD RID: 973
	public class ClosureView : View
	{
		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x0007A79B File Offset: 0x0007899B
		public static ClosureView Instance
		{
			get
			{
				return View.GetViewInstance<ClosureView>();
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x0600230B RID: 8971 RVA: 0x0007A7A2 File Offset: 0x000789A2
		private string EvacuatedTitleText
		{
			get
			{
				return LocalizationManager.ToPlainText(this.evacuatedTitleTextKey);
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x0007A7AF File Offset: 0x000789AF
		private string FailedTitleText
		{
			get
			{
				return LocalizationManager.ToPlainText(this.failedTitleTextKey);
			}
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0007A7BC File Offset: 0x000789BC
		protected override void Awake()
		{
			base.Awake();
			this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0007A7E0 File Offset: 0x000789E0
		private void OnContinueButtonClicked()
		{
			if (!this.canContinue)
			{
				return;
			}
			this.continueButtonClicked = true;
			this.contentFadeGroup.Hide();
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0007A7FD File Offset: 0x000789FD
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.contentFadeGroup.Show();
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0007A81B File Offset: 0x00078A1B
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0007A830 File Offset: 0x00078A30
		public static UniTask ShowAndReturnTask(float duration = 0.5f)
		{
			ClosureView.<ShowAndReturnTask>d__36 <ShowAndReturnTask>d__;
			<ShowAndReturnTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowAndReturnTask>d__.duration = duration;
			<ShowAndReturnTask>d__.<>1__state = -1;
			<ShowAndReturnTask>d__.<>t__builder.Start<ClosureView.<ShowAndReturnTask>d__36>(ref <ShowAndReturnTask>d__);
			return <ShowAndReturnTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x0007A874 File Offset: 0x00078A74
		public static UniTask ShowAndReturnTask(DamageInfo dmgInfo, float duration = 0.5f)
		{
			ClosureView.<ShowAndReturnTask>d__37 <ShowAndReturnTask>d__;
			<ShowAndReturnTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowAndReturnTask>d__.dmgInfo = dmgInfo;
			<ShowAndReturnTask>d__.duration = duration;
			<ShowAndReturnTask>d__.<>1__state = -1;
			<ShowAndReturnTask>d__.<>t__builder.Start<ClosureView.<ShowAndReturnTask>d__37>(ref <ShowAndReturnTask>d__);
			return <ShowAndReturnTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0007A8BF File Offset: 0x00078ABF
		private void SetupDamageInfo(DamageInfo dmgInfo)
		{
			this.damageSourceText.text = dmgInfo.GenerateDescription();
			this.damageInfoContainer.gameObject.SetActive(true);
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0007A8E4 File Offset: 0x00078AE4
		private UniTask ClosureTask()
		{
			ClosureView.<ClosureTask>d__39 <ClosureTask>d__;
			<ClosureTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ClosureTask>d__.<>4__this = this;
			<ClosureTask>d__.<>1__state = -1;
			<ClosureTask>d__.<>t__builder.Start<ClosureView.<ClosureTask>d__39>(ref <ClosureTask>d__);
			return <ClosureTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0007A928 File Offset: 0x00078B28
		private void SetupBeginning()
		{
			long cachedExp = EXPManager.CachedExp;
			long exp = EXPManager.EXP;
			this.Refresh(0f, cachedExp, exp);
			this.continueButton.gameObject.SetActive(false);
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0007A960 File Offset: 0x00078B60
		private void SetupTitle(bool dead)
		{
			if (dead)
			{
				this.titleText.color = this.failedTitleTextColor;
				this.titleText.text = this.FailedTitleText;
				return;
			}
			this.titleText.color = this.evacuatedTitleTextColor;
			this.titleText.text = this.EvacuatedTitleText;
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0007A9B8 File Offset: 0x00078BB8
		private UniTask AnimateExpBar(long fromExp, long toExp)
		{
			ClosureView.<AnimateExpBar>d__42 <AnimateExpBar>d__;
			<AnimateExpBar>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateExpBar>d__.<>4__this = this;
			<AnimateExpBar>d__.fromExp = fromExp;
			<AnimateExpBar>d__.toExp = toExp;
			<AnimateExpBar>d__.<>1__state = -1;
			<AnimateExpBar>d__.<>t__builder.Start<ClosureView.<AnimateExpBar>d__42>(ref <AnimateExpBar>d__);
			return <AnimateExpBar>d__.<>t__builder.Task;
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0007AA0C File Offset: 0x00078C0C
		private void SpitExpUpSfx(float expDelta)
		{
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime - this.lastTimeExpUpSfxPlayed < 0.05f)
			{
				return;
			}
			this.lastTimeExpUpSfxPlayed = unscaledTime;
			AudioManager.SetRTPC("ExpDelta", expDelta, null);
			AudioManager.Post(this.sfx_ExpUp);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0007AA50 File Offset: 0x00078C50
		private long Refresh(float t, long fromExp, long toExp)
		{
			long num = this.LongLerp(fromExp, toExp, t);
			this.SetExpDisplay(num, fromExp);
			this.SetLevelDisplay(this.cachedLevel);
			return num;
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0007AA7C File Offset: 0x00078C7C
		private long LongLerp(long from, long to, float t)
		{
			return (long)((float)(to - from) * t) + from;
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0007AA88 File Offset: 0x00078C88
		private void CacheLevelInfo(int level)
		{
			if (level == this.cachedLevel)
			{
				return;
			}
			this.cachedLevel = level;
			this.cachedLevelRange = EXPManager.Instance.GetLevelExpRange(level);
			this.cachedLevelLength = this.cachedLevelRange.Item2 - this.cachedLevelRange.Item1;
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0007AAD4 File Offset: 0x00078CD4
		private void SetExpDisplay(long currentExp, long oldExp)
		{
			int level = EXPManager.Instance.LevelFromExp(currentExp);
			this.CacheLevelInfo(level);
			float fillAmount = 0f;
			if (oldExp >= this.cachedLevelRange.Item1 && oldExp <= this.cachedLevelRange.Item2)
			{
				fillAmount = (float)(oldExp - this.cachedLevelRange.Item1) / (float)this.cachedLevelLength;
			}
			float fillAmount2 = (float)(currentExp - this.cachedLevelRange.Item1) / (float)this.cachedLevelLength;
			this.expBar_OldFill.fillAmount = fillAmount;
			this.expBar_CurrentFill.fillAmount = fillAmount2;
			string arg = (this.cachedLevelRange.Item2 >= long.MaxValue) ? "∞" : this.cachedLevelRange.Item2.ToString();
			this.expDisplay.text = string.Format(this.expFormat, currentExp, arg);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0007ABA7 File Offset: 0x00078DA7
		private void SetLevelDisplay(int level)
		{
			if (this.displayingLevel > 0 && level != this.displayingLevel)
			{
				this.LevelUpPunch();
			}
			this.displayingLevel = level;
			this.levelDisplay.text = string.Format(this.levelFormat, level);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0007ABE4 File Offset: 0x00078DE4
		private void LevelUpPunch()
		{
			PunchReceiver punchReceiver = this.levelDisplayPunchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			PunchReceiver punchReceiver2 = this.barPunchReceiver;
			if (punchReceiver2 != null)
			{
				punchReceiver2.Punch();
			}
			AudioManager.Post(this.sfx_LvUp);
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0007AC14 File Offset: 0x00078E14
		internal override void TryQuit()
		{
		}

		// Token: 0x040017B2 RID: 6066
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040017B3 RID: 6067
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x040017B4 RID: 6068
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x040017B5 RID: 6069
		[SerializeField]
		[LocalizationKey("Default")]
		private string evacuatedTitleTextKey = "UI_Closure_Escaped";

		// Token: 0x040017B6 RID: 6070
		[SerializeField]
		private Color evacuatedTitleTextColor = Color.white;

		// Token: 0x040017B7 RID: 6071
		[SerializeField]
		[LocalizationKey("Default")]
		private string failedTitleTextKey = "UI_Closure_Dead";

		// Token: 0x040017B8 RID: 6072
		[SerializeField]
		private Color failedTitleTextColor = Color.red;

		// Token: 0x040017B9 RID: 6073
		[SerializeField]
		private GameObject damageInfoContainer;

		// Token: 0x040017BA RID: 6074
		[SerializeField]
		private TextMeshProUGUI damageSourceText;

		// Token: 0x040017BB RID: 6075
		[SerializeField]
		private Image expBar_OldFill;

		// Token: 0x040017BC RID: 6076
		[SerializeField]
		private Image expBar_CurrentFill;

		// Token: 0x040017BD RID: 6077
		[SerializeField]
		private AnimationCurve expBarAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040017BE RID: 6078
		[SerializeField]
		private float expBarAnimationTime = 3f;

		// Token: 0x040017BF RID: 6079
		[SerializeField]
		private TextMeshProUGUI expDisplay;

		// Token: 0x040017C0 RID: 6080
		[SerializeField]
		private string expFormat = "{0}/<sub>{1}</sub>";

		// Token: 0x040017C1 RID: 6081
		[SerializeField]
		private TextMeshProUGUI levelDisplay;

		// Token: 0x040017C2 RID: 6082
		[SerializeField]
		private string levelFormat = "Lv.{0}";

		// Token: 0x040017C3 RID: 6083
		[SerializeField]
		private PunchReceiver levelDisplayPunchReceiver;

		// Token: 0x040017C4 RID: 6084
		[SerializeField]
		private PunchReceiver barPunchReceiver;

		// Token: 0x040017C5 RID: 6085
		[SerializeField]
		private Button continueButton;

		// Token: 0x040017C6 RID: 6086
		[SerializeField]
		private PunchReceiver continueButtonPunchReceiver;

		// Token: 0x040017C7 RID: 6087
		private string sfx_Pop = "UI/pop";

		// Token: 0x040017C8 RID: 6088
		private string sfx_ExpUp = "UI/exp_up";

		// Token: 0x040017C9 RID: 6089
		private string sfx_LvUp = "UI/level_up";

		// Token: 0x040017CA RID: 6090
		private bool continueButtonClicked;

		// Token: 0x040017CB RID: 6091
		private bool canContinue;

		// Token: 0x040017CC RID: 6092
		private float lastTimeExpUpSfxPlayed = float.MinValue;

		// Token: 0x040017CD RID: 6093
		private const float minIntervalForExpUpSfx = 0.05f;

		// Token: 0x040017CE RID: 6094
		private int cachedLevel = -1;

		// Token: 0x040017CF RID: 6095
		[TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		private ValueTuple<long, long> cachedLevelRange;

		// Token: 0x040017D0 RID: 6096
		private long cachedLevelLength;

		// Token: 0x040017D1 RID: 6097
		private int displayingLevel = -1;
	}
}
