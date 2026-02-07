using System;

namespace KINEMATION.KAnimationCore.Runtime.Attributes
{
	// Token: 0x02000027 RID: 39
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class ElementChainSelectorAttribute : RigAssetSelectorAttribute
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003CEE File Offset: 0x00001EEE
		public ElementChainSelectorAttribute(string rigName = "") : base("")
		{
			this.assetName = rigName;
		}
	}
}
