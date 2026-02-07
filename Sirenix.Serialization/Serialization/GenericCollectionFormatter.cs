using System;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x02000033 RID: 51
	public static class GenericCollectionFormatter
	{
		// Token: 0x0600027C RID: 636 RVA: 0x00012AC4 File Offset: 0x00010CC4
		public static bool CanFormat(Type type, out Type elementType)
		{
			if (type == null)
			{
				throw new ArgumentNullException();
			}
			if (type.IsAbstract || type.IsGenericTypeDefinition || type.IsInterface || type.GetConstructor(Type.EmptyTypes) == null || !type.ImplementsOpenGenericInterface(typeof(ICollection)))
			{
				elementType = null;
				return false;
			}
			elementType = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(ICollection))[0];
			return true;
		}
	}
}
