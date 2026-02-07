using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Services;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200001B RID: 27
	[SpoofAOT]
	[fsSerializeAsReference]
	[fsDeserializeOverwrite]
	[Serializable]
	public abstract class Node : IGraphElement, ISerializationCollectable
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000052C1 File Offset: 0x000034C1
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000052C9 File Offset: 0x000034C9
		public Graph graph
		{
			get
			{
				return this._graph;
			}
			internal set
			{
				this._graph = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000052D2 File Offset: 0x000034D2
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000052DA File Offset: 0x000034DA
		public int ID
		{
			get
			{
				return this._ID;
			}
			internal set
			{
				this._ID = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000052E4 File Offset: 0x000034E4
		public string UID
		{
			get
			{
				if (!string.IsNullOrEmpty(this._UID))
				{
					return this._UID;
				}
				return this._UID = Guid.NewGuid().ToString();
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00005321 File Offset: 0x00003521
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00005329 File Offset: 0x00003529
		public List<Connection> inConnections
		{
			get
			{
				return this._inConnections;
			}
			protected set
			{
				this._inConnections = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00005332 File Offset: 0x00003532
		// (set) Token: 0x0600015B RID: 347 RVA: 0x0000533A File Offset: 0x0000353A
		public List<Connection> outConnections
		{
			get
			{
				return this._outConnections;
			}
			protected set
			{
				this._outConnections = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00005343 File Offset: 0x00003543
		// (set) Token: 0x0600015D RID: 349 RVA: 0x0000534B File Offset: 0x0000354B
		public Vector2 position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00005354 File Offset: 0x00003554
		// (set) Token: 0x0600015F RID: 351 RVA: 0x0000535C File Offset: 0x0000355C
		private string customName
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00005365 File Offset: 0x00003565
		// (set) Token: 0x06000161 RID: 353 RVA: 0x0000536D File Offset: 0x0000356D
		public string tag
		{
			get
			{
				return this._tag;
			}
			set
			{
				this._tag = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005376 File Offset: 0x00003576
		// (set) Token: 0x06000163 RID: 355 RVA: 0x0000537E File Offset: 0x0000357E
		public string comments
		{
			get
			{
				return this._comment;
			}
			set
			{
				this._comment = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00005387 File Offset: 0x00003587
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000538F File Offset: 0x0000358F
		public bool isBreakpoint
		{
			get
			{
				return this._isBreakpoint;
			}
			set
			{
				this._isBreakpoint = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00005398 File Offset: 0x00003598
		// (set) Token: 0x06000167 RID: 359 RVA: 0x000053FB File Offset: 0x000035FB
		public virtual string name
		{
			get
			{
				if (!string.IsNullOrEmpty(this.customName))
				{
					return this.customName;
				}
				if (string.IsNullOrEmpty(this._nameCache))
				{
					NameAttribute nameAttribute = base.GetType().RTGetAttribute(true);
					this._nameCache = ((nameAttribute != null) ? nameAttribute.name : base.GetType().FriendlyName(false).SplitCamelCase());
				}
				return this._nameCache;
			}
			set
			{
				this.customName = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00005404 File Offset: 0x00003604
		public virtual string description
		{
			get
			{
				if (string.IsNullOrEmpty(this._descriptionCache))
				{
					DescriptionAttribute descriptionAttribute = base.GetType().RTGetAttribute(true);
					this._descriptionCache = ((descriptionAttribute != null) ? descriptionAttribute.description : "No Description");
				}
				return this._descriptionCache;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00005448 File Offset: 0x00003648
		public virtual int priority
		{
			get
			{
				if (this._priorityCache == -2147483648)
				{
					ExecutionPriorityAttribute executionPriorityAttribute = base.GetType().RTGetAttribute(true);
					this._priorityCache = ((executionPriorityAttribute != null) ? executionPriorityAttribute.priority : 0);
				}
				return this._priorityCache;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600016A RID: 362
		public abstract int maxInConnections { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600016B RID: 363
		public abstract int maxOutConnections { get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600016C RID: 364
		public abstract Type outConnectionType { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600016D RID: 365
		public abstract bool allowAsPrime { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600016E RID: 366
		public abstract bool canSelfConnect { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600016F RID: 367
		public abstract Alignment2x2 commentsAlignment { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000170 RID: 368
		public abstract Alignment2x2 iconAlignment { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00005487 File Offset: 0x00003687
		// (set) Token: 0x06000172 RID: 370 RVA: 0x0000548F File Offset: 0x0000368F
		public Status status
		{
			get
			{
				return this._status;
			}
			protected set
			{
				if (this._status == Status.Resting && value == Status.Running)
				{
					this.timeStarted = this.graph.elapsedTime;
				}
				this._status = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000054B6 File Offset: 0x000036B6
		public Component graphAgent
		{
			get
			{
				if (!(this.graph != null))
				{
					return null;
				}
				return this.graph.agent;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000054D3 File Offset: 0x000036D3
		public IBlackboard graphBlackboard
		{
			get
			{
				if (!(this.graph != null))
				{
					return null;
				}
				return this.graph.blackboard;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000054F0 File Offset: 0x000036F0
		public float elapsedTime
		{
			get
			{
				if (this.status != Status.Running)
				{
					return 0f;
				}
				return this.graph.elapsedTime - this.timeStarted;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00005513 File Offset: 0x00003713
		// (set) Token: 0x06000177 RID: 375 RVA: 0x0000551B File Offset: 0x0000371B
		private float timeStarted { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00005524 File Offset: 0x00003724
		// (set) Token: 0x06000179 RID: 377 RVA: 0x0000552C File Offset: 0x0000372C
		private bool isChecked { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00005535 File Offset: 0x00003735
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000553D File Offset: 0x0000373D
		private bool breakPointReached { get; set; }

		// Token: 0x0600017C RID: 380 RVA: 0x00005546 File Offset: 0x00003746
		public Node()
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005578 File Offset: 0x00003778
		public static Node Create(Graph targetGraph, Type nodeType, Vector2 pos)
		{
			if (targetGraph == null)
			{
				return null;
			}
			if (nodeType.IsGenericTypeDefinition)
			{
				nodeType = nodeType.RTMakeGenericType(new Type[]
				{
					nodeType.GetFirstGenericParameterConstraintType()
				});
			}
			Node node = (Node)Activator.CreateInstance(nodeType);
			node.graph = targetGraph;
			node.position = pos;
			BBParameter.SetBBFields(node, targetGraph.blackboard);
			node.Validate(targetGraph);
			node.OnCreate(targetGraph);
			return node;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000055E4 File Offset: 0x000037E4
		public Node Duplicate(Graph targetGraph)
		{
			if (targetGraph == null)
			{
				return null;
			}
			Node node = JSONSerializer.Clone<Node>(this);
			targetGraph.allNodes.Add(node);
			node.inConnections.Clear();
			node.outConnections.Clear();
			if (targetGraph == this.graph)
			{
				node.position += new Vector2(50f, 50f);
			}
			node._UID = null;
			node.graph = targetGraph;
			BBParameter.SetBBFields(node, targetGraph.blackboard);
			foreach (Task task in Graph.GetTasksInElement(node))
			{
				task.Validate(targetGraph);
			}
			node.Validate(targetGraph);
			return node;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000056B4 File Offset: 0x000038B4
		public void Validate(Graph assignedGraph)
		{
			this.OnValidate(assignedGraph);
			this.GetHardError();
			if (this is IGraphAssignable)
			{
				(this as IGraphAssignable).ValidateSubGraphAndParameters();
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000056D8 File Offset: 0x000038D8
		public Status Execute(Component agent, IBlackboard blackboard)
		{
			if (this.graph == null)
			{
				return Status.Failure;
			}
			if (!this.graph.isRunning)
			{
				return this.status;
			}
			return this.status = this.OnExecute(agent, blackboard);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000571C File Offset: 0x0000391C
		public void Reset(bool recursively = true)
		{
			if (this.status == Status.Resting || this.isChecked)
			{
				return;
			}
			this.OnReset();
			this.status = Status.Resting;
			this.isChecked = true;
			for (int i = 0; i < this.outConnections.Count; i++)
			{
				this.outConnections[i].Reset(recursively);
			}
			this.isChecked = false;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000577E File Offset: 0x0000397E
		private IEnumerator YieldBreak(Action resume)
		{
			Debug.Break();
			yield return null;
			resume.Invoke();
			yield break;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000578D File Offset: 0x0000398D
		public Status Error(object msg)
		{
			if (msg is Exception)
			{
				ParadoxNotion.Services.Logger.LogException((Exception)msg, "Execution", this);
			}
			this.status = Status.Error;
			return Status.Error;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000057B0 File Offset: 0x000039B0
		public Status Fail(string msg)
		{
			this.status = Status.Failure;
			return Status.Failure;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000057BA File Offset: 0x000039BA
		public void Warn(string msg)
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000057BC File Offset: 0x000039BC
		public void SetStatus(Status status)
		{
			this.status = status;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000057C5 File Offset: 0x000039C5
		protected void SendEvent(string eventName)
		{
			this.graph.SendEvent(eventName, null, this);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000057D5 File Offset: 0x000039D5
		protected void SendEvent<T>(string eventName, T value)
		{
			this.graph.SendEvent<T>(eventName, value, this);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000057E8 File Offset: 0x000039E8
		public static bool IsNewConnectionAllowed(Node sourceNode, Node targetNode, Connection refConnection = null)
		{
			return sourceNode != null && targetNode != null && (sourceNode != targetNode || sourceNode.canSelfConnect) && ((refConnection != null && refConnection.sourceNode == sourceNode) || sourceNode.outConnections.Count < sourceNode.maxOutConnections || sourceNode.maxOutConnections == -1) && ((refConnection != null && refConnection.targetNode == targetNode) || targetNode.maxInConnections > targetNode.inConnections.Count || targetNode.maxInConnections == -1) && (true & sourceNode.CanConnectToTarget(targetNode) & targetNode.CanConnectFromSource(sourceNode));
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00005870 File Offset: 0x00003A70
		protected virtual bool CanConnectToTarget(Node targetNode)
		{
			return true;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00005873 File Offset: 0x00003A73
		protected virtual bool CanConnectFromSource(Node sourceNode)
		{
			return true;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00005878 File Offset: 0x00003A78
		public static bool AreNodesConnected(Node a, Node b)
		{
			bool flag = a.outConnections.FirstOrDefault((Connection c) => c.targetNode == b) != null;
			bool flag2 = b.outConnections.FirstOrDefault((Connection c) => c.targetNode == a) != null;
			return flag || flag2;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000058DA File Offset: 0x00003ADA
		public Coroutine StartCoroutine(IEnumerator routine)
		{
			if (!(MonoManager.current != null))
			{
				return null;
			}
			return MonoManager.current.StartCoroutine(routine);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000058F6 File Offset: 0x00003AF6
		public void StopCoroutine(Coroutine routine)
		{
			if (MonoManager.current != null)
			{
				MonoManager.current.StopCoroutine(routine);
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00005910 File Offset: 0x00003B10
		public IEnumerable<Node> GetParentNodes()
		{
			if (this.inConnections.Count != 0)
			{
				return from c in this.inConnections
				select c.sourceNode;
			}
			return new Node[0];
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00005950 File Offset: 0x00003B50
		public IEnumerable<Node> GetChildNodes()
		{
			if (this.outConnections.Count != 0)
			{
				return from c in this.outConnections
				select c.targetNode;
			}
			return new Node[0];
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00005990 File Offset: 0x00003B90
		public bool IsChildOf(Node parentNode)
		{
			return this.inConnections.Any((Connection c) => c.sourceNode == parentNode);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000059C4 File Offset: 0x00003BC4
		public bool IsParentOf(Node childNode)
		{
			return this.outConnections.Any((Connection c) => c.targetNode == childNode);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000059F8 File Offset: 0x00003BF8
		protected virtual string GetWarningOrError()
		{
			string hardError = this.GetHardError();
			if (hardError != null)
			{
				return "* " + hardError;
			}
			string result = null;
			ITaskAssignable taskAssignable = this as ITaskAssignable;
			if (taskAssignable != null && taskAssignable.task != null)
			{
				result = taskAssignable.task.GetWarningOrError();
			}
			return result;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00005A3C File Offset: 0x00003C3C
		private string GetHardError()
		{
			if (this is IMissingRecoverable)
			{
				return string.Format("Missing Node '{0}'", (this as IMissingRecoverable).missingType);
			}
			if (this is IReflectedWrapper)
			{
				ISerializedReflectedInfo serializedInfo = (this as IReflectedWrapper).GetSerializedInfo();
				if (serializedInfo != null && serializedInfo.AsMemberInfo() == null)
				{
					return string.Format("Missing Reflected Info '{0}'", serializedInfo.AsString());
				}
			}
			return null;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00005A9E File Offset: 0x00003C9E
		protected virtual Status OnExecute(Component agent, IBlackboard blackboard)
		{
			return this.status;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00005AA6 File Offset: 0x00003CA6
		protected virtual void OnReset()
		{
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00005AA8 File Offset: 0x00003CA8
		public virtual void OnCreate(Graph assignedGraph)
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00005AAA File Offset: 0x00003CAA
		public virtual void OnValidate(Graph assignedGraph)
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00005AAC File Offset: 0x00003CAC
		public virtual void OnDestroy()
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00005AAE File Offset: 0x00003CAE
		public virtual void OnParentConnected(int connectionIndex)
		{
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00005AB0 File Offset: 0x00003CB0
		public virtual void OnParentDisconnected(int connectionIndex)
		{
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00005AB2 File Offset: 0x00003CB2
		public virtual void OnChildConnected(int connectionIndex)
		{
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00005AB4 File Offset: 0x00003CB4
		public virtual void OnChildDisconnected(int connectionIndex)
		{
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00005AB6 File Offset: 0x00003CB6
		public virtual void OnChildrenConnectionsSorted(int[] oldIndeces)
		{
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00005AB8 File Offset: 0x00003CB8
		public virtual void OnGraphStarted()
		{
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00005ABA File Offset: 0x00003CBA
		public virtual void OnPostGraphStarted()
		{
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00005ABC File Offset: 0x00003CBC
		public virtual void OnGraphStoped()
		{
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00005ABE File Offset: 0x00003CBE
		public virtual void OnPostGraphStoped()
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public virtual void OnGraphPaused()
		{
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00005AC2 File Offset: 0x00003CC2
		public virtual void OnGraphUnpaused()
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00005AC4 File Offset: 0x00003CC4
		public override string ToString()
		{
			string text = this.name;
			if (this is IReflectedWrapper)
			{
				ISerializedReflectedInfo serializedInfo = (this as IReflectedWrapper).GetSerializedInfo();
				MemberInfo memberInfo = (serializedInfo != null) ? serializedInfo.AsMemberInfo() : null;
				if (memberInfo != null)
				{
					text = memberInfo.FriendlyName();
				}
			}
			if (this is IGraphAssignable)
			{
				Graph subGraph = (this as IGraphAssignable).subGraph;
				if (subGraph != null)
				{
					text = subGraph.name;
				}
			}
			return string.Format("{0}{1}", text, (!string.IsNullOrEmpty(this.tag)) ? (" (" + this.tag + ")") : "");
		}

		// Token: 0x04000049 RID: 73
		[SerializeField]
		private string _UID;

		// Token: 0x0400004A RID: 74
		[SerializeField]
		private string _name;

		// Token: 0x0400004B RID: 75
		[SerializeField]
		private string _tag;

		// Token: 0x0400004C RID: 76
		[SerializeField]
		[fsIgnoreInBuild]
		private Vector2 _position;

		// Token: 0x0400004D RID: 77
		[SerializeField]
		[fsIgnoreInBuild]
		private string _comment;

		// Token: 0x0400004E RID: 78
		[SerializeField]
		[fsIgnoreInBuild]
		private bool _isBreakpoint;

		// Token: 0x0400004F RID: 79
		private Graph _graph;

		// Token: 0x04000050 RID: 80
		private int _ID;

		// Token: 0x04000051 RID: 81
		private List<Connection> _inConnections = new List<Connection>();

		// Token: 0x04000052 RID: 82
		private List<Connection> _outConnections = new List<Connection>();

		// Token: 0x04000053 RID: 83
		[NonSerialized]
		private Status _status = Status.Resting;

		// Token: 0x04000054 RID: 84
		[NonSerialized]
		private string _nameCache;

		// Token: 0x04000055 RID: 85
		[NonSerialized]
		private string _descriptionCache;

		// Token: 0x04000056 RID: 86
		[NonSerialized]
		private int _priorityCache = int.MinValue;

		// Token: 0x020000FE RID: 254
		[AttributeUsage(256)]
		protected class AutoSortWithChildrenConnections : Attribute
		{
		}
	}
}
