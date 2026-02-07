using System;
using UnityEngine;

namespace ECM2.Examples.ThirdPerson
{
	// Token: 0x0200007F RID: 127
	public class ThirdPersonController : MonoBehaviour
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x00010B9C File Offset: 0x0000ED9C
		public virtual void AddControlYawInput(float value)
		{
			this._cameraYaw = MathLib.ClampAngle(this._cameraYaw + value, -180f, 180f);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00010BBB File Offset: 0x0000EDBB
		public virtual void AddControlPitchInput(float value, float minValue = -80f, float maxValue = 80f)
		{
			this._cameraPitch = MathLib.ClampAngle(this._cameraPitch + value, minValue, maxValue);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00010BD2 File Offset: 0x0000EDD2
		public virtual void AddControlZoomInput(float value)
		{
			this.followDistance = Mathf.Clamp(this.followDistance - value, this.followMinDistance, this.followMaxDistance);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00010BF3 File Offset: 0x0000EDF3
		protected virtual void UpdateCameraRotation()
		{
			this._character.cameraTransform.rotation = Quaternion.Euler(this._cameraPitch, this._cameraYaw, 0f);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00010C1C File Offset: 0x0000EE1C
		protected virtual void UpdateCameraPosition()
		{
			Transform cameraTransform = this._character.cameraTransform;
			this._currentFollowDistance = Mathf.SmoothDamp(this._currentFollowDistance, this.followDistance, ref this._followDistanceSmoothVelocity, 0.1f);
			cameraTransform.position = this.followTarget.transform.position - cameraTransform.forward * this._currentFollowDistance;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00010C83 File Offset: 0x0000EE83
		protected virtual void UpdateCamera()
		{
			this.UpdateCameraRotation();
			this.UpdateCameraPosition();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00010C91 File Offset: 0x0000EE91
		protected virtual void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00010CA0 File Offset: 0x0000EEA0
		protected virtual void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Vector3 eulerAngles = this._character.cameraTransform.eulerAngles;
			this._cameraPitch = eulerAngles.x;
			this._cameraYaw = eulerAngles.y;
			this._currentFollowDistance = this.followDistance;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00010CE8 File Offset: 0x0000EEE8
		protected virtual void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = Input.GetAxisRaw("Vertical")
			};
			Vector3 vector2 = Vector3.zero;
			vector2 += Vector3.right * vector.x;
			vector2 += Vector3.forward * vector.y;
			if (this._character.cameraTransform)
			{
				vector2 = vector2.relativeTo(this._character.cameraTransform, this._character.GetUpVector(), true);
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
			vector3 *= this.mouseSensitivity;
			this.AddControlYawInput(vector3.x);
			this.AddControlPitchInput(this.invertLook ? (-vector3.y) : vector3.y, this.minPitch, this.maxPitch);
			float axisRaw = Input.GetAxisRaw("Mouse ScrollWheel");
			this.AddControlZoomInput(axisRaw);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00010E85 File Offset: 0x0000F085
		protected virtual void LateUpdate()
		{
			this.UpdateCamera();
		}

		// Token: 0x0400028F RID: 655
		[Space(15f)]
		public GameObject followTarget;

		// Token: 0x04000290 RID: 656
		[Tooltip("The default distance behind the Follow target.")]
		[SerializeField]
		public float followDistance = 5f;

		// Token: 0x04000291 RID: 657
		[Tooltip("The minimum distance to Follow target.")]
		[SerializeField]
		public float followMinDistance;

		// Token: 0x04000292 RID: 658
		[Tooltip("The maximum distance to Follow target.")]
		[SerializeField]
		public float followMaxDistance = 10f;

		// Token: 0x04000293 RID: 659
		[Space(15f)]
		public bool invertLook = true;

		// Token: 0x04000294 RID: 660
		[Tooltip("Mouse look sensitivity")]
		public Vector2 mouseSensitivity = new Vector2(1f, 1f);

		// Token: 0x04000295 RID: 661
		[Space(15f)]
		[Tooltip("How far in degrees can you move the camera down.")]
		public float minPitch = -80f;

		// Token: 0x04000296 RID: 662
		[Tooltip("How far in degrees can you move the camera up.")]
		public float maxPitch = 80f;

		// Token: 0x04000297 RID: 663
		protected float _cameraYaw;

		// Token: 0x04000298 RID: 664
		protected float _cameraPitch;

		// Token: 0x04000299 RID: 665
		protected float _currentFollowDistance;

		// Token: 0x0400029A RID: 666
		protected float _followDistanceSmoothVelocity;

		// Token: 0x0400029B RID: 667
		protected Character _character;
	}
}
