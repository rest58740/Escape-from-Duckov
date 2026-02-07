using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000809 RID: 2057
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
	public sealed class TypeForwardedFromAttribute : Attribute
	{
		// Token: 0x06004626 RID: 17958 RVA: 0x000E5967 File Offset: 0x000E3B67
		public TypeForwardedFromAttribute(string assemblyFullName)
		{
			if (string.IsNullOrEmpty(assemblyFullName))
			{
				throw new ArgumentNullException("assemblyFullName");
			}
			this.AssemblyFullName = assemblyFullName;
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06004627 RID: 17959 RVA: 0x000E5989 File Offset: 0x000E3B89
		public string AssemblyFullName { get; }
	}
}
