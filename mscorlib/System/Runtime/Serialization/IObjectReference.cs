using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000649 RID: 1609
	public interface IObjectReference
	{
		// Token: 0x06003C4D RID: 15437
		object GetRealObject(StreamingContext context);
	}
}
