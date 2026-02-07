using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000038 RID: 56
	public interface IFormatter
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600028F RID: 655
		Type SerializedType { get; }

		// Token: 0x06000290 RID: 656
		void Serialize(object value, IDataWriter writer);

		// Token: 0x06000291 RID: 657
		object Deserialize(IDataReader reader);
	}
}
