using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using Unity;

namespace System.Runtime.DesignerServices
{
	// Token: 0x02000BCE RID: 3022
	public sealed class WindowsRuntimeDesignerContext
	{
		// Token: 0x06006B5E RID: 27486 RVA: 0x000173AD File Offset: 0x000155AD
		[SecurityCritical]
		public WindowsRuntimeDesignerContext(IEnumerable<string> paths, string name)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06006B5F RID: 27487 RVA: 0x00052959 File Offset: 0x00050B59
		public string Name
		{
			get
			{
				ThrowStub.ThrowNotSupportedException();
				return null;
			}
		}

		// Token: 0x06006B60 RID: 27488 RVA: 0x00052959 File Offset: 0x00050B59
		[SecurityCritical]
		public Assembly GetAssembly(string assemblyName)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06006B61 RID: 27489 RVA: 0x00052959 File Offset: 0x00050B59
		[SecurityCritical]
		public Type GetType(string typeName)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06006B62 RID: 27490 RVA: 0x000173AD File Offset: 0x000155AD
		[SecurityCritical]
		public static void InitializeSharedContext(IEnumerable<string> paths)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06006B63 RID: 27491 RVA: 0x000173AD File Offset: 0x000155AD
		[SecurityCritical]
		public static void SetIterationContext(WindowsRuntimeDesignerContext context)
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}
}
