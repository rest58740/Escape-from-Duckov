using System;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000008 RID: 8
	public abstract class SerializedUnityObject : Object, ISerializationCallbackReceiver
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000022F8 File Offset: 0x000004F8
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData, null);
			this.OnAfterDeserialize();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002316 File Offset: 0x00000516
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			this.OnBeforeSerialize();
			UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData, false, null);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnAfterDeserialize()
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x04000008 RID: 8
		[SerializeField]
		[HideInInspector]
		private SerializationData serializationData;
	}
}
