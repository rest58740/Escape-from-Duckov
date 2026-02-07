using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.Options;
using Duckov.UI;
using Duckov.UI.DialogueBubbles;
using Duckov.Utilities;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000109 RID: 265
public class InputManager : MonoBehaviour
{
	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00027F08 File Offset: 0x00026108
	public static InputManager.InputDevices InputDevice
	{
		get
		{
			return InputManager.inputDevice;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00027F0F File Offset: 0x0002610F
	public Vector3 WorldMoveInput
	{
		get
		{
			return this.worldMoveInput;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x060008D5 RID: 2261 RVA: 0x00027F17 File Offset: 0x00026117
	public Transform AimTarget
	{
		get
		{
			return this.aimTargetCol;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00027F1F File Offset: 0x0002611F
	public Vector2 MoveAxisInput
	{
		get
		{
			return this.moveAxisInput;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x060008D7 RID: 2263 RVA: 0x00027F27 File Offset: 0x00026127
	public Vector2 AimScreenPoint
	{
		get
		{
			return this.aimScreenPoint;
		}
	}

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x060008D8 RID: 2264 RVA: 0x00027F30 File Offset: 0x00026130
	// (remove) Token: 0x060008D9 RID: 2265 RVA: 0x00027F64 File Offset: 0x00026164
	public static event Action OnInputDeviceChanged;

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x060008DA RID: 2266 RVA: 0x00027F97 File Offset: 0x00026197
	public Vector3 InputAimPoint
	{
		get
		{
			return this.inputAimPoint;
		}
	}

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x060008DB RID: 2267 RVA: 0x00027FA0 File Offset: 0x000261A0
	// (remove) Token: 0x060008DC RID: 2268 RVA: 0x00027FD4 File Offset: 0x000261D4
	public static event Action<int> OnSwitchBulletTypeInput;

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060008DD RID: 2269 RVA: 0x00028008 File Offset: 0x00026208
	// (remove) Token: 0x060008DE RID: 2270 RVA: 0x0002803C File Offset: 0x0002623C
	public static event Action<int> OnSwitchWeaponInput;

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x060008DF RID: 2271 RVA: 0x0002806F File Offset: 0x0002626F
	private static InputManager instance
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance.InputManager;
		}
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0002808A File Offset: 0x0002628A
	private void OnDestroy()
	{
		Cursor.visible = true;
		Cursor.lockState = 0;
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00028098 File Offset: 0x00026298
	public static bool InputActived
	{
		get
		{
			return InputManager.instance && !GameManager.Paused && !CameraMode.Active && LevelManager.LevelInited && CharacterMainControl.Main && !CharacterMainControl.Main.Health.IsDead && InputManager.instance.inputActiveCoolCounter <= 0;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x060008E2 RID: 2274 RVA: 0x000280FD File Offset: 0x000262FD
	public Vector2 MousePos
	{
		get
		{
			return this.inputMousePosition;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00028105 File Offset: 0x00026305
	public bool TriggerInput
	{
		get
		{
			return this.triggerInput;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0002810D File Offset: 0x0002630D
	// (set) Token: 0x060008E5 RID: 2277 RVA: 0x00028140 File Offset: 0x00026340
	private Vector2 AimMousePosition
	{
		get
		{
			if (!this.aimMousePosFirstSynced)
			{
				this.aimMousePosFirstSynced = true;
				if (Mouse.current != null)
				{
					this._aimMousePosCache = Mouse.current.position.ReadValue();
				}
			}
			return this._aimMousePosCache;
		}
		set
		{
			if (!this.aimMousePosFirstSynced)
			{
				this.aimMousePosFirstSynced = true;
				if (Mouse.current != null)
				{
					this._aimMousePosCache = Mouse.current.position.ReadValue();
				}
			}
			this._aimMousePosCache = value;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00028174 File Offset: 0x00026374
	public bool AimingEnemyHead
	{
		get
		{
			return this.aimingEnemyHead;
		}
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0002817C File Offset: 0x0002637C
	private void Start()
	{
		this.obsticleHits = new RaycastHit[3];
		this.obsticleLayers = (GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask);
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x000281B4 File Offset: 0x000263B4
	private void OnApplicationFocus(bool hasFocus)
	{
		this.currentFocus = hasFocus;
		if (!this.currentFocus)
		{
			Cursor.lockState = 0;
		}
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x000281CB File Offset: 0x000263CB
	private void Awake()
	{
		if (this.blockInputSources == null)
		{
			this.blockInputSources = new HashSet<GameObject>();
		}
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x000281E0 File Offset: 0x000263E0
	public static void DisableInput(GameObject source)
	{
		if (source == null)
		{
			return;
		}
		if (InputManager.instance == null)
		{
			return;
		}
		InputManager.instance.inputActiveCoolCounter = 2;
		InputManager.instance.blockInputSources.Add(source);
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00028216 File Offset: 0x00026416
	public static void ActiveInput(GameObject source)
	{
		if (source == null)
		{
			return;
		}
		InputManager.instance.blockInputSources.Remove(source);
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x00028233 File Offset: 0x00026433
	public static void SetInputDevice(InputManager.InputDevices _inputDevice)
	{
		Action onInputDeviceChanged = InputManager.OnInputDeviceChanged;
		if (onInputDeviceChanged == null)
		{
			return;
		}
		onInputDeviceChanged();
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x00028244 File Offset: 0x00026444
	private void UpdateCursor()
	{
		if (LevelManager.Instance == null || this.characterMainControl == null || !this.characterMainControl.gameObject.activeInHierarchy)
		{
			Cursor.visible = true;
			Cursor.lockState = 0;
			return;
		}
		bool flag = !this.characterMainControl || this.characterMainControl.Health.IsDead;
		bool flag2 = true;
		if (InputManager.InputActived && !flag)
		{
			flag2 = false;
		}
		if (CameraMode.Active)
		{
			flag2 = false;
		}
		if (View.ActiveView != null)
		{
			flag2 = true;
		}
		if (!Application.isFocused)
		{
			flag2 = true;
		}
		if (this.cursorVisable != flag2)
		{
			this.cursorVisable = !this.cursorVisable;
		}
		if (this.cursorVisable)
		{
			this.recoilNeedToRecover = Vector2.zero;
			if (Mouse.current != null)
			{
				this.AimMousePosition = Mouse.current.position.ReadValue();
			}
		}
		if (Application.isFocused)
		{
			Cursor.visible = this.cursorVisable;
		}
		else
		{
			Cursor.visible = true;
		}
		bool flag3 = false;
		if (CameraMode.Active)
		{
			flag3 = true;
		}
		if (this.currentFocus)
		{
			Cursor.lockState = (flag3 ? 1 : 2);
			return;
		}
		Cursor.lockState = 0;
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x00028364 File Offset: 0x00026564
	private void Update()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!this.mainCam)
		{
			this.mainCam = LevelManager.Instance.GameCamera.renderCamera;
			return;
		}
		this.UpdateInputActived();
		this.UpdateCursor();
		if (this.runInput)
		{
			if (this.runInptutThisFrame)
			{
				this.runInputBuffer = !this.runInputBuffer;
			}
		}
		else if (this.moveAxisInput.magnitude < 0.1f)
		{
			this.runInputBuffer = false;
		}
		else if (this.adsInput)
		{
			this.runInputBuffer = false;
		}
		this.characterMainControl.SetRunInput(InputManager.useRunInputBuffer ? this.runInputBuffer : this.runInput);
		this.SetMoveInput(this.moveAxisInput);
		if (InputManager.InputDevice == InputManager.InputDevices.touch)
		{
			this.UpdateJoystickAim();
			this.UpdateAimWhileUsingTouch();
		}
		if (this.checkGunDurabilityCoolTimer <= this.checkGunDurabilityCoolTime)
		{
			this.checkGunDurabilityCoolTimer += Time.deltaTime;
		}
		this.runInptutThisFrame = false;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00028460 File Offset: 0x00026660
	private void UpdateInputActived()
	{
		this.blockInputSources.RemoveWhere((GameObject x) => x == null || !x.activeInHierarchy);
		if (this.blockInputSources.Count > 0)
		{
			InputManager.instance.inputActiveCoolCounter = 2;
			return;
		}
		if (InputManager.instance.inputActiveCoolCounter > 0)
		{
			InputManager.instance.inputActiveCoolCounter--;
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x000284D1 File Offset: 0x000266D1
	private void UpdateAimWhileUsingTouch()
	{
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x000284D4 File Offset: 0x000266D4
	public void SetTrigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)
	{
		this.triggerInput = false;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.Trigger(false, false, false);
			return;
		}
		this.triggerInput = trigger;
		this.characterMainControl.Trigger(trigger, triggerThisFrame, releaseThisFrame);
		if (trigger)
		{
			this.CheckGunDurability();
		}
		if (triggerThisFrame)
		{
			this.runInputBuffer = false;
			this.characterMainControl.Attack();
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00028540 File Offset: 0x00026740
	private void CheckAttack()
	{
		if (InputManager.InputDevice != InputManager.InputDevices.touch)
		{
			return;
		}
		if (this.characterMainControl.CurrentAction && this.characterMainControl.CurrentAction.Running)
		{
			return;
		}
		ItemAgent_MeleeWeapon meleeWeapon = this.characterMainControl.GetMeleeWeapon();
		if (meleeWeapon == null)
		{
			return;
		}
		if (meleeWeapon.AttackableTargetInRange())
		{
			this.characterMainControl.Attack();
		}
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x000285A8 File Offset: 0x000267A8
	private void CheckGunDurability()
	{
		if (this.checkGunDurabilityCoolTimer <= this.checkGunDurabilityCoolTime)
		{
			return;
		}
		ItemAgent_Gun gun = this.characterMainControl.GetGun();
		if (gun != null && gun.Item.Durability <= 0f)
		{
			DialogueBubblesManager.Show(LocalizationManager.ToPlainText("Pop_GunBroken"), this.characterMainControl.transform, 2.5f, false, false, -1f, 2f).Forget();
		}
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0002861C File Offset: 0x0002681C
	private Vector3 TrnasAxisInputToWorld(Vector2 axisInput)
	{
		Vector3 result = Vector3.zero;
		if (!this.mainCam)
		{
			return result;
		}
		if (!this.characterMainControl)
		{
			return result;
		}
		if (MoveDirectionOptions.MoveViaCharacterDirection)
		{
			Vector3 vector = this.inputAimPoint - this.characterMainControl.transform.position;
			vector.y = 0f;
			if (vector.magnitude < 1f)
			{
				return this.characterMainControl.transform.forward;
			}
			vector.Normalize();
			Vector3 vector2 = Quaternion.Euler(0f, 90f, 0f) * vector;
			result = axisInput.x * vector2 + axisInput.y * vector;
		}
		else
		{
			Vector3 right = this.mainCam.transform.right;
			right.y = 0f;
			right.Normalize();
			Vector3 forward = this.mainCam.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			result = axisInput.x * right + axisInput.y * forward;
		}
		return result;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00028749 File Offset: 0x00026949
	public void SetSwitchBulletTypeInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		Action<int> onSwitchBulletTypeInput = InputManager.OnSwitchBulletTypeInput;
		if (onSwitchBulletTypeInput == null)
		{
			return;
		}
		onSwitchBulletTypeInput(dir);
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00028771 File Offset: 0x00026971
	public void SetSwitchWeaponInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		Action<int> onSwitchWeaponInput = InputManager.OnSwitchWeaponInput;
		if (onSwitchWeaponInput != null)
		{
			onSwitchWeaponInput(dir);
		}
		this.characterMainControl.SwitchWeapon(dir);
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x000287A6 File Offset: 0x000269A6
	public void SetSwitchInteractInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.SwitchInteractSelection((dir > 0) ? -1 : 1);
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x000287D4 File Offset: 0x000269D4
	public void SetMoveInput(Vector2 axisInput)
	{
		this.moveAxisInput = axisInput;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.SetMoveInput(Vector3.zero);
			return;
		}
		this.worldMoveInput = this.TrnasAxisInputToWorld(axisInput);
		Vector3 normalized = this.worldMoveInput;
		if (normalized.magnitude > 0.02f)
		{
			normalized = normalized.normalized;
		}
		this.characterMainControl.SetMoveInput(normalized);
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00028844 File Offset: 0x00026A44
	public void SetRunInput(bool run)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.runInput = false;
			this.runInptutThisFrame = false;
			this.characterMainControl.SetRunInput(false);
			return;
		}
		this.runInptutThisFrame = (!this.runInput && run);
		this.runInput = run;
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00028899 File Offset: 0x00026A99
	public void SetAdsInput(bool ads)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.SetAdsInput(false);
			this.adsInput = false;
			return;
		}
		this.adsInput = ads;
		this.characterMainControl.SetAdsInput(ads);
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x000288D7 File Offset: 0x00026AD7
	public void ToggleView()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		CameraArm.ToggleView();
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x000288F4 File Offset: 0x00026AF4
	public void ToggleNightVision()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ToggleNightVision();
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x00028917 File Offset: 0x00026B17
	public void SetAimInputUsingJoystick(Vector2 _joystickAxisInput)
	{
		if (InputManager.InputDevice == InputManager.InputDevices.mouseKeyboard)
		{
			return;
		}
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.joystickAxisInput = Vector3.zero;
			return;
		}
		this.joystickAxisInput = _joystickAxisInput;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0002894E File Offset: 0x00026B4E
	private void UpdateJoystickAim()
	{
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00028950 File Offset: 0x00026B50
	public void SetAimType(AimTypes aimType)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		SkillBase currentRunningSkill = this.characterMainControl.GetCurrentRunningSkill();
		if (aimType != this.characterMainControl.AimType && currentRunningSkill != null)
		{
			Debug.Log("skill is running:" + currentRunningSkill.name);
			return;
		}
		this.characterMainControl.SetAimType(aimType);
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x000289B8 File Offset: 0x00026BB8
	public void SetMousePosition(Vector2 mousePosition)
	{
		this.inputMousePosition = mousePosition;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x000289C4 File Offset: 0x00026BC4
	public void SetAimInputUsingMouse(Vector2 mouseDelta)
	{
		this.aimingEnemyHead = false;
		this.AimMousePosition += mouseDelta * OptionsManager.MouseSensitivity / 10f;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		ItemAgent_Gun gun = this.characterMainControl.GetGun();
		if (gun)
		{
			this.AimMousePosition = this.ProcessMousePosViaRecoil(this.AimMousePosition, mouseDelta, gun);
		}
		Vector2 vector = default(Vector2);
		if (Application.isFocused && InputManager.InputActived && !Application.isEditor)
		{
			Vector2 aimMousePosition = this.AimMousePosition;
			this.ClampMousePosInWindow(ref aimMousePosition, ref vector);
			this.AimMousePosition = aimMousePosition;
		}
		this.aimScreenPoint = this.AimMousePosition;
		this.characterMainControl.GetCurrentRunningSkill();
		Ray ray = LevelManager.Instance.GameCamera.renderCamera.ScreenPointToRay(this.aimScreenPoint);
		Plane plane;
		plane..ctor(Vector3.up, Vector3.up * (this.characterMainControl.transform.position.y + 0.5f));
		float num = 0f;
		plane.Raycast(ray, ref num);
		Vector3 vector2 = ray.origin + ray.direction * num;
		Debug.DrawLine(vector2, vector2 + Vector3.up * 3f, Color.yellow);
		Vector3 aimPoint = vector2;
		if (gun && this.characterMainControl.CanControlAim())
		{
			if (Physics.Raycast(ray, ref this.hittedHead, 100f, 1 << LayerMask.NameToLayer("HeadCollider")))
			{
				this.aimingEnemyHead = true;
			}
			Vector3 position = this.characterMainControl.transform.position;
			if (gun)
			{
				position = gun.muzzle.transform.position;
			}
			Vector3 vector3 = vector2 - position;
			vector3.y = 0f;
			vector3.Normalize();
			Vector3 vector4 = Vector3.Cross(vector3, ray.direction);
			this.aimCheckLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
			int num2 = 0;
			while ((float)num2 < 45f)
			{
				int num3 = num2;
				if (num2 > 23)
				{
					num3 = -(num2 - 23);
				}
				float num4 = 1.5f;
				Vector3 vector5 = Quaternion.AngleAxis(-2f * (float)num3, vector4) * vector3;
				Ray ray2;
				ray2..ctor(position + num4 * vector5, vector5);
				if (Physics.SphereCast(ray2, 0.02f, ref this.hittedCharacterDmgReceiverInfo, gun.BulletDistance, this.aimCheckLayers, 1) && this.hittedCharacterDmgReceiverInfo.distance > 0.1f && !Physics.SphereCast(ray2, 0.1f, ref this.hittedObsticleInfo, this.hittedCharacterDmgReceiverInfo.distance, this.obsticleLayers, 1))
				{
					aimPoint = this.hittedCharacterDmgReceiverInfo.point;
					break;
				}
				num2++;
			}
		}
		if (this.aimingEnemyHead)
		{
			Vector3 direction = ray.direction;
			Vector3 vector6 = this.hittedHead.collider.transform.position - this.hittedHead.point;
			float num5 = Vector3.Dot(direction, vector6);
			aimPoint = this.hittedHead.point + direction * num5 * 0.5f;
		}
		this.inputAimPoint = vector2;
		this.characterMainControl.SetAimPoint(aimPoint);
		if (Application.isFocused && this.currentFocus && InputManager.InputActived)
		{
			Mouse.current.WarpCursorPosition(this.AimMousePosition);
		}
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x00028D5C File Offset: 0x00026F5C
	private Vector2 ProcessMousePosViaCameraChange(Vector2 inputMousePos)
	{
		Camera renderCamera = LevelManager.Instance.GameCamera.renderCamera;
		if (this.fovCache < 0f)
		{
			this.fovCache = renderCamera.fieldOfView;
			return inputMousePos;
		}
		float fieldOfView = renderCamera.fieldOfView;
		Vector2 vector = new Vector2(inputMousePos.x / (float)Screen.width * 2f - 1f, inputMousePos.y / (float)Screen.height * 2f - 1f);
		float num = Mathf.Tan(this.fovCache * 0.017453292f / 2f) / Mathf.Tan(fieldOfView * 0.017453292f / 2f);
		Vector2 vector2 = vector * num;
		Vector2 result = new Vector2((vector2.x + 1f) * 0.5f * (float)Screen.width, (vector2.y + 1f) * 0.5f * (float)Screen.height);
		this.fovCache = fieldOfView;
		return result;
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00028E44 File Offset: 0x00027044
	private void ClampMousePosInWindow(ref Vector2 mousePosition, ref Vector2 deltaValue)
	{
		Vector2 zero = Vector2.zero;
		zero.x = Mathf.Clamp(mousePosition.x, 0f, (float)Screen.width);
		zero.y = Mathf.Clamp(mousePosition.y, 0f, (float)Screen.height);
		deltaValue = zero - mousePosition;
		mousePosition = zero;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00028EAA File Offset: 0x000270AA
	public void Interact()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.Interact();
		Action onInteractButtonDown = InputManager.OnInteractButtonDown;
		if (onInteractButtonDown == null)
		{
			return;
		}
		onInteractButtonDown();
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00028EDC File Offset: 0x000270DC
	public void PutAway()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ChangeHoldItem(null);
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00028F01 File Offset: 0x00027101
	public void Quack()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.Quack();
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00028F24 File Offset: 0x00027124
	public void SwitchItemAgent(int index)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		switch (index)
		{
		case 1:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.PrimaryWeaponSlotHash);
			return;
		case 2:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.SecondaryWeaponSlotHash);
			return;
		case 3:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.MeleeWeaponSlotHash);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00028F8E File Offset: 0x0002718E
	public void StopAction()
	{
		if (InputManager.InputActived && this.characterMainControl.CurrentAction && this.characterMainControl.CurrentAction.IsStopable())
		{
			this.characterMainControl.CurrentAction.StopAction();
		}
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00028FCC File Offset: 0x000271CC
	private bool CheckInAimAngleAndNoObsticle()
	{
		if (!this.characterMainControl)
		{
			return false;
		}
		if (this.aimTarget == null || this.characterMainControl.CurrentUsingAimSocket == null)
		{
			return false;
		}
		Vector3 position = this.characterMainControl.CurrentUsingAimSocket.position;
		position.y = 0f;
		Vector3 position2 = this.aimTarget.position;
		position2.y = 0f;
		Vector3 vector = position2 - position;
		float magnitude = vector.magnitude;
		vector.Normalize();
		float num = Mathf.Atan(0.25f / magnitude) * 57.29578f;
		if (Vector3.Angle(this.characterMainControl.CurrentAimDirection, vector) >= num)
		{
			return false;
		}
		Vector3 vector2 = position + Vector3.up * this.characterMainControl.CurrentUsingAimSocket.position.y;
		Vector3 vector3 = vector;
		Debug.DrawLine(vector2, vector2 + vector3 * magnitude);
		return Physics.SphereCastNonAlloc(vector2, 0.1f, vector3, this.obsticleHits, magnitude, this.obsticleLayers, 1) <= 0;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x000290E5 File Offset: 0x000272E5
	public void ReleaseItemSkill()
	{
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ReleaseSkill(SkillTypes.itemSkill);
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x000290FC File Offset: 0x000272FC
	public void ReleaseCharacterSkill()
	{
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ReleaseSkill(SkillTypes.characterSkill);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x00029113 File Offset: 0x00027313
	public bool CancleSkill()
	{
		return this.characterMainControl && this.characterMainControl.CancleSkill();
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0002912F File Offset: 0x0002732F
	public void Dash()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.TryCatchFishInput();
		this.characterMainControl.Dash();
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00029160 File Offset: 0x00027360
	public void StartCharacterSkillAim()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		if (this.characterMainControl.skillAction.characterSkillKeeper.Skill == null)
		{
			return;
		}
		if (this.characterMainControl.StartSkillAim(SkillTypes.characterSkill) && this.characterMainControl.skillAction.CurrentRunningSkill && this.characterMainControl.skillAction.CurrentRunningSkill.SkillContext.releaseOnStartAim)
		{
			this.characterMainControl.ReleaseSkill(SkillTypes.characterSkill);
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x000291F0 File Offset: 0x000273F0
	public void StartItemSkillAim()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		if (!this.characterMainControl.agentHolder.Skill)
		{
			return;
		}
		if (this.characterMainControl.StartSkillAim(SkillTypes.itemSkill) && this.characterMainControl.skillAction.CurrentRunningSkill && this.characterMainControl.skillAction.CurrentRunningSkill.SkillContext.releaseOnStartAim)
		{
			this.characterMainControl.ReleaseSkill(SkillTypes.itemSkill);
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0002927C File Offset: 0x0002747C
	public void AddRecoil(ItemAgent_Gun gun)
	{
		if (!gun)
		{
			return;
		}
		this.recoilGun = gun;
		float recoilMultiplier = LevelManager.Rule.RecoilMultiplier;
		this.recoilV = Random.Range(gun.RecoilVMin, gun.RecoilVMax) * gun.RecoilScaleV * (1f / gun.CharacterRecoilControl) * recoilMultiplier;
		this.recoilH = Random.Range(gun.RecoilHMin, gun.RecoilHMax) * gun.RecoilScaleH * (1f / gun.CharacterRecoilControl) * recoilMultiplier;
		this.recoilRecover = gun.RecoilRecover;
		this.recoilTime = Mathf.Min(gun.RecoilTime, 1f / gun.ShootSpeed);
		this.recoilRecoverTime = gun.RecoilRecoverTime;
		this.recoilTimer = 0f;
		this.newRecoil = true;
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x00029348 File Offset: 0x00027548
	private Vector2 ProcessMousePosViaRecoil(Vector2 mousePos, Vector2 mouseDelta, ItemAgent_Gun gun)
	{
		if (!gun || this.recoilGun != gun)
		{
			this.newRecoil = false;
			this.recoilNeedToRecover = Vector2.zero;
			return mousePos;
		}
		Vector3 position = this.characterMainControl.transform.position;
		if (this.newRecoil)
		{
			Vector2 vector = LevelManager.Instance.GameCamera.renderCamera.WorldToScreenPoint(position);
			Vector2 normalized = (mousePos - vector).normalized;
			this.recoilThisShot = normalized * this.recoilV + this.recoilH * -Vector2.Perpendicular(normalized);
		}
		Vector3.Distance(this.InputAimPoint, position);
		float num = Time.deltaTime;
		if (this.recoilTimer + num >= this.recoilTime)
		{
			num = this.recoilTime - this.recoilTimer;
		}
		if (num > 0f)
		{
			Vector2 vector2 = this.recoilThisShot * num / this.recoilTime * (float)Screen.height / 1440f;
			mousePos += vector2;
			this.recoilNeedToRecover += vector2;
			Vector2 zero = Vector2.zero;
			this.ClampMousePosInWindow(ref mousePos, ref zero);
			this.recoilNeedToRecover += zero;
		}
		if (num <= 0f && this.recoilTimer > this.recoilRecoverTime && this.recoilNeedToRecover.magnitude > 0f)
		{
			float num2 = Time.deltaTime;
			if (this.recoilTimer - num2 < this.recoilRecoverTime)
			{
				num2 = this.recoilTimer - this.recoilRecoverTime;
			}
			Vector2 vector3 = Vector2.MoveTowards(this.recoilNeedToRecover, Vector2.zero, num2 * this.recoilRecover * (float)Screen.height / 1440f);
			mousePos += vector3 - this.recoilNeedToRecover;
			this.recoilNeedToRecover = vector3;
		}
		float num3 = Vector2.Dot(-this.recoilNeedToRecover.normalized, mouseDelta);
		if (num3 > 0f)
		{
			this._oppositeDelta = 0f;
			this.recoilNeedToRecover = Vector2.MoveTowards(this.recoilNeedToRecover, Vector2.zero, num3);
		}
		else
		{
			this._oppositeDelta += mouseDelta.magnitude;
			if (this._oppositeDelta > 15f * (float)Screen.height / 1440f)
			{
				this._oppositeDelta = 0f;
				this.recoilNeedToRecover = Vector3.zero;
			}
		}
		this.recoilTimer += Time.deltaTime;
		this.newRecoil = false;
		return mousePos;
	}

	// Token: 0x0400081D RID: 2077
	private static InputManager.InputDevices inputDevice = InputManager.InputDevices.mouseKeyboard;

	// Token: 0x0400081E RID: 2078
	public CharacterMainControl characterMainControl;

	// Token: 0x0400081F RID: 2079
	public AimTargetFinder aimTargetFinder;

	// Token: 0x04000820 RID: 2080
	public float runThreshold = 0.85f;

	// Token: 0x04000821 RID: 2081
	private Vector3 worldMoveInput;

	// Token: 0x04000822 RID: 2082
	public static Action OnInteractButtonDown;

	// Token: 0x04000823 RID: 2083
	private Transform aimTargetCol;

	// Token: 0x04000824 RID: 2084
	private LayerMask obsticleLayers;

	// Token: 0x04000825 RID: 2085
	private RaycastHit[] obsticleHits;

	// Token: 0x04000826 RID: 2086
	private RaycastHit hittedCharacterDmgReceiverInfo;

	// Token: 0x04000827 RID: 2087
	private RaycastHit hittedObsticleInfo;

	// Token: 0x04000828 RID: 2088
	private RaycastHit hittedHead;

	// Token: 0x04000829 RID: 2089
	private LayerMask aimCheckLayers;

	// Token: 0x0400082A RID: 2090
	private CharacterMainControl foundCharacter;

	// Token: 0x0400082B RID: 2091
	public static readonly int PrimaryWeaponSlotHash = "PrimaryWeapon".GetHashCode();

	// Token: 0x0400082C RID: 2092
	public static readonly int SecondaryWeaponSlotHash = "SecondaryWeapon".GetHashCode();

	// Token: 0x0400082D RID: 2093
	public static readonly int MeleeWeaponSlotHash = "MeleeWeapon".GetHashCode();

	// Token: 0x0400082E RID: 2094
	private Camera mainCam;

	// Token: 0x0400082F RID: 2095
	private float checkGunDurabilityCoolTimer;

	// Token: 0x04000830 RID: 2096
	private float checkGunDurabilityCoolTime = 2f;

	// Token: 0x04000831 RID: 2097
	private Transform aimTarget;

	// Token: 0x04000832 RID: 2098
	private Vector2 joystickAxisInput;

	// Token: 0x04000833 RID: 2099
	private Vector2 moveAxisInput;

	// Token: 0x04000834 RID: 2100
	private Vector2 aimScreenPoint;

	// Token: 0x04000836 RID: 2102
	private Vector3 inputAimPoint;

	// Token: 0x04000837 RID: 2103
	public static bool useRunInputBuffer = false;

	// Token: 0x04000838 RID: 2104
	private HashSet<GameObject> blockInputSources = new HashSet<GameObject>();

	// Token: 0x0400083B RID: 2107
	private int inputActiveCoolCounter;

	// Token: 0x0400083C RID: 2108
	private bool adsInput;

	// Token: 0x0400083D RID: 2109
	private bool runInputBuffer;

	// Token: 0x0400083E RID: 2110
	private bool runInput;

	// Token: 0x0400083F RID: 2111
	private bool runInptutThisFrame;

	// Token: 0x04000840 RID: 2112
	private bool newRecoil;

	// Token: 0x04000841 RID: 2113
	private ItemAgent_Gun recoilGun;

	// Token: 0x04000842 RID: 2114
	private float recoilV;

	// Token: 0x04000843 RID: 2115
	private float recoilH;

	// Token: 0x04000844 RID: 2116
	private float recoilRecover;

	// Token: 0x04000845 RID: 2117
	private bool triggerInput;

	// Token: 0x04000846 RID: 2118
	private Vector2 recoilNeedToRecover;

	// Token: 0x04000847 RID: 2119
	private Vector2 inputMousePosition;

	// Token: 0x04000848 RID: 2120
	private Vector2 _aimMousePosCache;

	// Token: 0x04000849 RID: 2121
	private bool aimMousePosFirstSynced;

	// Token: 0x0400084A RID: 2122
	private bool cursorVisable = true;

	// Token: 0x0400084B RID: 2123
	private bool aimingEnemyHead;

	// Token: 0x0400084C RID: 2124
	private bool currentFocus = true;

	// Token: 0x0400084D RID: 2125
	private float fovCache = -1f;

	// Token: 0x0400084E RID: 2126
	private float _oppositeDelta;

	// Token: 0x0400084F RID: 2127
	private float recoilTimer;

	// Token: 0x04000850 RID: 2128
	private float recoilTime = 0.04f;

	// Token: 0x04000851 RID: 2129
	private float recoilRecoverTime = 0.1f;

	// Token: 0x04000852 RID: 2130
	private Vector2 recoilThisShot;

	// Token: 0x020004A7 RID: 1191
	public enum InputDevices
	{
		// Token: 0x04001C8A RID: 7306
		mouseKeyboard,
		// Token: 0x04001C8B RID: 7307
		touch
	}
}
