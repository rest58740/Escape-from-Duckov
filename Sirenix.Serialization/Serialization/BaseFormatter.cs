using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000024 RID: 36
	public abstract class BaseFormatter<T> : IFormatter<T>, IFormatter
	{
		// Token: 0x0600022D RID: 557 RVA: 0x0000F2A8 File Offset: 0x0000D4A8
		static BaseFormatter()
		{
			if (typeof(T).ImplementsOrInherits(typeof(Object)))
			{
				DefaultLoggers.DefaultLogger.LogWarning("A formatter has been created for the UnityEngine.Object type " + typeof(T).Name + " - this is *strongly* discouraged. Unity should be allowed to handle serialization and deserialization of its own weird objects. Remember to serialize with a UnityReferenceResolver as the external index reference resolver in the serialization context.\n\n Stacktrace: " + new StackTrace().ToString());
			}
			MethodInfo[] methods = typeof(T).GetMethods(52);
			List<BaseFormatter<T>.SerializationCallback> list = new List<BaseFormatter<T>.SerializationCallback>();
			BaseFormatter<T>.OnSerializingCallbacks = BaseFormatter<T>.GetCallbacks(methods, typeof(OnSerializingAttribute), ref list);
			BaseFormatter<T>.OnSerializedCallbacks = BaseFormatter<T>.GetCallbacks(methods, typeof(OnSerializedAttribute), ref list);
			BaseFormatter<T>.OnDeserializingCallbacks = BaseFormatter<T>.GetCallbacks(methods, typeof(OnDeserializingAttribute), ref list);
			BaseFormatter<T>.OnDeserializedCallbacks = BaseFormatter<T>.GetCallbacks(methods, typeof(OnDeserializedAttribute), ref list);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000F3E4 File Offset: 0x0000D5E4
		private static BaseFormatter<T>.SerializationCallback[] GetCallbacks(MethodInfo[] methods, Type callbackAttribute, ref List<BaseFormatter<T>.SerializationCallback> list)
		{
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.IsDefined(callbackAttribute, true))
				{
					BaseFormatter<T>.SerializationCallback serializationCallback = BaseFormatter<T>.CreateCallback(methodInfo);
					if (serializationCallback != null)
					{
						list.Add(serializationCallback);
					}
				}
			}
			BaseFormatter<T>.SerializationCallback[] result = list.ToArray();
			list.Clear();
			return result;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000F430 File Offset: 0x0000D630
		private static BaseFormatter<T>.SerializationCallback CreateCallback(MethodInfo info)
		{
			ParameterInfo[] parameters = info.GetParameters();
			if (parameters.Length == 0)
			{
				EmitUtilities.InstanceRefMethodCaller<T> action = EmitUtilities.CreateInstanceRefMethodCaller<T>(info);
				return delegate(ref T value, StreamingContext context)
				{
					action(ref value);
				};
			}
			if (parameters.Length == 1 && parameters[0].ParameterType == typeof(StreamingContext) && !parameters[0].ParameterType.IsByRef)
			{
				EmitUtilities.InstanceRefMethodCaller<T, StreamingContext> action = EmitUtilities.CreateInstanceRefMethodCaller<T, StreamingContext>(info);
				return delegate(ref T value, StreamingContext context)
				{
					action(ref value, context);
				};
			}
			DefaultLoggers.DefaultLogger.LogWarning("The method " + info.GetNiceName() + " has an invalid signature and will be ignored by the serialization system.");
			return null;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000F4D4 File Offset: 0x0000D6D4
		public Type SerializedType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000F4E0 File Offset: 0x0000D6E0
		void IFormatter.Serialize(object value, IDataWriter writer)
		{
			this.Serialize((T)((object)value), writer);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000F4EF File Offset: 0x0000D6EF
		object IFormatter.Deserialize(IDataReader reader)
		{
			return this.Deserialize(reader);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000F500 File Offset: 0x0000D700
		public T Deserialize(IDataReader reader)
		{
			DeserializationContext context = reader.Context;
			T t = this.GetUninitializedObject();
			if (BaseFormatter<T>.IsValueType)
			{
				this.InvokeOnDeserializingCallbacks(ref t, context);
			}
			else if (t != null)
			{
				this.RegisterReferenceID(t, reader);
				this.InvokeOnDeserializingCallbacks(ref t, context);
				if (BaseFormatter<T>.ImplementsIObjectReference)
				{
					try
					{
						t = (T)((object)(t as IObjectReference).GetRealObject(context.StreamingContext));
						this.RegisterReferenceID(t, reader);
					}
					catch (Exception exception)
					{
						context.Config.DebugContext.LogException(exception);
					}
				}
			}
			try
			{
				this.DeserializeImplementation(ref t, reader);
			}
			catch (Exception exception2)
			{
				context.Config.DebugContext.LogException(exception2);
			}
			if (BaseFormatter<T>.IsValueType || t != null)
			{
				for (int i = 0; i < BaseFormatter<T>.OnDeserializedCallbacks.Length; i++)
				{
					try
					{
						BaseFormatter<T>.OnDeserializedCallbacks[i](ref t, context.StreamingContext);
					}
					catch (Exception exception3)
					{
						context.Config.DebugContext.LogException(exception3);
					}
				}
				if (BaseFormatter<T>.ImplementsIDeserializationCallback)
				{
					IDeserializationCallback deserializationCallback = t as IDeserializationCallback;
					deserializationCallback.OnDeserialization(this);
					t = (T)((object)deserializationCallback);
				}
				if (BaseFormatter<T>.ImplementsISerializationCallbackReceiver)
				{
					try
					{
						ISerializationCallbackReceiver serializationCallbackReceiver = t as ISerializationCallbackReceiver;
						serializationCallbackReceiver.OnAfterDeserialize();
						t = (T)((object)serializationCallbackReceiver);
					}
					catch (Exception exception4)
					{
						context.Config.DebugContext.LogException(exception4);
					}
				}
			}
			return t;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000F694 File Offset: 0x0000D894
		public void Serialize(T value, IDataWriter writer)
		{
			SerializationContext context = writer.Context;
			for (int i = 0; i < BaseFormatter<T>.OnSerializingCallbacks.Length; i++)
			{
				try
				{
					BaseFormatter<T>.OnSerializingCallbacks[i](ref value, context.StreamingContext);
				}
				catch (Exception exception)
				{
					context.Config.DebugContext.LogException(exception);
				}
			}
			if (BaseFormatter<T>.ImplementsISerializationCallbackReceiver)
			{
				try
				{
					ISerializationCallbackReceiver serializationCallbackReceiver = value as ISerializationCallbackReceiver;
					serializationCallbackReceiver.OnBeforeSerialize();
					value = (T)((object)serializationCallbackReceiver);
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
			for (int j = 0; j < BaseFormatter<T>.OnSerializedCallbacks.Length; j++)
			{
				try
				{
					BaseFormatter<T>.OnSerializedCallbacks[j](ref value, context.StreamingContext);
				}
				catch (Exception exception4)
				{
					context.Config.DebugContext.LogException(exception4);
				}
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
		protected virtual T GetUninitializedObject()
		{
			if (BaseFormatter<T>.IsValueType)
			{
				return default(T);
			}
			return (T)((object)FormatterServices.GetUninitializedObject(typeof(T)));
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000F7EC File Offset: 0x0000D9EC
		protected void RegisterReferenceID(T value, IDataReader reader)
		{
			if (!BaseFormatter<T>.IsValueType)
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

		// Token: 0x06000237 RID: 567 RVA: 0x0000F838 File Offset: 0x0000DA38
		[Obsolete("Use the InvokeOnDeserializingCallbacks variant that takes a ref T value instead. This is for struct compatibility reasons.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void InvokeOnDeserializingCallbacks(T value, DeserializationContext context)
		{
			this.InvokeOnDeserializingCallbacks(ref value, context);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000F844 File Offset: 0x0000DA44
		protected void InvokeOnDeserializingCallbacks(ref T value, DeserializationContext context)
		{
			for (int i = 0; i < BaseFormatter<T>.OnDeserializingCallbacks.Length; i++)
			{
				try
				{
					BaseFormatter<T>.OnDeserializingCallbacks[i](ref value, context.StreamingContext);
				}
				catch (Exception exception)
				{
					context.Config.DebugContext.LogException(exception);
				}
			}
		}

		// Token: 0x06000239 RID: 569
		protected abstract void DeserializeImplementation(ref T value, IDataReader reader);

		// Token: 0x0600023A RID: 570
		protected abstract void SerializeImplementation(ref T value, IDataWriter writer);

		// Token: 0x04000090 RID: 144
		protected static readonly BaseFormatter<T>.SerializationCallback[] OnSerializingCallbacks;

		// Token: 0x04000091 RID: 145
		protected static readonly BaseFormatter<T>.SerializationCallback[] OnSerializedCallbacks;

		// Token: 0x04000092 RID: 146
		protected static readonly BaseFormatter<T>.SerializationCallback[] OnDeserializingCallbacks;

		// Token: 0x04000093 RID: 147
		protected static readonly BaseFormatter<T>.SerializationCallback[] OnDeserializedCallbacks;

		// Token: 0x04000094 RID: 148
		protected static readonly bool IsValueType = typeof(T).IsValueType;

		// Token: 0x04000095 RID: 149
		protected static readonly bool ImplementsISerializationCallbackReceiver = typeof(T).ImplementsOrInherits(typeof(ISerializationCallbackReceiver));

		// Token: 0x04000096 RID: 150
		protected static readonly bool ImplementsIDeserializationCallback = typeof(T).ImplementsOrInherits(typeof(IDeserializationCallback));

		// Token: 0x04000097 RID: 151
		protected static readonly bool ImplementsIObjectReference = typeof(T).ImplementsOrInherits(typeof(IObjectReference));

		// Token: 0x020000E4 RID: 228
		// (Invoke) Token: 0x0600067D RID: 1661
		protected delegate void SerializationCallback(ref T value, StreamingContext context);
	}
}
