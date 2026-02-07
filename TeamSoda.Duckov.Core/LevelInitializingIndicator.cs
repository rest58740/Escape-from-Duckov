using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class LevelInitializingIndicator : MonoBehaviour
{
	// Token: 0x06000B2E RID: 2862 RVA: 0x00030B18 File Offset: 0x0002ED18
	private void Awake()
	{
		SceneLoader.onBeforeSetSceneActive += this.SceneLoader_onBeforeSetSceneActive;
		SceneLoader.onAfterSceneInitialize += this.SceneLoader_onAfterSceneInitialize;
		LevelManager.OnLevelInitializingCommentChanged += this.OnCommentChanged;
		SceneLoader.OnSetLoadingComment += this.OnSetLoadingComment;
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x00030B74 File Offset: 0x0002ED74
	private void OnSetLoadingComment(string comment)
	{
		this.levelInitializationCommentText.text = SceneLoader.LoadingComment;
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x00030B86 File Offset: 0x0002ED86
	private void OnCommentChanged(string comment)
	{
		this.levelInitializationCommentText.text = SceneLoader.LoadingComment;
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x00030B98 File Offset: 0x0002ED98
	private void OnDestroy()
	{
		SceneLoader.onBeforeSetSceneActive -= this.SceneLoader_onBeforeSetSceneActive;
		SceneLoader.onAfterSceneInitialize -= this.SceneLoader_onAfterSceneInitialize;
		LevelManager.OnLevelInitializingCommentChanged -= this.OnCommentChanged;
		SceneLoader.OnSetLoadingComment -= this.OnSetLoadingComment;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x00030BE9 File Offset: 0x0002EDE9
	private void SceneLoader_onBeforeSetSceneActive(SceneLoadingContext obj)
	{
		this.fadeGroup.Show();
		this.levelInitializationCommentText.text = LevelManager.LevelInitializingComment;
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00030C06 File Offset: 0x0002EE06
	private void SceneLoader_onAfterSceneInitialize(SceneLoadingContext obj)
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x040009C1 RID: 2497
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x040009C2 RID: 2498
	[SerializeField]
	private TextMeshProUGUI levelInitializationCommentText;
}
