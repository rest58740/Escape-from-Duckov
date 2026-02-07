using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000600 RID: 1536
	[ComVisible(true)]
	[SecurityCritical]
	[Serializable]
	public sealed class CallContext
	{
		// Token: 0x06003A32 RID: 14898 RVA: 0x0000259F File Offset: 0x0000079F
		private CallContext()
		{
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x0000AF5E File Offset: 0x0000915E
		internal static object SetCurrentCallContext(LogicalCallContext ctx)
		{
			return null;
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x000CC50C File Offset: 0x000CA70C
		internal static LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
			LogicalCallContext logicalCallContext = mutableExecutionContext.LogicalCallContext;
			mutableExecutionContext.LogicalCallContext = callCtx;
			return logicalCallContext;
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x000CC531 File Offset: 0x000CA731
		[SecurityCritical]
		public static void FreeNamedDataSlot(string name)
		{
			ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
			mutableExecutionContext.LogicalCallContext.FreeNamedDataSlot(name);
			mutableExecutionContext.IllogicalCallContext.FreeNamedDataSlot(name);
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x000CC554 File Offset: 0x000CA754
		[SecurityCritical]
		public static object LogicalGetData(string name)
		{
			return Thread.CurrentThread.GetExecutionContextReader().LogicalCallContext.GetData(name);
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x000CC57C File Offset: 0x000CA77C
		private static object IllogicalGetData(string name)
		{
			return Thread.CurrentThread.GetExecutionContextReader().IllogicalCallContext.GetData(name);
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06003A38 RID: 14904 RVA: 0x000CC5A4 File Offset: 0x000CA7A4
		// (set) Token: 0x06003A39 RID: 14905 RVA: 0x000CC5CB File Offset: 0x000CA7CB
		internal static IPrincipal Principal
		{
			[SecurityCritical]
			get
			{
				return Thread.CurrentThread.GetExecutionContextReader().LogicalCallContext.Principal;
			}
			[SecurityCritical]
			set
			{
				Thread.CurrentThread.GetMutableExecutionContext().LogicalCallContext.Principal = value;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06003A3A RID: 14906 RVA: 0x000CC5E4 File Offset: 0x000CA7E4
		// (set) Token: 0x06003A3B RID: 14907 RVA: 0x000CC620 File Offset: 0x000CA820
		public static object HostContext
		{
			[SecurityCritical]
			get
			{
				ExecutionContext.Reader executionContextReader = Thread.CurrentThread.GetExecutionContextReader();
				object hostContext = executionContextReader.IllogicalCallContext.HostContext;
				if (hostContext == null)
				{
					hostContext = executionContextReader.LogicalCallContext.HostContext;
				}
				return hostContext;
			}
			[SecurityCritical]
			set
			{
				ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
				if (value is ILogicalThreadAffinative)
				{
					mutableExecutionContext.IllogicalCallContext.HostContext = null;
					mutableExecutionContext.LogicalCallContext.HostContext = value;
					return;
				}
				mutableExecutionContext.IllogicalCallContext.HostContext = value;
				mutableExecutionContext.LogicalCallContext.HostContext = null;
			}
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x000CC674 File Offset: 0x000CA874
		[SecurityCritical]
		public static object GetData(string name)
		{
			object obj = CallContext.LogicalGetData(name);
			if (obj == null)
			{
				return CallContext.IllogicalGetData(name);
			}
			return obj;
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x000CC693 File Offset: 0x000CA893
		[SecurityCritical]
		public static void SetData(string name, object data)
		{
			if (data is ILogicalThreadAffinative)
			{
				CallContext.LogicalSetData(name, data);
				return;
			}
			ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
			mutableExecutionContext.LogicalCallContext.FreeNamedDataSlot(name);
			mutableExecutionContext.IllogicalCallContext.SetData(name, data);
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x000CC6C7 File Offset: 0x000CA8C7
		[SecurityCritical]
		public static void LogicalSetData(string name, object data)
		{
			ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
			mutableExecutionContext.IllogicalCallContext.FreeNamedDataSlot(name);
			mutableExecutionContext.LogicalCallContext.SetData(name, data);
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x000CC6EB File Offset: 0x000CA8EB
		[SecurityCritical]
		public static Header[] GetHeaders()
		{
			return Thread.CurrentThread.GetMutableExecutionContext().LogicalCallContext.InternalGetHeaders();
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x000CC701 File Offset: 0x000CA901
		[SecurityCritical]
		public static void SetHeaders(Header[] headers)
		{
			Thread.CurrentThread.GetMutableExecutionContext().LogicalCallContext.InternalSetHeaders(headers);
		}
	}
}
