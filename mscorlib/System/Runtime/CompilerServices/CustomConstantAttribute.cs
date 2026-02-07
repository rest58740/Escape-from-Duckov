using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007EB RID: 2027
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public abstract class CustomConstantAttribute : Attribute
	{
		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x060045F1 RID: 17905
		public abstract object Value { get; }
	}
}
