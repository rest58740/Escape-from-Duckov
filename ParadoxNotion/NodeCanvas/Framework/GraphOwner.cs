using System;
using System.Collections.Generic;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Serialization;
using ParadoxNotion.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace NodeCanvas.Framework
{
	// Token: 0x0200000D RID: 13
	public abstract class GraphOwner : MonoBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00004519 File Offset: 0x00002719
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00004521 File Offset: 0x00002721
		public List<ExposedParameter> exposedParameters { get; set; }

		// Token: 0x060000E0 RID: 224 RVA: 0x0000452C File Offset: 0x0000272C
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.exposedParameters == null || this.exposedParameters.Count == 0)
			{
				this._serializedExposedParameters = null;
				return;
			}
			this._serializedExposedParameters = new SerializationPair[this.exposedParameters.Count];
			for (int i = 0; i < this._serializedExposedParameters.Length; i++)
			{
				SerializationPair serializationPair = new SerializationPair();
				serializationPair._json = JSONSerializer.Serialize(typeof(ExposedParameter), this.exposedParameters[i], serializationPair._references, false);
				this._serializedExposedParameters[i] = serializationPair;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000045B8 File Offset: 0x000027B8
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this._serializedExposedParameters != null)
			{
				if (this.exposedParameters == null)
				{
					this.exposedParameters = new List<ExposedParameter>();
				}
				else
				{
					this.exposedParameters.Clear();
				}
				for (int i = 0; i < this._serializedExposedParameters.Length; i++)
				{
					ExposedParameter exposedParameter = JSONSerializer.Deserialize<ExposedParameter>(this._serializedExposedParameters[i]._json, this._serializedExposedParameters[i]._references);
					this.exposedParameters.Add(exposedParameter);
				}
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000E2 RID: 226 RVA: 0x0000462C File Offset: 0x0000282C
		// (remove) Token: 0x060000E3 RID: 227 RVA: 0x00004660 File Offset: 0x00002860
		public static event Action<GraphOwner> onOwnerBehaviourStateChange;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000E4 RID: 228 RVA: 0x00004694 File Offset: 0x00002894
		// (remove) Token: 0x060000E5 RID: 229 RVA: 0x000046CC File Offset: 0x000028CC
		public event Action onMonoBehaviourStart;

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E6 RID: 230
		// (set) Token: 0x060000E7 RID: 231
		public abstract Graph graph { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E8 RID: 232
		// (set) Token: 0x060000E9 RID: 233
		public abstract IBlackboard blackboard { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000EA RID: 234
		public abstract Type graphType { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004701 File Offset: 0x00002901
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00004709 File Offset: 0x00002909
		public bool initialized { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004712 File Offset: 0x00002912
		// (set) Token: 0x060000EE RID: 238 RVA: 0x0000471A File Offset: 0x0000291A
		public bool enableCalled { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004723 File Offset: 0x00002923
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x0000472B File Offset: 0x0000292B
		public bool startCalled { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004734 File Offset: 0x00002934
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x0000473C File Offset: 0x0000293C
		public GraphSource boundGraphSource
		{
			get
			{
				return this._boundGraphSource;
			}
			private set
			{
				this._boundGraphSource = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004745 File Offset: 0x00002945
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x0000474D File Offset: 0x0000294D
		public string boundGraphSerialization
		{
			get
			{
				return this._boundGraphSerialization;
			}
			private set
			{
				this._boundGraphSerialization = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004756 File Offset: 0x00002956
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x0000475E File Offset: 0x0000295E
		public List<Object> boundGraphObjectReferences
		{
			get
			{
				return this._boundGraphObjectReferences;
			}
			private set
			{
				this._boundGraphObjectReferences = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004767 File Offset: 0x00002967
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004779 File Offset: 0x00002979
		public bool lockBoundGraphPrefabOverrides
		{
			get
			{
				return this._lockBoundGraphPrefabOverrides && this.graphIsBound;
			}
			set
			{
				this._lockBoundGraphPrefabOverrides = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004782 File Offset: 0x00002982
		// (set) Token: 0x060000FA RID: 250 RVA: 0x0000478A File Offset: 0x0000298A
		public bool preInitializeSubGraphs
		{
			get
			{
				return this._preInitializeSubGraphs;
			}
			set
			{
				this._preInitializeSubGraphs = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004793 File Offset: 0x00002993
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000479B File Offset: 0x0000299B
		public GraphOwner.FirstActivation firstActivation
		{
			get
			{
				return this._firstActivation;
			}
			set
			{
				this._firstActivation = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000047A4 File Offset: 0x000029A4
		// (set) Token: 0x060000FE RID: 254 RVA: 0x000047AC File Offset: 0x000029AC
		public GraphOwner.EnableAction enableAction
		{
			get
			{
				return this._enableAction;
			}
			set
			{
				this._enableAction = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000047B5 File Offset: 0x000029B5
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000047BD File Offset: 0x000029BD
		public GraphOwner.DisableAction disableAction
		{
			get
			{
				return this._disableAction;
			}
			set
			{
				this._disableAction = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000047C6 File Offset: 0x000029C6
		// (set) Token: 0x06000102 RID: 258 RVA: 0x000047CE File Offset: 0x000029CE
		public Graph.UpdateMode updateMode
		{
			get
			{
				return this._updateMode;
			}
			set
			{
				this._updateMode = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000047D7 File Offset: 0x000029D7
		public bool graphIsBound
		{
			get
			{
				return !string.IsNullOrEmpty(this.boundGraphSerialization);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000047E7 File Offset: 0x000029E7
		public bool isRunning
		{
			get
			{
				return this.graph != null && this.graph.isRunning;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004804 File Offset: 0x00002A04
		public bool isPaused
		{
			get
			{
				return this.graph != null && this.graph.isPaused;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00004821 File Offset: 0x00002A21
		public float elapsedTime
		{
			get
			{
				if (!(this.graph != null))
				{
					return 0f;
				}
				return this.graph.elapsedTime;
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004844 File Offset: 0x00002A44
		protected Graph GetInstance(Graph originalGraph)
		{
			if (originalGraph == null)
			{
				return null;
			}
			if (!Application.isPlaying)
			{
				return originalGraph;
			}
			if (this.instances.ContainsValue(originalGraph))
			{
				return originalGraph;
			}
			Graph graph;
			if (!this.instances.TryGetValue(originalGraph, ref graph))
			{
				graph = Graph.Clone<Graph>(originalGraph, null);
				this.instances[originalGraph] = graph;
			}
			return graph;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000489C File Offset: 0x00002A9C
		public Graph MakeRuntimeGraphInstance()
		{
			return this.graph = this.GetInstance(this.graph);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000048BE File Offset: 0x00002ABE
		public void StartBehaviour()
		{
			this.StartBehaviour(this.updateMode, null);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000048CD File Offset: 0x00002ACD
		public void StartBehaviour(Action<bool> callback)
		{
			this.StartBehaviour(this.updateMode, callback);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000048DC File Offset: 0x00002ADC
		public void StartBehaviour(Graph.UpdateMode updateMode, Action<bool> callback = null)
		{
			this.graph = this.GetInstance(this.graph);
			if (this.graph != null)
			{
				this.graph.StartGraph(this, this.blackboard, updateMode, callback);
				Action<GraphOwner> action = GraphOwner.onOwnerBehaviourStateChange;
				if (action == null)
				{
					return;
				}
				action.Invoke(this);
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000492D File Offset: 0x00002B2D
		public void PauseBehaviour()
		{
			if (this.graph != null)
			{
				this.graph.Pause();
				Action<GraphOwner> action = GraphOwner.onOwnerBehaviourStateChange;
				if (action == null)
				{
					return;
				}
				action.Invoke(this);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004958 File Offset: 0x00002B58
		public void StopBehaviour(bool success = true)
		{
			if (this.graph != null)
			{
				this.graph.Stop(success);
				Action<GraphOwner> action = GraphOwner.onOwnerBehaviourStateChange;
				if (action == null)
				{
					return;
				}
				action.Invoke(this);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004984 File Offset: 0x00002B84
		public void UpdateBehaviour()
		{
			Graph graph = this.graph;
			if (graph == null)
			{
				return;
			}
			graph.UpdateGraph();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004996 File Offset: 0x00002B96
		public void RestartBehaviour()
		{
			this.StopBehaviour(true);
			this.StartBehaviour();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000049A5 File Offset: 0x00002BA5
		public void SendEvent(string eventName)
		{
			Graph graph = this.graph;
			if (graph == null)
			{
				return;
			}
			graph.SendEvent(eventName, null, null);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000049BA File Offset: 0x00002BBA
		public void SendEvent(string eventName, object value, object sender)
		{
			Graph graph = this.graph;
			if (graph == null)
			{
				return;
			}
			graph.SendEvent(eventName, value, sender);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000049CF File Offset: 0x00002BCF
		public void SendEvent<T>(string eventName, T eventValue, object sender)
		{
			Graph graph = this.graph;
			if (graph == null)
			{
				return;
			}
			graph.SendEvent<T>(eventName, eventValue, sender);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000049E4 File Offset: 0x00002BE4
		public T GetExposedParameterValue<T>(string name)
		{
			ExposedParameter exposedParameter = this.exposedParameters.Find((ExposedParameter x) => x.varRefBoxed != null && x.varRefBoxed.name == name);
			if (exposedParameter == null)
			{
				return default(T);
			}
			return (exposedParameter as ExposedParameter<T>).value;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004A30 File Offset: 0x00002C30
		public void SetExposedParameterValue<T>(string name, T value)
		{
			List<ExposedParameter> exposedParameters = this.exposedParameters;
			ExposedParameter exposedParameter = (exposedParameters != null) ? exposedParameters.Find((ExposedParameter x) => x.varRefBoxed != null && x.varRefBoxed.name == name) : null;
			if (exposedParameter == null)
			{
				exposedParameter = this.MakeNewExposedParameter<T>(name);
			}
			if (exposedParameter != null)
			{
				(exposedParameter as ExposedParameter<T>).value = value;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004A88 File Offset: 0x00002C88
		public ExposedParameter MakeNewExposedParameter<T>(string name)
		{
			if (this.exposedParameters == null)
			{
				this.exposedParameters = new List<ExposedParameter>();
			}
			Variable<T> variable = this.graph.blackboard.GetVariable(name);
			if (variable != null && variable.isExposedPublic && !variable.isPropertyBound)
			{
				ExposedParameter exposedParameter = ExposedParameter.CreateInstance(variable);
				exposedParameter.Bind(this.graph.blackboard);
				this.exposedParameters.Add(exposedParameter);
				return exposedParameter;
			}
			return null;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004AF4 File Offset: 0x00002CF4
		protected void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004AFC File Offset: 0x00002CFC
		public void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			if (this.graph == null && !this.graphIsBound)
			{
				return;
			}
			Graph graph = (Graph)ScriptableObject.CreateInstance(this.graphType);
			GraphSource source;
			string json;
			List<Object> references;
			if (this.graphIsBound)
			{
				graph.name = this.graphType.Name;
				source = this.boundGraphSource;
				json = this.boundGraphSerialization;
				references = this.boundGraphObjectReferences;
				this.instances[graph] = graph;
			}
			else
			{
				graph.name = this.graph.name;
				source = this.graph.GetGraphSource();
				json = this.graph.GetSerializedJsonData();
				references = this.graph.GetSerializedReferencesData();
				this.instances[this.graph] = graph;
			}
			this.graph = graph;
			GraphLoadData data = default(GraphLoadData);
			data.source = source;
			data.json = json;
			data.references = references;
			data.agent = this;
			data.parentBlackboard = this.blackboard;
			data.preInitializeSubGraphs = this.preInitializeSubGraphs;
			if (this.firstActivation == GraphOwner.FirstActivation.Async)
			{
				this.graph.LoadOverwriteAsync(data, delegate
				{
					this.BindExposedParameters();
					if (!this.isRunning && this.enableAction == GraphOwner.EnableAction.EnableBehaviour && base.gameObject.activeInHierarchy)
					{
						this.StartBehaviour();
						this.InvokeStartEvent();
					}
					this.initialized = true;
				});
				return;
			}
			this.graph.LoadOverwrite(data);
			this.BindExposedParameters();
			this.initialized = true;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004C44 File Offset: 0x00002E44
		public void BindExposedParameters()
		{
			if (this.exposedParameters != null && this.graph != null)
			{
				for (int i = 0; i < this.exposedParameters.Count; i++)
				{
					this.exposedParameters[i].Bind(this.graph.blackboard);
				}
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004C9C File Offset: 0x00002E9C
		public void UnBindExposedParameters()
		{
			if (this.exposedParameters != null)
			{
				for (int i = 0; i < this.exposedParameters.Count; i++)
				{
					this.exposedParameters[i].UnBind();
				}
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004CD8 File Offset: 0x00002ED8
		protected void OnEnable()
		{
			if ((this.firstActivation == GraphOwner.FirstActivation.OnEnable || this.enableCalled) && (!this.isRunning || this.isPaused) && this.enableAction == GraphOwner.EnableAction.EnableBehaviour)
			{
				this.StartBehaviour();
			}
			this.enableCalled = true;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004D0F File Offset: 0x00002F0F
		protected void Start()
		{
			if (this.firstActivation == GraphOwner.FirstActivation.OnStart && !this.isRunning && this.enableAction == GraphOwner.EnableAction.EnableBehaviour)
			{
				this.StartBehaviour();
			}
			this.InvokeStartEvent();
			this.startCalled = true;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00004D3D File Offset: 0x00002F3D
		private void InvokeStartEvent()
		{
			if (this.onMonoBehaviourStart != null)
			{
				this.onMonoBehaviourStart.Invoke();
				this.onMonoBehaviourStart = null;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004D59 File Offset: 0x00002F59
		protected void OnDisable()
		{
			if (this.disableAction == GraphOwner.DisableAction.DisableBehaviour)
			{
				this.StopBehaviour(true);
			}
			if (this.disableAction == GraphOwner.DisableAction.PauseBehaviour)
			{
				this.PauseBehaviour();
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004D7C File Offset: 0x00002F7C
		protected void OnDestroy()
		{
			if (Threader.applicationIsPlaying)
			{
				this.StopBehaviour(true);
			}
			foreach (Graph graph in this.instances.Values)
			{
				foreach (Graph obj in graph.GetAllInstancedNestedGraphs())
				{
					Object.Destroy(obj);
				}
				Object.Destroy(graph);
			}
		}

		// Token: 0x04000036 RID: 54
		[SerializeField]
		private SerializationPair[] _serializedExposedParameters;

		// Token: 0x0400003A RID: 58
		[SerializeField]
		[FormerlySerializedAs("boundGraphSerialization")]
		private string _boundGraphSerialization;

		// Token: 0x0400003B RID: 59
		[SerializeField]
		[FormerlySerializedAs("boundGraphObjectReferences")]
		private List<Object> _boundGraphObjectReferences;

		// Token: 0x0400003C RID: 60
		[SerializeField]
		private GraphSource _boundGraphSource = new GraphSource();

		// Token: 0x0400003D RID: 61
		[SerializeField]
		[FormerlySerializedAs("firstActivation")]
		[Tooltip("When the graph will first activate. Async mode will load the graph on a separate thread (thus no spikes), but the graph will activate a few frames later.")]
		private GraphOwner.FirstActivation _firstActivation;

		// Token: 0x0400003E RID: 62
		[SerializeField]
		[FormerlySerializedAs("enableAction")]
		[Tooltip("What will happen when the GraphOwner is enabled")]
		private GraphOwner.EnableAction _enableAction;

		// Token: 0x0400003F RID: 63
		[SerializeField]
		[FormerlySerializedAs("disableAction")]
		[Tooltip("What will happen when the GraphOwner is disabled")]
		private GraphOwner.DisableAction _disableAction;

		// Token: 0x04000040 RID: 64
		[SerializeField]
		[Tooltip("If enabled, bound graph prefab overrides in instances will not be possible")]
		private bool _lockBoundGraphPrefabOverrides = true;

		// Token: 0x04000041 RID: 65
		[SerializeField]
		[Tooltip("If enabled, all subgraphs will be pre-initialized in Awake along with the root graph, but this may have a loading performance cost")]
		private bool _preInitializeSubGraphs;

		// Token: 0x04000042 RID: 66
		[SerializeField]
		[Tooltip("Specify when (if) the behaviour is updated. Changes to this only work when the behaviour starts, or re-starts")]
		private Graph.UpdateMode _updateMode;

		// Token: 0x04000043 RID: 67
		private Dictionary<Graph, Graph> instances = new Dictionary<Graph, Graph>();

		// Token: 0x020000F8 RID: 248
		public enum EnableAction
		{
			// Token: 0x0400027A RID: 634
			EnableBehaviour,
			// Token: 0x0400027B RID: 635
			DoNothing
		}

		// Token: 0x020000F9 RID: 249
		public enum DisableAction
		{
			// Token: 0x0400027D RID: 637
			DisableBehaviour,
			// Token: 0x0400027E RID: 638
			PauseBehaviour,
			// Token: 0x0400027F RID: 639
			DoNothing
		}

		// Token: 0x020000FA RID: 250
		public enum FirstActivation
		{
			// Token: 0x04000281 RID: 641
			OnEnable,
			// Token: 0x04000282 RID: 642
			OnStart,
			// Token: 0x04000283 RID: 643
			Async
		}
	}
}
