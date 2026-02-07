using System;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000022 RID: 34
	[AttributeUsage(1, AllowMultiple = false, Inherited = true)]
	public class CatalogReflectionContextAttribute : Attribute
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00004300 File Offset: 0x00002500
		public CatalogReflectionContextAttribute(Type reflectionContextType)
		{
			Requires.NotNull<Type>(reflectionContextType, "reflectionContextType");
			this._reflectionContextType = reflectionContextType;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000431C File Offset: 0x0000251C
		public ReflectionContext CreateReflectionContext()
		{
			Assumes.NotNull<Type>(this._reflectionContextType);
			ReflectionContext result = null;
			try
			{
				result = (ReflectionContext)Activator.CreateInstance(this._reflectionContextType);
			}
			catch (InvalidCastException ex)
			{
				throw new InvalidOperationException(Strings.ReflectionContext_Type_Required, ex);
			}
			catch (MissingMethodException ex2)
			{
				throw new MissingMethodException(Strings.ReflectionContext_Requires_DefaultConstructor, ex2);
			}
			return result;
		}

		// Token: 0x0400006B RID: 107
		private Type _reflectionContextType;
	}
}
