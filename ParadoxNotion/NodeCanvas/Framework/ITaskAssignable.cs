using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000015 RID: 21
	public interface ITaskAssignable : IGraphElement
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600014D RID: 333
		// (set) Token: 0x0600014E RID: 334
		Task task { get; set; }
	}
}
