using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200010F RID: 271
	internal static class RuntimeHelpersAbstraction
	{
		// Token: 0x0600064A RID: 1610 RVA: 0x0000E778 File Offset: 0x0000C978
		public static bool IsWellKnownNoReferenceContainsType<T>()
		{
			return RuntimeHelpersAbstraction.WellKnownNoReferenceContainsType<T>.IsWellKnownType;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0000E780 File Offset: 0x0000C980
		private static bool WellKnownNoReferenceContainsTypeInitialize(Type t)
		{
			if (t.IsPrimitive)
			{
				return true;
			}
			if (t.IsEnum)
			{
				return true;
			}
			if (t == typeof(DateTime))
			{
				return true;
			}
			if (t == typeof(DateTimeOffset))
			{
				return true;
			}
			if (t == typeof(Guid))
			{
				return true;
			}
			if (t == typeof(decimal))
			{
				return true;
			}
			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return RuntimeHelpersAbstraction.WellKnownNoReferenceContainsTypeInitialize(t.GetGenericArguments()[0]);
			}
			return t == typeof(Vector2) || t == typeof(Vector3) || t == typeof(Vector4) || t == typeof(Color) || t == typeof(Rect) || t == typeof(Bounds) || t == typeof(Quaternion) || t == typeof(Vector2Int) || t == typeof(Vector3Int);
		}

		// Token: 0x0200020D RID: 525
		private static class WellKnownNoReferenceContainsType<T>
		{
			// Token: 0x0400054D RID: 1357
			public static readonly bool IsWellKnownType = RuntimeHelpersAbstraction.WellKnownNoReferenceContainsTypeInitialize(typeof(T));
		}
	}
}
