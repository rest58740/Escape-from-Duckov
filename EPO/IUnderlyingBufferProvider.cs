using System;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x0200000D RID: 13
	public interface IUnderlyingBufferProvider
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000032 RID: 50
		CommandBuffer UnderlyingBuffer { get; }
	}
}
