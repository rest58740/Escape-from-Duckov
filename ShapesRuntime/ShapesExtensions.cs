using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000073 RID: 115
	internal static class ShapesExtensions
	{
		// Token: 0x06000CB4 RID: 3252 RVA: 0x00019D80 File Offset: 0x00017F80
		public static void ForEach<T>(this IEnumerable<T> elems, Action<T> action)
		{
			foreach (T t in elems)
			{
				action.Invoke(t);
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00019DC8 File Offset: 0x00017FC8
		public static Vector3 Rot90CCW(this Vector3 v)
		{
			return new Vector3(-v.y, v.x);
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00019DDC File Offset: 0x00017FDC
		public static int AsInt(this bool b)
		{
			if (!b)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00019DE4 File Offset: 0x00017FE4
		public static Vector4 ToVector4(this Rect r)
		{
			return new Vector4(r.x, r.y, r.width, r.height);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00019E07 File Offset: 0x00018007
		public static float TaxicabMagnitude(this Vector3 v)
		{
			return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00019E2C File Offset: 0x0001802C
		public static float AvgComponentMagnitude(this Vector3 v)
		{
			return (Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z)) / 3f;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00019E57 File Offset: 0x00018057
		internal static Color ColorSpaceAdjusted(this Color c)
		{
			if (QualitySettings.activeColorSpace != ColorSpace.Linear)
			{
				return c;
			}
			return c.linear;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00019E6A File Offset: 0x0001806A
		[MethodImpl(256)]
		public static void SetInt_Shapes(this Material m, int id, int value)
		{
			m.SetInt(id, value);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00019E74 File Offset: 0x00018074
		[MethodImpl(256)]
		public static void SetInt_Shapes(this MaterialPropertyBlock mpb, int id, int value)
		{
			mpb.SetInt(id, value);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00019E7E File Offset: 0x0001807E
		public static void DestroyBranched(this Object obj)
		{
			Object.Destroy(obj);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00019E86 File Offset: 0x00018086
		public static void DestroyEndOfFrameEmulated(this Object obj)
		{
			Object.Destroy(obj);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00019E8E File Offset: 0x0001808E
		public static void TryDestroyInOnDestroy(this Object caller, Object obj)
		{
			if (obj == null)
			{
				return;
			}
			Object.Destroy(obj);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00019EA0 File Offset: 0x000180A0
		public static int Product<T>(this IEnumerable<T> arr, Func<T, int> mulVal)
		{
			int num = 1;
			foreach (T t in arr)
			{
				num *= mulVal.Invoke(t);
			}
			return num;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00019EF0 File Offset: 0x000180F0
		public static float Product<T>(this IEnumerable<T> arr, Func<T, float> mulVal)
		{
			float num = 1f;
			foreach (T t in arr)
			{
				num *= mulVal.Invoke(t);
			}
			return num;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00019F44 File Offset: 0x00018144
		public static IEnumerable<TResult> Zip<T1, T2, T3, TResult>(this IEnumerable<T1> source, IEnumerable<T2> second, IEnumerable<T3> third, Func<T1, T2, T3, TResult> func)
		{
			using (IEnumerator<T1> e = source.GetEnumerator())
			{
				using (IEnumerator<T2> e2 = second.GetEnumerator())
				{
					using (IEnumerator<T3> e3 = third.GetEnumerator())
					{
						while (e.MoveNext() && e2.MoveNext() && e3.MoveNext())
						{
							yield return func.Invoke(e.Current, e2.Current, e3.Current);
						}
					}
					IEnumerator<T3> e3 = null;
				}
				IEnumerator<T2> e2 = null;
			}
			IEnumerator<T1> e = null;
			yield break;
			yield break;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00019F69 File Offset: 0x00018169
		public static int PopCount(this uint i)
		{
			i -= (i >> 1 & 1431655765U);
			i = (i & 858993459U) + (i >> 2 & 858993459U);
			i = (i + (i >> 4) & 252645135U) * 16843009U >> 24;
			return (int)i;
		}
	}
}
