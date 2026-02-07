using System;

namespace Sirenix.Serialization
{
	// Token: 0x020000AF RID: 175
	public interface IOverridesSerializationFormat
	{
		// Token: 0x060004DD RID: 1245
		DataFormat GetFormatToSerializeAs(bool isPlayer);
	}
}
