using System;
using System.IO;

namespace Sirenix.Serialization
{
	// Token: 0x0200000B RID: 11
	public abstract class BaseDataWriter : BaseDataReaderWriter, IDataWriter, IDisposable
	{
		// Token: 0x06000061 RID: 97 RVA: 0x000027E9 File Offset: 0x000009E9
		protected BaseDataWriter(Stream stream, SerializationContext context)
		{
			this.context = context;
			if (stream != null)
			{
				this.Stream = stream;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002802 File Offset: 0x00000A02
		// (set) Token: 0x06000063 RID: 99 RVA: 0x0000280A File Offset: 0x00000A0A
		public virtual Stream Stream
		{
			get
			{
				return this.stream;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!value.CanWrite)
				{
					throw new ArgumentException("Cannot write to stream");
				}
				this.stream = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002834 File Offset: 0x00000A34
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000284F File Offset: 0x00000A4F
		public SerializationContext Context
		{
			get
			{
				if (this.context == null)
				{
					this.context = new SerializationContext();
				}
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002858 File Offset: 0x00000A58
		public virtual void FlushToStream()
		{
			this.Stream.Flush();
		}

		// Token: 0x06000067 RID: 103
		public abstract void BeginReferenceNode(string name, Type type, int id);

		// Token: 0x06000068 RID: 104
		public abstract void BeginStructNode(string name, Type type);

		// Token: 0x06000069 RID: 105
		public abstract void EndNode(string name);

		// Token: 0x0600006A RID: 106
		public abstract void BeginArrayNode(long length);

		// Token: 0x0600006B RID: 107
		public abstract void EndArrayNode();

		// Token: 0x0600006C RID: 108
		public abstract void WritePrimitiveArray<T>(T[] array) where T : struct;

		// Token: 0x0600006D RID: 109
		public abstract void WriteNull(string name);

		// Token: 0x0600006E RID: 110
		public abstract void WriteInternalReference(string name, int id);

		// Token: 0x0600006F RID: 111
		public abstract void WriteExternalReference(string name, int index);

		// Token: 0x06000070 RID: 112
		public abstract void WriteExternalReference(string name, Guid guid);

		// Token: 0x06000071 RID: 113
		public abstract void WriteExternalReference(string name, string id);

		// Token: 0x06000072 RID: 114
		public abstract void WriteChar(string name, char value);

		// Token: 0x06000073 RID: 115
		public abstract void WriteString(string name, string value);

		// Token: 0x06000074 RID: 116
		public abstract void WriteGuid(string name, Guid value);

		// Token: 0x06000075 RID: 117
		public abstract void WriteSByte(string name, sbyte value);

		// Token: 0x06000076 RID: 118
		public abstract void WriteInt16(string name, short value);

		// Token: 0x06000077 RID: 119
		public abstract void WriteInt32(string name, int value);

		// Token: 0x06000078 RID: 120
		public abstract void WriteInt64(string name, long value);

		// Token: 0x06000079 RID: 121
		public abstract void WriteByte(string name, byte value);

		// Token: 0x0600007A RID: 122
		public abstract void WriteUInt16(string name, ushort value);

		// Token: 0x0600007B RID: 123
		public abstract void WriteUInt32(string name, uint value);

		// Token: 0x0600007C RID: 124
		public abstract void WriteUInt64(string name, ulong value);

		// Token: 0x0600007D RID: 125
		public abstract void WriteDecimal(string name, decimal value);

		// Token: 0x0600007E RID: 126
		public abstract void WriteSingle(string name, float value);

		// Token: 0x0600007F RID: 127
		public abstract void WriteDouble(string name, double value);

		// Token: 0x06000080 RID: 128
		public abstract void WriteBoolean(string name, bool value);

		// Token: 0x06000081 RID: 129
		public abstract void Dispose();

		// Token: 0x06000082 RID: 130 RVA: 0x00002518 File Offset: 0x00000718
		public virtual void PrepareNewSerializationSession()
		{
			base.ClearNodes();
		}

		// Token: 0x06000083 RID: 131
		public abstract string GetDataDump();

		// Token: 0x0400000D RID: 13
		private SerializationContext context;

		// Token: 0x0400000E RID: 14
		private Stream stream;
	}
}
