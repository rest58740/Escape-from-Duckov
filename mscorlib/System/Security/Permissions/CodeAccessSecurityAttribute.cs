using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000432 RID: 1074
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public abstract class CodeAccessSecurityAttribute : SecurityAttribute
	{
		// Token: 0x06002B99 RID: 11161 RVA: 0x0009D5BF File Offset: 0x0009B7BF
		protected CodeAccessSecurityAttribute(SecurityAction action) : base(action)
		{
		}
	}
}
