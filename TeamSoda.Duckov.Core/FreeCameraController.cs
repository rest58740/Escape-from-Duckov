using System;
using CameraSystems;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001C0 RID: 448
public class FreeCameraController : MonoBehaviour
{
	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000D6F RID: 3439 RVA: 0x000386D0 File Offset: 0x000368D0
	private Gamepad Gamepad
	{
		get
		{
			return Gamepad.current;
		}
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x000386D7 File Offset: 0x000368D7
	private void Awake()
	{
		if (!this.propertiesControl)
		{
			this.propertiesControl = base.GetComponent<CameraPropertiesControl>();
		}
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x000386F2 File Offset: 0x000368F2
	private void OnEnable()
	{
		this.SetRotation(base.transform.rotation);
		this.SnapToMainCamera();
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x0003870C File Offset: 0x0003690C
	public void SetRotation(Quaternion rotation)
	{
		Vector3 eulerAngles = rotation.eulerAngles;
		this.yaw = eulerAngles.y;
		this.pitch = eulerAngles.x;
		this.yawTarget = this.yaw;
		this.pitchTarget = this.pitch;
		if (this.pitch > 180f)
		{
			this.pitch -= 360f;
		}
		if (this.pitch < -180f)
		{
			this.pitch += 360f;
		}
		this.pitch = Mathf.Clamp(this.pitch, -89f, 89f);
		base.transform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x000387CC File Offset: 0x000369CC
	private unsafe void Update()
	{
		if (this.Gamepad == null)
		{
			return;
		}
		bool isPressed = this.Gamepad.rightShoulder.isPressed;
		float num = this.moveSpeed * (float)(isPressed ? 2 : 1);
		CharacterMainControl main = CharacterMainControl.Main;
		Vector2 vector = *this.Gamepad.leftStick.value;
		float num2 = *this.Gamepad.rightTrigger.value - *this.Gamepad.leftTrigger.value;
		Vector3 vector2 = new Vector3(vector.x * num, 0f, vector.y * num) * Time.unscaledDeltaTime;
		Vector3 vector3 = this.projectMovementOnXZPlane ? Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized : base.transform.forward;
		Vector3 vector4 = this.projectMovementOnXZPlane ? Vector3.ProjectOnPlane(base.transform.right, Vector3.up).normalized : base.transform.right;
		Vector3 vector5 = num2 * Vector3.up * num * 0.5f * Time.unscaledDeltaTime;
		Vector3 vector6 = vector3 * vector2.z + vector4 * vector2.x + vector5;
		if (!this.followCharacter || main == null)
		{
			this.worldPosTarget += vector6;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, this.worldPosTarget, ref this.velocityWorldSpace, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
			if (main == null)
			{
				this.followCharacter = false;
			}
		}
		else
		{
			this.offsetFromCharacter += vector6;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, main.transform.position + this.offsetFromCharacter, ref this.velocityLocalSpace, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		}
		Vector3 vector7 = *this.Gamepad.rightStick.value * this.rotateSpeed * this.vCamera.m_Lens.FieldOfView / 60f;
		this.yawTarget += vector7.x * Time.unscaledDeltaTime;
		this.yaw = Mathf.SmoothDamp(this.yaw, this.yawTarget, ref this.yawVelocity, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		this.pitchTarget += -vector7.y * Time.unscaledDeltaTime;
		this.pitch = Mathf.SmoothDamp(this.pitch, this.pitchTarget, ref this.pitchVelocity, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		this.pitch = Mathf.Clamp(this.pitch, -89f, 89f);
		base.transform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
		if (this.Gamepad.buttonNorth.wasPressedThisFrame)
		{
			this.SnapToMainCamera();
		}
		if (this.Gamepad.buttonEast.wasPressedThisFrame)
		{
			this.ToggleFollowTarget();
		}
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x00038B42 File Offset: 0x00036D42
	private void OnDestroy()
	{
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x00038B44 File Offset: 0x00036D44
	private void ToggleFollowTarget()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		this.followCharacter = !this.followCharacter;
		if (this.followCharacter)
		{
			this.offsetFromCharacter = base.transform.position - main.transform.position;
		}
		this.worldPosTarget = base.transform.position;
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x00038BAC File Offset: 0x00036DAC
	private void SnapToMainCamera()
	{
		if (GameCamera.Instance == null)
		{
			return;
		}
		Camera renderCamera = GameCamera.Instance.renderCamera;
		if (renderCamera == null)
		{
			return;
		}
		base.transform.position = renderCamera.transform.position;
		this.worldPosTarget = renderCamera.transform.position;
		this.vCamera.m_Lens.FieldOfView = renderCamera.fieldOfView;
		this.SetRotation(renderCamera.transform.rotation);
		CharacterMainControl main = CharacterMainControl.Main;
		if (main != null && this.followCharacter)
		{
			this.offsetFromCharacter = base.transform.position - main.transform.position;
		}
	}

	// Token: 0x04000BA5 RID: 2981
	[SerializeField]
	private CameraPropertiesControl propertiesControl;

	// Token: 0x04000BA6 RID: 2982
	[SerializeField]
	private float moveSpeed = 10f;

	// Token: 0x04000BA7 RID: 2983
	[SerializeField]
	private float rotateSpeed = 180f;

	// Token: 0x04000BA8 RID: 2984
	[SerializeField]
	private float smoothTime = 2f;

	// Token: 0x04000BA9 RID: 2985
	[SerializeField]
	private Vector2 minMaxXRotation = new Vector2(-89f, 89f);

	// Token: 0x04000BAA RID: 2986
	[SerializeField]
	private bool projectMovementOnXZPlane;

	// Token: 0x04000BAB RID: 2987
	[Range(-180f, 180f)]
	private float yaw;

	// Token: 0x04000BAC RID: 2988
	[Range(-89f, 89f)]
	private float pitch;

	// Token: 0x04000BAD RID: 2989
	[SerializeField]
	private CinemachineVirtualCamera vCamera;

	// Token: 0x04000BAE RID: 2990
	private bool followCharacter;

	// Token: 0x04000BAF RID: 2991
	private Vector3 offsetFromCharacter;

	// Token: 0x04000BB0 RID: 2992
	private Vector3 worldPosTarget;

	// Token: 0x04000BB1 RID: 2993
	private Vector3 velocityWorldSpace;

	// Token: 0x04000BB2 RID: 2994
	private Vector3 velocityLocalSpace;

	// Token: 0x04000BB3 RID: 2995
	private float yawVelocity;

	// Token: 0x04000BB4 RID: 2996
	private float pitchVelocity;

	// Token: 0x04000BB5 RID: 2997
	private float yawTarget;

	// Token: 0x04000BB6 RID: 2998
	private float pitchTarget;
}
