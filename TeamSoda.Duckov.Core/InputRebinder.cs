using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Saves;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001C4 RID: 452
public class InputRebinder : MonoBehaviour
{
	// Token: 0x06000D9F RID: 3487 RVA: 0x00039299 File Offset: 0x00037499
	public void Rebind()
	{
		InputRebinder.RebindAsync(this.action, this.index, this.excludes, false).Forget<bool>();
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x000392B8 File Offset: 0x000374B8
	private static PlayerInput PlayerInput
	{
		get
		{
			return GameManager.MainPlayerInput;
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x000392BF File Offset: 0x000374BF
	private static bool OperationPending
	{
		get
		{
			return InputRebinder.operation.started && !InputRebinder.operation.canceled && !InputRebinder.operation.completed;
		}
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x000392EA File Offset: 0x000374EA
	private void Awake()
	{
		InputRebinder.Load();
		UIInputManager.OnCancelEarly += this.OnUICancel;
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x00039302 File Offset: 0x00037502
	private void OnDestroy()
	{
		UIInputManager.OnCancelEarly -= this.OnUICancel;
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00039315 File Offset: 0x00037515
	private void OnUICancel(UIInputEventData data)
	{
		if (InputRebinder.OperationPending)
		{
			data.Use();
		}
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x00039324 File Offset: 0x00037524
	public static void Load()
	{
		string text = SavesSystem.LoadGlobal<string>("InputBinding", null);
		string.IsNullOrEmpty(text);
		try
		{
			InputActionRebindingExtensions.LoadBindingOverridesFromJson(InputRebinder.PlayerInput.actions, text, true);
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			InputActionRebindingExtensions.RemoveAllBindingOverrides(InputRebinder.PlayerInput.actions);
		}
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x00039380 File Offset: 0x00037580
	public static void Save()
	{
		string text = InputActionRebindingExtensions.SaveBindingOverridesAsJson(InputRebinder.PlayerInput.actions);
		SavesSystem.SaveGlobal<string>("InputBinding", text);
		Debug.Log(text);
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x000393AE File Offset: 0x000375AE
	public static void Clear()
	{
		InputActionRebindingExtensions.RemoveAllBindingOverrides(InputRebinder.PlayerInput.actions);
		Action onBindingChanged = InputRebinder.OnBindingChanged;
		if (onBindingChanged != null)
		{
			onBindingChanged();
		}
		InputIndicator.NotifyBindingChanged();
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x000393D4 File Offset: 0x000375D4
	private static void Rebind(string name, int index, string[] excludes = null)
	{
		if (InputRebinder.OperationPending)
		{
			return;
		}
		InputAction inputAction = InputRebinder.PlayerInput.actions[name];
		if (inputAction == null)
		{
			Debug.LogError("找不到名为 " + name + " 的 action");
			return;
		}
		Action<InputAction> onRebindBegin = InputRebinder.OnRebindBegin;
		if (onRebindBegin != null)
		{
			onRebindBegin(inputAction);
		}
		Debug.Log("Resetting");
		InputRebinder.operation.Reset();
		Debug.Log("Settingup");
		inputAction.actionMap.Disable();
		InputRebinder.operation.WithCancelingThrough("<Keyboard>/escape").WithAction(inputAction).WithTargetBinding(index).OnComplete(new Action<InputActionRebindingExtensions.RebindingOperation>(InputRebinder.OnComplete)).OnCancel(new Action<InputActionRebindingExtensions.RebindingOperation>(InputRebinder.OnCancel));
		if (excludes != null)
		{
			foreach (string text in excludes)
			{
				InputRebinder.operation.WithControlsExcluding(text);
			}
		}
		Debug.Log("Starting");
		InputRebinder.operation.Start();
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x000394C4 File Offset: 0x000376C4
	public static UniTask<bool> RebindAsync(string name, int index, string[] excludes = null, bool save = false)
	{
		InputRebinder.<RebindAsync>d__20 <RebindAsync>d__;
		<RebindAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<RebindAsync>d__.name = name;
		<RebindAsync>d__.index = index;
		<RebindAsync>d__.excludes = excludes;
		<RebindAsync>d__.save = save;
		<RebindAsync>d__.<>1__state = -1;
		<RebindAsync>d__.<>t__builder.Start<InputRebinder.<RebindAsync>d__20>(ref <RebindAsync>d__);
		return <RebindAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x00039520 File Offset: 0x00037720
	public static void ClearRebind(string name)
	{
		if (InputRebinder.OperationPending)
		{
			return;
		}
		InputAction inputAction = InputRebinder.PlayerInput.actions[name];
		if (inputAction == null)
		{
			Debug.LogError("找不到名为 " + name + " 的 action");
			return;
		}
		InputActionRebindingExtensions.RemoveAllBindingOverrides(inputAction);
		InputIndicator.NotifyBindingChanged();
		InputRebinder.Save();
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x00039570 File Offset: 0x00037770
	private static void OnCancel(InputActionRebindingExtensions.RebindingOperation operation)
	{
		Debug.Log(operation.action.name + " binding canceled");
		operation.action.actionMap.Enable();
		Action<InputAction> onRebindComplete = InputRebinder.OnRebindComplete;
		if (onRebindComplete == null)
		{
			return;
		}
		onRebindComplete(operation.action);
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x000395BC File Offset: 0x000377BC
	private static void OnComplete(InputActionRebindingExtensions.RebindingOperation operation)
	{
		Debug.Log(operation.action.name + " bind to " + operation.selectedControl.name);
		operation.action.actionMap.Enable();
		Action<InputAction> onRebindComplete = InputRebinder.OnRebindComplete;
		if (onRebindComplete != null)
		{
			onRebindComplete(operation.action);
		}
		Action onBindingChanged = InputRebinder.OnBindingChanged;
		if (onBindingChanged != null)
		{
			onBindingChanged();
		}
		InputIndicator.NotifyRebindComplete(operation.action);
	}

	// Token: 0x04000BC1 RID: 3009
	[Header("Debug")]
	[SerializeField]
	private string action = "MoveAxis";

	// Token: 0x04000BC2 RID: 3010
	[SerializeField]
	private int index = 2;

	// Token: 0x04000BC3 RID: 3011
	[SerializeField]
	private string[] excludes = new string[]
	{
		"<Mouse>/leftButton",
		"<Mouse>/rightButton",
		"<Pointer>/position",
		"<Pointer>/delta",
		"<Pointer>/Press"
	};

	// Token: 0x04000BC4 RID: 3012
	public static Action<InputAction> OnRebindBegin;

	// Token: 0x04000BC5 RID: 3013
	public static Action<InputAction> OnRebindComplete;

	// Token: 0x04000BC6 RID: 3014
	public static Action OnBindingChanged;

	// Token: 0x04000BC7 RID: 3015
	private static InputActionRebindingExtensions.RebindingOperation operation = new InputActionRebindingExtensions.RebindingOperation();

	// Token: 0x04000BC8 RID: 3016
	private const string SaveKey = "InputBinding";
}
