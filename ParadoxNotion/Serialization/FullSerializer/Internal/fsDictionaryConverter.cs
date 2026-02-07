using System;
using System.Collections;
using System.Collections.Generic;

namespace ParadoxNotion.Serialization.FullSerializer.Internal
{
	// Token: 0x020000B4 RID: 180
	public class fsDictionaryConverter : fsConverter
	{
		// Token: 0x060006C0 RID: 1728 RVA: 0x00014746 File Offset: 0x00012946
		public override bool CanProcess(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00014767 File Offset: 0x00012967
		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(storageType).CreateInstance();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00014774 File Offset: 0x00012974
		public override fsResult TrySerialize(object instance_, out fsData serialized, Type storageType)
		{
			serialized = fsData.Null;
			fsResult fsResult = fsResult.Success;
			IDictionary dictionary = (IDictionary)instance_;
			Type[] array = dictionary.GetType().RTGetGenericArguments();
			Type storageType2 = array[0];
			Type storageType3 = array[1];
			bool flag = true;
			List<fsData> list = new List<fsData>(dictionary.Count);
			List<fsData> list2 = new List<fsData>(dictionary.Count);
			IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				fsData fsData;
				fsResult fsResult2;
				fsResult = (fsResult2 = fsResult + this.Serializer.TrySerialize(storageType2, enumerator.Key, out fsData));
				if (fsResult2.Failed)
				{
					return fsResult;
				}
				fsData fsData2;
				fsResult = (fsResult2 = fsResult + this.Serializer.TrySerialize(storageType3, enumerator.Value, out fsData2));
				if (fsResult2.Failed)
				{
					return fsResult;
				}
				list.Add(fsData);
				list2.Add(fsData2);
				flag &= fsData.IsString;
			}
			if (flag)
			{
				serialized = fsData.CreateDictionary();
				Dictionary<string, fsData> asDictionary = serialized.AsDictionary;
				for (int i = 0; i < list.Count; i++)
				{
					fsData fsData3 = list[i];
					fsData fsData4 = list2[i];
					asDictionary[fsData3.AsString] = fsData4;
				}
			}
			else
			{
				serialized = fsData.CreateList(list.Count);
				List<fsData> asList = serialized.AsList;
				for (int j = 0; j < list.Count; j++)
				{
					fsData fsData5 = list[j];
					fsData fsData6 = list2[j];
					Dictionary<string, fsData> dictionary2 = new Dictionary<string, fsData>();
					dictionary2["Key"] = fsData5;
					dictionary2["Value"] = fsData6;
					asList.Add(new fsData(dictionary2));
				}
			}
			return fsResult;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001490C File Offset: 0x00012B0C
		public override fsResult TryDeserialize(fsData data, ref object instance_, Type storageType)
		{
			IDictionary dictionary = (IDictionary)instance_;
			fsResult fsResult = fsResult.Success;
			Type[] array = dictionary.GetType().RTGetGenericArguments();
			Type storageType2 = array[0];
			Type storageType3 = array[1];
			dictionary.Clear();
			if (data.IsDictionary)
			{
				foreach (KeyValuePair<string, fsData> keyValuePair in data.AsDictionary)
				{
					if (!fsSerializer.IsReservedKeyword(keyValuePair.Key))
					{
						fsData data2 = new fsData(keyValuePair.Key);
						fsData value = keyValuePair.Value;
						object obj = null;
						object obj2 = null;
						fsResult fsResult2;
						fsResult = (fsResult2 = fsResult + this.Serializer.TryDeserialize(data2, storageType2, ref obj));
						if (fsResult2.Failed)
						{
							return fsResult;
						}
						fsResult fsResult3;
						fsResult = (fsResult3 = fsResult + this.Serializer.TryDeserialize(value, storageType3, ref obj2));
						if (fsResult3.Failed)
						{
							return fsResult;
						}
						dictionary.Add(obj, obj2);
					}
				}
				return fsResult;
			}
			if (data.IsList)
			{
				List<fsData> asList = data.AsList;
				for (int i = 0; i < asList.Count; i++)
				{
					fsData data3 = asList[i];
					fsResult fsResult3;
					fsResult = (fsResult3 = fsResult + base.CheckType(data3, fsDataType.Object));
					if (fsResult3.Failed)
					{
						return fsResult;
					}
					fsData data4;
					fsResult = (fsResult3 = fsResult + base.CheckKey(data3, "Key", out data4));
					if (fsResult3.Failed)
					{
						return fsResult;
					}
					fsData data5;
					fsResult = (fsResult3 = fsResult + base.CheckKey(data3, "Value", out data5));
					if (fsResult3.Failed)
					{
						return fsResult;
					}
					object obj3 = null;
					object obj4 = null;
					fsResult = (fsResult3 = fsResult + this.Serializer.TryDeserialize(data4, storageType2, ref obj3));
					if (fsResult3.Failed)
					{
						return fsResult;
					}
					fsResult = (fsResult3 = fsResult + this.Serializer.TryDeserialize(data5, storageType3, ref obj4));
					if (fsResult3.Failed)
					{
						return fsResult;
					}
					dictionary.Add(obj3, obj4);
				}
				return fsResult;
			}
			return base.FailExpectedType(data, new fsDataType[]
			{
				fsDataType.Array,
				fsDataType.Object
			});
		}
	}
}
