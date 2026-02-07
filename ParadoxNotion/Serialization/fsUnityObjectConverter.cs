using System;
using System.Collections.Generic;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x02000087 RID: 135
	public class fsUnityObjectConverter : fsConverter
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x0001021C File Offset: 0x0000E41C
		public override bool CanProcess(Type type)
		{
			return typeof(Object).RTIsAssignableFrom(type);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001022E File Offset: 0x0000E42E
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00010231 File Offset: 0x0000E431
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00010234 File Offset: 0x0000E434
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			List<Object> referencesDatabase = this.Serializer.ReferencesDatabase;
			if (referencesDatabase == null)
			{
				serialized = new fsData();
				return fsResult.Success;
			}
			Object @object = instance as Object;
			if (@object == null)
			{
				serialized = new fsData(0L);
				return fsResult.Success;
			}
			if (referencesDatabase.Count == 0)
			{
				referencesDatabase.Add(null);
			}
			int num = -1;
			for (int i = 0; i < referencesDatabase.Count; i++)
			{
				if (referencesDatabase[i] == @object)
				{
					num = i;
					break;
				}
			}
			if (num <= 0)
			{
				num = referencesDatabase.Count;
				referencesDatabase.Add(@object);
			}
			serialized = new fsData((long)num);
			return fsResult.Success;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x000102C8 File Offset: 0x0000E4C8
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			List<Object> referencesDatabase = this.Serializer.ReferencesDatabase;
			if (referencesDatabase == null)
			{
				return fsResult.Warn("A Unity Object reference has not been deserialized because no database references was provided.");
			}
			int num = (int)data.AsInt64;
			if (num >= referencesDatabase.Count)
			{
				return fsResult.Warn("A Unity Object reference has not been deserialized because no database entry was found in provided database references.");
			}
			Object @object = referencesDatabase[num];
			if (@object == null || storageType.RTIsAssignableFrom(@object.GetType()))
			{
				instance = @object;
			}
			return fsResult.Success;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001032C File Offset: 0x0000E52C
		public override object CreateInstance(fsData data, Type storageType)
		{
			return null;
		}
	}
}
