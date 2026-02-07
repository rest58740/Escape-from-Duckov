using System;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000004 RID: 4
	public abstract class SerializedComponent : Component, ISerializationCallbackReceiver, ISupportsPrefabSerialization
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021C2 File Offset: 0x000003C2
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000021CA File Offset: 0x000003CA
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

		// Token: 0x06000010 RID: 16 RVA: 0x000021D3 File Offset: 0x000003D3
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData, null);
			this.OnAfterDeserialize();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021F1 File Offset: 0x000003F1
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.SafeIsUnityNull())
			{
				return;
			}
			this.OnBeforeSerialize();
			UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData, false, null);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnAfterDeserialize()
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021B8 File Offset: 0x000003B8
		protected virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x04000004 RID: 4
		[SerializeField]
		[HideInInspector]
		private SerializationData serializationData;
	}
}
