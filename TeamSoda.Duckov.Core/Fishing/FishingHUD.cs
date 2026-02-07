using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
	// Token: 0x0200021F RID: 543
	public class FishingHUD : MonoBehaviour
	{
		// Token: 0x0600105A RID: 4186 RVA: 0x00040BAD File Offset: 0x0003EDAD
		private void Awake()
		{
			Action_Fishing.OnPlayerStartCatching += this.OnStartCatching;
			Action_Fishing.OnPlayerStopCatching += this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing += this.OnStopFishing;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00040BE2 File Offset: 0x0003EDE2
		private void OnDestroy()
		{
			Action_Fishing.OnPlayerStartCatching -= this.OnStartCatching;
			Action_Fishing.OnPlayerStopCatching -= this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing -= this.OnStopFishing;
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00040C17 File Offset: 0x0003EE17
		private void OnStopFishing(Action_Fishing fishing)
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00040C24 File Offset: 0x0003EE24
		private void OnStopCatching(Action_Fishing fishing, Item item, Action<bool> action)
		{
			this.StopCatchingTask(item, action).Forget();
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00040C33 File Offset: 0x0003EE33
		private void OnStartCatching(Action_Fishing fishing, float totalTime, Func<float> currentTimeGetter)
		{
			this.CatchingTask(fishing, totalTime, currentTimeGetter).Forget();
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00040C44 File Offset: 0x0003EE44
		private UniTask CatchingTask(Action_Fishing fishing, float totalTime, Func<float> currentTimeGetter)
		{
			FishingHUD.<CatchingTask>d__9 <CatchingTask>d__;
			<CatchingTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CatchingTask>d__.<>4__this = this;
			<CatchingTask>d__.fishing = fishing;
			<CatchingTask>d__.totalTime = totalTime;
			<CatchingTask>d__.currentTimeGetter = currentTimeGetter;
			<CatchingTask>d__.<>1__state = -1;
			<CatchingTask>d__.<>t__builder.Start<FishingHUD.<CatchingTask>d__9>(ref <CatchingTask>d__);
			return <CatchingTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x00040CA0 File Offset: 0x0003EEA0
		private void UpdateBar(float totalTime, float currentTime)
		{
			if (totalTime <= 0f)
			{
				return;
			}
			float fillAmount = 1f - currentTime / totalTime;
			this.countDownFill.fillAmount = fillAmount;
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00040CCC File Offset: 0x0003EECC
		private UniTask StopCatchingTask(Item item, Action<bool> confirmCallback)
		{
			FishingHUD.<StopCatchingTask>d__11 <StopCatchingTask>d__;
			<StopCatchingTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<StopCatchingTask>d__.<>4__this = this;
			<StopCatchingTask>d__.item = item;
			<StopCatchingTask>d__.<>1__state = -1;
			<StopCatchingTask>d__.<>t__builder.Start<FishingHUD.<StopCatchingTask>d__11>(ref <StopCatchingTask>d__);
			return <StopCatchingTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000D37 RID: 3383
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000D38 RID: 3384
		[SerializeField]
		private Image countDownFill;

		// Token: 0x04000D39 RID: 3385
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x04000D3A RID: 3386
		[SerializeField]
		private FadeGroup failIndicator;
	}
}
