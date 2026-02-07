using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x02000021 RID: 33
	public class KTwoBoneIK
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00003810 File Offset: 0x00001A10
		public static void Solve(ref KTwoBoneIkData ikData)
		{
			Vector3 position = ikData.root.position;
			Vector3 position2 = ikData.mid.position;
			Vector3 position3 = ikData.tip.position;
			Vector3 a = Vector3.Lerp(position3, ikData.target.position, ikData.posWeight);
			Quaternion rotation = Quaternion.Lerp(ikData.tip.rotation, ikData.target.rotation, ikData.rotWeight);
			bool flag = ikData.hasValidHint && ikData.hintWeight > 0f;
			Vector3 vector = position2 - position;
			Vector3 rhs = position3 - position2;
			Vector3 vector2 = position3 - position;
			Vector3 vector3 = a - position;
			float magnitude = vector.magnitude;
			float magnitude2 = rhs.magnitude;
			float magnitude3 = vector2.magnitude;
			float magnitude4 = vector3.magnitude;
			float num = KMath.TriangleAngle(magnitude3, magnitude, magnitude2);
			float num2 = KMath.TriangleAngle(magnitude4, magnitude, magnitude2);
			Vector3 vector4 = Vector3.Cross(vector, rhs);
			if (vector4.sqrMagnitude < 1E-08f)
			{
				vector4 = (flag ? Vector3.Cross(ikData.hint.position - position, rhs) : Vector3.zero);
				if (vector4.sqrMagnitude < 1E-08f)
				{
					vector4 = Vector3.Cross(vector3, rhs);
				}
				if (vector4.sqrMagnitude < 1E-08f)
				{
					vector4 = Vector3.up;
				}
			}
			vector4 = Vector3.Normalize(vector4);
			float f = 0.5f * (num - num2);
			float num3 = Mathf.Sin(f);
			float w = Mathf.Cos(f);
			Quaternion lhs = new Quaternion(vector4.x * num3, vector4.y * num3, vector4.z * num3, w);
			KTransform relativeTransform = ikData.mid.GetRelativeTransform(ikData.tip, false);
			ikData.mid.rotation = lhs * ikData.mid.rotation;
			ikData.tip = ikData.mid.GetWorldTransform(relativeTransform, false);
			vector2 = ikData.tip.position - position;
			KTransform relativeTransform2 = ikData.root.GetRelativeTransform(ikData.mid, false);
			relativeTransform = ikData.mid.GetRelativeTransform(ikData.tip, false);
			ikData.root.rotation = KMath.FromToRotation(vector2, vector3) * ikData.root.rotation;
			ikData.mid = ikData.root.GetWorldTransform(relativeTransform2, false);
			ikData.tip = ikData.mid.GetWorldTransform(relativeTransform, false);
			if (flag)
			{
				float sqrMagnitude = vector2.sqrMagnitude;
				if (sqrMagnitude > 0f)
				{
					position2 = ikData.mid.position;
					Vector3 position4 = ikData.tip.position;
					vector = position2 - position;
					vector2 = position4 - position;
					Vector3 vector5 = vector2 / Mathf.Sqrt(sqrMagnitude);
					Vector3 vector6 = ikData.hint.position - position;
					Vector3 from = vector - vector5 * Vector3.Dot(vector, vector5);
					Vector3 to = vector6 - vector5 * Vector3.Dot(vector6, vector5);
					float num4 = magnitude + magnitude2;
					if (from.sqrMagnitude > num4 * num4 * 0.001f && to.sqrMagnitude > 0f)
					{
						Quaternion quaternion = KMath.FromToRotation(from, to);
						quaternion.x *= ikData.hintWeight;
						quaternion.y *= ikData.hintWeight;
						quaternion.z *= ikData.hintWeight;
						quaternion = KMath.NormalizeSafe(quaternion);
						ikData.root.rotation = quaternion * ikData.root.rotation;
						ikData.mid = ikData.root.GetWorldTransform(relativeTransform2, false);
						ikData.tip = ikData.mid.GetWorldTransform(relativeTransform, false);
					}
				}
			}
			ikData.tip.rotation = rotation;
		}
	}
}
