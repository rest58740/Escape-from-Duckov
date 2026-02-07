using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x02000004 RID: 4
	public static class Extensions
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00004A66 File Offset: 0x00002C66
		public static int square(this int value)
		{
			return value * value;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00004A6B File Offset: 0x00002C6B
		public static float square(this float value)
		{
			return value * value;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004A70 File Offset: 0x00002C70
		public static bool isZero(this float value)
		{
			return Mathf.Abs(value) < 1E-10f;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004A7F File Offset: 0x00002C7F
		public static Vector3 onlyX(this Vector3 vector3)
		{
			vector3.y = 0f;
			vector3.z = 0f;
			return vector3;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004A9A File Offset: 0x00002C9A
		public static Vector3 onlyY(this Vector3 vector3)
		{
			vector3.x = 0f;
			vector3.z = 0f;
			return vector3;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00004AB5 File Offset: 0x00002CB5
		public static Vector3 onlyZ(this Vector3 vector3)
		{
			vector3.x = 0f;
			vector3.y = 0f;
			return vector3;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public static Vector3 onlyXY(this Vector3 vector3)
		{
			vector3.z = 0f;
			return vector3;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004ADF File Offset: 0x00002CDF
		public static Vector3 onlyXZ(this Vector3 vector3)
		{
			vector3.y = 0f;
			return vector3;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004AEE File Offset: 0x00002CEE
		public static bool isZero(this Vector2 vector2)
		{
			return (double)vector2.sqrMagnitude < 9.99999943962493E-11;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004B03 File Offset: 0x00002D03
		public static bool isZero(this Vector3 vector3)
		{
			return (double)vector3.sqrMagnitude < 9.99999943962493E-11;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004B18 File Offset: 0x00002D18
		public static bool isExceeding(this Vector3 vector3, float magnitude)
		{
			return vector3.sqrMagnitude > magnitude * magnitude * 1.01f;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004B2C File Offset: 0x00002D2C
		public static Vector3 normalized(this Vector3 vector3, out float magnitude)
		{
			magnitude = vector3.magnitude;
			if ((double)magnitude > 9.99999974737875E-06)
			{
				return vector3 / magnitude;
			}
			magnitude = 0f;
			return Vector3.zero;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004B5A File Offset: 0x00002D5A
		public static float dot(this Vector3 vector3, Vector3 otherVector3)
		{
			return Vector3.Dot(vector3, otherVector3);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004B63 File Offset: 0x00002D63
		public static Vector3 projectedOn(this Vector3 thisVector, Vector3 normal)
		{
			return Vector3.Project(thisVector, normal);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004B6C File Offset: 0x00002D6C
		public static Vector3 projectedOnPlane(this Vector3 thisVector, Vector3 planeNormal)
		{
			return Vector3.ProjectOnPlane(thisVector, planeNormal);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00004B75 File Offset: 0x00002D75
		public static Vector3 clampedTo(this Vector3 vector3, float maxLength)
		{
			return Vector3.ClampMagnitude(vector3, maxLength);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00004B80 File Offset: 0x00002D80
		public static Vector3 perpendicularTo(this Vector3 thisVector, Vector3 otherVector)
		{
			return Vector3.Cross(thisVector, otherVector).normalized;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004B9C File Offset: 0x00002D9C
		public static Vector3 tangentTo(this Vector3 thisVector, Vector3 normal, Vector3 up)
		{
			Vector3 otherVector = thisVector.perpendicularTo(up);
			return normal.perpendicularTo(otherVector) * thisVector.magnitude;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00004BC4 File Offset: 0x00002DC4
		public static Vector3 relativeTo(this Vector3 vector3, Transform relativeToThis, bool isPlanar = true)
		{
			Vector3 vector4 = relativeToThis.forward;
			if (isPlanar)
			{
				Vector3 up = Vector3.up;
				vector4 = vector4.projectedOnPlane(up);
				if (vector4.isZero())
				{
					vector4 = Vector3.ProjectOnPlane(relativeToThis.up, up);
				}
			}
			return Quaternion.LookRotation(vector4) * vector3;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00004C0C File Offset: 0x00002E0C
		public static Vector3 relativeTo(this Vector3 vector3, Transform relativeToThis, Vector3 upAxis, bool isPlanar = true)
		{
			Vector3 vector4 = relativeToThis.forward;
			if (isPlanar)
			{
				vector4 = Vector3.ProjectOnPlane(vector4, upAxis);
				if (vector4.isZero())
				{
					vector4 = Vector3.ProjectOnPlane(relativeToThis.up, upAxis);
				}
			}
			return Quaternion.LookRotation(vector4, upAxis) * vector3;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00004C50 File Offset: 0x00002E50
		public static Quaternion clampPitch(this Quaternion quaternion, float minPitchAngle, float maxPitchAngle)
		{
			quaternion.x /= quaternion.w;
			quaternion.y /= quaternion.w;
			quaternion.z /= quaternion.w;
			quaternion.w = 1f;
			float num = Mathf.Clamp(114.59156f * Mathf.Atan(quaternion.x), minPitchAngle, maxPitchAngle);
			quaternion.x = Mathf.Tan(num * 0.5f * 0.017453292f);
			return quaternion;
		}
	}
}
