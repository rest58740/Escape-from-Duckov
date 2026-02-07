using System;
using System.Collections.Generic;

namespace Sirenix.Utilities
{
	// Token: 0x02000005 RID: 5
	public static class GarbageFreeIterators
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000025D2 File Offset: 0x000007D2
		public static GarbageFreeIterators.ListIterator<T> GFIterator<T>(this List<T> list)
		{
			return new GarbageFreeIterators.ListIterator<T>(list);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000025DA File Offset: 0x000007DA
		public static GarbageFreeIterators.DictionaryIterator<T1, T2> GFIterator<T1, T2>(this Dictionary<T1, T2> dictionary)
		{
			return new GarbageFreeIterators.DictionaryIterator<T1, T2>(dictionary);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025E2 File Offset: 0x000007E2
		public static GarbageFreeIterators.DictionaryValueIterator<T1, T2> GFValueIterator<T1, T2>(this Dictionary<T1, T2> dictionary)
		{
			return new GarbageFreeIterators.DictionaryValueIterator<T1, T2>(dictionary);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025EA File Offset: 0x000007EA
		public static GarbageFreeIterators.HashsetIterator<T> GFIterator<T>(this HashSet<T> hashset)
		{
			return new GarbageFreeIterators.HashsetIterator<T>(hashset);
		}

		// Token: 0x0200003C RID: 60
		public struct ListIterator<T> : IDisposable
		{
			// Token: 0x0600024B RID: 587 RVA: 0x0000DDAC File Offset: 0x0000BFAC
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

			// Token: 0x0600024C RID: 588 RVA: 0x0000DDEC File Offset: 0x0000BFEC
			public GarbageFreeIterators.ListIterator<T> GetEnumerator()
			{
				return this;
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x0600024D RID: 589 RVA: 0x0000DDF4 File Offset: 0x0000BFF4
			public T Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			// Token: 0x0600024E RID: 590 RVA: 0x0000DE01 File Offset: 0x0000C001
			public bool MoveNext()
			{
				return !this.isNull && this.enumerator.MoveNext();
			}

			// Token: 0x0600024F RID: 591 RVA: 0x0000DE18 File Offset: 0x0000C018
			public void Dispose()
			{
				this.enumerator.Dispose();
			}

			// Token: 0x0400007F RID: 127
			private bool isNull;

			// Token: 0x04000080 RID: 128
			private List<T> list;

			// Token: 0x04000081 RID: 129
			private List<T>.Enumerator enumerator;
		}

		// Token: 0x0200003D RID: 61
		public struct HashsetIterator<T> : IDisposable
		{
			// Token: 0x06000250 RID: 592 RVA: 0x0000DE25 File Offset: 0x0000C025
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

			// Token: 0x06000251 RID: 593 RVA: 0x0000DE65 File Offset: 0x0000C065
			public GarbageFreeIterators.HashsetIterator<T> GetEnumerator()
			{
				return this;
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x06000252 RID: 594 RVA: 0x0000DE6D File Offset: 0x0000C06D
			public T Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			// Token: 0x06000253 RID: 595 RVA: 0x0000DE7A File Offset: 0x0000C07A
			public bool MoveNext()
			{
				return !this.isNull && this.enumerator.MoveNext();
			}

			// Token: 0x06000254 RID: 596 RVA: 0x0000DE91 File Offset: 0x0000C091
			public void Dispose()
			{
				this.enumerator.Dispose();
			}

			// Token: 0x04000082 RID: 130
			private bool isNull;

			// Token: 0x04000083 RID: 131
			private HashSet<T> hashset;

			// Token: 0x04000084 RID: 132
			private HashSet<T>.Enumerator enumerator;
		}

		// Token: 0x0200003E RID: 62
		public struct DictionaryIterator<T1, T2> : IDisposable
		{
			// Token: 0x06000255 RID: 597 RVA: 0x0000DE9E File Offset: 0x0000C09E
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

			// Token: 0x06000256 RID: 598 RVA: 0x0000DEDE File Offset: 0x0000C0DE
			public GarbageFreeIterators.DictionaryIterator<T1, T2> GetEnumerator()
			{
				return this;
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x06000257 RID: 599 RVA: 0x0000DEE6 File Offset: 0x0000C0E6
			public KeyValuePair<T1, T2> Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			// Token: 0x06000258 RID: 600 RVA: 0x0000DEF3 File Offset: 0x0000C0F3
			public bool MoveNext()
			{
				return !this.isNull && this.enumerator.MoveNext();
			}

			// Token: 0x06000259 RID: 601 RVA: 0x0000DF0A File Offset: 0x0000C10A
			public void Dispose()
			{
				this.enumerator.Dispose();
			}

			// Token: 0x04000085 RID: 133
			private Dictionary<T1, T2> dictionary;

			// Token: 0x04000086 RID: 134
			private Dictionary<T1, T2>.Enumerator enumerator;

			// Token: 0x04000087 RID: 135
			private bool isNull;
		}

		// Token: 0x0200003F RID: 63
		public struct DictionaryValueIterator<T1, T2> : IDisposable
		{
			// Token: 0x0600025A RID: 602 RVA: 0x0000DF17 File Offset: 0x0000C117
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

			// Token: 0x0600025B RID: 603 RVA: 0x0000DF57 File Offset: 0x0000C157
			public GarbageFreeIterators.DictionaryValueIterator<T1, T2> GetEnumerator()
			{
				return this;
			}

			// Token: 0x17000049 RID: 73
			// (get) Token: 0x0600025C RID: 604 RVA: 0x0000DF60 File Offset: 0x0000C160
			public T2 Current
			{
				get
				{
					KeyValuePair<T1, T2> keyValuePair = this.enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x0600025D RID: 605 RVA: 0x0000DF80 File Offset: 0x0000C180
			public bool MoveNext()
			{
				return !this.isNull && this.enumerator.MoveNext();
			}

			// Token: 0x0600025E RID: 606 RVA: 0x0000DF97 File Offset: 0x0000C197
			public void Dispose()
			{
				this.enumerator.Dispose();
			}

			// Token: 0x04000088 RID: 136
			private Dictionary<T1, T2> dictionary;

			// Token: 0x04000089 RID: 137
			private Dictionary<T1, T2>.Enumerator enumerator;

			// Token: 0x0400008A RID: 138
			private bool isNull;
		}
	}
}
