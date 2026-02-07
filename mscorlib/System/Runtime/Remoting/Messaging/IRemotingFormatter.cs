using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000621 RID: 1569
	[ComVisible(true)]
	public interface IRemotingFormatter : IFormatter
	{
		// Token: 0x06003B12 RID: 15122
		object Deserialize(Stream serializationStream, HeaderHandler handler);

		// Token: 0x06003B13 RID: 15123
		void Serialize(Stream serializationStream, object graph, Header[] headers);
	}
}
