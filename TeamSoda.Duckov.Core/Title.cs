using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

// Token: 0x02000170 RID: 368
public class Title : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000B54 RID: 2900 RVA: 0x00030F67 File Offset: 0x0002F167
	private void Start()
	{
		this.StartTask().Forget();
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00030F74 File Offset: 0x0002F174
	private UniTask StartTask()
	{
		Title.<StartTask>d__5 <StartTask>d__;
		<StartTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<StartTask>d__.<>4__this = this;
		<StartTask>d__.<>1__state = -1;
		<StartTask>d__.<>t__builder.Start<Title.<StartTask>d__5>(ref <StartTask>d__);
		return <StartTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00030FB8 File Offset: 0x0002F1B8
	private UniTask ContinueTask()
	{
		Title.<ContinueTask>d__6 <ContinueTask>d__;
		<ContinueTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<ContinueTask>d__.<>4__this = this;
		<ContinueTask>d__.<>1__state = -1;
		<ContinueTask>d__.<>t__builder.Start<Title.<ContinueTask>d__6>(ref <ContinueTask>d__);
		return <ContinueTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00030FFC File Offset: 0x0002F1FC
	private UniTask WaitForTimeline(PlayableDirector timeline)
	{
		Title.<WaitForTimeline>d__7 <WaitForTimeline>d__;
		<WaitForTimeline>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<WaitForTimeline>d__.timeline = timeline;
		<WaitForTimeline>d__.<>1__state = -1;
		<WaitForTimeline>d__.<>t__builder.Start<Title.<WaitForTimeline>d__7>(ref <WaitForTimeline>d__);
		return <WaitForTimeline>d__.<>t__builder.Task;
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0003103F File Offset: 0x0002F23F
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.fadeGroup.IsShown)
		{
			this.ContinueTask().Forget();
		}
	}

	// Token: 0x040009D5 RID: 2517
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x040009D6 RID: 2518
	[SerializeField]
	private PlayableDirector timelineToTitle;

	// Token: 0x040009D7 RID: 2519
	[SerializeField]
	private PlayableDirector timelineToMainMenu;

	// Token: 0x040009D8 RID: 2520
	private string sfx_PressStart = "UI/game_start";
}
