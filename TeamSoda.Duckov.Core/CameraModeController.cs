using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Token: 0x020001AB RID: 427
public class CameraModeController : MonoBehaviour
{
	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00036789 File Offset: 0x00034989
	private static string filePath
	{
		get
		{
			if (GameMetaData.Instance.Platform == Platform.WeGame)
			{
				return Application.streamingAssetsPath + "/ScreenShots";
			}
			return Application.persistentDataPath + "/ScreenShots";
		}
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x000367B8 File Offset: 0x000349B8
	private void UpdateInput()
	{
		this.moveInput = this.inputActionAsset["CameraModeMove"].ReadValue<Vector2>();
		this.focusInput = this.inputActionAsset["CameraModeFocus"].IsPressed();
		this.upDownInput = this.inputActionAsset["CameraModeUpDown"].ReadValue<float>();
		this.fovInput = this.inputActionAsset["CameraModeFOV"].ReadValue<float>();
		this.aimInput = this.inputActionAsset["CameraModeAim"].ReadValue<Vector2>();
		this.captureInput = this.inputActionAsset["CameraModeCapture"].WasPressedThisFrame();
		this.fastInput = this.inputActionAsset["CameraModeFaster"].IsPressed();
		this.openFolderInput = this.inputActionAsset["CameraModeOpenFolder"].WasPressedThisFrame();
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x000368A0 File Offset: 0x00034AA0
	private void Awake()
	{
		CameraMode.OnCameraModeActivated = (Action)Delegate.Combine(CameraMode.OnCameraModeActivated, new Action(this.OnCameraModeActivated));
		CameraMode.OnCameraModeDeactivated = (Action)Delegate.Combine(CameraMode.OnCameraModeDeactivated, new Action(this.OnCameraModeDeactivated));
		this.inputActionAsset.Enable();
		this.vCam.gameObject.SetActive(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x00036918 File Offset: 0x00034B18
	private void Update()
	{
		if (!this.actived)
		{
			return;
		}
		this.UpdateInput();
		if (this.shootting)
		{
			return;
		}
		this.UpdateMove();
		this.UpdateLook();
		this.UpdateFov();
		if (this.captureInput)
		{
			this.Shot().Forget();
		}
		if (this.openFolderInput)
		{
			CameraModeController.OpenFolder();
			this.openFolderInput = false;
		}
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00036979 File Offset: 0x00034B79
	private void LateUpdate()
	{
		this.UpdateFocus();
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x00036984 File Offset: 0x00034B84
	private void UpdateMove()
	{
		Vector3 forward = this.vCam.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		Vector3 right = this.vCam.transform.right;
		right.y = 0f;
		right.Normalize();
		Vector3 vector = right * this.moveInput.x + forward * this.moveInput.y;
		vector.Normalize();
		vector += this.upDownInput * Vector3.up;
		this.vCam.transform.position += Time.unscaledDeltaTime * vector * (this.fastInput ? this.fastMoveSpeed : this.moveSpeed);
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00036A60 File Offset: 0x00034C60
	private void UpdateLook()
	{
		this.pitch += -this.aimInput.y * this.aimSpeed * Time.unscaledDeltaTime;
		this.pitch = Mathf.Clamp(this.pitch, -89.9f, 89.9f);
		this.yaw += this.aimInput.x * this.aimSpeed * Time.unscaledDeltaTime;
		this.vCam.transform.localRotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x00036AFC File Offset: 0x00034CFC
	private void UpdateFocus()
	{
		if (this.focusInput)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.vCam.transform.position, this.vCam.transform.forward, ref raycastHit, 100f, this.dofLayerMask))
			{
				this.dofTargetPoint = raycastHit.point + this.vCam.transform.forward * -0.2f;
				this.dofTarget.position = this.dofTargetPoint;
			}
			this.focusMeshTimer = this.focusMeshAppearTime;
			if (!this.focusMesh.gameObject.activeSelf)
			{
				this.focusMesh.gameObject.SetActive(true);
			}
		}
		else if (this.focusMeshTimer > 0f)
		{
			this.focusMeshTimer -= Time.unscaledDeltaTime;
			if (this.focusMeshTimer <= 0f)
			{
				this.focusMeshTimer = 0f;
				this.focusMesh.gameObject.SetActive(false);
			}
		}
		if (this.focusMesh.gameObject.activeSelf)
		{
			this.focusMesh.transform.localScale = Vector3.one * this.focusMeshSize * this.focusMeshTimer / this.focusMeshAppearTime;
		}
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x00036C4C File Offset: 0x00034E4C
	private void UpdateFov()
	{
		float num = this.vCam.m_Lens.FieldOfView;
		num += -this.fovChangeSpeed * this.fovInput;
		num = Mathf.Clamp(num, this.fovRange.x, this.fovRange.y);
		this.vCam.m_Lens.FieldOfView = num;
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x00036CAC File Offset: 0x00034EAC
	private void OnDestroy()
	{
		CameraMode.OnCameraModeActivated = (Action)Delegate.Remove(CameraMode.OnCameraModeActivated, new Action(this.OnCameraModeActivated));
		CameraMode.OnCameraModeDeactivated = (Action)Delegate.Remove(CameraMode.OnCameraModeDeactivated, new Action(this.OnCameraModeDeactivated));
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x00036CFC File Offset: 0x00034EFC
	private void OnCameraModeActivated()
	{
		GameCamera instance = GameCamera.Instance;
		if (instance != null)
		{
			CameraArm mianCameraArm = instance.mianCameraArm;
			this.yaw = mianCameraArm.yaw;
			this.pitch = mianCameraArm.pitch;
			this.vCam.transform.position = instance.renderCamera.transform.position;
			this.dofTargetPoint = instance.target.transform.position;
			this.actived = true;
			this.vCam.m_Lens.FieldOfView = instance.renderCamera.fieldOfView;
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x00036D9E File Offset: 0x00034F9E
	public static void OpenFolder()
	{
		GUIUtility.systemCopyBuffer = CameraModeController.filePath;
		NotificationText.Push(CameraModeController.filePath ?? "");
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x00036DBD File Offset: 0x00034FBD
	private void OnCameraModeDeactivated()
	{
		this.actived = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x00036DD4 File Offset: 0x00034FD4
	private UniTaskVoid Shot()
	{
		CameraModeController.<Shot>d__44 <Shot>d__;
		<Shot>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<Shot>d__.<>4__this = this;
		<Shot>d__.<>1__state = -1;
		<Shot>d__.<>t__builder.Start<CameraModeController.<Shot>d__44>(ref <Shot>d__);
		return <Shot>d__.<>t__builder.Task;
	}

	// Token: 0x04000B21 RID: 2849
	public CinemachineVirtualCamera vCam;

	// Token: 0x04000B22 RID: 2850
	private bool actived;

	// Token: 0x04000B23 RID: 2851
	public Transform dofTarget;

	// Token: 0x04000B24 RID: 2852
	private Vector3 dofTargetPoint;

	// Token: 0x04000B25 RID: 2853
	public InputActionAsset inputActionAsset;

	// Token: 0x04000B26 RID: 2854
	public LayerMask dofLayerMask;

	// Token: 0x04000B27 RID: 2855
	private Vector2 moveInput;

	// Token: 0x04000B28 RID: 2856
	private float upDownInput;

	// Token: 0x04000B29 RID: 2857
	private bool focusInput;

	// Token: 0x04000B2A RID: 2858
	private bool captureInput;

	// Token: 0x04000B2B RID: 2859
	private bool fastInput;

	// Token: 0x04000B2C RID: 2860
	private bool openFolderInput;

	// Token: 0x04000B2D RID: 2861
	public GameObject focusMesh;

	// Token: 0x04000B2E RID: 2862
	public float focusMeshSize = 0.3f;

	// Token: 0x04000B2F RID: 2863
	private float focusMeshCurrentSize = 0.3f;

	// Token: 0x04000B30 RID: 2864
	public float focusMeshAppearTime = 1f;

	// Token: 0x04000B31 RID: 2865
	private float focusMeshTimer = 0.3f;

	// Token: 0x04000B32 RID: 2866
	private float fovInput;

	// Token: 0x04000B33 RID: 2867
	private Vector2 aimInput;

	// Token: 0x04000B34 RID: 2868
	public float moveSpeed;

	// Token: 0x04000B35 RID: 2869
	public float fastMoveSpeed;

	// Token: 0x04000B36 RID: 2870
	public float aimSpeed;

	// Token: 0x04000B37 RID: 2871
	private float yaw;

	// Token: 0x04000B38 RID: 2872
	private float pitch;

	// Token: 0x04000B39 RID: 2873
	private bool shootting;

	// Token: 0x04000B3A RID: 2874
	public ColorPunch colorPunch;

	// Token: 0x04000B3B RID: 2875
	public Vector2 fovRange = new Vector2(5f, 60f);

	// Token: 0x04000B3C RID: 2876
	[Range(0.01f, 0.5f)]
	public float fovChangeSpeed = 10f;

	// Token: 0x04000B3D RID: 2877
	public CanvasGroup indicatorGroup;

	// Token: 0x04000B3E RID: 2878
	public UnityEvent OnCapturedEvent;
}
