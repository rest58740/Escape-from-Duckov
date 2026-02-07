using System;
using System.IO;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x02000231 RID: 561
	public class GraphSerializationContext
	{
		// Token: 0x06000D4D RID: 3405 RVA: 0x00053CA0 File Offset: 0x00051EA0
		public GraphSerializationContext(BinaryReader reader, GraphNode[] id2NodeMapping, uint graphIndex, GraphMeta meta)
		{
			this.reader = reader;
			this.id2NodeMapping = id2NodeMapping;
			this.graphIndex = graphIndex;
			this.meta = meta;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00053CC5 File Offset: 0x00051EC5
		public GraphSerializationContext(BinaryWriter writer, bool[] persistentGraphs)
		{
			this.writer = writer;
			this.persistentGraphs = persistentGraphs;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00053CDB File Offset: 0x00051EDB
		public void SerializeNodeReference(GraphNode node)
		{
			this.writer.Write((int)((node == null) ? uint.MaxValue : node.NodeIndex));
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00053CF4 File Offset: 0x00051EF4
		public void SerializeConnections(Connection[] connections, bool serializeMetadata)
		{
			if (connections == null)
			{
				this.writer.Write(-1);
				return;
			}
			int num = 0;
			for (int i = 0; i < connections.Length; i++)
			{
				num += (this.persistentGraphs[(int)connections[i].node.GraphIndex] ? 1 : 0);
			}
			this.writer.Write(num);
			for (int j = 0; j < connections.Length; j++)
			{
				if (this.persistentGraphs[(int)connections[j].node.GraphIndex])
				{
					this.SerializeNodeReference(connections[j].node);
					this.writer.Write(connections[j].cost);
					if (serializeMetadata)
					{
						this.writer.Write(connections[j].shapeEdgeInfo);
					}
				}
			}
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00053DBC File Offset: 0x00051FBC
		public Connection[] DeserializeConnections(bool deserializeMetadata)
		{
			int num = this.reader.ReadInt32();
			if (num == -1)
			{
				return null;
			}
			Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num);
			for (int i = 0; i < num; i++)
			{
				GraphNode node = this.DeserializeNodeReference();
				uint cost = this.reader.ReadUInt32();
				if (deserializeMetadata)
				{
					byte b = 15;
					if (!(this.meta.version < AstarSerializer.V4_1_0))
					{
						if (this.meta.version < AstarSerializer.V4_3_68)
						{
							this.reader.ReadByte();
						}
						else
						{
							b = this.reader.ReadByte();
						}
					}
					if (this.meta.version < AstarSerializer.V4_3_85)
					{
						b &= 79;
					}
					if (this.meta.version < AstarSerializer.V4_3_87)
					{
						b |= 48;
					}
					array[i] = new Connection(node, cost, b);
				}
				else
				{
					array[i] = new Connection(node, cost, true, true);
				}
			}
			return array;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00053EC0 File Offset: 0x000520C0
		public GraphNode DeserializeNodeReference()
		{
			int num = this.reader.ReadInt32();
			if (this.id2NodeMapping == null)
			{
				throw new Exception("Calling DeserializeNodeReference when not deserializing node references");
			}
			if (num == -1)
			{
				return null;
			}
			GraphNode graphNode = this.id2NodeMapping[num];
			if (graphNode == null)
			{
				throw new Exception("Invalid id (" + num.ToString() + ")");
			}
			return graphNode;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00053F19 File Offset: 0x00052119
		public void SerializeVector3(Vector3 v)
		{
			this.writer.Write(v.x);
			this.writer.Write(v.y);
			this.writer.Write(v.z);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00053F4E File Offset: 0x0005214E
		public Vector3 DeserializeVector3()
		{
			return new Vector3(this.reader.ReadSingle(), this.reader.ReadSingle(), this.reader.ReadSingle());
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00053F76 File Offset: 0x00052176
		public void SerializeInt3(Int3 v)
		{
			this.writer.Write(v.x);
			this.writer.Write(v.y);
			this.writer.Write(v.z);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00053FAB File Offset: 0x000521AB
		public Int3 DeserializeInt3()
		{
			return new Int3(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00053FD4 File Offset: 0x000521D4
		public unsafe UnsafeSpan<T> ReadSpan<[IsUnmanaged] T>(Allocator allocator) where T : struct, ValueType
		{
			UnsafeSpan<T> result = new UnsafeSpan<T>(allocator, this.reader.ReadInt32());
			if (UnsafeUtility.SizeOf<T>() % 4 != 0)
			{
				string str = "Cannot read data of type ";
				Type typeFromHandle = typeof(T);
				throw new Exception(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " because it has a size which is not a multiple of 4 bytes");
			}
			UnsafeSpan<int> unsafeSpan = result.Reinterpret<int>(UnsafeUtility.SizeOf<T>());
			for (int i = 0; i < unsafeSpan.Length; i++)
			{
				*unsafeSpan[i] = this.reader.ReadInt32();
			}
			return result;
		}

		// Token: 0x04000A46 RID: 2630
		private readonly GraphNode[] id2NodeMapping;

		// Token: 0x04000A47 RID: 2631
		public readonly BinaryReader reader;

		// Token: 0x04000A48 RID: 2632
		public readonly BinaryWriter writer;

		// Token: 0x04000A49 RID: 2633
		public readonly uint graphIndex;

		// Token: 0x04000A4A RID: 2634
		public readonly GraphMeta meta;

		// Token: 0x04000A4B RID: 2635
		public bool[] persistentGraphs;
	}
}
