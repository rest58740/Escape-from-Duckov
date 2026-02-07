using System;
using System.Reflection;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x02000057 RID: 87
	public static class DOTweenUtils
	{
		// Token: 0x06000310 RID: 784 RVA: 0x00011B28 File Offset: 0x0000FD28
		internal static Vector3 Vector3FromAngle(float degrees, float magnitude)
		{
			float f = degrees * 0.017453292f;
			return new Vector3(magnitude * Mathf.Cos(f), magnitude * Mathf.Sin(f), 0f);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00011B58 File Offset: 0x0000FD58
		internal static float Angle2D(Vector3 from, Vector3 to)
		{
			Vector2 right = Vector2.right;
			to -= from;
			float num = Vector2.Angle(right, to);
			if (Vector3.Cross(right, to).z > 0f)
			{
				num = 360f - num;
			}
			return num * -1f;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00011BA8 File Offset: 0x0000FDA8
		internal static Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
		{
			return rotation * (point - pivot) + pivot;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00011BC0 File Offset: 0x0000FDC0
		public static Vector2 GetPointOnCircle(Vector2 center, float radius, float degrees)
		{
			degrees = 90f - degrees;
			float f = degrees * 0.017453292f;
			return center + new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * radius;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00011BFB File Offset: 0x0000FDFB
		internal static bool Vector3AreApproximatelyEqual(Vector3 a, Vector3 b)
		{
			return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00011C38 File Offset: 0x0000FE38
		internal static Type GetLooseScriptType(string typeName)
		{
			for (int i = 0; i < DOTweenUtils._defAssembliesToQuery.Length; i++)
			{
				Type type = Type.GetType(string.Format("{0}, {1}", typeName, DOTweenUtils._defAssembliesToQuery[i]));
				if (type != null)
				{
					return type;
				}
			}
			if (DOTweenUtils._loadedAssemblies == null)
			{
				DOTweenUtils._loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			}
			for (int j = 0; j < DOTweenUtils._loadedAssemblies.Length; j++)
			{
				Type type2 = Type.GetType(string.Format("{0}, {1}", typeName, DOTweenUtils._loadedAssemblies[j].GetName()));
				if (type2 != null)
				{
					return type2;
				}
			}
			return null;
		}

		// Token: 0x0400019A RID: 410
		private static Assembly[] _loadedAssemblies;

		// Token: 0x0400019B RID: 411
		private static readonly string[] _defAssembliesToQuery = new string[]
		{
			"DOTween.Modules",
			"Assembly-CSharp",
			"Assembly-CSharp-firstpass"
		};
	}
}
