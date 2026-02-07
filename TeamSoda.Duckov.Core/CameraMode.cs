using System;
using Duckov.UI;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class CameraMode : MonoBehaviour
{
	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00036558 File Offset: 0x00034758
	// (set) Token: 0x06000CC9 RID: 3273 RVA: 0x0003655F File Offset: 0x0003475F
	public static CameraMode Instance { get; private set; }

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00036567 File Offset: 0x00034767
	public static bool Active
	{
		get
		{
			return !(CameraMode.Instance == null) && CameraMode.Instance.active;
		}
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x00036584 File Offset: 0x00034784
	private void Awake()
	{
		if (CameraMode.Instance != null)
		{
			Debug.LogError("检测到多个Camera Mode", base.gameObject);
			return;
		}
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		CameraMode.Instance = this;
		UIInputManager.OnToggleCameraMode += this.OnToggleCameraMode;
		UIInputManager.OnCancel += this.OnUICancel;
		ManagedUIElement.onOpen += this.OnViewOpen;
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x000365F8 File Offset: 0x000347F8
	private void OnDestroy()
	{
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		UIInputManager.OnToggleCameraMode -= this.OnToggleCameraMode;
		UIInputManager.OnCancel -= this.OnUICancel;
		ManagedUIElement.onOpen -= this.OnViewOpen;
		Shader.SetGlobalFloat("CameraModeOn", 0f);
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x00036656 File Offset: 0x00034856
	private void OnViewOpen(ManagedUIElement element)
	{
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
		}
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x00036664 File Offset: 0x00034864
	private void OnUICancel(UIInputEventData data)
	{
		if (data.Used)
		{
			return;
		}
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
			data.Use();
		}
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x00036681 File Offset: 0x00034881
	private void OnToggleCameraMode(UIInputEventData data)
	{
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
		}
		else
		{
			CameraMode.Activate();
		}
		data.Use();
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0003669C File Offset: 0x0003489C
	private void MActivate()
	{
		if (View.ActiveView != null)
		{
			return;
		}
		this.active = true;
		Shader.SetGlobalFloat("CameraModeOn", 1f);
		Action onCameraModeActivated = CameraMode.OnCameraModeActivated;
		if (onCameraModeActivated != null)
		{
			onCameraModeActivated();
		}
		Action<bool> onCameraModeChanged = CameraMode.OnCameraModeChanged;
		if (onCameraModeChanged == null)
		{
			return;
		}
		onCameraModeChanged(this.active);
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x000366F2 File Offset: 0x000348F2
	private void MDeactivate()
	{
		this.active = false;
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		Action onCameraModeDeactivated = CameraMode.OnCameraModeDeactivated;
		if (onCameraModeDeactivated != null)
		{
			onCameraModeDeactivated();
		}
		Action<bool> onCameraModeChanged = CameraMode.OnCameraModeChanged;
		if (onCameraModeChanged == null)
		{
			return;
		}
		onCameraModeChanged(this.active);
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0003672F File Offset: 0x0003492F
	public static void Activate()
	{
		if (CameraMode.Instance == null)
		{
			return;
		}
		Shader.SetGlobalFloat("CameraModeOn", 1f);
		CameraMode.Instance.MActivate();
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00036758 File Offset: 0x00034958
	public static void Deactivate()
	{
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		if (CameraMode.Instance == null)
		{
			return;
		}
		CameraMode.Instance.MDeactivate();
	}

	// Token: 0x04000B1D RID: 2845
	public static Action OnCameraModeActivated;

	// Token: 0x04000B1E RID: 2846
	public static Action OnCameraModeDeactivated;

	// Token: 0x04000B1F RID: 2847
	public static Action<bool> OnCameraModeChanged;

	// Token: 0x04000B20 RID: 2848
	private bool active;
}
