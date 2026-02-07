using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000014 RID: 20
	public interface IGraphAssignable<T> : IGraphAssignable, IGraphElement where T : Graph
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000149 RID: 329
		// (set) Token: 0x0600014A RID: 330
		T subGraph { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600014B RID: 331
		// (set) Token: 0x0600014C RID: 332
		T currentInstance { get; set; }
	}
}
