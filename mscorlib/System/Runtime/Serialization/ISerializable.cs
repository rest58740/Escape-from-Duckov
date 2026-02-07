using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200064A RID: 1610
	public interface ISerializable
	{
		// Token: 0x06003C4E RID: 15438
		void GetObjectData(SerializationInfo info, StreamingContext context);
	}
}
