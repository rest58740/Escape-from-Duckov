using System;

namespace System.Reflection
{
	// Token: 0x020008BD RID: 2237
	public abstract class ReflectionContext
	{
		// Token: 0x06004A09 RID: 18953
		public abstract Assembly MapAssembly(Assembly assembly);

		// Token: 0x06004A0A RID: 18954
		public abstract TypeInfo MapType(TypeInfo type);

		// Token: 0x06004A0B RID: 18955 RVA: 0x000EF534 File Offset: 0x000ED734
		public virtual TypeInfo GetTypeForObject(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this.MapType(value.GetType().GetTypeInfo());
		}
	}
}
