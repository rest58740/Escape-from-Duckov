using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.Threading
{
	// Token: 0x020002C6 RID: 710
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
	public class EventWaitHandle : WaitHandle
	{
		// Token: 0x06001ED0 RID: 7888 RVA: 0x000727F6 File Offset: 0x000709F6
		[SecuritySafeCritical]
		public EventWaitHandle(bool initialState, EventResetMode mode) : this(initialState, mode, null)
		{
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x00072804 File Offset: 0x00070A04
		[SecurityCritical]
		public EventWaitHandle(bool initialState, EventResetMode mode, string name)
		{
			if (name != null && 260 < name.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("The name can be no more than 260 characters in length.", new object[]
				{
					name
				}));
			}
			int num;
			SafeWaitHandle safeWaitHandle;
			if (mode != EventResetMode.AutoReset)
			{
				if (mode != EventResetMode.ManualReset)
				{
					throw new ArgumentException(Environment.GetResourceString("Value of flags is invalid.", new object[]
					{
						name
					}));
				}
				safeWaitHandle = new SafeWaitHandle(NativeEventCalls.CreateEvent_internal(true, initialState, name, out num), true);
			}
			else
			{
				safeWaitHandle = new SafeWaitHandle(NativeEventCalls.CreateEvent_internal(false, initialState, name, out num), true);
			}
			if (safeWaitHandle.IsInvalid)
			{
				safeWaitHandle.SetHandleAsInvalid();
				if (name != null && name.Length != 0 && 6 == num)
				{
					throw new WaitHandleCannotBeOpenedException(Environment.GetResourceString("A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.", new object[]
					{
						name
					}));
				}
				__Error.WinIOError(num, name);
			}
			base.SetHandleInternal(safeWaitHandle);
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000728CF File Offset: 0x00070ACF
		[SecurityCritical]
		public EventWaitHandle(bool initialState, EventResetMode mode, string name, out bool createdNew) : this(initialState, mode, name, out createdNew, null)
		{
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000728E0 File Offset: 0x00070AE0
		[SecurityCritical]
		public EventWaitHandle(bool initialState, EventResetMode mode, string name, out bool createdNew, EventWaitHandleSecurity eventSecurity)
		{
			if (name != null && 260 < name.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("The name can be no more than 260 characters in length.", new object[]
				{
					name
				}));
			}
			bool manual;
			if (mode != EventResetMode.AutoReset)
			{
				if (mode != EventResetMode.ManualReset)
				{
					throw new ArgumentException(Environment.GetResourceString("Value of flags is invalid.", new object[]
					{
						name
					}));
				}
				manual = true;
			}
			else
			{
				manual = false;
			}
			int num;
			SafeWaitHandle safeWaitHandle = new SafeWaitHandle(NativeEventCalls.CreateEvent_internal(manual, initialState, name, out num), true);
			if (safeWaitHandle.IsInvalid)
			{
				safeWaitHandle.SetHandleAsInvalid();
				if (name != null && name.Length != 0 && 6 == num)
				{
					throw new WaitHandleCannotBeOpenedException(Environment.GetResourceString("A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.", new object[]
					{
						name
					}));
				}
				__Error.WinIOError(num, name);
			}
			createdNew = (num != 183);
			base.SetHandleInternal(safeWaitHandle);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000729AC File Offset: 0x00070BAC
		[SecurityCritical]
		private EventWaitHandle(SafeWaitHandle handle)
		{
			base.SetHandleInternal(handle);
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000729BB File Offset: 0x00070BBB
		[SecurityCritical]
		public static EventWaitHandle OpenExisting(string name)
		{
			return EventWaitHandle.OpenExisting(name, EventWaitHandleRights.Modify | EventWaitHandleRights.Synchronize);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000729C8 File Offset: 0x00070BC8
		[SecurityCritical]
		public static EventWaitHandle OpenExisting(string name, EventWaitHandleRights rights)
		{
			EventWaitHandle result;
			switch (EventWaitHandle.OpenExistingWorker(name, rights, out result))
			{
			case WaitHandle.OpenExistingResult.NameNotFound:
				throw new WaitHandleCannotBeOpenedException();
			case WaitHandle.OpenExistingResult.PathNotFound:
				__Error.WinIOError(3, "");
				return result;
			case WaitHandle.OpenExistingResult.NameInvalid:
				throw new WaitHandleCannotBeOpenedException(Environment.GetResourceString("A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.", new object[]
				{
					name
				}));
			default:
				return result;
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x00072A23 File Offset: 0x00070C23
		[SecurityCritical]
		public static bool TryOpenExisting(string name, out EventWaitHandle result)
		{
			return EventWaitHandle.OpenExistingWorker(name, EventWaitHandleRights.Modify | EventWaitHandleRights.Synchronize, out result) == WaitHandle.OpenExistingResult.Success;
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x00072A34 File Offset: 0x00070C34
		[SecurityCritical]
		public static bool TryOpenExisting(string name, EventWaitHandleRights rights, out EventWaitHandle result)
		{
			return EventWaitHandle.OpenExistingWorker(name, rights, out result) == WaitHandle.OpenExistingResult.Success;
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x00072A44 File Offset: 0x00070C44
		[SecurityCritical]
		private static WaitHandle.OpenExistingResult OpenExistingWorker(string name, EventWaitHandleRights rights, out EventWaitHandle result)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", Environment.GetResourceString("Parameter '{0}' cannot be null."));
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Empty name is not legal."), "name");
			}
			if (name != null && 260 < name.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("The name can be no more than 260 characters in length.", new object[]
				{
					name
				}));
			}
			result = null;
			int num;
			SafeWaitHandle safeWaitHandle = new SafeWaitHandle(NativeEventCalls.OpenEvent_internal(name, rights, out num), true);
			if (safeWaitHandle.IsInvalid)
			{
				if (2 == num || 123 == num)
				{
					return WaitHandle.OpenExistingResult.NameNotFound;
				}
				if (3 == num)
				{
					return WaitHandle.OpenExistingResult.PathNotFound;
				}
				if (name != null && name.Length != 0 && 6 == num)
				{
					return WaitHandle.OpenExistingResult.NameInvalid;
				}
				__Error.WinIOError(num, "");
			}
			result = new EventWaitHandle(safeWaitHandle);
			return WaitHandle.OpenExistingResult.Success;
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00072B01 File Offset: 0x00070D01
		[SecuritySafeCritical]
		public bool Reset()
		{
			bool flag = NativeEventCalls.ResetEvent(this.safeWaitHandle);
			if (!flag)
			{
				throw new IOException();
			}
			return flag;
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x00072B19 File Offset: 0x00070D19
		[SecuritySafeCritical]
		public bool Set()
		{
			bool flag = NativeEventCalls.SetEvent(this.safeWaitHandle);
			if (!flag)
			{
				throw new IOException();
			}
			return flag;
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00072B31 File Offset: 0x00070D31
		[SecuritySafeCritical]
		public EventWaitHandleSecurity GetAccessControl()
		{
			return new EventWaitHandleSecurity(this.safeWaitHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x00072B42 File Offset: 0x00070D42
		[SecuritySafeCritical]
		public void SetAccessControl(EventWaitHandleSecurity eventSecurity)
		{
			if (eventSecurity == null)
			{
				throw new ArgumentNullException("eventSecurity");
			}
			eventSecurity.Persist(this.safeWaitHandle);
		}
	}
}
