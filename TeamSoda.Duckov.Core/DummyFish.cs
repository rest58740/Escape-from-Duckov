using System;
using Duckov.Aquariums;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class DummyFish : MonoBehaviour, IAquariumContent
{
	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00035336 File Offset: 0x00033536
	private Vector3 TargetPosition
	{
		get
		{
			return this.target.position;
		}
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00035343 File Offset: 0x00033543
	private void Awake()
	{
		this.rigidbody.useGravity = false;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00035351 File Offset: 0x00033551
	public void Setup(Aquarium master)
	{
		this.master = master;
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x0003535C File Offset: 0x0003355C
	private void FixedUpdate()
	{
		Vector3 up = Vector3.up;
		Vector3 forward = base.transform.forward;
		Vector3 right = base.transform.right;
		Vector3 vector = this.TargetPosition - this.rigidbody.position;
		Vector3 normalized = vector.normalized;
		Vector3 vector2 = Vector3.Cross(up, normalized);
		float num = Vector3.Dot(normalized, forward);
		float num2 = Mathf.Max(0f, num);
		this.swim = ((vector.magnitude > this.deadZone) ? 1f : (vector.magnitude / this.deadZone)) * num2;
		Vector3 vector3 = -(Vector3.Dot(vector2, this.rigidbody.velocity) * vector2);
		this.rigidbody.velocity += forward * this.swimForce * this.swim * Time.deltaTime + vector3 * 0.5f;
		this.rigidbody.angularVelocity = Vector3.zero;
		Vector3 vector4 = vector;
		vector4.y = 0f;
		float num3 = Mathf.Clamp01(vector4.magnitude / this.deadZone - 0.5f);
		Vector3 normalized2 = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
		this._debug_projectedForward = normalized2;
		Vector3 vector5 = Vector3.Lerp(normalized2, normalized, num3);
		this._debug_idealRotForward = vector5;
		float num4 = Vector3.SignedAngle(forward, vector5, right);
		float num5 = Vector3.SignedAngle(forward, vector5, Vector3.up);
		float num6 = this.rotateForce * num4;
		float num7 = this.rotateForce * num5;
		this.rotVelocityX += num6 * Time.fixedDeltaTime;
		this.rotVelocityY += num7 * Time.fixedDeltaTime * num3;
		this.rotVelocityX *= 1f - this.rotationDamping;
		this.rotVelocityY *= 1f - this.rotationDamping;
		Vector3 eulerAngles = this.rigidbody.rotation.eulerAngles;
		eulerAngles.y += this.rotVelocityY * Time.deltaTime;
		eulerAngles.x += this.rotVelocityX * Time.deltaTime;
		if (eulerAngles.x < -179f)
		{
			eulerAngles.x += 360f;
		}
		if (eulerAngles.x > 179f)
		{
			eulerAngles.x -= 360f;
		}
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, -45f, 45f);
		eulerAngles.z = 0f;
		Quaternion quaternion = Quaternion.Euler(eulerAngles);
		this.rigidbody.MoveRotation(quaternion);
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00035614 File Offset: 0x00033814
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(base.transform.position, base.transform.position + this._debug_idealRotForward);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(base.transform.position, base.transform.position + this._debug_projectedForward);
	}

	// Token: 0x04000AE1 RID: 2785
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x04000AE2 RID: 2786
	[SerializeField]
	private float rotateForce = 10f;

	// Token: 0x04000AE3 RID: 2787
	[SerializeField]
	private float swimForce = 10f;

	// Token: 0x04000AE4 RID: 2788
	[SerializeField]
	private float deadZone = 2f;

	// Token: 0x04000AE5 RID: 2789
	[SerializeField]
	private float rotationDamping = 0.1f;

	// Token: 0x04000AE6 RID: 2790
	[Header("Control")]
	[SerializeField]
	private Transform target;

	// Token: 0x04000AE7 RID: 2791
	[Range(0f, 1f)]
	[SerializeField]
	private float swim;

	// Token: 0x04000AE8 RID: 2792
	private float rotVelocityX;

	// Token: 0x04000AE9 RID: 2793
	private float rotVelocityY;

	// Token: 0x04000AEA RID: 2794
	private Aquarium master;

	// Token: 0x04000AEB RID: 2795
	private Vector3 _debug_idealRotForward;

	// Token: 0x04000AEC RID: 2796
	private Vector3 _debug_projectedForward;
}
