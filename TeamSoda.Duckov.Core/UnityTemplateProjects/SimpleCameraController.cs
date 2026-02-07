using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityTemplateProjects
{
	// Token: 0x0200022C RID: 556
	public class SimpleCameraController : MonoBehaviour
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x00041F9C File Offset: 0x0004019C
		private void Start()
		{
			InputActionMap inputActionMap = new InputActionMap("Simple Camera Controller");
			this.lookAction = InputActionSetupExtensions.AddAction(inputActionMap, "look", 0, "<Mouse>/delta", null, null, null, null);
			this.movementAction = InputActionSetupExtensions.AddAction(inputActionMap, "move", 0, "<Gamepad>/leftStick", null, null, null, null);
			this.verticalMovementAction = InputActionSetupExtensions.AddAction(inputActionMap, "Vertical Movement", 0, null, null, null, null, null);
			this.boostFactorAction = InputActionSetupExtensions.AddAction(inputActionMap, "Boost Factor", 0, "<Mouse>/scroll", null, null, null, null);
			InputActionSetupExtensions.AddBinding(this.lookAction, "<Gamepad>/rightStick", null, null, null).WithProcessor("scaleVector2(x=15, y=15)");
			InputActionSetupExtensions.AddCompositeBinding(this.movementAction, "Dpad", null, null).With("Up", "<Keyboard>/w", null, null).With("Up", "<Keyboard>/upArrow", null, null).With("Down", "<Keyboard>/s", null, null).With("Down", "<Keyboard>/downArrow", null, null).With("Left", "<Keyboard>/a", null, null).With("Left", "<Keyboard>/leftArrow", null, null).With("Right", "<Keyboard>/d", null, null).With("Right", "<Keyboard>/rightArrow", null, null);
			InputActionSetupExtensions.AddCompositeBinding(this.verticalMovementAction, "Dpad", null, null).With("Up", "<Keyboard>/pageUp", null, null).With("Down", "<Keyboard>/pageDown", null, null).With("Up", "<Keyboard>/e", null, null).With("Down", "<Keyboard>/q", null, null).With("Up", "<Gamepad>/rightshoulder", null, null).With("Down", "<Gamepad>/leftshoulder", null, null);
			InputActionSetupExtensions.AddBinding(this.boostFactorAction, "<Gamepad>/Dpad", null, null, null).WithProcessor("scaleVector2(x=1, y=4)");
			this.movementAction.Enable();
			this.lookAction.Enable();
			this.verticalMovementAction.Enable();
			this.boostFactorAction.Enable();
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x000421C8 File Offset: 0x000403C8
		private void OnEnable()
		{
			this.m_TargetCameraState.SetFromTransform(base.transform);
			this.m_InterpolatingCameraState.SetFromTransform(base.transform);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x000421EC File Offset: 0x000403EC
		private Vector3 GetInputTranslationDirection()
		{
			Vector3 zero = Vector3.zero;
			Vector2 vector = this.movementAction.ReadValue<Vector2>();
			zero.x = vector.x;
			zero.z = vector.y;
			zero.y = this.verticalMovementAction.ReadValue<Vector2>().y;
			return zero;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00042240 File Offset: 0x00040440
		private void Update()
		{
			if (this.IsEscapePressed())
			{
				Application.Quit();
			}
			if (this.IsRightMouseButtonDown())
			{
				Cursor.lockState = 1;
			}
			if (this.IsRightMouseButtonUp())
			{
				Cursor.visible = true;
				Cursor.lockState = 0;
			}
			if (this.IsCameraRotationAllowed())
			{
				Vector2 vector = this.GetInputLookRotation() * Time.deltaTime * 5f;
				if (this.invertY)
				{
					vector.y = -vector.y;
				}
				float num = this.mouseSensitivityCurve.Evaluate(vector.magnitude);
				this.m_TargetCameraState.yaw += vector.x * num;
				this.m_TargetCameraState.pitch += vector.y * num;
			}
			Vector3 vector2 = this.GetInputTranslationDirection() * Time.deltaTime;
			if (this.IsBoostPressed())
			{
				vector2 *= 10f;
			}
			this.boost += this.GetBoostFactor();
			vector2 *= Mathf.Pow(2f, this.boost);
			this.m_TargetCameraState.Translate(vector2);
			float positionLerpPct = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.positionLerpTime * Time.deltaTime);
			float rotationLerpPct = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.rotationLerpTime * Time.deltaTime);
			this.m_InterpolatingCameraState.LerpTowards(this.m_TargetCameraState, positionLerpPct, rotationLerpPct);
			this.m_InterpolatingCameraState.UpdateTransform(base.transform);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x000423C4 File Offset: 0x000405C4
		private float GetBoostFactor()
		{
			return this.boostFactorAction.ReadValue<Vector2>().y * 0.01f;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x000423DC File Offset: 0x000405DC
		private Vector2 GetInputLookRotation()
		{
			return this.lookAction.ReadValue<Vector2>();
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000423E9 File Offset: 0x000405E9
		private bool IsBoostPressed()
		{
			return (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed) | (Gamepad.current != null && Gamepad.current.xButton.isPressed);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0004241E File Offset: 0x0004061E
		private bool IsEscapePressed()
		{
			return Keyboard.current != null && Keyboard.current.escapeKey.isPressed;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00042438 File Offset: 0x00040638
		private bool IsCameraRotationAllowed()
		{
			return (Mouse.current != null && Mouse.current.rightButton.isPressed) | (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0f);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00042487 File Offset: 0x00040687
		private bool IsRightMouseButtonDown()
		{
			return Mouse.current != null && Mouse.current.rightButton.isPressed;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x000424A1 File Offset: 0x000406A1
		private bool IsRightMouseButtonUp()
		{
			return Mouse.current != null && !Mouse.current.rightButton.isPressed;
		}

		// Token: 0x04000D85 RID: 3461
		private SimpleCameraController.CameraState m_TargetCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000D86 RID: 3462
		private SimpleCameraController.CameraState m_InterpolatingCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000D87 RID: 3463
		[Header("Movement Settings")]
		[Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
		public float boost = 3.5f;

		// Token: 0x04000D88 RID: 3464
		[Tooltip("Time it takes to interpolate camera position 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float positionLerpTime = 0.2f;

		// Token: 0x04000D89 RID: 3465
		[Header("Rotation Settings")]
		[Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
		public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f, 0f, 5f),
			new Keyframe(1f, 2.5f, 0f, 0f)
		});

		// Token: 0x04000D8A RID: 3466
		[Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float rotationLerpTime = 0.01f;

		// Token: 0x04000D8B RID: 3467
		[Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
		public bool invertY;

		// Token: 0x04000D8C RID: 3468
		private InputAction movementAction;

		// Token: 0x04000D8D RID: 3469
		private InputAction verticalMovementAction;

		// Token: 0x04000D8E RID: 3470
		private InputAction lookAction;

		// Token: 0x04000D8F RID: 3471
		private InputAction boostFactorAction;

		// Token: 0x04000D90 RID: 3472
		private bool mouseRightButtonPressed;

		// Token: 0x02000527 RID: 1319
		private class CameraState
		{
			// Token: 0x0600289E RID: 10398 RVA: 0x00095148 File Offset: 0x00093348
			public void SetFromTransform(Transform t)
			{
				this.pitch = t.eulerAngles.x;
				this.yaw = t.eulerAngles.y;
				this.roll = t.eulerAngles.z;
				this.x = t.position.x;
				this.y = t.position.y;
				this.z = t.position.z;
			}

			// Token: 0x0600289F RID: 10399 RVA: 0x000951BC File Offset: 0x000933BC
			public void Translate(Vector3 translation)
			{
				Vector3 vector = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
				this.x += vector.x;
				this.y += vector.y;
				this.z += vector.z;
			}

			// Token: 0x060028A0 RID: 10400 RVA: 0x00095220 File Offset: 0x00093420
			public void LerpTowards(SimpleCameraController.CameraState target, float positionLerpPct, float rotationLerpPct)
			{
				this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
				this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
				this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
				this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
				this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
				this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
			}

			// Token: 0x060028A1 RID: 10401 RVA: 0x000952BD File Offset: 0x000934BD
			public void UpdateTransform(Transform t)
			{
				t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
				t.position = new Vector3(this.x, this.y, this.z);
			}

			// Token: 0x04001EA7 RID: 7847
			public float yaw;

			// Token: 0x04001EA8 RID: 7848
			public float pitch;

			// Token: 0x04001EA9 RID: 7849
			public float roll;

			// Token: 0x04001EAA RID: 7850
			public float x;

			// Token: 0x04001EAB RID: 7851
			public float y;

			// Token: 0x04001EAC RID: 7852
			public float z;
		}
	}
}
