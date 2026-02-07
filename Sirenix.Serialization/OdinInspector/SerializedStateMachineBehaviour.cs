using System;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000007 RID: 7
	public abstract class SerializedStateMachineBehaviour : StateMachineBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x06000021 RID: 33 RVA: 0x000022B3 File Offset: 0x000004B3
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData, null);
			this.OnAfterDeserialize();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000022D1 File Offset: 0x000004D1
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			this.OnBeforeSerialize();
			UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData, false, null);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnAfterDeserialize()
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x04000007 RID: 7
		[SerializeField]
		[HideInInspector]
		private SerializationData serializationData;
	}
}
