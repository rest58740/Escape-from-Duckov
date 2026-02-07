using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x02000004 RID: 4
public class InputIndicator : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000007 RID: 7 RVA: 0x00002128 File Offset: 0x00000328
	// (remove) Token: 0x06000008 RID: 8 RVA: 0x0000215C File Offset: 0x0000035C
	private static event Action<InputAction> onNotifiedRebindComplete;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000009 RID: 9 RVA: 0x00002190 File Offset: 0x00000390
	// (remove) Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000003C4
	private static event Action onBindingChanged;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600000B RID: 11 RVA: 0x000021F8 File Offset: 0x000003F8
	// (remove) Token: 0x0600000C RID: 12 RVA: 0x0000222C File Offset: 0x0000042C
	public static event Action<InputIndicator> OnAfterRefresh;

	// Token: 0x0600000D RID: 13 RVA: 0x0000225F File Offset: 0x0000045F
	public static void NotifyRebindComplete(InputAction action)
	{
		Action<InputAction> action2 = InputIndicator.onNotifiedRebindComplete;
		if (action2 == null)
		{
			return;
		}
		action2.Invoke(action);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002271 File Offset: 0x00000471
	public static void NotifyBindingChanged()
	{
		Action action = InputIndicator.onBindingChanged;
		if (action == null)
		{
			return;
		}
		action.Invoke();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002282 File Offset: 0x00000482
	private void Awake()
	{
		InputSystem.onDeviceChange += new Action<InputDevice, InputDeviceChange>(this.OnDeviceChanged);
		InputIndicator.onNotifiedRebindComplete += new Action<InputAction>(this.OnRebind);
		InputIndicator.onBindingChanged += new Action(this.Refresh);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000022B7 File Offset: 0x000004B7
	private void OnDestroy()
	{
		InputSystem.onDeviceChange -= new Action<InputDevice, InputDeviceChange>(this.OnDeviceChanged);
		InputIndicator.onNotifiedRebindComplete -= new Action<InputAction>(this.OnRebind);
		InputIndicator.onBindingChanged -= new Action(this.Refresh);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000022EC File Offset: 0x000004EC
	private void OnRebind(InputAction action)
	{
		if (this.inputActionRef.action.name != action.name)
		{
			return;
		}
		Debug.Log("rebind Refreshing " + action.name);
		this.Refresh();
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002327 File Offset: 0x00000527
	private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
	{
		this.Refresh();
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000232F File Offset: 0x0000052F
	public void Setup(InputActionReference inputActionRef, int index = -1)
	{
		this.inputActionRef = inputActionRef;
		this.bindingIndex = index;
		this.Refresh();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002345 File Offset: 0x00000545
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0000234D File Offset: 0x0000054D
	private void OnControlChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002355 File Offset: 0x00000555
	private void ShowInvalid()
	{
		this.text.text = "-";
		this.ShowText();
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002370 File Offset: 0x00000570
	private InputBinding GetBinding()
	{
		if (!Application.isPlaying)
		{
			return default(InputBinding);
		}
		if (this.inputActionRef.action.bindings.Count <= 0)
		{
			return default(InputBinding);
		}
		string group = "Keyboard";
		PlayerInput playerInput = null;
		if (PlayerInput.all.Count > 0)
		{
			playerInput = PlayerInput.all[0];
			group = playerInput.currentControlScheme;
		}
		int num = this.bindingIndex;
		InputAction inputAction;
		if (playerInput != null)
		{
			inputAction = playerInput.actions[this.inputActionRef.action.name];
		}
		else
		{
			inputAction = this.inputActionRef.action;
		}
		if (this.bindingIndex < 0)
		{
			num = inputAction.GetBindingIndex(InputBinding.MaskByGroup(group));
			if (num < 0)
			{
				num = 0;
			}
		}
		if (num < 0)
		{
			num = 0;
		}
		if (num >= inputAction.bindings.Count)
		{
			num = 0;
		}
		return inputAction.bindings[num];
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002468 File Offset: 0x00000668
	private void Refresh()
	{
		if (this.inputActionRef == null)
		{
			this.ShowInvalid();
			return;
		}
		InputBinding binding = this.GetBinding();
		bool isComposite = binding.isComposite;
		string deviceLayoutName;
		string controlPath;
		string text = binding.ToDisplayString(out deviceLayoutName, out controlPath, this.displayStringOptions, null);
		if (text == "Control")
		{
			text = "Ctrl";
		}
		this.text.text = text;
		Sprite sprite = this.GetIcon(deviceLayoutName, controlPath);
		if (sprite != null)
		{
			this.icon.sprite = sprite;
			this.ShowIcon();
		}
		else
		{
			this.ShowText();
		}
		Action<InputIndicator> onAfterRefresh = InputIndicator.OnAfterRefresh;
		if (onAfterRefresh == null)
		{
			return;
		}
		onAfterRefresh.Invoke(this);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000250A File Offset: 0x0000070A
	private void ShowText()
	{
		this.textContainer.SetActive(true);
		this.icon.gameObject.SetActive(false);
		this.text.gameObject.SetActive(true);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000253A File Offset: 0x0000073A
	private void ShowIcon()
	{
		this.textContainer.SetActive(false);
		this.icon.gameObject.SetActive(true);
		this.text.gameObject.SetActive(false);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x0000256A File Offset: 0x0000076A
	private Sprite GetIcon(string deviceLayoutName, string controlPath)
	{
		return InputIconDatabase.GetIcon(deviceLayoutName, controlPath);
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002573 File Offset: 0x00000773
	private void OnValidate()
	{
		this.Refresh();
	}

	// Token: 0x04000004 RID: 4
	[SerializeField]
	private InputActionReference inputActionRef;

	// Token: 0x04000005 RID: 5
	[SerializeField]
	private GameObject textContainer;

	// Token: 0x04000006 RID: 6
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04000007 RID: 7
	[SerializeField]
	private Image icon;

	// Token: 0x04000008 RID: 8
	[SerializeField]
	private int bindingIndex = -1;

	// Token: 0x04000009 RID: 9
	[SerializeField]
	private InputBinding.DisplayStringOptions displayStringOptions;
}
