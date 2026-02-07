using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex91
{
	// Token: 0x02000065 RID: 101
	public class FirstPersonController : MonoBehaviour
	{
		// Token: 0x06000354 RID: 852 RVA: 0x0000ED2E File Offset: 0x0000CF2E
		public void AddControlYawInput(float value)
		{
			this._character.AddYawInput(value);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000ED3C File Offset: 0x0000CF3C
		public void AddControlPitchInput(float value, float minValue = -80f, float maxValue = 80f)
		{
			if (value == 0f)
			{
				return;
			}
			this._cameraTargetPitch = MathLib.ClampAngle(this._cameraTargetPitch + value, minValue, maxValue);
			this.cameraTarget.transform.localRotation = Quaternion.Euler(-this._cameraTargetPitch, 0f, 0f);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000ED8D File Offset: 0x0000CF8D
		private void OnCrouched()
		{
			this.crouchedCamera.SetActive(true);
			this.unCrouchedCamera.SetActive(false);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000EDA7 File Offset: 0x0000CFA7
		private void OnUnCrouched()
		{
			this.crouchedCamera.SetActive(false);
			this.unCrouchedCamera.SetActive(true);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000EDC1 File Offset: 0x0000CFC1
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000EDCF File Offset: 0x0000CFCF
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			this._character.SetRotationMode(Character.RotationMode.None);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000EDE3 File Offset: 0x0000CFE3
		private void OnEnable()
		{
			this._character.Crouched += this.OnCrouched;
			this._character.UnCrouched += this.OnUnCrouched;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000EE13 File Offset: 0x0000D013
		private void OnDisable()
		{
			this._character.Crouched -= this.OnCrouched;
			this._character.UnCrouched -= this.OnUnCrouched;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000EE44 File Offset: 0x0000D044
		private void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = Input.GetAxisRaw("Vertical")
			};
			Vector3 vector2 = Vector3.zero;
			vector2 += this._character.GetRightVector() * vector.x;
			vector2 += this._character.GetForwardVector() * vector.y;
			this._character.SetMovementDirection(vector2);
			Vector2 vector3 = new Vector2
			{
				x = Input.GetAxisRaw("Mouse X"),
				y = Input.GetAxisRaw("Mouse Y")
			};
			this.AddControlYawInput(vector3.x * this.lookSensitivity.x);
			this.AddControlPitchInput(vector3.y * this.lookSensitivity.y, this.minPitch, this.maxPitch);
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
				return;
			}
			if (Input.GetButtonUp("Jump"))
			{
				this._character.StopJumping();
			}
		}

		// Token: 0x0400024E RID: 590
		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow.")]
		public GameObject cameraTarget;

		// Token: 0x0400024F RID: 591
		[Tooltip("How far in degrees can you move the camera up.")]
		public float maxPitch = 80f;

		// Token: 0x04000250 RID: 592
		[Tooltip("How far in degrees can you move the camera down.")]
		public float minPitch = -80f;

		// Token: 0x04000251 RID: 593
		[Space(15f)]
		[Tooltip("Cinemachine Virtual Camera positioned at desired crouched height.")]
		public GameObject crouchedCamera;

		// Token: 0x04000252 RID: 594
		[Tooltip("Cinemachine Virtual Camera positioned at desired un-crouched height.")]
		public GameObject unCrouchedCamera;

		// Token: 0x04000253 RID: 595
		[Space(15f)]
		[Tooltip("Mouse look sensitivity")]
		public Vector2 lookSensitivity = new Vector2(1.5f, 1.25f);

		// Token: 0x04000254 RID: 596
		private Character _character;

		// Token: 0x04000255 RID: 597
		private float _cameraTargetPitch;
	}
}
