using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200009C RID: 156
	public class ColorBlockFormatterLocator : IFormatterLocator
	{
		// Token: 0x06000494 RID: 1172 RVA: 0x0002056C File Offset: 0x0001E76C
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, bool allowWeakFallbackFormatters, out IFormatter formatter)
		{
			if (step == FormatterLocationStep.BeforeRegisteredFormatters && type.FullName == "UnityEngine.UI.ColorBlock")
			{
				try
				{
					formatter = (IFormatter)Activator.CreateInstance(typeof(ColorBlockFormatter<>).MakeGenericType(new Type[]
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
					formatter = new WeakColorBlockFormatter(type);
				}
				return true;
			}
			formatter = null;
			return false;
		}
	}
}
