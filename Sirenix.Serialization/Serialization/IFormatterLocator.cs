using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200001D RID: 29
	public interface IFormatterLocator
	{
		// Token: 0x06000218 RID: 536
		bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, bool allowWeakFallbackFormatters, out IFormatter formatter);
	}
}
