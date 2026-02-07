using System;

namespace System.Reflection
{
	// Token: 0x020008A8 RID: 2216
	public class ManifestResourceInfo
	{
		// Token: 0x06004908 RID: 18696 RVA: 0x000EE928 File Offset: 0x000ECB28
		public ManifestResourceInfo(Assembly containingAssembly, string containingFileName, ResourceLocation resourceLocation)
		{
			this.ReferencedAssembly = containingAssembly;
			this.FileName = containingFileName;
			this.ResourceLocation = resourceLocation;
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004909 RID: 18697 RVA: 0x000EE945 File Offset: 0x000ECB45
		public virtual Assembly ReferencedAssembly { get; }

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x0600490A RID: 18698 RVA: 0x000EE94D File Offset: 0x000ECB4D
		public virtual string FileName { get; }

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x0600490B RID: 18699 RVA: 0x000EE955 File Offset: 0x000ECB55
		public virtual ResourceLocation ResourceLocation { get; }
	}
}
