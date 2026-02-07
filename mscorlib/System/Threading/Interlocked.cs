using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Unity;

namespace System.Threading
{
	// Token: 0x020002F0 RID: 752
	public static class Interlocked
	{
		// Token: 0x060020B3 RID: 8371
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CompareExchange(ref int location1, int value, int comparand);

		// Token: 0x060020B4 RID: 8372
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int CompareExchange(ref int location1, int value, int comparand, ref bool succeeded);

		// Token: 0x060020B5 RID: 8373
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CompareExchange(ref object location1, ref object value, ref object comparand, ref object result);

		// Token: 0x060020B6 RID: 8374 RVA: 0x000769D4 File Offset: 0x00074BD4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static object CompareExchange(ref object location1, object value, object comparand)
		{
			object result = null;
			Interlocked.CompareExchange(ref location1, ref value, ref comparand, ref result);
			return result;
		}

		// Token: 0x060020B7 RID: 8375
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CompareExchange(ref float location1, float value, float comparand);

		// Token: 0x060020B8 RID: 8376
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Decrement(ref int location);

		// Token: 0x060020B9 RID: 8377
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long Decrement(ref long location);

		// Token: 0x060020BA RID: 8378
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Increment(ref int location);

		// Token: 0x060020BB RID: 8379
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long Increment(ref long location);

		// Token: 0x060020BC RID: 8380
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Exchange(ref int location1, int value);

		// Token: 0x060020BD RID: 8381
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Exchange(ref object location1, ref object value, ref object result);

		// Token: 0x060020BE RID: 8382 RVA: 0x000769F0 File Offset: 0x00074BF0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static object Exchange(ref object location1, object value)
		{
			object result = null;
			Interlocked.Exchange(ref location1, ref value, ref result);
			return result;
		}

		// Token: 0x060020BF RID: 8383
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float Exchange(ref float location1, float value);

		// Token: 0x060020C0 RID: 8384
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long CompareExchange(ref long location1, long value, long comparand);

		// Token: 0x060020C1 RID: 8385
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand);

		// Token: 0x060020C2 RID: 8386
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double CompareExchange(ref double location1, double value, double comparand);

		// Token: 0x060020C3 RID: 8387 RVA: 0x00076A0C File Offset: 0x00074C0C
		[ComVisible(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[Intrinsic]
		public static T CompareExchange<T>(ref T location1, T value, T comparand) where T : class
		{
			if (Unsafe.AsPointer<T>(ref location1) == null)
			{
				throw new NullReferenceException();
			}
			T result = default(T);
			Interlocked.CompareExchange(Unsafe.As<T, object>(ref location1), Unsafe.As<T, object>(ref value), Unsafe.As<T, object>(ref comparand), Unsafe.As<T, object>(ref result));
			return result;
		}

		// Token: 0x060020C4 RID: 8388
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long Exchange(ref long location1, long value);

		// Token: 0x060020C5 RID: 8389
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr Exchange(ref IntPtr location1, IntPtr value);

		// Token: 0x060020C6 RID: 8390
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Exchange(ref double location1, double value);

		// Token: 0x060020C7 RID: 8391 RVA: 0x00076A54 File Offset: 0x00074C54
		[ComVisible(false)]
		[Intrinsic]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static T Exchange<T>(ref T location1, T value) where T : class
		{
			if (Unsafe.AsPointer<T>(ref location1) == null)
			{
				throw new NullReferenceException();
			}
			T result = default(T);
			Interlocked.Exchange(Unsafe.As<T, object>(ref location1), Unsafe.As<T, object>(ref value), Unsafe.As<T, object>(ref result));
			return result;
		}

		// Token: 0x060020C8 RID: 8392
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long Read(ref long location);

		// Token: 0x060020C9 RID: 8393
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Add(ref int location1, int value);

		// Token: 0x060020CA RID: 8394
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long Add(ref long location1, long value);

		// Token: 0x060020CB RID: 8395 RVA: 0x00076A93 File Offset: 0x00074C93
		public static void MemoryBarrier()
		{
			Thread.MemoryBarrier();
		}

		// Token: 0x060020CC RID: 8396
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemoryBarrierProcessWide();

		// Token: 0x060020CD RID: 8397 RVA: 0x000173AD File Offset: 0x000155AD
		[SecuritySafeCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void SpeculationBarrier()
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}
}
