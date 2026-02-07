using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Sirenix.Serialization
{
	// Token: 0x02000018 RID: 24
	public class SerializationNodeDataWriter : BaseDataWriter
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000D0BB File Offset: 0x0000B2BB
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x0000D0D6 File Offset: 0x0000B2D6
		public List<SerializationNode> Nodes
		{
			get
			{
				if (this.nodes == null)
				{
					this.nodes = new List<SerializationNode>();
				}
				return this.nodes;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.nodes = value;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000D0E8 File Offset: 0x0000B2E8
		public SerializationNodeDataWriter(SerializationContext context) : base(null, context)
		{
			Dictionary<Type, Delegate> dictionary = new Dictionary<Type, Delegate>();
			dictionary.Add(typeof(char), new Action<string, char>(this.WriteChar));
			dictionary.Add(typeof(sbyte), new Action<string, sbyte>(this.WriteSByte));
			dictionary.Add(typeof(short), new Action<string, short>(this.WriteInt16));
			dictionary.Add(typeof(int), new Action<string, int>(this.WriteInt32));
			dictionary.Add(typeof(long), new Action<string, long>(this.WriteInt64));
			dictionary.Add(typeof(byte), new Action<string, byte>(this.WriteByte));
			dictionary.Add(typeof(ushort), new Action<string, ushort>(this.WriteUInt16));
			dictionary.Add(typeof(uint), new Action<string, uint>(this.WriteUInt32));
			dictionary.Add(typeof(ulong), new Action<string, ulong>(this.WriteUInt64));
			dictionary.Add(typeof(decimal), new Action<string, decimal>(this.WriteDecimal));
			dictionary.Add(typeof(bool), new Action<string, bool>(this.WriteBoolean));
			dictionary.Add(typeof(float), new Action<string, float>(this.WriteSingle));
			dictionary.Add(typeof(double), new Action<string, double>(this.WriteDouble));
			dictionary.Add(typeof(Guid), new Action<string, Guid>(this.WriteGuid));
			this.primitiveTypeWriters = dictionary;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		public override Stream Stream
		{
			get
			{
				throw new NotSupportedException("This data writer has no stream.");
			}
			set
			{
				throw new NotSupportedException("This data writer has no stream.");
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000D2AC File Offset: 0x0000B4AC
		public override void BeginArrayNode(long length)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = string.Empty,
				Entry = EntryType.StartOfArray,
				Data = length.ToString(CultureInfo.InvariantCulture)
			});
			base.PushArray();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000D2FC File Offset: 0x0000B4FC
		public override void BeginReferenceNode(string name, Type type, int id)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.StartOfNode,
				Data = ((type != null) ? (id.ToString(CultureInfo.InvariantCulture) + "|" + base.Context.Binder.BindToName(type, base.Context.Config.DebugContext)) : id.ToString(CultureInfo.InvariantCulture))
			});
			base.PushNode(name, id, type);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000D38C File Offset: 0x0000B58C
		public override void BeginStructNode(string name, Type type)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.StartOfNode,
				Data = ((type != null) ? base.Context.Binder.BindToName(type, base.Context.Config.DebugContext) : "")
			});
			base.PushNode(name, -1, type);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000D3FE File Offset: 0x0000B5FE
		public override void Dispose()
		{
			this.nodes = null;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000D408 File Offset: 0x0000B608
		public override void EndArrayNode()
		{
			base.PopArray();
			this.Nodes.Add(new SerializationNode
			{
				Name = string.Empty,
				Entry = EntryType.EndOfArray,
				Data = string.Empty
			});
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000D450 File Offset: 0x0000B650
		public override void EndNode(string name)
		{
			base.PopNode(name);
			this.Nodes.Add(new SerializationNode
			{
				Name = string.Empty,
				Entry = EntryType.EndOfNode,
				Data = string.Empty
			});
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000D498 File Offset: 0x0000B698
		public override void PrepareNewSerializationSession()
		{
			base.PrepareNewSerializationSession();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000D4A0 File Offset: 0x0000B6A0
		public override void WriteBoolean(string name, bool value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Boolean,
				Data = (value ? "true" : "false")
			});
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
		public override void WriteByte(string name, byte value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Integer,
				Data = value.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000D534 File Offset: 0x0000B734
		public override void WriteChar(string name, char value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.String,
				Data = value.ToString(CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000D578 File Offset: 0x0000B778
		public override void WriteDecimal(string name, decimal value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.FloatingPoint,
				Data = value.ToString("G", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000D5C4 File Offset: 0x0000B7C4
		public override void WriteSingle(string name, float value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.FloatingPoint,
				Data = value.ToString("R", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000D610 File Offset: 0x0000B810
		public override void WriteDouble(string name, double value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.FloatingPoint,
				Data = value.ToString("R", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000D65C File Offset: 0x0000B85C
		public override void WriteExternalReference(string name, Guid guid)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.ExternalReferenceByGuid,
				Data = guid.ToString("N", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		public override void WriteExternalReference(string name, string id)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.ExternalReferenceByString,
				Data = id
			});
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000D6E4 File Offset: 0x0000B8E4
		public override void WriteExternalReference(string name, int index)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.ExternalReferenceByIndex,
				Data = index.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D730 File Offset: 0x0000B930
		public override void WriteGuid(string name, Guid value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Guid,
				Data = value.ToString("N", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000D77C File Offset: 0x0000B97C
		public override void WriteInt16(string name, short value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Integer,
				Data = value.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
		public override void WriteInt32(string name, int value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Integer,
				Data = value.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000D814 File Offset: 0x0000BA14
		public override void WriteInt64(string name, long value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Integer,
				Data = value.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000D860 File Offset: 0x0000BA60
		public override void WriteInternalReference(string name, int id)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.InternalReference,
				Data = id.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		public override void WriteNull(string name)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Null,
				Data = string.Empty
			});
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000D8EC File Offset: 0x0000BAEC
		public override void WritePrimitiveArray<T>(T[] array)
		{
			if (!FormatterUtilities.IsPrimitiveArrayType(typeof(T)))
			{
				throw new ArgumentException("Type " + typeof(T).Name + " is not a valid primitive array type.");
			}
			if (typeof(T) == typeof(byte))
			{
				string data = ProperBitConverter.BytesToHexString((byte[])array, true);
				this.Nodes.Add(new SerializationNode
				{
					Name = string.Empty,
					Entry = EntryType.PrimitiveArray,
					Data = data
				});
				return;
			}
			this.Nodes.Add(new SerializationNode
			{
				Name = string.Empty,
				Entry = EntryType.PrimitiveArray,
				Data = ((long)array.Length).ToString(CultureInfo.InvariantCulture)
			});
			base.PushArray();
			Action<string, T> action = (Action<string, T>)this.primitiveTypeWriters[typeof(T)];
			for (int i = 0; i < array.Length; i++)
			{
				action.Invoke(string.Empty, array[i]);
			}
			this.EndArrayNode();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000DA14 File Offset: 0x0000BC14
		public override void WriteSByte(string name, sbyte value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Integer,
				Data = value.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000DA60 File Offset: 0x0000BC60
		public override void WriteString(string name, string value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.String,
				Data = value
			});
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000DA9C File Offset: 0x0000BC9C
		public override void WriteUInt16(string name, ushort value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Integer,
				Data = value.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000DAE8 File Offset: 0x0000BCE8
		public override void WriteUInt32(string name, uint value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Integer,
				Data = value.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000DB34 File Offset: 0x0000BD34
		public override void WriteUInt64(string name, ulong value)
		{
			this.Nodes.Add(new SerializationNode
			{
				Name = name,
				Entry = EntryType.Integer,
				Data = value.ToString("D", CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000021B8 File Offset: 0x000003B8
		public override void FlushToStream()
		{
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000DB80 File Offset: 0x0000BD80
		public override string GetDataDump()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Nodes: \n\n");
			for (int i = 0; i < this.nodes.Count; i++)
			{
				SerializationNode serializationNode = this.nodes[i];
				stringBuilder.AppendLine("    - Name: " + serializationNode.Name);
				StringBuilder stringBuilder2 = stringBuilder;
				string text = "      Entry: ";
				int entry = (int)serializationNode.Entry;
				stringBuilder2.AppendLine(text + entry.ToString());
				stringBuilder.AppendLine("      Data: " + serializationNode.Data);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000083 RID: 131
		private List<SerializationNode> nodes;

		// Token: 0x04000084 RID: 132
		private Dictionary<Type, Delegate> primitiveTypeWriters;
	}
}
