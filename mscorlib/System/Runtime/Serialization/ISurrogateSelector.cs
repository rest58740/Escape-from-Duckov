using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000653 RID: 1619
	public interface ISurrogateSelector
	{
		// Token: 0x06003C9C RID: 15516
		void ChainSelector(ISurrogateSelector selector);

		// Token: 0x06003C9D RID: 15517
		ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector);

		// Token: 0x06003C9E RID: 15518
		ISurrogateSelector GetNextSelector();
	}
}
