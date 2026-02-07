using System;
using System.Collections;
using System.Collections.Generic;

namespace ParadoxNotion.Serialization.FullSerializer.Internal
{
	// Token: 0x020000B3 RID: 179
	public class fsArrayConverter : fsConverter
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x000145D0 File Offset: 0x000127D0
		public override bool CanProcess(Type type)
		{
			return type.IsArray;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000145D8 File Offset: 0x000127D8
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000145DB File Offset: 0x000127DB
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000145E0 File Offset: 0x000127E0
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			IList list = (Array)instance;
			Type elementType = storageType.GetElementType();
			fsResult success = fsResult.Success;
			serialized = fsData.CreateList(list.Count);
			List<fsData> asList = serialized.AsList;
			for (int i = 0; i < list.Count; i++)
			{
				object instance2 = list[i];
				fsData fsData;
				fsResult result = this.Serializer.TrySerialize(elementType, instance2, out fsData);
				success.AddMessages(result);
				if (!result.Failed)
				{
					asList.Add(fsData);
				}
			}
			return success;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00014664 File Offset: 0x00012864
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Array));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			Type elementType = storageType.GetElementType();
			List<fsData> asList = data.AsList;
			ArrayList arrayList = new ArrayList(asList.Count);
			int count = arrayList.Count;
			for (int i = 0; i < asList.Count; i++)
			{
				fsData data2 = asList[i];
				object obj = null;
				if (i < count)
				{
					obj = arrayList[i];
				}
				fsResult result = this.Serializer.TryDeserialize(data2, elementType, ref obj);
				fsResult.AddMessages(result);
				if (!result.Failed)
				{
					if (i < count)
					{
						arrayList[i] = obj;
					}
					else
					{
						arrayList.Add(obj);
					}
				}
			}
			instance = arrayList.ToArray(elementType);
			return fsResult;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00014731 File Offset: 0x00012931
		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(storageType).CreateInstance();
		}
	}
}
