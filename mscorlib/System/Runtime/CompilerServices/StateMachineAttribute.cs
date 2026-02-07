using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000805 RID: 2053
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[Serializable]
	public class StateMachineAttribute : Attribute
	{
		// Token: 0x06004620 RID: 17952 RVA: 0x000E592B File Offset: 0x000E3B2B
		public StateMachineAttribute(Type stateMachineType)
		{
			this.StateMachineType = stateMachineType;
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06004621 RID: 17953 RVA: 0x000E593A File Offset: 0x000E3B3A
		public Type StateMachineType { get; }
	}
}
