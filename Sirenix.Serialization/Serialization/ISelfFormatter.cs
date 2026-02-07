using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200006A RID: 106
	public interface ISelfFormatter
	{
		// Token: 0x06000379 RID: 889
		void Serialize(IDataWriter writer);

		// Token: 0x0600037A RID: 890
		void Deserialize(IDataReader reader);
	}
}
