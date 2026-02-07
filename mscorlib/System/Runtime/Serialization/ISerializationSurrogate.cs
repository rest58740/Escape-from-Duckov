using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000652 RID: 1618
	public interface ISerializationSurrogate
	{
		// Token: 0x06003C9A RID: 15514
		void GetObjectData(object obj, SerializationInfo info, StreamingContext context);

		// Token: 0x06003C9B RID: 15515
		object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector);
	}
}
