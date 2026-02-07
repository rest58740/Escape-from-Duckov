using System;
using System.Collections.Generic;
using ES3Internal;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000019 RID: 25
	public class ES3Type_ES3PrefabInternal : ES3Type
	{
		// Token: 0x060001DB RID: 475 RVA: 0x00006D5B File Offset: 0x00004F5B
		public ES3Type_ES3PrefabInternal() : base(typeof(ES3Type_ES3PrefabInternal))
		{
			ES3Type_ES3PrefabInternal.Instance = this;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006D74 File Offset: 0x00004F74
		public override void Write(object obj, ES3Writer writer)
		{
			ES3Prefab es3Prefab = (ES3Prefab)obj;
			writer.WriteProperty("prefabId", es3Prefab.prefabId.ToString(), ES3Type_string.Instance);
			writer.WriteProperty("refs", es3Prefab.GetReferences());
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006DB4 File Offset: 0x00004FB4
		public override object Read<T>(ES3Reader reader)
		{
			long id = reader.ReadRefProperty();
			if (ES3ReferenceMgrBase.Current == null)
			{
				return null;
			}
			ES3Prefab prefab = ES3ReferenceMgrBase.Current.GetPrefab(id, false);
			if (prefab == null)
			{
				throw new MissingReferenceException("Prefab with ID " + id.ToString() + " could not be found.\nPress the 'Refresh References' button on the ES3ReferenceMgr Component of the Easy Save 3 Manager in the scene to refresh prefabs.");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab.gameObject);
			ES3Prefab component = gameObject.GetComponent<ES3Prefab>();
			if (component == null)
			{
				throw new MissingReferenceException("Prefab with ID " + id.ToString() + " was found, but it does not have an ES3Prefab component attached.");
			}
			this.ReadInto<T>(reader, gameObject);
			return component.gameObject;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006E4C File Offset: 0x0000504C
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			Dictionary<ES3Ref, ES3Ref> dictionary = reader.ReadProperty<Dictionary<ES3Ref, ES3Ref>>(ES3Type_ES3RefDictionary.Instance);
			Dictionary<long, long> dictionary2 = new Dictionary<long, long>();
			foreach (KeyValuePair<ES3Ref, ES3Ref> keyValuePair in dictionary)
			{
				dictionary2.Add(keyValuePair.Key.id, keyValuePair.Value.id);
			}
			if (ES3ReferenceMgrBase.Current == null)
			{
				return;
			}
			((GameObject)obj).GetComponent<ES3Prefab>().ApplyReferences(dictionary2);
		}

		// Token: 0x04000053 RID: 83
		public static ES3Type Instance = new ES3Type_ES3PrefabInternal();
	}
}
