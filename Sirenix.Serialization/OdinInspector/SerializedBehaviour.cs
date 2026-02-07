using System;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000003 RID: 3
	public abstract class SerializedBehaviour : Behaviour, ISerializationCallbackReceiver, ISupportsPrefabSerialization
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000216A File Offset: 0x0000036A
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002172 File Offset: 0x00000372
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

		// Token: 0x06000009 RID: 9 RVA: 0x0000217B File Offset: 0x0000037B
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData, null);
			this.OnAfterDeserialize();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002199 File Offset: 0x00000399
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			this.OnBeforeSerialize();
			UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData, false, null);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnAfterDeserialize()
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x04000003 RID: 3
		[SerializeField]
		[HideInInspector]
		private SerializationData serializationData;
	}
}
