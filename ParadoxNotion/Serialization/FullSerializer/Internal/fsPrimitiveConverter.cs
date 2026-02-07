using System;

namespace ParadoxNotion.Serialization.FullSerializer.Internal
{
	// Token: 0x020000B8 RID: 184
	public class fsPrimitiveConverter : fsConverter
	{
		// Token: 0x060006D8 RID: 1752 RVA: 0x000150CE File Offset: 0x000132CE
		public override bool CanProcess(Type type)
		{
			return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000150FC File Offset: 0x000132FC
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000150FF File Offset: 0x000132FF
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00015102 File Offset: 0x00013302
		private static bool UseBool(Type type)
		{
			return type == typeof(bool);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00015114 File Offset: 0x00013314
		private static bool UseInt64(Type type)
		{
			return type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000151B1 File Offset: 0x000133B1
		private static bool UseDouble(Type type)
		{
			return type == typeof(float) || type == typeof(double) || type == typeof(decimal);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x000151E9 File Offset: 0x000133E9
		private static bool UseString(Type type)
		{
			return type == typeof(string) || type == typeof(char);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00015210 File Offset: 0x00013410
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			Type type = instance.GetType();
			if (fsGlobalConfig.Serialize64BitIntegerAsString && (type == typeof(long) || type == typeof(ulong)))
			{
				serialized = new fsData((string)Convert.ChangeType(instance, typeof(string)));
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseBool(type))
			{
				serialized = new fsData((bool)instance);
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseInt64(type))
			{
				serialized = new fsData((long)Convert.ChangeType(instance, typeof(long)));
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseDouble(type))
			{
				if (instance.GetType() == typeof(float) && (float)instance != -3.4028235E+38f && (float)instance != 3.4028235E+38f && !float.IsInfinity((float)instance) && !float.IsNaN((float)instance))
				{
					serialized = new fsData((double)((decimal)((float)instance)));
					return fsResult.Success;
				}
				serialized = new fsData((double)Convert.ChangeType(instance, typeof(double)));
				return fsResult.Success;
			}
			else
			{
				if (fsPrimitiveConverter.UseString(type))
				{
					serialized = new fsData((string)Convert.ChangeType(instance, typeof(string)));
					return fsResult.Success;
				}
				serialized = null;
				string text = "Unhandled primitive type ";
				Type type2 = instance.GetType();
				return fsResult.Fail(text + ((type2 != null) ? type2.ToString() : null));
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001539C File Offset: 0x0001359C
		public override fsResult TryDeserialize(fsData storage, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			if (fsPrimitiveConverter.UseBool(storageType))
			{
				fsResult fsResult2;
				fsResult = (fsResult2 = fsResult + base.CheckType(storage, fsDataType.Boolean));
				if (fsResult2.Succeeded)
				{
					instance = storage.AsBool;
				}
				return fsResult;
			}
			if (fsPrimitiveConverter.UseDouble(storageType) || fsPrimitiveConverter.UseInt64(storageType))
			{
				if (storage.IsDouble)
				{
					if (storageType == typeof(float))
					{
						instance = (float)storage.AsDouble;
					}
					else
					{
						instance = Convert.ChangeType(storage.AsDouble, storageType);
					}
				}
				else if (storage.IsInt64)
				{
					if (storageType == typeof(int))
					{
						instance = (int)storage.AsInt64;
					}
					else
					{
						instance = Convert.ChangeType(storage.AsInt64, storageType);
					}
				}
				else
				{
					if (!fsGlobalConfig.Serialize64BitIntegerAsString || !storage.IsString || (!(storageType == typeof(long)) && !(storageType == typeof(ulong))))
					{
						return fsResult.Fail(string.Concat(new string[]
						{
							base.GetType().Name,
							" expected number but got ",
							storage.Type.ToString(),
							" in ",
							(storage != null) ? storage.ToString() : null
						}));
					}
					instance = Convert.ChangeType(storage.AsString, storageType);
				}
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseString(storageType))
			{
				fsResult fsResult2;
				fsResult = (fsResult2 = fsResult + base.CheckType(storage, fsDataType.String));
				if (fsResult2.Succeeded)
				{
					if (storageType == typeof(char))
					{
						instance = storage.AsString.get_Chars(0);
					}
					else
					{
						instance = storage.AsString;
					}
				}
				return fsResult;
			}
			return fsResult.Fail(base.GetType().Name + ": Bad data; expected bool, number, string, but got " + ((storage != null) ? storage.ToString() : null));
		}
	}
}
