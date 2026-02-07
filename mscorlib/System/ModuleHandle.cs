using System;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200023C RID: 572
	[ComVisible(true)]
	public struct ModuleHandle
	{
		// Token: 0x06001A1E RID: 6686 RVA: 0x0005FF98 File Offset: 0x0005E198
		internal ModuleHandle(IntPtr v)
		{
			this.value = v;
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x0005FFA1 File Offset: 0x0005E1A1
		internal IntPtr Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0005FFA9 File Offset: 0x0005E1A9
		public int MDStreamVersion
		{
			get
			{
				if (this.value == IntPtr.Zero)
				{
					throw new ArgumentNullException(string.Empty, "Invalid handle");
				}
				return RuntimeModule.GetMDStreamVersion(this.value);
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x0005FFD8 File Offset: 0x0005E1D8
		internal void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
		{
			if (this.value == IntPtr.Zero)
			{
				throw new ArgumentNullException(string.Empty, "Invalid handle");
			}
			RuntimeModule.GetPEKind(this.value, out peKind, out machine);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00060009 File Offset: 0x0005E209
		public RuntimeFieldHandle ResolveFieldHandle(int fieldToken)
		{
			return this.ResolveFieldHandle(fieldToken, null, null);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00060014 File Offset: 0x0005E214
		public RuntimeMethodHandle ResolveMethodHandle(int methodToken)
		{
			return this.ResolveMethodHandle(methodToken, null, null);
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0006001F File Offset: 0x0005E21F
		public RuntimeTypeHandle ResolveTypeHandle(int typeToken)
		{
			return this.ResolveTypeHandle(typeToken, null, null);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0006002C File Offset: 0x0005E22C
		private IntPtr[] ptrs_from_handles(RuntimeTypeHandle[] handles)
		{
			if (handles == null)
			{
				return null;
			}
			IntPtr[] array = new IntPtr[handles.Length];
			for (int i = 0; i < handles.Length; i++)
			{
				array[i] = handles[i].Value;
			}
			return array;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x00060068 File Offset: 0x0005E268
		public RuntimeTypeHandle ResolveTypeHandle(int typeToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			if (this.value == IntPtr.Zero)
			{
				throw new ArgumentNullException(string.Empty, "Invalid handle");
			}
			ResolveTokenError resolveTokenError;
			IntPtr intPtr = RuntimeModule.ResolveTypeToken(this.value, typeToken, this.ptrs_from_handles(typeInstantiationContext), this.ptrs_from_handles(methodInstantiationContext), out resolveTokenError);
			if (intPtr == IntPtr.Zero)
			{
				throw new TypeLoadException(string.Format("Could not load type '0x{0:x}' from assembly '0x{1:x}'", typeToken, this.value.ToInt64()));
			}
			return new RuntimeTypeHandle(intPtr);
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x000600F0 File Offset: 0x0005E2F0
		public RuntimeMethodHandle ResolveMethodHandle(int methodToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			if (this.value == IntPtr.Zero)
			{
				throw new ArgumentNullException(string.Empty, "Invalid handle");
			}
			ResolveTokenError resolveTokenError;
			IntPtr intPtr = RuntimeModule.ResolveMethodToken(this.value, methodToken, this.ptrs_from_handles(typeInstantiationContext), this.ptrs_from_handles(methodInstantiationContext), out resolveTokenError);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception(string.Format("Could not load method '0x{0:x}' from assembly '0x{1:x}'", methodToken, this.value.ToInt64()));
			}
			return new RuntimeMethodHandle(intPtr);
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x00060178 File Offset: 0x0005E378
		public RuntimeFieldHandle ResolveFieldHandle(int fieldToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			if (this.value == IntPtr.Zero)
			{
				throw new ArgumentNullException(string.Empty, "Invalid handle");
			}
			ResolveTokenError resolveTokenError;
			IntPtr intPtr = RuntimeModule.ResolveFieldToken(this.value, fieldToken, this.ptrs_from_handles(typeInstantiationContext), this.ptrs_from_handles(methodInstantiationContext), out resolveTokenError);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception(string.Format("Could not load field '0x{0:x}' from assembly '0x{1:x}'", fieldToken, this.value.ToInt64()));
			}
			return new RuntimeFieldHandle(intPtr);
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x000601FD File Offset: 0x0005E3FD
		public RuntimeFieldHandle GetRuntimeFieldHandleFromMetadataToken(int fieldToken)
		{
			return this.ResolveFieldHandle(fieldToken);
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00060206 File Offset: 0x0005E406
		public RuntimeMethodHandle GetRuntimeMethodHandleFromMetadataToken(int methodToken)
		{
			return this.ResolveMethodHandle(methodToken);
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0006020F File Offset: 0x0005E40F
		public RuntimeTypeHandle GetRuntimeTypeHandleFromMetadataToken(int typeToken)
		{
			return this.ResolveTypeHandle(typeToken);
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00060218 File Offset: 0x0005E418
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && this.value == ((ModuleHandle)obj).Value;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00060260 File Offset: 0x0005E460
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(ModuleHandle handle)
		{
			return this.value == handle.Value;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x00060274 File Offset: 0x0005E474
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00060281 File Offset: 0x0005E481
		public static bool operator ==(ModuleHandle left, ModuleHandle right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x00060294 File Offset: 0x0005E494
		public static bool operator !=(ModuleHandle left, ModuleHandle right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x0400171F RID: 5919
		private IntPtr value;

		// Token: 0x04001720 RID: 5920
		public static readonly ModuleHandle EmptyHandle = new ModuleHandle(IntPtr.Zero);
	}
}
