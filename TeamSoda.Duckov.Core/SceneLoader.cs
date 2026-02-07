using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Scenes;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Token: 0x02000131 RID: 305
public class SceneLoader : MonoBehaviour
{
	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0002BDC9 File Offset: 0x00029FC9
	public static SceneLoader Instance
	{
		get
		{
			return GameManager.SceneLoader;
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0002BDD0 File Offset: 0x00029FD0
	// (set) Token: 0x06000A17 RID: 2583 RVA: 0x0002BDD7 File Offset: 0x00029FD7
	public static bool IsSceneLoading { get; private set; }

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x06000A18 RID: 2584 RVA: 0x0002BDE0 File Offset: 0x00029FE0
	// (remove) Token: 0x06000A19 RID: 2585 RVA: 0x0002BE14 File Offset: 0x0002A014
	public static event Action<SceneLoadingContext> onStartedLoadingScene;

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06000A1A RID: 2586 RVA: 0x0002BE48 File Offset: 0x0002A048
	// (remove) Token: 0x06000A1B RID: 2587 RVA: 0x0002BE7C File Offset: 0x0002A07C
	public static event Action<SceneLoadingContext> onFinishedLoadingScene;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06000A1C RID: 2588 RVA: 0x0002BEB0 File Offset: 0x0002A0B0
	// (remove) Token: 0x06000A1D RID: 2589 RVA: 0x0002BEE4 File Offset: 0x0002A0E4
	public static event Action<SceneLoadingContext> onBeforeSetSceneActive;

	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06000A1E RID: 2590 RVA: 0x0002BF18 File Offset: 0x0002A118
	// (remove) Token: 0x06000A1F RID: 2591 RVA: 0x0002BF4C File Offset: 0x0002A14C
	public static event Action<SceneLoadingContext> onAfterSceneInitialize;

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0002BF7F File Offset: 0x0002A17F
	// (set) Token: 0x06000A21 RID: 2593 RVA: 0x0002BFA7 File Offset: 0x0002A1A7
	public static string LoadingComment
	{
		get
		{
			if (LevelManager.LevelInitializing)
			{
				return LevelManager.LevelInitializingComment;
			}
			if (SceneLoader.Instance != null)
			{
				return SceneLoader.Instance._loadingComment;
			}
			return null;
		}
		set
		{
			if (SceneLoader.Instance == null)
			{
				return;
			}
			SceneLoader.Instance._loadingComment = value;
			Action<string> onSetLoadingComment = SceneLoader.OnSetLoadingComment;
			if (onSetLoadingComment == null)
			{
				return;
			}
			onSetLoadingComment(value);
		}
	}

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06000A22 RID: 2594 RVA: 0x0002BFD4 File Offset: 0x0002A1D4
	// (remove) Token: 0x06000A23 RID: 2595 RVA: 0x0002C008 File Offset: 0x0002A208
	public static event Action<string> OnSetLoadingComment;

	// Token: 0x06000A24 RID: 2596 RVA: 0x0002C03C File Offset: 0x0002A23C
	private void Awake()
	{
		if (SceneLoader.Instance != this)
		{
			Debug.LogError(base.gameObject.scene.name + " 场景中出现了应当删除的Scene Loader");
			Object.Destroy(base.gameObject);
			return;
		}
		this.pointerClickEventRecevier.onPointerClick.AddListener(new UnityAction<PointerEventData>(this.NotifyPointerClick));
		this.pointerClickEventRecevier.gameObject.SetActive(false);
		this.content.Hide();
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0002C0BC File Offset: 0x0002A2BC
	public UniTask LoadScene(string sceneID, MultiSceneLocation location, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__39 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneID = sceneID;
		<LoadScene>d__.location = location;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__39>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0002C144 File Offset: 0x0002A344
	public UniTask LoadScene(string sceneID, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool useLocation = false, MultiSceneLocation location = default(MultiSceneLocation), bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__40 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneID = sceneID;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.useLocation = useLocation;
		<LoadScene>d__.location = location;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__40>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0002C1D5 File Offset: 0x0002A3D5
	// (set) Token: 0x06000A28 RID: 2600 RVA: 0x0002C1DC File Offset: 0x0002A3DC
	public static bool HideTips { get; private set; }

	// Token: 0x06000A29 RID: 2601 RVA: 0x0002C1E4 File Offset: 0x0002A3E4
	public UniTask LoadScene(SceneReference sceneReference, SceneReference overrideCurtainScene = null, bool clickToConinue = false, bool notifyEvacuation = false, bool doCircleFade = true, bool useLocation = false, MultiSceneLocation location = default(MultiSceneLocation), bool saveToFile = true, bool hideTips = false)
	{
		SceneLoader.<LoadScene>d__45 <LoadScene>d__;
		<LoadScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadScene>d__.<>4__this = this;
		<LoadScene>d__.sceneReference = sceneReference;
		<LoadScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadScene>d__.clickToConinue = clickToConinue;
		<LoadScene>d__.notifyEvacuation = notifyEvacuation;
		<LoadScene>d__.doCircleFade = doCircleFade;
		<LoadScene>d__.useLocation = useLocation;
		<LoadScene>d__.location = location;
		<LoadScene>d__.saveToFile = saveToFile;
		<LoadScene>d__.hideTips = hideTips;
		<LoadScene>d__.<>1__state = -1;
		<LoadScene>d__.<>t__builder.Start<SceneLoader.<LoadScene>d__45>(ref <LoadScene>d__);
		return <LoadScene>d__.<>t__builder.Task;
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0002C278 File Offset: 0x0002A478
	public void LoadTarget()
	{
		this.LoadScene(this.target, null, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0002C2A8 File Offset: 0x0002A4A8
	public UniTask LoadBaseScene(SceneReference overrideCurtainScene = null, bool doCircleFade = true)
	{
		SceneLoader.<LoadBaseScene>d__47 <LoadBaseScene>d__;
		<LoadBaseScene>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadBaseScene>d__.<>4__this = this;
		<LoadBaseScene>d__.overrideCurtainScene = overrideCurtainScene;
		<LoadBaseScene>d__.doCircleFade = doCircleFade;
		<LoadBaseScene>d__.<>1__state = -1;
		<LoadBaseScene>d__.<>t__builder.Start<SceneLoader.<LoadBaseScene>d__47>(ref <LoadBaseScene>d__);
		return <LoadBaseScene>d__.<>t__builder.Task;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0002C2FB File Offset: 0x0002A4FB
	public void NotifyPointerClick(PointerEventData eventData)
	{
		this.clicked = true;
		AudioManager.Post("UI/sceneloader_click");
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0002C30F File Offset: 0x0002A50F
	internal static void StaticLoadSingle(SceneReference sceneReference)
	{
		SceneManager.LoadScene(sceneReference.Name, 0);
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0002C31D File Offset: 0x0002A51D
	internal static void StaticLoadSingle(string sceneID)
	{
		SceneManager.LoadScene(SceneInfoCollection.GetBuildIndex(sceneID), 0);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0002C32C File Offset: 0x0002A52C
	public static void LoadMainMenu(bool circleFade = true)
	{
		if (SceneLoader.Instance)
		{
			SceneLoader.Instance.LoadScene(GameplayDataSettings.SceneManagement.MainMenuScene, null, false, false, circleFade, false, default(MultiSceneLocation), true, false).Forget();
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0002C38C File Offset: 0x0002A58C
	[CompilerGenerated]
	internal static float <LoadScene>g__TimeSinceLoadingStarted|45_0(ref SceneLoader.<>c__DisplayClass45_0 A_0)
	{
		return Time.unscaledTime - A_0.timeWhenLoadingStarted;
	}

	// Token: 0x040008E2 RID: 2274
	public SceneReference defaultCurtainScene;

	// Token: 0x040008E3 RID: 2275
	[SerializeField]
	private OnPointerClick pointerClickEventRecevier;

	// Token: 0x040008E4 RID: 2276
	[SerializeField]
	private float minimumLoadingTime = 1f;

	// Token: 0x040008E5 RID: 2277
	[SerializeField]
	private float waitAfterSceneLoaded = 1f;

	// Token: 0x040008E6 RID: 2278
	[SerializeField]
	private FadeGroup content;

	// Token: 0x040008E7 RID: 2279
	[SerializeField]
	private FadeGroup loadingIndicator;

	// Token: 0x040008E8 RID: 2280
	[SerializeField]
	private FadeGroup clickIndicator;

	// Token: 0x040008E9 RID: 2281
	[SerializeField]
	private AnimationCurve fadeCurve1;

	// Token: 0x040008EA RID: 2282
	[SerializeField]
	private AnimationCurve fadeCurve2;

	// Token: 0x040008EB RID: 2283
	[SerializeField]
	private AnimationCurve fadeCurve3;

	// Token: 0x040008EC RID: 2284
	[SerializeField]
	private AnimationCurve fadeCurve4;

	// Token: 0x040008F2 RID: 2290
	private string _loadingComment;

	// Token: 0x040008F4 RID: 2292
	[SerializeField]
	private SceneReference target;

	// Token: 0x040008F5 RID: 2293
	private bool clicked;
}
