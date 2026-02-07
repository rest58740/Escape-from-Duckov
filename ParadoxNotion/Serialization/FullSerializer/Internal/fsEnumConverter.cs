using System;
using System.Collections.Generic;
using System.Text;

namespace ParadoxNotion.Serialization.FullSerializer.Internal
{
	// Token: 0x020000B5 RID: 181
	public class fsEnumConverter : fsConverter
	{
		// Token: 0x060006C5 RID: 1733 RVA: 0x00014B38 File Offset: 0x00012D38
		public override bool CanProcess(Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00014B40 File Offset: 0x00012D40
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00014B43 File Offset: 0x00012D43
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00014B46 File Offset: 0x00012D46
		public override object CreateInstance(fsData data, Type storageType)
		{
			return Enum.ToObject(storageType, 0);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00014B54 File Offset: 0x00012D54
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			if (fsGlobalConfig.SerializeEnumsAsInteger)
			{
				serialized = new fsData(Convert.ToInt64(instance));
			}
			else if (storageType.RTIsDefined(false))
			{
				long num = Convert.ToInt64(instance);
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				foreach (object obj in Enum.GetValues(storageType))
				{
					long num2 = Convert.ToInt64(obj);
					if ((num & num2) == num2)
					{
						if (!flag)
						{
							stringBuilder.Append(",");
						}
						flag = false;
						stringBuilder.Append(obj.ToString());
					}
				}
				serialized = new fsData(stringBuilder.ToString());
			}
			else
			{
				serialized = new fsData(Enum.GetName(storageType, instance));
			}
			return fsResult.Success;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00014C30 File Offset: 0x00012E30
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (data.IsString)
			{
				string[] array = data.AsString.Split(new char[]
				{
					','
				}, 1);
				long num = 0L;
				foreach (string text in array)
				{
					if (!fsEnumConverter.ArrayContains<string>(Enum.GetNames(storageType), text))
					{
						return fsResult.Fail("Cannot find enum name " + text + " on type " + ((storageType != null) ? storageType.ToString() : null));
					}
					long num2 = (long)Convert.ChangeType(Enum.Parse(storageType, text), typeof(long));
					num |= num2;
				}
				instance = Enum.ToObject(storageType, num);
				return fsResult.Success;
			}
			if (data.IsInt64)
			{
				int num3 = (int)data.AsInt64;
				instance = Enum.ToObject(storageType, num3);
				return fsResult.Success;
			}
			return fsResult.Fail("EnumConverter encountered an unknown JSON data type");
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00014D0C File Offset: 0x00012F0C
		private static bool ArrayContains<T>(T[] values, T value)
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (EqualityComparer<T>.Default.Equals(values[i], value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
