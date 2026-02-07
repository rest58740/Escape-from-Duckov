using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace System.Runtime.Serialization
{
	// Token: 0x02000650 RID: 1616
	[CLSCompliant(false)]
	[Serializable]
	public abstract class Formatter : IFormatter
	{
		// Token: 0x06003C69 RID: 15465 RVA: 0x000D163E File Offset: 0x000CF83E
		protected Formatter()
		{
			this.m_objectQueue = new Queue();
			this.m_idGenerator = new ObjectIDGenerator();
		}

		// Token: 0x06003C6A RID: 15466
		public abstract object Deserialize(Stream serializationStream);

		// Token: 0x06003C6B RID: 15467 RVA: 0x000D165C File Offset: 0x000CF85C
		protected virtual object GetNext(out long objID)
		{
			if (this.m_objectQueue.Count == 0)
			{
				objID = 0L;
				return null;
			}
			object obj = this.m_objectQueue.Dequeue();
			bool flag;
			objID = this.m_idGenerator.HasId(obj, out flag);
			if (flag)
			{
				throw new SerializationException("Object has never been assigned an objectID");
			}
			return obj;
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x000D16A8 File Offset: 0x000CF8A8
		protected virtual long Schedule(object obj)
		{
			if (obj == null)
			{
				return 0L;
			}
			bool flag;
			long id = this.m_idGenerator.GetId(obj, out flag);
			if (flag)
			{
				this.m_objectQueue.Enqueue(obj);
			}
			return id;
		}

		// Token: 0x06003C6D RID: 15469
		public abstract void Serialize(Stream serializationStream, object graph);

		// Token: 0x06003C6E RID: 15470
		protected abstract void WriteArray(object obj, string name, Type memberType);

		// Token: 0x06003C6F RID: 15471
		protected abstract void WriteBoolean(bool val, string name);

		// Token: 0x06003C70 RID: 15472
		protected abstract void WriteByte(byte val, string name);

		// Token: 0x06003C71 RID: 15473
		protected abstract void WriteChar(char val, string name);

		// Token: 0x06003C72 RID: 15474
		protected abstract void WriteDateTime(DateTime val, string name);

		// Token: 0x06003C73 RID: 15475
		protected abstract void WriteDecimal(decimal val, string name);

		// Token: 0x06003C74 RID: 15476
		protected abstract void WriteDouble(double val, string name);

		// Token: 0x06003C75 RID: 15477
		protected abstract void WriteInt16(short val, string name);

		// Token: 0x06003C76 RID: 15478
		protected abstract void WriteInt32(int val, string name);

		// Token: 0x06003C77 RID: 15479
		protected abstract void WriteInt64(long val, string name);

		// Token: 0x06003C78 RID: 15480
		protected abstract void WriteObjectRef(object obj, string name, Type memberType);

		// Token: 0x06003C79 RID: 15481 RVA: 0x000D16D8 File Offset: 0x000CF8D8
		protected virtual void WriteMember(string memberName, object data)
		{
			if (data == null)
			{
				this.WriteObjectRef(data, memberName, typeof(object));
				return;
			}
			Type type = data.GetType();
			if (type == typeof(bool))
			{
				this.WriteBoolean(Convert.ToBoolean(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(char))
			{
				this.WriteChar(Convert.ToChar(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(sbyte))
			{
				this.WriteSByte(Convert.ToSByte(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(byte))
			{
				this.WriteByte(Convert.ToByte(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(short))
			{
				this.WriteInt16(Convert.ToInt16(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(int))
			{
				this.WriteInt32(Convert.ToInt32(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(long))
			{
				this.WriteInt64(Convert.ToInt64(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(float))
			{
				this.WriteSingle(Convert.ToSingle(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(double))
			{
				this.WriteDouble(Convert.ToDouble(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(DateTime))
			{
				this.WriteDateTime(Convert.ToDateTime(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(decimal))
			{
				this.WriteDecimal(Convert.ToDecimal(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(ushort))
			{
				this.WriteUInt16(Convert.ToUInt16(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(uint))
			{
				this.WriteUInt32(Convert.ToUInt32(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(ulong))
			{
				this.WriteUInt64(Convert.ToUInt64(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type.IsArray)
			{
				this.WriteArray(data, memberName, type);
				return;
			}
			if (type.IsValueType)
			{
				this.WriteValueType(data, memberName, type);
				return;
			}
			this.WriteObjectRef(data, memberName, type);
		}

		// Token: 0x06003C7A RID: 15482
		[CLSCompliant(false)]
		protected abstract void WriteSByte(sbyte val, string name);

		// Token: 0x06003C7B RID: 15483
		protected abstract void WriteSingle(float val, string name);

		// Token: 0x06003C7C RID: 15484
		protected abstract void WriteTimeSpan(TimeSpan val, string name);

		// Token: 0x06003C7D RID: 15485
		[CLSCompliant(false)]
		protected abstract void WriteUInt16(ushort val, string name);

		// Token: 0x06003C7E RID: 15486
		[CLSCompliant(false)]
		protected abstract void WriteUInt32(uint val, string name);

		// Token: 0x06003C7F RID: 15487
		[CLSCompliant(false)]
		protected abstract void WriteUInt64(ulong val, string name);

		// Token: 0x06003C80 RID: 15488
		protected abstract void WriteValueType(object obj, string name, Type memberType);

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06003C81 RID: 15489
		// (set) Token: 0x06003C82 RID: 15490
		public abstract ISurrogateSelector SurrogateSelector { get; set; }

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06003C83 RID: 15491
		// (set) Token: 0x06003C84 RID: 15492
		public abstract SerializationBinder Binder { get; set; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06003C85 RID: 15493
		// (set) Token: 0x06003C86 RID: 15494
		public abstract StreamingContext Context { get; set; }

		// Token: 0x0400271F RID: 10015
		protected ObjectIDGenerator m_idGenerator;

		// Token: 0x04002720 RID: 10016
		protected Queue m_objectQueue;
	}
}
