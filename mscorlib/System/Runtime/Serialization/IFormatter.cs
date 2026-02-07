using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200065E RID: 1630
	[ComVisible(true)]
	public interface IFormatter
	{
		// Token: 0x06003CDA RID: 15578
		object Deserialize(Stream serializationStream);

		// Token: 0x06003CDB RID: 15579
		void Serialize(Stream serializationStream, object graph);

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06003CDC RID: 15580
		// (set) Token: 0x06003CDD RID: 15581
		ISurrogateSelector SurrogateSelector { get; set; }

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06003CDE RID: 15582
		// (set) Token: 0x06003CDF RID: 15583
		SerializationBinder Binder { get; set; }

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06003CE0 RID: 15584
		// (set) Token: 0x06003CE1 RID: 15585
		StreamingContext Context { get; set; }
	}
}
