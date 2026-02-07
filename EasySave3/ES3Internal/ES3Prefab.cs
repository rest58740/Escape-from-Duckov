using System;
using System.Collections.Generic;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000D4 RID: 212
	public class ES3Prefab : MonoBehaviour
	{
		// Token: 0x0600042E RID: 1070 RVA: 0x0001AED8 File Offset: 0x000190D8
		public void Awake()
		{
			ES3ReferenceMgrBase es3ReferenceMgrBase = ES3ReferenceMgrBase.Current;
			if (es3ReferenceMgrBase == null)
			{
				return;
			}
			foreach (KeyValuePair<UnityEngine.Object, long> keyValuePair in this.localRefs)
			{
				if (keyValuePair.Key != null)
				{
					es3ReferenceMgrBase.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001AF54 File Offset: 0x00019154
		public long Get(UnityEngine.Object obj)
		{
			long result;
			if (this.localRefs.TryGetValue(obj, out result))
			{
				return result;
			}
			return -1L;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001AF78 File Offset: 0x00019178
		public long Add(UnityEngine.Object obj)
		{
			long newRefID;
			if (this.localRefs.TryGetValue(obj, out newRefID))
			{
				return newRefID;
			}
			if (!ES3ReferenceMgrBase.CanBeSaved(obj))
			{
				return -1L;
			}
			newRefID = ES3Prefab.GetNewRefID();
			this.localRefs.Add(obj, newRefID);
			return newRefID;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001AFB8 File Offset: 0x000191B8
		public Dictionary<string, string> GetReferences()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			ES3ReferenceMgrBase es3ReferenceMgrBase = ES3ReferenceMgrBase.Current;
			if (es3ReferenceMgrBase == null)
			{
				return dictionary;
			}
			foreach (KeyValuePair<UnityEngine.Object, long> keyValuePair in this.localRefs)
			{
				long num = es3ReferenceMgrBase.Add(keyValuePair.Key);
				dictionary[keyValuePair.Value.ToString()] = num.ToString();
			}
			return dictionary;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001B048 File Offset: 0x00019248
		public void ApplyReferences(Dictionary<long, long> localToGlobal)
		{
			if (ES3ReferenceMgrBase.Current == null)
			{
				return;
			}
			foreach (KeyValuePair<UnityEngine.Object, long> keyValuePair in this.localRefs)
			{
				long id;
				if (localToGlobal.TryGetValue(keyValuePair.Value, out id))
				{
					ES3ReferenceMgrBase.Current.Add(keyValuePair.Key, id);
				}
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001B0C8 File Offset: 0x000192C8
		public static long GetNewRefID()
		{
			return ES3ReferenceMgrBase.GetNewRefID();
		}

		// Token: 0x04000123 RID: 291
		public long prefabId = ES3Prefab.GetNewRefID();

		// Token: 0x04000124 RID: 292
		public ES3RefIdDictionary localRefs = new ES3RefIdDictionary();
	}
}
