using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Unity;

namespace System.Collections.ObjectModel
{
	// Token: 0x02000A83 RID: 2691
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(DictionaryDebugView<, >))]
	[Serializable]
	public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<!0, !1>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x06006054 RID: 24660 RVA: 0x00142E2E File Offset: 0x0014102E
		public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.m_dictionary = dictionary;
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06006055 RID: 24661 RVA: 0x00142E4B File Offset: 0x0014104B
		protected IDictionary<TKey, TValue> Dictionary
		{
			get
			{
				return this.m_dictionary;
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06006056 RID: 24662 RVA: 0x00142E53 File Offset: 0x00141053
		public ReadOnlyDictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new ReadOnlyDictionary<TKey, TValue>.KeyCollection(this.m_dictionary.Keys);
				}
				return this._keys;
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06006057 RID: 24663 RVA: 0x00142E79 File Offset: 0x00141079
		public ReadOnlyDictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new ReadOnlyDictionary<TKey, TValue>.ValueCollection(this.m_dictionary.Values);
				}
				return this._values;
			}
		}

		// Token: 0x06006058 RID: 24664 RVA: 0x00142E9F File Offset: 0x0014109F
		public bool ContainsKey(TKey key)
		{
			return this.m_dictionary.ContainsKey(key);
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06006059 RID: 24665 RVA: 0x00142EAD File Offset: 0x001410AD
		ICollection<TKey> IDictionary<!0, !1>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x00142EB5 File Offset: 0x001410B5
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.m_dictionary.TryGetValue(key, out value);
		}

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x0600605B RID: 24667 RVA: 0x00142EC4 File Offset: 0x001410C4
		ICollection<TValue> IDictionary<!0, !1>.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x17001100 RID: 4352
		public TValue this[TKey key]
		{
			get
			{
				return this.m_dictionary[key];
			}
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0013B59D File Offset: 0x0013979D
		void IDictionary<!0, !1>.Add(TKey key, TValue value)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x0600605E RID: 24670 RVA: 0x0013B59D File Offset: 0x0013979D
		bool IDictionary<!0, !1>.Remove(TKey key)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x17001101 RID: 4353
		TValue IDictionary<!0, !1>.this[TKey key]
		{
			get
			{
				return this.m_dictionary[key];
			}
			set
			{
				throw new NotSupportedException("Collection is read-only.");
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06006061 RID: 24673 RVA: 0x00142EDA File Offset: 0x001410DA
		public int Count
		{
			get
			{
				return this.m_dictionary.Count;
			}
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x00142EE7 File Offset: 0x001410E7
		bool ICollection<KeyValuePair<!0, !1>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.m_dictionary.Contains(item);
		}

		// Token: 0x06006063 RID: 24675 RVA: 0x00142EF5 File Offset: 0x001410F5
		void ICollection<KeyValuePair<!0, !1>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.m_dictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06006064 RID: 24676 RVA: 0x000040F7 File Offset: 0x000022F7
		bool ICollection<KeyValuePair<!0, !1>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006065 RID: 24677 RVA: 0x0013B59D File Offset: 0x0013979D
		void ICollection<KeyValuePair<!0, !1>>.Add(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x06006066 RID: 24678 RVA: 0x0013B59D File Offset: 0x0013979D
		void ICollection<KeyValuePair<!0, !1>>.Clear()
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x06006067 RID: 24679 RVA: 0x0013B59D File Offset: 0x0013979D
		bool ICollection<KeyValuePair<!0, !1>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x06006068 RID: 24680 RVA: 0x00142F04 File Offset: 0x00141104
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.m_dictionary.GetEnumerator();
		}

		// Token: 0x06006069 RID: 24681 RVA: 0x00142F11 File Offset: 0x00141111
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.m_dictionary.GetEnumerator();
		}

		// Token: 0x0600606A RID: 24682 RVA: 0x00142F1E File Offset: 0x0014111E
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return key is TKey;
		}

		// Token: 0x0600606B RID: 24683 RVA: 0x0013B59D File Offset: 0x0013979D
		void IDictionary.Add(object key, object value)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x0600606C RID: 24684 RVA: 0x0013B59D File Offset: 0x0013979D
		void IDictionary.Clear()
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x0600606D RID: 24685 RVA: 0x00142F37 File Offset: 0x00141137
		bool IDictionary.Contains(object key)
		{
			return ReadOnlyDictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x0600606E RID: 24686 RVA: 0x00142F50 File Offset: 0x00141150
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			IDictionary dictionary = this.m_dictionary as IDictionary;
			if (dictionary != null)
			{
				return dictionary.GetEnumerator();
			}
			return new ReadOnlyDictionary<TKey, TValue>.DictionaryEnumerator(this.m_dictionary);
		}

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x0600606F RID: 24687 RVA: 0x000040F7 File Offset: 0x000022F7
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06006070 RID: 24688 RVA: 0x000040F7 File Offset: 0x000022F7
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x06006071 RID: 24689 RVA: 0x00142EAD File Offset: 0x001410AD
		ICollection IDictionary.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x06006072 RID: 24690 RVA: 0x0013B59D File Offset: 0x0013979D
		void IDictionary.Remove(object key)
		{
			throw new NotSupportedException("Collection is read-only.");
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06006073 RID: 24691 RVA: 0x00142EC4 File Offset: 0x001410C4
		ICollection IDictionary.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x17001108 RID: 4360
		object IDictionary.this[object key]
		{
			get
			{
				if (ReadOnlyDictionary<TKey, TValue>.IsCompatibleKey(key))
				{
					return this[(TKey)((object)key)];
				}
				return null;
			}
			set
			{
				throw new NotSupportedException("Collection is read-only.");
			}
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x00142FA0 File Offset: 0x001411A0
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			if (array.GetLowerBound(0) != 0)
			{
				throw new ArgumentException("The lower bound of target array must be zero.");
			}
			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				this.m_dictionary.CopyTo(array2, index);
				return;
			}
			DictionaryEntry[] array3 = array as DictionaryEntry[];
			if (array3 != null)
			{
				using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = this.m_dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
						array3[index++] = new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
					}
					return;
				}
			}
			object[] array4 = array as object[];
			if (array4 == null)
			{
				throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
			}
			try
			{
				foreach (KeyValuePair<TKey, TValue> keyValuePair2 in this.m_dictionary)
				{
					array4[index++] = new KeyValuePair<TKey, TValue>(keyValuePair2.Key, keyValuePair2.Value);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException("Target array type is not compatible with the type of items in the collection.");
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06006077 RID: 24695 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x00143128 File Offset: 0x00141328
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					ICollection collection = this.m_dictionary as ICollection;
					if (collection != null)
					{
						this._syncRoot = collection.SyncRoot;
					}
					else
					{
						Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
					}
				}
				return this._syncRoot;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06006079 RID: 24697 RVA: 0x00142EAD File Offset: 0x001410AD
		IEnumerable<TKey> IReadOnlyDictionary<!0, !1>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x0600607A RID: 24698 RVA: 0x00142EC4 File Offset: 0x001410C4
		IEnumerable<TValue> IReadOnlyDictionary<!0, !1>.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x040039B1 RID: 14769
		private readonly IDictionary<TKey, TValue> m_dictionary;

		// Token: 0x040039B2 RID: 14770
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x040039B3 RID: 14771
		[NonSerialized]
		private ReadOnlyDictionary<TKey, TValue>.KeyCollection _keys;

		// Token: 0x040039B4 RID: 14772
		[NonSerialized]
		private ReadOnlyDictionary<TKey, TValue>.ValueCollection _values;

		// Token: 0x02000A84 RID: 2692
		[Serializable]
		private struct DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x0600607B RID: 24699 RVA: 0x00143172 File Offset: 0x00141372
			public DictionaryEnumerator(IDictionary<TKey, TValue> dictionary)
			{
				this._dictionary = dictionary;
				this._enumerator = this._dictionary.GetEnumerator();
			}

			// Token: 0x1700110D RID: 4365
			// (get) Token: 0x0600607C RID: 24700 RVA: 0x0014318C File Offset: 0x0014138C
			public DictionaryEntry Entry
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					object key = keyValuePair.Key;
					keyValuePair = this._enumerator.Current;
					return new DictionaryEntry(key, keyValuePair.Value);
				}
			}

			// Token: 0x1700110E RID: 4366
			// (get) Token: 0x0600607D RID: 24701 RVA: 0x001431D0 File Offset: 0x001413D0
			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x1700110F RID: 4367
			// (get) Token: 0x0600607E RID: 24702 RVA: 0x001431F8 File Offset: 0x001413F8
			public object Value
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x17001110 RID: 4368
			// (get) Token: 0x0600607F RID: 24703 RVA: 0x0014321D File Offset: 0x0014141D
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x06006080 RID: 24704 RVA: 0x0014322A File Offset: 0x0014142A
			public bool MoveNext()
			{
				return this._enumerator.MoveNext();
			}

			// Token: 0x06006081 RID: 24705 RVA: 0x00143237 File Offset: 0x00141437
			public void Reset()
			{
				this._enumerator.Reset();
			}

			// Token: 0x040039B5 RID: 14773
			private readonly IDictionary<TKey, TValue> _dictionary;

			// Token: 0x040039B6 RID: 14774
			private IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;
		}

		// Token: 0x02000A85 RID: 2693
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
		[Serializable]
		public sealed class KeyCollection : ICollection<!0>, IEnumerable<!0>, IEnumerable, ICollection, IReadOnlyCollection<TKey>
		{
			// Token: 0x06006082 RID: 24706 RVA: 0x00143244 File Offset: 0x00141444
			internal KeyCollection(ICollection<TKey> collection)
			{
				if (collection == null)
				{
					throw new ArgumentNullException("collection");
				}
				this._collection = collection;
			}

			// Token: 0x06006083 RID: 24707 RVA: 0x0013B59D File Offset: 0x0013979D
			void ICollection<!0>.Add(TKey item)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			// Token: 0x06006084 RID: 24708 RVA: 0x0013B59D File Offset: 0x0013979D
			void ICollection<!0>.Clear()
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			// Token: 0x06006085 RID: 24709 RVA: 0x00143261 File Offset: 0x00141461
			bool ICollection<!0>.Contains(TKey item)
			{
				return this._collection.Contains(item);
			}

			// Token: 0x06006086 RID: 24710 RVA: 0x0014326F File Offset: 0x0014146F
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				this._collection.CopyTo(array, arrayIndex);
			}

			// Token: 0x17001111 RID: 4369
			// (get) Token: 0x06006087 RID: 24711 RVA: 0x0014327E File Offset: 0x0014147E
			public int Count
			{
				get
				{
					return this._collection.Count;
				}
			}

			// Token: 0x17001112 RID: 4370
			// (get) Token: 0x06006088 RID: 24712 RVA: 0x000040F7 File Offset: 0x000022F7
			bool ICollection<!0>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06006089 RID: 24713 RVA: 0x0013B59D File Offset: 0x0013979D
			bool ICollection<!0>.Remove(TKey item)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			// Token: 0x0600608A RID: 24714 RVA: 0x0014328B File Offset: 0x0014148B
			public IEnumerator<TKey> GetEnumerator()
			{
				return this._collection.GetEnumerator();
			}

			// Token: 0x0600608B RID: 24715 RVA: 0x00143298 File Offset: 0x00141498
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._collection.GetEnumerator();
			}

			// Token: 0x0600608C RID: 24716 RVA: 0x001432A5 File Offset: 0x001414A5
			void ICollection.CopyTo(Array array, int index)
			{
				ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TKey>(this._collection, array, index);
			}

			// Token: 0x17001113 RID: 4371
			// (get) Token: 0x0600608D RID: 24717 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001114 RID: 4372
			// (get) Token: 0x0600608E RID: 24718 RVA: 0x001432B4 File Offset: 0x001414B4
			object ICollection.SyncRoot
			{
				get
				{
					if (this._syncRoot == null)
					{
						ICollection collection = this._collection as ICollection;
						if (collection != null)
						{
							this._syncRoot = collection.SyncRoot;
						}
						else
						{
							Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
						}
					}
					return this._syncRoot;
				}
			}

			// Token: 0x0600608F RID: 24719 RVA: 0x000173AD File Offset: 0x000155AD
			internal KeyCollection()
			{
				ThrowStub.ThrowNotSupportedException();
			}

			// Token: 0x040039B7 RID: 14775
			private readonly ICollection<TKey> _collection;

			// Token: 0x040039B8 RID: 14776
			[NonSerialized]
			private object _syncRoot;
		}

		// Token: 0x02000A86 RID: 2694
		[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
		[DebuggerDisplay("Count = {Count}")]
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, ICollection, IReadOnlyCollection<TValue>
		{
			// Token: 0x06006090 RID: 24720 RVA: 0x001432FE File Offset: 0x001414FE
			internal ValueCollection(ICollection<TValue> collection)
			{
				if (collection == null)
				{
					throw new ArgumentNullException("collection");
				}
				this._collection = collection;
			}

			// Token: 0x06006091 RID: 24721 RVA: 0x0013B59D File Offset: 0x0013979D
			void ICollection<!1>.Add(TValue item)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			// Token: 0x06006092 RID: 24722 RVA: 0x0013B59D File Offset: 0x0013979D
			void ICollection<!1>.Clear()
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			// Token: 0x06006093 RID: 24723 RVA: 0x0014331B File Offset: 0x0014151B
			bool ICollection<!1>.Contains(TValue item)
			{
				return this._collection.Contains(item);
			}

			// Token: 0x06006094 RID: 24724 RVA: 0x00143329 File Offset: 0x00141529
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				this._collection.CopyTo(array, arrayIndex);
			}

			// Token: 0x17001115 RID: 4373
			// (get) Token: 0x06006095 RID: 24725 RVA: 0x00143338 File Offset: 0x00141538
			public int Count
			{
				get
				{
					return this._collection.Count;
				}
			}

			// Token: 0x17001116 RID: 4374
			// (get) Token: 0x06006096 RID: 24726 RVA: 0x000040F7 File Offset: 0x000022F7
			bool ICollection<!1>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06006097 RID: 24727 RVA: 0x0013B59D File Offset: 0x0013979D
			bool ICollection<!1>.Remove(TValue item)
			{
				throw new NotSupportedException("Collection is read-only.");
			}

			// Token: 0x06006098 RID: 24728 RVA: 0x00143345 File Offset: 0x00141545
			public IEnumerator<TValue> GetEnumerator()
			{
				return this._collection.GetEnumerator();
			}

			// Token: 0x06006099 RID: 24729 RVA: 0x00143352 File Offset: 0x00141552
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._collection.GetEnumerator();
			}

			// Token: 0x0600609A RID: 24730 RVA: 0x0014335F File Offset: 0x0014155F
			void ICollection.CopyTo(Array array, int index)
			{
				ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TValue>(this._collection, array, index);
			}

			// Token: 0x17001117 RID: 4375
			// (get) Token: 0x0600609B RID: 24731 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001118 RID: 4376
			// (get) Token: 0x0600609C RID: 24732 RVA: 0x00143370 File Offset: 0x00141570
			object ICollection.SyncRoot
			{
				get
				{
					if (this._syncRoot == null)
					{
						ICollection collection = this._collection as ICollection;
						if (collection != null)
						{
							this._syncRoot = collection.SyncRoot;
						}
						else
						{
							Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
						}
					}
					return this._syncRoot;
				}
			}

			// Token: 0x0600609D RID: 24733 RVA: 0x000173AD File Offset: 0x000155AD
			internal ValueCollection()
			{
				ThrowStub.ThrowNotSupportedException();
			}

			// Token: 0x040039B9 RID: 14777
			private readonly ICollection<TValue> _collection;

			// Token: 0x040039BA RID: 14778
			[NonSerialized]
			private object _syncRoot;
		}
	}
}
