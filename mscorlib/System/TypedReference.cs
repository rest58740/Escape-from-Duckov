using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x0200020F RID: 527
	[CLSCompliant(false)]
	[ComVisible(true)]
	[NonVersionable]
	public ref struct TypedReference
	{
		// Token: 0x06001736 RID: 5942 RVA: 0x0005A744 File Offset: 0x00058944
		[CLSCompliant(false)]
		[SecurityCritical]
		public unsafe static TypedReference MakeTypedReference(object target, FieldInfo[] flds)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (flds == null)
			{
				throw new ArgumentNullException("flds");
			}
			if (flds.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Array must not be of length zero."), "flds");
			}
			IntPtr[] array = new IntPtr[flds.Length];
			RuntimeType runtimeType = (RuntimeType)target.GetType();
			for (int i = 0; i < flds.Length; i++)
			{
				RuntimeFieldInfo runtimeFieldInfo = flds[i] as RuntimeFieldInfo;
				if (runtimeFieldInfo == null)
				{
					throw new ArgumentException(Environment.GetResourceString("FieldInfo must be a runtime FieldInfo object."));
				}
				if (runtimeFieldInfo.IsStatic)
				{
					throw new ArgumentException(Environment.GetResourceString("Field in TypedReferences cannot be static or init only."));
				}
				if (runtimeType != runtimeFieldInfo.GetDeclaringTypeInternal() && !runtimeType.IsSubclassOf(runtimeFieldInfo.GetDeclaringTypeInternal()))
				{
					throw new MissingMemberException(Environment.GetResourceString("FieldInfo does not match the target Type."));
				}
				RuntimeType runtimeType2 = (RuntimeType)runtimeFieldInfo.FieldType;
				if (runtimeType2.IsPrimitive)
				{
					throw new ArgumentException(Environment.GetResourceString("TypedReferences cannot be redefined as primitives."));
				}
				if (i < flds.Length - 1 && !runtimeType2.IsValueType)
				{
					throw new MissingMemberException(Environment.GetResourceString("TypedReference can only be made on nested value Types."));
				}
				array[i] = runtimeFieldInfo.FieldHandle.Value;
				runtimeType = runtimeType2;
			}
			TypedReference result = default(TypedReference);
			TypedReference.InternalMakeTypedReference((void*)(&result), target, array, runtimeType);
			return result;
		}

		// Token: 0x06001737 RID: 5943
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void InternalMakeTypedReference(void* result, object target, IntPtr[] flds, RuntimeType lastFieldType);

		// Token: 0x06001738 RID: 5944 RVA: 0x0005A88C File Offset: 0x00058A8C
		public override int GetHashCode()
		{
			if (this.Type == IntPtr.Zero)
			{
				return 0;
			}
			return __reftype(this).GetHashCode();
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0005A8B4 File Offset: 0x00058AB4
		public override bool Equals(object o)
		{
			throw new NotSupportedException(Environment.GetResourceString("This feature is not currently implemented."));
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x0005A8C5 File Offset: 0x00058AC5
		[SecuritySafeCritical]
		public unsafe static object ToObject(TypedReference value)
		{
			return TypedReference.InternalToObject((void*)(&value));
		}

		// Token: 0x0600173B RID: 5947
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern object InternalToObject(void* value);

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x0005A8CF File Offset: 0x00058ACF
		internal bool IsNull
		{
			get
			{
				return this.Value == IntPtr.Zero && this.Type == IntPtr.Zero;
			}
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x0005A8F5 File Offset: 0x00058AF5
		public static Type GetTargetType(TypedReference value)
		{
			return __reftype(value);
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x0005A8FF File Offset: 0x00058AFF
		public static RuntimeTypeHandle TargetTypeToken(TypedReference value)
		{
			return __reftype(value).TypeHandle;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x0005A90E File Offset: 0x00058B0E
		[SecuritySafeCritical]
		[CLSCompliant(false)]
		public static void SetTypedReference(TypedReference target, object value)
		{
			throw new NotImplementedException("SetTypedReference");
		}

		// Token: 0x0400162C RID: 5676
		private RuntimeTypeHandle type;

		// Token: 0x0400162D RID: 5677
		private IntPtr Value;

		// Token: 0x0400162E RID: 5678
		private IntPtr Type;
	}
}
