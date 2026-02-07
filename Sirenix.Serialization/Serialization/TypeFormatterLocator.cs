using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000020 RID: 32
	internal class TypeFormatterLocator : IFormatterLocator
	{
		// Token: 0x0600021D RID: 541 RVA: 0x0000EE48 File Offset: 0x0000D048
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, bool allowWeakFallbackFormatters, out IFormatter formatter)
		{
			if (!typeof(Type).IsAssignableFrom(type))
			{
				formatter = null;
				return false;
			}
			formatter = new TypeFormatter();
			return true;
		}
	}
}
