using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007DC RID: 2012
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[Serializable]
	public sealed class AsyncStateMachineAttribute : StateMachineAttribute
	{
		// Token: 0x060045C3 RID: 17859 RVA: 0x000E51C6 File Offset: 0x000E33C6
		public AsyncStateMachineAttribute(Type stateMachineType) : base(stateMachineType)
		{
		}
	}
}
