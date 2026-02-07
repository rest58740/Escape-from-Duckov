using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x0200024A RID: 586
	[ComVisible(true)]
	[Serializable]
	public struct RuntimeFieldHandle : ISerializable
	{
		// Token: 0x06001AEC RID: 6892 RVA: 0x00064598 File Offset: 0x00062798
		internal RuntimeFieldHandle(IntPtr v)
		{
			this.value = v;
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x000645A4 File Offset: 0x000627A4
		private RuntimeFieldHandle(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			RuntimeFieldInfo runtimeFieldInfo = (RuntimeFieldInfo)info.GetValue("FieldObj", typeof(RuntimeFieldInfo));
			this.value = runtimeFieldInfo.FieldHandle.Value;
			if (this.value == IntPtr.Zero)
			{
				throw new SerializationException("Insufficient state.");
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x0006460B File Offset: 0x0006280B
		public IntPtr Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00064613 File Offset: 0x00062813
		internal bool IsNullHandle()
		{
			return this.value == IntPtr.Zero;
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00064628 File Offset: 0x00062828
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.value == IntPtr.Zero)
			{
				throw new SerializationException("Object fields may not be properly initialized");
			}
			info.AddValue("FieldObj", (RuntimeFieldInfo)FieldInfo.GetFieldFromHandle(this), typeof(RuntimeFieldInfo));
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x00064688 File Offset: 0x00062888
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && this.value == ((RuntimeFieldHandle)obj).Value;
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000646D0 File Offset: 0x000628D0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(RuntimeFieldHandle handle)
		{
			return this.value == handle.Value;
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000646E4 File Offset: 0x000628E4
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000646F1 File Offset: 0x000628F1
		public static bool operator ==(RuntimeFieldHandle left, RuntimeFieldHandle right)
		{
			return left.Equals(right);
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x000646FB File Offset: 0x000628FB
		public static bool operator !=(RuntimeFieldHandle left, RuntimeFieldHandle right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06001AF6 RID: 6902
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetValueInternal(FieldInfo fi, object obj, object value);

		// Token: 0x06001AF7 RID: 6903 RVA: 0x00064708 File Offset: 0x00062908
		internal static void SetValue(RuntimeFieldInfo field, object obj, object value, RuntimeType fieldType, FieldAttributes fieldAttr, RuntimeType declaringType, ref bool domainInitialized)
		{
			RuntimeFieldHandle.SetValueInternal(field, obj, value);
		}

		// Token: 0x06001AF8 RID: 6904
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern object GetValueDirect(RuntimeFieldInfo field, RuntimeType fieldType, void* pTypedRef, RuntimeType contextType);

		// Token: 0x06001AF9 RID: 6905
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void SetValueDirect(RuntimeFieldInfo field, RuntimeType fieldType, void* pTypedRef, object value, RuntimeType contextType);

		// Token: 0x04001778 RID: 6008
		private IntPtr value;
	}
}
