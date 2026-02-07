using System;
using System.Collections.Generic;

namespace System
{
	// Token: 0x02000164 RID: 356
	public static class Nullable
	{
		// Token: 0x06000DF5 RID: 3573 RVA: 0x00036156 File Offset: 0x00034356
		public static int Compare<T>(T? n1, T? n2) where T : struct
		{
			if (n1 != null)
			{
				if (n2 != null)
				{
					return Comparer<T>.Default.Compare(n1.value, n2.value);
				}
				return 1;
			}
			else
			{
				if (n2 != null)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0003618F File Offset: 0x0003438F
		public static bool Equals<T>(T? n1, T? n2) where T : struct
		{
			if (n1 != null)
			{
				return n2 != null && EqualityComparer<T>.Default.Equals(n1.value, n2.value);
			}
			return n2 == null;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000361C8 File Offset: 0x000343C8
		public static Type GetUnderlyingType(Type nullableType)
		{
			if (nullableType == null)
			{
				throw new ArgumentNullException("nullableType");
			}
			if (nullableType.IsGenericType && !nullableType.IsGenericTypeDefinition && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return nullableType.GetGenericArguments()[0];
			}
			return null;
		}
	}
}
