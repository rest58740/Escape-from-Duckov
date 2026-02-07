using System;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000042 RID: 66
	public interface ICompositionService
	{
		// Token: 0x060001E1 RID: 481
		void SatisfyImportsOnce(ComposablePart part);
	}
}
