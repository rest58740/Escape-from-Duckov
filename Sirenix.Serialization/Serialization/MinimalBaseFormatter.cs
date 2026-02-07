using System;
using System.Runtime.Serialization;

namespace Sirenix.Serialization
{
	// Token: 0x02000040 RID: 64
	public abstract class MinimalBaseFormatter<T> : IFormatter<T>, IFormatter
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00014122 File Offset: 0x00012322
		public Type SerializedType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00014130 File Offset: 0x00012330
		public T Deserialize(IDataReader reader)
		{
			T uninitializedObject = this.GetUninitializedObject();
			if (!MinimalBaseFormatter<T>.IsValueType && uninitializedObject != null)
			{
				this.RegisterReferenceID(uninitializedObject, reader);
			}
			this.Read(ref uninitializedObject, reader);
			return uninitializedObject;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00014165 File Offset: 0x00012365
		public void Serialize(T value, IDataWriter writer)
		{
			this.Write(ref value, writer);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00014170 File Offset: 0x00012370
		void IFormatter.Serialize(object value, IDataWriter writer)
		{
			if (value is T)
			{
				this.Serialize((T)((object)value), writer);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00014187 File Offset: 0x00012387
		object IFormatter.Deserialize(IDataReader reader)
		{
			return this.Deserialize(reader);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00014198 File Offset: 0x00012398
		protected virtual T GetUninitializedObject()
		{
			if (MinimalBaseFormatter<T>.IsValueType)
			{
				return default(T);
			}
			return (T)((object)FormatterServices.GetUninitializedObject(typeof(T)));
		}

		// Token: 0x060002B0 RID: 688
		protected abstract void Read(ref T value, IDataReader reader);

		// Token: 0x060002B1 RID: 689
		protected abstract void Write(ref T value, IDataWriter writer);

		// Token: 0x060002B2 RID: 690 RVA: 0x000141CC File Offset: 0x000123CC
		protected void RegisterReferenceID(T value, IDataReader reader)
		{
			if (!MinimalBaseFormatter<T>.IsValueType)
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

		// Token: 0x040000D8 RID: 216
		protected static readonly bool IsValueType = typeof(T).IsValueType;
	}
}
