using System;
using System.Collections;
using System.Collections.Generic;

namespace ParadoxNotion.Serialization.FullSerializer.Internal
{
	// Token: 0x020000B7 RID: 183
	public class fsListConverter : fsConverter
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x00014EA6 File Offset: 0x000130A6
		public override bool CanProcess(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00014EC7 File Offset: 0x000130C7
		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(storageType).CreateInstance();
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00014ED4 File Offset: 0x000130D4
		public override fsResult TrySerialize(object instance_, out fsData serialized, Type storageType)
		{
			IList list = (IList)instance_;
			fsResult success = fsResult.Success;
			Type type = storageType.RTGetGenericArguments()[0];
			serialized = fsData.CreateList(list.Count);
			List<fsData> asList = serialized.AsList;
			for (int i = 0; i < list.Count; i++)
			{
				object obj = list[i];
				if (obj == null && type.RTIsDefined(true))
				{
					obj = fsMetaType.Get(type).CreateInstance();
					list[i] = obj;
				}
				fsData fsData;
				fsResult result = this.Serializer.TrySerialize(type, obj, out fsData);
				success.AddMessages(result);
				if (!result.Failed)
				{
					asList.Add(fsData);
				}
			}
			return success;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00014F7C File Offset: 0x0001317C
		public override fsResult TryDeserialize(fsData data, ref object instance_, Type storageType)
		{
			IList list = (IList)instance_;
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Array));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			if (data.AsList.Count == 0)
			{
				return fsResult.Success;
			}
			Type type = storageType.RTGetGenericArguments()[0];
			if (list.Count == data.AsList.Count && fsMetaType.Get(type).DeserializeOverwriteRequest)
			{
				for (int i = 0; i < data.AsList.Count; i++)
				{
					object obj = list[i];
					if (!this.Serializer.TryDeserialize(data.AsList[i], type, ref obj).Failed)
					{
						list[i] = obj;
					}
				}
				return fsResult.Success;
			}
			list.Clear();
			list.GetType().RTGetProperty("Capacity").SetValue(list, data.AsList.Count);
			for (int j = 0; j < data.AsList.Count; j++)
			{
				object obj2 = null;
				if (!this.Serializer.TryDeserialize(data.AsList[j], type, ref obj2).Failed)
				{
					list.Add(obj2);
				}
			}
			return fsResult.Success;
		}
	}
}
