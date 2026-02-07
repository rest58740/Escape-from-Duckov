using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sirenix.Serialization
{
	// Token: 0x02000050 RID: 80
	public sealed class WeakSerializableFormatter : WeakBaseFormatter
	{
		// Token: 0x06000301 RID: 769 RVA: 0x00015FC0 File Offset: 0x000141C0
		public WeakSerializableFormatter(Type serializedType) : base(serializedType)
		{
			WeakSerializableFormatter <>4__this = this;
			Type type = serializedType;
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
				this.ISerializableConstructor = delegate(SerializationInfo info, StreamingContext context)
				{
					ISerializable serializable = (ISerializable)FormatterServices.GetUninitializedObject(<>4__this.SerializedType);
					constructor.Invoke(serializable, new object[]
					{
						info,
						context
					});
					return serializable;
				};
				return;
			}
			DefaultLoggers.DefaultLogger.LogWarning(string.Concat(new string[]
			{
				"Type ",
				this.SerializedType.Name,
				" implements the interface ISerializable but does not implement the required constructor with signature ",
				this.SerializedType.Name,
				"(SerializationInfo info, StreamingContext context). The interface declaration will be ignored, and the formatter fallbacks to reflection."
			}));
			this.ReflectionFormatter = new WeakReflectionFormatter(this.SerializedType);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000160C8 File Offset: 0x000142C8
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			if (this.ISerializableConstructor != null)
			{
				SerializationInfo serializationInfo = this.ReadSerializationInfo(reader);
				if (serializationInfo == null)
				{
					return;
				}
				try
				{
					value = this.ISerializableConstructor.Invoke(serializationInfo, reader.Context.StreamingContext);
					base.InvokeOnDeserializingCallbacks(value, reader.Context);
					if (!this.IsValueType)
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
			value = this.ReflectionFormatter.Deserialize(reader);
			base.InvokeOnDeserializingCallbacks(value, reader.Context);
			if (!this.IsValueType)
			{
				base.RegisterReferenceID(value, reader);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0001617C File Offset: 0x0001437C
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			if (this.ISerializableConstructor != null)
			{
				ISerializable serializable = value as ISerializable;
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
			this.ReflectionFormatter.Serialize(value, writer);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00016208 File Offset: 0x00014408
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
					SerializationInfo serializationInfo = new SerializationInfo(this.SerializedType, reader.Context.FormatterConverter);
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

		// Token: 0x06000306 RID: 774 RVA: 0x00016300 File Offset: 0x00014500
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

		// Token: 0x040000F1 RID: 241
		private readonly Func<SerializationInfo, StreamingContext, ISerializable> ISerializableConstructor;

		// Token: 0x040000F2 RID: 242
		private readonly WeakReflectionFormatter ReflectionFormatter;
	}
}
