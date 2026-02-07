using System;
using System.Reflection;
using UnityEngine;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000C0 RID: 192
	internal static class UnityExtensions
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x000270A0 File Offset: 0x000252A0
		static UnityExtensions()
		{
			FieldInfo field = typeof(Object).GetField("m_CachedPtr", 52);
			if (field != null)
			{
				UnityExtensions.UnityObjectCachedPtrFieldGetter = EmitUtilities.CreateInstanceFieldGetter<Object, IntPtr>(field);
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000270D8 File Offset: 0x000252D8
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

		// Token: 0x04000206 RID: 518
		private static readonly ValueGetter<Object, IntPtr> UnityObjectCachedPtrFieldGetter;
	}
}
