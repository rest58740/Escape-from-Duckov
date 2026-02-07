using System;
using System.Collections;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal
{
	// Token: 0x020000B9 RID: 185
	public class fsReflectedConverter : fsConverter
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x0001559E File Offset: 0x0001379E
		public override bool CanProcess(Type type)
		{
			return !type.IsArray && !typeof(ICollection).IsAssignableFrom(type);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000155C0 File Offset: 0x000137C0
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			serialized = fsData.CreateDictionary();
			fsResult success = fsResult.Success;
			fsMetaType fsMetaType = fsMetaType.Get(instance.GetType());
			object obj = null;
			if (!fsGlobalConfig.SerializeDefaultValues && !(instance is Object))
			{
				obj = fsMetaType.GetDefaultInstance();
			}
			for (int i = 0; i < fsMetaType.Properties.Length; i++)
			{
				fsMetaProperty fsMetaProperty = fsMetaType.Properties[i];
				if (!fsMetaProperty.WriteOnly && (!fsMetaProperty.AsReference || !this.Serializer.IgnoreSerializeCycleReferences))
				{
					object obj2 = fsMetaProperty.Read(instance);
					if (obj2 == null && fsMetaProperty.AutoInstance)
					{
						obj2 = fsMetaType.Get(fsMetaProperty.StorageType).CreateInstance();
						fsMetaProperty.Write(instance, obj2);
					}
					else if (!fsGlobalConfig.SerializeDefaultValues && obj != null && object.Equals(obj2, fsMetaProperty.Read(obj)))
					{
						goto IL_F7;
					}
					fsData fsData;
					fsResult result = this.Serializer.TrySerialize(fsMetaProperty.StorageType, obj2, out fsData);
					success.AddMessages(result);
					if (!result.Failed)
					{
						serialized.AsDictionary[fsMetaProperty.JsonName] = fsData;
					}
				}
				IL_F7:;
			}
			return success;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000156D8 File Offset: 0x000138D8
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Object));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			if (data.AsDictionary.Count == 0)
			{
				return fsResult.Success;
			}
			fsMetaType fsMetaType = fsMetaType.Get(storageType);
			for (int i = 0; i < fsMetaType.Properties.Length; i++)
			{
				fsMetaProperty fsMetaProperty = fsMetaType.Properties[i];
				fsData data2;
				if (!fsMetaProperty.ReadOnly && data.AsDictionary.TryGetValue(fsMetaProperty.JsonName, ref data2))
				{
					object value = null;
					if (fsGlobalConfig.SerializeDefaultValues && (fsMetaType.DeserializeOverwriteRequest || typeof(ICollection).IsAssignableFrom(storageType)))
					{
						value = fsMetaProperty.Read(instance);
					}
					fsResult result = this.Serializer.TryDeserialize(data2, fsMetaProperty.StorageType, ref value, null);
					fsResult.AddMessages(result);
					if (!result.Failed)
					{
						fsMetaProperty.Write(instance, value);
					}
				}
			}
			return fsResult;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x000157C9 File Offset: 0x000139C9
		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(storageType).CreateInstance();
		}
	}
}
