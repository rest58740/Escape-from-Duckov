using System;

namespace System.Reflection
{
	// Token: 0x020008A6 RID: 2214
	public static class IntrospectionExtensions
	{
		// Token: 0x06004903 RID: 18691 RVA: 0x000EE8C4 File Offset: 0x000ECAC4
		public static TypeInfo GetTypeInfo(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			IReflectableType reflectableType = type as IReflectableType;
			if (reflectableType != null)
			{
				return reflectableType.GetTypeInfo();
			}
			return new TypeDelegator(type);
		}
	}
}
