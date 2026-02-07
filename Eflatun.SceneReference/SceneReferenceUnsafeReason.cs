using System;
using JetBrains.Annotations;

namespace Eflatun.SceneReference
{
	// Token: 0x0200000D RID: 13
	[PublicAPI]
	public enum SceneReferenceUnsafeReason
	{
		// Token: 0x04000020 RID: 32
		None,
		// Token: 0x04000021 RID: 33
		Empty,
		// Token: 0x04000022 RID: 34
		NotInMaps,
		// Token: 0x04000023 RID: 35
		NotInBuild
	}
}
