using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007FF RID: 2047
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[Serializable]
	public sealed class IteratorStateMachineAttribute : StateMachineAttribute
	{
		// Token: 0x06004610 RID: 17936 RVA: 0x000E51C6 File Offset: 0x000E33C6
		public IteratorStateMachineAttribute(Type stateMachineType) : base(stateMachineType)
		{
		}
	}
}
