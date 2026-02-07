using System;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x02000096 RID: 150
	public interface ICompositionElement
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003F5 RID: 1013
		string DisplayName { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003F6 RID: 1014
		ICompositionElement Origin { get; }
	}
}
