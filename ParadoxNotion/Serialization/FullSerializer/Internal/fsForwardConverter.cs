using System;

namespace ParadoxNotion.Serialization.FullSerializer.Internal
{
	// Token: 0x020000B6 RID: 182
	public class fsForwardConverter : fsConverter
	{
		// Token: 0x060006CD RID: 1741 RVA: 0x00014D46 File Offset: 0x00012F46
		public fsForwardConverter(fsForwardAttribute attribute)
		{
			this._memberName = attribute.MemberName;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00014D5A File Offset: 0x00012F5A
		public override bool CanProcess(Type type)
		{
			throw new NotSupportedException("Please use the [fsForward(...)] attribute.");
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00014D68 File Offset: 0x00012F68
		private fsResult GetProperty(object instance, out fsMetaProperty property)
		{
			fsMetaProperty[] properties = fsMetaType.Get(instance.GetType()).Properties;
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].MemberName == this._memberName)
				{
					property = properties[i];
					return fsResult.Success;
				}
			}
			property = null;
			return fsResult.Fail("No property named \"" + this._memberName + "\" on " + instance.GetType().FriendlyName(false));
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00014DE0 File Offset: 0x00012FE0
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			serialized = fsData.Null;
			fsResult fsResult = fsResult.Success;
			fsMetaProperty fsMetaProperty;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + this.GetProperty(instance, out fsMetaProperty));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			object instance2 = fsMetaProperty.Read(instance);
			return this.Serializer.TrySerialize(fsMetaProperty.StorageType, instance2, out serialized);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00014E34 File Offset: 0x00013034
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsMetaProperty fsMetaProperty;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + this.GetProperty(instance, out fsMetaProperty));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			object value = null;
			fsResult = (fsResult2 = fsResult + this.Serializer.TryDeserialize(data, fsMetaProperty.StorageType, ref value));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			fsMetaProperty.Write(instance, value);
			return fsResult;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00014E99 File Offset: 0x00013099
		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(storageType).CreateInstance();
		}

		// Token: 0x04000218 RID: 536
		private string _memberName;
	}
}
