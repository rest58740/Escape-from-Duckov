using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000039 RID: 57
	public interface IFormatter<T> : IFormatter
	{
		// Token: 0x06000292 RID: 658
		void Serialize(T value, IDataWriter writer);

		// Token: 0x06000293 RID: 659
		T Deserialize(IDataReader reader);
	}
}
