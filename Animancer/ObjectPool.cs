using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200000B RID: 11
	public static class ObjectPool
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x000032FB File Offset: 0x000014FB
		public static T Acquire<T>() where T : class, new()
		{
			return ObjectPool<T>.Acquire();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003302 File Offset: 0x00001502
		public static void Acquire<T>(out T item) where T : class, new()
		{
			item = ObjectPool<T>.Acquire();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000330F File Offset: 0x0000150F
		public static void Release<T>(T item) where T : class, new()
		{
			ObjectPool<T>.Release(item);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003317 File Offset: 0x00001517
		public static void Release<T>(ref T item) where T : class, new()
		{
			ObjectPool<T>.Release(item);
			item = default(T);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000332B File Offset: 0x0000152B
		public static List<T> AcquireList<T>()
		{
			return ObjectPool<List<T>>.Acquire();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003332 File Offset: 0x00001532
		public static void Acquire<T>(out List<T> list)
		{
			list = ObjectPool.AcquireList<T>();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000333B File Offset: 0x0000153B
		public static void Release<T>(List<T> list)
		{
			list.Clear();
			ObjectPool<List<T>>.Release(list);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003349 File Offset: 0x00001549
		public static void Release<T>(ref List<T> list)
		{
			ObjectPool.Release<T>(list);
			list = null;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003355 File Offset: 0x00001555
		public static HashSet<T> AcquireSet<T>()
		{
			return ObjectPool<HashSet<T>>.Acquire();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000335C File Offset: 0x0000155C
		public static void Acquire<T>(out HashSet<T> set)
		{
			set = ObjectPool.AcquireSet<T>();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003365 File Offset: 0x00001565
		public static void Release<T>(HashSet<T> set)
		{
			set.Clear();
			ObjectPool<HashSet<T>>.Release(set);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003373 File Offset: 0x00001573
		public static void Release<T>(ref HashSet<T> set)
		{
			ObjectPool.Release<T>(set);
			set = null;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000337F File Offset: 0x0000157F
		public static StringBuilder AcquireStringBuilder()
		{
			return ObjectPool<StringBuilder>.Acquire();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00003386 File Offset: 0x00001586
		public static void Release(StringBuilder builder)
		{
			builder.Length = 0;
			ObjectPool<StringBuilder>.Release(builder);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00003395 File Offset: 0x00001595
		public static string ReleaseToString(this StringBuilder builder)
		{
			string result = builder.ToString();
			ObjectPool.Release(builder);
			return result;
		}

		// Token: 0x04000010 RID: 16
		public const string NotClearError = " They must be cleared before being released to the pool and not modified after that.";

		// Token: 0x0200007C RID: 124
		public static class Disposable
		{
			// Token: 0x060005B7 RID: 1463 RVA: 0x0000F31E File Offset: 0x0000D51E
			public static ObjectPool<T>.Disposable Acquire<T>(out T item) where T : class, new()
			{
				return new ObjectPool<T>.Disposable(ref item, null);
			}

			// Token: 0x060005B8 RID: 1464 RVA: 0x0000F327 File Offset: 0x0000D527
			public static ObjectPool<List<T>>.Disposable AcquireList<T>(out List<T> list)
			{
				return new ObjectPool<List<T>>.Disposable(ref list, delegate(List<T> l)
				{
					l.Clear();
				});
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x0000F34E File Offset: 0x0000D54E
			public static ObjectPool<HashSet<T>>.Disposable AcquireSet<T>(out HashSet<T> set)
			{
				return new ObjectPool<HashSet<T>>.Disposable(ref set, delegate(HashSet<T> s)
				{
					s.Clear();
				});
			}

			// Token: 0x060005BA RID: 1466 RVA: 0x0000F375 File Offset: 0x0000D575
			public static ObjectPool<GUIContent>.Disposable AcquireContent(out GUIContent content, string text = null, string tooltip = null, bool narrowText = true)
			{
				ObjectPool<GUIContent>.Disposable result = new ObjectPool<GUIContent>.Disposable(ref content, delegate(GUIContent c)
				{
					c.text = null;
					c.tooltip = null;
					c.image = null;
				});
				content.text = text;
				content.tooltip = tooltip;
				content.image = null;
				return result;
			}
		}
	}
}
