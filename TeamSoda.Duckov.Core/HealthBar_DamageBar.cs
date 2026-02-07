using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000202 RID: 514
public class HealthBar_DamageBar : MonoBehaviour
{
	// Token: 0x06000F47 RID: 3911 RVA: 0x0003D7E3 File Offset: 0x0003B9E3
	private void Awake()
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = (base.transform as RectTransform);
		}
		if (this.image == null)
		{
			this.image = base.GetComponent<Image>();
		}
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0003D820 File Offset: 0x0003BA20
	public UniTask Animate(float damageBarPostion, float damageBarWidth, Action onComplete)
	{
		HealthBar_DamageBar.<Animate>d__7 <Animate>d__;
		<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Animate>d__.<>4__this = this;
		<Animate>d__.damageBarPostion = damageBarPostion;
		<Animate>d__.damageBarWidth = damageBarWidth;
		<Animate>d__.onComplete = onComplete;
		<Animate>d__.<>1__state = -1;
		<Animate>d__.<>t__builder.Start<HealthBar_DamageBar.<Animate>d__7>(ref <Animate>d__);
		return <Animate>d__.<>t__builder.Task;
	}

	// Token: 0x04000CAD RID: 3245
	[SerializeField]
	internal RectTransform rectTransform;

	// Token: 0x04000CAE RID: 3246
	[SerializeField]
	internal Image image;

	// Token: 0x04000CAF RID: 3247
	[SerializeField]
	private float duration;

	// Token: 0x04000CB0 RID: 3248
	[SerializeField]
	private float targetSizeDelta = 4f;

	// Token: 0x04000CB1 RID: 3249
	[SerializeField]
	private AnimationCurve curve;

	// Token: 0x04000CB2 RID: 3250
	[SerializeField]
	private Gradient colorOverTime;
}
