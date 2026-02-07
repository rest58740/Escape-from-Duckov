using System;
using System.ComponentModel;

namespace Sirenix.Serialization
{
	// Token: 0x0200000A RID: 10
	public abstract class BaseDataReaderWriter
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002520 File Offset: 0x00000720
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002559 File Offset: 0x00000759
		[Obsolete("Use the Binder member on the writer's SerializationContext/DeserializationContext instead.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public TwoWaySerializationBinder Binder
		{
			get
			{
				if (this is IDataWriter)
				{
					return (this as IDataWriter).Context.Binder;
				}
				if (this is IDataReader)
				{
					return (this as IDataReader).Context.Binder;
				}
				return TwoWaySerializationBinder.Default;
			}
			set
			{
				if (this is IDataWriter)
				{
					(this as IDataWriter).Context.Binder = value;
					return;
				}
				if (this is IDataReader)
				{
					(this as IDataReader).Context.Binder = value;
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000258E File Offset: 0x0000078E
		public bool IsInArrayNode
		{
			get
			{
				return this.nodesLength != 0 && this.nodes[this.nodesLength - 1].IsArray;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000025B2 File Offset: 0x000007B2
		protected int NodeDepth
		{
			get
			{
				return this.nodesLength;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000025BA File Offset: 0x000007BA
		protected NodeInfo[] NodesArray
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000025C2 File Offset: 0x000007C2
		protected NodeInfo CurrentNode
		{
			get
			{
				if (this.nodesLength != 0)
				{
					return this.nodes[this.nodesLength - 1];
				}
				return NodeInfo.Empty;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000025E5 File Offset: 0x000007E5
		protected void PushNode(NodeInfo node)
		{
			if (this.nodesLength == this.nodes.Length)
			{
				this.ExpandNodes();
			}
			this.nodes[this.nodesLength] = node;
			this.nodesLength++;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000261D File Offset: 0x0000081D
		protected void PushNode(string name, int id, Type type)
		{
			if (this.nodesLength == this.nodes.Length)
			{
				this.ExpandNodes();
			}
			this.nodes[this.nodesLength] = new NodeInfo(name, id, type, false);
			this.nodesLength++;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002660 File Offset: 0x00000860
		protected void PushArray()
		{
			if (this.nodesLength == this.nodes.Length)
			{
				this.ExpandNodes();
			}
			if (this.nodesLength == 0 || this.nodes[this.nodesLength - 1].IsArray)
			{
				this.nodes[this.nodesLength] = new NodeInfo(null, -1, null, true);
			}
			else
			{
				NodeInfo nodeInfo = this.nodes[this.nodesLength - 1];
				this.nodes[this.nodesLength] = new NodeInfo(nodeInfo.Name, nodeInfo.Id, nodeInfo.Type, true);
			}
			this.nodesLength++;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000270C File Offset: 0x0000090C
		private void ExpandNodes()
		{
			NodeInfo[] array = new NodeInfo[this.nodes.Length * 2];
			NodeInfo[] array2 = this.nodes;
			for (int i = 0; i < array2.Length; i++)
			{
				array[i] = array2[i];
			}
			this.nodes = array;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002753 File Offset: 0x00000953
		protected void PopNode(string name)
		{
			if (this.nodesLength == 0)
			{
				throw new InvalidOperationException("There are no nodes to pop.");
			}
			this.nodesLength--;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002778 File Offset: 0x00000978
		protected void PopArray()
		{
			if (this.nodesLength == 0)
			{
				throw new InvalidOperationException("There are no nodes to pop.");
			}
			if (!this.nodes[this.nodesLength - 1].IsArray)
			{
				throw new InvalidOperationException("Was not in array when exiting array.");
			}
			this.nodesLength--;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000027CB File Offset: 0x000009CB
		protected void ClearNodes()
		{
			this.nodesLength = 0;
		}

		// Token: 0x0400000B RID: 11
		private NodeInfo[] nodes = new NodeInfo[32];

		// Token: 0x0400000C RID: 12
		private int nodesLength;
	}
}
