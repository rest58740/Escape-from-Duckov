using System;

namespace NodeCanvas.Framework
{
	// Token: 0x0200001C RID: 28
	public interface INodeReference
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001A6 RID: 422
		Type type { get; }

		// Token: 0x060001A7 RID: 423
		Node Get(Graph graph);

		// Token: 0x060001A8 RID: 424
		void Set(Node target);
	}
}
