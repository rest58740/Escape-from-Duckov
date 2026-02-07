using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200001A RID: 26
	internal class DelegateFormatterLocator : IFormatterLocator
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, bool allowWeakFallbackFormatters, out IFormatter formatter)
		{
			if (!typeof(Delegate).IsAssignableFrom(type))
			{
				formatter = null;
				return false;
			}
			try
			{
				formatter = (IFormatter)Activator.CreateInstance(typeof(DelegateFormatter<>).MakeGenericType(new Type[]
				{
					type
				}));
			}
			catch (Exception ex)
			{
				if (!allowWeakFallbackFormatters || (!(ex is ExecutionEngineException) && !(ex.GetBaseException() is ExecutionEngineException)))
				{
					throw;
				}
				formatter = new WeakDelegateFormatter(type);
			}
			return true;
		}
	}
}
