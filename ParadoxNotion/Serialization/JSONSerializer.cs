using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Serialization.FullSerializer.Internal;
using ParadoxNotion.Services;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x0200008B RID: 139
	public static class JSONSerializer
	{
		// Token: 0x0600059C RID: 1436 RVA: 0x00010337 File Offset: 0x0000E537
		static JSONSerializer()
		{
			JSONSerializer.FlushMem();
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00010348 File Offset: 0x0000E548
		public static void FlushMem()
		{
			JSONSerializer.serializer = new fsSerializer();
			JSONSerializer.dataCache = new Dictionary<string, fsData>();
			fsMetaType.FlushMem();
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00010363 File Offset: 0x0000E563
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void __FlushDataCache()
		{
			JSONSerializer.dataCache = new Dictionary<string, fsData>();
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00010370 File Offset: 0x0000E570
		public static string Serialize(Type type, object instance, List<Object> references = null, bool pretyJson = false)
		{
			object obj = JSONSerializer.serializerLock;
			string result;
			lock (obj)
			{
				JSONSerializer.serializer.PurgeTemporaryData();
				JSONSerializer.serializer.ReferencesDatabase = references;
				Type overrideConverterType = typeof(Object).RTIsAssignableFrom(type) ? typeof(fsReflectedConverter) : null;
				fsData fsData;
				bool hasWarnings = JSONSerializer.serializer.TrySerialize(type, instance, out fsData, overrideConverterType).AssertSuccess().HasWarnings;
				JSONSerializer.serializer.ReferencesDatabase = null;
				string text = fsJsonPrinter.ToJson(fsData, pretyJson);
				if (Threader.applicationIsPlaying || Application.isPlaying)
				{
					JSONSerializer.dataCache[text] = fsData;
				}
				result = text;
			}
			return result;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00010438 File Offset: 0x0000E638
		public static T Deserialize<T>(string json, List<Object> references = null)
		{
			return (T)((object)JSONSerializer.Internal_Deserialize(typeof(T), json, references, null));
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00010451 File Offset: 0x0000E651
		public static object Deserialize(Type type, string json, List<Object> references = null)
		{
			return JSONSerializer.Internal_Deserialize(type, json, references, null);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001045C File Offset: 0x0000E65C
		public static T TryDeserializeOverwrite<T>(T instance, string json, List<Object> references = null) where T : class
		{
			return (T)((object)JSONSerializer.Internal_Deserialize(typeof(T), json, references, instance));
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001047A File Offset: 0x0000E67A
		public static object TryDeserializeOverwrite(object instance, string json, List<Object> references = null)
		{
			return JSONSerializer.Internal_Deserialize(instance.GetType(), json, references, instance);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001048C File Offset: 0x0000E68C
		private static object Internal_Deserialize(Type type, string json, List<Object> references, object instance)
		{
			object obj = JSONSerializer.serializerLock;
			object result;
			lock (obj)
			{
				JSONSerializer.serializer.PurgeTemporaryData();
				fsData data = null;
				if (Threader.applicationIsPlaying)
				{
					if (!JSONSerializer.dataCache.TryGetValue(json, ref data))
					{
						data = (JSONSerializer.dataCache[json] = fsJsonParser.Parse(json));
					}
				}
				else
				{
					data = fsJsonParser.Parse(json);
				}
				JSONSerializer.serializer.ReferencesDatabase = references;
				Type overrideConverterType = (instance is Object) ? typeof(fsReflectedConverter) : null;
				bool hasWarnings = JSONSerializer.serializer.TryDeserialize(data, type, ref instance, overrideConverterType).AssertSuccess().HasWarnings;
				JSONSerializer.serializer.ReferencesDatabase = null;
				result = instance;
			}
			return result;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00010558 File Offset: 0x0000E758
		public static void SerializeAndExecuteNoCycles(Type type, object instance, Action<object, fsData> call)
		{
			object obj = JSONSerializer.serializerLock;
			lock (obj)
			{
				JSONSerializer.serializer.IgnoreSerializeCycleReferences = true;
				JSONSerializer.serializer.onAfterObjectSerialized += call;
				try
				{
					JSONSerializer.Serialize(type, instance, null, false);
				}
				finally
				{
					JSONSerializer.serializer.IgnoreSerializeCycleReferences = false;
					JSONSerializer.serializer.onAfterObjectSerialized -= call;
				}
			}
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000105D4 File Offset: 0x0000E7D4
		public static void SerializeAndExecuteNoCycles(Type type, object instance, Action<object> beforeCall, Action<object, fsData> afterCall)
		{
			object obj = JSONSerializer.serializerLock;
			lock (obj)
			{
				JSONSerializer.serializer.IgnoreSerializeCycleReferences = true;
				JSONSerializer.serializer.onBeforeObjectSerialized += beforeCall;
				JSONSerializer.serializer.onAfterObjectSerialized += afterCall;
				try
				{
					JSONSerializer.Serialize(type, instance, null, false);
				}
				finally
				{
					JSONSerializer.serializer.IgnoreSerializeCycleReferences = false;
					JSONSerializer.serializer.onBeforeObjectSerialized -= beforeCall;
					JSONSerializer.serializer.onAfterObjectSerialized -= afterCall;
				}
			}
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00010668 File Offset: 0x0000E868
		public static T Clone<T>(T original)
		{
			return (T)((object)JSONSerializer.Clone(original));
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001067C File Offset: 0x0000E87C
		public static object Clone(object original)
		{
			Type type = original.GetType();
			List<Object> references = new List<Object>();
			string json = JSONSerializer.Serialize(type, original, references, false);
			return JSONSerializer.Deserialize(type, json, references);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000106A8 File Offset: 0x0000E8A8
		public static void CopySerialized(object source, object target)
		{
			Type type = source.GetType();
			List<Object> references = new List<Object>();
			string json = JSONSerializer.Serialize(type, source, references, false);
			JSONSerializer.TryDeserializeOverwrite(target, json, references);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000106D4 File Offset: 0x0000E8D4
		public static void ShowData(string json, string fileName = "")
		{
			string text = JSONSerializer.PrettifyJson(json);
			string text2 = Path.GetTempPath() + (string.IsNullOrEmpty(fileName) ? Guid.NewGuid().ToString() : fileName) + ".json";
			File.WriteAllText(text2, text);
			Process.Start(text2);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00010722 File Offset: 0x0000E922
		public static string PrettifyJson(string json)
		{
			return fsJsonPrinter.PrettyJson(fsJsonParser.Parse(json));
		}

		// Token: 0x040001BD RID: 445
		private static object serializerLock = new object();

		// Token: 0x040001BE RID: 446
		private static fsSerializer serializer;

		// Token: 0x040001BF RID: 447
		private static Dictionary<string, fsData> dataCache;
	}
}
