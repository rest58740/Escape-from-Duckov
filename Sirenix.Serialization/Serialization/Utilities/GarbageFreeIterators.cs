using System;
using System.Collections.Generic;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000B7 RID: 183
	internal static class GarbageFreeIterators
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x00023A26 File Offset: 0x00021C26
		public static GarbageFreeIterators.ListIterator<T> GFIterator<T>(this List<T> list)
		{
			return new GarbageFreeIterators.ListIterator<T>(list);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00023A2E File Offset: 0x00021C2E
		public static GarbageFreeIterators.DictionaryIterator<T1, T2> GFIterator<T1, T2>(this Dictionary<T1, T2> dictionary)
		{
			return new GarbageFreeIterators.DictionaryIterator<T1, T2>(dictionary);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00023A36 File Offset: 0x00021C36
		public static GarbageFreeIterators.DictionaryValueIterator<T1, T2> GFValueIterator<T1, T2>(this Dictionary<T1, T2> dictionary)
		{
			return new GarbageFreeIterators.DictionaryValueIterator<T1, T2>(dictionary);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00023A3E File Offset: 0x00021C3E
		public static GarbageFreeIterators.HashsetIterator<T> GFIterator<T>(this HashSet<T> hashset)
		{
			return new GarbageFreeIterators.HashsetIterator<T>(hashset);
		}

		// Token: 0x02000112 RID: 274
		public struct ListIterator<T> : IDisposable
		{
			// Token: 0x060006F3 RID: 1779 RVA: 0x0002BDCA File Offset: 0x00029FCA
			public ListIterator(List<T> list)
			{
				this.isNull = (list == null);
				if (this.isNull)
				{
					this.list = null;
					this.enumerator = default(List<T>.Enumerator);
					return;
				}
				this.list = list;
				this.enumerator = this.list.GetEnumerator();
			}

			// Token: 0x060006F4 RID: 1780 RVA: 0x0002BE0A File Offset: 0x0002A00A
			public GarbageFreeIterators.ListIterator<T> GetEnumerator()
			{
				return this;
			}

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0002BE12 File Offset: 0x0002A012
			public T Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			// Token: 0x060006F6 RID: 1782 RVA: 0x0002BE1F File Offset: 0x0002A01F
			public bool MoveNext()
			{
				return !this.isNull && this.enumerator.MoveNext();
			}

			// Token: 0x060006F7 RID: 1783 RVA: 0x0002BE36 File Offset: 0x0002A036
			public void Dispose()
			{
				this.enumerator.Dispose();
			}

			// Token: 0x040002E6 RID: 742
			private bool isNull;

			// Token: 0x040002E7 RID: 743
			private List<T> list;

			// Token: 0x040002E8 RID: 744
			private List<T>.Enumerator enumerator;
		}

		// Token: 0x02000113 RID: 275
		public struct HashsetIterator<T> : IDisposable
		{
			// Token: 0x060006F8 RID: 1784 RVA: 0x0002BE43 File Offset: 0x0002A043
			public HashsetIterator(HashSet<T> hashset)
			{
				this.isNull = (hashset == null);
				if (this.isNull)
				{
					this.hashset = null;
					this.enumerator = default(HashSet<T>.Enumerator);
					return;
				}
				this.hashset = hashset;
				this.enumerator = this.hashset.GetEnumerator();
			}

			// Token: 0x060006F9 RID: 1785 RVA: 0x0002BE83 File Offset: 0x0002A083
			public GarbageFreeIterators.HashsetIterator<T> GetEnumerator()
			{
				return this;
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x060006FA RID: 1786 RVA: 0x0002BE8B File Offset: 0x0002A08B
			public T Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			// Token: 0x060006FB RID: 1787 RVA: 0x0002BE98 File Offset: 0x0002A098
			public bool MoveNext()
			{
				return !this.isNull && this.enumerator.MoveNext();
			}

			// Token: 0x060006FC RID: 1788 RVA: 0x0002BEAF File Offset: 0x0002A0AF
			public void Dispose()
			{
				this.enumerator.Dispose();
			}

			// Token: 0x040002E9 RID: 745
			private bool isNull;

			// Token: 0x040002EA RID: 746
			private HashSet<T> hashset;

			// Token: 0x040002EB RID: 747
			private HashSet<T>.Enumerator enumerator;
		}

		// Token: 0x02000114 RID: 276
		public struct DictionaryIterator<T1, T2> : IDisposable
		{
			// Token: 0x060006FD RID: 1789 RVA: 0x0002BEBC File Offset: 0x0002A0BC
			public DictionaryIterator(Dictionary<T1, T2> dictionary)
			{
				this.isNull = (dictionary == null);
				if (this.isNull)
				{
					this.dictionary = null;
					this.enumerator = default(Dictionary<T1, T2>.Enumerator);
					return;
				}
				this.dictionary = dictionary;
				this.enumerator = this.dictionary.GetEnumerator();
			}

			// Token: 0x060006FE RID: 1790 RVA: 0x0002BEFC File Offset: 0x0002A0FC
			public GarbageFreeIterators.DictionaryIterator<T1, T2> GetEnumerator()
			{
				return this;
			}

			// Token: 0x1700009F RID: 159
			// (get) Token: 0x060006FF RID: 1791 RVA: 0x0002BF04 File Offset: 0x0002A104
			public KeyValuePair<T1, T2> Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			// Token: 0x06000700 RID: 1792 RVA: 0x0002BF11 File Offset: 0x0002A111
			public bool MoveNext()
			{
				return !this.isNull && this.enumerator.MoveNext();
			}

			// Token: 0x06000701 RID: 1793 RVA: 0x0002BF28 File Offset: 0x0002A128
			public void Dispose()
			{
				this.enumerator.Dispose();
			}

			// Token: 0x040002EC RID: 748
			private Dictionary<T1, T2> dictionary;

			// Token: 0x040002ED RID: 749
			private Dictionary<T1, T2>.Enumerator enumerator;

			// Token: 0x040002EE RID: 750
			private bool isNull;
		}

		// Token: 0x02000115 RID: 277
		public struct DictionaryValueIterator<T1, T2> : IDisposable
		{
			// Token: 0x06000702 RID: 1794 RVA: 0x0002BF35 File Offset: 0x0002A135
			public DictionaryValueIterator(Dictionary<T1, T2> dictionary)
			{
				this.isNull = (dictionary == null);
				if (this.isNull)
				{
					this.dictionary = null;
					this.enumerator = default(Dictionary<T1, T2>.Enumerator);
					return;
				}
				this.dictionary = dictionary;
				this.enumerator = this.dictionary.GetEnumerator();
			}

			// Token: 0x06000703 RID: 1795 RVA: 0x0002BF75 File Offset: 0x0002A175
			public GarbageFreeIterators.DictionaryValueIterator<T1, T2> GetEnumerator()
			{
				return this;
			}

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x06000704 RID: 1796 RVA: 0x0002BF80 File Offset: 0x0002A180
			public T2 Current
			{
				get
				{
					KeyValuePair<T1, T2> keyValuePair = this.enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x06000705 RID: 1797 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
			public bool MoveNext()
			{
				return !this.isNull && this.enumerator.MoveNext();
			}

			// Token: 0x06000706 RID: 1798 RVA: 0x0002BFB7 File Offset: 0x0002A1B7
			public void Dispose()
			{
				this.enumerator.Dispose();
			}

			// Token: 0x040002EF RID: 751
			private Dictionary<T1, T2> dictionary;

			// Token: 0x040002F0 RID: 752
			private Dictionary<T1, T2>.Enumerator enumerator;

			// Token: 0x040002F1 RID: 753
			private bool isNull;
		}
	}
}
