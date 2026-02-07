using System;
using DG.Tweening;
using Duckov;
using Duckov.Achievements;
using Duckov.Modding;
using Duckov.NoteIndexs;
using Duckov.Rules;
using Duckov.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

// Token: 0x0200009E RID: 158
public class GameManager : MonoBehaviour
{
	// Token: 0x1700011A RID: 282
	// (get) Token: 0x0600055A RID: 1370 RVA: 0x000184B0 File Offset: 0x000166B0
	public static GameManager Instance
	{
		get
		{
			if (!Application.isPlaying)
			{
				return null;
			}
			if (GameManager._instance == null)
			{
				GameManager._instance = Object.FindObjectOfType<GameManager>();
				if (GameManager._instance)
				{
					Object.DontDestroyOnLoad(GameManager._instance.gameObject);
				}
			}
			if (GameManager._instance == null)
			{
				GameObject gameObject = Resources.Load<GameObject>("GameManager");
				if (gameObject == null)
				{
					Debug.LogError("Resources中找不到GameManager的Prefab");
				}
				GameManager component = Object.Instantiate<GameObject>(gameObject).GetComponent<GameManager>();
				if (component == null)
				{
					Debug.LogError("GameManager的prefab上没有GameManager组件");
					return null;
				}
				GameManager._instance = component;
				if (GameManager._instance)
				{
					Object.DontDestroyOnLoad(GameManager._instance.gameObject);
				}
			}
			return GameManager._instance;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x0600055B RID: 1371 RVA: 0x00018568 File Offset: 0x00016768
	public static bool Paused
	{
		get
		{
			return !(GameManager.Instance == null) && GameManager.Instance.pauseMenu.Shown;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001858D File Offset: 0x0001678D
	public static AudioManager AudioManager
	{
		get
		{
			return GameManager.Instance.audioManager;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x0600055D RID: 1373 RVA: 0x00018599 File Offset: 0x00016799
	public static UIInputManager UiInputManager
	{
		get
		{
			return GameManager.Instance.uiInputManager;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x0600055E RID: 1374 RVA: 0x000185A5 File Offset: 0x000167A5
	public static PauseMenu PauseMenu
	{
		get
		{
			return GameManager.Instance.pauseMenu;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600055F RID: 1375 RVA: 0x000185B1 File Offset: 0x000167B1
	public static GameRulesManager DifficultyManager
	{
		get
		{
			return GameManager.Instance.difficultyManager;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000560 RID: 1376 RVA: 0x000185BD File Offset: 0x000167BD
	public static SceneLoader SceneLoader
	{
		get
		{
			return GameManager.Instance.sceneLoader;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000561 RID: 1377 RVA: 0x000185C9 File Offset: 0x000167C9
	public static BlackScreen BlackScreen
	{
		get
		{
			return GameManager.Instance.blackScreen;
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000562 RID: 1378 RVA: 0x000185D5 File Offset: 0x000167D5
	public static EventSystem EventSystem
	{
		get
		{
			return GameManager.Instance.eventSystem;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000563 RID: 1379 RVA: 0x000185E1 File Offset: 0x000167E1
	public static NightVisionVisual NightVision
	{
		get
		{
			return GameManager.Instance.nightVision;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000564 RID: 1380 RVA: 0x000185ED File Offset: 0x000167ED
	public static bool BloodFxOn
	{
		get
		{
			return GameMetaData.BloodFxOn;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000565 RID: 1381 RVA: 0x000185F4 File Offset: 0x000167F4
	public static PlayerInput MainPlayerInput
	{
		get
		{
			return GameManager.Instance.mainPlayerInput;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000566 RID: 1382 RVA: 0x00018600 File Offset: 0x00016800
	public static ModManager ModManager
	{
		get
		{
			return GameManager.Instance.modManager;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001860C File Offset: 0x0001680C
	public static NoteIndex NoteIndex
	{
		get
		{
			return GameManager.Instance.noteIndex;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000568 RID: 1384 RVA: 0x00018618 File Offset: 0x00016818
	public static AchievementManager AchievementManager
	{
		get
		{
			return GameManager.Instance.achievementManager;
		}
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00018624 File Offset: 0x00016824
	private void Awake()
	{
		if (GameManager._instance == null)
		{
			GameManager._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (GameManager._instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		DOTween.defaultTimeScaleIndependent = true;
		DebugManager.instance.enableRuntimeUI = false;
		DebugManager.instance.displayRuntimeUI = false;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00018685 File Offset: 0x00016885
	private void Update()
	{
		bool isEditor = Application.isEditor;
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0001868D File Offset: 0x0001688D
	public static void TimeTravelDetected()
	{
		Debug.Log("检测到穿越者");
	}

	// Token: 0x040004E0 RID: 1248
	private static GameManager _instance;

	// Token: 0x040004E1 RID: 1249
	[SerializeField]
	private AudioManager audioManager;

	// Token: 0x040004E2 RID: 1250
	[SerializeField]
	private UIInputManager uiInputManager;

	// Token: 0x040004E3 RID: 1251
	[SerializeField]
	private GameRulesManager difficultyManager;

	// Token: 0x040004E4 RID: 1252
	[SerializeField]
	private PauseMenu pauseMenu;

	// Token: 0x040004E5 RID: 1253
	[SerializeField]
	private SceneLoader sceneLoader;

	// Token: 0x040004E6 RID: 1254
	[SerializeField]
	private BlackScreen blackScreen;

	// Token: 0x040004E7 RID: 1255
	[SerializeField]
	private EventSystem eventSystem;

	// Token: 0x040004E8 RID: 1256
	[SerializeField]
	private PlayerInput mainPlayerInput;

	// Token: 0x040004E9 RID: 1257
	[SerializeField]
	private NightVisionVisual nightVision;

	// Token: 0x040004EA RID: 1258
	[SerializeField]
	private ModManager modManager;

	// Token: 0x040004EB RID: 1259
	[SerializeField]
	private NoteIndex noteIndex;

	// Token: 0x040004EC RID: 1260
	[SerializeField]
	private AchievementManager achievementManager;

	// Token: 0x040004ED RID: 1261
	public static bool newBoot;
}
