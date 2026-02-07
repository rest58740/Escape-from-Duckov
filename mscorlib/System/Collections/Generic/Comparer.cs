using System;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Collections.Generic
{
	// Token: 0x02000AC0 RID: 2752
	[TypeDependency("System.Collections.Generic.ObjectComparer`1")]
	[Serializable]
	public abstract class Comparer<T> : IComparer, IComparer<T>
	{
		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x06006274 RID: 25204 RVA: 0x00149B30 File Offset: 0x00147D30
		public static Comparer<T> Default
		{
			get
			{
				Comparer<T> comparer = Comparer<T>.defaultComparer;
				if (comparer == null)
				{
					comparer = Comparer<T>.CreateComparer();
					Comparer<T>.defaultComparer = comparer;
				}
				return comparer;
			}
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x00149B57 File Offset: 0x00147D57
		public static Comparer<T> Create(Comparison<T> comparison)
		{
			if (comparison == null)
			{
				throw new ArgumentNullException("comparison");
			}
			return new ComparisonComparer<T>(comparison);
		}

		// Token: 0x06006276 RID: 25206 RVA: 0x00149B70 File Offset: 0x00147D70
		[SecuritySafeCritical]
		private static Comparer<T> CreateComparer()
		{
			RuntimeType runtimeType = (RuntimeType)typeof(T);
			if (typeof(IComparable<T>).IsAssignableFrom(runtimeType))
			{
				return (Comparer<T>)RuntimeType.CreateInstanceForAnotherGenericParameter(typeof(GenericComparer<>), runtimeType);
			}
			if (runtimeType.IsGenericType && runtimeType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				RuntimeType runtimeType2 = (RuntimeType)runtimeType.GetGenericArguments()[0];
				if (typeof(IComparable<>).MakeGenericType(new Type[]
				{
					runtimeType2
				}).IsAssignableFrom(runtimeType2))
				{
					return (Comparer<T>)RuntimeType.CreateInstanceForAnotherGenericParameter(typeof(NullableComparer<>), runtimeType2);
				}
			}
			return new ObjectComparer<T>();
		}

		// Token: 0x06006277 RID: 25207
		public abstract int Compare(T x, T y);

		// Token: 0x06006278 RID: 25208 RVA: 0x00149C1E File Offset: 0x00147E1E
		int IComparer.Compare(object x, object y)
		{
			if (x == null)
			{
				if (y != null)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (y == null)
				{
					return 1;
				}
				if (x is T && y is T)
				{
					return this.Compare((T)((object)x), (T)((object)y));
				}
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
				return 0;
			}
		}

		// Token: 0x04003A34 RID: 14900
		private static volatile Comparer<T> defaultComparer;
	}
}
