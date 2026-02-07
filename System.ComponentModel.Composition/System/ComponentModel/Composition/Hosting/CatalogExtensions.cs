using System;
using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x020000BD RID: 189
	public static class CatalogExtensions
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x0000E35F File Offset: 0x0000C55F
		public static CompositionService CreateCompositionService(this ComposablePartCatalog composablePartCatalog)
		{
			Requires.NotNull<ComposablePartCatalog>(composablePartCatalog, "composablePartCatalog");
			return new CompositionService(composablePartCatalog);
		}
	}
}
