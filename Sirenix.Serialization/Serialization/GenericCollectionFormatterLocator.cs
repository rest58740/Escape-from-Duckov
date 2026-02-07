using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200001C RID: 28
	internal class GenericCollectionFormatterLocator : IFormatterLocator
	{
		// Token: 0x06000216 RID: 534 RVA: 0x0000EC94 File Offset: 0x0000CE94
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, bool allowWeakFallbackFormatters, out IFormatter formatter)
		{
			Type type2;
			if (step != FormatterLocationStep.AfterRegisteredFormatters || !GenericCollectionFormatter.CanFormat(type, out type2))
			{
				formatter = null;
				return false;
			}
			try
			{
				formatter = (IFormatter)Activator.CreateInstance(typeof(GenericCollectionFormatter<, >).MakeGenericType(new Type[]
				{
					type,
					type2
				}));
			}
			catch (Exception ex)
			{
				if (!allowWeakFallbackFormatters || (!(ex is ExecutionEngineException) && !(ex.GetBaseException() is ExecutionEngineException)))
				{
					throw;
				}
				formatter = new WeakGenericCollectionFormatter(type, type2);
			}
			return true;
		}
	}
}
