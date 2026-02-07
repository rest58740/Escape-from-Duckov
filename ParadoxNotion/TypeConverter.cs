using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000076 RID: 118
	public static class TypeConverter
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000485 RID: 1157 RVA: 0x0000C620 File Offset: 0x0000A820
		// (remove) Token: 0x06000486 RID: 1158 RVA: 0x0000C654 File Offset: 0x0000A854
		public static event TypeConverter.CustomConverter customConverter;

		// Token: 0x06000487 RID: 1159 RVA: 0x0000C688 File Offset: 0x0000A888
		public static Func<object, object> Get(Type fromType, Type toType)
		{
			if (TypeConverter.customConverter != null)
			{
				Func<object, object> func = TypeConverter.customConverter(fromType, toType);
				if (func != null)
				{
					return func;
				}
			}
			if (toType.RTIsAssignableFrom(fromType))
			{
				return (object value) => value;
			}
			if (toType == typeof(string))
			{
				return delegate(object value)
				{
					if (value == null)
					{
						return "NULL";
					}
					return value.ToString();
				};
			}
			if (typeof(IConvertible).RTIsAssignableFrom(toType) && typeof(IConvertible).RTIsAssignableFrom(fromType))
			{
				return delegate(object value)
				{
					object result;
					try
					{
						result = Convert.ChangeType(value, toType);
					}
					catch
					{
						result = ((!toType.RTIsAbstract()) ? Activator.CreateInstance(toType) : null);
					}
					return result;
				};
			}
			if (typeof(Object).RTIsAssignableFrom(fromType) && toType == typeof(bool))
			{
				return (object value) => value != null;
			}
			if (fromType == typeof(GameObject) && typeof(Component).RTIsAssignableFrom(toType))
			{
				return delegate(object value)
				{
					if (!(value as GameObject != null))
					{
						return null;
					}
					return (value as GameObject).GetComponent(toType);
				};
			}
			if (typeof(Component).RTIsAssignableFrom(fromType) && toType == typeof(GameObject))
			{
				return delegate(object value)
				{
					if (!(value as Component != null))
					{
						return null;
					}
					return (value as Component).gameObject;
				};
			}
			if (typeof(Component).RTIsAssignableFrom(fromType) && typeof(Component).RTIsAssignableFrom(toType))
			{
				return delegate(object value)
				{
					if (!(value as Component != null))
					{
						return null;
					}
					return (value as Component).gameObject.GetComponent(toType);
				};
			}
			if (fromType == typeof(GameObject) && toType.RTIsInterface())
			{
				return delegate(object value)
				{
					if (!(value as GameObject != null))
					{
						return null;
					}
					return (value as GameObject).GetComponent(toType);
				};
			}
			if (typeof(Component).RTIsAssignableFrom(fromType) && toType.RTIsInterface())
			{
				return delegate(object value)
				{
					if (!(value as Component != null))
					{
						return null;
					}
					return (value as Component).gameObject.GetComponent(toType);
				};
			}
			if (fromType == typeof(GameObject) && toType == typeof(Vector3))
			{
				return (object value) => (value as GameObject != null) ? (value as GameObject).transform.position : Vector3.zero;
			}
			if (typeof(Component).RTIsAssignableFrom(fromType) && toType == typeof(Vector3))
			{
				return (object value) => (value as Component != null) ? (value as Component).transform.position : Vector3.zero;
			}
			if (fromType == typeof(GameObject) && toType == typeof(Quaternion))
			{
				return (object value) => (value as GameObject != null) ? (value as GameObject).transform.rotation : Quaternion.identity;
			}
			if (typeof(Component).RTIsAssignableFrom(fromType) && toType == typeof(Quaternion))
			{
				return (object value) => (value as Component != null) ? (value as Component).transform.rotation : Quaternion.identity;
			}
			if (fromType == typeof(Quaternion) && toType == typeof(Vector3))
			{
				return (object value) => ((Quaternion)value).eulerAngles;
			}
			if (fromType == typeof(Vector3) && toType == typeof(Quaternion))
			{
				return (object value) => Quaternion.Euler((Vector3)value);
			}
			if (fromType == typeof(Vector2) && toType == typeof(Vector3))
			{
				return (object value) => (Vector2)value;
			}
			if (fromType == typeof(Vector3) && toType == typeof(Vector2))
			{
				return (object value) => (Vector3)value;
			}
			return null;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000CAFA File Offset: 0x0000ACFA
		public static bool CanConvert(Type fromType, Type toType)
		{
			return TypeConverter.Get(fromType, toType) != null;
		}

		// Token: 0x0200011D RID: 285
		// (Invoke) Token: 0x06000810 RID: 2064
		public delegate Func<object, object> CustomConverter(Type fromType, Type toType);
	}
}
