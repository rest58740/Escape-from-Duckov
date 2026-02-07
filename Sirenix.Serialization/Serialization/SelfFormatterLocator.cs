using System;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200001F RID: 31
	internal class SelfFormatterLocator : IFormatterLocator
	{
		// Token: 0x0600021B RID: 539 RVA: 0x0000EDAC File Offset: 0x0000CFAC
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, bool allowWeakFallbackFormatters, out IFormatter formatter)
		{
			formatter = null;
			if (!typeof(ISelfFormatter).IsAssignableFrom(type))
			{
				return false;
			}
			if ((step == FormatterLocationStep.BeforeRegisteredFormatters && type.IsDefined<AlwaysFormatsSelfAttribute>()) || step == FormatterLocationStep.AfterRegisteredFormatters)
			{
				try
				{
					formatter = (IFormatter)Activator.CreateInstance(typeof(SelfFormatterFormatter<>).MakeGenericType(new Type[]
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
					formatter = new WeakSelfFormatterFormatter(type);
				}
				return true;
			}
			return false;
		}
	}
}
