using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000025 RID: 37
	public abstract class WeakBaseFormatter : IFormatter
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
		Type IFormatter.SerializedType
		{
			get
			{
				return this.SerializedType;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public WeakBaseFormatter(Type serializedType)
		{
			this.SerializedType = serializedType;
			this.ImplementsISerializationCallbackReceiver = this.SerializedType.ImplementsOrInherits(typeof(ISerializationCallbackReceiver));
			this.ImplementsIDeserializationCallback = this.SerializedType.ImplementsOrInherits(typeof(IDeserializationCallback));
			this.ImplementsIObjectReference = this.SerializedType.ImplementsOrInherits(typeof(IObjectReference));
			if (this.SerializedType.ImplementsOrInherits(typeof(Object)))
			{
				DefaultLoggers.DefaultLogger.LogWarning("A formatter has been created for the UnityEngine.Object type " + this.SerializedType.Name + " - this is *strongly* discouraged. Unity should be allowed to handle serialization and deserialization of its own weird objects. Remember to serialize with a UnityReferenceResolver as the external index reference resolver in the serialization context.\n\n Stacktrace: " + new StackTrace().ToString());
			}
			MethodInfo[] methods = this.SerializedType.GetMethods(52);
			List<WeakBaseFormatter.SerializationCallback> list = new List<WeakBaseFormatter.SerializationCallback>();
			this.OnSerializingCallbacks = WeakBaseFormatter.GetCallbacks(methods, typeof(OnSerializingAttribute), ref list);
			this.OnSerializedCallbacks = WeakBaseFormatter.GetCallbacks(methods, typeof(OnSerializedAttribute), ref list);
			this.OnDeserializingCallbacks = WeakBaseFormatter.GetCallbacks(methods, typeof(OnDeserializingAttribute), ref list);
			this.OnDeserializedCallbacks = WeakBaseFormatter.GetCallbacks(methods, typeof(OnDeserializedAttribute), ref list);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000F9CC File Offset: 0x0000DBCC
		private static WeakBaseFormatter.SerializationCallback[] GetCallbacks(MethodInfo[] methods, Type callbackAttribute, ref List<WeakBaseFormatter.SerializationCallback> list)
		{
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.IsDefined(callbackAttribute, true))
				{
					WeakBaseFormatter.SerializationCallback serializationCallback = WeakBaseFormatter.CreateCallback(methodInfo);
					if (serializationCallback != null)
					{
						list.Add(serializationCallback);
					}
				}
			}
			WeakBaseFormatter.SerializationCallback[] result = list.ToArray();
			list.Clear();
			return result;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000FA18 File Offset: 0x0000DC18
		private static WeakBaseFormatter.SerializationCallback CreateCallback(MethodInfo info)
		{
			ParameterInfo[] parameters = info.GetParameters();
			if (parameters.Length == 0)
			{
				return delegate(object value, StreamingContext context)
				{
					info.Invoke(value, null);
				};
			}
			if (parameters.Length == 1 && parameters[0].ParameterType == typeof(StreamingContext) && !parameters[0].ParameterType.IsByRef)
			{
				return delegate(object value, StreamingContext context)
				{
					info.Invoke(value, new object[]
					{
						context
					});
				};
			}
			DefaultLoggers.DefaultLogger.LogWarning("The method " + info.GetNiceName() + " has an invalid signature and will be ignored by the serialization system.");
			return null;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000FAB0 File Offset: 0x0000DCB0
		public void Serialize(object value, IDataWriter writer)
		{
			SerializationContext context = writer.Context;
			for (int i = 0; i < this.OnSerializingCallbacks.Length; i++)
			{
				try
				{
					this.OnSerializingCallbacks[i](value, context.StreamingContext);
				}
				catch (Exception exception)
				{
					context.Config.DebugContext.LogException(exception);
				}
			}
			if (this.ImplementsISerializationCallbackReceiver)
			{
				try
				{
					ISerializationCallbackReceiver serializationCallbackReceiver = value as ISerializationCallbackReceiver;
					serializationCallbackReceiver.OnBeforeSerialize();
					value = serializationCallbackReceiver;
				}
				catch (Exception exception2)
				{
					context.Config.DebugContext.LogException(exception2);
				}
			}
			try
			{
				this.SerializeImplementation(ref value, writer);
			}
			catch (Exception exception3)
			{
				context.Config.DebugContext.LogException(exception3);
			}
			for (int j = 0; j < this.OnSerializedCallbacks.Length; j++)
			{
				try
				{
					this.OnSerializedCallbacks[j](value, context.StreamingContext);
				}
				catch (Exception exception4)
				{
					context.Config.DebugContext.LogException(exception4);
				}
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000FBD0 File Offset: 0x0000DDD0
		public object Deserialize(IDataReader reader)
		{
			DeserializationContext context = reader.Context;
			object obj = this.GetUninitializedObject();
			if (this.IsValueType)
			{
				if (obj == null)
				{
					obj = Activator.CreateInstance(this.SerializedType);
				}
				this.InvokeOnDeserializingCallbacks(obj, context);
			}
			else if (obj != null)
			{
				this.RegisterReferenceID(obj, reader);
				this.InvokeOnDeserializingCallbacks(obj, context);
				if (this.ImplementsIObjectReference)
				{
					try
					{
						obj = (obj as IObjectReference).GetRealObject(context.StreamingContext);
						this.RegisterReferenceID(obj, reader);
					}
					catch (Exception exception)
					{
						context.Config.DebugContext.LogException(exception);
					}
				}
			}
			try
			{
				this.DeserializeImplementation(ref obj, reader);
			}
			catch (Exception exception2)
			{
				context.Config.DebugContext.LogException(exception2);
			}
			if (this.IsValueType || obj != null)
			{
				for (int i = 0; i < this.OnDeserializedCallbacks.Length; i++)
				{
					try
					{
						this.OnDeserializedCallbacks[i](obj, context.StreamingContext);
					}
					catch (Exception exception3)
					{
						context.Config.DebugContext.LogException(exception3);
					}
				}
				if (this.ImplementsIDeserializationCallback)
				{
					IDeserializationCallback deserializationCallback = obj as IDeserializationCallback;
					deserializationCallback.OnDeserialization(this);
					obj = deserializationCallback;
				}
				if (this.ImplementsISerializationCallbackReceiver)
				{
					try
					{
						ISerializationCallbackReceiver serializationCallbackReceiver = obj as ISerializationCallbackReceiver;
						serializationCallbackReceiver.OnAfterDeserialize();
						obj = serializationCallbackReceiver;
					}
					catch (Exception exception4)
					{
						context.Config.DebugContext.LogException(exception4);
					}
				}
			}
			return obj;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000FD4C File Offset: 0x0000DF4C
		protected void RegisterReferenceID(object value, IDataReader reader)
		{
			if (!this.IsValueType)
			{
				int currentNodeId = reader.CurrentNodeId;
				if (currentNodeId < 0)
				{
					reader.Context.Config.DebugContext.LogWarning("Reference type node is missing id upon deserialization. Some references may be broken. This tends to happen if a value type has changed to a reference type (IE, struct to class) since serialization took place.");
					return;
				}
				reader.Context.RegisterInternalReference(currentNodeId, value);
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000FD94 File Offset: 0x0000DF94
		protected void InvokeOnDeserializingCallbacks(object value, DeserializationContext context)
		{
			for (int i = 0; i < this.OnDeserializingCallbacks.Length; i++)
			{
				try
				{
					this.OnDeserializingCallbacks[i](value, context.StreamingContext);
				}
				catch (Exception exception)
				{
					context.Config.DebugContext.LogException(exception);
				}
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000FDF0 File Offset: 0x0000DFF0
		protected virtual object GetUninitializedObject()
		{
			if (!this.IsValueType)
			{
				return FormatterServices.GetUninitializedObject(this.SerializedType);
			}
			return Activator.CreateInstance(this.SerializedType);
		}

		// Token: 0x06000245 RID: 581
		protected abstract void DeserializeImplementation(ref object value, IDataReader reader);

		// Token: 0x06000246 RID: 582
		protected abstract void SerializeImplementation(ref object value, IDataWriter writer);

		// Token: 0x04000098 RID: 152
		protected readonly Type SerializedType;

		// Token: 0x04000099 RID: 153
		protected readonly WeakBaseFormatter.SerializationCallback[] OnSerializingCallbacks;

		// Token: 0x0400009A RID: 154
		protected readonly WeakBaseFormatter.SerializationCallback[] OnSerializedCallbacks;

		// Token: 0x0400009B RID: 155
		protected readonly WeakBaseFormatter.SerializationCallback[] OnDeserializingCallbacks;

		// Token: 0x0400009C RID: 156
		protected readonly WeakBaseFormatter.SerializationCallback[] OnDeserializedCallbacks;

		// Token: 0x0400009D RID: 157
		protected readonly bool IsValueType;

		// Token: 0x0400009E RID: 158
		protected readonly bool ImplementsISerializationCallbackReceiver;

		// Token: 0x0400009F RID: 159
		protected readonly bool ImplementsIDeserializationCallback;

		// Token: 0x040000A0 RID: 160
		protected readonly bool ImplementsIObjectReference;

		// Token: 0x020000E7 RID: 231
		// (Invoke) Token: 0x06000685 RID: 1669
		protected delegate void SerializationCallback(object value, StreamingContext context);
	}
}
