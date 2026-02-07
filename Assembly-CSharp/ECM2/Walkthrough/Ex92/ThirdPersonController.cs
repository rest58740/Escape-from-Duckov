using System;
using Cinemachine;
using UnityEngine;

namespace ECM2.Walkthrough.Ex92
{
	// Token: 0x02000064 RID: 100
	public class ThirdPersonController : MonoBehaviour
	{
		// Token: 0x0600034B RID: 843 RVA: 0x0000EA17 File Offset: 0x0000CC17
		public void AddControlYawInput(float value, float minValue = -180f, float maxValue = 180f)
		{
			if (value != 0f)
			{
				this._cameraTargetYaw = MathLib.ClampAngle(this._cameraTargetYaw + value, minValue, maxValue);
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000EA36 File Offset: 0x0000CC36
		public void AddControlPitchInput(float value, float minValue = -80f, float maxValue = 80f)
		{
			if (value == 0f)
			{
				return;
			}
			if (this.invertLook)
			{
				value = -value;
			}
			this._cameraTargetPitch = MathLib.ClampAngle(this._cameraTargetPitch + value, minValue, maxValue);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000EA62 File Offset: 0x0000CC62
		public virtual void AddControlZoomInput(float value)
		{
			this.followDistance = Mathf.Clamp(this.followDistance - value, this.followMinDistance, this.followMaxDistance);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000EA84 File Offset: 0x0000CC84
		private void UpdateCamera()
		{
			this.followTarget.transform.rotation = Quaternion.Euler(this._cameraTargetPitch, this._cameraTargetYaw, 0f);
			this._cmThirdPersonFollow.CameraDistance = Mathf.SmoothDamp(this._cmThirdPersonFollow.CameraDistance, this.followDistance, ref this._followDistanceSmoothVelocity, 0.1f);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000EAE3 File Offset: 0x0000CCE3
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
			this._cmThirdPersonFollow = this.followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
			if (this._cmThirdPersonFollow)
			{
				this._cmThirdPersonFollow.CameraDistance = this.followDistance;
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000EB20 File Offset: 0x0000CD20
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000EB28 File Offset: 0x0000CD28
		private void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = Input.GetAxisRaw("Vertical")
			};
			Vector3 vector2 = Vector3.zero;
			vector2 += Vector3.right * vector.x;
			vector2 += Vector3.forward * vector.y;
			if (this._character.camera)
			{
				vector2 = vector2.relativeTo(this._character.cameraTransform, true);
			}
			this._character.SetMovementDirection(vector2);
			if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
			{
				this._character.Crouch();
			}
			else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
			{
				this._character.UnCrouch();
			}
			if (Input.GetButtonDown("Jump"))
			{
				this._character.Jump();
			}
			else if (Input.GetButtonUp("Jump"))
			{
				this._character.StopJumping();
			}
			Vector2 vector3 = new Vector2
			{
				x = Input.GetAxisRaw("Mouse X"),
				y = Input.GetAxisRaw("Mouse Y")
			};
			this.AddControlYawInput(vector3.x * this.lookSensitivity.x, -180f, 180f);
			this.AddControlPitchInput(vector3.y * this.lookSensitivity.y, this.minPitch, this.maxPitch);
			float axisRaw = Input.GetAxisRaw("Mouse ScrollWheel");
			this.AddControlZoomInput(axisRaw);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000ECBE File Offset: 0x0000CEBE
		private void LateUpdate()
		{
			this.UpdateCamera();
		}

		// Token: 0x04000240 RID: 576
		[Header("Cinemachine")]
		[Tooltip("The CM virtual Camera following the target.")]
		public CinemachineVirtualCamera followCamera;

		// Token: 0x04000241 RID: 577
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow.")]
		public GameObject followTarget;

		// Token: 0x04000242 RID: 578
		[Tooltip("The default distance behind the Follow target.")]
		[SerializeField]
		public float followDistance = 5f;

		// Token: 0x04000243 RID: 579
		[Tooltip("The minimum distance to Follow target.")]
		[SerializeField]
		public float followMinDistance = 2f;

		// Token: 0x04000244 RID: 580
		[Tooltip("The maximum distance to Follow target.")]
		[SerializeField]
		public float followMaxDistance = 10f;

		// Token: 0x04000245 RID: 581
		[Tooltip("How far in degrees can you move the camera up.")]
		public float maxPitch = 80f;

		// Token: 0x04000246 RID: 582
		[Tooltip("How far in degrees can you move the camera down.")]
		public float minPitch = -80f;

		// Token: 0x04000247 RID: 583
		[Space(15f)]
		public bool invertLook = true;

		// Token: 0x04000248 RID: 584
		[Tooltip("Mouse look sensitivity")]
		public Vector2 lookSensitivity = new Vector2(1.5f, 1.25f);

		// Token: 0x04000249 RID: 585
		private Character _character;

		// Token: 0x0400024A RID: 586
		private float _cameraTargetYaw;

		// Token: 0x0400024B RID: 587
		private float _cameraTargetPitch;

		// Token: 0x0400024C RID: 588
		private Cinemachine3rdPersonFollow _cmThirdPersonFollow;

		// Token: 0x0400024D RID: 589
		protected float _followDistanceSmoothVelocity;
	}
}
