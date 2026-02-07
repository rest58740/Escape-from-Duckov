using System;

namespace System.Diagnostics
{
	// Token: 0x020009AE RID: 2478
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	[Serializable]
	public sealed class ConditionalAttribute : Attribute
	{
		// Token: 0x0600598A RID: 22922 RVA: 0x00132EAB File Offset: 0x001310AB
		public ConditionalAttribute(string conditionString)
		{
			this.ConditionString = conditionString;
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x0600598B RID: 22923 RVA: 0x00132EBA File Offset: 0x001310BA
		public string ConditionString { get; }
	}
}
