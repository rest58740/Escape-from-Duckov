using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sirenix.Serialization
{
	// Token: 0x0200004F RID: 79
	public sealed class SerializableFormatter<T> : BaseFormatter<T> where T : ISerializable
	{
		// Token: 0x060002FA RID: 762 RVA: 0x00015B88 File Offset: 0x00013D88
		static SerializableFormatter()
		{
			Type type = typeof(T);
			ConstructorInfo constructor = null;
			do
			{
				constructor = type.GetConstructor(52, null, new Type[]
				{
					typeof(SerializationInfo),
					typeof(StreamingContext)
				}, null);
				type = type.BaseType;
			}
			while (constructor == null && type != typeof(object) && type != null);
			if (constructor != null)
			{
				SerializableFormatter<T>.ISerializableConstructor = delegate(SerializationInfo info, StreamingContext context)
				{
					T t = (T)((object)FormatterServices.GetUninitializedObject(typeof(T)));
					constructor.Invoke(t, new object[]
					{
						info,
						context
					});
					return t;
				};
				return;
			}
			DefaultLoggers.DefaultLogger.LogWarning(string.Concat(new string[]
			{
				"Type ",
				typeof(T).Name,
				" implements the interface ISerializable but does not implement the required constructor with signature ",
				typeof(T).Name,
				"(SerializationInfo info, StreamingContext context). The interface declaration will be ignored, and the formatter fallbacks to reflection."
			}));
			SerializableFormatter<T>.ReflectionFormatter = new ReflectionFormatter<T>();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00015C88 File Offset: 0x00013E88
		protected override T GetUninitializedObject()
		{
			return default(T);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00015CA0 File Offset: 0x00013EA0
		protected override void DeserializeImplementation(ref T value, IDataReader reader)
		{
			if (SerializableFormatter<T>.ISerializableConstructor != null)
			{
				SerializationInfo serializationInfo = this.ReadSerializationInfo(reader);
				if (serializationInfo == null)
				{
					return;
				}
				try
				{
					value = SerializableFormatter<T>.ISerializableConstructor.Invoke(serializationInfo, reader.Context.StreamingContext);
					base.InvokeOnDeserializingCallbacks(ref value, reader.Context);
					if (!BaseFormatter<T>.IsValueType)
					{
						base.RegisterReferenceID(value, reader);
					}
					return;
				}
				catch (Exception exception)
				{
					reader.Context.Config.DebugContext.LogException(exception);
					return;
				}
			}
			value = SerializableFormatter<T>.ReflectionFormatter.Deserialize(reader);
			base.InvokeOnDeserializingCallbacks(ref value, reader.Context);
			if (!BaseFormatter<T>.IsValueType)
			{
				base.RegisterReferenceID(value, reader);
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00015D5C File Offset: 0x00013F5C
		protected override void SerializeImplementation(ref T value, IDataWriter writer)
		{
			if (SerializableFormatter<T>.ISerializableConstructor != null)
			{
				ISerializable serializable = value;
				SerializationInfo serializationInfo = new SerializationInfo(value.GetType(), writer.Context.FormatterConverter);
				try
				{
					serializable.GetObjectData(serializationInfo, writer.Context.StreamingContext);
				}
				catch (Exception exception)
				{
					writer.Context.Config.DebugContext.LogException(exception);
				}
				this.WriteSerializationInfo(serializationInfo, writer);
				return;
			}
			SerializableFormatter<T>.ReflectionFormatter.Serialize(value, writer);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00015DF4 File Offset: 0x00013FF4
		private SerializationInfo ReadSerializationInfo(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					SerializationInfo serializationInfo = new SerializationInfo(typeof(T), reader.Context.FormatterConverter);
					int num2 = 0;
					while ((long)num2 < num)
					{
						Type type = null;
						entryType = reader.PeekEntry(out text);
						if (entryType == EntryType.String && text == "type")
						{
							string typeName;
							reader.ReadString(out typeName);
							type = reader.Context.Binder.BindToType(typeName, reader.Context.Config.DebugContext);
						}
						if (type == null)
						{
							reader.SkipEntry();
						}
						else
						{
							entryType = reader.PeekEntry(out text);
							Serializer serializer = Serializer.Get(type);
							object obj = serializer.ReadValueWeak(reader);
							serializationInfo.AddValue(text, obj);
						}
						num2++;
					}
					return serializationInfo;
				}
				finally
				{
					reader.ExitArray();
				}
			}
			return null;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00015EF0 File Offset: 0x000140F0
		private void WriteSerializationInfo(SerializationInfo info, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)info.MemberCount);
				foreach (SerializationEntry serializationEntry in info)
				{
					try
					{
						writer.WriteString("type", writer.Context.Binder.BindToName(serializationEntry.ObjectType, writer.Context.Config.DebugContext));
						Serializer serializer = Serializer.Get(serializationEntry.ObjectType);
						serializer.WriteValueWeak(serializationEntry.Name, serializationEntry.Value, writer);
					}
					catch (Exception exception)
					{
						writer.Context.Config.DebugContext.LogException(exception);
					}
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000EF RID: 239
		private static readonly Func<SerializationInfo, StreamingContext, T> ISerializableConstructor;

		// Token: 0x040000F0 RID: 240
		private static readonly ReflectionFormatter<T> ReflectionFormatter;
	}
}
