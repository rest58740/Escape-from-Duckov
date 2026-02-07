using System;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000005 RID: 5
	public abstract class SerializedMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver, ISupportsPrefabSerialization
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002218 File Offset: 0x00000418
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002220 File Offset: 0x00000420
		SerializationData ISupportsPrefabSerialization.SerializationData
		{
			get
			{
				return this.serializationData;
			}
			set
			{
				this.serializationData = value;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002229 File Offset: 0x00000429
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData, null);
			this.OnAfterDeserialize();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002247 File Offset: 0x00000447
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			this.OnBeforeSerialize();
			UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData, false, null);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnAfterDeserialize()
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x04000005 RID: 5
		[SerializeField]
		[HideInInspector]
		private SerializationData serializationData;
	}
}
