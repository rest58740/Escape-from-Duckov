using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000019 RID: 25
	internal class ArrayFormatterLocator : IFormatterLocator
	{
		// Token: 0x06000207 RID: 519 RVA: 0x0000DC18 File Offset: 0x0000BE18
		public bool TryGetFormatter(Type type, FormatterLocationStep step, ISerializationPolicy policy, bool allowWeakFallbackFormatters, out IFormatter formatter)
		{
			if (!type.IsArray)
			{
				formatter = null;
				return false;
			}
			Type elementType = type.GetElementType();
			if (type.GetArrayRank() == 1)
			{
				if (FormatterUtilities.IsPrimitiveArrayType(elementType))
				{
					try
					{
						formatter = (IFormatter)Activator.CreateInstance(typeof(PrimitiveArrayFormatter<>).MakeGenericType(new Type[]
						{
							elementType
						}));
						return true;
					}
					catch (Exception ex)
					{
						if (allowWeakFallbackFormatters && (ex is ExecutionEngineException || ex.GetBaseException() is ExecutionEngineException))
						{
							formatter = new WeakPrimitiveArrayFormatter(type, elementType);
							return true;
						}
						throw;
					}
				}
				try
				{
					formatter = (IFormatter)Activator.CreateInstance(typeof(ArrayFormatter<>).MakeGenericType(new Type[]
					{
						elementType
					}));
					return true;
				}
				catch (Exception ex2)
				{
					if (allowWeakFallbackFormatters && (ex2 is ExecutionEngineException || ex2.GetBaseException() is ExecutionEngineException))
					{
						formatter = new WeakArrayFormatter(type, elementType);
						return true;
					}
					throw;
				}
			}
			try
			{
				formatter = (IFormatter)Activator.CreateInstance(typeof(MultiDimensionalArrayFormatter<, >).MakeGenericType(new Type[]
				{
					type,
					type.GetElementType()
				}));
			}
			catch (Exception ex3)
			{
				if (!allowWeakFallbackFormatters || (!(ex3 is ExecutionEngineException) && !(ex3.GetBaseException() is ExecutionEngineException)))
				{
					throw;
				}
				formatter = new WeakMultiDimensionalArrayFormatter(type, elementType);
			}
			return true;
		}
	}
}
