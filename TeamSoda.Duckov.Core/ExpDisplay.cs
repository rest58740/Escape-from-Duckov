using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000201 RID: 513
public class ExpDisplay : MonoBehaviour
{
	// Token: 0x06000F3B RID: 3899 RVA: 0x0003D568 File Offset: 0x0003B768
	private void Refresh()
	{
		EXPManager instance = EXPManager.Instance;
		if (instance == null)
		{
			return;
		}
		int num = instance.LevelFromExp(this.displayExp);
		if (this.displayingLevel != num)
		{
			this.displayingLevel = num;
			this.OnDisplayingLevelChanged();
		}
		ValueTuple<long, long> levelExpRange = this.GetLevelExpRange(num);
		long num2 = levelExpRange.Item2 - levelExpRange.Item1;
		this.txtLevel.text = num.ToString();
		this.txtCurrentExp.text = this.displayExp.ToString();
		string text;
		if (levelExpRange.Item2 == 9223372036854775807L)
		{
			text = "∞";
		}
		else
		{
			text = levelExpRange.Item2.ToString();
		}
		this.txtMaxExp.text = text;
		float fillAmount = (float)((double)(this.displayExp - levelExpRange.Item1) / (double)num2);
		this.expBarFill.fillAmount = fillAmount;
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x0003D63C File Offset: 0x0003B83C
	private void OnDisplayingLevelChanged()
	{
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x0003D640 File Offset: 0x0003B840
	[return: TupleElementNames(new string[]
	{
		"from",
		"to"
	})]
	private ValueTuple<long, long> GetLevelExpRange(int level)
	{
		ValueTuple<long, long> result;
		if (this.cachedLevelExpRange.TryGetValue(level, out result))
		{
			return result;
		}
		EXPManager instance = EXPManager.Instance;
		if (instance == null)
		{
			return new ValueTuple<long, long>(0L, 0L);
		}
		ValueTuple<long, long> levelExpRange = instance.GetLevelExpRange(level);
		this.cachedLevelExpRange[level] = levelExpRange;
		return levelExpRange;
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x0003D68E File Offset: 0x0003B88E
	private void SnapToCurrent()
	{
		this.displayExp = EXPManager.EXP;
		this.Refresh();
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x0003D6A4 File Offset: 0x0003B8A4
	private UniTask Animate(long targetExp, float duration, AnimationCurve curve)
	{
		ExpDisplay.<Animate>d__15 <Animate>d__;
		<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Animate>d__.<>4__this = this;
		<Animate>d__.targetExp = targetExp;
		<Animate>d__.duration = duration;
		<Animate>d__.curve = curve;
		<Animate>d__.<>1__state = -1;
		<Animate>d__.<>t__builder.Start<ExpDisplay.<Animate>d__15>(ref <Animate>d__);
		return <Animate>d__.<>t__builder.Task;
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x0003D700 File Offset: 0x0003B900
	private long LongLerp(long a, long b, float t)
	{
		long num = b - a;
		return a + (long)(t * (float)num);
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x0003D718 File Offset: 0x0003B918
	private void OnEnable()
	{
		if (this.snapToCurrentOnEnable)
		{
			this.SnapToCurrent();
		}
		this.RegisterEvents();
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x0003D72E File Offset: 0x0003B92E
	private void OnDisable()
	{
		this.UnregisterEvents();
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x0003D736 File Offset: 0x0003B936
	private void RegisterEvents()
	{
		EXPManager.onExpChanged = (Action<long>)Delegate.Combine(EXPManager.onExpChanged, new Action<long>(this.OnExpChanged));
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x0003D758 File Offset: 0x0003B958
	private void UnregisterEvents()
	{
		EXPManager.onExpChanged = (Action<long>)Delegate.Remove(EXPManager.onExpChanged, new Action<long>(this.OnExpChanged));
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x0003D77A File Offset: 0x0003B97A
	private void OnExpChanged(long exp)
	{
		this.Animate(exp, this.animationDuration, this.animationCurve).Forget();
	}

	// Token: 0x04000CA2 RID: 3234
	[SerializeField]
	private TextMeshProUGUI txtLevel;

	// Token: 0x04000CA3 RID: 3235
	[SerializeField]
	private TextMeshProUGUI txtCurrentExp;

	// Token: 0x04000CA4 RID: 3236
	[SerializeField]
	private TextMeshProUGUI txtMaxExp;

	// Token: 0x04000CA5 RID: 3237
	[SerializeField]
	private Image expBarFill;

	// Token: 0x04000CA6 RID: 3238
	[SerializeField]
	private bool snapToCurrentOnEnable;

	// Token: 0x04000CA7 RID: 3239
	[SerializeField]
	private float animationDuration = 0.1f;

	// Token: 0x04000CA8 RID: 3240
	[SerializeField]
	private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000CA9 RID: 3241
	[SerializeField]
	private long displayExp;

	// Token: 0x04000CAA RID: 3242
	private int displayingLevel = -1;

	// Token: 0x04000CAB RID: 3243
	[TupleElementNames(new string[]
	{
		"from",
		"to"
	})]
	private Dictionary<int, ValueTuple<long, long>> cachedLevelExpRange = new Dictionary<int, ValueTuple<long, long>>();

	// Token: 0x04000CAC RID: 3244
	private int currentToken;
}
