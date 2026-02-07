using System;
using System.Collections.Generic;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000033 RID: 51
	[fsDeserializeOverwrite]
	[Serializable]
	public class GraphSource : ISerializationCollector, ISerializationCollectable
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00008E59 File Offset: 0x00007059
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x00008E61 File Offset: 0x00007061
		public List<Task> allTasks { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00008E6A File Offset: 0x0000706A
		// (set) Token: 0x060002FB RID: 763 RVA: 0x00008E72 File Offset: 0x00007072
		public List<BBParameter> allParameters { get; private set; }

		// Token: 0x060002FC RID: 764 RVA: 0x00008E7B File Offset: 0x0000707B
		void ISerializationCollector.OnPush(ISerializationCollector parent)
		{
			this.allTasks = new List<Task>();
			this.allParameters = new List<BBParameter>();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00008E93 File Offset: 0x00007093
		void ISerializationCollector.OnCollect(ISerializationCollectable child, int depth)
		{
			if (child is Task)
			{
				this.allTasks.Add((Task)child);
			}
			if (child is BBParameter)
			{
				this.allParameters.Add((BBParameter)child);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00008EC7 File Offset: 0x000070C7
		void ISerializationCollector.OnPop(ISerializationCollector parent)
		{
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00008EC9 File Offset: 0x000070C9
		// (set) Token: 0x06000300 RID: 768 RVA: 0x00008ED1 File Offset: 0x000070D1
		public float version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00008EDA File Offset: 0x000070DA
		// (set) Token: 0x06000302 RID: 770 RVA: 0x00008EE2 File Offset: 0x000070E2
		public string category
		{
			get
			{
				return this._category;
			}
			set
			{
				this._category = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00008EEB File Offset: 0x000070EB
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00008EF3 File Offset: 0x000070F3
		public string comments
		{
			get
			{
				return this._comments;
			}
			set
			{
				this._comments = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00008EFC File Offset: 0x000070FC
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00008F04 File Offset: 0x00007104
		public Vector2 translation
		{
			get
			{
				return this._translation;
			}
			set
			{
				this._translation = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00008F0D File Offset: 0x0000710D
		// (set) Token: 0x06000308 RID: 776 RVA: 0x00008F15 File Offset: 0x00007115
		public float zoomFactor
		{
			get
			{
				return this._zoomFactor;
			}
			set
			{
				this._zoomFactor = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00008F1E File Offset: 0x0000711E
		// (set) Token: 0x0600030A RID: 778 RVA: 0x00008F26 File Offset: 0x00007126
		public string type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00008F2F File Offset: 0x0000712F
		// (set) Token: 0x0600030C RID: 780 RVA: 0x00008F37 File Offset: 0x00007137
		public List<Node> nodes
		{
			get
			{
				return this._nodes;
			}
			set
			{
				this._nodes = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00008F40 File Offset: 0x00007140
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00008F48 File Offset: 0x00007148
		public List<Connection> connections
		{
			get
			{
				return this._connections;
			}
			private set
			{
				this._connections = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00008F51 File Offset: 0x00007151
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00008F59 File Offset: 0x00007159
		public List<CanvasGroup> canvasGroups
		{
			get
			{
				return this._canvasGroups;
			}
			set
			{
				this._canvasGroups = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00008F62 File Offset: 0x00007162
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00008F6A File Offset: 0x0000716A
		public BlackboardSource localBlackboard
		{
			get
			{
				return this._localBlackboard;
			}
			set
			{
				this._localBlackboard = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00008F73 File Offset: 0x00007173
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00008F7B File Offset: 0x0000717B
		public object derivedData
		{
			get
			{
				return this._derivedData;
			}
			set
			{
				this._derivedData = value;
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00008F84 File Offset: 0x00007184
		public GraphSource()
		{
			this.zoomFactor = 1f;
			this.nodes = new List<Node>();
			this.canvasGroups = new List<CanvasGroup>();
			this.localBlackboard = new BlackboardSource();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00008FB8 File Offset: 0x000071B8
		public GraphSource Pack(Graph graph)
		{
			this.version = 3.3f;
			this.type = graph.GetType().FullName;
			List<Connection> list = new List<Connection>();
			for (int i = 0; i < this.nodes.Count; i++)
			{
				for (int j = 0; j < this.nodes[i].outConnections.Count; j++)
				{
					list.Add(this.nodes[i].outConnections[j]);
				}
			}
			this.connections = list;
			this.derivedData = graph.OnDerivedDataSerialization();
			return this;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00009050 File Offset: 0x00007250
		public GraphSource Unpack(Graph graph)
		{
			this.localBlackboard.unityContextObject = graph;
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].outConnections.Clear();
				this.nodes[i].inConnections.Clear();
				this.nodes[i].graph = graph;
				this.nodes[i].ID = i;
			}
			for (int j = 0; j < this.connections.Count; j++)
			{
				this.connections[j].sourceNode.outConnections.Add(this.connections[j]);
				this.connections[j].targetNode.inConnections.Add(this.connections[j]);
			}
			graph.OnDerivedDataDeserialization(this.derivedData);
			return this;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00009140 File Offset: 0x00007340
		public GraphSource SetMetaData(GraphSource source)
		{
			this.version = source.version;
			this.category = source.category;
			this.comments = source.comments;
			this.translation = source.translation;
			this.zoomFactor = source.zoomFactor;
			return this;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000917F File Offset: 0x0000737F
		public void PurgeRedundantReferences()
		{
			this.connections.Clear();
		}

		// Token: 0x040000AC RID: 172
		public const float FRAMEWORK_VERSION = 3.3f;

		// Token: 0x040000AD RID: 173
		[SerializeField]
		[fsSerializeAs("version")]
		[fsWriteOnly]
		[fsIgnoreInBuild]
		private float _version;

		// Token: 0x040000AE RID: 174
		[SerializeField]
		[fsSerializeAs("category")]
		[fsWriteOnly]
		[fsIgnoreInBuild]
		private string _category;

		// Token: 0x040000AF RID: 175
		[SerializeField]
		[fsSerializeAs("comments")]
		[fsWriteOnly]
		[fsIgnoreInBuild]
		private string _comments;

		// Token: 0x040000B0 RID: 176
		[SerializeField]
		[fsSerializeAs("translation")]
		[fsWriteOnly]
		[fsIgnoreInBuild]
		private Vector2 _translation;

		// Token: 0x040000B1 RID: 177
		[SerializeField]
		[fsSerializeAs("zoomFactor")]
		[fsWriteOnly]
		[fsIgnoreInBuild]
		private float _zoomFactor;

		// Token: 0x040000B2 RID: 178
		[fsSerializeAs("type")]
		private string _type;

		// Token: 0x040000B3 RID: 179
		[fsSerializeAs("nodes")]
		private List<Node> _nodes;

		// Token: 0x040000B4 RID: 180
		[fsSerializeAs("connections")]
		private List<Connection> _connections;

		// Token: 0x040000B5 RID: 181
		[fsSerializeAs("canvasGroups")]
		[fsIgnoreInBuild]
		private List<CanvasGroup> _canvasGroups;

		// Token: 0x040000B6 RID: 182
		[fsSerializeAs("localBlackboard")]
		private BlackboardSource _localBlackboard;

		// Token: 0x040000B7 RID: 183
		[fsSerializeAs("derivedData")]
		private object _derivedData;
	}
}
