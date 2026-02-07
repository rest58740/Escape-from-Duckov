using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;

namespace System.Threading
{
	// Token: 0x020002F3 RID: 755
	[ComVisible(true)]
	public sealed class Mutex : WaitHandle
	{
		// Token: 0x060020D9 RID: 8409
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr CreateMutex_icall(bool initiallyOwned, char* name, int name_length, out bool created);

		// Token: 0x060020DA RID: 8410
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr OpenMutex_icall(char* name, int name_length, MutexRights rights, out MonoIOError error);

		// Token: 0x060020DB RID: 8411
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ReleaseMutex_internal(IntPtr handle);

		// Token: 0x060020DC RID: 8412 RVA: 0x00076C4C File Offset: 0x00074E4C
		private unsafe static IntPtr CreateMutex_internal(bool initiallyOwned, string name, out bool created)
		{
			char* ptr = name;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return Mutex.CreateMutex_icall(initiallyOwned, ptr, (name != null) ? name.Length : 0, out created);
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00076C80 File Offset: 0x00074E80
		private unsafe static IntPtr OpenMutex_internal(string name, MutexRights rights, out MonoIOError error)
		{
			char* ptr = name;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return Mutex.OpenMutex_icall(ptr, (name != null) ? name.Length : 0, rights, out error);
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00076CB1 File Offset: 0x00074EB1
		private Mutex(IntPtr handle)
		{
			this.Handle = handle;
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00076CC0 File Offset: 0x00074EC0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Mutex()
		{
			bool flag;
			this.Handle = Mutex.CreateMutex_internal(false, null, out flag);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00076CE4 File Offset: 0x00074EE4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Mutex(bool initiallyOwned)
		{
			bool flag;
			this.Handle = Mutex.CreateMutex_internal(initiallyOwned, null, out flag);
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00076D08 File Offset: 0x00074F08
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public Mutex(bool initiallyOwned, string name)
		{
			bool flag;
			this.Handle = Mutex.CreateMutex_internal(initiallyOwned, name, out flag);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00076D2A File Offset: 0x00074F2A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public Mutex(bool initiallyOwned, string name, out bool createdNew)
		{
			this.Handle = Mutex.CreateMutex_internal(initiallyOwned, name, out createdNew);
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00076D2A File Offset: 0x00074F2A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MonoTODO("Use MutexSecurity in CreateMutex_internal")]
		public Mutex(bool initiallyOwned, string name, out bool createdNew, MutexSecurity mutexSecurity)
		{
			this.Handle = Mutex.CreateMutex_internal(initiallyOwned, name, out createdNew);
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00076D40 File Offset: 0x00074F40
		public MutexSecurity GetAccessControl()
		{
			return new MutexSecurity(base.SafeWaitHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00076D4F File Offset: 0x00074F4F
		public static Mutex OpenExisting(string name)
		{
			return Mutex.OpenExisting(name, MutexRights.Modify | MutexRights.Synchronize);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00076D5C File Offset: 0x00074F5C
		public static Mutex OpenExisting(string name, MutexRights rights)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0 || name.Length > 260)
			{
				throw new ArgumentException("name", Locale.GetText("Invalid length [1-260]."));
			}
			MonoIOError monoIOError;
			IntPtr intPtr = Mutex.OpenMutex_internal(name, rights, out monoIOError);
			if (!(intPtr == (IntPtr)null))
			{
				return new Mutex(intPtr);
			}
			if (monoIOError == MonoIOError.ERROR_FILE_NOT_FOUND)
			{
				throw new WaitHandleCannotBeOpenedException(Locale.GetText("Named Mutex handle does not exist: ") + name);
			}
			if (monoIOError == MonoIOError.ERROR_ACCESS_DENIED)
			{
				throw new UnauthorizedAccessException();
			}
			throw new IOException(Locale.GetText("Win32 IO error: ") + monoIOError.ToString());
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x00076E04 File Offset: 0x00075004
		public static bool TryOpenExisting(string name, out Mutex result)
		{
			return Mutex.TryOpenExisting(name, MutexRights.Modify | MutexRights.Synchronize, out result);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00076E14 File Offset: 0x00075014
		public static bool TryOpenExisting(string name, MutexRights rights, out Mutex result)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0 || name.Length > 260)
			{
				throw new ArgumentException("name", Locale.GetText("Invalid length [1-260]."));
			}
			MonoIOError monoIOError;
			IntPtr intPtr = Mutex.OpenMutex_internal(name, rights, out monoIOError);
			if (intPtr == (IntPtr)null)
			{
				result = null;
				return false;
			}
			result = new Mutex(intPtr);
			return true;
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00076E80 File Offset: 0x00075080
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void ReleaseMutex()
		{
			if (!Mutex.ReleaseMutex_internal(this.Handle))
			{
				throw new ApplicationException("Mutex is not owned");
			}
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00076E9A File Offset: 0x0007509A
		public void SetAccessControl(MutexSecurity mutexSecurity)
		{
			if (mutexSecurity == null)
			{
				throw new ArgumentNullException("mutexSecurity");
			}
			mutexSecurity.PersistModifications(base.SafeWaitHandle);
		}
	}
}
