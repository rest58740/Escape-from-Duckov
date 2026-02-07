using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x02000936 RID: 2358
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodRental))]
	public sealed class MethodRental : _MethodRental
	{
		// Token: 0x0600516A RID: 20842 RVA: 0x0000259F File Offset: 0x0000079F
		private MethodRental()
		{
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x000FE904 File Offset: 0x000FCB04
		[MonoTODO]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public static void SwapMethodBody(Type cls, int methodtoken, IntPtr rgIL, int methodSize, int flags)
		{
			if (methodSize <= 0 || methodSize >= 4128768)
			{
				throw new ArgumentException("Data size must be > 0 and < 0x3f0000", "methodSize");
			}
			if (cls == null)
			{
				throw new ArgumentNullException("cls");
			}
			if (cls is TypeBuilder && !((TypeBuilder)cls).is_created)
			{
				throw new NotSupportedException("Type '" + ((cls != null) ? cls.ToString() : null) + "' is not yet created.");
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodRental.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodRental.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodRental.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodRental.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040031D6 RID: 12758
		public const int JitImmediate = 1;

		// Token: 0x040031D7 RID: 12759
		public const int JitOnDemand = 0;
	}
}
