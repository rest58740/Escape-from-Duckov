using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000016 RID: 22
	public interface ITaskAssignable<T> : ITaskAssignable, IGraphElement where T : Task
	{
	}
}
