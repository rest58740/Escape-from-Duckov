using System;
using System.ComponentModel;

namespace Sirenix.Serialization
{
	// Token: 0x02000061 RID: 97
	[AttributeUsage(1, AllowMultiple = false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class EmittedAssemblyAttribute : Attribute
	{
		// Token: 0x0600035F RID: 863 RVA: 0x00011B5B File Offset: 0x0000FD5B
		[Obsolete("This attribute cannot be used in code, and is only meant to be applied to dynamically emitted assemblies.", true)]
		public EmittedAssemblyAttribute()
		{
		}
	}
}
