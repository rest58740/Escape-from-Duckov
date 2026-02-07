using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParadoxNotion.Serialization.FullSerializer.Internal;
using ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000AE RID: 174
	public class fsSerializer
	{
		// Token: 0x06000690 RID: 1680 RVA: 0x000135EC File Offset: 0x000117EC
		public static bool IsReservedKeyword(string key)
		{
			return key == "$ref" || key == "$id" || key == "$type" || key == "$version" || key == "$content";
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00013648 File Offset: 0x00011848
		public static void RemoveMetaData(ref fsData data)
		{
			if (data.IsDictionary)
			{
				data.AsDictionary.Remove("$ref");
				data.AsDictionary.Remove("$id");
				data.AsDictionary.Remove("$type");
				data.AsDictionary.Remove("$version");
				data.AsDictionary.Remove("$content");
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000136B8 File Offset: 0x000118B8
		private static void EnsureDictionary(ref fsData data)
		{
			if (!data.IsDictionary)
			{
				fsData fsData = data.Clone();
				data.BecomeDictionary();
				data.AsDictionary["$content"] = fsData;
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000136EF File Offset: 0x000118EF
		private static bool IsObjectReference(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$ref");
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001370B File Offset: 0x0001190B
		private static bool IsObjectDefinition(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$id");
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00013727 File Offset: 0x00011927
		private static bool IsVersioned(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$version");
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00013743 File Offset: 0x00011943
		private static bool IsTypeSpecified(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$type");
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001375F File Offset: 0x0001195F
		private static bool IsWrappedData(fsData data)
		{
			return data.IsDictionary && data.AsDictionary.ContainsKey("$content");
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001377C File Offset: 0x0001197C
		private static void Invoke_OnBeforeSerialize(List<fsObjectProcessor> processors, Type storageType, object instance)
		{
			for (int i = 0; i < processors.Count; i++)
			{
				processors[i].OnBeforeSerialize(storageType, instance);
			}
			if (instance is ISerializationCallbackReceiver && !(instance is Object))
			{
				((ISerializationCallbackReceiver)instance).OnBeforeSerialize();
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000137C4 File Offset: 0x000119C4
		private static void Invoke_OnAfterSerialize(List<fsObjectProcessor> processors, Type storageType, object instance, ref fsData data)
		{
			for (int i = processors.Count - 1; i >= 0; i--)
			{
				processors[i].OnAfterSerialize(storageType, instance, ref data);
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000137F4 File Offset: 0x000119F4
		private static void Invoke_OnBeforeDeserialize(List<fsObjectProcessor> processors, Type storageType, ref fsData data)
		{
			for (int i = 0; i < processors.Count; i++)
			{
				processors[i].OnBeforeDeserialize(storageType, ref data);
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00013820 File Offset: 0x00011A20
		private static void Invoke_OnBeforeDeserializeAfterInstanceCreation(List<fsObjectProcessor> processors, Type storageType, object instance, ref fsData data)
		{
			for (int i = 0; i < processors.Count; i++)
			{
				processors[i].OnBeforeDeserializeAfterInstanceCreation(storageType, instance, ref data);
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00013850 File Offset: 0x00011A50
		private static void Invoke_OnAfterDeserialize(List<fsObjectProcessor> processors, Type storageType, object instance)
		{
			for (int i = processors.Count - 1; i >= 0; i--)
			{
				processors[i].OnAfterDeserialize(storageType, instance);
			}
			if (instance is ISerializationCallbackReceiver && !(instance is Object))
			{
				((ISerializationCallbackReceiver)instance).OnAfterDeserialize();
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00013899 File Offset: 0x00011A99
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x000138A1 File Offset: 0x00011AA1
		public List<Object> ReferencesDatabase { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000138AA File Offset: 0x00011AAA
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x000138B2 File Offset: 0x00011AB2
		public bool IgnoreSerializeCycleReferences { get; set; }

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x060006A1 RID: 1697 RVA: 0x000138BC File Offset: 0x00011ABC
		// (remove) Token: 0x060006A2 RID: 1698 RVA: 0x000138F4 File Offset: 0x00011AF4
		public event Action<object> onBeforeObjectSerialized;

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060006A3 RID: 1699 RVA: 0x0001392C File Offset: 0x00011B2C
		// (remove) Token: 0x060006A4 RID: 1700 RVA: 0x00013964 File Offset: 0x00011B64
		public event Action<object, fsData> onAfterObjectSerialized;

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001399C File Offset: 0x00011B9C
		public fsSerializer()
		{
			this._cachedOverrideConverterInstances = new Dictionary<Type, fsBaseConverter>();
			this._cachedConverters = new Dictionary<Type, fsBaseConverter>();
			this._cachedProcessors = new Dictionary<Type, List<fsObjectProcessor>>();
			this._references = new fsCyclicReferenceManager();
			this._lazyReferenceWriter = new fsSerializer.fsLazyCycleDefinitionWriter();
			this._collectors = new Stack<ISerializationCollector>();
			List<fsConverter> list = new List<fsConverter>();
			list.Add(new fsUnityObjectConverter
			{
				Serializer = this
			});
			list.Add(new fsTypeConverter
			{
				Serializer = this
			});
			list.Add(new fsEnumConverter
			{
				Serializer = this
			});
			list.Add(new fsPrimitiveConverter
			{
				Serializer = this
			});
			list.Add(new fsArrayConverter
			{
				Serializer = this
			});
			list.Add(new fsDictionaryConverter
			{
				Serializer = this
			});
			list.Add(new fsListConverter
			{
				Serializer = this
			});
			list.Add(new fsReflectedConverter
			{
				Serializer = this
			});
			this._availableConverters = list;
			this._availableDirectConverters = new Dictionary<Type, fsDirectConverter>();
			this._processors = new List<fsObjectProcessor>();
			this.AddConverter(new AnimationCurve_DirectConverter());
			this.AddConverter(new Bounds_DirectConverter());
			this.AddConverter(new GUIStyleState_DirectConverter());
			this.AddConverter(new GUIStyle_DirectConverter());
			this.AddConverter(new Gradient_DirectConverter());
			this.AddConverter(new Keyframe_DirectConverter());
			this.AddConverter(new LayerMask_DirectConverter());
			this.AddConverter(new RectOffset_DirectConverter());
			this.AddConverter(new Rect_DirectConverter());
			this.AddConverter(new Vector2Int_DirectConverter());
			this.AddConverter(new Vector3Int_DirectConverter());
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00013B1B File Offset: 0x00011D1B
		public void PurgeTemporaryData()
		{
			this._references.Clear();
			this._lazyReferenceWriter.Clear();
			this._collectors.Clear();
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00013B40 File Offset: 0x00011D40
		private List<fsObjectProcessor> GetProcessors(Type type)
		{
			List<fsObjectProcessor> list;
			if (this._cachedProcessors.TryGetValue(type, ref list))
			{
				return list;
			}
			fsObjectAttribute fsObjectAttribute = type.RTGetAttribute(true);
			if (fsObjectAttribute != null && fsObjectAttribute.Processor != null)
			{
				fsObjectProcessor fsObjectProcessor = (fsObjectProcessor)Activator.CreateInstance(fsObjectAttribute.Processor);
				list = new List<fsObjectProcessor>();
				list.Add(fsObjectProcessor);
				this._cachedProcessors[type] = list;
			}
			else if (!this._cachedProcessors.TryGetValue(type, ref list))
			{
				list = new List<fsObjectProcessor>();
				for (int i = 0; i < this._processors.Count; i++)
				{
					fsObjectProcessor fsObjectProcessor2 = this._processors[i];
					if (fsObjectProcessor2.CanProcess(type))
					{
						list.Add(fsObjectProcessor2);
					}
				}
				this._cachedProcessors[type] = list;
			}
			return list;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00013C00 File Offset: 0x00011E00
		public void AddConverter(fsBaseConverter converter)
		{
			if (converter.Serializer != null)
			{
				throw new InvalidOperationException("Cannot add a single converter instance to multiple fsConverters -- please construct a new instance for " + ((converter != null) ? converter.ToString() : null));
			}
			if (converter is fsDirectConverter)
			{
				fsDirectConverter fsDirectConverter = (fsDirectConverter)converter;
				this._availableDirectConverters[fsDirectConverter.ModelType] = fsDirectConverter;
			}
			else
			{
				if (!(converter is fsConverter))
				{
					throw new InvalidOperationException("Unable to add converter " + ((converter != null) ? converter.ToString() : null) + "; the type association strategy is unknown. Please use either fsDirectConverter or fsConverter as your base type.");
				}
				this._availableConverters.Insert(0, (fsConverter)converter);
			}
			converter.Serializer = this;
			this._cachedConverters = new Dictionary<Type, fsBaseConverter>();
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00013CA8 File Offset: 0x00011EA8
		private fsBaseConverter GetConverter(Type type, Type overrideConverterType)
		{
			if (overrideConverterType != null)
			{
				fsBaseConverter fsBaseConverter;
				if (!this._cachedOverrideConverterInstances.TryGetValue(overrideConverterType, ref fsBaseConverter))
				{
					fsBaseConverter = (fsBaseConverter)Activator.CreateInstance(overrideConverterType);
					fsBaseConverter.Serializer = this;
					this._cachedOverrideConverterInstances[overrideConverterType] = fsBaseConverter;
				}
				return fsBaseConverter;
			}
			fsBaseConverter fsBaseConverter2;
			if (this._cachedConverters.TryGetValue(type, ref fsBaseConverter2))
			{
				return fsBaseConverter2;
			}
			fsObjectAttribute fsObjectAttribute = type.RTGetAttribute(true);
			if (fsObjectAttribute != null && fsObjectAttribute.Converter != null)
			{
				fsBaseConverter2 = (fsBaseConverter)Activator.CreateInstance(fsObjectAttribute.Converter);
				fsBaseConverter2.Serializer = this;
				return this._cachedConverters[type] = fsBaseConverter2;
			}
			fsForwardAttribute fsForwardAttribute = type.RTGetAttribute(true);
			if (fsForwardAttribute != null)
			{
				fsBaseConverter2 = new fsForwardConverter(fsForwardAttribute);
				fsBaseConverter2.Serializer = this;
				return this._cachedConverters[type] = fsBaseConverter2;
			}
			fsDirectConverter fsDirectConverter;
			if (this._availableDirectConverters.TryGetValue(type, ref fsDirectConverter))
			{
				return this._cachedConverters[type] = fsDirectConverter;
			}
			for (int i = 0; i < this._availableConverters.Count; i++)
			{
				if (this._availableConverters[i].CanProcess(type))
				{
					return this._cachedConverters[type] = this._availableConverters[i];
				}
			}
			return this._cachedConverters[type] = null;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00013DEF File Offset: 0x00011FEF
		public fsResult TrySerialize(Type storageType, object instance, out fsData data)
		{
			return this.TrySerialize(storageType, instance, out data, null);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00013DFC File Offset: 0x00011FFC
		public fsResult TrySerialize(Type storageType, object instance, out fsData data, Type overrideConverterType)
		{
			Type type = (instance == null) ? storageType : instance.GetType();
			List<fsObjectProcessor> processors = this.GetProcessors(type);
			fsSerializer.Invoke_OnBeforeSerialize(processors, storageType, instance);
			if (instance == null)
			{
				data = new fsData();
				fsSerializer.Invoke_OnAfterSerialize(processors, storageType, instance, ref data);
				return fsResult.Success;
			}
			if (this.onBeforeObjectSerialized != null)
			{
				this.onBeforeObjectSerialized.Invoke(instance);
			}
			fsResult result;
			try
			{
				this._references.Enter();
				result = this.Internal_Serialize(storageType, instance, out data, overrideConverterType);
			}
			finally
			{
				if (this._references.Exit())
				{
					this._lazyReferenceWriter.Clear();
				}
			}
			this.TrySerializeVersioning(instance, ref data);
			fsSerializer.Invoke_OnAfterSerialize(processors, storageType, instance, ref data);
			if (this.onAfterObjectSerialized != null)
			{
				this.onAfterObjectSerialized.Invoke(instance, data);
			}
			return result;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00013EC0 File Offset: 0x000120C0
		private fsResult Internal_Serialize(Type storageType, object instance, out fsData data, Type overrideConverterType)
		{
			Type type = instance.GetType();
			fsBaseConverter converter = this.GetConverter(type, overrideConverterType);
			if (converter == null)
			{
				data = new fsData();
				return fsResult.Success;
			}
			bool flag = type.RTIsDefined(true);
			if (flag)
			{
				if (this._references.IsReference(instance))
				{
					data = fsData.CreateDictionary();
					this._lazyReferenceWriter.WriteReference(this._references.GetReferenceId(instance), data.AsDictionary);
					return fsResult.Success;
				}
				this._references.MarkSerialized(instance);
			}
			this.TryPush(instance);
			fsResult result = converter.TrySerialize(instance, out data, type);
			this.TryPop(instance);
			if (result.Failed)
			{
				return result;
			}
			if (storageType != type && this.GetConverter(storageType, overrideConverterType).RequestInheritanceSupport(storageType))
			{
				fsSerializer.EnsureDictionary(ref data);
				data.AsDictionary["$type"] = new fsData(type.FullName);
			}
			if (flag)
			{
				this._lazyReferenceWriter.WriteDefinition(this._references.GetReferenceId(instance), data);
			}
			return result;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00013FBA File Offset: 0x000121BA
		public fsResult TryDeserialize(fsData data, Type storageType, ref object result)
		{
			return this.TryDeserialize(data, storageType, ref result, null);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00013FC8 File Offset: 0x000121C8
		public fsResult TryDeserialize(fsData data, Type storageType, ref object result, Type overrideConverterType)
		{
			if (data.IsNull)
			{
				result = null;
				List<fsObjectProcessor> processors = this.GetProcessors(storageType);
				fsSerializer.Invoke_OnBeforeDeserialize(processors, storageType, ref data);
				fsSerializer.Invoke_OnAfterDeserialize(processors, storageType, null);
				return fsResult.Success;
			}
			fsResult result2;
			try
			{
				this._references.Enter();
				result2 = this.Internal_Deserialize(data, storageType, ref result, overrideConverterType);
			}
			finally
			{
				this._references.Exit();
			}
			return result2;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00014038 File Offset: 0x00012238
		private fsResult Internal_Deserialize(fsData data, Type storageType, ref object result, Type overrideConverterType)
		{
			if (fsSerializer.IsObjectReference(data))
			{
				int id = int.Parse(data.AsDictionary["$ref"].AsString);
				result = this._references.GetReferenceObject(id);
				return fsResult.Success;
			}
			fsResult fsResult = fsResult.Success;
			Type type = (result != null) ? result.GetType() : storageType;
			Type type2 = null;
			List<fsObjectProcessor> processors = this.GetProcessors(type);
			fsSerializer.Invoke_OnBeforeDeserialize(processors, type, ref data);
			if (fsSerializer.IsTypeSpecified(data))
			{
				fsData fsData = data.AsDictionary["$type"];
				if (!fsData.IsString)
				{
					fsResult.AddMessage(string.Format("{0} value must be a string", "$type"));
				}
				else
				{
					string asString = fsData.AsString;
					Type type3 = ReflectionTools.GetType(asString, storageType);
					if (type3 == null)
					{
						fsResult.AddMessage(string.Format("{0} type can not be resolved", asString));
					}
					else
					{
						fsMigrateToAttribute fsMigrateToAttribute = type3.RTGetAttribute(true);
						if (fsMigrateToAttribute != null)
						{
							if (!typeof(IMigratable).IsAssignableFrom(fsMigrateToAttribute.targetType))
							{
								throw new Exception("TargetType of [fsMigrateToAttribute] must implement IMigratable<T> with T being the target type");
							}
							type2 = type3;
							if (type3.IsGenericType && fsMigrateToAttribute.targetType.IsGenericTypeDefinition)
							{
								type3 = fsMigrateToAttribute.targetType.MakeGenericType(type3.GetGenericArguments());
							}
							else
							{
								type3 = fsMigrateToAttribute.targetType;
							}
						}
						if (!storageType.IsAssignableFrom(type3))
						{
							fsResult.AddMessage(string.Format("Ignoring type specifier. Field or type {0} can't hold and instance of type {1}", storageType, type3));
						}
						else
						{
							type = type3;
						}
					}
				}
			}
			fsBaseConverter converter = this.GetConverter(type, overrideConverterType);
			if (converter == null)
			{
				return fsResult.Warn(string.Format("No Converter available for {0}", type));
			}
			if (result == null || result.GetType() != type)
			{
				result = converter.CreateInstance(data, type);
			}
			if (type2 != null)
			{
				object previousInstance = this.GetConverter(type2, null).CreateInstance(data, type2);
				this.TryDeserializeVersioning(ref previousInstance, ref data);
				this.TryDeserializeMigration(ref result, ref data, type2, previousInstance);
			}
			else
			{
				this.TryDeserializeVersioning(ref result, ref data);
			}
			fsSerializer.Invoke_OnBeforeDeserializeAfterInstanceCreation(processors, type, result, ref data);
			if (fsSerializer.IsObjectDefinition(data))
			{
				int id2 = int.Parse(data.AsDictionary["$id"].AsString);
				this._references.AddReferenceWithId(id2, result);
			}
			if (fsSerializer.IsWrappedData(data))
			{
				data = data.AsDictionary["$content"];
			}
			this.TryPush(result);
			fsResult += converter.TryDeserialize(data, ref result, type);
			if (fsResult.Succeeded)
			{
				fsSerializer.Invoke_OnAfterDeserialize(processors, type, result);
			}
			this.TryPop(result);
			return fsResult;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000142B0 File Offset: 0x000124B0
		private void TryPush(object o)
		{
			if (o is ISerializationCollectable)
			{
				this._collectableDepth++;
				if (this._collectors.Count > 0)
				{
					this._collectors.Peek().OnCollect((ISerializationCollectable)o, this._collectableDepth);
				}
			}
			if (o is ISerializationCollector)
			{
				this._collectableDepth = -1;
				ISerializationCollector parent = (this._collectors.Count > 0) ? this._collectors.Peek() : null;
				this._collectors.Push((ISerializationCollector)o);
				((ISerializationCollector)o).OnPush(parent);
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00014348 File Offset: 0x00012548
		private void TryPop(object o)
		{
			if (o is ISerializationCollector)
			{
				this._collectableDepth = 0;
				this._collectors.Pop().OnPop((this._collectors.Count > 0) ? this._collectors.Peek() : null);
			}
			if (o is ISerializationCollectable)
			{
				this._collectableDepth--;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000143A8 File Offset: 0x000125A8
		private void TrySerializeVersioning(object currentInstance, ref fsData data)
		{
			if (currentInstance is IMigratable && data.IsDictionary)
			{
				fsMigrateVersionsAttribute fsMigrateVersionsAttribute = currentInstance.GetType().RTGetAttribute(true);
				if (fsMigrateVersionsAttribute != null && fsMigrateVersionsAttribute.previousTypes.Length != 0)
				{
					data.AsDictionary["$version"] = new fsData((long)fsMigrateVersionsAttribute.previousTypes.Length);
				}
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00014400 File Offset: 0x00012600
		private void TryDeserializeVersioning(ref object currentInstance, ref fsData currentData)
		{
			if (currentInstance is IMigratable && currentData.IsDictionary)
			{
				Type type = currentInstance.GetType();
				int num = 0;
				fsData fsData;
				if (currentData.AsDictionary.TryGetValue("$version", ref fsData))
				{
					num = (int)fsData.AsInt64;
				}
				fsMigrateVersionsAttribute fsMigrateVersionsAttribute = type.RTGetAttribute(true);
				if (fsMigrateVersionsAttribute != null)
				{
					Type[] previousTypes = fsMigrateVersionsAttribute.previousTypes;
					if (previousTypes.Length > num)
					{
						Type previousType = previousTypes[num];
						this.TryDeserializeMigration(ref currentInstance, ref currentData, previousType, null);
					}
				}
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001446C File Offset: 0x0001266C
		private void TryDeserializeMigration(ref object currentInstance, ref fsData currentData, Type previousType, object previousInstance)
		{
			if (currentInstance is IMigratable && currentData.IsDictionary)
			{
				Type type = currentInstance.GetType();
				if (type.IsGenericType && previousType.IsGenericTypeDefinition)
				{
					previousType = previousType.MakeGenericType(type.GetGenericArguments());
				}
				InterfaceMapping interfaceMap;
				try
				{
					interfaceMap = type.GetInterfaceMap(typeof(IMigratable<>).MakeGenericType(new Type[]
					{
						previousType
					}));
				}
				catch (Exception ex)
				{
					throw new Exception("Type must implement IMigratable<T> for each one of the types specified in the [fsMigrateVersionsAttribute] or [fsMigrateToAttribute]\n" + ex.Message);
				}
				MethodBase methodBase = interfaceMap.InterfaceMethods.First((MethodInfo m) => m.Name == "Migrate");
				fsBaseConverter converter = this.GetConverter(previousType, null);
				if (previousInstance == null)
				{
					previousInstance = converter.CreateInstance(currentData, previousType);
				}
				converter.TryDeserialize(currentData, ref previousInstance, previousType).AssertSuccess();
				methodBase.Invoke(currentInstance, ReflectionTools.SingleTempArgsArray(previousInstance));
				fsData fsData;
				converter.TrySerialize(previousInstance, out fsData, previousType).AssertSuccess();
				foreach (KeyValuePair<string, fsData> keyValuePair in fsData.AsDictionary)
				{
					currentData.AsDictionary.Remove(keyValuePair.Key);
				}
			}
		}

		// Token: 0x04000205 RID: 517
		public const string KEY_OBJECT_REFERENCE = "$ref";

		// Token: 0x04000206 RID: 518
		public const string KEY_OBJECT_DEFINITION = "$id";

		// Token: 0x04000207 RID: 519
		public const string KEY_INSTANCE_TYPE = "$type";

		// Token: 0x04000208 RID: 520
		public const string KEY_VERSION = "$version";

		// Token: 0x04000209 RID: 521
		public const string KEY_CONTENT = "$content";

		// Token: 0x0400020A RID: 522
		private Dictionary<Type, fsBaseConverter> _cachedOverrideConverterInstances;

		// Token: 0x0400020B RID: 523
		private Dictionary<Type, fsBaseConverter> _cachedConverters;

		// Token: 0x0400020C RID: 524
		private readonly List<fsConverter> _availableConverters;

		// Token: 0x0400020D RID: 525
		private readonly Dictionary<Type, fsDirectConverter> _availableDirectConverters;

		// Token: 0x0400020E RID: 526
		private readonly List<fsObjectProcessor> _processors;

		// Token: 0x0400020F RID: 527
		private Dictionary<Type, List<fsObjectProcessor>> _cachedProcessors;

		// Token: 0x04000210 RID: 528
		private fsCyclicReferenceManager _references;

		// Token: 0x04000211 RID: 529
		private fsSerializer.fsLazyCycleDefinitionWriter _lazyReferenceWriter;

		// Token: 0x04000212 RID: 530
		private Stack<ISerializationCollector> _collectors;

		// Token: 0x04000213 RID: 531
		private int _collectableDepth;

		// Token: 0x02000135 RID: 309
		internal class fsLazyCycleDefinitionWriter
		{
			// Token: 0x0600086D RID: 2157 RVA: 0x000186D8 File Offset: 0x000168D8
			public void WriteDefinition(int id, fsData data)
			{
				if (this._references.Contains(id))
				{
					fsSerializer.EnsureDictionary(ref data);
					data.AsDictionary["$id"] = new fsData(id.ToString());
					return;
				}
				this._pendingDefinitions[id] = data;
			}

			// Token: 0x0600086E RID: 2158 RVA: 0x00018724 File Offset: 0x00016924
			public void WriteReference(int id, Dictionary<string, fsData> dict)
			{
				fsData fsData;
				if (this._pendingDefinitions.TryGetValue(id, ref fsData))
				{
					fsSerializer.EnsureDictionary(ref fsData);
					fsData.AsDictionary["$id"] = new fsData(id.ToString());
					this._pendingDefinitions.Remove(id);
				}
				else
				{
					this._references.Add(id);
				}
				dict["$ref"] = new fsData(id.ToString());
			}

			// Token: 0x0600086F RID: 2159 RVA: 0x00018797 File Offset: 0x00016997
			public void Clear()
			{
				this._pendingDefinitions.Clear();
				this._references.Clear();
			}

			// Token: 0x04000311 RID: 785
			private Dictionary<int, fsData> _pendingDefinitions = new Dictionary<int, fsData>();

			// Token: 0x04000312 RID: 786
			private HashSet<int> _references = new HashSet<int>();
		}
	}
}
