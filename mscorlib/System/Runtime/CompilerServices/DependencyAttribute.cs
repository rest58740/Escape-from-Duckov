using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200082B RID: 2091
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[Serializable]
	public sealed class DependencyAttribute : Attribute
	{
		// Token: 0x060046AA RID: 18090 RVA: 0x000E70ED File Offset: 0x000E52ED
		public DependencyAttribute(string dependentAssemblyArgument, LoadHint loadHintArgument)
		{
			this.dependentAssembly = dependentAssemblyArgument;
			this.loadHint = loadHintArgument;
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x060046AB RID: 18091 RVA: 0x000E7103 File Offset: 0x000E5303
		public string DependentAssembly
		{
			get
			{
				return this.dependentAssembly;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x060046AC RID: 18092 RVA: 0x000E710B File Offset: 0x000E530B
		public LoadHint LoadHint
		{
			get
			{
				return this.loadHint;
			}
		}

		// Token: 0x04002D77 RID: 11639
		private string dependentAssembly;

		// Token: 0x04002D78 RID: 11640
		private LoadHint loadHint;
	}
}
