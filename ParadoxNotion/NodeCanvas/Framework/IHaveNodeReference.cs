using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000017 RID: 23
	public interface IHaveNodeReference : IGraphElement
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600014F RID: 335
		INodeReference targetReference { get; }
	}
}
