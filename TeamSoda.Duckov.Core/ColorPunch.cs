using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F7 RID: 503
public class ColorPunch : MonoBehaviour
{
	// Token: 0x06000EFE RID: 3838 RVA: 0x0003CCFB File Offset: 0x0003AEFB
	private void Awake()
	{
		if (this.graphic == null)
		{
			this.graphic = base.GetComponent<Graphic>();
		}
		this.resetColor = this.graphic.color;
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0003CD28 File Offset: 0x0003AF28
	public void Punch()
	{
		this.DoTask().Forget();
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x0003CD35 File Offset: 0x0003AF35
	private int NewToken()
	{
		this.activeToken = Random.Range(1, int.MaxValue);
		return this.activeToken;
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0003CD50 File Offset: 0x0003AF50
	private UniTask DoTask()
	{
		ColorPunch.<DoTask>d__9 <DoTask>d__;
		<DoTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DoTask>d__.<>4__this = this;
		<DoTask>d__.<>1__state = -1;
		<DoTask>d__.<>t__builder.Start<ColorPunch.<DoTask>d__9>(ref <DoTask>d__);
		return <DoTask>d__.<>t__builder.Task;
	}

	// Token: 0x04000C7C RID: 3196
	[SerializeField]
	private Graphic graphic;

	// Token: 0x04000C7D RID: 3197
	[SerializeField]
	private float duration;

	// Token: 0x04000C7E RID: 3198
	[SerializeField]
	private Gradient gradient;

	// Token: 0x04000C7F RID: 3199
	[SerializeField]
	private Color tint = Color.white;

	// Token: 0x04000C80 RID: 3200
	private Color resetColor;

	// Token: 0x04000C81 RID: 3201
	private int activeToken;
}
