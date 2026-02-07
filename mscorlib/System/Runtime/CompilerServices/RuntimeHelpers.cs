using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000852 RID: 2130
	public static class RuntimeHelpers
	{
		// Token: 0x060046F1 RID: 18161
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitializeArray(Array array, IntPtr fldHandle);

		// Token: 0x060046F2 RID: 18162 RVA: 0x000E7DB7 File Offset: 0x000E5FB7
		public static void InitializeArray(Array array, RuntimeFieldHandle fldHandle)
		{
			if (array == null || fldHandle.Value == IntPtr.Zero)
			{
				throw new ArgumentNullException();
			}
			RuntimeHelpers.InitializeArray(array, fldHandle.Value);
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x060046F3 RID: 18163
		public static extern int OffsetToStringData { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x060046F4 RID: 18164 RVA: 0x00064583 File Offset: 0x00062783
		public static int GetHashCode(object o)
		{
			return object.InternalGetHashCode(o);
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x000E7DE2 File Offset: 0x000E5FE2
		public new static bool Equals(object o1, object o2)
		{
			if (o1 == o2)
			{
				return true;
			}
			if (o1 == null || o2 == null)
			{
				return false;
			}
			if (o1 is ValueType)
			{
				return ValueType.DefaultEquals(o1, o2);
			}
			return object.Equals(o1, o2);
		}

		// Token: 0x060046F6 RID: 18166
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectValue(object obj);

		// Token: 0x060046F7 RID: 18167
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RunClassConstructor(IntPtr type);

		// Token: 0x060046F8 RID: 18168 RVA: 0x000E7E09 File Offset: 0x000E6009
		public static void RunClassConstructor(RuntimeTypeHandle type)
		{
			if (type.Value == IntPtr.Zero)
			{
				throw new ArgumentException("Handle is not initialized.", "type");
			}
			RuntimeHelpers.RunClassConstructor(type.Value);
		}

		// Token: 0x060046F9 RID: 18169
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SufficientExecutionStack();

		// Token: 0x060046FA RID: 18170 RVA: 0x000E7E3A File Offset: 0x000E603A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void EnsureSufficientExecutionStack()
		{
			if (RuntimeHelpers.SufficientExecutionStack())
			{
				return;
			}
			throw new InsufficientExecutionStackException();
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x000E7E49 File Offset: 0x000E6049
		public static bool TryEnsureSufficientExecutionStack()
		{
			return RuntimeHelpers.SufficientExecutionStack();
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void ExecuteCodeWithGuaranteedCleanup(RuntimeHelpers.TryCode code, RuntimeHelpers.CleanupCode backoutCode, object userData)
		{
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void PrepareConstrainedRegions()
		{
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void PrepareConstrainedRegionsNoOP()
		{
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void ProbeForSufficientStack()
		{
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[SecurityCritical]
		public static void PrepareDelegate(Delegate d)
		{
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[SecurityCritical]
		public static void PrepareContractedDelegate(Delegate d)
		{
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void PrepareMethod(RuntimeMethodHandle method)
		{
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void PrepareMethod(RuntimeMethodHandle method, RuntimeTypeHandle[] instantiation)
		{
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x000E7E50 File Offset: 0x000E6050
		public static void RunModuleConstructor(ModuleHandle module)
		{
			if (module == ModuleHandle.EmptyHandle)
			{
				throw new ArgumentException("Handle is not initialized.", "module");
			}
			RuntimeHelpers.RunModuleConstructor(module.Value);
		}

		// Token: 0x06004705 RID: 18181
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RunModuleConstructor(IntPtr module);

		// Token: 0x06004706 RID: 18182 RVA: 0x000E7E7B File Offset: 0x000E607B
		public static bool IsReferenceOrContainsReferences<T>()
		{
			return !typeof(T).IsValueType || RuntimeTypeHandle.HasReferences(typeof(T) as RuntimeType);
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x000E7EA4 File Offset: 0x000E60A4
		public static object GetUninitializedObject(Type type)
		{
			return FormatterServices.GetUninitializedObject(type);
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x000E7EAC File Offset: 0x000E60AC
		public static T[] GetSubArray<T>(T[] array, Range range)
		{
			Type elementType = array.GetType().GetElementType();
			Span<T> span = array.AsSpan(range);
			if (elementType.IsValueType)
			{
				return span.ToArray();
			}
			T[] array2 = (T[])Array.CreateInstance(elementType, span.Length);
			span.CopyTo(array2);
			return array2;
		}

		// Token: 0x02000853 RID: 2131
		// (Invoke) Token: 0x0600470A RID: 18186
		public delegate void TryCode(object userData);

		// Token: 0x02000854 RID: 2132
		// (Invoke) Token: 0x0600470E RID: 18190
		public delegate void CleanupCode(object userData, bool exceptionThrown);
	}
}
