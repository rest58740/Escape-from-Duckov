using System;
using System.Runtime.Serialization;

namespace Sirenix.Serialization
{
	// Token: 0x0200001E RID: 30
	internal class ISerializableFormatterLocator : IFormatterLocator
	{
		// Token: 0x06000219 RID: 537 RVA: 0x0000ED20 File Offset: 0x0000CF20
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, bool allowWeakFallbackFormatters, out IFormatter formatter)
		{
			if (step != FormatterLocationStep.AfterRegisteredFormatters || !typeof(ISerializable).IsAssignableFrom(type))
			{
				formatter = null;
				return false;
			}
			try
			{
				formatter = (IFormatter)Activator.CreateInstance(typeof(SerializableFormatter<>).MakeGenericType(new Type[]
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
				formatter = new WeakSerializableFormatter(type);
			}
			return true;
		}
	}
}
