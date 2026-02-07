using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000079 RID: 121
	public static class SerializationUtility
	{
		// Token: 0x060003EA RID: 1002 RVA: 0x0001ADB4 File Offset: 0x00018FB4
		public static IDataWriter CreateWriter(Stream stream, SerializationContext context, DataFormat format)
		{
			switch (format)
			{
			case 0:
				return new BinaryDataWriter(stream, context);
			case 1:
				return new JsonDataWriter(stream, context, true);
			case 2:
				Debug.LogError("Cannot automatically create a writer for the format '" + 2.ToString() + "', because it does not use a stream.");
				return null;
			default:
				throw new NotImplementedException(format.ToString());
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001AE20 File Offset: 0x00019020
		public static IDataReader CreateReader(Stream stream, DeserializationContext context, DataFormat format)
		{
			switch (format)
			{
			case 0:
				return new BinaryDataReader(stream, context);
			case 1:
				return new JsonDataReader(stream, context);
			case 2:
				Debug.LogError("Cannot automatically create a reader for the format '" + 2.ToString() + "', because it does not use a stream.");
				return null;
			default:
				throw new NotImplementedException(format.ToString());
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001AE88 File Offset: 0x00019088
		private static IDataWriter GetCachedWriter(out IDisposable cache, DataFormat format, Stream stream, SerializationContext context)
		{
			IDataWriter result;
			if (format == null)
			{
				Cache<BinaryDataWriter> cache2 = Cache<BinaryDataWriter>.Claim();
				BinaryDataWriter value = cache2.Value;
				value.Stream = stream;
				value.Context = context;
				value.PrepareNewSerializationSession();
				result = value;
				cache = cache2;
			}
			else if (format == 1)
			{
				Cache<JsonDataWriter> cache3 = Cache<JsonDataWriter>.Claim();
				JsonDataWriter value2 = cache3.Value;
				value2.Stream = stream;
				value2.Context = context;
				value2.PrepareNewSerializationSession();
				result = value2;
				cache = cache3;
			}
			else
			{
				if (format == 2)
				{
					throw new InvalidOperationException("Cannot automatically create a writer for the format '" + 2.ToString() + "', because it does not use a stream.");
				}
				throw new NotImplementedException(format.ToString());
			}
			return result;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001AF30 File Offset: 0x00019130
		private static IDataReader GetCachedReader(out IDisposable cache, DataFormat format, Stream stream, DeserializationContext context)
		{
			IDataReader result;
			if (format == null)
			{
				Cache<BinaryDataReader> cache2 = Cache<BinaryDataReader>.Claim();
				BinaryDataReader value = cache2.Value;
				value.Stream = stream;
				value.Context = context;
				value.PrepareNewSerializationSession();
				result = value;
				cache = cache2;
			}
			else if (format == 1)
			{
				Cache<JsonDataReader> cache3 = Cache<JsonDataReader>.Claim();
				JsonDataReader value2 = cache3.Value;
				value2.Stream = stream;
				value2.Context = context;
				value2.PrepareNewSerializationSession();
				result = value2;
				cache = cache3;
			}
			else
			{
				if (format == 2)
				{
					throw new InvalidOperationException("Cannot automatically create a reader for the format '" + 2.ToString() + "', because it does not use a stream.");
				}
				throw new NotImplementedException(format.ToString());
			}
			return result;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001AFD6 File Offset: 0x000191D6
		public static void SerializeValueWeak(object value, IDataWriter writer)
		{
			Serializer.GetForValue(value).WriteValueWeak(value, writer);
			writer.FlushToStream();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001AFEC File Offset: 0x000191EC
		public static void SerializeValueWeak(object value, IDataWriter writer, out List<Object> unityObjects)
		{
			using (Cache<UnityReferenceResolver> cache = Cache<UnityReferenceResolver>.Claim())
			{
				writer.Context.IndexReferenceResolver = cache.Value;
				Serializer.GetForValue(value).WriteValueWeak(value, writer);
				writer.FlushToStream();
				unityObjects = cache.Value.GetReferencedUnityObjects();
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001B04C File Offset: 0x0001924C
		public static void SerializeValue<T>(T value, IDataWriter writer)
		{
			if (EmitUtilities.CanEmit)
			{
				Serializer.Get<T>().WriteValue(value, writer);
			}
			else
			{
				Serializer serializer = Serializer.Get(typeof(T));
				Serializer<T> serializer2 = serializer as Serializer<T>;
				if (serializer2 != null)
				{
					serializer2.WriteValue(value, writer);
				}
				else
				{
					serializer.WriteValueWeak(value, writer);
				}
			}
			writer.FlushToStream();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001B0A8 File Offset: 0x000192A8
		public static void SerializeValue<T>(T value, IDataWriter writer, out List<Object> unityObjects)
		{
			using (Cache<UnityReferenceResolver> cache = Cache<UnityReferenceResolver>.Claim())
			{
				writer.Context.IndexReferenceResolver = cache.Value;
				if (EmitUtilities.CanEmit)
				{
					Serializer.Get<T>().WriteValue(value, writer);
				}
				else
				{
					Serializer serializer = Serializer.Get(typeof(T));
					Serializer<T> serializer2 = serializer as Serializer<T>;
					if (serializer2 != null)
					{
						serializer2.WriteValue(value, writer);
					}
					else
					{
						serializer.WriteValueWeak(value, writer);
					}
				}
				writer.FlushToStream();
				unityObjects = cache.Value.GetReferencedUnityObjects();
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001B144 File Offset: 0x00019344
		public static void SerializeValueWeak(object value, Stream stream, DataFormat format, SerializationContext context = null)
		{
			IDisposable disposable;
			IDataWriter cachedWriter = SerializationUtility.GetCachedWriter(out disposable, format, stream, context);
			try
			{
				if (context != null)
				{
					SerializationUtility.SerializeValueWeak(value, cachedWriter);
				}
				else
				{
					using (Cache<SerializationContext> cache = Cache<SerializationContext>.Claim())
					{
						cachedWriter.Context = cache;
						SerializationUtility.SerializeValueWeak(value, cachedWriter);
					}
				}
			}
			finally
			{
				disposable.Dispose();
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001B1B0 File Offset: 0x000193B0
		public static void SerializeValueWeak(object value, Stream stream, DataFormat format, out List<Object> unityObjects, SerializationContext context = null)
		{
			IDisposable disposable;
			IDataWriter cachedWriter = SerializationUtility.GetCachedWriter(out disposable, format, stream, context);
			try
			{
				if (context != null)
				{
					SerializationUtility.SerializeValueWeak(value, cachedWriter, out unityObjects);
				}
				else
				{
					using (Cache<SerializationContext> cache = Cache<SerializationContext>.Claim())
					{
						cachedWriter.Context = cache;
						SerializationUtility.SerializeValueWeak(value, cachedWriter, out unityObjects);
					}
				}
			}
			finally
			{
				disposable.Dispose();
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001B220 File Offset: 0x00019420
		public static void SerializeValue<T>(T value, Stream stream, DataFormat format, SerializationContext context = null)
		{
			IDisposable disposable;
			IDataWriter cachedWriter = SerializationUtility.GetCachedWriter(out disposable, format, stream, context);
			try
			{
				if (context != null)
				{
					SerializationUtility.SerializeValue<T>(value, cachedWriter);
				}
				else
				{
					using (Cache<SerializationContext> cache = Cache<SerializationContext>.Claim())
					{
						cachedWriter.Context = cache;
						SerializationUtility.SerializeValue<T>(value, cachedWriter);
					}
				}
			}
			finally
			{
				disposable.Dispose();
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001B28C File Offset: 0x0001948C
		public static void SerializeValue<T>(T value, Stream stream, DataFormat format, out List<Object> unityObjects, SerializationContext context = null)
		{
			IDisposable disposable;
			IDataWriter cachedWriter = SerializationUtility.GetCachedWriter(out disposable, format, stream, context);
			try
			{
				if (context != null)
				{
					SerializationUtility.SerializeValue<T>(value, cachedWriter, out unityObjects);
				}
				else
				{
					using (Cache<SerializationContext> cache = Cache<SerializationContext>.Claim())
					{
						cachedWriter.Context = cache;
						SerializationUtility.SerializeValue<T>(value, cachedWriter, out unityObjects);
					}
				}
			}
			finally
			{
				disposable.Dispose();
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001B2FC File Offset: 0x000194FC
		public static byte[] SerializeValueWeak(object value, DataFormat format, SerializationContext context = null)
		{
			byte[] result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(null))
			{
				SerializationUtility.SerializeValueWeak(value, cache.Value.MemoryStream, format, context);
				result = cache.Value.MemoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001B354 File Offset: 0x00019554
		public static byte[] SerializeValueWeak(object value, DataFormat format, out List<Object> unityObjects)
		{
			byte[] result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(null))
			{
				SerializationUtility.SerializeValueWeak(value, cache.Value.MemoryStream, format, out unityObjects, null);
				result = cache.Value.MemoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001B3AC File Offset: 0x000195AC
		public static byte[] SerializeValue<T>(T value, DataFormat format, SerializationContext context = null)
		{
			byte[] result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(null))
			{
				SerializationUtility.SerializeValue<T>(value, cache.Value.MemoryStream, format, context);
				result = cache.Value.MemoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001B404 File Offset: 0x00019604
		public static byte[] SerializeValue<T>(T value, DataFormat format, out List<Object> unityObjects, SerializationContext context = null)
		{
			byte[] result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(null))
			{
				SerializationUtility.SerializeValue<T>(value, cache.Value.MemoryStream, format, out unityObjects, context);
				result = cache.Value.MemoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001B45C File Offset: 0x0001965C
		public static object DeserializeValueWeak(IDataReader reader)
		{
			return Serializer.Get(typeof(object)).ReadValueWeak(reader);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001B474 File Offset: 0x00019674
		public static object DeserializeValueWeak(IDataReader reader, List<Object> referencedUnityObjects)
		{
			object result;
			using (Cache<UnityReferenceResolver> cache = Cache<UnityReferenceResolver>.Claim())
			{
				cache.Value.SetReferencedUnityObjects(referencedUnityObjects);
				reader.Context.IndexReferenceResolver = cache.Value;
				result = Serializer.Get(typeof(object)).ReadValueWeak(reader);
			}
			return result;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001B4D8 File Offset: 0x000196D8
		public static T DeserializeValue<T>(IDataReader reader)
		{
			if (EmitUtilities.CanEmit)
			{
				return Serializer.Get<T>().ReadValue(reader);
			}
			Serializer serializer = Serializer.Get(typeof(T));
			Serializer<T> serializer2 = serializer as Serializer<T>;
			if (serializer2 != null)
			{
				return serializer2.ReadValue(reader);
			}
			return (T)((object)serializer.ReadValueWeak(reader));
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001B528 File Offset: 0x00019728
		public static T DeserializeValue<T>(IDataReader reader, List<Object> referencedUnityObjects)
		{
			T result;
			using (Cache<UnityReferenceResolver> cache = Cache<UnityReferenceResolver>.Claim())
			{
				cache.Value.SetReferencedUnityObjects(referencedUnityObjects);
				reader.Context.IndexReferenceResolver = cache.Value;
				if (EmitUtilities.CanEmit)
				{
					result = Serializer.Get<T>().ReadValue(reader);
				}
				else
				{
					Serializer serializer = Serializer.Get(typeof(T));
					Serializer<T> serializer2 = serializer as Serializer<T>;
					if (serializer2 != null)
					{
						result = serializer2.ReadValue(reader);
					}
					else
					{
						result = (T)((object)serializer.ReadValueWeak(reader));
					}
				}
			}
			return result;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001B5BC File Offset: 0x000197BC
		public static object DeserializeValueWeak(Stream stream, DataFormat format, DeserializationContext context = null)
		{
			IDisposable disposable;
			IDataReader cachedReader = SerializationUtility.GetCachedReader(out disposable, format, stream, context);
			object result;
			try
			{
				if (context != null)
				{
					result = SerializationUtility.DeserializeValueWeak(cachedReader);
				}
				else
				{
					using (Cache<DeserializationContext> cache = Cache<DeserializationContext>.Claim())
					{
						cachedReader.Context = cache;
						result = SerializationUtility.DeserializeValueWeak(cachedReader);
					}
				}
			}
			finally
			{
				disposable.Dispose();
			}
			return result;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001B62C File Offset: 0x0001982C
		public static object DeserializeValueWeak(Stream stream, DataFormat format, List<Object> referencedUnityObjects, DeserializationContext context = null)
		{
			IDisposable disposable;
			IDataReader cachedReader = SerializationUtility.GetCachedReader(out disposable, format, stream, context);
			object result;
			try
			{
				if (context != null)
				{
					result = SerializationUtility.DeserializeValueWeak(cachedReader, referencedUnityObjects);
				}
				else
				{
					using (Cache<DeserializationContext> cache = Cache<DeserializationContext>.Claim())
					{
						cachedReader.Context = cache;
						result = SerializationUtility.DeserializeValueWeak(cachedReader, referencedUnityObjects);
					}
				}
			}
			finally
			{
				disposable.Dispose();
			}
			return result;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001B69C File Offset: 0x0001989C
		public static T DeserializeValue<T>(Stream stream, DataFormat format, DeserializationContext context = null)
		{
			IDisposable disposable;
			IDataReader cachedReader = SerializationUtility.GetCachedReader(out disposable, format, stream, context);
			T result;
			try
			{
				if (context != null)
				{
					result = SerializationUtility.DeserializeValue<T>(cachedReader);
				}
				else
				{
					using (Cache<DeserializationContext> cache = Cache<DeserializationContext>.Claim())
					{
						cachedReader.Context = cache;
						result = SerializationUtility.DeserializeValue<T>(cachedReader);
					}
				}
			}
			finally
			{
				disposable.Dispose();
			}
			return result;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001B70C File Offset: 0x0001990C
		public static T DeserializeValue<T>(Stream stream, DataFormat format, List<Object> referencedUnityObjects, DeserializationContext context = null)
		{
			IDisposable disposable;
			IDataReader cachedReader = SerializationUtility.GetCachedReader(out disposable, format, stream, context);
			T result;
			try
			{
				if (context != null)
				{
					result = SerializationUtility.DeserializeValue<T>(cachedReader, referencedUnityObjects);
				}
				else
				{
					using (Cache<DeserializationContext> cache = Cache<DeserializationContext>.Claim())
					{
						cachedReader.Context = cache;
						result = SerializationUtility.DeserializeValue<T>(cachedReader, referencedUnityObjects);
					}
				}
			}
			finally
			{
				disposable.Dispose();
			}
			return result;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001B77C File Offset: 0x0001997C
		public static object DeserializeValueWeak(byte[] bytes, DataFormat format, DeserializationContext context = null)
		{
			object result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(bytes))
			{
				result = SerializationUtility.DeserializeValueWeak(cache.Value.MemoryStream, format, context);
			}
			return result;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001B7C0 File Offset: 0x000199C0
		public static object DeserializeValueWeak(byte[] bytes, DataFormat format, List<Object> referencedUnityObjects)
		{
			object result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(bytes))
			{
				result = SerializationUtility.DeserializeValueWeak(cache.Value.MemoryStream, format, referencedUnityObjects, null);
			}
			return result;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001B808 File Offset: 0x00019A08
		public static T DeserializeValue<T>(byte[] bytes, DataFormat format, DeserializationContext context = null)
		{
			T result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(bytes))
			{
				result = SerializationUtility.DeserializeValue<T>(cache.Value.MemoryStream, format, context);
			}
			return result;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001B84C File Offset: 0x00019A4C
		public static T DeserializeValue<T>(byte[] bytes, DataFormat format, List<Object> referencedUnityObjects, DeserializationContext context = null)
		{
			T result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(bytes))
			{
				result = SerializationUtility.DeserializeValue<T>(cache.Value.MemoryStream, format, referencedUnityObjects, context);
			}
			return result;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001B894 File Offset: 0x00019A94
		public static object CreateCopy(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is string)
			{
				return obj;
			}
			Type type = obj.GetType();
			if (type.IsValueType)
			{
				return obj;
			}
			if (type.InheritsFrom(typeof(Object)) || type.InheritsFrom(typeof(MemberInfo)) || type.InheritsFrom(typeof(Assembly)) || type.InheritsFrom(typeof(Module)))
			{
				return obj;
			}
			object result;
			using (Cache<CachedMemoryStream> cache = CachedMemoryStream.Claim(null))
			{
				using (Cache<SerializationContext> cache2 = Cache<SerializationContext>.Claim())
				{
					using (Cache<DeserializationContext> cache3 = Cache<DeserializationContext>.Claim())
					{
						cache2.Value.Config.SerializationPolicy = SerializationPolicies.Everything;
						cache3.Value.Config.SerializationPolicy = SerializationPolicies.Everything;
						List<Object> referencedUnityObjects;
						SerializationUtility.SerializeValue<object>(obj, cache.Value.MemoryStream, 0, out referencedUnityObjects, cache2);
						cache.Value.MemoryStream.Position = 0L;
						result = SerializationUtility.DeserializeValue<object>(cache.Value.MemoryStream, 0, referencedUnityObjects, cache3);
					}
				}
			}
			return result;
		}
	}
}
