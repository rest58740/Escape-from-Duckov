using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public class CustomDataCollection : ICollection<CustomData>, IEnumerable<CustomData>, IEnumerable
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000026F7 File Offset: 0x000008F7
		private Dictionary<int, CustomData> Dictionary
		{
			get
			{
				if (this._dictionary == null || this.dirty)
				{
					this.RebuildDictionary();
				}
				return this._dictionary;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002715 File Offset: 0x00000915
		public int Count
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002722 File Offset: 0x00000922
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002725 File Offset: 0x00000925
		private void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002730 File Offset: 0x00000930
		private void RebuildDictionary()
		{
			if (this._dictionary == null)
			{
				this._dictionary = new Dictionary<int, CustomData>();
			}
			this._dictionary.Clear();
			foreach (CustomData customData in this.entries)
			{
				if (customData != null && !string.IsNullOrWhiteSpace(customData.Key))
				{
					this._dictionary[customData.Key.GetHashCode()] = customData;
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000027C4 File Offset: 0x000009C4
		public CustomData GetEntry(int hash)
		{
			CustomData result;
			if (this.Dictionary.TryGetValue(hash, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027E4 File Offset: 0x000009E4
		public CustomData GetEntry(string key)
		{
			int hashCode = key.GetHashCode();
			return this.GetEntry(hashCode);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002800 File Offset: 0x00000A00
		public void SetRaw(string key, CustomDataType type, byte[] bytes, bool createNewIfNotExist = true, bool display = false)
		{
			CustomData customData = this.GetEntry(key);
			if (customData == null)
			{
				if (createNewIfNotExist)
				{
					customData = new CustomData(key, type, bytes);
					this.Add(customData);
				}
				else
				{
					Debug.LogError("Data with key " + key + " doesn't exist.");
				}
			}
			else
			{
				customData.SetRaw(bytes);
			}
			customData.Display = display;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002854 File Offset: 0x00000A54
		public void SetRaw(int hash, CustomDataType type, byte[] bytes)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				Debug.LogError(string.Format("Data with hash {0} doesn't exist.", hash));
				return;
			}
			entry.SetRaw(bytes);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000288C File Offset: 0x00000A8C
		public byte[] GetRawCopied(string key, byte[] defaultResult = null)
		{
			CustomData entry = this.GetEntry(key);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetRawCopied();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028AC File Offset: 0x00000AAC
		public byte[] GetRawCopied(int hash, byte[] defaultResult = null)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetRawCopied();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000028CC File Offset: 0x00000ACC
		public float GetFloat(string key, float defaultResult = 0f)
		{
			CustomData entry = this.GetEntry(key);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetFloat();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000028EC File Offset: 0x00000AEC
		public int GetInt(string key, int defaultResult = 0)
		{
			CustomData entry = this.GetEntry(key);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetInt();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000290C File Offset: 0x00000B0C
		public bool GetBool(string key, bool defaultResult = false)
		{
			CustomData entry = this.GetEntry(key);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetBool();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000292C File Offset: 0x00000B2C
		public string GetString(string key, string defaultResult = null)
		{
			CustomData entry = this.GetEntry(key);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetString();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000294C File Offset: 0x00000B4C
		public float GetFloat(int hash, float defaultResult = 0f)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetFloat();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000296C File Offset: 0x00000B6C
		public int GetInt(int hash, int defaultResult = 0)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetInt();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000298C File Offset: 0x00000B8C
		public bool GetBool(int hash, bool defaultResult = false)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetBool();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000029AC File Offset: 0x00000BAC
		public string GetString(int hash, string defaultResult = null)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				return defaultResult;
			}
			return entry.GetString();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000029CC File Offset: 0x00000BCC
		public void SetFloat(string key, float value, bool createNewIfNotExist = true)
		{
			CustomData customData = this.GetEntry(key);
			if (customData != null)
			{
				customData.SetFloat(value);
				return;
			}
			if (createNewIfNotExist)
			{
				customData = new CustomData(key, value);
				this.Add(customData);
				return;
			}
			Debug.LogError("Data with key " + key + " doesn't exist.");
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A14 File Offset: 0x00000C14
		public void SetInt(string key, int value, bool createNewIfNotExist = true)
		{
			CustomData customData = this.GetEntry(key);
			if (customData != null)
			{
				customData.SetInt(value);
				return;
			}
			if (createNewIfNotExist)
			{
				customData = new CustomData(key, value);
				this.Add(customData);
				return;
			}
			Debug.LogError("Data with key " + key + " doesn't exist.");
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A5C File Offset: 0x00000C5C
		public void SetBool(string key, bool value, bool createNewIfNotExist = true)
		{
			CustomData customData = this.GetEntry(key);
			if (customData != null)
			{
				customData.SetBool(value);
				return;
			}
			if (createNewIfNotExist)
			{
				customData = new CustomData(key, value);
				this.Add(customData);
				return;
			}
			Debug.LogError("Data with key " + key + " doesn't exist.");
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002AA4 File Offset: 0x00000CA4
		public void SetString(string key, string value, bool createNewIfNotExist = true)
		{
			CustomData customData = this.GetEntry(key);
			if (customData != null)
			{
				customData.SetString(value);
				return;
			}
			if (createNewIfNotExist)
			{
				customData = new CustomData(key, value);
				this.Add(customData);
				return;
			}
			Debug.LogError("Data with key " + key + " doesn't exist.");
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002AEC File Offset: 0x00000CEC
		public void SetFloat(int hash, float value)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				Debug.LogError(string.Format("Data with hash {0} not found", hash));
				return;
			}
			entry.SetFloat(value);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B24 File Offset: 0x00000D24
		public void SetInt(int hash, int value)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				Debug.LogError(string.Format("Data with hash {0} not found", hash));
				return;
			}
			entry.SetInt(value);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B5C File Offset: 0x00000D5C
		public void SetBool(int hash, bool value)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				Debug.LogError(string.Format("Data with hash {0} not found", hash));
				return;
			}
			entry.SetBool(value);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002B94 File Offset: 0x00000D94
		public void SetString(int hash, string value)
		{
			CustomData entry = this.GetEntry(hash);
			if (entry == null)
			{
				Debug.LogError(string.Format("Data with hash {0} not found", hash));
				return;
			}
			entry.SetString(value);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002BC9 File Offset: 0x00000DC9
		public void Set(string key, float value, bool createNewIfNotExist = true)
		{
			this.SetFloat(key, value, createNewIfNotExist);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public void Set(string key, int value, bool createNewIfNotExist = true)
		{
			this.SetInt(key, value, createNewIfNotExist);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002BDF File Offset: 0x00000DDF
		public void Set(string key, bool value, bool createNewIfNotExist = true)
		{
			this.SetBool(key, value, createNewIfNotExist);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002BEA File Offset: 0x00000DEA
		public void Set(string key, string value, bool createNewIfNotExist = true)
		{
			this.SetString(key, value, createNewIfNotExist);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BF5 File Offset: 0x00000DF5
		public void Set(int hash, float value)
		{
			this.SetFloat(hash, value);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002BFF File Offset: 0x00000DFF
		public void Set(int hash, int value)
		{
			this.SetInt(hash, value);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002C09 File Offset: 0x00000E09
		public void Set(int hash, bool value)
		{
			this.SetBool(hash, value);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C13 File Offset: 0x00000E13
		public void Set(int hash, string value)
		{
			this.SetString(hash, value);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002C1D File Offset: 0x00000E1D
		public void Add(CustomData item)
		{
			this.entries.Add(item);
			this.SetDirty();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002C31 File Offset: 0x00000E31
		public void Clear()
		{
			this.entries.Clear();
			this.SetDirty();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002C44 File Offset: 0x00000E44
		public bool Contains(CustomData item)
		{
			return this.entries.Contains(item);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C52 File Offset: 0x00000E52
		public void CopyTo(CustomData[] array, int arrayIndex)
		{
			this.entries.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002C61 File Offset: 0x00000E61
		public bool Remove(CustomData item)
		{
			bool result = this.entries.Remove(item);
			this.SetDirty();
			return result;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C75 File Offset: 0x00000E75
		public IEnumerator<CustomData> GetEnumerator()
		{
			return this.entries.GetEnumerator();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C87 File Offset: 0x00000E87
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.entries.GetEnumerator();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C9C File Offset: 0x00000E9C
		public void SetDisplay(string key, bool display)
		{
			CustomData entry = this.GetEntry(key);
			if (entry == null)
			{
				return;
			}
			entry.Display = display;
		}

		// Token: 0x04000013 RID: 19
		[SerializeField]
		private List<CustomData> entries = new List<CustomData>();

		// Token: 0x04000014 RID: 20
		private bool dirty = true;

		// Token: 0x04000015 RID: 21
		private Dictionary<int, CustomData> _dictionary;
	}
}
