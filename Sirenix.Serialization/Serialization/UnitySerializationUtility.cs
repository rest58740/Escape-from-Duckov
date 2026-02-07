using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using Sirenix.Serialization.Utilities;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Sirenix.Serialization
{
	// Token: 0x020000B5 RID: 181
	public static class UnitySerializationUtility
	{
		// Token: 0x060004F4 RID: 1268 RVA: 0x00021754 File Offset: 0x0001F954
		public static bool OdinWillSerialize(MemberInfo member, bool serializeUnityFields, ISerializationPolicy policy = null)
		{
			Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult> dictionary;
			if (policy == null || policy == UnitySerializationUtility.UnityPolicy)
			{
				dictionary = UnitySerializationUtility.OdinWillSerializeCache_UnityPolicy;
			}
			else if (policy == UnitySerializationUtility.EverythingPolicy)
			{
				dictionary = UnitySerializationUtility.OdinWillSerializeCache_EverythingPolicy;
			}
			else if (policy == UnitySerializationUtility.StrictPolicy)
			{
				dictionary = UnitySerializationUtility.OdinWillSerializeCache_StrictPolicy;
			}
			else
			{
				Dictionary<ISerializationPolicy, Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult>> odinWillSerializeCache_CustomPolicies = UnitySerializationUtility.OdinWillSerializeCache_CustomPolicies;
				lock (odinWillSerializeCache_CustomPolicies)
				{
					if (!UnitySerializationUtility.OdinWillSerializeCache_CustomPolicies.TryGetValue(policy, ref dictionary))
					{
						dictionary = new Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<MemberInfo>.Default);
						UnitySerializationUtility.OdinWillSerializeCache_CustomPolicies.Add(policy, dictionary);
					}
				}
			}
			Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult> dictionary2 = dictionary;
			bool result;
			lock (dictionary2)
			{
				UnitySerializationUtility.CachedSerializationBackendResult cachedSerializationBackendResult;
				if (!dictionary.TryGetValue(member, ref cachedSerializationBackendResult))
				{
					cachedSerializationBackendResult = default(UnitySerializationUtility.CachedSerializationBackendResult);
					if (serializeUnityFields)
					{
						cachedSerializationBackendResult.SerializeUnityFieldsTrueResult = UnitySerializationUtility.CalculateOdinWillSerialize(member, serializeUnityFields, policy ?? UnitySerializationUtility.UnityPolicy);
						cachedSerializationBackendResult.HasCalculatedSerializeUnityFieldsTrueResult = true;
					}
					else
					{
						cachedSerializationBackendResult.SerializeUnityFieldsFalseResult = UnitySerializationUtility.CalculateOdinWillSerialize(member, serializeUnityFields, policy ?? UnitySerializationUtility.UnityPolicy);
						cachedSerializationBackendResult.HasCalculatedSerializeUnityFieldsFalseResult = true;
					}
					dictionary.Add(member, cachedSerializationBackendResult);
				}
				else if (serializeUnityFields && !cachedSerializationBackendResult.HasCalculatedSerializeUnityFieldsTrueResult)
				{
					cachedSerializationBackendResult.SerializeUnityFieldsTrueResult = UnitySerializationUtility.CalculateOdinWillSerialize(member, serializeUnityFields, policy ?? UnitySerializationUtility.UnityPolicy);
					cachedSerializationBackendResult.HasCalculatedSerializeUnityFieldsTrueResult = true;
					dictionary[member] = cachedSerializationBackendResult;
				}
				else if (!serializeUnityFields && !cachedSerializationBackendResult.HasCalculatedSerializeUnityFieldsFalseResult)
				{
					cachedSerializationBackendResult.SerializeUnityFieldsFalseResult = UnitySerializationUtility.CalculateOdinWillSerialize(member, serializeUnityFields, policy ?? UnitySerializationUtility.UnityPolicy);
					cachedSerializationBackendResult.HasCalculatedSerializeUnityFieldsFalseResult = true;
					dictionary[member] = cachedSerializationBackendResult;
				}
				result = (serializeUnityFields ? cachedSerializationBackendResult.SerializeUnityFieldsTrueResult : cachedSerializationBackendResult.SerializeUnityFieldsFalseResult);
			}
			return result;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000218F0 File Offset: 0x0001FAF0
		private static bool CalculateOdinWillSerialize(MemberInfo member, bool serializeUnityFields, ISerializationPolicy policy)
		{
			if (member.DeclaringType == typeof(Object))
			{
				return false;
			}
			if (!policy.ShouldSerializeMember(member))
			{
				return false;
			}
			if (member is FieldInfo && member.IsDefined(typeof(OdinSerializeAttribute), true))
			{
				return true;
			}
			if (serializeUnityFields)
			{
				return true;
			}
			try
			{
				if (UnitySerializationUtility.SerializeReferenceAttributeType != null && member.IsDefined(UnitySerializationUtility.SerializeReferenceAttributeType, true))
				{
					return false;
				}
			}
			catch
			{
			}
			return !UnitySerializationUtility.GuessIfUnityWillSerialize(member);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00021988 File Offset: 0x0001FB88
		public static bool GuessIfUnityWillSerialize(MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			Dictionary<MemberInfo, bool> unityWillSerializeMembersCache = UnitySerializationUtility.UnityWillSerializeMembersCache;
			bool flag2;
			lock (unityWillSerializeMembersCache)
			{
				if (!UnitySerializationUtility.UnityWillSerializeMembersCache.TryGetValue(member, ref flag2))
				{
					flag2 = UnitySerializationUtility.GuessIfUnityWillSerializePrivate(member);
					UnitySerializationUtility.UnityWillSerializeMembersCache[member] = flag2;
				}
			}
			return flag2;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000219F8 File Offset: 0x0001FBF8
		private static bool GuessIfUnityWillSerializePrivate(MemberInfo member)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo == null || fieldInfo.IsStatic || fieldInfo.IsInitOnly)
			{
				return false;
			}
			if (fieldInfo.IsDefined<NonSerializedAttribute>())
			{
				return false;
			}
			if (UnitySerializationUtility.SerializeReferenceAttributeType != null && fieldInfo.IsDefined(UnitySerializationUtility.SerializeReferenceAttributeType, true))
			{
				return true;
			}
			if (!typeof(Object).IsAssignableFrom(fieldInfo.FieldType) && fieldInfo.FieldType == fieldInfo.DeclaringType)
			{
				return false;
			}
			if (!fieldInfo.IsPublic && !fieldInfo.IsDefined<SerializeField>())
			{
				return false;
			}
			if (fieldInfo.IsDefined<FixedBufferAttribute>())
			{
				return Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2017, 1);
			}
			return UnitySerializationUtility.GuessIfUnityWillSerialize(fieldInfo.FieldType);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00021AAC File Offset: 0x0001FCAC
		public static bool GuessIfUnityWillSerialize(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Dictionary<Type, bool> unityWillSerializeTypesCache = UnitySerializationUtility.UnityWillSerializeTypesCache;
			bool flag2;
			lock (unityWillSerializeTypesCache)
			{
				if (!UnitySerializationUtility.UnityWillSerializeTypesCache.TryGetValue(type, ref flag2))
				{
					flag2 = UnitySerializationUtility.GuessIfUnityWillSerializePrivate(type);
					UnitySerializationUtility.UnityWillSerializeTypesCache[type] = flag2;
				}
			}
			return flag2;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00021B1C File Offset: 0x0001FD1C
		private static bool GuessIfUnityWillSerializePrivate(Type type)
		{
			if (UnitySerializationUtility.UnityNeverSerializesTypes.Contains(type) || UnitySerializationUtility.UnityNeverSerializesTypeNames.Contains(type.FullName))
			{
				return false;
			}
			if (typeof(Object).IsAssignableFrom(type))
			{
				return !type.IsGenericType || Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2020, 1);
			}
			if (type.IsAbstract || type.IsInterface || type == typeof(object))
			{
				return false;
			}
			if (type.IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(5, 6))
				{
					return underlyingType != typeof(long) && underlyingType != typeof(ulong);
				}
				return underlyingType == typeof(int) || underlyingType == typeof(byte);
			}
			else
			{
				if (type.IsPrimitive || type == typeof(string))
				{
					return true;
				}
				if (typeof(Delegate).IsAssignableFrom(type))
				{
					return false;
				}
				if (typeof(UnityEventBase).IsAssignableFrom(type))
				{
					return (!type.IsGenericType || Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2020, 1)) && (type == typeof(UnityEvent) || type.IsDefined(false));
				}
				if (type.IsArray)
				{
					Type elementType = type.GetElementType();
					return type.GetArrayRank() == 1 && !elementType.IsArray && !elementType.ImplementsOpenGenericClass(typeof(List)) && UnitySerializationUtility.GuessIfUnityWillSerialize(elementType);
				}
				if (type.IsGenericType && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(List))
				{
					Type type2 = type.GetArgumentsOfInheritedOpenGenericClass(typeof(List))[0];
					return !type2.IsArray && !type2.ImplementsOpenGenericClass(typeof(List)) && UnitySerializationUtility.GuessIfUnityWillSerialize(type2);
				}
				if (type.Assembly.FullName.StartsWith("UnityEngine", 2) || type.Assembly.FullName.StartsWith("UnityEditor", 2))
				{
					return true;
				}
				if (type.IsGenericType && !Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2020, 1))
				{
					return false;
				}
				if (type.Assembly == UnitySerializationUtility.String_Assembly || type.Assembly == UnitySerializationUtility.HashSet_Assembly || type.Assembly == UnitySerializationUtility.LinkedList_Assembly)
				{
					return false;
				}
				if (type.IsDefined(false))
				{
					return Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(4, 5) || type.IsClass;
				}
				if (!Sirenix.Serialization.Utilities.UnityVersion.IsVersionOrGreater(2018, 2))
				{
					Type baseType = type.BaseType;
					while (baseType != null && baseType != typeof(object))
					{
						if (baseType.IsGenericType && baseType.GetGenericTypeDefinition().FullName == "UnityEngine.Networking.SyncListStruct`1")
						{
							return true;
						}
						baseType = baseType.BaseType;
					}
				}
				return false;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00021DFC File Offset: 0x0001FFFC
		public static void SerializeUnityObject(Object unityObject, ref SerializationData data, bool serializeUnityFields = false, SerializationContext context = null)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			IOverridesSerializationFormat overridesSerializationFormat = unityObject as IOverridesSerializationFormat;
			DataFormat dataFormat;
			if (overridesSerializationFormat != null)
			{
				dataFormat = overridesSerializationFormat.GetFormatToSerializeAs(true);
			}
			else if (GlobalConfig<GlobalSerializationConfig>.HasInstanceLoaded)
			{
				dataFormat = GlobalConfig<GlobalSerializationConfig>.Instance.BuildSerializationFormat;
			}
			else
			{
				dataFormat = 0;
			}
			if (dataFormat == 2)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"The serialization format '",
					dataFormat.ToString(),
					"' is disabled outside of the editor. Defaulting to the format '",
					0.ToString(),
					"' instead."
				}));
				dataFormat = 0;
			}
			UnitySerializationUtility.SerializeUnityObject(unityObject, ref data.SerializedBytes, ref data.ReferencedUnityObjects, dataFormat, false, null);
			data.SerializedFormat = dataFormat;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00021EB4 File Offset: 0x000200B4
		public static void SerializeUnityObject(Object unityObject, ref string base64Bytes, ref List<Object> referencedUnityObjects, DataFormat format, bool serializeUnityFields = false, SerializationContext context = null)
		{
			byte[] array = null;
			UnitySerializationUtility.SerializeUnityObject(unityObject, ref array, ref referencedUnityObjects, format, serializeUnityFields, context);
			base64Bytes = Convert.ToBase64String(array);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00021EDC File Offset: 0x000200DC
		public static void SerializeUnityObject(Object unityObject, ref byte[] bytes, ref List<Object> referencedUnityObjects, DataFormat format, bool serializeUnityFields = false, SerializationContext context = null)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (format == 2)
			{
				Debug.LogError("The serialization data format '" + format.ToString() + "' is not supported by this method. You must create your own writer.");
				return;
			}
			if (referencedUnityObjects == null)
			{
				referencedUnityObjects = new List<Object>();
			}
			else
			{
				referencedUnityObjects.Clear();
			}
			using (Cache<CachedMemoryStream> cache = Cache<CachedMemoryStream>.Claim())
			{
				using (Cache<UnityReferenceResolver> cache2 = Cache<UnityReferenceResolver>.Claim())
				{
					cache2.Value.SetReferencedUnityObjects(referencedUnityObjects);
					if (context != null)
					{
						context.IndexReferenceResolver = cache2.Value;
						using (ICache cachedUnityWriter = UnitySerializationUtility.GetCachedUnityWriter(format, cache.Value.MemoryStream, context))
						{
							UnitySerializationUtility.SerializeUnityObject(unityObject, cachedUnityWriter.Value as IDataWriter, serializeUnityFields);
							goto IL_1CB;
						}
					}
					using (Cache<SerializationContext> cache3 = Cache<SerializationContext>.Claim())
					{
						cache3.Value.Config.SerializationPolicy = SerializationPolicies.Unity;
						if (GlobalConfig<GlobalSerializationConfig>.HasInstanceLoaded)
						{
							cache3.Value.Config.DebugContext.ErrorHandlingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.ErrorHandlingPolicy;
							cache3.Value.Config.DebugContext.LoggingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.LoggingPolicy;
							cache3.Value.Config.DebugContext.Logger = GlobalConfig<GlobalSerializationConfig>.Instance.Logger;
						}
						else
						{
							cache3.Value.Config.DebugContext.ErrorHandlingPolicy = 0;
							cache3.Value.Config.DebugContext.LoggingPolicy = 0;
							cache3.Value.Config.DebugContext.Logger = DefaultLoggers.UnityLogger;
						}
						cache3.Value.IndexReferenceResolver = cache2.Value;
						using (ICache cachedUnityWriter2 = UnitySerializationUtility.GetCachedUnityWriter(format, cache.Value.MemoryStream, cache3))
						{
							UnitySerializationUtility.SerializeUnityObject(unityObject, cachedUnityWriter2.Value as IDataWriter, serializeUnityFields);
						}
					}
					IL_1CB:
					bytes = cache.Value.MemoryStream.ToArray();
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00022158 File Offset: 0x00020358
		public static void SerializeUnityObject(Object unityObject, IDataWriter writer, bool serializeUnityFields = false)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			try
			{
				writer.PrepareNewSerializationSession();
				MemberInfo[] serializableMembers = FormatterUtilities.GetSerializableMembers(unityObject.GetType(), writer.Context.Config.SerializationPolicy);
				object obj = unityObject;
				foreach (MemberInfo memberInfo in serializableMembers)
				{
					Sirenix.Serialization.Utilities.WeakValueGetter cachedUnityMemberGetter;
					if (UnitySerializationUtility.OdinWillSerialize(memberInfo, serializeUnityFields, writer.Context.Config.SerializationPolicy) && (cachedUnityMemberGetter = UnitySerializationUtility.GetCachedUnityMemberGetter(memberInfo)) != null)
					{
						object obj2 = cachedUnityMemberGetter(ref obj);
						if (obj2 == null || !(obj2.GetType() == typeof(SerializationData)))
						{
							Serializer serializer = Serializer.Get(FormatterUtilities.GetContainedType(memberInfo));
							try
							{
								serializer.WriteValueWeak(memberInfo.Name, obj2, writer);
							}
							catch (Exception exception)
							{
								writer.Context.Config.DebugContext.LogException(exception);
							}
						}
					}
				}
				writer.FlushToStream();
			}
			catch (SerializationAbortException innerException)
			{
				throw new SerializationAbortException("Serialization of type '" + unityObject.GetType().GetNiceFullName() + "' aborted.", innerException);
			}
			catch (Exception ex)
			{
				Debug.LogException(new Exception("Exception thrown while serializing type '" + unityObject.GetType().GetNiceFullName() + "': " + ex.Message, ex));
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000222D8 File Offset: 0x000204D8
		public static void DeserializeUnityObject(Object unityObject, ref SerializationData data, DeserializationContext context = null)
		{
			UnitySerializationUtility.DeserializeUnityObject(unityObject, ref data, context, false, null);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x000222E4 File Offset: 0x000204E4
		private static void DeserializeUnityObject(Object unityObject, ref SerializationData data, DeserializationContext context, bool isPrefabData, List<Object> prefabInstanceUnityObjects)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (isPrefabData && prefabInstanceUnityObjects == null)
			{
				prefabInstanceUnityObjects = new List<Object>();
			}
			if (data.SerializedBytes != null && data.SerializedBytes.Length != 0 && (data.SerializationNodes == null || data.SerializationNodes.Count == 0))
			{
				if (data.SerializedFormat == 2)
				{
					DataFormat format = (data.SerializedBytes[0] == 123) ? 1 : 0;
					try
					{
						string text = ProperBitConverter.BytesToHexString(data.SerializedBytes, true);
						Debug.LogWarning("Serialization data has only bytes stored, but the serialized format is marked as being 'Nodes', which is incompatible with data stored as a byte array. Based on the appearance of the serialized bytes, Odin has guessed that the data format is '" + format.ToString() + "', and will attempt to deserialize the bytes using that format. The serialized bytes follow, converted to a hex string: " + text);
					}
					catch
					{
					}
					UnitySerializationUtility.DeserializeUnityObject(unityObject, ref data.SerializedBytes, ref data.ReferencedUnityObjects, format, context);
				}
				else
				{
					UnitySerializationUtility.DeserializeUnityObject(unityObject, ref data.SerializedBytes, ref data.ReferencedUnityObjects, data.SerializedFormat, context);
				}
				UnitySerializationUtility.ApplyPrefabModifications(unityObject, data.PrefabModifications, data.PrefabModificationsReferencedUnityObjects);
				return;
			}
			Cache<DeserializationContext> cache = null;
			try
			{
				if (context == null)
				{
					cache = Cache<DeserializationContext>.Claim();
					context = cache;
					context.Config.SerializationPolicy = SerializationPolicies.Unity;
					if (GlobalConfig<GlobalSerializationConfig>.HasInstanceLoaded)
					{
						context.Config.DebugContext.ErrorHandlingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.ErrorHandlingPolicy;
						context.Config.DebugContext.LoggingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.LoggingPolicy;
						context.Config.DebugContext.Logger = GlobalConfig<GlobalSerializationConfig>.Instance.Logger;
					}
					else
					{
						context.Config.DebugContext.ErrorHandlingPolicy = 0;
						context.Config.DebugContext.LoggingPolicy = 0;
						context.Config.DebugContext.Logger = DefaultLoggers.UnityLogger;
					}
				}
				IOverridesSerializationPolicy overridesSerializationPolicy = unityObject as IOverridesSerializationPolicy;
				if (overridesSerializationPolicy != null)
				{
					ISerializationPolicy serializationPolicy = overridesSerializationPolicy.SerializationPolicy;
					if (serializationPolicy != null)
					{
						context.Config.SerializationPolicy = serializationPolicy;
					}
				}
				if (!isPrefabData && !data.Prefab.SafeIsUnityNull())
				{
					if (data.Prefab is ISupportsPrefabSerialization)
					{
						if (data.Prefab != unityObject || data.PrefabModifications == null || data.PrefabModifications.Count <= 0)
						{
							SerializationData serializationData = (data.Prefab as ISupportsPrefabSerialization).SerializationData;
							if (!serializationData.ContainsData)
							{
								UnitySerializationUtility.DeserializeUnityObject(unityObject, ref data, context, true, data.ReferencedUnityObjects);
							}
							else
							{
								UnitySerializationUtility.DeserializeUnityObject(unityObject, ref serializationData, context, true, data.ReferencedUnityObjects);
							}
							UnitySerializationUtility.ApplyPrefabModifications(unityObject, data.PrefabModifications, data.PrefabModificationsReferencedUnityObjects);
							return;
						}
					}
					else if (data.Prefab.GetType() != typeof(Object))
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"The type ",
							data.Prefab.GetType().GetNiceName(),
							" no longer supports special prefab serialization (the interface ",
							typeof(ISupportsPrefabSerialization).GetNiceName(),
							") upon deserialization of an instance of a prefab; prefab data may be lost. Has a type been lost?"
						}));
					}
				}
				List<Object> referencedUnityObjects = isPrefabData ? prefabInstanceUnityObjects : data.ReferencedUnityObjects;
				if (data.SerializedFormat == 2)
				{
					using (SerializationNodeDataReader serializationNodeDataReader = new SerializationNodeDataReader(context))
					{
						using (Cache<UnityReferenceResolver> cache2 = Cache<UnityReferenceResolver>.Claim())
						{
							cache2.Value.SetReferencedUnityObjects(referencedUnityObjects);
							context.IndexReferenceResolver = cache2.Value;
							serializationNodeDataReader.Nodes = data.SerializationNodes;
							UnitySerializationUtility.DeserializeUnityObject(unityObject, serializationNodeDataReader);
							goto IL_368;
						}
					}
				}
				if (data.SerializedBytes != null && data.SerializedBytes.Length != 0)
				{
					UnitySerializationUtility.DeserializeUnityObject(unityObject, ref data.SerializedBytes, ref referencedUnityObjects, data.SerializedFormat, context);
				}
				else
				{
					UnitySerializationUtility.DeserializeUnityObject(unityObject, ref data.SerializedBytesString, ref referencedUnityObjects, data.SerializedFormat, context);
				}
				IL_368:
				UnitySerializationUtility.ApplyPrefabModifications(unityObject, data.PrefabModifications, data.PrefabModificationsReferencedUnityObjects);
			}
			finally
			{
				if (cache != null)
				{
					Cache<DeserializationContext>.Release(cache);
				}
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000226DC File Offset: 0x000208DC
		public static void DeserializeUnityObject(Object unityObject, ref string base64Bytes, ref List<Object> referencedUnityObjects, DataFormat format, DeserializationContext context = null)
		{
			if (string.IsNullOrEmpty(base64Bytes))
			{
				return;
			}
			byte[] array = null;
			try
			{
				array = Convert.FromBase64String(base64Bytes);
			}
			catch (FormatException)
			{
				Debug.LogError("Invalid base64 string when deserializing data: " + base64Bytes);
			}
			if (array != null)
			{
				UnitySerializationUtility.DeserializeUnityObject(unityObject, ref array, ref referencedUnityObjects, format, context);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00022734 File Offset: 0x00020934
		public static void DeserializeUnityObject(Object unityObject, ref byte[] bytes, ref List<Object> referencedUnityObjects, DataFormat format, DeserializationContext context = null)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (bytes == null || bytes.Length == 0)
			{
				return;
			}
			if (format == 2)
			{
				try
				{
					Debug.LogError("The serialization data format '" + format.ToString() + "' is not supported by this method. You must create your own reader.");
				}
				catch
				{
				}
				return;
			}
			if (referencedUnityObjects == null)
			{
				referencedUnityObjects = new List<Object>();
			}
			using (Cache<CachedMemoryStream> cache = Cache<CachedMemoryStream>.Claim())
			{
				using (Cache<UnityReferenceResolver> cache2 = Cache<UnityReferenceResolver>.Claim())
				{
					cache.Value.MemoryStream.Write(bytes, 0, bytes.Length);
					cache.Value.MemoryStream.Position = 0L;
					cache2.Value.SetReferencedUnityObjects(referencedUnityObjects);
					if (context != null)
					{
						context.IndexReferenceResolver = cache2.Value;
						using (ICache cachedUnityReader = UnitySerializationUtility.GetCachedUnityReader(format, cache.Value.MemoryStream, context))
						{
							UnitySerializationUtility.DeserializeUnityObject(unityObject, cachedUnityReader.Value as IDataReader);
							return;
						}
					}
					using (Cache<DeserializationContext> cache3 = Cache<DeserializationContext>.Claim())
					{
						cache3.Value.Config.SerializationPolicy = SerializationPolicies.Unity;
						if (GlobalConfig<GlobalSerializationConfig>.HasInstanceLoaded)
						{
							cache3.Value.Config.DebugContext.ErrorHandlingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.ErrorHandlingPolicy;
							cache3.Value.Config.DebugContext.LoggingPolicy = GlobalConfig<GlobalSerializationConfig>.Instance.LoggingPolicy;
							cache3.Value.Config.DebugContext.Logger = GlobalConfig<GlobalSerializationConfig>.Instance.Logger;
						}
						else
						{
							cache3.Value.Config.DebugContext.ErrorHandlingPolicy = 0;
							cache3.Value.Config.DebugContext.LoggingPolicy = 0;
							cache3.Value.Config.DebugContext.Logger = DefaultLoggers.UnityLogger;
						}
						cache3.Value.IndexReferenceResolver = cache2.Value;
						using (ICache cachedUnityReader2 = UnitySerializationUtility.GetCachedUnityReader(format, cache.Value.MemoryStream, cache3))
						{
							UnitySerializationUtility.DeserializeUnityObject(unityObject, cachedUnityReader2.Value as IDataReader);
						}
					}
				}
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000229E0 File Offset: 0x00020BE0
		public static void DeserializeUnityObject(Object unityObject, IDataReader reader)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			IOverridesSerializationPolicy overridesSerializationPolicy = unityObject as IOverridesSerializationPolicy;
			if (overridesSerializationPolicy != null)
			{
				ISerializationPolicy serializationPolicy = overridesSerializationPolicy.SerializationPolicy;
				if (serializationPolicy != null)
				{
					reader.Context.Config.SerializationPolicy = serializationPolicy;
				}
			}
			try
			{
				reader.PrepareNewSerializationSession();
				Dictionary<string, MemberInfo> serializableMembersMap = FormatterUtilities.GetSerializableMembersMap(unityObject.GetType(), reader.Context.Config.SerializationPolicy);
				int num = 0;
				object obj = unityObject;
				string text;
				EntryType entryType;
				while ((entryType = reader.PeekEntry(out text)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
				{
					MemberInfo member = null;
					Sirenix.Serialization.Utilities.WeakValueSetter weakValueSetter = null;
					bool flag = false;
					if (entryType == EntryType.Invalid)
					{
						string text2 = "Encountered invalid entry while reading serialization data for Unity object of type '" + unityObject.GetType().GetNiceFullName() + "'. This likely means that Unity has filled Odin's stored serialization data with garbage, which can randomly happen after upgrading the Unity version of the project, or when otherwise doing things that have a lot of fragile interactions with the asset database. Locating the asset which causes this error log and causing it to reserialize (IE, modifying it and then causing it to be saved to disk) is likely to 'fix' the issue and make this message go away. Experience shows that this issue is particularly likely to occur on prefab instances, and if this is the case, the parent prefab is also under suspicion, and should be re-saved and re-imported. Note that DATA MAY HAVE BEEN LOST, and you should verify with your version control system (you're using one, right?!) that everything is alright, and if not, use it to rollback the asset to recover your data.\n\n\n";
						text2 = text2 + "IF YOU HAVE CONSISTENT REPRODUCTION STEPS THAT MAKE THIS ISSUE REOCCUR, please report it at this issue at 'https://bitbucket.org/sirenix/odin-inspector/issues/526', and copy paste this debug message into your comment, along with any potential actions or recent changes in the project that might have happened to cause this message to occur. If the data dump in this message is cut off, please find the editor's log file (see https://docs.unity3d.com/Manual/LogFiles.html) and copy paste the full version of this message from there.\n\n\nData dump:\n\n    Reader type: " + reader.GetType().Name + "\n";
						try
						{
							text2 = text2 + "    Data dump: " + reader.GetDataDump();
							goto IL_169;
						}
						finally
						{
							reader.Context.Config.DebugContext.LogError(text2);
							flag = true;
						}
						goto IL_EF;
					}
					goto IL_EF;
					IL_169:
					if (flag)
					{
						reader.SkipEntry();
						continue;
					}
					Type containedType = FormatterUtilities.GetContainedType(member);
					Serializer serializer = Serializer.Get(containedType);
					try
					{
						object value = serializer.ReadValueWeak(reader);
						weakValueSetter(ref obj, value);
					}
					catch (Exception exception)
					{
						reader.Context.Config.DebugContext.LogException(exception);
					}
					num++;
					if (num > 1000)
					{
						reader.Context.Config.DebugContext.LogError("Breaking out of infinite reading loop! (Read more than a thousand entries for one type!)");
						break;
					}
					continue;
					IL_EF:
					if (string.IsNullOrEmpty(text))
					{
						reader.Context.Config.DebugContext.LogError(string.Concat(new string[]
						{
							"Entry of type \"",
							entryType.ToString(),
							"\" in node \"",
							reader.CurrentNodeName,
							"\" is missing a name."
						}));
						flag = true;
						goto IL_169;
					}
					if (!serializableMembersMap.TryGetValue(text, ref member) || (weakValueSetter = UnitySerializationUtility.GetCachedUnityMemberSetter(member)) == null)
					{
						flag = true;
						goto IL_169;
					}
					goto IL_169;
				}
			}
			catch (SerializationAbortException innerException)
			{
				throw new SerializationAbortException("Deserialization of type '" + unityObject.GetType().GetNiceFullName() + "' aborted.", innerException);
			}
			catch (Exception ex)
			{
				Debug.LogException(new Exception("Exception thrown while deserializing type '" + unityObject.GetType().GetNiceFullName() + "': " + ex.Message, ex));
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00022CA8 File Offset: 0x00020EA8
		public static List<string> SerializePrefabModifications(List<PrefabModification> modifications, ref List<Object> referencedUnityObjects)
		{
			if (referencedUnityObjects == null)
			{
				referencedUnityObjects = new List<Object>();
			}
			else if (referencedUnityObjects.Count > 0)
			{
				referencedUnityObjects.Clear();
			}
			if (modifications == null || modifications.Count == 0)
			{
				return new List<string>();
			}
			modifications.Sort(delegate(PrefabModification a, PrefabModification b)
			{
				int num = a.Path.CompareTo(b.Path);
				if (num == 0)
				{
					if ((a.ModificationType == PrefabModificationType.ListLength || a.ModificationType == PrefabModificationType.Dictionary) && b.ModificationType == PrefabModificationType.Value)
					{
						return 1;
					}
					if (a.ModificationType == PrefabModificationType.Value && (b.ModificationType == PrefabModificationType.ListLength || b.ModificationType == PrefabModificationType.Dictionary))
					{
						return -1;
					}
				}
				return num;
			});
			List<string> list = new List<string>();
			using (Cache<SerializationContext> cache = Cache<SerializationContext>.Claim())
			{
				using (Cache<CachedMemoryStream> cache2 = CachedMemoryStream.Claim(null))
				{
					using (Cache<JsonDataWriter> cache3 = Cache<JsonDataWriter>.Claim())
					{
						using (Cache<UnityReferenceResolver> cache4 = Cache<UnityReferenceResolver>.Claim())
						{
							JsonDataWriter value = cache3.Value;
							value.Context = cache;
							value.Stream = cache2.Value.MemoryStream;
							value.PrepareNewSerializationSession();
							value.FormatAsReadable = false;
							value.EnableTypeOptimization = false;
							cache4.Value.SetReferencedUnityObjects(referencedUnityObjects);
							value.Context.IndexReferenceResolver = cache4.Value;
							for (int i = 0; i < modifications.Count; i++)
							{
								PrefabModification prefabModification = modifications[i];
								if (prefabModification.ModificationType == PrefabModificationType.ListLength)
								{
									value.MarkJustStarted();
									value.WriteString("path", prefabModification.Path);
									value.WriteInt32("length", prefabModification.NewLength);
									value.FlushToStream();
									list.Add(UnitySerializationUtility.GetStringFromStreamAndReset(cache2.Value.MemoryStream));
								}
								else if (prefabModification.ModificationType == PrefabModificationType.Value)
								{
									value.MarkJustStarted();
									value.WriteString("path", prefabModification.Path);
									if (prefabModification.ReferencePaths != null && prefabModification.ReferencePaths.Count > 0)
									{
										value.BeginStructNode("references", null);
										for (int j = 0; j < prefabModification.ReferencePaths.Count; j++)
										{
											value.WriteString(null, prefabModification.ReferencePaths[j]);
										}
										value.EndNode("references");
									}
									Serializer<object> serializer = Serializer.Get<object>();
									serializer.WriteValueWeak("value", prefabModification.ModifiedValue, value);
									value.FlushToStream();
									list.Add(UnitySerializationUtility.GetStringFromStreamAndReset(cache2.Value.MemoryStream));
								}
								else if (prefabModification.ModificationType == PrefabModificationType.Dictionary)
								{
									value.MarkJustStarted();
									value.WriteString("path", prefabModification.Path);
									Serializer.Get<object[]>().WriteValue("add_keys", prefabModification.DictionaryKeysAdded, value);
									Serializer.Get<object[]>().WriteValue("remove_keys", prefabModification.DictionaryKeysRemoved, value);
									value.FlushToStream();
									list.Add(UnitySerializationUtility.GetStringFromStreamAndReset(cache2.Value.MemoryStream));
								}
								value.Context.ResetInternalReferences();
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00022FD8 File Offset: 0x000211D8
		private static string GetStringFromStreamAndReset(Stream stream)
		{
			byte[] array = new byte[stream.Position];
			stream.Position = 0L;
			stream.Read(array, 0, array.Length);
			stream.Position = 0L;
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0002301C File Offset: 0x0002121C
		public static List<PrefabModification> DeserializePrefabModifications(List<string> modifications, List<Object> referencedUnityObjects)
		{
			if (modifications == null || modifications.Count == 0)
			{
				return new List<PrefabModification>();
			}
			List<PrefabModification> list = new List<PrefabModification>();
			int num = 0;
			for (int i = 0; i < modifications.Count; i++)
			{
				int num2 = modifications[i].Length * 2;
				if (num2 > num)
				{
					num = num2;
				}
			}
			using (Cache<DeserializationContext> cache = Cache<DeserializationContext>.Claim())
			{
				using (Cache<CachedMemoryStream> cache2 = CachedMemoryStream.Claim(num))
				{
					using (Cache<JsonDataReader> cache3 = Cache<JsonDataReader>.Claim())
					{
						using (Cache<UnityReferenceResolver> cache4 = Cache<UnityReferenceResolver>.Claim())
						{
							MemoryStream memoryStream = cache2.Value.MemoryStream;
							JsonDataReader value = cache3.Value;
							value.Context = cache;
							value.Stream = memoryStream;
							cache4.Value.SetReferencedUnityObjects(referencedUnityObjects);
							value.Context.IndexReferenceResolver = cache4.Value;
							for (int j = 0; j < modifications.Count; j++)
							{
								string text = modifications[j];
								byte[] bytes = Encoding.UTF8.GetBytes(text);
								memoryStream.SetLength((long)bytes.Length);
								memoryStream.Position = 0L;
								memoryStream.Write(bytes, 0, bytes.Length);
								memoryStream.Position = 0L;
								PrefabModification prefabModification = new PrefabModification();
								value.PrepareNewSerializationSession();
								string text2;
								EntryType entryType = value.PeekEntry(out text2);
								if (entryType == EntryType.EndOfStream)
								{
									value.SkipEntry();
								}
								while ((entryType = value.PeekEntry(out text2)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
								{
									if (text2 == null)
									{
										Debug.LogError("Unexpected entry of type " + entryType.ToString() + " without a name.");
										value.SkipEntry();
									}
									else if (text2.Equals("path", 3))
									{
										value.ReadString(out prefabModification.Path);
									}
									else if (text2.Equals("length", 3))
									{
										value.ReadInt32(out prefabModification.NewLength);
										prefabModification.ModificationType = PrefabModificationType.ListLength;
									}
									else if (text2.Equals("references", 3))
									{
										prefabModification.ReferencePaths = new List<string>();
										Type type;
										value.EnterNode(out type);
										while (value.PeekEntry(out text2) == EntryType.String)
										{
											string text3;
											value.ReadString(out text3);
											prefabModification.ReferencePaths.Add(text3);
										}
										value.ExitNode();
									}
									else if (text2.Equals("value", 3))
									{
										prefabModification.ModifiedValue = Serializer.Get<object>().ReadValue(value);
										prefabModification.ModificationType = PrefabModificationType.Value;
									}
									else if (text2.Equals("add_keys", 3))
									{
										prefabModification.DictionaryKeysAdded = Serializer.Get<object[]>().ReadValue(value);
										prefabModification.ModificationType = PrefabModificationType.Dictionary;
									}
									else if (text2.Equals("remove_keys", 3))
									{
										prefabModification.DictionaryKeysRemoved = Serializer.Get<object[]>().ReadValue(value);
										prefabModification.ModificationType = PrefabModificationType.Dictionary;
									}
									else
									{
										Debug.LogError("Unexpected entry name '" + text2 + "' while deserializing prefab modifications.");
										value.SkipEntry();
									}
								}
								if (prefabModification.Path != null)
								{
									list.Add(prefabModification);
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000233A0 File Offset: 0x000215A0
		public static object CreateDefaultUnityInitializedObject(Type type)
		{
			return UnitySerializationUtility.CreateDefaultUnityInitializedObject(type, 0);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000233AC File Offset: 0x000215AC
		private static object CreateDefaultUnityInitializedObject(Type type, int depth)
		{
			if (depth > 5)
			{
				return null;
			}
			if (!UnitySerializationUtility.GuessIfUnityWillSerialize(type))
			{
				if (!type.IsValueType)
				{
					return null;
				}
				return Activator.CreateInstance(type);
			}
			else
			{
				if (type == typeof(string))
				{
					return "";
				}
				if (type.IsEnum)
				{
					Array values = Enum.GetValues(type);
					if (values.Length <= 0)
					{
						return Enum.ToObject(type, 0);
					}
					return values.GetValue(0);
				}
				else
				{
					if (type.IsPrimitive)
					{
						return Activator.CreateInstance(type);
					}
					if (type.IsArray)
					{
						return Array.CreateInstance(type.GetElementType(), 0);
					}
					if (type.ImplementsOpenGenericClass(typeof(List)) || typeof(UnityEventBase).IsAssignableFrom(type))
					{
						try
						{
							return Activator.CreateInstance(type);
						}
						catch
						{
							return null;
						}
					}
					if (typeof(Object).IsAssignableFrom(type))
					{
						return null;
					}
					if ((type.Assembly.GetName().Name.StartsWith("UnityEngine") || type.Assembly.GetName().Name.StartsWith("UnityEditor")) && type.GetConstructor(Type.EmptyTypes) != null)
					{
						try
						{
							return Activator.CreateInstance(type);
						}
						catch (Exception exception)
						{
							Debug.LogException(exception);
							return null;
						}
					}
					if (type.GetConstructor(Type.EmptyTypes) != null)
					{
						return Activator.CreateInstance(type);
					}
					object uninitializedObject = FormatterServices.GetUninitializedObject(type);
					foreach (FieldInfo fieldInfo in type.GetFields(52))
					{
						if (UnitySerializationUtility.GuessIfUnityWillSerialize(fieldInfo))
						{
							fieldInfo.SetValue(uninitializedObject, UnitySerializationUtility.CreateDefaultUnityInitializedObject(fieldInfo.FieldType, depth + 1));
						}
					}
					return uninitializedObject;
				}
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0002356C File Offset: 0x0002176C
		private static void ApplyPrefabModifications(Object unityObject, List<string> modificationData, List<Object> referencedUnityObjects)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException("unityObject");
			}
			if (modificationData == null || modificationData.Count == 0)
			{
				return;
			}
			List<PrefabModification> list = UnitySerializationUtility.DeserializePrefabModifications(modificationData, referencedUnityObjects);
			for (int i = 0; i < list.Count; i++)
			{
				PrefabModification prefabModification = list[i];
				try
				{
					prefabModification.Apply(unityObject);
				}
				catch (Exception exception)
				{
					Debug.Log("The following exception was thrown when trying to apply a prefab modification for path '" + prefabModification.Path + "':");
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000235F8 File Offset: 0x000217F8
		private static Sirenix.Serialization.Utilities.WeakValueGetter GetCachedUnityMemberGetter(MemberInfo member)
		{
			Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueGetter> unityMemberGetters = UnitySerializationUtility.UnityMemberGetters;
			Sirenix.Serialization.Utilities.WeakValueGetter result;
			lock (unityMemberGetters)
			{
				Sirenix.Serialization.Utilities.WeakValueGetter weakValueGetter;
				if (!UnitySerializationUtility.UnityMemberGetters.TryGetValue(member, ref weakValueGetter))
				{
					if (member is FieldInfo)
					{
						weakValueGetter = Sirenix.Serialization.Utilities.EmitUtilities.CreateWeakInstanceFieldGetter(member.DeclaringType, member as FieldInfo);
					}
					else if (member is PropertyInfo)
					{
						weakValueGetter = Sirenix.Serialization.Utilities.EmitUtilities.CreateWeakInstancePropertyGetter(member.DeclaringType, member as PropertyInfo);
					}
					else
					{
						weakValueGetter = delegate(ref object instance)
						{
							return FormatterUtilities.GetMemberValue(member, instance);
						};
					}
					UnitySerializationUtility.UnityMemberGetters.Add(member, weakValueGetter);
				}
				result = weakValueGetter;
			}
			return result;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000236CC File Offset: 0x000218CC
		private static Sirenix.Serialization.Utilities.WeakValueSetter GetCachedUnityMemberSetter(MemberInfo member)
		{
			Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueSetter> unityMemberSetters = UnitySerializationUtility.UnityMemberSetters;
			Sirenix.Serialization.Utilities.WeakValueSetter result;
			lock (unityMemberSetters)
			{
				Sirenix.Serialization.Utilities.WeakValueSetter weakValueSetter;
				if (!UnitySerializationUtility.UnityMemberSetters.TryGetValue(member, ref weakValueSetter))
				{
					if (member is FieldInfo)
					{
						weakValueSetter = Sirenix.Serialization.Utilities.EmitUtilities.CreateWeakInstanceFieldSetter(member.DeclaringType, member as FieldInfo);
					}
					else if (member is PropertyInfo)
					{
						weakValueSetter = Sirenix.Serialization.Utilities.EmitUtilities.CreateWeakInstancePropertySetter(member.DeclaringType, member as PropertyInfo);
					}
					else
					{
						weakValueSetter = delegate(ref object instance, object value)
						{
							FormatterUtilities.SetMemberValue(member, instance, value);
						};
					}
					UnitySerializationUtility.UnityMemberSetters.Add(member, weakValueSetter);
				}
				result = weakValueSetter;
			}
			return result;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000237A0 File Offset: 0x000219A0
		private static ICache GetCachedUnityWriter(DataFormat format, Stream stream, SerializationContext context)
		{
			ICache cache2;
			switch (format)
			{
			case 0:
			{
				Cache<BinaryDataWriter> cache = Cache<BinaryDataWriter>.Claim();
				cache.Value.Stream = stream;
				cache2 = cache;
				break;
			}
			case 1:
			{
				Cache<JsonDataWriter> cache3 = Cache<JsonDataWriter>.Claim();
				cache3.Value.Stream = stream;
				cache2 = cache3;
				break;
			}
			case 2:
				throw new InvalidOperationException("Don't do this for nodes!");
			default:
				throw new NotImplementedException(format.ToString());
			}
			(cache2.Value as IDataWriter).Context = context;
			return cache2;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00023820 File Offset: 0x00021A20
		private static ICache GetCachedUnityReader(DataFormat format, Stream stream, DeserializationContext context)
		{
			ICache cache2;
			switch (format)
			{
			case 0:
			{
				Cache<BinaryDataReader> cache = Cache<BinaryDataReader>.Claim();
				cache.Value.Stream = stream;
				cache2 = cache;
				break;
			}
			case 1:
			{
				Cache<JsonDataReader> cache3 = Cache<JsonDataReader>.Claim();
				cache3.Value.Stream = stream;
				cache2 = cache3;
				break;
			}
			case 2:
				throw new InvalidOperationException("Don't do this for nodes!");
			default:
				throw new NotImplementedException(format.ToString());
			}
			(cache2.Value as IDataReader).Context = context;
			return cache2;
		}

		// Token: 0x040001C8 RID: 456
		public static readonly Type SerializeReferenceAttributeType = typeof(SerializeField).Assembly.GetType("UnityEngine.SerializeReference");

		// Token: 0x040001C9 RID: 457
		private static readonly Assembly String_Assembly = typeof(string).Assembly;

		// Token: 0x040001CA RID: 458
		private static readonly Assembly HashSet_Assembly = typeof(HashSet<>).Assembly;

		// Token: 0x040001CB RID: 459
		private static readonly Assembly LinkedList_Assembly = typeof(LinkedList<>).Assembly;

		// Token: 0x040001CC RID: 460
		private static readonly Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueGetter> UnityMemberGetters = new Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueGetter>();

		// Token: 0x040001CD RID: 461
		private static readonly Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueSetter> UnityMemberSetters = new Dictionary<MemberInfo, Sirenix.Serialization.Utilities.WeakValueSetter>();

		// Token: 0x040001CE RID: 462
		private static readonly Dictionary<MemberInfo, bool> UnityWillSerializeMembersCache = new Dictionary<MemberInfo, bool>();

		// Token: 0x040001CF RID: 463
		private static readonly Dictionary<Type, bool> UnityWillSerializeTypesCache = new Dictionary<Type, bool>();

		// Token: 0x040001D0 RID: 464
		private static readonly HashSet<Type> UnityNeverSerializesTypes = new HashSet<Type>
		{
			typeof(Coroutine)
		};

		// Token: 0x040001D1 RID: 465
		private static readonly HashSet<string> UnityNeverSerializesTypeNames = new HashSet<string>
		{
			"UnityEngine.AnimationState"
		};

		// Token: 0x040001D2 RID: 466
		private static readonly ISerializationPolicy UnityPolicy = SerializationPolicies.Unity;

		// Token: 0x040001D3 RID: 467
		private static readonly ISerializationPolicy EverythingPolicy = SerializationPolicies.Everything;

		// Token: 0x040001D4 RID: 468
		private static readonly ISerializationPolicy StrictPolicy = SerializationPolicies.Strict;

		// Token: 0x040001D5 RID: 469
		private static readonly Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult> OdinWillSerializeCache_UnityPolicy = new Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<MemberInfo>.Default);

		// Token: 0x040001D6 RID: 470
		private static readonly Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult> OdinWillSerializeCache_EverythingPolicy = new Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<MemberInfo>.Default);

		// Token: 0x040001D7 RID: 471
		private static readonly Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult> OdinWillSerializeCache_StrictPolicy = new Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<MemberInfo>.Default);

		// Token: 0x040001D8 RID: 472
		private static readonly Dictionary<ISerializationPolicy, Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult>> OdinWillSerializeCache_CustomPolicies = new Dictionary<ISerializationPolicy, Dictionary<MemberInfo, UnitySerializationUtility.CachedSerializationBackendResult>>(Sirenix.Serialization.Utilities.ReferenceEqualityComparer<ISerializationPolicy>.Default);

		// Token: 0x0200010E RID: 270
		private struct CachedSerializationBackendResult
		{
			// Token: 0x040002DE RID: 734
			public bool HasCalculatedSerializeUnityFieldsTrueResult;

			// Token: 0x040002DF RID: 735
			public bool HasCalculatedSerializeUnityFieldsFalseResult;

			// Token: 0x040002E0 RID: 736
			public bool SerializeUnityFieldsTrueResult;

			// Token: 0x040002E1 RID: 737
			public bool SerializeUnityFieldsFalseResult;
		}
	}
}
