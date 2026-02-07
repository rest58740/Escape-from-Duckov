using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200080A RID: 2058
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public sealed class TypeForwardedToAttribute : Attribute
	{
		// Token: 0x06004628 RID: 17960 RVA: 0x000E5991 File Offset: 0x000E3B91
		public TypeForwardedToAttribute(Type destination)
		{
			this.Destination = destination;
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06004629 RID: 17961 RVA: 0x000E59A0 File Offset: 0x000E3BA0
		public Type Destination { get; }
	}
}
