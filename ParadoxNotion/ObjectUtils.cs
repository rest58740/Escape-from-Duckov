using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x0200007B RID: 123
	public static class ObjectUtils
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x0000D1A9 File Offset: 0x0000B3A9
		public static bool AnyEquals(object a, object b)
		{
			if ((a is Object || a == null) && (b is Object || b == null))
			{
				return a as Object == b as Object;
			}
			return a == b || object.Equals(a, b) || a == b;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000D1E8 File Offset: 0x0000B3E8
		public static List<T> Shuffle<T>(this List<T> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int num = (int)Mathf.Floor(Random.value * (float)(i + 1));
				T t = list[i];
				list[i] = list[num];
				list[num] = t;
			}
			return list;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000D238 File Offset: 0x0000B438
		public static bool Is<T>(this object o, out T result)
		{
			if (o is T)
			{
				result = (T)((object)o);
				return true;
			}
			result = default(T);
			return false;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000D258 File Offset: 0x0000B458
		public static T GetAddComponent<T>(this GameObject gameObject) where T : Component
		{
			if (gameObject == null)
			{
				return default(T);
			}
			T t = gameObject.GetComponent<T>();
			if (t == null)
			{
				t = gameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000D298 File Offset: 0x0000B498
		public static Component TransformToType(this Component current, Type type)
		{
			if (current != null && type != null && !type.RTIsAssignableFrom(current.GetType()) && (type.RTIsSubclassOf(typeof(Component)) || type.RTIsInterface()))
			{
				current = current.GetComponent(type);
			}
			return current;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000D2EC File Offset: 0x0000B4EC
		public static IEnumerable<GameObject> FindGameObjectsWithinLayerMask(LayerMask mask, GameObject exclude = null)
		{
			return from x in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
			where x != exclude && x.IsInLayerMask(mask)
			select x;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000D324 File Offset: 0x0000B524
		public static bool IsInLayerMask(this GameObject gameObject, LayerMask mask)
		{
			return mask == (mask | 1 << gameObject.layer);
		}
	}
}
