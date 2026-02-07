using System;
using Cysharp.Threading.Tasks;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000162 RID: 354
public class ReselectButton : MonoBehaviour
{
	// Token: 0x06000B0D RID: 2829 RVA: 0x00030707 File Offset: 0x0002E907
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x00030725 File Offset: 0x0002E925
	private void OnEnable()
	{
		this.setActiveGroup.SetActive(LevelManager.Instance && LevelManager.Instance.IsBaseLevel);
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0003074B File Offset: 0x0002E94B
	private void OnDisable()
	{
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x00030750 File Offset: 0x0002E950
	private void OnButtonClicked()
	{
		SceneLoader.Instance.LoadScene(this.prepareSceneID, null, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
		if (PauseMenu.Instance && PauseMenu.Instance.Shown)
		{
			PauseMenu.Hide();
		}
	}

	// Token: 0x040009B6 RID: 2486
	[SerializeField]
	private GameObject setActiveGroup;

	// Token: 0x040009B7 RID: 2487
	[SerializeField]
	private Button button;

	// Token: 0x040009B8 RID: 2488
	[SerializeField]
	[SceneID]
	private string prepareSceneID = "Prepare";
}
