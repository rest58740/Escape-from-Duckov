using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000229 RID: 553
	[StructLayout(LayoutKind.Auto)]
	public struct ArgIterator
	{
		// Token: 0x060018CC RID: 6348
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Setup(IntPtr argsp, IntPtr start);

		// Token: 0x060018CD RID: 6349 RVA: 0x0005E054 File Offset: 0x0005C254
		public ArgIterator(RuntimeArgumentHandle arglist)
		{
			this.sig = IntPtr.Zero;
			this.args = IntPtr.Zero;
			this.next_arg = (this.num_args = 0);
			if (arglist.args == IntPtr.Zero)
			{
				throw new PlatformNotSupportedException();
			}
			this.Setup(arglist.args, IntPtr.Zero);
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0005E0B4 File Offset: 0x0005C2B4
		[CLSCompliant(false)]
		public unsafe ArgIterator(RuntimeArgumentHandle arglist, void* ptr)
		{
			this.sig = IntPtr.Zero;
			this.args = IntPtr.Zero;
			this.next_arg = (this.num_args = 0);
			if (arglist.args == IntPtr.Zero)
			{
				throw new PlatformNotSupportedException();
			}
			this.Setup(arglist.args, (IntPtr)ptr);
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0005E113 File Offset: 0x0005C313
		public void End()
		{
			this.next_arg = this.num_args;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0005E121 File Offset: 0x0005C321
		public override bool Equals(object o)
		{
			throw new NotSupportedException("ArgIterator does not support Equals.");
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0005E12D File Offset: 0x0005C32D
		public override int GetHashCode()
		{
			return this.sig.GetHashCode();
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0005E13C File Offset: 0x0005C33C
		[CLSCompliant(false)]
		public unsafe TypedReference GetNextArg()
		{
			if (this.num_args == this.next_arg)
			{
				throw new InvalidOperationException("Invalid iterator position.");
			}
			TypedReference result = default(TypedReference);
			this.IntGetNextArg((void*)(&result));
			return result;
		}

		// Token: 0x060018D3 RID: 6355
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void IntGetNextArg(void* res);

		// Token: 0x060018D4 RID: 6356 RVA: 0x0005E174 File Offset: 0x0005C374
		[CLSCompliant(false)]
		public unsafe TypedReference GetNextArg(RuntimeTypeHandle rth)
		{
			if (this.num_args == this.next_arg)
			{
				throw new InvalidOperationException("Invalid iterator position.");
			}
			TypedReference result = default(TypedReference);
			this.IntGetNextArgWithType((void*)(&result), rth.Value);
			return result;
		}

		// Token: 0x060018D5 RID: 6357
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void IntGetNextArgWithType(void* res, IntPtr rth);

		// Token: 0x060018D6 RID: 6358 RVA: 0x0005E1B3 File Offset: 0x0005C3B3
		public RuntimeTypeHandle GetNextArgType()
		{
			if (this.num_args == this.next_arg)
			{
				throw new InvalidOperationException("Invalid iterator position.");
			}
			return new RuntimeTypeHandle(this.IntGetNextArgType());
		}

		// Token: 0x060018D7 RID: 6359
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr IntGetNextArgType();

		// Token: 0x060018D8 RID: 6360 RVA: 0x0005E1D9 File Offset: 0x0005C3D9
		public int GetRemainingCount()
		{
			return this.num_args - this.next_arg;
		}

		// Token: 0x040016D7 RID: 5847
		private IntPtr sig;

		// Token: 0x040016D8 RID: 5848
		private IntPtr args;

		// Token: 0x040016D9 RID: 5849
		private int next_arg;

		// Token: 0x040016DA RID: 5850
		private int num_args;
	}
}
