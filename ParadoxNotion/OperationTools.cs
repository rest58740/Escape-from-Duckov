using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000074 RID: 116
	public static class OperationTools
	{
		// Token: 0x0600041D RID: 1053 RVA: 0x0000A57E File Offset: 0x0000877E
		public static string GetOperationString(OperationMethod om)
		{
			if (om == OperationMethod.Set)
			{
				return " = ";
			}
			if (om == OperationMethod.Add)
			{
				return " += ";
			}
			if (om == OperationMethod.Subtract)
			{
				return " -= ";
			}
			if (om == OperationMethod.Multiply)
			{
				return " *= ";
			}
			if (om == OperationMethod.Divide)
			{
				return " /= ";
			}
			return string.Empty;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000A5B6 File Offset: 0x000087B6
		public static float Operate(float a, float b, OperationMethod om, float delta = 1f)
		{
			if (om == OperationMethod.Set)
			{
				return b;
			}
			if (om == OperationMethod.Add)
			{
				return a + b * delta;
			}
			if (om == OperationMethod.Subtract)
			{
				return a - b * delta;
			}
			if (om == OperationMethod.Multiply)
			{
				return a * (b * delta);
			}
			if (om == OperationMethod.Divide)
			{
				return a / (b * delta);
			}
			return a;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000A5E6 File Offset: 0x000087E6
		public static int Operate(int a, int b, OperationMethod om)
		{
			if (om == OperationMethod.Set)
			{
				return b;
			}
			if (om == OperationMethod.Add)
			{
				return a + b;
			}
			if (om == OperationMethod.Subtract)
			{
				return a - b;
			}
			if (om == OperationMethod.Multiply)
			{
				return a * b;
			}
			if (om == OperationMethod.Divide)
			{
				return a / b;
			}
			return a;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000A610 File Offset: 0x00008810
		public static Vector3 Operate(Vector3 a, Vector3 b, OperationMethod om, float delta = 1f)
		{
			if (om == OperationMethod.Set)
			{
				return b;
			}
			if (om == OperationMethod.Add)
			{
				return a + b * delta;
			}
			if (om == OperationMethod.Subtract)
			{
				return a - b * delta;
			}
			if (om == OperationMethod.Multiply)
			{
				return Vector3.Scale(a, b * delta);
			}
			if (om == OperationMethod.Divide)
			{
				b *= delta;
				return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
			}
			return a;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000A693 File Offset: 0x00008893
		public static string GetCompareString(CompareMethod cm)
		{
			if (cm == CompareMethod.EqualTo)
			{
				return " == ";
			}
			if (cm == CompareMethod.GreaterThan)
			{
				return " > ";
			}
			if (cm == CompareMethod.LessThan)
			{
				return " < ";
			}
			if (cm == CompareMethod.GreaterOrEqualTo)
			{
				return " >= ";
			}
			if (cm == CompareMethod.LessOrEqualTo)
			{
				return " <= ";
			}
			return string.Empty;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000A6CB File Offset: 0x000088CB
		public static bool Compare(float a, float b, CompareMethod cm, float floatingPoint)
		{
			if (cm == CompareMethod.EqualTo)
			{
				return Mathf.Abs(a - b) <= floatingPoint;
			}
			if (cm == CompareMethod.GreaterThan)
			{
				return a > b;
			}
			if (cm == CompareMethod.LessThan)
			{
				return a < b;
			}
			if (cm == CompareMethod.GreaterOrEqualTo)
			{
				return a >= b;
			}
			return cm != CompareMethod.LessOrEqualTo || a <= b;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000A70A File Offset: 0x0000890A
		public static bool Compare(int a, int b, CompareMethod cm)
		{
			if (cm == CompareMethod.EqualTo)
			{
				return a == b;
			}
			if (cm == CompareMethod.GreaterThan)
			{
				return a > b;
			}
			if (cm == CompareMethod.LessThan)
			{
				return a < b;
			}
			if (cm == CompareMethod.GreaterOrEqualTo)
			{
				return a >= b;
			}
			return cm != CompareMethod.LessOrEqualTo || a <= b;
		}
	}
}
