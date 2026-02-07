using System;
using JetBrains.Annotations;

namespace Eflatun.SceneReference
{
	// Token: 0x0200000B RID: 11
	[PublicAPI]
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class SceneReferenceOptionsAttribute : Attribute
	{
		// Token: 0x04000018 RID: 24
		public ColoringBehaviour SceneInBuildColoring;

		// Token: 0x04000019 RID: 25
		public ToolboxBehaviour Toolbox;

		// Token: 0x0400001A RID: 26
		public ColoringBehaviour AddressableColoring;
	}
}
