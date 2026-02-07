using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Collections.Concurrent
{
	// Token: 0x02000A58 RID: 2648
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(IDictionaryDebugView<, >))]
	[Serializable]
	public class ConcurrentDictionary<TKey, TValue> : IDictionary<!0, !1>, ICollection<KeyValuePair<!0, !1>>, IEnumerable<KeyValuePair<!0, !1>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<!0, !1>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x06005EF3 RID: 24307 RVA: 0x0013F0E0 File Offset: 0x0013D2E0
		private static bool IsValueWriteAtomic()
		{
			Type typeFromHandle = typeof(TValue);
			if (!typeFromHandle.IsValueType)
			{
				return true;
			}
			switch (Type.GetTypeCode(typeFromHandle))
			{
			case TypeCode.Boolean:
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Single:
				return true;
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Double:
				return IntPtr.Size == 8;
			default:
				return false;
			}
		}

		// Token: 0x06005EF4 RID: 24308 RVA: 0x0013F14F File Offset: 0x0013D34F
		public ConcurrentDictionary() : this(ConcurrentDictionary<TKey, TValue>.DefaultConcurrencyLevel, 31, true, null)
		{
		}

		// Token: 0x06005EF5 RID: 24309 RVA: 0x0013F160 File Offset: 0x0013D360
		public ConcurrentDictionary(int concurrencyLevel, int capacity) : this(concurrencyLevel, capacity, false, null)
		{
		}

		// Token: 0x06005EF6 RID: 24310 RVA: 0x0013F16C File Offset: 0x0013D36C
		public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, null)
		{
		}

		// Token: 0x06005EF7 RID: 24311 RVA: 0x0013F176 File Offset: 0x0013D376
		public ConcurrentDictionary(IEqualityComparer<TKey> comparer) : this(ConcurrentDictionary<TKey, TValue>.DefaultConcurrencyLevel, 31, true, comparer)
		{
		}

		// Token: 0x06005EF8 RID: 24312 RVA: 0x0013F187 File Offset: 0x0013D387
		public ConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : this(comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x06005EF9 RID: 24313 RVA: 0x0013F1A5 File Offset: 0x0013D3A5
		public ConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : this(concurrencyLevel, 31, false, comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x06005EFA RID: 24314 RVA: 0x0013F1C8 File Offset: 0x0013D3C8
		private void InitializeFromCollection(IEnumerable<KeyValuePair<TKey, TValue>> collection)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in collection)
			{
				if (keyValuePair.Key == null)
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
				}
				TValue tvalue;
				if (!this.TryAddInternal(keyValuePair.Key, this._comparer.GetHashCode(keyValuePair.Key), keyValuePair.Value, false, false, out tvalue))
				{
					throw new ArgumentException("The source argument contains duplicate keys.");
				}
			}
			if (this._budget == 0)
			{
				this._budget = this._tables._buckets.Length / this._tables._locks.Length;
			}
		}

		// Token: 0x06005EFB RID: 24315 RVA: 0x0013F280 File Offset: 0x0013D480
		public ConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer) : this(concurrencyLevel, capacity, false, comparer)
		{
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x0013F28C File Offset: 0x0013D48C
		internal ConcurrentDictionary(int concurrencyLevel, int capacity, bool growLockArray, IEqualityComparer<TKey> comparer)
		{
			if (concurrencyLevel < 1)
			{
				throw new ArgumentOutOfRangeException("concurrencyLevel", "The concurrencyLevel argument must be positive.");
			}
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", "The capacity argument must be greater than or equal to zero.");
			}
			if (capacity < concurrencyLevel)
			{
				capacity = concurrencyLevel;
			}
			object[] array = new object[concurrencyLevel];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new object();
			}
			int[] countPerLock = new int[array.Length];
			ConcurrentDictionary<TKey, TValue>.Node[] array2 = new ConcurrentDictionary<TKey, TValue>.Node[capacity];
			this._tables = new ConcurrentDictionary<TKey, TValue>.Tables(array2, array, countPerLock);
			this._comparer = (comparer ?? EqualityComparer<TKey>.Default);
			this._growLockArray = growLockArray;
			this._budget = array2.Length / array.Length;
		}

		// Token: 0x06005EFD RID: 24317 RVA: 0x0013F330 File Offset: 0x0013D530
		public bool TryAdd(TKey key, TValue value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			TValue tvalue;
			return this.TryAddInternal(key, this._comparer.GetHashCode(key), value, false, true, out tvalue);
		}

		// Token: 0x06005EFE RID: 24318 RVA: 0x0013F364 File Offset: 0x0013D564
		public bool ContainsKey(TKey key)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			TValue tvalue;
			return this.TryGetValue(key, out tvalue);
		}

		// Token: 0x06005EFF RID: 24319 RVA: 0x0013F388 File Offset: 0x0013D588
		public bool TryRemove(TKey key, out TValue value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			return this.TryRemoveInternal(key, out value, false, default(TValue));
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x0013F3B4 File Offset: 0x0013D5B4
		private bool TryRemoveInternal(TKey key, out TValue value, bool matchValue, TValue oldValue)
		{
			int hashCode = this._comparer.GetHashCode(key);
			for (;;)
			{
				ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
				int num;
				int num2;
				ConcurrentDictionary<TKey, TValue>.GetBucketAndLockNo(hashCode, out num, out num2, tables._buckets.Length, tables._locks.Length);
				object obj = tables._locks[num2];
				lock (obj)
				{
					if (tables != this._tables)
					{
						continue;
					}
					ConcurrentDictionary<TKey, TValue>.Node node = null;
					ConcurrentDictionary<TKey, TValue>.Node node2 = tables._buckets[num];
					while (node2 != null)
					{
						if (hashCode == node2._hashcode && this._comparer.Equals(node2._key, key))
						{
							if (matchValue && !EqualityComparer<TValue>.Default.Equals(oldValue, node2._value))
							{
								value = default(TValue);
								return false;
							}
							if (node == null)
							{
								Volatile.Write<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[num], node2._next);
							}
							else
							{
								node._next = node2._next;
							}
							value = node2._value;
							tables._countPerLock[num2]--;
							return true;
						}
						else
						{
							node = node2;
							node2 = node2._next;
						}
					}
				}
				break;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x06005F01 RID: 24321 RVA: 0x0013F508 File Offset: 0x0013D708
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			return this.TryGetValueInternal(key, this._comparer.GetHashCode(key), out value);
		}

		// Token: 0x06005F02 RID: 24322 RVA: 0x0013F52C File Offset: 0x0013D72C
		private bool TryGetValueInternal(TKey key, int hashcode, out TValue value)
		{
			ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
			int bucket = ConcurrentDictionary<TKey, TValue>.GetBucket(hashcode, tables._buckets.Length);
			for (ConcurrentDictionary<TKey, TValue>.Node node = Volatile.Read<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[bucket]); node != null; node = node._next)
			{
				if (hashcode == node._hashcode && this._comparer.Equals(node._key, key))
				{
					value = node._value;
					return true;
				}
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x06005F03 RID: 24323 RVA: 0x0013F5A4 File Offset: 0x0013D7A4
		public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			return this.TryUpdateInternal(key, this._comparer.GetHashCode(key), newValue, comparisonValue);
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x0013F5C8 File Offset: 0x0013D7C8
		private bool TryUpdateInternal(TKey key, int hashcode, TValue newValue, TValue comparisonValue)
		{
			IEqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			bool result;
			for (;;)
			{
				ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
				int num;
				int num2;
				ConcurrentDictionary<TKey, TValue>.GetBucketAndLockNo(hashcode, out num, out num2, tables._buckets.Length, tables._locks.Length);
				object obj = tables._locks[num2];
				lock (obj)
				{
					if (tables != this._tables)
					{
						continue;
					}
					ConcurrentDictionary<TKey, TValue>.Node node = null;
					ConcurrentDictionary<TKey, TValue>.Node node2 = tables._buckets[num];
					while (node2 != null)
					{
						if (hashcode == node2._hashcode && this._comparer.Equals(node2._key, key))
						{
							if (@default.Equals(node2._value, comparisonValue))
							{
								if (ConcurrentDictionary<TKey, TValue>.s_isValueWriteAtomic)
								{
									node2._value = newValue;
								}
								else
								{
									ConcurrentDictionary<TKey, TValue>.Node node3 = new ConcurrentDictionary<TKey, TValue>.Node(node2._key, newValue, hashcode, node2._next);
									if (node == null)
									{
										Volatile.Write<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[num], node3);
									}
									else
									{
										node._next = node3;
									}
								}
								return true;
							}
							return false;
						}
						else
						{
							node = node2;
							node2 = node2._next;
						}
					}
					result = false;
				}
				break;
			}
			return result;
		}

		// Token: 0x06005F05 RID: 24325 RVA: 0x0013F6F4 File Offset: 0x0013D8F4
		public void Clear()
		{
			int toExclusive = 0;
			try
			{
				this.AcquireAllLocks(ref toExclusive);
				ConcurrentDictionary<TKey, TValue>.Tables tables = new ConcurrentDictionary<TKey, TValue>.Tables(new ConcurrentDictionary<TKey, TValue>.Node[31], this._tables._locks, new int[this._tables._countPerLock.Length]);
				this._tables = tables;
				this._budget = Math.Max(1, tables._buckets.Length / tables._locks.Length);
			}
			finally
			{
				this.ReleaseLocks(0, toExclusive);
			}
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x0013F77C File Offset: 0x0013D97C
		void ICollection<KeyValuePair<!0, !1>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "The index argument is less than zero.");
			}
			int toExclusive = 0;
			try
			{
				this.AcquireAllLocks(ref toExclusive);
				int num = 0;
				int num2 = 0;
				while (num2 < this._tables._locks.Length && num >= 0)
				{
					num += this._tables._countPerLock[num2];
					num2++;
				}
				if (array.Length - num < index || num < 0)
				{
					throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the dictionary is greater than the available space from index to the end of the destination array.");
				}
				this.CopyToPairs(array, index);
			}
			finally
			{
				this.ReleaseLocks(0, toExclusive);
			}
		}

		// Token: 0x06005F07 RID: 24327 RVA: 0x0013F824 File Offset: 0x0013DA24
		public KeyValuePair<TKey, TValue>[] ToArray()
		{
			int toExclusive = 0;
			checked
			{
				KeyValuePair<TKey, TValue>[] result;
				try
				{
					this.AcquireAllLocks(ref toExclusive);
					int num = 0;
					for (int i = 0; i < this._tables._locks.Length; i++)
					{
						num += this._tables._countPerLock[i];
					}
					if (num == 0)
					{
						result = Array.Empty<KeyValuePair<TKey, TValue>>();
					}
					else
					{
						KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[num];
						this.CopyToPairs(array, 0);
						result = array;
					}
				}
				finally
				{
					this.ReleaseLocks(0, toExclusive);
				}
				return result;
			}
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x0013F8A8 File Offset: 0x0013DAA8
		private void CopyToPairs(KeyValuePair<TKey, TValue>[] array, int index)
		{
			foreach (ConcurrentDictionary<TKey, TValue>.Node node in this._tables._buckets)
			{
				while (node != null)
				{
					array[index] = new KeyValuePair<TKey, TValue>(node._key, node._value);
					index++;
					node = node._next;
				}
			}
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x0013F900 File Offset: 0x0013DB00
		private void CopyToEntries(DictionaryEntry[] array, int index)
		{
			foreach (ConcurrentDictionary<TKey, TValue>.Node node in this._tables._buckets)
			{
				while (node != null)
				{
					array[index] = new DictionaryEntry(node._key, node._value);
					index++;
					node = node._next;
				}
			}
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x0013F964 File Offset: 0x0013DB64
		private void CopyToObjects(object[] array, int index)
		{
			foreach (ConcurrentDictionary<TKey, TValue>.Node node in this._tables._buckets)
			{
				while (node != null)
				{
					array[index] = new KeyValuePair<TKey, TValue>(node._key, node._value);
					index++;
					node = node._next;
				}
			}
		}

		// Token: 0x06005F0B RID: 24331 RVA: 0x0013F9BD File Offset: 0x0013DBBD
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			ConcurrentDictionary<TKey, TValue>.Node[] buckets = this._tables._buckets;
			int num;
			for (int i = 0; i < buckets.Length; i = num + 1)
			{
				ConcurrentDictionary<TKey, TValue>.Node current;
				for (current = Volatile.Read<ConcurrentDictionary<TKey, TValue>.Node>(ref buckets[i]); current != null; current = current._next)
				{
					yield return new KeyValuePair<TKey, TValue>(current._key, current._value);
				}
				current = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x06005F0C RID: 24332 RVA: 0x0013F9CC File Offset: 0x0013DBCC
		private bool TryAddInternal(TKey key, int hashcode, TValue value, bool updateIfExists, bool acquireLock, out TValue resultingValue)
		{
			checked
			{
				ConcurrentDictionary<TKey, TValue>.Tables tables;
				bool flag;
				for (;;)
				{
					tables = this._tables;
					int num;
					int num2;
					ConcurrentDictionary<TKey, TValue>.GetBucketAndLockNo(hashcode, out num, out num2, tables._buckets.Length, tables._locks.Length);
					flag = false;
					bool flag2 = false;
					try
					{
						if (acquireLock)
						{
							Monitor.Enter(tables._locks[num2], ref flag2);
						}
						if (tables != this._tables)
						{
							continue;
						}
						ConcurrentDictionary<TKey, TValue>.Node node = null;
						for (ConcurrentDictionary<TKey, TValue>.Node node2 = tables._buckets[num]; node2 != null; node2 = node2._next)
						{
							if (hashcode == node2._hashcode && this._comparer.Equals(node2._key, key))
							{
								if (updateIfExists)
								{
									if (ConcurrentDictionary<TKey, TValue>.s_isValueWriteAtomic)
									{
										node2._value = value;
									}
									else
									{
										ConcurrentDictionary<TKey, TValue>.Node node3 = new ConcurrentDictionary<TKey, TValue>.Node(node2._key, value, hashcode, node2._next);
										if (node == null)
										{
											Volatile.Write<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[num], node3);
										}
										else
										{
											node._next = node3;
										}
									}
									resultingValue = value;
								}
								else
								{
									resultingValue = node2._value;
								}
								return false;
							}
							node = node2;
						}
						Volatile.Write<ConcurrentDictionary<TKey, TValue>.Node>(ref tables._buckets[num], new ConcurrentDictionary<TKey, TValue>.Node(key, value, hashcode, tables._buckets[num]));
						tables._countPerLock[num2]++;
						if (tables._countPerLock[num2] > this._budget)
						{
							flag = true;
						}
					}
					finally
					{
						if (flag2)
						{
							Monitor.Exit(tables._locks[num2]);
						}
					}
					break;
				}
				if (flag)
				{
					this.GrowTable(tables);
				}
				resultingValue = value;
				return true;
			}
		}

		// Token: 0x170010A8 RID: 4264
		public TValue this[TKey key]
		{
			get
			{
				TValue result;
				if (!this.TryGetValue(key, out result))
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNotFoundException(key);
				}
				return result;
			}
			set
			{
				if (key == null)
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
				}
				TValue tvalue;
				this.TryAddInternal(key, this._comparer.GetHashCode(key), value, true, true, out tvalue);
			}
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x0004DFAA File Offset: 0x0004C1AA
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowKeyNotFoundException(object key)
		{
			throw new KeyNotFoundException(SR.Format("The given key '{0}' was not present in the dictionary.", key.ToString()));
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x0013FBC3 File Offset: 0x0013DDC3
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowKeyNullException()
		{
			throw new ArgumentNullException("key");
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06005F11 RID: 24337 RVA: 0x0013FBD0 File Offset: 0x0013DDD0
		public int Count
		{
			get
			{
				int toExclusive = 0;
				int countInternal;
				try
				{
					this.AcquireAllLocks(ref toExclusive);
					countInternal = this.GetCountInternal();
				}
				finally
				{
					this.ReleaseLocks(0, toExclusive);
				}
				return countInternal;
			}
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x0013FC0C File Offset: 0x0013DE0C
		private int GetCountInternal()
		{
			int num = 0;
			for (int i = 0; i < this._tables._countPerLock.Length; i++)
			{
				num += this._tables._countPerLock[i];
			}
			return num;
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x0013FC4C File Offset: 0x0013DE4C
		public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (valueFactory == null)
			{
				throw new ArgumentNullException("valueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue result;
			if (!this.TryGetValueInternal(key, hashCode, out result))
			{
				this.TryAddInternal(key, hashCode, valueFactory(key), false, true, out result);
			}
			return result;
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x0013FCA4 File Offset: 0x0013DEA4
		public TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (valueFactory == null)
			{
				throw new ArgumentNullException("valueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue result;
			if (!this.TryGetValueInternal(key, hashCode, out result))
			{
				this.TryAddInternal(key, hashCode, valueFactory(key, factoryArgument), false, true, out result);
			}
			return result;
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x0013FCFC File Offset: 0x0013DEFC
		public TValue GetOrAdd(TKey key, TValue value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue result;
			if (!this.TryGetValueInternal(key, hashCode, out result))
			{
				this.TryAddInternal(key, hashCode, value, false, true, out result);
			}
			return result;
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x0013FD40 File Offset: 0x0013DF40
		public TValue AddOrUpdate<TArg>(TKey key, Func<TKey, TArg, TValue> addValueFactory, Func<TKey, TValue, TArg, TValue> updateValueFactory, TArg factoryArgument)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (addValueFactory == null)
			{
				throw new ArgumentNullException("addValueFactory");
			}
			if (updateValueFactory == null)
			{
				throw new ArgumentNullException("updateValueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue2;
			for (;;)
			{
				TValue tvalue;
				TValue result;
				if (this.TryGetValueInternal(key, hashCode, out tvalue))
				{
					tvalue2 = updateValueFactory(key, tvalue, factoryArgument);
					if (this.TryUpdateInternal(key, hashCode, tvalue2, tvalue))
					{
						break;
					}
				}
				else if (this.TryAddInternal(key, hashCode, addValueFactory(key, factoryArgument), false, true, out result))
				{
					return result;
				}
			}
			return tvalue2;
		}

		// Token: 0x06005F17 RID: 24343 RVA: 0x0013FDC0 File Offset: 0x0013DFC0
		public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (addValueFactory == null)
			{
				throw new ArgumentNullException("addValueFactory");
			}
			if (updateValueFactory == null)
			{
				throw new ArgumentNullException("updateValueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue2;
			for (;;)
			{
				TValue tvalue;
				TValue result;
				if (this.TryGetValueInternal(key, hashCode, out tvalue))
				{
					tvalue2 = updateValueFactory(key, tvalue);
					if (this.TryUpdateInternal(key, hashCode, tvalue2, tvalue))
					{
						break;
					}
				}
				else if (this.TryAddInternal(key, hashCode, addValueFactory(key), false, true, out result))
				{
					return result;
				}
			}
			return tvalue2;
		}

		// Token: 0x06005F18 RID: 24344 RVA: 0x0013FE3C File Offset: 0x0013E03C
		public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (updateValueFactory == null)
			{
				throw new ArgumentNullException("updateValueFactory");
			}
			int hashCode = this._comparer.GetHashCode(key);
			TValue tvalue2;
			for (;;)
			{
				TValue tvalue;
				TValue result;
				if (this.TryGetValueInternal(key, hashCode, out tvalue))
				{
					tvalue2 = updateValueFactory(key, tvalue);
					if (this.TryUpdateInternal(key, hashCode, tvalue2, tvalue))
					{
						break;
					}
				}
				else if (this.TryAddInternal(key, hashCode, addValue, false, true, out result))
				{
					return result;
				}
			}
			return tvalue2;
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06005F19 RID: 24345 RVA: 0x0013FEA4 File Offset: 0x0013E0A4
		public bool IsEmpty
		{
			get
			{
				int toExclusive = 0;
				try
				{
					this.AcquireAllLocks(ref toExclusive);
					for (int i = 0; i < this._tables._countPerLock.Length; i++)
					{
						if (this._tables._countPerLock[i] != 0)
						{
							return false;
						}
					}
				}
				finally
				{
					this.ReleaseLocks(0, toExclusive);
				}
				return true;
			}
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x0013FF0C File Offset: 0x0013E10C
		void IDictionary<!0, !1>.Add(TKey key, TValue value)
		{
			if (!this.TryAdd(key, value))
			{
				throw new ArgumentException("The key already existed in the dictionary.");
			}
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x0013FF24 File Offset: 0x0013E124
		bool IDictionary<!0, !1>.Remove(TKey key)
		{
			TValue tvalue;
			return this.TryRemove(key, out tvalue);
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06005F1C RID: 24348 RVA: 0x0013FF3A File Offset: 0x0013E13A
		public ICollection<TKey> Keys
		{
			get
			{
				return this.GetKeys();
			}
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06005F1D RID: 24349 RVA: 0x0013FF3A File Offset: 0x0013E13A
		IEnumerable<TKey> IReadOnlyDictionary<!0, !1>.Keys
		{
			get
			{
				return this.GetKeys();
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06005F1E RID: 24350 RVA: 0x0013FF42 File Offset: 0x0013E142
		public ICollection<TValue> Values
		{
			get
			{
				return this.GetValues();
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06005F1F RID: 24351 RVA: 0x0013FF42 File Offset: 0x0013E142
		IEnumerable<TValue> IReadOnlyDictionary<!0, !1>.Values
		{
			get
			{
				return this.GetValues();
			}
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x0013FF4A File Offset: 0x0013E14A
		void ICollection<KeyValuePair<!0, !1>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			((IDictionary<!0, !1>)this).Add(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x06005F21 RID: 24353 RVA: 0x0013FF60 File Offset: 0x0013E160
		bool ICollection<KeyValuePair<!0, !1>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			TValue x;
			return this.TryGetValue(keyValuePair.Key, out x) && EqualityComparer<TValue>.Default.Equals(x, keyValuePair.Value);
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06005F22 RID: 24354 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection<KeyValuePair<!0, !1>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005F23 RID: 24355 RVA: 0x0013FF94 File Offset: 0x0013E194
		bool ICollection<KeyValuePair<!0, !1>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			if (keyValuePair.Key == null)
			{
				throw new ArgumentNullException("keyValuePair", "TKey is a reference type and item.Key is null.");
			}
			TValue tvalue;
			return this.TryRemoveInternal(keyValuePair.Key, out tvalue, true, keyValuePair.Value);
		}

		// Token: 0x06005F24 RID: 24356 RVA: 0x0013FFD6 File Offset: 0x0013E1D6
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06005F25 RID: 24357 RVA: 0x0013FFE0 File Offset: 0x0013E1E0
		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (!(key is TKey))
			{
				throw new ArgumentException("The key was of an incorrect type for this dictionary.");
			}
			TValue value2;
			try
			{
				value2 = (TValue)((object)value);
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException("The value was of an incorrect type for this dictionary.");
			}
			((IDictionary<!0, !1>)this).Add((TKey)((object)key), value2);
		}

		// Token: 0x06005F26 RID: 24358 RVA: 0x0014003C File Offset: 0x0013E23C
		bool IDictionary.Contains(object key)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			return key is TKey && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x06005F27 RID: 24359 RVA: 0x0014005C File Offset: 0x0013E25C
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new ConcurrentDictionary<TKey, TValue>.DictionaryEnumerator(this);
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06005F28 RID: 24360 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06005F29 RID: 24361 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x06005F2A RID: 24362 RVA: 0x0013FF3A File Offset: 0x0013E13A
		ICollection IDictionary.Keys
		{
			get
			{
				return this.GetKeys();
			}
		}

		// Token: 0x06005F2B RID: 24363 RVA: 0x00140064 File Offset: 0x0013E264
		void IDictionary.Remove(object key)
		{
			if (key == null)
			{
				ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
			}
			if (key is TKey)
			{
				TValue tvalue;
				this.TryRemove((TKey)((object)key), out tvalue);
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06005F2C RID: 24364 RVA: 0x0013FF42 File Offset: 0x0013E142
		ICollection IDictionary.Values
		{
			get
			{
				return this.GetValues();
			}
		}

		// Token: 0x170010B4 RID: 4276
		object IDictionary.this[object key]
		{
			get
			{
				if (key == null)
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
				}
				TValue tvalue;
				if (key is TKey && this.TryGetValue((TKey)((object)key), out tvalue))
				{
					return tvalue;
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					ConcurrentDictionary<TKey, TValue>.ThrowKeyNullException();
				}
				if (!(key is TKey))
				{
					throw new ArgumentException("The key was of an incorrect type for this dictionary.");
				}
				if (!(value is TValue))
				{
					throw new ArgumentException("The value was of an incorrect type for this dictionary.");
				}
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x06005F2F RID: 24367 RVA: 0x00140118 File Offset: 0x0013E318
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "The index argument is less than zero.");
			}
			int toExclusive = 0;
			try
			{
				this.AcquireAllLocks(ref toExclusive);
				ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
				int num = 0;
				int num2 = 0;
				while (num2 < tables._locks.Length && num >= 0)
				{
					num += tables._countPerLock[num2];
					num2++;
				}
				if (array.Length - num < index || num < 0)
				{
					throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the dictionary is greater than the available space from index to the end of the destination array.");
				}
				KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
				if (array2 != null)
				{
					this.CopyToPairs(array2, index);
				}
				else
				{
					DictionaryEntry[] array3 = array as DictionaryEntry[];
					if (array3 != null)
					{
						this.CopyToEntries(array3, index);
					}
					else
					{
						object[] array4 = array as object[];
						if (array4 == null)
						{
							throw new ArgumentException("The array is multidimensional, or the type parameter for the set cannot be cast automatically to the type of the destination array.", "array");
						}
						this.CopyToObjects(array4, index);
					}
				}
			}
			finally
			{
				this.ReleaseLocks(0, toExclusive);
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06005F30 RID: 24368 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06005F31 RID: 24369 RVA: 0x0013E2AF File Offset: 0x0013C4AF
		object ICollection.SyncRoot
		{
			get
			{
				throw new NotSupportedException("The SyncRoot property may not be used for the synchronization of concurrent collections.");
			}
		}

		// Token: 0x06005F32 RID: 24370 RVA: 0x0014020C File Offset: 0x0013E40C
		private void GrowTable(ConcurrentDictionary<TKey, TValue>.Tables tables)
		{
			int toExclusive = 0;
			try
			{
				this.AcquireLocks(0, 1, ref toExclusive);
				if (tables == this._tables)
				{
					long num = 0L;
					for (int i = 0; i < tables._countPerLock.Length; i++)
					{
						num += (long)tables._countPerLock[i];
					}
					if (num < (long)(tables._buckets.Length / 4))
					{
						this._budget = 2 * this._budget;
						if (this._budget < 0)
						{
							this._budget = int.MaxValue;
						}
					}
					else
					{
						int num2 = 0;
						bool flag = false;
						object[] array;
						checked
						{
							try
							{
								num2 = tables._buckets.Length * 2 + 1;
								while (num2 % 3 == 0 || num2 % 5 == 0 || num2 % 7 == 0)
								{
									num2 += 2;
								}
								if (num2 > 2146435071)
								{
									flag = true;
								}
							}
							catch (OverflowException)
							{
								flag = true;
							}
							if (flag)
							{
								num2 = 2146435071;
								this._budget = int.MaxValue;
							}
							this.AcquireLocks(1, tables._locks.Length, ref toExclusive);
							array = tables._locks;
						}
						if (this._growLockArray && tables._locks.Length < 1024)
						{
							array = new object[tables._locks.Length * 2];
							Array.Copy(tables._locks, 0, array, 0, tables._locks.Length);
							for (int j = tables._locks.Length; j < array.Length; j++)
							{
								array[j] = new object();
							}
						}
						ConcurrentDictionary<TKey, TValue>.Node[] array2 = new ConcurrentDictionary<TKey, TValue>.Node[num2];
						int[] array3 = new int[array.Length];
						for (int k = 0; k < tables._buckets.Length; k++)
						{
							checked
							{
								ConcurrentDictionary<TKey, TValue>.Node next;
								for (ConcurrentDictionary<TKey, TValue>.Node node = tables._buckets[k]; node != null; node = next)
								{
									next = node._next;
									int num3;
									int num4;
									ConcurrentDictionary<TKey, TValue>.GetBucketAndLockNo(node._hashcode, out num3, out num4, array2.Length, array.Length);
									array2[num3] = new ConcurrentDictionary<TKey, TValue>.Node(node._key, node._value, node._hashcode, array2[num3]);
									array3[num4]++;
								}
							}
						}
						this._budget = Math.Max(1, array2.Length / array.Length);
						this._tables = new ConcurrentDictionary<TKey, TValue>.Tables(array2, array, array3);
					}
				}
			}
			finally
			{
				this.ReleaseLocks(0, toExclusive);
			}
		}

		// Token: 0x06005F33 RID: 24371 RVA: 0x00140454 File Offset: 0x0013E654
		private static int GetBucket(int hashcode, int bucketCount)
		{
			return (hashcode & int.MaxValue) % bucketCount;
		}

		// Token: 0x06005F34 RID: 24372 RVA: 0x0014045F File Offset: 0x0013E65F
		private static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount)
		{
			bucketNo = (hashcode & int.MaxValue) % bucketCount;
			lockNo = bucketNo % lockCount;
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06005F35 RID: 24373 RVA: 0x00140473 File Offset: 0x0013E673
		private static int DefaultConcurrencyLevel
		{
			get
			{
				return PlatformHelper.ProcessorCount;
			}
		}

		// Token: 0x06005F36 RID: 24374 RVA: 0x0014047C File Offset: 0x0013E67C
		private void AcquireAllLocks(ref int locksAcquired)
		{
			if (CDSCollectionETWBCLProvider.Log.IsEnabled())
			{
				CDSCollectionETWBCLProvider.Log.ConcurrentDictionary_AcquiringAllLocks(this._tables._buckets.Length);
			}
			this.AcquireLocks(0, 1, ref locksAcquired);
			this.AcquireLocks(1, this._tables._locks.Length, ref locksAcquired);
		}

		// Token: 0x06005F37 RID: 24375 RVA: 0x001404D0 File Offset: 0x0013E6D0
		private void AcquireLocks(int fromInclusive, int toExclusive, ref int locksAcquired)
		{
			object[] locks = this._tables._locks;
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				bool flag = false;
				try
				{
					Monitor.Enter(locks[i], ref flag);
				}
				finally
				{
					if (flag)
					{
						locksAcquired++;
					}
				}
			}
		}

		// Token: 0x06005F38 RID: 24376 RVA: 0x00140520 File Offset: 0x0013E720
		private void ReleaseLocks(int fromInclusive, int toExclusive)
		{
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				Monitor.Exit(this._tables._locks[i]);
			}
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x00140550 File Offset: 0x0013E750
		private ReadOnlyCollection<TKey> GetKeys()
		{
			int toExclusive = 0;
			ReadOnlyCollection<TKey> result;
			try
			{
				this.AcquireAllLocks(ref toExclusive);
				int countInternal = this.GetCountInternal();
				if (countInternal < 0)
				{
					throw new OutOfMemoryException();
				}
				List<TKey> list = new List<TKey>(countInternal);
				for (int i = 0; i < this._tables._buckets.Length; i++)
				{
					for (ConcurrentDictionary<TKey, TValue>.Node node = this._tables._buckets[i]; node != null; node = node._next)
					{
						list.Add(node._key);
					}
				}
				result = new ReadOnlyCollection<TKey>(list);
			}
			finally
			{
				this.ReleaseLocks(0, toExclusive);
			}
			return result;
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x001405E8 File Offset: 0x0013E7E8
		private ReadOnlyCollection<TValue> GetValues()
		{
			int toExclusive = 0;
			ReadOnlyCollection<TValue> result;
			try
			{
				this.AcquireAllLocks(ref toExclusive);
				int countInternal = this.GetCountInternal();
				if (countInternal < 0)
				{
					throw new OutOfMemoryException();
				}
				List<TValue> list = new List<TValue>(countInternal);
				for (int i = 0; i < this._tables._buckets.Length; i++)
				{
					for (ConcurrentDictionary<TKey, TValue>.Node node = this._tables._buckets[i]; node != null; node = node._next)
					{
						list.Add(node._value);
					}
				}
				result = new ReadOnlyCollection<TValue>(list);
			}
			finally
			{
				this.ReleaseLocks(0, toExclusive);
			}
			return result;
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x00140680 File Offset: 0x0013E880
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			ConcurrentDictionary<TKey, TValue>.Tables tables = this._tables;
			this._serializationArray = this.ToArray();
			this._serializationConcurrencyLevel = tables._locks.Length;
			this._serializationCapacity = tables._buckets.Length;
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x001406BE File Offset: 0x0013E8BE
		[OnSerialized]
		private void OnSerialized(StreamingContext context)
		{
			this._serializationArray = null;
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x001406C8 File Offset: 0x0013E8C8
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			KeyValuePair<TKey, TValue>[] serializationArray = this._serializationArray;
			ConcurrentDictionary<TKey, TValue>.Node[] buckets = new ConcurrentDictionary<TKey, TValue>.Node[this._serializationCapacity];
			int[] countPerLock = new int[this._serializationConcurrencyLevel];
			object[] array = new object[this._serializationConcurrencyLevel];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new object();
			}
			this._tables = new ConcurrentDictionary<TKey, TValue>.Tables(buckets, array, countPerLock);
			this.InitializeFromCollection(serializationArray);
			this._serializationArray = null;
		}

		// Token: 0x0400393D RID: 14653
		[NonSerialized]
		private volatile ConcurrentDictionary<TKey, TValue>.Tables _tables;

		// Token: 0x0400393E RID: 14654
		private IEqualityComparer<TKey> _comparer;

		// Token: 0x0400393F RID: 14655
		[NonSerialized]
		private readonly bool _growLockArray;

		// Token: 0x04003940 RID: 14656
		[NonSerialized]
		private int _budget;

		// Token: 0x04003941 RID: 14657
		private KeyValuePair<TKey, TValue>[] _serializationArray;

		// Token: 0x04003942 RID: 14658
		private int _serializationConcurrencyLevel;

		// Token: 0x04003943 RID: 14659
		private int _serializationCapacity;

		// Token: 0x04003944 RID: 14660
		private const int DefaultCapacity = 31;

		// Token: 0x04003945 RID: 14661
		private const int MaxLockNumber = 1024;

		// Token: 0x04003946 RID: 14662
		private static readonly bool s_isValueWriteAtomic = ConcurrentDictionary<TKey, TValue>.IsValueWriteAtomic();

		// Token: 0x02000A59 RID: 2649
		private sealed class Tables
		{
			// Token: 0x06005F3F RID: 24383 RVA: 0x00140745 File Offset: 0x0013E945
			internal Tables(ConcurrentDictionary<TKey, TValue>.Node[] buckets, object[] locks, int[] countPerLock)
			{
				this._buckets = buckets;
				this._locks = locks;
				this._countPerLock = countPerLock;
			}

			// Token: 0x04003947 RID: 14663
			internal readonly ConcurrentDictionary<TKey, TValue>.Node[] _buckets;

			// Token: 0x04003948 RID: 14664
			internal readonly object[] _locks;

			// Token: 0x04003949 RID: 14665
			internal volatile int[] _countPerLock;
		}

		// Token: 0x02000A5A RID: 2650
		[Serializable]
		private sealed class Node
		{
			// Token: 0x06005F40 RID: 24384 RVA: 0x00140764 File Offset: 0x0013E964
			internal Node(TKey key, TValue value, int hashcode, ConcurrentDictionary<TKey, TValue>.Node next)
			{
				this._key = key;
				this._value = value;
				this._next = next;
				this._hashcode = hashcode;
			}

			// Token: 0x0400394A RID: 14666
			internal readonly TKey _key;

			// Token: 0x0400394B RID: 14667
			internal TValue _value;

			// Token: 0x0400394C RID: 14668
			internal volatile ConcurrentDictionary<TKey, TValue>.Node _next;

			// Token: 0x0400394D RID: 14669
			internal readonly int _hashcode;
		}

		// Token: 0x02000A5B RID: 2651
		[Serializable]
		private sealed class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06005F41 RID: 24385 RVA: 0x0014078B File Offset: 0x0013E98B
			internal DictionaryEnumerator(ConcurrentDictionary<TKey, TValue> dictionary)
			{
				this._enumerator = dictionary.GetEnumerator();
			}

			// Token: 0x170010B8 RID: 4280
			// (get) Token: 0x06005F42 RID: 24386 RVA: 0x001407A0 File Offset: 0x0013E9A0
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

			// Token: 0x170010B9 RID: 4281
			// (get) Token: 0x06005F43 RID: 24387 RVA: 0x001407E4 File Offset: 0x0013E9E4
			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x170010BA RID: 4282
			// (get) Token: 0x06005F44 RID: 24388 RVA: 0x0014080C File Offset: 0x0013EA0C
			public object Value
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x170010BB RID: 4283
			// (get) Token: 0x06005F45 RID: 24389 RVA: 0x00140831 File Offset: 0x0013EA31
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x06005F46 RID: 24390 RVA: 0x0014083E File Offset: 0x0013EA3E
			public bool MoveNext()
			{
				return this._enumerator.MoveNext();
			}

			// Token: 0x06005F47 RID: 24391 RVA: 0x0014084B File Offset: 0x0013EA4B
			public void Reset()
			{
				this._enumerator.Reset();
			}

			// Token: 0x0400394E RID: 14670
			private IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;
		}
	}
}
