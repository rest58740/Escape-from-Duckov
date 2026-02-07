using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Threading
{
	// Token: 0x020002EA RID: 746
	[ComVisible(true)]
	[StructLayout(LayoutKind.Sequential)]
	public abstract class WaitHandle : MarshalByRefObject, IDisposable
	{
		// Token: 0x06002073 RID: 8307 RVA: 0x00075FF7 File Offset: 0x000741F7
		protected WaitHandle()
		{
			this.Init();
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x00076005 File Offset: 0x00074205
		[SecuritySafeCritical]
		private void Init()
		{
			this.safeWaitHandle = null;
			this.waitHandle = WaitHandle.InvalidHandle;
			this.hasThreadAffinity = false;
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x00076022 File Offset: 0x00074222
		// (set) Token: 0x06002076 RID: 8310 RVA: 0x00076044 File Offset: 0x00074244
		[Obsolete("Use the SafeWaitHandle property instead.")]
		public virtual IntPtr Handle
		{
			[SecuritySafeCritical]
			get
			{
				if (this.safeWaitHandle != null)
				{
					return this.safeWaitHandle.DangerousGetHandle();
				}
				return WaitHandle.InvalidHandle;
			}
			[SecurityCritical]
			set
			{
				if (value == WaitHandle.InvalidHandle)
				{
					if (this.safeWaitHandle != null)
					{
						this.safeWaitHandle.SetHandleAsInvalid();
						this.safeWaitHandle = null;
					}
				}
				else
				{
					this.safeWaitHandle = new SafeWaitHandle(value, true);
				}
				this.waitHandle = value;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x00076096 File Offset: 0x00074296
		// (set) Token: 0x06002078 RID: 8312 RVA: 0x000760C0 File Offset: 0x000742C0
		public SafeWaitHandle SafeWaitHandle
		{
			[SecurityCritical]
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				if (this.safeWaitHandle == null)
				{
					this.safeWaitHandle = new SafeWaitHandle(WaitHandle.InvalidHandle, false);
				}
				return this.safeWaitHandle;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[SecurityCritical]
			set
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					if (value == null)
					{
						this.safeWaitHandle = null;
						this.waitHandle = WaitHandle.InvalidHandle;
					}
					else
					{
						this.safeWaitHandle = value;
						this.waitHandle = this.safeWaitHandle.DangerousGetHandle();
					}
				}
			}
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x0007611C File Offset: 0x0007431C
		[SecurityCritical]
		internal void SetHandleInternal(SafeWaitHandle handle)
		{
			this.safeWaitHandle = handle;
			this.waitHandle = handle.DangerousGetHandle();
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x00076133 File Offset: 0x00074333
		public virtual bool WaitOne(int millisecondsTimeout, bool exitContext)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			return this.WaitOne((long)millisecondsTimeout, exitContext);
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00076158 File Offset: 0x00074358
		public virtual bool WaitOne(TimeSpan timeout, bool exitContext)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (-1L > num || 2147483647L < num)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			return this.WaitOne(num, exitContext);
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00076199 File Offset: 0x00074399
		public virtual bool WaitOne()
		{
			return this.WaitOne(-1, false);
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000761A3 File Offset: 0x000743A3
		public virtual bool WaitOne(int millisecondsTimeout)
		{
			return this.WaitOne(millisecondsTimeout, false);
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000761AD File Offset: 0x000743AD
		public virtual bool WaitOne(TimeSpan timeout)
		{
			return this.WaitOne(timeout, false);
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000761B7 File Offset: 0x000743B7
		[SecuritySafeCritical]
		private bool WaitOne(long timeout, bool exitContext)
		{
			return WaitHandle.InternalWaitOne(this.safeWaitHandle, timeout, this.hasThreadAffinity, exitContext);
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000761D0 File Offset: 0x000743D0
		[SecurityCritical]
		internal static bool InternalWaitOne(SafeHandle waitableSafeHandle, long millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
		{
			if (waitableSafeHandle == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a disposed object."));
			}
			int num = WaitHandle.WaitOneNative(waitableSafeHandle, (uint)millisecondsTimeout, hasThreadAffinity, exitContext);
			if (num == 128)
			{
				WaitHandle.ThrowAbandonedMutexException();
			}
			return num != 258 && num != int.MaxValue;
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00076220 File Offset: 0x00074420
		[SecurityCritical]
		internal bool WaitOneWithoutFAS()
		{
			if (this.safeWaitHandle == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a disposed object."));
			}
			long num = -1L;
			int num2 = WaitHandle.WaitOneNative(this.safeWaitHandle, (uint)num, this.hasThreadAffinity, false);
			if (num2 == 128)
			{
				WaitHandle.ThrowAbandonedMutexException();
			}
			return num2 != 258 && num2 != int.MaxValue;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00076284 File Offset: 0x00074484
		[SecuritySafeCritical]
		public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
		{
			if (waitHandles == null)
			{
				throw new ArgumentNullException(Environment.GetResourceString("The waitHandles parameter cannot be null."));
			}
			if (waitHandles.Length == 0)
			{
				throw new ArgumentNullException(Environment.GetResourceString("Waithandle array may not be empty."));
			}
			if (waitHandles.Length > 64)
			{
				throw new NotSupportedException(Environment.GetResourceString("The number of WaitHandles must be less than or equal to 64."));
			}
			if (-1 > millisecondsTimeout)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			WaitHandle[] array = new WaitHandle[waitHandles.Length];
			for (int i = 0; i < waitHandles.Length; i++)
			{
				WaitHandle waitHandle = waitHandles[i];
				if (waitHandle == null)
				{
					throw new ArgumentNullException(Environment.GetResourceString("At least one element in the specified array was null."));
				}
				if (RemotingServices.IsTransparentProxy(waitHandle))
				{
					throw new InvalidOperationException(Environment.GetResourceString("Cannot wait on a transparent proxy."));
				}
				array[i] = waitHandle;
			}
			int num = WaitHandle.WaitMultiple(array, millisecondsTimeout, exitContext, true);
			if (128 <= num && 128 + array.Length > num)
			{
				WaitHandle.ThrowAbandonedMutexException();
			}
			GC.KeepAlive(array);
			return num != 258 && num != int.MaxValue;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00076370 File Offset: 0x00074570
		public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (-1L > num || 2147483647L < num)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			return WaitHandle.WaitAll(waitHandles, (int)num, exitContext);
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000763B2 File Offset: 0x000745B2
		public static bool WaitAll(WaitHandle[] waitHandles)
		{
			return WaitHandle.WaitAll(waitHandles, -1, true);
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x000763BC File Offset: 0x000745BC
		public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout)
		{
			return WaitHandle.WaitAll(waitHandles, millisecondsTimeout, true);
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000763C6 File Offset: 0x000745C6
		public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout)
		{
			return WaitHandle.WaitAll(waitHandles, timeout, true);
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000763D0 File Offset: 0x000745D0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecuritySafeCritical]
		public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
		{
			if (waitHandles == null)
			{
				throw new ArgumentNullException(Environment.GetResourceString("The waitHandles parameter cannot be null."));
			}
			if (waitHandles.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Waithandle array may not be empty."));
			}
			if (64 < waitHandles.Length)
			{
				throw new NotSupportedException(Environment.GetResourceString("The number of WaitHandles must be less than or equal to 64."));
			}
			if (-1 > millisecondsTimeout)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			WaitHandle[] array = new WaitHandle[waitHandles.Length];
			for (int i = 0; i < waitHandles.Length; i++)
			{
				WaitHandle waitHandle = waitHandles[i];
				if (waitHandle == null)
				{
					throw new ArgumentNullException(Environment.GetResourceString("At least one element in the specified array was null."));
				}
				if (RemotingServices.IsTransparentProxy(waitHandle))
				{
					throw new InvalidOperationException(Environment.GetResourceString("Cannot wait on a transparent proxy."));
				}
				array[i] = waitHandle;
			}
			int num = WaitHandle.WaitMultiple(array, millisecondsTimeout, exitContext, false);
			if (128 <= num && 128 + array.Length > num)
			{
				int num2 = num - 128;
				if (0 <= num2 && num2 < array.Length)
				{
					WaitHandle.ThrowAbandonedMutexException(num2, array[num2]);
				}
				else
				{
					WaitHandle.ThrowAbandonedMutexException();
				}
			}
			GC.KeepAlive(array);
			return num;
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000764CC File Offset: 0x000746CC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (-1L > num || 2147483647L < num)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			return WaitHandle.WaitAny(waitHandles, (int)num, exitContext);
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x0007650E File Offset: 0x0007470E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles, TimeSpan timeout)
		{
			return WaitHandle.WaitAny(waitHandles, timeout, true);
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00076518 File Offset: 0x00074718
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles)
		{
			return WaitHandle.WaitAny(waitHandles, -1, true);
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x00076522 File Offset: 0x00074722
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout)
		{
			return WaitHandle.WaitAny(waitHandles, millisecondsTimeout, true);
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x0007652C File Offset: 0x0007472C
		public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn)
		{
			return WaitHandle.SignalAndWait(toSignal, toWaitOn, -1, false);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00076538 File Offset: 0x00074738
		public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn, TimeSpan timeout, bool exitContext)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (-1L > num || 2147483647L < num)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			return WaitHandle.SignalAndWait(toSignal, toWaitOn, (int)num, exitContext);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x0007657C File Offset: 0x0007477C
		[SecuritySafeCritical]
		public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn, int millisecondsTimeout, bool exitContext)
		{
			if (toSignal == null)
			{
				throw new ArgumentNullException("toSignal");
			}
			if (toWaitOn == null)
			{
				throw new ArgumentNullException("toWaitOn");
			}
			if (-1 > millisecondsTimeout)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			int num = WaitHandle.SignalAndWaitOne(toSignal.safeWaitHandle, toWaitOn.safeWaitHandle, millisecondsTimeout, toWaitOn.hasThreadAffinity, exitContext);
			if (2147483647 != num && toSignal.hasThreadAffinity)
			{
				Thread.EndCriticalRegion();
				Thread.EndThreadAffinity();
			}
			if (128 == num)
			{
				WaitHandle.ThrowAbandonedMutexException();
			}
			if (298 == num)
			{
				throw new InvalidOperationException(Environment.GetResourceString("The WaitHandle cannot be signaled because it would exceed its maximum count."));
			}
			if (299 == num)
			{
				throw new ApplicationException("Attempt to release mutex not owned by caller");
			}
			return num == 0;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00076634 File Offset: 0x00074834
		private static void ThrowAbandonedMutexException()
		{
			throw new AbandonedMutexException();
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x0007663B File Offset: 0x0007483B
		private static void ThrowAbandonedMutexException(int location, WaitHandle handle)
		{
			throw new AbandonedMutexException(location, handle);
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00076644 File Offset: 0x00074844
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x00076653 File Offset: 0x00074853
		[SecuritySafeCritical]
		protected virtual void Dispose(bool explicitDisposing)
		{
			if (this.safeWaitHandle != null)
			{
				this.safeWaitHandle.Close();
			}
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00076644 File Offset: 0x00074844
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x0007666C File Offset: 0x0007486C
		private unsafe static int WaitOneNative(SafeHandle waitableSafeHandle, uint millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
		{
			bool flag = false;
			SynchronizationContext synchronizationContext = SynchronizationContext.Current;
			int result;
			try
			{
				waitableSafeHandle.DangerousAddRef(ref flag);
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				if (synchronizationContext != null && synchronizationContext.IsWaitNotificationRequired())
				{
					result = synchronizationContext.Wait(new IntPtr[]
					{
						waitableSafeHandle.DangerousGetHandle()
					}, false, (int)millisecondsTimeout);
				}
				else
				{
					IntPtr intPtr = waitableSafeHandle.DangerousGetHandle();
					result = WaitHandle.Wait_internal(&intPtr, 1, false, (int)millisecondsTimeout);
				}
			}
			finally
			{
				if (flag)
				{
					waitableSafeHandle.DangerousRelease();
				}
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
			}
			return result;
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000766F0 File Offset: 0x000748F0
		private unsafe static int WaitMultiple(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext, bool WaitAll)
		{
			if (waitHandles.Length > 64)
			{
				return int.MaxValue;
			}
			int num = -1;
			SynchronizationContext synchronizationContext = SynchronizationContext.Current;
			int result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				for (int i = 0; i < waitHandles.Length; i++)
				{
					try
					{
					}
					finally
					{
						bool flag = false;
						waitHandles[i].SafeWaitHandle.DangerousAddRef(ref flag);
						num = i;
					}
				}
				if (synchronizationContext != null && synchronizationContext.IsWaitNotificationRequired())
				{
					IntPtr[] array = new IntPtr[waitHandles.Length];
					for (int j = 0; j < waitHandles.Length; j++)
					{
						array[j] = waitHandles[j].SafeWaitHandle.DangerousGetHandle();
					}
					result = synchronizationContext.Wait(array, false, millisecondsTimeout);
				}
				else
				{
					IntPtr* ptr = stackalloc IntPtr[checked(unchecked((UIntPtr)waitHandles.Length) * (UIntPtr)sizeof(IntPtr))];
					for (int k = 0; k < waitHandles.Length; k++)
					{
						ptr[k] = waitHandles[k].SafeWaitHandle.DangerousGetHandle();
					}
					result = WaitHandle.Wait_internal(ptr, waitHandles.Length, WaitAll, millisecondsTimeout);
				}
			}
			finally
			{
				for (int l = num; l >= 0; l--)
				{
					waitHandles[l].SafeWaitHandle.DangerousRelease();
				}
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
			}
			return result;
		}

		// Token: 0x06002096 RID: 8342
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int Wait_internal(IntPtr* handles, int numHandles, bool waitAll, int ms);

		// Token: 0x06002097 RID: 8343 RVA: 0x0007681C File Offset: 0x00074A1C
		private static int SignalAndWaitOne(SafeWaitHandle waitHandleToSignal, SafeWaitHandle waitHandleToWaitOn, int millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
		{
			bool flag = false;
			bool flag2 = false;
			int result;
			try
			{
				waitHandleToSignal.DangerousAddRef(ref flag);
				waitHandleToWaitOn.DangerousAddRef(ref flag2);
				result = WaitHandle.SignalAndWait_Internal(waitHandleToSignal.DangerousGetHandle(), waitHandleToWaitOn.DangerousGetHandle(), millisecondsTimeout);
			}
			finally
			{
				if (flag)
				{
					waitHandleToSignal.DangerousRelease();
				}
				if (flag2)
				{
					waitHandleToWaitOn.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x06002098 RID: 8344
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SignalAndWait_Internal(IntPtr toSignal, IntPtr toWaitOn, int ms);

		// Token: 0x06002099 RID: 8345 RVA: 0x00076878 File Offset: 0x00074A78
		internal static int ToTimeoutMilliseconds(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", "Number must be either non-negative and less than or equal to Int32.MaxValue or -1.");
			}
			return (int)num;
		}

		// Token: 0x04001B5A RID: 7002
		public const int WaitTimeout = 258;

		// Token: 0x04001B5B RID: 7003
		private const int MAX_WAITHANDLES = 64;

		// Token: 0x04001B5C RID: 7004
		private IntPtr waitHandle;

		// Token: 0x04001B5D RID: 7005
		[SecurityCritical]
		internal volatile SafeWaitHandle safeWaitHandle;

		// Token: 0x04001B5E RID: 7006
		internal bool hasThreadAffinity;

		// Token: 0x04001B5F RID: 7007
		private const int WAIT_OBJECT_0 = 0;

		// Token: 0x04001B60 RID: 7008
		private const int WAIT_ABANDONED = 128;

		// Token: 0x04001B61 RID: 7009
		private const int WAIT_FAILED = 2147483647;

		// Token: 0x04001B62 RID: 7010
		private const int ERROR_TOO_MANY_POSTS = 298;

		// Token: 0x04001B63 RID: 7011
		private const int ERROR_NOT_OWNED_BY_CALLER = 299;

		// Token: 0x04001B64 RID: 7012
		protected static readonly IntPtr InvalidHandle = (IntPtr)(-1);

		// Token: 0x04001B65 RID: 7013
		internal const int MaxWaitHandles = 64;

		// Token: 0x020002EB RID: 747
		internal enum OpenExistingResult
		{
			// Token: 0x04001B67 RID: 7015
			Success,
			// Token: 0x04001B68 RID: 7016
			NameNotFound,
			// Token: 0x04001B69 RID: 7017
			PathNotFound,
			// Token: 0x04001B6A RID: 7018
			NameInvalid
		}
	}
}
