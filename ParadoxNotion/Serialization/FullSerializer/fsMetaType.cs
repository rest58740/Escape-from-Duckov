using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000AB RID: 171
	public class fsMetaType
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00013058 File Offset: 0x00011258
		public static fsMetaType Get(Type type)
		{
			fsMetaType fsMetaType;
			if (!fsMetaType._metaTypes.TryGetValue(type, ref fsMetaType))
			{
				fsMetaType = new fsMetaType(type);
				fsMetaType._metaTypes[type] = fsMetaType;
			}
			return fsMetaType;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00013088 File Offset: 0x00011288
		public static void FlushMem()
		{
			fsMetaType._metaTypes = new Dictionary<Type, fsMetaType>();
			fsMetaType._defaultInstances = new Dictionary<Type, object>();
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0001309E File Offset: 0x0001129E
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x000130A6 File Offset: 0x000112A6
		public Type reflectedType { get; private set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x000130AF File Offset: 0x000112AF
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x000130B7 File Offset: 0x000112B7
		public fsMetaProperty[] Properties { get; private set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x000130C0 File Offset: 0x000112C0
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x000130C8 File Offset: 0x000112C8
		public bool DeserializeOverwriteRequest { get; private set; }

		// Token: 0x06000671 RID: 1649 RVA: 0x000130D4 File Offset: 0x000112D4
		private fsMetaType(Type reflectedType)
		{
			this.reflectedType = reflectedType;
			this.generator = fsMetaType.GetGenerator(reflectedType);
			List<fsMetaProperty> list = new List<fsMetaProperty>();
			fsMetaType.CollectProperties(list, reflectedType);
			this.Properties = list.ToArray();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00013114 File Offset: 0x00011314
		private static void CollectProperties(List<fsMetaProperty> properties, Type reflectedType)
		{
			foreach (FieldInfo fieldInfo in reflectedType.RTGetFields())
			{
				if (!(fieldInfo.DeclaringType != reflectedType) && fsMetaType.CanSerializeField(fieldInfo))
				{
					properties.Add(new fsMetaProperty(fieldInfo));
				}
			}
			if (reflectedType.BaseType != null)
			{
				fsMetaType.CollectProperties(properties, reflectedType.BaseType);
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00013178 File Offset: 0x00011378
		public static bool CanSerializeField(FieldInfo field)
		{
			if (field.IsStatic)
			{
				return false;
			}
			if (typeof(Delegate).IsAssignableFrom(field.FieldType))
			{
				return false;
			}
			if (field.RTIsDefined(true))
			{
				return false;
			}
			if (field.RTIsDefined(true))
			{
				return false;
			}
			for (int i = 0; i < fsGlobalConfig.IgnoreSerializeAttributes.Length; i++)
			{
				if (field.RTIsDefined(fsGlobalConfig.IgnoreSerializeAttributes[i], true))
				{
					return false;
				}
			}
			if (field.IsPublic)
			{
				return true;
			}
			for (int j = 0; j < fsGlobalConfig.SerializeAttributes.Length; j++)
			{
				if (field.RTIsDefined(fsGlobalConfig.SerializeAttributes[j], true))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00013214 File Offset: 0x00011414
		private static fsMetaType.ObjectGenerator GetGenerator(Type reflectedType)
		{
			if (reflectedType.IsInterface || reflectedType.IsAbstract)
			{
				return delegate()
				{
					string text = "Cannot create an instance of an interface or abstract type for ";
					Type reflectedType2 = reflectedType;
					throw new Exception(text + ((reflectedType2 != null) ? reflectedType2.ToString() : null));
				};
			}
			if (typeof(ScriptableObject).IsAssignableFrom(reflectedType))
			{
				return () => ScriptableObject.CreateInstance(reflectedType);
			}
			if (reflectedType.IsArray)
			{
				return () => Array.CreateInstance(reflectedType.GetElementType(), 0);
			}
			if (reflectedType == typeof(string))
			{
				return () => string.Empty;
			}
			if (reflectedType.IsValueType || reflectedType.RTIsDefined(true) || !fsMetaType.HasDefaultConstructor(reflectedType))
			{
				return () => FormatterServices.GetSafeUninitializedObject(reflectedType);
			}
			return delegate()
			{
				object result;
				try
				{
					result = Activator.CreateInstance(reflectedType, true);
				}
				catch
				{
					result = null;
				}
				return result;
			};
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001330B File Offset: 0x0001150B
		private static bool HasDefaultConstructor(Type reflectedType)
		{
			return reflectedType.IsArray || reflectedType.IsValueType || reflectedType.RTGetDefaultConstructor() != null;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00013330 File Offset: 0x00011530
		public object GetDefaultInstance()
		{
			object result = null;
			if (fsMetaType._defaultInstances.TryGetValue(this.reflectedType, ref result))
			{
				return result;
			}
			return fsMetaType._defaultInstances[this.reflectedType] = this.CreateInstance();
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001336E File Offset: 0x0001156E
		public object CreateInstance()
		{
			if (this.generator != null)
			{
				return this.generator();
			}
			string text = "Cant create instance generator for ";
			Type reflectedType = this.reflectedType;
			throw new Exception(text + ((reflectedType != null) ? reflectedType.ToString() : null));
		}

		// Token: 0x040001FB RID: 507
		private static Dictionary<Type, fsMetaType> _metaTypes = new Dictionary<Type, fsMetaType>();

		// Token: 0x040001FC RID: 508
		private static Dictionary<Type, object> _defaultInstances = new Dictionary<Type, object>();

		// Token: 0x040001FD RID: 509
		private fsMetaType.ObjectGenerator generator;

		// Token: 0x02000132 RID: 306
		// (Invoke) Token: 0x06000861 RID: 2145
		private delegate object ObjectGenerator();
	}
}
