using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Shapes
{
	// Token: 0x02000008 RID: 8
	[ExecuteAlways]
	public class FpsController : ImmediateModeShapeDrawer
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002E58 File Offset: 0x00001058
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.InputFocus = true;
			base.StartCoroutine(this.FixedSteps());
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002E78 File Offset: 0x00001078
		public override void DrawShapes(Camera cam)
		{
			if (cam != this.cam)
			{
				return;
			}
			using (Draw.Command(cam, RenderPassEvent.BeforeRenderingPostProcessing))
			{
				Draw.ZTest = CompareFunction.Always;
				Draw.Matrix = this.crosshairTransform.localToWorldMatrix;
				Draw.BlendMode = 1;
				Draw.LineGeometry = 0;
				this.crosshair.DrawCrosshair();
				float barRadius = this.ammoBarRadius + this.fireSidebarRadiusPunchAmount * this.crosshair.fireDecayer.value;
				this.ammoBar.DrawBar(this, barRadius);
				this.chargeBar.DrawBar(this, barRadius);
				this.compass.DrawCompass(this.head.transform.forward);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002F40 File Offset: 0x00001140
		private IEnumerator FixedSteps()
		{
			for (;;)
			{
				this.FixedUpdateManual();
				yield return new WaitForSeconds(0.01f);
			}
			yield break;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002F50 File Offset: 0x00001150
		public static void DrawRoundedArcOutline(Vector2 origin, float radius, float thickness, float outlineThickness, float angStart, float angEnd)
		{
			float num = radius - thickness / 2f;
			float num2 = radius + thickness / 2f;
			Draw.Arc(origin, num, outlineThickness, angStart - 0.01f, angEnd + 0.01f);
			Draw.Arc(origin, num2, outlineThickness, angStart - 0.01f, angEnd + 0.01f);
			Vector2 v = origin + ShapesMath.AngToDir(angStart) * radius;
			Vector2 v2 = origin + ShapesMath.AngToDir(angEnd) * radius;
			Draw.Arc(v, thickness / 2f, outlineThickness, angStart, angStart - 3.1415927f);
			Draw.Arc(v2, thickness / 2f, outlineThickness, angEnd, angEnd + 3.1415927f);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000300C File Offset: 0x0000120C
		public Vector2 GetShake(float speed, float amp)
		{
			float time = ShapesMath.Frac(Time.time * speed);
			float x = this.shakeAnimX.Evaluate(time);
			float y = this.shakeAnimY.Evaluate(time);
			return new Vector2(x, y) * amp;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000304B File Offset: 0x0000124B
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00003055 File Offset: 0x00001255
		private bool InputFocus
		{
			get
			{
				return !Cursor.visible;
			}
			set
			{
				Cursor.lockState = (value ? CursorLockMode.Locked : CursorLockMode.None);
				Cursor.visible = !value;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000306C File Offset: 0x0000126C
		private void FixedUpdateManual()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.InputFocus)
			{
				Vector3 right = this.head.right;
				Vector3 forward = this.head.forward;
				forward.y = 0f;
				this.moveVel += (this.moveInput.y * forward + this.moveInput.x * right) * (Time.fixedDeltaTime * this.moveSpeed);
			}
			base.transform.position += this.moveVel * Time.deltaTime;
			this.moveVel *= this.smoof;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003134 File Offset: 0x00001334
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.crosshair.UpdateCrosshairDecay();
			this.chargeBar.UpdateCharge();
			if (this.InputFocus)
			{
				this.yaw += Input.GetAxis("Mouse X") * this.lookSensitivity;
				this.pitch -= Input.GetAxis("Mouse Y") * this.lookSensitivity;
				this.pitch = Mathf.Clamp(this.pitch, -90f, 90f);
				this.head.localRotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
				this.chargeBar.isCharging = Input.GetMouseButton(1);
				if (Input.GetKey(KeyCode.R))
				{
					this.ammoBar.Reload();
				}
				if (Input.GetMouseButtonDown(0) && this.ammoBar.HasBulletsLeft)
				{
					this.ammoBar.Fire();
					this.crosshair.Fire();
					RaycastHit raycastHit;
					if (Physics.Raycast(new Ray(this.head.position, this.head.forward), out raycastHit) && raycastHit.collider.gameObject.name == "Enemy")
					{
						this.crosshair.FireHit();
					}
				}
				this.moveInput = Vector2.zero;
				this.<Update>g__DoInput|30_0(KeyCode.W, Vector2.up);
				this.<Update>g__DoInput|30_0(KeyCode.S, Vector2.down);
				this.<Update>g__DoInput|30_0(KeyCode.D, Vector2.right);
				this.<Update>g__DoInput|30_0(KeyCode.A, Vector2.left);
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.InputFocus = false;
					return;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.InputFocus = true;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003374 File Offset: 0x00001574
		[CompilerGenerated]
		private void <Update>g__DoInput|30_0(KeyCode key, Vector2 dir)
		{
			if (Input.GetKey(key))
			{
				this.moveInput += dir;
			}
		}

		// Token: 0x0400003A RID: 58
		public Transform head;

		// Token: 0x0400003B RID: 59
		public Camera cam;

		// Token: 0x0400003C RID: 60
		public Crosshair crosshair;

		// Token: 0x0400003D RID: 61
		public ChargeBar chargeBar;

		// Token: 0x0400003E RID: 62
		public AmmoBar ammoBar;

		// Token: 0x0400003F RID: 63
		public Compass compass;

		// Token: 0x04000040 RID: 64
		public Transform crosshairTransform;

		// Token: 0x04000041 RID: 65
		[Header("Player Movement")]
		[Range(0.8f, 1f)]
		public float smoof = 0.99f;

		// Token: 0x04000042 RID: 66
		public float moveSpeed = 1f;

		// Token: 0x04000043 RID: 67
		public float lookSensitivity = 1f;

		// Token: 0x04000044 RID: 68
		private float yaw;

		// Token: 0x04000045 RID: 69
		private float pitch;

		// Token: 0x04000046 RID: 70
		private Vector2 moveInput = Vector2.zero;

		// Token: 0x04000047 RID: 71
		private Vector3 moveVel = Vector3.zero;

		// Token: 0x04000048 RID: 72
		[Header("Sidebar Style")]
		[Range(0f, 3.1415927f)]
		public float ammoBarAngularSpanRad;

		// Token: 0x04000049 RID: 73
		[Range(0f, 0.05f)]
		public float ammoBarOutlineThickness = 0.1f;

		// Token: 0x0400004A RID: 74
		[Range(0f, 0.2f)]
		public float ammoBarThickness;

		// Token: 0x0400004B RID: 75
		[Range(0f, 0.2f)]
		public float ammoBarRadius;

		// Token: 0x0400004C RID: 76
		[Header("Animation")]
		[Range(0f, 0.3f)]
		public float fireSidebarRadiusPunchAmount = 0.1f;

		// Token: 0x0400004D RID: 77
		public AnimationCurve shakeAnimX = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x0400004E RID: 78
		public AnimationCurve shakeAnimY = AnimationCurve.Constant(0f, 1f, 0f);
	}
}
