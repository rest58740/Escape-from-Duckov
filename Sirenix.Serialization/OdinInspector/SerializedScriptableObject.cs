using System;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000006 RID: 6
	public abstract class SerializedScriptableObject : ScriptableObject, ISerializationCallbackReceiver
	{
		// Token: 0x0600001C RID: 28 RVA: 0x0000226E File Offset: 0x0000046E
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData, null);
			this.OnAfterDeserialize();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000228C File Offset: 0x0000048C
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			this.OnBeforeSerialize();
			UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData, false, null);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnAfterDeserialize()
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x04000006 RID: 6
		[SerializeField]
		[HideInInspector]
		private SerializationData serializationData;
	}
}
