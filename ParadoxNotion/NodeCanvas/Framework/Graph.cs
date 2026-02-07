using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Services;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public abstract class Graph : ScriptableObject, ITaskSystem, ISerializationCallbackReceiver
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002734 File Offset: 0x00000934
		// (set) Token: 0x0600003F RID: 63 RVA: 0x0000273C File Offset: 0x0000093C
		public TextAsset externalSerializationFile
		{
			get
			{
				return this._externalSerializationFile;
			}
			internal set
			{
				this._externalSerializationFile = value;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000040 RID: 64 RVA: 0x00002748 File Offset: 0x00000948
		// (remove) Token: 0x06000041 RID: 65 RVA: 0x0000277C File Offset: 0x0000097C
		public static event Action<Graph> onGraphSerialized;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000042 RID: 66 RVA: 0x000027B0 File Offset: 0x000009B0
		// (remove) Token: 0x06000043 RID: 67 RVA: 0x000027E4 File Offset: 0x000009E4
		public static event Action<Graph> onGraphDeserialized;

		// Token: 0x06000044 RID: 68 RVA: 0x00002817 File Offset: 0x00000A17
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.SelfSerialize();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002820 File Offset: 0x00000A20
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.SelfDeserialize();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002829 File Offset: 0x00000A29
		protected void OnEnable()
		{
			this.Validate();
			this.OnGraphObjectEnable();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002837 File Offset: 0x00000A37
		protected void OnDisable()
		{
			this.OnGraphObjectDisable();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000283F File Offset: 0x00000A3F
		protected void OnDestroy()
		{
			if (Threader.applicationIsPlaying)
			{
				this.Stop(true);
			}
			this.OnGraphObjectDestroy();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002855 File Offset: 0x00000A55
		protected void OnValidate()
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002857 File Offset: 0x00000A57
		protected void Reset()
		{
			this.OnGraphValidate();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002860 File Offset: 0x00000A60
		public bool SelfSerialize()
		{
			if (this._haltSerialization)
			{
				return false;
			}
			if (this.haltForUndo)
			{
				return false;
			}
			List<Object> list = new List<Object>();
			string text = this.Serialize(list);
			if (text != this._serializedGraph || !list.SequenceEqual(this._objectReferences))
			{
				this.haltForUndo = true;
				this.haltForUndo = false;
				this._serializedGraph = text;
				this._objectReferences = list;
				if (Graph.onGraphSerialized != null)
				{
					Graph.onGraphSerialized.Invoke(this);
				}
				this.graphSource.PurgeRedundantReferences();
				this.flatMetaGraph = null;
				this.fullMetaGraph = null;
				this.nestedMetaGraph = null;
				return true;
			}
			return false;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000028FC File Offset: 0x00000AFC
		public bool SelfDeserialize()
		{
			if (this.Deserialize(this._serializedGraph, this._objectReferences, false))
			{
				if (Graph.onGraphDeserialized != null)
				{
					Graph.onGraphDeserialized.Invoke(this);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002928 File Offset: 0x00000B28
		public string Serialize(List<Object> references)
		{
			if (references == null)
			{
				references = new List<Object>();
			}
			this.UpdateNodeIDs(true);
			return JSONSerializer.Serialize(typeof(GraphSource), this.graphSource.Pack(this), references, false);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002958 File Offset: 0x00000B58
		public bool Deserialize(string serializedGraph, List<Object> references, bool validate)
		{
			if (string.IsNullOrEmpty(serializedGraph))
			{
				return false;
			}
			if (references == null)
			{
				references = this._objectReferences;
			}
			bool result;
			try
			{
				JSONSerializer.TryDeserializeOverwrite<GraphSource>(this.graphSource, serializedGraph, references);
				if (this.graphSource.type != base.GetType().FullName)
				{
					this._haltSerialization = true;
					result = false;
				}
				else
				{
					this._graphSource = this.graphSource.Unpack(this);
					this._serializedGraph = serializedGraph;
					this._objectReferences = references;
					this._haltSerialization = false;
					if (validate)
					{
						this.Validate();
					}
					result = true;
				}
			}
			catch (Exception exception)
			{
				ParadoxNotion.Services.Logger.LogException(exception, "Serialization", this);
				this._haltSerialization = true;
				result = false;
			}
			return result;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002A0C File Offset: 0x00000C0C
		public GraphSource GetGraphSource()
		{
			return this._graphSource;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002A14 File Offset: 0x00000C14
		public string GetSerializedJsonData()
		{
			return this._serializedGraph;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002A1C File Offset: 0x00000C1C
		public List<Object> GetSerializedReferencesData()
		{
			List<Object> objectReferences = this._objectReferences;
			if (objectReferences == null)
			{
				return null;
			}
			return objectReferences.ToList<Object>();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002A2F File Offset: 0x00000C2F
		public GraphSource GetGraphSourceMetaDataCopy()
		{
			return new GraphSource().SetMetaData(this.graphSource);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002A41 File Offset: 0x00000C41
		public void SetGraphSourceMetaData(GraphSource source)
		{
			this.graphSource.SetMetaData(source);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002A50 File Offset: 0x00000C50
		public string SerializeLocalBlackboard(ref List<Object> references)
		{
			if (references != null)
			{
				references.Clear();
			}
			return JSONSerializer.Serialize(typeof(BlackboardSource), this.localBlackboard, references, false);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002A75 File Offset: 0x00000C75
		public bool DeserializeLocalBlackboard(string json, List<Object> references)
		{
			this.localBlackboard = JSONSerializer.TryDeserializeOverwrite<BlackboardSource>(this.localBlackboard, json, references);
			return true;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002A8C File Offset: 0x00000C8C
		public static T Clone<T>(T graph, Graph parentGraph) where T : Graph
		{
			T t = Object.Instantiate<T>(graph);
			t.name = t.name.Replace("(Clone)", string.Empty);
			t.parentGraph = parentGraph;
			return t;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public void Validate()
		{
			if (string.IsNullOrEmpty(this._serializedGraph))
			{
				return;
			}
			for (int i = 0; i < this.allNodes.Count; i++)
			{
				try
				{
					this.allNodes[i].Validate(this);
				}
				catch (Exception exception)
				{
					ParadoxNotion.Services.Logger.LogException(exception, "Validation", this.allNodes[i]);
				}
			}
			for (int j = 0; j < this.allTasks.Count; j++)
			{
				try
				{
					this.allTasks[j].Validate(this);
				}
				catch (Exception exception2)
				{
					ParadoxNotion.Services.Logger.LogException(exception2, "Validation", this.allTasks[j]);
				}
			}
			this.OnGraphValidate();
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000058 RID: 88 RVA: 0x00002B98 File Offset: 0x00000D98
		// (remove) Token: 0x06000059 RID: 89 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public event Action<bool> onFinish;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002C05 File Offset: 0x00000E05
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002C0D File Offset: 0x00000E0D
		private bool hasInitialized { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002C16 File Offset: 0x00000E16
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002C1E File Offset: 0x00000E1E
		private HierarchyTree.Element flatMetaGraph { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002C27 File Offset: 0x00000E27
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002C2F File Offset: 0x00000E2F
		private HierarchyTree.Element fullMetaGraph { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002C38 File Offset: 0x00000E38
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002C40 File Offset: 0x00000E40
		private HierarchyTree.Element nestedMetaGraph { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000062 RID: 98
		public abstract Type baseNodeType { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000063 RID: 99
		public abstract bool requiresAgent { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000064 RID: 100
		public abstract bool requiresPrimeNode { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000065 RID: 101
		public abstract bool isTree { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000066 RID: 102
		public abstract PlanarDirection flowDirection { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000067 RID: 103
		public abstract bool allowBlackboardOverrides { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000068 RID: 104
		public abstract bool canAcceptVariableDrops { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002C49 File Offset: 0x00000E49
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002C51 File Offset: 0x00000E51
		private GraphSource graphSource
		{
			get
			{
				return this._graphSource;
			}
			set
			{
				this._graphSource = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002C5A File Offset: 0x00000E5A
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002C67 File Offset: 0x00000E67
		public string category
		{
			get
			{
				return this.graphSource.category;
			}
			set
			{
				this.graphSource.category = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002C75 File Offset: 0x00000E75
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002C82 File Offset: 0x00000E82
		public string comments
		{
			get
			{
				return this.graphSource.comments;
			}
			set
			{
				this.graphSource.comments = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002C90 File Offset: 0x00000E90
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00002C9D File Offset: 0x00000E9D
		public Vector2 translation
		{
			get
			{
				return this.graphSource.translation;
			}
			set
			{
				this.graphSource.translation = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002CAB File Offset: 0x00000EAB
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public float zoomFactor
		{
			get
			{
				return this.graphSource.zoomFactor;
			}
			set
			{
				this.graphSource.zoomFactor = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002CC6 File Offset: 0x00000EC6
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00002CD3 File Offset: 0x00000ED3
		public List<Node> allNodes
		{
			get
			{
				return this.graphSource.nodes;
			}
			set
			{
				this.graphSource.nodes = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002CE1 File Offset: 0x00000EE1
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00002CEE File Offset: 0x00000EEE
		public List<CanvasGroup> canvasGroups
		{
			get
			{
				return this.graphSource.canvasGroups;
			}
			set
			{
				this.graphSource.canvasGroups = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002CFC File Offset: 0x00000EFC
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00002D09 File Offset: 0x00000F09
		private BlackboardSource localBlackboard
		{
			get
			{
				return this.graphSource.localBlackboard;
			}
			set
			{
				this.graphSource.localBlackboard = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002D17 File Offset: 0x00000F17
		private List<Task> allTasks
		{
			get
			{
				return this.graphSource.allTasks;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002D24 File Offset: 0x00000F24
		private List<BBParameter> allParameters
		{
			get
			{
				return this.graphSource.allParameters;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002D31 File Offset: 0x00000F31
		private List<Connection> allConnections
		{
			get
			{
				return this.graphSource.connections;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002D40 File Offset: 0x00000F40
		public Graph rootGraph
		{
			get
			{
				Graph graph = this;
				while (graph.parentGraph != null)
				{
					graph = graph.parentGraph;
				}
				return graph;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002D67 File Offset: 0x00000F67
		public bool serializationHalted
		{
			get
			{
				return this._haltSerialization;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002D6F File Offset: 0x00000F6F
		public static IEnumerable<Graph> runningGraphs
		{
			get
			{
				return Graph._runningGraphs;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002D76 File Offset: 0x00000F76
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00002D7E File Offset: 0x00000F7E
		public Graph parentGraph { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002D87 File Offset: 0x00000F87
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00002D8F File Offset: 0x00000F8F
		public float elapsedTime { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00002D98 File Offset: 0x00000F98
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public float deltaTime { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00002DA9 File Offset: 0x00000FA9
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00002DB1 File Offset: 0x00000FB1
		public int lastUpdateFrame { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002DBA File Offset: 0x00000FBA
		public bool didUpdateLastFrame
		{
			get
			{
				return this.lastUpdateFrame >= Time.frameCount - 1;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00002DCE File Offset: 0x00000FCE
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00002DD6 File Offset: 0x00000FD6
		public bool isRunning { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00002DDF File Offset: 0x00000FDF
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00002DE7 File Offset: 0x00000FE7
		public bool isPaused { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00002DF0 File Offset: 0x00000FF0
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public Graph.UpdateMode updateMode { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00002E04 File Offset: 0x00001004
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00002E38 File Offset: 0x00001038
		public Node primeNode
		{
			get
			{
				if (this.allNodes.Count > 0)
				{
					Node node = this.allNodes[0];
					if (node.allowAsPrime)
					{
						return node;
					}
				}
				return null;
			}
			set
			{
				if (this.primeNode != value && value != null && value.allowAsPrime && this.allNodes.Contains(value))
				{
					if (this.isRunning)
					{
						if (this.primeNode != null)
						{
							this.primeNode.Reset(true);
						}
						value.Reset(true);
					}
					this.allNodes.Remove(value);
					this.allNodes.Insert(0, value);
					this.UpdateNodeIDs(true);
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00002EAB File Offset: 0x000010AB
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00002EB3 File Offset: 0x000010B3
		public Component agent { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00002EBC File Offset: 0x000010BC
		public IBlackboard blackboard
		{
			get
			{
				return this.localBlackboard;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002EC4 File Offset: 0x000010C4
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00002ECC File Offset: 0x000010CC
		public IBlackboard parentBlackboard { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00002ED5 File Offset: 0x000010D5
		Object ITaskSystem.contextObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002ED8 File Offset: 0x000010D8
		public void UpdateReferencesFromOwner(GraphOwner owner, bool force = false)
		{
			this.UpdateReferences(owner, (owner != null) ? owner.blackboard : null, force);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002EF4 File Offset: 0x000010F4
		public void UpdateReferences(Component newAgent, IBlackboard newParentBlackboard, bool force = false)
		{
			if (this.agent != newAgent || this.parentBlackboard != newParentBlackboard || force)
			{
				this.agent = newAgent;
				this.parentBlackboard = newParentBlackboard;
				if (newParentBlackboard != this.localBlackboard && this.allowBlackboardOverrides)
				{
					this.localBlackboard.parent = newParentBlackboard;
				}
				else
				{
					this.localBlackboard.parent = null;
				}
				this.localBlackboard.propertiesBindTarget = newAgent;
				this.localBlackboard.unityContextObject = this;
				this.UpdateNodeBBFields();
				((ITaskSystem)this).UpdateTasksOwner();
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002F7C File Offset: 0x0000117C
		private void UpdateNodeBBFields()
		{
			for (int i = 0; i < this.allParameters.Count; i++)
			{
				this.allParameters[i].bb = this.blackboard;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002FB8 File Offset: 0x000011B8
		void ITaskSystem.UpdateTasksOwner()
		{
			for (int i = 0; i < this.allTasks.Count; i++)
			{
				this.allTasks[i].SetOwnerSystem(this);
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002FF0 File Offset: 0x000011F0
		public void UpdateNodeIDs(bool alsoReorderList)
		{
			if (this.allNodes.Count == 0)
			{
				return;
			}
			int lastID = -1;
			Node[] source = new Node[this.allNodes.Count];
			if (this.primeNode != null)
			{
				lastID = this.AssignNodeID(this.primeNode, lastID, ref source);
			}
			foreach (Node node in from n in this.allNodes
			orderby ((n.inConnections.Count == 0) ? 0 : 1) + n.priority * -1
			select n)
			{
				lastID = this.AssignNodeID(node, lastID, ref source);
			}
			if (alsoReorderList)
			{
				this.allNodes = source.ToList<Node>();
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000030B0 File Offset: 0x000012B0
		private int AssignNodeID(Node node, int lastID, ref Node[] parsed)
		{
			if (!parsed.Contains(node))
			{
				lastID++;
				node.ID = lastID;
				parsed[lastID] = node;
				for (int i = 0; i < node.outConnections.Count; i++)
				{
					Node targetNode = node.outConnections[i].targetNode;
					lastID = this.AssignNodeID(targetNode, lastID, ref parsed);
				}
			}
			return lastID;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600009C RID: 156 RVA: 0x0000310C File Offset: 0x0000130C
		// (remove) Token: 0x0600009D RID: 157 RVA: 0x00003144 File Offset: 0x00001344
		private event Action delayedInitCalls;

		// Token: 0x0600009E RID: 158 RVA: 0x00003179 File Offset: 0x00001379
		protected void ThreadSafeInitCall(Action call)
		{
			if (Threader.isMainThread)
			{
				call.Invoke();
				return;
			}
			this.delayedInitCalls += call;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003190 File Offset: 0x00001390
		public void LoadOverwriteAsync(GraphLoadData data, Action callback)
		{
			Graph.<LoadOverwriteAsync>d__161 <LoadOverwriteAsync>d__;
			<LoadOverwriteAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<LoadOverwriteAsync>d__.<>4__this = this;
			<LoadOverwriteAsync>d__.data = data;
			<LoadOverwriteAsync>d__.callback = callback;
			<LoadOverwriteAsync>d__.<>1__state = -1;
			<LoadOverwriteAsync>d__.<>t__builder.Start<Graph.<LoadOverwriteAsync>d__161>(ref <LoadOverwriteAsync>d__);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000031D8 File Offset: 0x000013D8
		public void LoadOverwrite(GraphLoadData data)
		{
			this.SetGraphSourceMetaData(data.source);
			this.Deserialize(data.json, data.references, false);
			this.UpdateReferences(data.agent, data.parentBlackboard, false);
			this.Validate();
			this.OnGraphInitialize();
			if (data.preInitializeSubGraphs)
			{
				this.ThreadSafeInitCall(new Action(this.PreInitializeSubGraphs));
			}
			this.ThreadSafeInitCall(delegate
			{
				this.localBlackboard.InitializePropertiesBinding(data.agent, false);
			});
			this.hasInitialized = true;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003289 File Offset: 0x00001489
		public void Initialize(Component newAgent, IBlackboard newParentBlackboard, bool preInitializeSubGraphs)
		{
			this.UpdateReferences(newAgent, newParentBlackboard, false);
			this.OnGraphInitialize();
			if (preInitializeSubGraphs)
			{
				this.PreInitializeSubGraphs();
			}
			this.localBlackboard.InitializePropertiesBinding(newAgent, false);
			this.hasInitialized = true;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000032B8 File Offset: 0x000014B8
		private void PreInitializeSubGraphs()
		{
			foreach (IGraphAssignable assignable in this.allNodes.OfType<IGraphAssignable>())
			{
				Graph graph = assignable.CheckInstance();
				if (graph != null)
				{
					graph.Initialize(this.agent, this.blackboard.parent, true);
				}
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000332C File Offset: 0x0000152C
		public void StartGraph(Component newAgent, IBlackboard newParentBlackboard, Graph.UpdateMode newUpdateMode, Action<bool> callback = null)
		{
			if (newAgent == null && this.requiresAgent)
			{
				return;
			}
			if (this.primeNode == null && this.requiresPrimeNode)
			{
				return;
			}
			if (this.isRunning && !this.isPaused)
			{
				return;
			}
			if (!this.hasInitialized)
			{
				this.Initialize(newAgent, newParentBlackboard, false);
			}
			else
			{
				this.UpdateReferences(newAgent, newParentBlackboard, false);
			}
			if (callback != null)
			{
				this.onFinish = callback;
			}
			if (this.isRunning && this.isPaused)
			{
				this.Resume();
				return;
			}
			if (Graph._runningGraphs == null)
			{
				Graph._runningGraphs = new List<Graph>();
			}
			Graph._runningGraphs.Add(this);
			this.elapsedTime = 0f;
			this.isRunning = true;
			this.isPaused = false;
			this.OnGraphStarted();
			for (int i = 0; i < this.allNodes.Count; i++)
			{
				this.allNodes[i].OnGraphStarted();
			}
			for (int j = 0; j < this.allNodes.Count; j++)
			{
				this.allNodes[j].OnPostGraphStarted();
			}
			if (this.isRunning)
			{
				this.updateMode = newUpdateMode;
				if (this.updateMode != Graph.UpdateMode.Manual)
				{
					MonoManager.current.AddUpdateCall((MonoManager.UpdateMode)this.updateMode, new Action(this.UpdateGraph));
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000346C File Offset: 0x0000166C
		public void Stop(bool success = true)
		{
			if (!this.isRunning)
			{
				return;
			}
			Graph._runningGraphs.Remove(this);
			if (this.updateMode != Graph.UpdateMode.Manual)
			{
				MonoManager.current.RemoveUpdateCall((MonoManager.UpdateMode)this.updateMode, new Action(this.UpdateGraph));
			}
			for (int i = 0; i < this.allNodes.Count; i++)
			{
				Node node = this.allNodes[i];
				if (node is IGraphAssignable)
				{
					(node as IGraphAssignable).TryStopSubGraph();
				}
				node.Reset(false);
				node.OnGraphStoped();
			}
			for (int j = 0; j < this.allNodes.Count; j++)
			{
				this.allNodes[j].OnPostGraphStoped();
			}
			this.OnGraphStoped();
			this.isRunning = false;
			this.isPaused = false;
			if (this.onFinish != null)
			{
				this.onFinish.Invoke(success);
				this.onFinish = null;
			}
			this.elapsedTime = 0f;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003558 File Offset: 0x00001758
		public void Pause()
		{
			if (!this.isRunning || this.isPaused)
			{
				return;
			}
			if (this.updateMode != Graph.UpdateMode.Manual)
			{
				MonoManager.current.RemoveUpdateCall((MonoManager.UpdateMode)this.updateMode, new Action(this.UpdateGraph));
			}
			this.isRunning = true;
			this.isPaused = true;
			for (int i = 0; i < this.allNodes.Count; i++)
			{
				Node node = this.allNodes[i];
				if (node is IGraphAssignable)
				{
					(node as IGraphAssignable).TryPauseSubGraph();
				}
				node.OnGraphPaused();
			}
			this.OnGraphPaused();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000035EC File Offset: 0x000017EC
		public void Resume()
		{
			if (!this.isRunning || !this.isPaused)
			{
				return;
			}
			this.isRunning = true;
			this.isPaused = false;
			this.OnGraphUnpaused();
			for (int i = 0; i < this.allNodes.Count; i++)
			{
				Node node = this.allNodes[i];
				if (node is IGraphAssignable)
				{
					(node as IGraphAssignable).TryResumeSubGraph();
				}
				node.OnGraphUnpaused();
			}
			if (this.updateMode != Graph.UpdateMode.Manual)
			{
				MonoManager.current.AddUpdateCall((MonoManager.UpdateMode)this.updateMode, new Action(this.UpdateGraph));
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003680 File Offset: 0x00001880
		public void Restart()
		{
			this.Stop(true);
			this.StartGraph(this.agent, this.blackboard, this.updateMode, this.onFinish);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000036A7 File Offset: 0x000018A7
		public void UpdateGraph()
		{
			this.UpdateGraph(Time.deltaTime);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000036B4 File Offset: 0x000018B4
		public void UpdateGraph(float deltaTime)
		{
			if (this.isRunning)
			{
				this.deltaTime = deltaTime;
				this.elapsedTime += deltaTime;
				this.lastUpdateFrame = Time.frameCount;
				this.OnGraphUpdate();
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000036E4 File Offset: 0x000018E4
		public virtual object OnDerivedDataSerialization()
		{
			return null;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000036E7 File Offset: 0x000018E7
		public virtual void OnDerivedDataDeserialization(object data)
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000036E9 File Offset: 0x000018E9
		protected virtual void OnGraphInitialize()
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000036EB File Offset: 0x000018EB
		protected virtual void OnGraphStarted()
		{
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000036ED File Offset: 0x000018ED
		protected virtual void OnGraphUpdate()
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000036EF File Offset: 0x000018EF
		protected virtual void OnGraphStoped()
		{
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000036F1 File Offset: 0x000018F1
		protected virtual void OnGraphPaused()
		{
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000036F3 File Offset: 0x000018F3
		protected virtual void OnGraphUnpaused()
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000036F5 File Offset: 0x000018F5
		protected virtual void OnGraphObjectEnable()
		{
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000036F7 File Offset: 0x000018F7
		protected virtual void OnGraphObjectDisable()
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000036F9 File Offset: 0x000018F9
		protected virtual void OnGraphObjectDestroy()
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000036FB File Offset: 0x000018FB
		protected virtual void OnGraphValidate()
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003700 File Offset: 0x00001900
		public void SendEvent(string name, object value, object sender)
		{
			if (this.agent == null || !this.isRunning)
			{
				return;
			}
			EventRouter component = this.agent.GetComponent<EventRouter>();
			if (component != null)
			{
				component.InvokeCustomEvent(name, value, sender);
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003744 File Offset: 0x00001944
		public void SendEvent<T>(string name, T value, object sender)
		{
			if (this.agent == null || !this.isRunning)
			{
				return;
			}
			EventRouter component = this.agent.GetComponent<EventRouter>();
			if (component != null)
			{
				component.InvokeCustomEvent<T>(name, value, sender);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003788 File Offset: 0x00001988
		public static void SendGlobalEvent(string name, object value, object sender)
		{
			if (Graph._runningGraphs == null)
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			foreach (Graph graph in Graph._runningGraphs.ToArray())
			{
				if (graph.agent != null && !list.Contains(graph.agent.gameObject))
				{
					list.Add(graph.agent.gameObject);
					graph.SendEvent(name, value, sender);
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000037FC File Offset: 0x000019FC
		public static void SendGlobalEvent<T>(string name, T value, object sender)
		{
			if (Graph._runningGraphs == null)
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			foreach (Graph graph in Graph._runningGraphs.ToArray())
			{
				if (graph.agent != null && !list.Contains(graph.agent.gameObject))
				{
					list.Add(graph.agent.gameObject);
					graph.SendEvent<T>(name, value, sender);
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000386F File Offset: 0x00001A6F
		public IEnumerable<BBParameter> GetAllParameters()
		{
			return this.allParameters;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003877 File Offset: 0x00001A77
		public IEnumerable<Connection> GetAllConnections()
		{
			return this.allConnections;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000387F File Offset: 0x00001A7F
		public IEnumerable<T> GetAllTasksOfType<T>() where T : Task
		{
			return this.allTasks.OfType<T>();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000388C File Offset: 0x00001A8C
		public Node GetNodeWithID(int searchID)
		{
			if (searchID < this.allNodes.Count && searchID >= 0)
			{
				return this.allNodes.Find((Node n) => n.ID == searchID);
			}
			return null;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000038DB File Offset: 0x00001ADB
		public IEnumerable<T> GetAllNodesOfType<T>() where T : Node
		{
			return this.allNodes.OfType<T>();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000038E8 File Offset: 0x00001AE8
		public T GetNodeWithTag<T>(string tagName) where T : Node
		{
			return this.allNodes.OfType<T>().FirstOrDefault((T n) => n.tag == tagName);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003920 File Offset: 0x00001B20
		public IEnumerable<T> GetNodesWithTag<T>(string tagName) where T : Node
		{
			return from n in this.allNodes.OfType<T>()
			where n.tag == tagName
			select n;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003956 File Offset: 0x00001B56
		public IEnumerable<T> GetAllTagedNodes<T>() where T : Node
		{
			return from n in this.allNodes.OfType<T>()
			where !string.IsNullOrEmpty(n.tag)
			select n;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003987 File Offset: 0x00001B87
		public IEnumerable<Node> GetRootNodes()
		{
			return from n in this.allNodes
			where n.inConnections.Count == 0
			select n;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000039B3 File Offset: 0x00001BB3
		public IEnumerable<Node> GetLeafNodes()
		{
			return from n in this.allNodes
			where n.outConnections.Count == 0
			select n;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000039E0 File Offset: 0x00001BE0
		public IEnumerable<T> GetAllNestedGraphs<T>(bool recursive) where T : Graph
		{
			List<T> list = new List<T>();
			foreach (IGraphAssignable graphAssignable in this.allNodes.OfType<IGraphAssignable>())
			{
				if (graphAssignable.subGraph is T)
				{
					list.Add((T)((object)graphAssignable.subGraph));
					if (recursive)
					{
						list.AddRange(graphAssignable.subGraph.GetAllNestedGraphs<T>(recursive));
					}
				}
			}
			return list.Distinct<T>();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003A6C File Offset: 0x00001C6C
		public IEnumerable<Graph> GetAllInstancedNestedGraphs()
		{
			List<Graph> list = new List<Graph>();
			foreach (IGraphAssignable graphAssignable in this.allNodes.OfType<IGraphAssignable>())
			{
				if (graphAssignable.instances != null)
				{
					Dictionary<Graph, Graph>.ValueCollection values = graphAssignable.instances.Values;
					list.AddRange(values);
					foreach (Graph graph in values)
					{
						list.AddRange(graph.GetAllInstancedNestedGraphs());
					}
				}
			}
			return list;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003B20 File Offset: 0x00001D20
		public IEnumerable<BBParameter> GetDefinedParameters()
		{
			return from p in this.allParameters
			where p != null && p.isDefined
			select p;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003B4C File Offset: 0x00001D4C
		public void PromoteMissingParametersToVariables(IBlackboard bb)
		{
			foreach (BBParameter bbparameter in this.GetDefinedParameters())
			{
				if (bbparameter.varRef == null && !bbparameter.isPresumedDynamic)
				{
					bbparameter.PromoteToVariable(bb);
				}
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003BAC File Offset: 0x00001DAC
		public static Graph GetElementGraph(object obj)
		{
			if (obj is GraphOwner)
			{
				return (obj as GraphOwner).graph;
			}
			if (obj is Graph)
			{
				return (Graph)obj;
			}
			if (obj is Node)
			{
				return (obj as Node).graph;
			}
			if (obj is Connection)
			{
				return (obj as Connection).graph;
			}
			if (obj is Task)
			{
				return (obj as Task).ownerSystem as Graph;
			}
			if (obj is BlackboardSource)
			{
				return (obj as BlackboardSource).unityContextObject as Graph;
			}
			return null;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003C38 File Offset: 0x00001E38
		public HierarchyTree.Element GetFlatMetaGraph()
		{
			if (this.flatMetaGraph != null)
			{
				return this.flatMetaGraph;
			}
			HierarchyTree.Element element = new HierarchyTree.Element(this);
			int num = 0;
			for (int i = 0; i < this.allNodes.Count; i++)
			{
				element.AddChild(Graph.GetTreeNodeElement(this.allNodes[i], false, ref num));
			}
			return this.flatMetaGraph = element;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003C98 File Offset: 0x00001E98
		public HierarchyTree.Element GetFullMetaGraph()
		{
			if (this.fullMetaGraph != null)
			{
				return this.fullMetaGraph;
			}
			HierarchyTree.Element element = new HierarchyTree.Element(this);
			int num = 0;
			if (this.primeNode != null)
			{
				element.AddChild(Graph.GetTreeNodeElement(this.primeNode, true, ref num));
			}
			for (int i = 0; i < this.allNodes.Count; i++)
			{
				Node node = this.allNodes[i];
				if (node.ID > num && node.inConnections.Count == 0)
				{
					element.AddChild(Graph.GetTreeNodeElement(node, true, ref num));
				}
			}
			return this.fullMetaGraph = element;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003D30 File Offset: 0x00001F30
		public HierarchyTree.Element GetNestedMetaGraph()
		{
			if (this.nestedMetaGraph != null)
			{
				return this.nestedMetaGraph;
			}
			HierarchyTree.Element element = new HierarchyTree.Element(this);
			Graph.DigNestedGraphs(this, element);
			return this.nestedMetaGraph = element;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003D64 File Offset: 0x00001F64
		private static void DigNestedGraphs(Graph currentGraph, HierarchyTree.Element currentElement)
		{
			for (int i = 0; i < currentGraph.allNodes.Count; i++)
			{
				IGraphAssignable graphAssignable = currentGraph.allNodes[i] as IGraphAssignable;
				if (graphAssignable != null && graphAssignable.subGraph != null)
				{
					Graph.DigNestedGraphs(graphAssignable.subGraph, currentElement.AddChild(new HierarchyTree.Element(graphAssignable)));
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003DC4 File Offset: 0x00001FC4
		private static HierarchyTree.Element GetTreeNodeElement(Node node, bool recurse, ref int lastID)
		{
			HierarchyTree.Element element = Graph.CollectSubElements(node);
			for (int i = 0; i < node.outConnections.Count; i++)
			{
				HierarchyTree.Element element2 = Graph.CollectSubElements(node.outConnections[i]);
				element.AddChild(element2);
				if (recurse)
				{
					Node targetNode = node.outConnections[i].targetNode;
					if (targetNode.ID > node.ID)
					{
						element2.AddChild(Graph.GetTreeNodeElement(targetNode, recurse, ref lastID));
					}
				}
			}
			lastID = node.ID;
			return element;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003E44 File Offset: 0x00002044
		private static HierarchyTree.Element CollectSubElements(IGraphElement obj)
		{
			HierarchyTree.Element parentElement = null;
			Stack<HierarchyTree.Element> stack = new Stack<HierarchyTree.Element>();
			JSONSerializer.SerializeAndExecuteNoCycles(obj.GetType(), obj, delegate(object o)
			{
				if (o is ISerializationCollectable)
				{
					HierarchyTree.Element element = new HierarchyTree.Element(o);
					if (stack.Count > 0)
					{
						stack.Peek().AddChild(element);
					}
					stack.Push(element);
				}
			}, delegate(object o, fsData d)
			{
				if (o is ISerializationCollectable)
				{
					parentElement = stack.Pop();
				}
			});
			return parentElement;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003E94 File Offset: 0x00002094
		public IGraphElement GetTaskParentElement(Task targetTask)
		{
			HierarchyTree.Element element = this.GetFlatMetaGraph().FindReferenceElement(targetTask);
			if (element == null)
			{
				return null;
			}
			return element.GetFirstParentReferenceOfType<IGraphElement>();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003EBC File Offset: 0x000020BC
		public IGraphElement GetParameterParentElement(BBParameter targetParameter)
		{
			HierarchyTree.Element element = this.GetFlatMetaGraph().FindReferenceElement(targetParameter);
			if (element == null)
			{
				return null;
			}
			return element.GetFirstParentReferenceOfType<IGraphElement>();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003EE4 File Offset: 0x000020E4
		public static IEnumerable<Task> GetTasksInElement(IGraphElement target)
		{
			List<Task> result = new List<Task>();
			JSONSerializer.SerializeAndExecuteNoCycles(target.GetType(), target, delegate(object o, fsData d)
			{
				if (o is Task)
				{
					result.Add((Task)o);
				}
			});
			return result;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003F20 File Offset: 0x00002120
		public static IEnumerable<BBParameter> GetParametersInElement(IGraphElement target)
		{
			List<BBParameter> result = new List<BBParameter>();
			JSONSerializer.SerializeAndExecuteNoCycles(target.GetType(), target, delegate(object o, fsData d)
			{
				if (o is BBParameter)
				{
					result.Add((BBParameter)o);
				}
			});
			return result;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003F5C File Offset: 0x0000215C
		public T AddNode<T>() where T : Node
		{
			return (T)((object)this.AddNode(typeof(T)));
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003F73 File Offset: 0x00002173
		public T AddNode<T>(Vector2 pos) where T : Node
		{
			return (T)((object)this.AddNode(typeof(T), pos));
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003F8B File Offset: 0x0000218B
		public Node AddNode(Type nodeType)
		{
			return this.AddNode(nodeType, new Vector2(0f, 0f));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003FA4 File Offset: 0x000021A4
		public Node AddNode(Type nodeType, Vector2 pos)
		{
			if (!nodeType.RTIsSubclassOf(this.baseNodeType))
			{
				return null;
			}
			Node node = Node.Create(this, nodeType, pos);
			this.allNodes.Add(node);
			if (this.primeNode == null)
			{
				this.primeNode = node;
			}
			this.UpdateNodeIDs(false);
			return node;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003FF0 File Offset: 0x000021F0
		public void RemoveNode(Node node, bool recordUndo = true, bool force = false)
		{
			if (!force && node.GetType().RTIsDefined(true) && (from n in this.allNodes
			where n.GetType() == node.GetType()
			select n).Count<Node>() == 1)
			{
				return;
			}
			if (!this.allNodes.Contains(node))
			{
				return;
			}
			if (!Application.isPlaying && this.isTree && node.inConnections.Count == 1 && node.outConnections.Count == 1)
			{
				Node targetNode = node.outConnections[0].targetNode;
				if (targetNode != node.inConnections[0].sourceNode)
				{
					this.RemoveConnection(node.outConnections[0], true);
					node.inConnections[0].SetTargetNode(targetNode, -1);
				}
			}
			node.OnDestroy();
			int count = node.inConnections.Count;
			while (count-- > 0)
			{
				this.RemoveConnection(node.inConnections[count], true);
			}
			int count2 = node.outConnections.Count;
			while (count2-- > 0)
			{
				this.RemoveConnection(node.outConnections[count2], true);
			}
			this.allNodes.Remove(node);
			if (node == this.primeNode)
			{
				this.primeNode = this.GetNodeWithID(this.primeNode.ID + 1);
			}
			this.UpdateNodeIDs(false);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000041A3 File Offset: 0x000023A3
		public Connection ConnectNodes(Node sourceNode, Node targetNode, int sourceIndex = -1, int targetIndex = -1)
		{
			if (!Node.IsNewConnectionAllowed(sourceNode, targetNode, null))
			{
				return null;
			}
			Connection result = Connection.Create(sourceNode, targetNode, sourceIndex, targetIndex);
			this.UpdateNodeIDs(false);
			return result;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000041C4 File Offset: 0x000023C4
		public void RemoveConnection(Connection connection, bool recordUndo = true)
		{
			if (Application.isPlaying)
			{
				connection.Reset(true);
			}
			connection.OnDestroy();
			connection.sourceNode.OnChildDisconnected(connection.sourceNode.outConnections.IndexOf(connection));
			connection.targetNode.OnParentDisconnected(connection.targetNode.inConnections.IndexOf(connection));
			connection.sourceNode.outConnections.Remove(connection);
			connection.targetNode.inConnections.Remove(connection);
			this.UpdateNodeIDs(false);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000424C File Offset: 0x0000244C
		public static List<Node> CloneNodes(List<Node> originalNodes, Graph targetGraph = null, Vector2 originPosition = default(Vector2))
		{
			if (targetGraph != null && originalNodes.Any((Node n) => !n.GetType().IsSubclassOf(targetGraph.baseNodeType)))
			{
				return null;
			}
			List<Node> list = new List<Node>();
			Dictionary<Connection, KeyValuePair<int, int>> dictionary = new Dictionary<Connection, KeyValuePair<int, int>>();
			foreach (Node node in originalNodes)
			{
				Node node2 = (targetGraph != null) ? node.Duplicate(targetGraph) : JSONSerializer.Clone<Node>(node);
				list.Add(node2);
				foreach (Connection connection in node.outConnections)
				{
					int num = originalNodes.IndexOf(connection.sourceNode);
					int num2 = originalNodes.IndexOf(connection.targetNode);
					dictionary[connection] = new KeyValuePair<int, int>(num, num2);
				}
			}
			foreach (KeyValuePair<Connection, KeyValuePair<int, int>> keyValuePair in dictionary)
			{
				if (keyValuePair.Value.Value != -1)
				{
					Node newSource = list[keyValuePair.Value.Key];
					Node newTarget = list[keyValuePair.Value.Value];
					keyValuePair.Key.Duplicate(newSource, newTarget);
				}
			}
			if (originPosition != default(Vector2) && list.Count > 0)
			{
				if (list.Count == 1)
				{
					list[0].position = originPosition;
				}
				else
				{
					Vector2 b = list[0].position - originPosition;
					list[0].position = originPosition;
					for (int i = 1; i < list.Count; i++)
					{
						list[i].position -= b;
					}
				}
			}
			if (targetGraph != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					list[j].Validate(targetGraph);
				}
			}
			return list;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000044B8 File Offset: 0x000026B8
		public void ClearGraph()
		{
			this.canvasGroups = null;
			foreach (Node node in this.allNodes.ToArray())
			{
				this.RemoveNode(node, true, false);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000044F3 File Offset: 0x000026F3
		[Obsolete("Use 'Graph.StartGraph' with the 'Graph.UpdateMode' parameter.")]
		public void StartGraph(Component newAgent, IBlackboard newBlackboard, bool autoUpdate, Action<bool> callback = null)
		{
			this.StartGraph(newAgent, newBlackboard, autoUpdate ? Graph.UpdateMode.NormalUpdate : Graph.UpdateMode.Manual, callback);
		}

		// Token: 0x0400001E RID: 30
		[SerializeField]
		private string _serializedGraph;

		// Token: 0x0400001F RID: 31
		[SerializeField]
		private List<Object> _objectReferences;

		// Token: 0x04000020 RID: 32
		[SerializeField]
		private GraphSource _graphSource = new GraphSource();

		// Token: 0x04000021 RID: 33
		[SerializeField]
		private bool _haltSerialization;

		// Token: 0x04000022 RID: 34
		[SerializeField]
		[Tooltip("An external text asset file to serialize the graph on top of the internal serialization")]
		private TextAsset _externalSerializationFile;

		// Token: 0x04000023 RID: 35
		[NonSerialized]
		private bool haltForUndo;

		// Token: 0x04000027 RID: 39
		private static List<Graph> _runningGraphs;

		// Token: 0x020000EA RID: 234
		public enum UpdateMode
		{
			// Token: 0x0400025B RID: 603
			NormalUpdate,
			// Token: 0x0400025C RID: 604
			LateUpdate,
			// Token: 0x0400025D RID: 605
			FixedUpdate,
			// Token: 0x0400025E RID: 606
			Manual
		}
	}
}
