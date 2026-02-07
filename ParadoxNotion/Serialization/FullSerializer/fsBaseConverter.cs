using System;
using System.Collections.Generic;
using System.Linq;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000A0 RID: 160
	public abstract class fsBaseConverter
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x00011240 File Offset: 0x0000F440
		public virtual object CreateInstance(fsData data, Type storageType)
		{
			if (this.RequestCycleSupport(storageType))
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"Please override CreateInstance for ",
					base.GetType().FullName,
					"; the object graph for ",
					(storageType != null) ? storageType.ToString() : null,
					" can contain potentially contain cycles, so separated instance creation is needed"
				}));
			}
			return storageType;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001129E File Offset: 0x0000F49E
		public virtual bool RequestCycleSupport(Type storageType)
		{
			return !(storageType == typeof(string)) && (storageType.IsClass || storageType.IsInterface);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000112C4 File Offset: 0x0000F4C4
		public virtual bool RequestInheritanceSupport(Type storageType)
		{
			return !storageType.IsSealed;
		}

		// Token: 0x060005F7 RID: 1527
		public abstract fsResult TrySerialize(object instance, out fsData serialized, Type storageType);

		// Token: 0x060005F8 RID: 1528
		public abstract fsResult TryDeserialize(fsData data, ref object instance, Type storageType);

		// Token: 0x060005F9 RID: 1529 RVA: 0x000112D0 File Offset: 0x0000F4D0
		protected fsResult FailExpectedType(fsData data, params fsDataType[] types)
		{
			string[] array = new string[7];
			array[0] = base.GetType().Name;
			array[1] = " expected one of ";
			array[2] = string.Join(", ", (from t in types
			select t.ToString()).ToArray<string>());
			array[3] = " but got ";
			array[4] = data.Type.ToString();
			array[5] = " in ";
			array[6] = ((data != null) ? data.ToString() : null);
			return fsResult.Fail(string.Concat(array));
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00011374 File Offset: 0x0000F574
		protected fsResult CheckType(fsData data, fsDataType type)
		{
			if (data.Type != type)
			{
				return fsResult.Fail(string.Concat(new string[]
				{
					base.GetType().Name,
					" expected ",
					type.ToString(),
					" but got ",
					data.Type.ToString(),
					" in ",
					(data != null) ? data.ToString() : null
				}));
			}
			return fsResult.Success;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000113FD File Offset: 0x0000F5FD
		protected fsResult CheckKey(fsData data, string key, out fsData subitem)
		{
			return this.CheckKey(data.AsDictionary, key, out subitem);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00011410 File Offset: 0x0000F610
		protected fsResult CheckKey(Dictionary<string, fsData> data, string key, out fsData subitem)
		{
			if (!data.TryGetValue(key, ref subitem))
			{
				return fsResult.Fail(string.Concat(new string[]
				{
					base.GetType().Name,
					" requires a <",
					key,
					"> key in the data ",
					(data != null) ? data.ToString() : null
				}));
			}
			return fsResult.Success;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00011470 File Offset: 0x0000F670
		protected fsResult SerializeMember<T>(Dictionary<string, fsData> data, Type overrideConverterType, string name, T value)
		{
			fsData fsData;
			fsResult result = this.Serializer.TrySerialize(typeof(T), value, out fsData, overrideConverterType);
			if (result.Succeeded)
			{
				data[name] = fsData;
			}
			return result;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000114B0 File Offset: 0x0000F6B0
		protected fsResult DeserializeMember<T>(Dictionary<string, fsData> data, Type overrideConverterType, string name, out T value)
		{
			fsData data2;
			if (!data.TryGetValue(name, ref data2))
			{
				value = default(T);
				return fsResult.Fail("Unable to find member \"" + name + "\"");
			}
			object obj = null;
			fsResult result = this.Serializer.TryDeserialize(data2, typeof(T), ref obj, overrideConverterType);
			value = (T)((object)obj);
			return result;
		}

		// Token: 0x040001DA RID: 474
		public fsSerializer Serializer;
	}
}
