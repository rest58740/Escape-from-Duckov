using System;

namespace ParadoxNotion.Serialization.FullSerializer.Internal
{
	// Token: 0x020000BA RID: 186
	public class fsTypeConverter : fsConverter
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x000157DE File Offset: 0x000139DE
		public override bool CanProcess(Type type)
		{
			return typeof(Type).IsAssignableFrom(type);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x000157F0 File Offset: 0x000139F0
		public override bool RequestCycleSupport(Type type)
		{
			return false;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000157F3 File Offset: 0x000139F3
		public override bool RequestInheritanceSupport(Type type)
		{
			return false;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000157F8 File Offset: 0x000139F8
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			Type type = (Type)instance;
			serialized = new fsData(type.FullName);
			return fsResult.Success;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00015820 File Offset: 0x00013A20
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (!data.IsString)
			{
				return fsResult.Fail("Type converter requires a string");
			}
			instance = ReflectionTools.GetType(data.AsString, true, null);
			if (instance == null)
			{
				return fsResult.Fail("Unable to find type " + data.AsString);
			}
			return fsResult.Success;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001586E File Offset: 0x00013A6E
		public override object CreateInstance(fsData data, Type storageType)
		{
			return storageType;
		}
	}
}
