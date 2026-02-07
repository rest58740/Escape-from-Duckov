using System;
using System.Reflection;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000011 RID: 17
	public static class UnityExtensions
	{
		// Token: 0x060000EC RID: 236 RVA: 0x000076D4 File Offset: 0x000058D4
		static UnityExtensions()
		{
			FieldInfo field = typeof(Object).GetField("m_CachedPtr", 52);
			if (field != null)
			{
				UnityExtensions.UnityObjectCachedPtrFieldGetter = EmitUtilities.CreateInstanceFieldGetter<Object, IntPtr>(field);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000770C File Offset: 0x0000590C
		public static bool SafeIsUnityNull(this Object obj)
		{
			if (obj == null)
			{
				return true;
			}
			if (UnityExtensions.UnityObjectCachedPtrFieldGetter == null)
			{
				throw new NotSupportedException("Could not find the field 'm_CachedPtr' in the class UnityEngine.Object; cannot perform a special null check.");
			}
			IntPtr intPtr = UnityExtensions.UnityObjectCachedPtrFieldGetter(ref obj);
			return intPtr == IntPtr.Zero;
		}

		// Token: 0x04000034 RID: 52
		private static readonly ValueGetter<Object, IntPtr> UnityObjectCachedPtrFieldGetter;
	}
}
