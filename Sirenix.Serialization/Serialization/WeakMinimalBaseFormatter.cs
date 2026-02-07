using System;
using System.Runtime.Serialization;

namespace Sirenix.Serialization
{
	// Token: 0x02000041 RID: 65
	public abstract class WeakMinimalBaseFormatter : IFormatter
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0001422E File Offset: 0x0001242E
		Type IFormatter.SerializedType
		{
			get
			{
				return this.SerializedType;
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00014236 File Offset: 0x00012436
		public WeakMinimalBaseFormatter(Type serializedType)
		{
			this.SerializedType = serializedType;
			this.IsValueType = this.SerializedType.IsValueType;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00014258 File Offset: 0x00012458
		public object Deserialize(IDataReader reader)
		{
			object uninitializedObject = this.GetUninitializedObject();
			if (!this.IsValueType && uninitializedObject != null)
			{
				this.RegisterReferenceID(uninitializedObject, reader);
			}
			this.Read(ref uninitializedObject, reader);
			return uninitializedObject;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00014289 File Offset: 0x00012489
		public void Serialize(object value, IDataWriter writer)
		{
			this.Write(ref value, writer);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00014294 File Offset: 0x00012494
		protected virtual object GetUninitializedObject()
		{
			if (this.IsValueType)
			{
				return Activator.CreateInstance(this.SerializedType);
			}
			return FormatterServices.GetUninitializedObject(this.SerializedType);
		}

		// Token: 0x060002BA RID: 698
		protected abstract void Read(ref object value, IDataReader reader);

		// Token: 0x060002BB RID: 699
		protected abstract void Write(ref object value, IDataWriter writer);

		// Token: 0x060002BC RID: 700 RVA: 0x000142B8 File Offset: 0x000124B8
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

		// Token: 0x040000D9 RID: 217
		protected readonly Type SerializedType;

		// Token: 0x040000DA RID: 218
		protected readonly bool IsValueType;
	}
}
