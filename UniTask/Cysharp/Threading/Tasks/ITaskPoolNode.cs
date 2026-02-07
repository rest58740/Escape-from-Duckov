using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000032 RID: 50
	public interface ITaskPoolNode<T>
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000CA RID: 202
		ref T NextNode { get; }
	}
}
