using System;
using Duckov.UI;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000178 RID: 376
public class UIInputManager : MonoBehaviour
{
	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00031469 File Offset: 0x0002F669
	public static UIInputManager Instance
	{
		get
		{
			return GameManager.UiInputManager;
		}
	}

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06000B7C RID: 2940 RVA: 0x00031470 File Offset: 0x0002F670
	// (remove) Token: 0x06000B7D RID: 2941 RVA: 0x000314A4 File Offset: 0x0002F6A4
	public static event Action<UIInputEventData> OnNavigate;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06000B7E RID: 2942 RVA: 0x000314D8 File Offset: 0x0002F6D8
	// (remove) Token: 0x06000B7F RID: 2943 RVA: 0x0003150C File Offset: 0x0002F70C
	public static event Action<UIInputEventData> OnConfirm;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06000B80 RID: 2944 RVA: 0x00031540 File Offset: 0x0002F740
	// (remove) Token: 0x06000B81 RID: 2945 RVA: 0x00031574 File Offset: 0x0002F774
	public static event Action<UIInputEventData> OnToggleIndicatorHUD;

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06000B82 RID: 2946 RVA: 0x000315A8 File Offset: 0x0002F7A8
	// (remove) Token: 0x06000B83 RID: 2947 RVA: 0x000315DC File Offset: 0x0002F7DC
	public static event Action<UIInputEventData> OnCancelEarly;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06000B84 RID: 2948 RVA: 0x00031610 File Offset: 0x0002F810
	// (remove) Token: 0x06000B85 RID: 2949 RVA: 0x00031644 File Offset: 0x0002F844
	public static event Action<UIInputEventData> OnCancel;

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06000B86 RID: 2950 RVA: 0x00031678 File Offset: 0x0002F878
	// (remove) Token: 0x06000B87 RID: 2951 RVA: 0x000316AC File Offset: 0x0002F8AC
	public static event Action<UIInputEventData> OnFastPick;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06000B88 RID: 2952 RVA: 0x000316E0 File Offset: 0x0002F8E0
	// (remove) Token: 0x06000B89 RID: 2953 RVA: 0x00031714 File Offset: 0x0002F914
	public static event Action<UIInputEventData> OnDropItem;

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06000B8A RID: 2954 RVA: 0x00031748 File Offset: 0x0002F948
	// (remove) Token: 0x06000B8B RID: 2955 RVA: 0x0003177C File Offset: 0x0002F97C
	public static event Action<UIInputEventData> OnUseItem;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06000B8C RID: 2956 RVA: 0x000317B0 File Offset: 0x0002F9B0
	// (remove) Token: 0x06000B8D RID: 2957 RVA: 0x000317E4 File Offset: 0x0002F9E4
	public static event Action<UIInputEventData> OnToggleCameraMode;

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06000B8E RID: 2958 RVA: 0x00031818 File Offset: 0x0002FA18
	// (remove) Token: 0x06000B8F RID: 2959 RVA: 0x0003184C File Offset: 0x0002FA4C
	public static event Action<UIInputEventData> OnWishlistHoveringItem;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06000B90 RID: 2960 RVA: 0x00031880 File Offset: 0x0002FA80
	// (remove) Token: 0x06000B91 RID: 2961 RVA: 0x000318B4 File Offset: 0x0002FAB4
	public static event Action<UIInputEventData> OnNextPage;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06000B92 RID: 2962 RVA: 0x000318E8 File Offset: 0x0002FAE8
	// (remove) Token: 0x06000B93 RID: 2963 RVA: 0x0003191C File Offset: 0x0002FB1C
	public static event Action<UIInputEventData> OnPreviousPage;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06000B94 RID: 2964 RVA: 0x00031950 File Offset: 0x0002FB50
	// (remove) Token: 0x06000B95 RID: 2965 RVA: 0x00031984 File Offset: 0x0002FB84
	public static event Action<UIInputEventData> OnLockInventoryIndex;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06000B96 RID: 2966 RVA: 0x000319B8 File Offset: 0x0002FBB8
	// (remove) Token: 0x06000B97 RID: 2967 RVA: 0x000319EC File Offset: 0x0002FBEC
	public static event Action<UIInputEventData, int> OnShortcutInput;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06000B98 RID: 2968 RVA: 0x00031A20 File Offset: 0x0002FC20
	// (remove) Token: 0x06000B99 RID: 2969 RVA: 0x00031A54 File Offset: 0x0002FC54
	public static event Action<InputAction.CallbackContext> OnInteractInputContext;

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00031A87 File Offset: 0x0002FC87
	public static bool Ctrl
	{
		get
		{
			return Keyboard.current != null && Keyboard.current.ctrlKey.isPressed;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00031AA1 File Offset: 0x0002FCA1
	public static bool Alt
	{
		get
		{
			return Keyboard.current != null && Keyboard.current.altKey.isPressed;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00031ABB File Offset: 0x0002FCBB
	public static bool Shift
	{
		get
		{
			return Keyboard.current != null && Keyboard.current.shiftKey.isPressed;
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000B9D RID: 2973 RVA: 0x00031AD8 File Offset: 0x0002FCD8
	public static Vector2 Point
	{
		get
		{
			if (!Application.isPlaying)
			{
				return default(Vector2);
			}
			if (UIInputManager.Instance == null)
			{
				return default(Vector2);
			}
			if (UIInputManager.Instance.inputActionPoint == null)
			{
				return default(Vector2);
			}
			return UIInputManager.Instance.inputActionPoint.ReadValue<Vector2>();
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00031B34 File Offset: 0x0002FD34
	public static Vector2 MouseDelta
	{
		get
		{
			if (!Application.isPlaying)
			{
				return default(Vector2);
			}
			if (UIInputManager.Instance == null)
			{
				return default(Vector2);
			}
			if (UIInputManager.Instance.inputActionMouseDelta == null)
			{
				return default(Vector2);
			}
			return UIInputManager.Instance.inputActionMouseDelta.ReadValue<Vector2>();
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00031B8E File Offset: 0x0002FD8E
	public static bool WasClickedThisFrame
	{
		get
		{
			return Application.isPlaying && !(UIInputManager.Instance == null) && UIInputManager.Instance.inputActionMouseClick != null && UIInputManager.Instance.inputActionMouseClick.WasPressedThisFrame();
		}
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00031BC8 File Offset: 0x0002FDC8
	public static Ray GetPointRay()
	{
		if (UIInputManager.Instance == null)
		{
			return default(Ray);
		}
		GameCamera instance = GameCamera.Instance;
		if (instance == null)
		{
			return default(Ray);
		}
		return instance.renderCamera.ScreenPointToRay(UIInputManager.Point);
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x00031C1C File Offset: 0x0002FE1C
	private void Awake()
	{
		if (UIInputManager.Instance != this)
		{
			return;
		}
		InputActionAsset actions = GameManager.MainPlayerInput.actions;
		this.inputActionNavigate = actions["UI_Navigate"];
		this.inputActionConfirm = actions["UI_Confirm"];
		this.inputActionCancel = actions["UI_Cancel"];
		this.inputActionPoint = actions["Point"];
		this.inputActionFastPick = actions["Interact"];
		this.inputActionDropItem = actions["UI_Item_Drop"];
		this.inputActionUseItem = actions["UI_Item_use"];
		this.inputActionToggleIndicatorHUD = actions["UI_ToggleIndicatorHUD"];
		this.inputActionToggleCameraMode = actions["UI_ToggleCameraMode"];
		this.inputActionWishlistHoveringItem = actions["UI_WishlistHoveringItem"];
		this.inputActionNextPage = actions["UI_NextPage"];
		this.inputActionPreviousPage = actions["UI_PreviousPage"];
		this.inputActionLockInventoryIndex = actions["UI_LockInventoryIndex"];
		this.inputActionMouseDelta = actions["MouseDelta"];
		this.inputActionMouseClick = actions["Click"];
		this.inputActionInteract = actions["Interact"];
		this.Bind(this.inputActionNavigate, new Action<InputAction.CallbackContext>(this.OnInputActionNavigate));
		this.Bind(this.inputActionConfirm, new Action<InputAction.CallbackContext>(this.OnInputActionConfirm));
		this.Bind(this.inputActionCancel, new Action<InputAction.CallbackContext>(this.OnInputActionCancel));
		this.Bind(this.inputActionFastPick, new Action<InputAction.CallbackContext>(this.OnInputActionFastPick));
		this.Bind(this.inputActionDropItem, new Action<InputAction.CallbackContext>(this.OnInputActionDropItem));
		this.Bind(this.inputActionUseItem, new Action<InputAction.CallbackContext>(this.OnInputActionUseItem));
		this.Bind(this.inputActionToggleIndicatorHUD, new Action<InputAction.CallbackContext>(this.OnInputActionToggleIndicatorHUD));
		this.Bind(this.inputActionToggleCameraMode, new Action<InputAction.CallbackContext>(this.OnInputActionToggleCameraMode));
		this.Bind(this.inputActionWishlistHoveringItem, new Action<InputAction.CallbackContext>(this.OnInputWishlistHoveringItem));
		this.Bind(this.inputActionNextPage, new Action<InputAction.CallbackContext>(this.OnInputActionNextPage));
		this.Bind(this.inputActionPreviousPage, new Action<InputAction.CallbackContext>(this.OnInputActionPrevioursPage));
		this.Bind(this.inputActionLockInventoryIndex, new Action<InputAction.CallbackContext>(this.OnInputActionLockInventoryIndex));
		this.Bind(this.inputActionInteract, new Action<InputAction.CallbackContext>(this.OnInputActionInteract));
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00031E8C File Offset: 0x0003008C
	private void OnDestroy()
	{
		this.UnBind(this.inputActionNavigate, new Action<InputAction.CallbackContext>(this.OnInputActionNavigate));
		this.UnBind(this.inputActionConfirm, new Action<InputAction.CallbackContext>(this.OnInputActionConfirm));
		this.UnBind(this.inputActionCancel, new Action<InputAction.CallbackContext>(this.OnInputActionCancel));
		this.UnBind(this.inputActionFastPick, new Action<InputAction.CallbackContext>(this.OnInputActionFastPick));
		this.UnBind(this.inputActionUseItem, new Action<InputAction.CallbackContext>(this.OnInputActionUseItem));
		this.UnBind(this.inputActionToggleIndicatorHUD, new Action<InputAction.CallbackContext>(this.OnInputActionToggleIndicatorHUD));
		this.UnBind(this.inputActionToggleCameraMode, new Action<InputAction.CallbackContext>(this.OnInputActionToggleCameraMode));
		this.UnBind(this.inputActionWishlistHoveringItem, new Action<InputAction.CallbackContext>(this.OnInputWishlistHoveringItem));
		this.UnBind(this.inputActionNextPage, new Action<InputAction.CallbackContext>(this.OnInputActionNextPage));
		this.UnBind(this.inputActionPreviousPage, new Action<InputAction.CallbackContext>(this.OnInputActionPrevioursPage));
		this.UnBind(this.inputActionLockInventoryIndex, new Action<InputAction.CallbackContext>(this.OnInputActionLockInventoryIndex));
		this.UnBind(this.inputActionInteract, new Action<InputAction.CallbackContext>(this.OnInputActionInteract));
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00031FB9 File Offset: 0x000301B9
	private void OnInputActionInteract(InputAction.CallbackContext context)
	{
		Action<InputAction.CallbackContext> onInteractInputContext = UIInputManager.OnInteractInputContext;
		if (onInteractInputContext == null)
		{
			return;
		}
		onInteractInputContext(context);
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00031FCB File Offset: 0x000301CB
	private void OnInputActionLockInventoryIndex(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onLockInventoryIndex = UIInputManager.OnLockInventoryIndex;
			if (onLockInventoryIndex == null)
			{
				return;
			}
			onLockInventoryIndex(new UIInputEventData());
		}
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00031FEA File Offset: 0x000301EA
	private void OnInputActionNextPage(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onNextPage = UIInputManager.OnNextPage;
			if (onNextPage == null)
			{
				return;
			}
			onNextPage(new UIInputEventData());
		}
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00032009 File Offset: 0x00030209
	private void OnInputActionPrevioursPage(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onPreviousPage = UIInputManager.OnPreviousPage;
			if (onPreviousPage == null)
			{
				return;
			}
			onPreviousPage(new UIInputEventData());
		}
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x00032028 File Offset: 0x00030228
	private void OnInputWishlistHoveringItem(InputAction.CallbackContext context)
	{
		if (!context.started)
		{
			return;
		}
		Action<UIInputEventData> onWishlistHoveringItem = UIInputManager.OnWishlistHoveringItem;
		if (onWishlistHoveringItem == null)
		{
			return;
		}
		onWishlistHoveringItem(new UIInputEventData());
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00032048 File Offset: 0x00030248
	private void OnInputActionToggleCameraMode(InputAction.CallbackContext context)
	{
		if (View.ActiveView != null)
		{
			return;
		}
		if (context.started)
		{
			Action<UIInputEventData> onToggleCameraMode = UIInputManager.OnToggleCameraMode;
			if (onToggleCameraMode == null)
			{
				return;
			}
			onToggleCameraMode(new UIInputEventData());
		}
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00032075 File Offset: 0x00030275
	private void OnInputActionDropItem(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onDropItem = UIInputManager.OnDropItem;
			if (onDropItem == null)
			{
				return;
			}
			onDropItem(new UIInputEventData());
		}
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x00032094 File Offset: 0x00030294
	private void OnInputActionUseItem(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onUseItem = UIInputManager.OnUseItem;
			if (onUseItem == null)
			{
				return;
			}
			onUseItem(new UIInputEventData());
		}
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x000320B3 File Offset: 0x000302B3
	private void OnInputActionFastPick(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onFastPick = UIInputManager.OnFastPick;
			if (onFastPick == null)
			{
				return;
			}
			onFastPick(new UIInputEventData());
		}
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x000320D4 File Offset: 0x000302D4
	private void OnInputActionCancel(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			UIInputEventData uiinputEventData = new UIInputEventData
			{
				cancel = true
			};
			Action<UIInputEventData> onCancelEarly = UIInputManager.OnCancelEarly;
			if (onCancelEarly != null)
			{
				onCancelEarly(uiinputEventData);
			}
			if (uiinputEventData.Used)
			{
				return;
			}
			Action<UIInputEventData> onCancel = UIInputManager.OnCancel;
			if (onCancel != null)
			{
				onCancel(uiinputEventData);
			}
			if (uiinputEventData.Used)
			{
				return;
			}
			if (LevelManager.Instance != null && View.ActiveView == null && !SceneLoader.IsSceneLoading)
			{
				PauseMenu.Toggle();
			}
		}
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00032151 File Offset: 0x00030351
	private void OnInputActionConfirm(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onConfirm = UIInputManager.OnConfirm;
			if (onConfirm == null)
			{
				return;
			}
			onConfirm(new UIInputEventData
			{
				confirm = true
			});
		}
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00032178 File Offset: 0x00030378
	private void OnInputActionNavigate(InputAction.CallbackContext context)
	{
		Vector2 vector = context.ReadValue<Vector2>();
		Action<UIInputEventData> onNavigate = UIInputManager.OnNavigate;
		if (onNavigate == null)
		{
			return;
		}
		onNavigate(new UIInputEventData
		{
			vector = vector
		});
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x000321A8 File Offset: 0x000303A8
	private void OnInputActionToggleIndicatorHUD(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onToggleIndicatorHUD = UIInputManager.OnToggleIndicatorHUD;
			if (onToggleIndicatorHUD == null)
			{
				return;
			}
			onToggleIndicatorHUD(new UIInputEventData());
		}
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x000321C7 File Offset: 0x000303C7
	private void Bind(InputAction inputAction, Action<InputAction.CallbackContext> action)
	{
		inputAction.Enable();
		inputAction.started += action;
		inputAction.performed += action;
		inputAction.canceled += action;
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x000321E4 File Offset: 0x000303E4
	private void UnBind(InputAction inputAction, Action<InputAction.CallbackContext> action)
	{
		if (inputAction != null)
		{
			inputAction.started -= action;
			inputAction.performed -= action;
			inputAction.canceled -= action;
		}
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x000321FE File Offset: 0x000303FE
	internal static void NotifyShortcutInput(int index)
	{
		UIInputManager.OnShortcutInput(new UIInputEventData
		{
			confirm = true
		}, index);
	}

	// Token: 0x040009E9 RID: 2537
	private static bool instantiated;

	// Token: 0x040009EA RID: 2538
	private InputAction inputActionNavigate;

	// Token: 0x040009EB RID: 2539
	private InputAction inputActionConfirm;

	// Token: 0x040009EC RID: 2540
	private InputAction inputActionCancel;

	// Token: 0x040009ED RID: 2541
	private InputAction inputActionPoint;

	// Token: 0x040009EE RID: 2542
	private InputAction inputActionMouseDelta;

	// Token: 0x040009EF RID: 2543
	private InputAction inputActionMouseClick;

	// Token: 0x040009F0 RID: 2544
	private InputAction inputActionFastPick;

	// Token: 0x040009F1 RID: 2545
	private InputAction inputActionDropItem;

	// Token: 0x040009F2 RID: 2546
	private InputAction inputActionUseItem;

	// Token: 0x040009F3 RID: 2547
	private InputAction inputActionToggleIndicatorHUD;

	// Token: 0x040009F4 RID: 2548
	private InputAction inputActionToggleCameraMode;

	// Token: 0x040009F5 RID: 2549
	private InputAction inputActionWishlistHoveringItem;

	// Token: 0x040009F6 RID: 2550
	private InputAction inputActionNextPage;

	// Token: 0x040009F7 RID: 2551
	private InputAction inputActionPreviousPage;

	// Token: 0x040009F8 RID: 2552
	private InputAction inputActionLockInventoryIndex;

	// Token: 0x040009F9 RID: 2553
	private InputAction inputActionInteract;
}
