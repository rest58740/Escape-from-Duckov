using System;
using System.Collections.Generic;
using System.Reflection;
using Dialogues;
using Duckov;
using Duckov.MiniMaps.UI;
using Duckov.Quests.UI;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000078 RID: 120
public class CharacterInputControl : MonoBehaviour
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000477 RID: 1143 RVA: 0x00014BC9 File Offset: 0x00012DC9
	// (set) Token: 0x06000478 RID: 1144 RVA: 0x00014BD0 File Offset: 0x00012DD0
	public static CharacterInputControl Instance { get; private set; }

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000479 RID: 1145 RVA: 0x00014BD8 File Offset: 0x00012DD8
	private PlayerInput PlayerInput
	{
		get
		{
			return GameManager.MainPlayerInput;
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x0600047A RID: 1146 RVA: 0x00014BDF File Offset: 0x00012DDF
	private bool usingMouseAndKeyboard
	{
		get
		{
			return InputManager.InputDevice == InputManager.InputDevices.mouseKeyboard;
		}
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x00014BE9 File Offset: 0x00012DE9
	private void Awake()
	{
		CharacterInputControl.Instance = this;
		this.inputActions = new CharacterInputControl.InputActionReferences(this.PlayerInput);
		this.RegisterEvents();
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00014C08 File Offset: 0x00012E08
	private void OnDestroy()
	{
		this.UnregisterEvent();
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00014C10 File Offset: 0x00012E10
	private void RegisterEvents()
	{
		this.Bind(this.inputActions.MoveAxis, new Action<InputAction.CallbackContext>(this.OnPlayerMoveInput));
		this.Bind(this.inputActions.Run, new Action<InputAction.CallbackContext>(this.OnPlayerRunInput));
		this.Bind(this.inputActions.MousePos, new Action<InputAction.CallbackContext>(this.OnPlayerMouseMove));
		this.Bind(this.inputActions.Skill_1_StartAim, new Action<InputAction.CallbackContext>(this.OnStartCharacterSkillAim));
		this.Bind(this.inputActions.Reload, new Action<InputAction.CallbackContext>(this.OnReloadInput));
		this.Bind(this.inputActions.Interact, new Action<InputAction.CallbackContext>(this.OnInteractInput));
		this.Bind(this.inputActions.Quack, new Action<InputAction.CallbackContext>(this.OnQuackInput));
		this.Bind(this.inputActions.ScrollWheel, new Action<InputAction.CallbackContext>(this.OnMouseScollerInput));
		this.Bind(this.inputActions.SwitchWeapon, new Action<InputAction.CallbackContext>(this.OnSwitchWeaponInput));
		this.Bind(this.inputActions.SwitchInteractAndBulletType, new Action<InputAction.CallbackContext>(this.OnSwitchInteractAndBulletTypeInput));
		this.Bind(this.inputActions.Trigger, new Action<InputAction.CallbackContext>(this.OnPlayerTriggerInputUsingMouseKeyboard));
		this.Bind(this.inputActions.ToggleView, new Action<InputAction.CallbackContext>(this.OnToggleViewInput));
		this.Bind(this.inputActions.ToggleNightVision, new Action<InputAction.CallbackContext>(this.OnToggleNightVisionInput));
		this.Bind(this.inputActions.CancelSkill, new Action<InputAction.CallbackContext>(this.OnCancelSkillInput));
		this.Bind(this.inputActions.Dash, new Action<InputAction.CallbackContext>(this.OnDashInput));
		this.Bind(this.inputActions.ItemShortcut1, new Action<InputAction.CallbackContext>(this.OnPlayerSwitchItemAgent1));
		this.Bind(this.inputActions.ItemShortcut2, new Action<InputAction.CallbackContext>(this.OnPlayerSwitchItemAgent2));
		this.Bind(this.inputActions.ItemShortcut3, new Action<InputAction.CallbackContext>(this.OnShortCutInput3));
		this.Bind(this.inputActions.ItemShortcut4, new Action<InputAction.CallbackContext>(this.OnShortCutInput4));
		this.Bind(this.inputActions.ItemShortcut5, new Action<InputAction.CallbackContext>(this.OnShortCutInput5));
		this.Bind(this.inputActions.ItemShortcut6, new Action<InputAction.CallbackContext>(this.OnShortCutInput6));
		this.Bind(this.inputActions.ItemShortcut7, new Action<InputAction.CallbackContext>(this.OnShortCutInput7));
		this.Bind(this.inputActions.ItemShortcut8, new Action<InputAction.CallbackContext>(this.OnShortCutInput8));
		this.Bind(this.inputActions.ADS, new Action<InputAction.CallbackContext>(this.OnPlayerAdsInput));
		this.Bind(this.inputActions.UI_Inventory, new Action<InputAction.CallbackContext>(this.OnUIInventoryInput));
		this.Bind(this.inputActions.UI_Map, new Action<InputAction.CallbackContext>(this.OnUIMapInput));
		this.Bind(this.inputActions.UI_Quest, new Action<InputAction.CallbackContext>(this.OnUIQuestViewInput));
		this.Bind(this.inputActions.StopAction, new Action<InputAction.CallbackContext>(this.OnPlayerStopAction));
		this.Bind(this.inputActions.PutAway, new Action<InputAction.CallbackContext>(this.OnPutAwayInput));
		this.Bind(this.inputActions.ItemShortcut_Melee, new Action<InputAction.CallbackContext>(this.OnPlayerSwitchItemAgentMelee));
		this.Bind(this.inputActions.MouseDelta, new Action<InputAction.CallbackContext>(this.OnPlayerMouseDelta));
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00014FA0 File Offset: 0x000131A0
	private void UnregisterEvent()
	{
		while (this.unbindCommands.Count > 0)
		{
			this.unbindCommands.Dequeue()();
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00014FC4 File Offset: 0x000131C4
	private void Bind(InputAction action, Action<InputAction.CallbackContext> method)
	{
		action.performed += method;
		action.started += method;
		action.canceled += method;
		this.unbindCommands.Enqueue(delegate
		{
			this.Unbind(action, method);
		});
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00015036 File Offset: 0x00013236
	private void Unbind(InputAction action, Action<InputAction.CallbackContext> method)
	{
		action.performed -= method;
		action.started -= method;
		action.canceled -= method;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00015050 File Offset: 0x00013250
	private void Update()
	{
		if (!this.character)
		{
			this.character = CharacterMainControl.Main;
			if (!this.character)
			{
				return;
			}
		}
		if (this.usingMouseAndKeyboard)
		{
			this.inputManager.SetMousePosition(this.mousePos);
			this.inputManager.SetAimInputUsingMouse(this.mouseDelta);
			this.inputManager.SetTrigger(this.mouseKeyboardTriggerInput, this.mouseKeyboardTriggerInputThisFrame, this.mouseKeyboardTriggerReleaseThisFrame);
			if (this.character.skillAction.holdItemSkillKeeper.CheckSkillAndBinding())
			{
				this.inputManager.SetAimType(AimTypes.handheldSkill);
				if (this.mouseKeyboardTriggerInputThisFrame)
				{
					this.inputManager.StartItemSkillAim();
				}
				else if (this.mouseKeyboardTriggerReleaseThisFrame)
				{
					Debug.Log("Release");
					this.inputManager.ReleaseItemSkill();
				}
			}
			else
			{
				this.inputManager.SetAimType(AimTypes.normalAim);
			}
			this.UpdateScollerInput();
		}
		this.mouseKeyboardTriggerInputThisFrame = false;
		this.mouseKeyboardTriggerReleaseThisFrame = false;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00015144 File Offset: 0x00013344
	public void OnPlayerMoveInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Vector2 moveInput = context.ReadValue<Vector2>();
			this.inputManager.SetMoveInput(moveInput);
		}
		if (context.canceled)
		{
			this.inputManager.SetMoveInput(Vector2.zero);
		}
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00015188 File Offset: 0x00013388
	public void OnPlayerRunInput(InputAction.CallbackContext context)
	{
		this.runInput = false;
		if (context.started)
		{
			this.inputManager.SetRunInput(true);
			this.runInput = true;
		}
		if (context.canceled)
		{
			this.inputManager.SetRunInput(false);
			this.runInput = false;
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x000151D4 File Offset: 0x000133D4
	public void OnPlayerAdsInput(InputAction.CallbackContext context)
	{
		this.adsInput = false;
		if (context.started)
		{
			this.inputManager.SetAdsInput(true);
			this.adsInput = true;
		}
		if (context.canceled)
		{
			this.inputManager.SetAdsInput(false);
			this.adsInput = false;
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00015220 File Offset: 0x00013420
	public void OnToggleViewInput(InputAction.CallbackContext context)
	{
		if (GameManager.Paused)
		{
			return;
		}
		if (context.started)
		{
			this.inputManager.ToggleView();
		}
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0001523E File Offset: 0x0001343E
	public void OnToggleNightVisionInput(InputAction.CallbackContext context)
	{
		if (GameManager.Paused)
		{
			return;
		}
		if (context.started)
		{
			this.inputManager.ToggleNightVision();
		}
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0001525C File Offset: 0x0001345C
	public void OnPlayerTriggerInputUsingMouseKeyboard(InputAction.CallbackContext context)
	{
		if (InputManager.InputDevice != InputManager.InputDevices.mouseKeyboard)
		{
			return;
		}
		if (context.started)
		{
			this.mouseKeyboardTriggerInputThisFrame = true;
			this.mouseKeyboardTriggerInput = true;
			this.mouseKeyboardTriggerReleaseThisFrame = false;
			return;
		}
		if (context.canceled)
		{
			this.mouseKeyboardTriggerInputThisFrame = false;
			this.mouseKeyboardTriggerInput = false;
			this.mouseKeyboardTriggerReleaseThisFrame = true;
		}
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x000152AE File Offset: 0x000134AE
	public void OnPlayerMouseMove(InputAction.CallbackContext context)
	{
		this.mousePos = context.ReadValue<Vector2>();
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x000152BD File Offset: 0x000134BD
	public void OnPlayerMouseDelta(InputAction.CallbackContext context)
	{
		this.mouseDelta = context.ReadValue<Vector2>();
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x000152CC File Offset: 0x000134CC
	public void OnPlayerStopAction(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.StopAction();
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x000152E2 File Offset: 0x000134E2
	public void OnPlayerSwitchItemAgent1(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.SwitchItemAgent(1);
		}
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x000152F9 File Offset: 0x000134F9
	public void OnPlayerSwitchItemAgent2(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.SwitchItemAgent(2);
		}
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00015310 File Offset: 0x00013510
	public void OnPlayerSwitchItemAgentMelee(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.SwitchItemAgent(3);
		}
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00015327 File Offset: 0x00013527
	public void OnStartCharacterSkillAim(InputAction.CallbackContext context)
	{
		this.inputManager.StartCharacterSkillAim();
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00015334 File Offset: 0x00013534
	public void OnCharacterSkillRelease()
	{
		this.inputManager.ReleaseCharacterSkill();
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00015341 File Offset: 0x00013541
	public void OnReloadInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		main.TryToReload(null);
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00015360 File Offset: 0x00013560
	public void OnUIInventoryInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (GameManager.Paused)
		{
			return;
		}
		if (DialogueUI.Active)
		{
			return;
		}
		if (SceneLoader.IsSceneLoading)
		{
			return;
		}
		if (!(View.ActiveView == null))
		{
			View.ActiveView.TryQuit();
			return;
		}
		if (LevelManager.Instance.IsBaseLevel)
		{
			PlayerStorage.Instance.InteractableLootBox.InteractWithMainCharacter();
			return;
		}
		InventoryView.Show();
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x000153C8 File Offset: 0x000135C8
	public void OnUIQuestViewInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (GameManager.Paused)
		{
			return;
		}
		if (DialogueUI.Active)
		{
			return;
		}
		if (View.ActiveView == null)
		{
			QuestView.Show();
			return;
		}
		if (View.ActiveView is QuestView)
		{
			View.ActiveView.TryQuit();
		}
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00015418 File Offset: 0x00013618
	public void OnDashInput(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.Dash();
		}
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00015430 File Offset: 0x00013630
	public void OnUIMapInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (GameManager.Paused)
		{
			return;
		}
		if (SceneLoader.IsSceneLoading)
		{
			return;
		}
		if (View.ActiveView == null)
		{
			MiniMapView.Show();
			return;
		}
		MiniMapView miniMapView = View.ActiveView as MiniMapView;
		if (miniMapView != null)
		{
			miniMapView.Close();
		}
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0001547E File Offset: 0x0001367E
	public void OnCancelSkillInput(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.CancleSkill();
		}
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00015495 File Offset: 0x00013695
	public void OnInteractInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.inputManager.Interact();
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x000154AC File Offset: 0x000136AC
	public void OnQuackInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.inputManager.Quack();
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x000154C3 File Offset: 0x000136C3
	public void OnPutAwayInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.inputManager.PutAway();
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x000154DA File Offset: 0x000136DA
	public void OnMouseScollerInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			this.scollY = context.ReadValue<Vector2>().y;
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x000154F8 File Offset: 0x000136F8
	private void UpdateScollerInput()
	{
		float num = 1f;
		if (Mathf.Abs(this.scollY) > 0.5f && (float)this.scollYZeroFrames > num)
		{
			if (ScrollWheelBehaviour.CurrentBehaviour == ScrollWheelBehaviour.Behaviour.AmmoAndInteract)
			{
				this.inputManager.SetSwitchInteractInput((this.scollY > 0f) ? 1 : -1);
				this.inputManager.SetSwitchBulletTypeInput((this.scollY > 0f) ? 1 : -1);
			}
			else
			{
				this.inputManager.SetSwitchWeaponInput((this.scollY > 0f) ? 1 : -1);
			}
		}
		if (Mathf.Abs(this.scollY) < 0.5f)
		{
			this.scollYZeroFrames++;
			return;
		}
		this.scollYZeroFrames = 0;
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x000155AC File Offset: 0x000137AC
	public void OnSwitchWeaponInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			float num = context.ReadValue<float>();
			this.inputManager.SetSwitchWeaponInput((num > 0f) ? -1 : 1);
		}
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x000155E4 File Offset: 0x000137E4
	public void OnSwitchInteractAndBulletTypeInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			float num = context.ReadValue<float>();
			this.inputManager.SetSwitchInteractInput((num > 0f) ? -1 : 1);
			this.inputManager.SetSwitchBulletTypeInput((num > 0f) ? -1 : 1);
		}
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00015630 File Offset: 0x00013830
	private void ShortCutInput(int index)
	{
		if (PauseMenu.Instance && PauseMenu.Instance.Shown)
		{
			return;
		}
		if (View.ActiveView != null)
		{
			UIInputManager.NotifyShortcutInput(index - 3);
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		Item item = ItemShortcut.Get(index - 3);
		if (item == null)
		{
			return;
		}
		if (!this.character)
		{
			return;
		}
		if (item && item.UsageUtilities && item.UsageUtilities.IsUsable(item, this.character))
		{
			this.character.UseItem(item);
			return;
		}
		if (item && item.GetBool("IsSkill", false))
		{
			this.character.ChangeHoldItem(item);
			return;
		}
		if (item && item.HasHandHeldAgent)
		{
			Debug.Log("has hand held");
			this.character.ChangeHoldItem(item);
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00015716 File Offset: 0x00013916
	public void OnShortCutInput3(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(3);
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00015729 File Offset: 0x00013929
	public void OnShortCutInput4(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(4);
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0001573C File Offset: 0x0001393C
	public void OnShortCutInput5(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(5);
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0001574F File Offset: 0x0001394F
	public void OnShortCutInput6(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(6);
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00015762 File Offset: 0x00013962
	public void OnShortCutInput7(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(7);
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00015775 File Offset: 0x00013975
	public void OnShortCutInput8(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(8);
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00015788 File Offset: 0x00013988
	internal static InputAction GetInputAction(string name)
	{
		if (CharacterInputControl.Instance == null)
		{
			return null;
		}
		InputAction result;
		try
		{
			result = CharacterInputControl.Instance.PlayerInput.actions[name];
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			Debug.LogError("查找 Input Action " + name + " 时发生错误, 返回null");
			result = null;
		}
		return result;
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x000157EC File Offset: 0x000139EC
	public static bool GetChangeBulletTypeWasPressed()
	{
		return CharacterInputControl.Instance.inputActions.SwitchBulletType.WasPressedThisFrame();
	}

	// Token: 0x040003E5 RID: 997
	public InputManager inputManager;

	// Token: 0x040003E6 RID: 998
	private bool runInput;

	// Token: 0x040003E7 RID: 999
	private bool adsInput;

	// Token: 0x040003E8 RID: 1000
	private bool aimDown;

	// Token: 0x040003E9 RID: 1001
	private Vector2 mousePos;

	// Token: 0x040003EA RID: 1002
	private Vector2 mouseDelta;

	// Token: 0x040003EB RID: 1003
	private bool mouseKeyboardTriggerInput;

	// Token: 0x040003EC RID: 1004
	private bool mouseKeyboardTriggerReleaseThisFrame;

	// Token: 0x040003ED RID: 1005
	private bool mouseKeyboardTriggerInputThisFrame;

	// Token: 0x040003EE RID: 1006
	private CharacterMainControl character;

	// Token: 0x040003EF RID: 1007
	private CharacterInputControl.InputActionReferences inputActions;

	// Token: 0x040003F0 RID: 1008
	private Queue<Action> unbindCommands = new Queue<Action>();

	// Token: 0x040003F1 RID: 1009
	private float scollY;

	// Token: 0x040003F2 RID: 1010
	private int scollYZeroFrames;

	// Token: 0x02000457 RID: 1111
	private class InputActionReferences
	{
		// Token: 0x06002743 RID: 10051 RVA: 0x00088388 File Offset: 0x00086588
		public InputActionReferences(PlayerInput playerInput)
		{
			InputActionAsset actions = playerInput.actions;
			Type typeFromHandle = typeof(CharacterInputControl.InputActionReferences);
			Type typeFromHandle2 = typeof(InputAction);
			FieldInfo[] fields = typeFromHandle.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType != typeFromHandle2)
				{
					Debug.LogError(fieldInfo.FieldType.Name);
				}
				else
				{
					InputAction inputAction = actions[fieldInfo.Name];
					if (inputAction == null)
					{
						Debug.LogError("找不到名为 " + fieldInfo.Name + " 的input action");
					}
					else
					{
						fieldInfo.SetValue(this, inputAction);
					}
				}
			}
			foreach (FieldInfo fieldInfo2 in fields)
			{
				if (!(fieldInfo2.FieldType != typeFromHandle2))
				{
					fieldInfo2.GetValue(this);
				}
			}
		}

		// Token: 0x04001B14 RID: 6932
		public InputAction MoveAxis;

		// Token: 0x04001B15 RID: 6933
		public InputAction Run;

		// Token: 0x04001B16 RID: 6934
		public InputAction Aim;

		// Token: 0x04001B17 RID: 6935
		public InputAction MousePos;

		// Token: 0x04001B18 RID: 6936
		public InputAction ItemShortcut1;

		// Token: 0x04001B19 RID: 6937
		public InputAction ItemShortcut2;

		// Token: 0x04001B1A RID: 6938
		public InputAction Skill_1_StartAim;

		// Token: 0x04001B1B RID: 6939
		public InputAction Reload;

		// Token: 0x04001B1C RID: 6940
		public InputAction UI_Inventory;

		// Token: 0x04001B1D RID: 6941
		public InputAction UI_Map;

		// Token: 0x04001B1E RID: 6942
		public InputAction Interact;

		// Token: 0x04001B1F RID: 6943
		public InputAction ScrollWheel;

		// Token: 0x04001B20 RID: 6944
		public InputAction SwitchWeapon;

		// Token: 0x04001B21 RID: 6945
		public InputAction SwitchInteractAndBulletType;

		// Token: 0x04001B22 RID: 6946
		public InputAction Trigger;

		// Token: 0x04001B23 RID: 6947
		public InputAction ToggleView;

		// Token: 0x04001B24 RID: 6948
		public InputAction ToggleNightVision;

		// Token: 0x04001B25 RID: 6949
		public InputAction CancelSkill;

		// Token: 0x04001B26 RID: 6950
		public InputAction Dash;

		// Token: 0x04001B27 RID: 6951
		public InputAction ItemShortcut3;

		// Token: 0x04001B28 RID: 6952
		public InputAction ItemShortcut4;

		// Token: 0x04001B29 RID: 6953
		public InputAction ItemShortcut5;

		// Token: 0x04001B2A RID: 6954
		public InputAction ItemShortcut6;

		// Token: 0x04001B2B RID: 6955
		public InputAction ItemShortcut7;

		// Token: 0x04001B2C RID: 6956
		public InputAction ItemShortcut8;

		// Token: 0x04001B2D RID: 6957
		public InputAction Quack;

		// Token: 0x04001B2E RID: 6958
		public InputAction ADS;

		// Token: 0x04001B2F RID: 6959
		public InputAction UI_Quest;

		// Token: 0x04001B30 RID: 6960
		public InputAction StopAction;

		// Token: 0x04001B31 RID: 6961
		public InputAction PutAway;

		// Token: 0x04001B32 RID: 6962
		public InputAction ItemShortcut_Melee;

		// Token: 0x04001B33 RID: 6963
		public InputAction MouseDelta;

		// Token: 0x04001B34 RID: 6964
		public InputAction SwitchBulletType;
	}
}
