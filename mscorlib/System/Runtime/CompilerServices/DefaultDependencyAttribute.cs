using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200082A RID: 2090
	[AttributeUsage(AttributeTargets.Assembly)]
	[Serializable]
	public sealed class DefaultDependencyAttribute : Attribute
	{
		// Token: 0x060046A8 RID: 18088 RVA: 0x000E70D6 File Offset: 0x000E52D6
		public DefaultDependencyAttribute(LoadHint loadHintArgument)
		{
			this.loadHint = loadHintArgument;
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x060046A9 RID: 18089 RVA: 0x000E70E5 File Offset: 0x000E52E5
		public LoadHint LoadHint
		{
			get
			{
				return this.loadHint;
			}
		}

		// Token: 0x04002D76 RID: 11638
		private LoadHint loadHint;
	}
}
