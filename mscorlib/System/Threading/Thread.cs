using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Principal;
using Internal.Runtime.Augments;

namespace System.Threading
{
	// Token: 0x020002D5 RID: 725
	[StructLayout(LayoutKind.Sequential)]
	public sealed class Thread : CriticalFinalizerObject, _Thread
	{
		// Token: 0x06001F63 RID: 8035 RVA: 0x00073C77 File Offset: 0x00071E77
		private static void AsyncLocalSetCurrentCulture(AsyncLocalValueChangedArgs<CultureInfo> args)
		{
			Thread.m_CurrentCulture = args.CurrentValue;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x00073C85 File Offset: 0x00071E85
		private static void AsyncLocalSetCurrentUICulture(AsyncLocalValueChangedArgs<CultureInfo> args)
		{
			Thread.m_CurrentUICulture = args.CurrentValue;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x00073C93 File Offset: 0x00071E93
		[SecuritySafeCritical]
		public Thread(ThreadStart start)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			this.SetStartHelper(start, 0);
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x00073CB1 File Offset: 0x00071EB1
		[SecuritySafeCritical]
		public Thread(ThreadStart start, int maxStackSize)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			if (0 > maxStackSize)
			{
				throw new ArgumentOutOfRangeException("maxStackSize", Environment.GetResourceString("Non-negative number required."));
			}
			this.SetStartHelper(start, maxStackSize);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x00073C93 File Offset: 0x00071E93
		[SecuritySafeCritical]
		public Thread(ParameterizedThreadStart start)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			this.SetStartHelper(start, 0);
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x00073CB1 File Offset: 0x00071EB1
		[SecuritySafeCritical]
		public Thread(ParameterizedThreadStart start, int maxStackSize)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			if (0 > maxStackSize)
			{
				throw new ArgumentOutOfRangeException("maxStackSize", Environment.GetResourceString("Non-negative number required."));
			}
			this.SetStartHelper(start, maxStackSize);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x00073CE8 File Offset: 0x00071EE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Start()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this.Start(ref stackCrawlMark);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x00073D00 File Offset: 0x00071F00
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Start(object parameter)
		{
			if (this.m_Delegate is ThreadStart)
			{
				throw new InvalidOperationException(Environment.GetResourceString("The thread was created with a ThreadStart delegate that does not accept a parameter."));
			}
			this.m_ThreadStartArg = parameter;
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this.Start(ref stackCrawlMark);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x00073D3C File Offset: 0x00071F3C
		[SecuritySafeCritical]
		private void Start(ref StackCrawlMark stackMark)
		{
			if (this.m_Delegate != null)
			{
				ThreadHelper threadHelper = (ThreadHelper)this.m_Delegate.Target;
				ExecutionContext executionContextHelper = ExecutionContext.Capture(ref stackMark, ExecutionContext.CaptureOptions.IgnoreSyncCtx);
				threadHelper.SetExecutionContextHelper(executionContextHelper);
			}
			object obj = null;
			this.StartInternal(obj, ref stackMark);
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x00073D79 File Offset: 0x00071F79
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal ExecutionContext.Reader GetExecutionContextReader()
		{
			return new ExecutionContext.Reader(this.m_ExecutionContext);
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x00073D86 File Offset: 0x00071F86
		// (set) Token: 0x06001F6E RID: 8046 RVA: 0x00073D91 File Offset: 0x00071F91
		internal bool ExecutionContextBelongsToCurrentScope
		{
			get
			{
				return !this.m_ExecutionContextBelongsToOuterScope;
			}
			set
			{
				this.m_ExecutionContextBelongsToOuterScope = !value;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001F6F RID: 8047 RVA: 0x00073DA0 File Offset: 0x00071FA0
		public ExecutionContext ExecutionContext
		{
			[SecuritySafeCritical]
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				ExecutionContext result;
				if (this == Thread.CurrentThread)
				{
					result = this.GetMutableExecutionContext();
				}
				else
				{
					result = this.m_ExecutionContext;
				}
				return result;
			}
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x00073DC8 File Offset: 0x00071FC8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityCritical]
		internal ExecutionContext GetMutableExecutionContext()
		{
			if (this.m_ExecutionContext == null)
			{
				this.m_ExecutionContext = new ExecutionContext();
			}
			else if (!this.ExecutionContextBelongsToCurrentScope)
			{
				ExecutionContext executionContext = this.m_ExecutionContext.CreateMutableCopy();
				this.m_ExecutionContext = executionContext;
			}
			this.ExecutionContextBelongsToCurrentScope = true;
			return this.m_ExecutionContext;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x00073E12 File Offset: 0x00072012
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void SetExecutionContext(ExecutionContext value, bool belongsToCurrentScope)
		{
			this.m_ExecutionContext = value;
			this.ExecutionContextBelongsToCurrentScope = belongsToCurrentScope;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x00073E22 File Offset: 0x00072022
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void SetExecutionContext(ExecutionContext.Reader value, bool belongsToCurrentScope)
		{
			this.m_ExecutionContext = value.DangerousGetRawExecutionContext();
			this.ExecutionContextBelongsToCurrentScope = belongsToCurrentScope;
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x00073E38 File Offset: 0x00072038
		[Obsolete("Thread.SetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
		public void SetCompressedStack(CompressedStack stack)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Use CompressedStack.(Capture/Run) or ExecutionContext.(Capture/Run) APIs instead."));
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x00073E38 File Offset: 0x00072038
		[SecurityCritical]
		[Obsolete("Thread.GetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
		public CompressedStack GetCompressedStack()
		{
			throw new InvalidOperationException(Environment.GetResourceString("Use CompressedStack.(Capture/Run) or ExecutionContext.(Capture/Run) APIs instead."));
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00073E49 File Offset: 0x00072049
		public static void ResetAbort()
		{
			Thread currentThread = Thread.CurrentThread;
			if ((currentThread.ThreadState & ThreadState.AbortRequested) == ThreadState.Running)
			{
				throw new ThreadStateException(Environment.GetResourceString("Unable to reset abort because no abort was requested."));
			}
			currentThread.ResetAbortNative();
			currentThread.ClearAbortReason();
		}

		// Token: 0x06001F76 RID: 8054
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResetAbortNative();

		// Token: 0x06001F77 RID: 8055 RVA: 0x00073E79 File Offset: 0x00072079
		[Obsolete("Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[SecuritySafeCritical]
		public void Suspend()
		{
			this.SuspendInternal();
		}

		// Token: 0x06001F78 RID: 8056
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SuspendInternal();

		// Token: 0x06001F79 RID: 8057 RVA: 0x00073E81 File Offset: 0x00072081
		[SecuritySafeCritical]
		[Obsolete("Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		public void Resume()
		{
			this.ResumeInternal();
		}

		// Token: 0x06001F7A RID: 8058
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResumeInternal();

		// Token: 0x06001F7B RID: 8059 RVA: 0x00073E89 File Offset: 0x00072089
		public void Interrupt()
		{
			this.InterruptInternal();
		}

		// Token: 0x06001F7C RID: 8060
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InterruptInternal();

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x00073E91 File Offset: 0x00072091
		// (set) Token: 0x06001F7E RID: 8062 RVA: 0x00073E99 File Offset: 0x00072099
		public ThreadPriority Priority
		{
			[SecuritySafeCritical]
			get
			{
				return (ThreadPriority)this.GetPriorityNative();
			}
			set
			{
				this.SetPriorityNative((int)value);
			}
		}

		// Token: 0x06001F7F RID: 8063
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPriorityNative();

		// Token: 0x06001F80 RID: 8064
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPriorityNative(int priority);

		// Token: 0x06001F81 RID: 8065
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool JoinInternal(int millisecondsTimeout);

		// Token: 0x06001F82 RID: 8066 RVA: 0x00073EA2 File Offset: 0x000720A2
		public void Join()
		{
			this.JoinInternal(-1);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x00073EAC File Offset: 0x000720AC
		public bool Join(int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			return this.JoinInternal(millisecondsTimeout);
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x00073ED0 File Offset: 0x000720D0
		public bool Join(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			return this.Join((int)num);
		}

		// Token: 0x06001F85 RID: 8069
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SleepInternal(int millisecondsTimeout);

		// Token: 0x06001F86 RID: 8070 RVA: 0x00073F11 File Offset: 0x00072111
		[SecuritySafeCritical]
		public static void Sleep(int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			Thread.SleepInternal(millisecondsTimeout);
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x00073F34 File Offset: 0x00072134
		public static void Sleep(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			Thread.Sleep((int)num);
		}

		// Token: 0x06001F88 RID: 8072
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool YieldInternal();

		// Token: 0x06001F89 RID: 8073 RVA: 0x00073F74 File Offset: 0x00072174
		public static bool Yield()
		{
			return Thread.YieldInternal();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x00073F7C File Offset: 0x0007217C
		[SecurityCritical]
		private void SetStartHelper(Delegate start, int maxStackSize)
		{
			maxStackSize = Thread.GetProcessDefaultStackSize(maxStackSize);
			ThreadHelper @object = new ThreadHelper(start);
			if (start is ThreadStart)
			{
				this.SetStart(new ThreadStart(@object.ThreadStart), maxStackSize);
				return;
			}
			this.SetStart(new ParameterizedThreadStart(@object.ThreadStart), maxStackSize);
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x00073FC7 File Offset: 0x000721C7
		public static LocalDataStoreSlot AllocateDataSlot()
		{
			return Thread.LocalDataStoreManager.AllocateDataSlot();
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x00073FD3 File Offset: 0x000721D3
		public static LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			return Thread.LocalDataStoreManager.AllocateNamedDataSlot(name);
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x00073FE0 File Offset: 0x000721E0
		public static LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			return Thread.LocalDataStoreManager.GetNamedDataSlot(name);
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x00073FED File Offset: 0x000721ED
		public static void FreeNamedDataSlot(string name)
		{
			Thread.LocalDataStoreManager.FreeNamedDataSlot(name);
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x00073FFC File Offset: 0x000721FC
		public static object GetData(LocalDataStoreSlot slot)
		{
			LocalDataStoreHolder localDataStoreHolder = Thread.s_LocalDataStore;
			if (localDataStoreHolder == null)
			{
				Thread.LocalDataStoreManager.ValidateSlot(slot);
				return null;
			}
			return localDataStoreHolder.Store.GetData(slot);
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0007402C File Offset: 0x0007222C
		public static void SetData(LocalDataStoreSlot slot, object data)
		{
			LocalDataStoreHolder localDataStoreHolder = Thread.s_LocalDataStore;
			if (localDataStoreHolder == null)
			{
				localDataStoreHolder = Thread.LocalDataStoreManager.CreateLocalDataStore();
				Thread.s_LocalDataStore = localDataStoreHolder;
			}
			localDataStoreHolder.Store.SetData(slot, data);
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x00074060 File Offset: 0x00072260
		// (set) Token: 0x06001F92 RID: 8082 RVA: 0x00074080 File Offset: 0x00072280
		public CultureInfo CurrentUICulture
		{
			get
			{
				if (AppDomain.IsAppXModel())
				{
					return CultureInfo.GetCultureInfoForUserPreferredLanguageInAppX() ?? this.GetCurrentUICultureNoAppX();
				}
				return this.GetCurrentUICultureNoAppX();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				CultureInfo.VerifyCultureName(value, true);
				if (AppDomain.IsAppXModel())
				{
					CultureInfo.SetCultureInfoForUserPreferredLanguageInAppX(value);
					return;
				}
				if (Thread.m_CurrentUICulture == null && Thread.m_CurrentCulture == null)
				{
					Thread.nativeInitCultureAccessors();
				}
				if (!AppContextSwitches.NoAsyncCurrentCulture)
				{
					if (Thread.s_asyncLocalCurrentUICulture == null)
					{
						Interlocked.CompareExchange<AsyncLocal<CultureInfo>>(ref Thread.s_asyncLocalCurrentUICulture, new AsyncLocal<CultureInfo>(new Action<AsyncLocalValueChangedArgs<CultureInfo>>(Thread.AsyncLocalSetCurrentUICulture)), null);
					}
					Thread.s_asyncLocalCurrentUICulture.Value = value;
					return;
				}
				Thread.m_CurrentUICulture = value;
			}
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x00074104 File Offset: 0x00072304
		internal CultureInfo GetCurrentUICultureNoAppX()
		{
			if (Thread.m_CurrentUICulture != null)
			{
				return Thread.m_CurrentUICulture;
			}
			CultureInfo defaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture;
			if (defaultThreadCurrentUICulture == null)
			{
				return CultureInfo.UserDefaultUICulture;
			}
			return defaultThreadCurrentUICulture;
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001F94 RID: 8084 RVA: 0x0007412E File Offset: 0x0007232E
		// (set) Token: 0x06001F95 RID: 8085 RVA: 0x00074150 File Offset: 0x00072350
		public CultureInfo CurrentCulture
		{
			get
			{
				if (AppDomain.IsAppXModel())
				{
					return CultureInfo.GetCultureInfoForUserPreferredLanguageInAppX() ?? this.GetCurrentCultureNoAppX();
				}
				return this.GetCurrentCultureNoAppX();
			}
			[SecuritySafeCritical]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (AppDomain.IsAppXModel())
				{
					CultureInfo.SetCultureInfoForUserPreferredLanguageInAppX(value);
					return;
				}
				if (Thread.m_CurrentCulture == null && Thread.m_CurrentUICulture == null)
				{
					Thread.nativeInitCultureAccessors();
				}
				if (!AppContextSwitches.NoAsyncCurrentCulture)
				{
					if (Thread.s_asyncLocalCurrentCulture == null)
					{
						Interlocked.CompareExchange<AsyncLocal<CultureInfo>>(ref Thread.s_asyncLocalCurrentCulture, new AsyncLocal<CultureInfo>(new Action<AsyncLocalValueChangedArgs<CultureInfo>>(Thread.AsyncLocalSetCurrentCulture)), null);
					}
					Thread.s_asyncLocalCurrentCulture.Value = value;
					return;
				}
				Thread.m_CurrentCulture = value;
			}
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x000741CC File Offset: 0x000723CC
		private CultureInfo GetCurrentCultureNoAppX()
		{
			if (Thread.m_CurrentCulture != null)
			{
				return Thread.m_CurrentCulture;
			}
			CultureInfo defaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
			if (defaultThreadCurrentCulture == null)
			{
				return CultureInfo.UserDefaultCulture;
			}
			return defaultThreadCurrentCulture;
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x000741F6 File Offset: 0x000723F6
		private static void nativeInitCultureAccessors()
		{
			Thread.m_CurrentCulture = CultureInfo.ConstructCurrentCulture();
			Thread.m_CurrentUICulture = CultureInfo.ConstructCurrentUICulture();
		}

		// Token: 0x06001F98 RID: 8088
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemoryBarrier();

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0007420C File Offset: 0x0007240C
		private static LocalDataStoreMgr LocalDataStoreManager
		{
			get
			{
				if (Thread.s_LocalDataStoreMgr == null)
				{
					Interlocked.CompareExchange<LocalDataStoreMgr>(ref Thread.s_LocalDataStoreMgr, new LocalDataStoreMgr(), null);
				}
				return Thread.s_LocalDataStoreMgr;
			}
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Thread.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Thread.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Thread.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Thread.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F9E RID: 8094
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ConstructInternalThread();

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x0007422B File Offset: 0x0007242B
		private InternalThread Internal
		{
			get
			{
				if (this.internal_thread == null)
				{
					this.ConstructInternalThread();
				}
				return this.internal_thread;
			}
		}

		// Token: 0x06001FA0 RID: 8096
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte[] ByteArrayToRootDomain(byte[] arr);

		// Token: 0x06001FA1 RID: 8097
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte[] ByteArrayToCurrentDomain(byte[] arr);

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x00074241 File Offset: 0x00072441
		public static Context CurrentContext
		{
			get
			{
				return AppDomain.InternalGetContext();
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x00074248 File Offset: 0x00072448
		private static void DeserializePrincipal(Thread th)
		{
			MemoryStream memoryStream = new MemoryStream(Thread.ByteArrayToCurrentDomain(th.Internal._serialized_principal));
			int num = memoryStream.ReadByte();
			if (num == 0)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				th.principal = (IPrincipal)binaryFormatter.Deserialize(memoryStream);
				th.principal_version = th.Internal._serialized_principal_version;
				return;
			}
			if (num == 1)
			{
				BinaryReader binaryReader = new BinaryReader(memoryStream);
				string name = binaryReader.ReadString();
				string type = binaryReader.ReadString();
				int num2 = binaryReader.ReadInt32();
				string[] array = null;
				if (num2 >= 0)
				{
					array = new string[num2];
					for (int i = 0; i < num2; i++)
					{
						array[i] = binaryReader.ReadString();
					}
				}
				th.principal = new GenericPrincipal(new GenericIdentity(name, type), array);
				return;
			}
			if (num == 2 || num == 3)
			{
				string[] roles = (num == 2) ? null : new string[0];
				th.principal = new GenericPrincipal(new GenericIdentity("", ""), roles);
			}
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0007433C File Offset: 0x0007253C
		private static void SerializePrincipal(Thread th, IPrincipal value)
		{
			MemoryStream memoryStream = new MemoryStream();
			bool flag = false;
			if (value.GetType() == typeof(GenericPrincipal))
			{
				GenericPrincipal genericPrincipal = (GenericPrincipal)value;
				if (genericPrincipal.Identity != null && genericPrincipal.Identity.GetType() == typeof(GenericIdentity))
				{
					GenericIdentity genericIdentity = (GenericIdentity)genericPrincipal.Identity;
					if (genericIdentity.Name == "" && genericIdentity.AuthenticationType == "")
					{
						if (genericPrincipal.Roles == null)
						{
							memoryStream.WriteByte(2);
							flag = true;
						}
						else if (genericPrincipal.Roles.Length == 0)
						{
							memoryStream.WriteByte(3);
							flag = true;
						}
					}
					else
					{
						memoryStream.WriteByte(1);
						BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
						binaryWriter.Write(genericPrincipal.Identity.Name);
						binaryWriter.Write(genericPrincipal.Identity.AuthenticationType);
						string[] roles = genericPrincipal.Roles;
						if (roles == null)
						{
							binaryWriter.Write(-1);
						}
						else
						{
							binaryWriter.Write(roles.Length);
							foreach (string value2 in roles)
							{
								binaryWriter.Write(value2);
							}
						}
						binaryWriter.Flush();
						flag = true;
					}
				}
			}
			if (!flag)
			{
				memoryStream.WriteByte(0);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				try
				{
					binaryFormatter.Serialize(memoryStream, value);
				}
				catch
				{
				}
			}
			th.Internal._serialized_principal = Thread.ByteArrayToRootDomain(memoryStream.ToArray());
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x000744C8 File Offset: 0x000726C8
		// (set) Token: 0x06001FA6 RID: 8102 RVA: 0x0007457C File Offset: 0x0007277C
		public static IPrincipal CurrentPrincipal
		{
			get
			{
				Thread currentThread = Thread.CurrentThread;
				IPrincipal principal = currentThread.GetExecutionContextReader().LogicalCallContext.Principal;
				if (principal != null)
				{
					return principal;
				}
				if (currentThread.principal_version != currentThread.Internal._serialized_principal_version)
				{
					currentThread.principal = null;
				}
				if (currentThread.principal != null)
				{
					return currentThread.principal;
				}
				if (currentThread.Internal._serialized_principal != null)
				{
					try
					{
						Thread.DeserializePrincipal(currentThread);
						return currentThread.principal;
					}
					catch
					{
					}
				}
				currentThread.principal = Thread.GetDomain().DefaultPrincipal;
				currentThread.principal_version = currentThread.Internal._serialized_principal_version;
				return currentThread.principal;
			}
			set
			{
				Thread currentThread = Thread.CurrentThread;
				currentThread.GetMutableExecutionContext().LogicalCallContext.Principal = value;
				if (value != Thread.GetDomain().DefaultPrincipal)
				{
					currentThread.Internal._serialized_principal_version++;
					try
					{
						Thread.SerializePrincipal(currentThread, value);
					}
					catch (Exception)
					{
						currentThread.Internal._serialized_principal = null;
					}
					currentThread.principal_version = currentThread.Internal._serialized_principal_version;
				}
				else
				{
					currentThread.Internal._serialized_principal = null;
				}
				currentThread.principal = value;
			}
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x00074610 File Offset: 0x00072810
		public static AppDomain GetDomain()
		{
			return AppDomain.CurrentDomain;
		}

		// Token: 0x06001FA8 RID: 8104
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCurrentThread_icall(ref Thread thread);

		// Token: 0x06001FA9 RID: 8105 RVA: 0x00074618 File Offset: 0x00072818
		private static Thread GetCurrentThread()
		{
			Thread result = null;
			Thread.GetCurrentThread_icall(ref result);
			return result;
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x00074630 File Offset: 0x00072830
		public static Thread CurrentThread
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				Thread thread = Thread.current_thread;
				if (thread != null)
				{
					return thread;
				}
				return Thread.GetCurrentThread();
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x0007464D File Offset: 0x0007284D
		internal static int CurrentThreadId
		{
			get
			{
				return (int)Thread.CurrentThread.internal_thread.thread_id;
			}
		}

		// Token: 0x06001FAC RID: 8108
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetDomainID();

		// Token: 0x06001FAD RID: 8109
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Thread_internal(MulticastDelegate start);

		// Token: 0x06001FAE RID: 8110 RVA: 0x0007465F File Offset: 0x0007285F
		private Thread(InternalThread it)
		{
			this.internal_thread = it;
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x00074670 File Offset: 0x00072870
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		~Thread()
		{
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x00074698 File Offset: 0x00072898
		// (set) Token: 0x06001FB1 RID: 8113 RVA: 0x000746AC File Offset: 0x000728AC
		[Obsolete("Deprecated in favor of GetApartmentState, SetApartmentState and TrySetApartmentState.")]
		public ApartmentState ApartmentState
		{
			get
			{
				this.ValidateThreadState();
				return (ApartmentState)this.Internal.apartment_state;
			}
			set
			{
				this.ValidateThreadState();
				this.TrySetApartmentState(value);
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x000746BD File Offset: 0x000728BD
		public bool IsThreadPoolThread
		{
			get
			{
				return this.IsThreadPoolThreadInternal;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x000746C5 File Offset: 0x000728C5
		// (set) Token: 0x06001FB4 RID: 8116 RVA: 0x000746D2 File Offset: 0x000728D2
		internal bool IsThreadPoolThreadInternal
		{
			get
			{
				return this.Internal.threadpool_thread;
			}
			set
			{
				this.Internal.threadpool_thread = value;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x000746E0 File Offset: 0x000728E0
		public bool IsAlive
		{
			get
			{
				ThreadState state = Thread.GetState(this.Internal);
				return (state & ThreadState.Aborted) == ThreadState.Running && (state & ThreadState.Stopped) == ThreadState.Running && (state & ThreadState.Unstarted) == ThreadState.Running;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x00074710 File Offset: 0x00072910
		// (set) Token: 0x06001FB7 RID: 8119 RVA: 0x0007471D File Offset: 0x0007291D
		public bool IsBackground
		{
			get
			{
				return (this.ValidateThreadState() & ThreadState.Background) > ThreadState.Running;
			}
			set
			{
				this.ValidateThreadState();
				if (value)
				{
					Thread.SetState(this.Internal, ThreadState.Background);
					return;
				}
				Thread.ClrState(this.Internal, ThreadState.Background);
			}
		}

		// Token: 0x06001FB8 RID: 8120
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName_internal(InternalThread thread);

		// Token: 0x06001FB9 RID: 8121
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void SetName_icall(InternalThread thread, char* name, int nameLength);

		// Token: 0x06001FBA RID: 8122 RVA: 0x00074744 File Offset: 0x00072944
		private unsafe static void SetName_internal(InternalThread thread, string name)
		{
			fixed (string text = name)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				Thread.SetName_icall(thread, ptr, (name != null) ? name.Length : 0);
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001FBB RID: 8123 RVA: 0x00074776 File Offset: 0x00072976
		// (set) Token: 0x06001FBC RID: 8124 RVA: 0x00074783 File Offset: 0x00072983
		public string Name
		{
			get
			{
				return Thread.GetName_internal(this.Internal);
			}
			set
			{
				Thread.SetName_internal(this.Internal, value);
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x00074791 File Offset: 0x00072991
		public ThreadState ThreadState
		{
			get
			{
				return Thread.GetState(this.Internal);
			}
		}

		// Token: 0x06001FBE RID: 8126
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Abort_internal(InternalThread thread, object stateInfo);

		// Token: 0x06001FBF RID: 8127 RVA: 0x0007479E File Offset: 0x0007299E
		public void Abort()
		{
			Thread.Abort_internal(this.Internal, null);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000747AC File Offset: 0x000729AC
		public void Abort(object stateInfo)
		{
			Thread.Abort_internal(this.Internal, stateInfo);
		}

		// Token: 0x06001FC1 RID: 8129
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object GetAbortExceptionState();

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001FC2 RID: 8130 RVA: 0x000747BA File Offset: 0x000729BA
		internal object AbortReason
		{
			get
			{
				return this.GetAbortExceptionState();
			}
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x00004BF9 File Offset: 0x00002DF9
		private void ClearAbortReason()
		{
		}

		// Token: 0x06001FC4 RID: 8132
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SpinWait_nop();

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000747C2 File Offset: 0x000729C2
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void SpinWait(int iterations)
		{
			if (iterations < 0)
			{
				return;
			}
			while (iterations-- > 0)
			{
				Thread.SpinWait_nop();
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000747D7 File Offset: 0x000729D7
		private void StartInternal(object principal, ref StackCrawlMark stackMark)
		{
			this.Internal._serialized_principal = Thread.CurrentThread.Internal._serialized_principal;
			if (!this.Thread_internal(this.m_Delegate))
			{
				throw new SystemException("Thread creation failed.");
			}
			this.m_ThreadStartArg = null;
		}

		// Token: 0x06001FC7 RID: 8135
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetState(InternalThread thread, ThreadState set);

		// Token: 0x06001FC8 RID: 8136
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClrState(InternalThread thread, ThreadState clr);

		// Token: 0x06001FC9 RID: 8137
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ThreadState GetState(InternalThread thread);

		// Token: 0x06001FCA RID: 8138
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte VolatileRead(ref byte address);

		// Token: 0x06001FCB RID: 8139
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double VolatileRead(ref double address);

		// Token: 0x06001FCC RID: 8140
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern short VolatileRead(ref short address);

		// Token: 0x06001FCD RID: 8141
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int VolatileRead(ref int address);

		// Token: 0x06001FCE RID: 8142
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long VolatileRead(ref long address);

		// Token: 0x06001FCF RID: 8143
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr VolatileRead(ref IntPtr address);

		// Token: 0x06001FD0 RID: 8144
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object VolatileRead(ref object address);

		// Token: 0x06001FD1 RID: 8145
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern sbyte VolatileRead(ref sbyte address);

		// Token: 0x06001FD2 RID: 8146
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float VolatileRead(ref float address);

		// Token: 0x06001FD3 RID: 8147
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort VolatileRead(ref ushort address);

		// Token: 0x06001FD4 RID: 8148
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint VolatileRead(ref uint address);

		// Token: 0x06001FD5 RID: 8149
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong VolatileRead(ref ulong address);

		// Token: 0x06001FD6 RID: 8150
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern UIntPtr VolatileRead(ref UIntPtr address);

		// Token: 0x06001FD7 RID: 8151
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref byte address, byte value);

		// Token: 0x06001FD8 RID: 8152
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref double address, double value);

		// Token: 0x06001FD9 RID: 8153
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref short address, short value);

		// Token: 0x06001FDA RID: 8154
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref int address, int value);

		// Token: 0x06001FDB RID: 8155
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref long address, long value);

		// Token: 0x06001FDC RID: 8156
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref IntPtr address, IntPtr value);

		// Token: 0x06001FDD RID: 8157
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref object address, object value);

		// Token: 0x06001FDE RID: 8158
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref sbyte address, sbyte value);

		// Token: 0x06001FDF RID: 8159
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref float address, float value);

		// Token: 0x06001FE0 RID: 8160
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref ushort address, ushort value);

		// Token: 0x06001FE1 RID: 8161
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref uint address, uint value);

		// Token: 0x06001FE2 RID: 8162
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref ulong address, ulong value);

		// Token: 0x06001FE3 RID: 8163
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void VolatileWrite(ref UIntPtr address, UIntPtr value);

		// Token: 0x06001FE4 RID: 8164
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SystemMaxStackStize();

		// Token: 0x06001FE5 RID: 8165 RVA: 0x00074814 File Offset: 0x00072A14
		private static int GetProcessDefaultStackSize(int maxStackSize)
		{
			if (maxStackSize == 0)
			{
				return 0;
			}
			if (maxStackSize < 131072)
			{
				return 131072;
			}
			int pageSize = Environment.GetPageSize();
			if (maxStackSize % pageSize != 0)
			{
				maxStackSize = maxStackSize / (pageSize - 1) * pageSize;
			}
			return Math.Min(maxStackSize, Thread.SystemMaxStackStize());
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00074853 File Offset: 0x00072A53
		private void SetStart(MulticastDelegate start, int maxStackSize)
		{
			this.m_Delegate = start;
			this.Internal.stack_size = maxStackSize;
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x00074868 File Offset: 0x00072A68
		public int ManagedThreadId
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.Internal.managed_id;
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x00074875 File Offset: 0x00072A75
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void BeginCriticalRegion()
		{
			Thread.CurrentThread.Internal.critical_region_level++;
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x00074892 File Offset: 0x00072A92
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void EndCriticalRegion()
		{
			Thread.CurrentThread.Internal.critical_region_level--;
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void BeginThreadAffinity()
		{
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void EndThreadAffinity()
		{
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x00074698 File Offset: 0x00072898
		public ApartmentState GetApartmentState()
		{
			this.ValidateThreadState();
			return (ApartmentState)this.Internal.apartment_state;
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000748AF File Offset: 0x00072AAF
		public void SetApartmentState(ApartmentState state)
		{
			if (!this.TrySetApartmentState(state))
			{
				throw new InvalidOperationException("Failed to set the specified COM apartment state.");
			}
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000748C8 File Offset: 0x00072AC8
		public bool TrySetApartmentState(ApartmentState state)
		{
			if ((this.ThreadState & ThreadState.Unstarted) == ThreadState.Running)
			{
				throw new ThreadStateException("Thread was in an invalid state for the operation being executed.");
			}
			if (this.Internal.apartment_state != 2 && (ApartmentState)this.Internal.apartment_state != state)
			{
				return false;
			}
			this.Internal.apartment_state = (byte)state;
			return true;
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00074916 File Offset: 0x00072B16
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return this.ManagedThreadId;
		}

		// Token: 0x06001FF0 RID: 8176
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetStackTraces(out Thread[] threads, out object[] stack_frames);

		// Token: 0x06001FF1 RID: 8177 RVA: 0x00074920 File Offset: 0x00072B20
		internal static Dictionary<Thread, StackTrace> Mono_GetStackTraces()
		{
			Thread[] array;
			object[] array2;
			Thread.GetStackTraces(out array, out array2);
			Dictionary<Thread, StackTrace> dictionary = new Dictionary<Thread, StackTrace>();
			for (int i = 0; i < array.Length; i++)
			{
				dictionary[array[i]] = new StackTrace((StackFrame[])array2[i]);
			}
			return dictionary;
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0001B98F File Offset: 0x00019B8F
		public void DisableComObjectEagerCleanup()
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00074961 File Offset: 0x00072B61
		private ThreadState ValidateThreadState()
		{
			ThreadState state = Thread.GetState(this.Internal);
			if ((state & ThreadState.Stopped) != ThreadState.Running)
			{
				throw new ThreadStateException("Thread is dead; state can not be accessed.");
			}
			return state;
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0007497F File Offset: 0x00072B7F
		public static int GetCurrentProcessorId()
		{
			return RuntimeThread.GetCurrentProcessorId();
		}

		// Token: 0x04001B15 RID: 6933
		private static LocalDataStoreMgr s_LocalDataStoreMgr;

		// Token: 0x04001B16 RID: 6934
		[ThreadStatic]
		private static LocalDataStoreHolder s_LocalDataStore;

		// Token: 0x04001B17 RID: 6935
		[ThreadStatic]
		internal static CultureInfo m_CurrentCulture;

		// Token: 0x04001B18 RID: 6936
		[ThreadStatic]
		internal static CultureInfo m_CurrentUICulture;

		// Token: 0x04001B19 RID: 6937
		private static AsyncLocal<CultureInfo> s_asyncLocalCurrentCulture;

		// Token: 0x04001B1A RID: 6938
		private static AsyncLocal<CultureInfo> s_asyncLocalCurrentUICulture;

		// Token: 0x04001B1B RID: 6939
		private InternalThread internal_thread;

		// Token: 0x04001B1C RID: 6940
		private object m_ThreadStartArg;

		// Token: 0x04001B1D RID: 6941
		private object pending_exception;

		// Token: 0x04001B1E RID: 6942
		[ThreadStatic]
		private static Thread current_thread;

		// Token: 0x04001B1F RID: 6943
		private MulticastDelegate m_Delegate;

		// Token: 0x04001B20 RID: 6944
		private ExecutionContext m_ExecutionContext;

		// Token: 0x04001B21 RID: 6945
		private bool m_ExecutionContextBelongsToOuterScope;

		// Token: 0x04001B22 RID: 6946
		private IPrincipal principal;

		// Token: 0x04001B23 RID: 6947
		private int principal_version;
	}
}
