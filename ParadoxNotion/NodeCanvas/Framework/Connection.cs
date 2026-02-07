using System;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000009 RID: 9
	[SpoofAOT]
	[fsDeserializeOverwrite]
	[Serializable]
	public abstract class Connection : IGraphElement, ISerializationCollectable
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002214 File Offset: 0x00000414
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

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002251 File Offset: 0x00000451
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002259 File Offset: 0x00000459
		public Node sourceNode
		{
			get
			{
				return this._sourceNode;
			}
			protected set
			{
				this._sourceNode = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002262 File Offset: 0x00000462
		// (set) Token: 0x06000014 RID: 20 RVA: 0x0000226A File Offset: 0x0000046A
		public Node targetNode
		{
			get
			{
				return this._targetNode;
			}
			protected set
			{
				this._targetNode = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002273 File Offset: 0x00000473
		string IGraphElement.name
		{
			get
			{
				return "Connection";
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000227A File Offset: 0x0000047A
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002285 File Offset: 0x00000485
		public bool isActive
		{
			get
			{
				return !this._isDisabled;
			}
			set
			{
				if (!this._isDisabled && !value)
				{
					this.Reset(true);
				}
				this._isDisabled = !value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000022A3 File Offset: 0x000004A3
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000022AB File Offset: 0x000004AB
		public Status status
		{
			get
			{
				return this._status;
			}
			set
			{
				this._status = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000022B4 File Offset: 0x000004B4
		public Graph graph
		{
			get
			{
				if (this.sourceNode == null)
				{
					return null;
				}
				return this.sourceNode.graph;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022CB File Offset: 0x000004CB
		public Connection()
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000022DC File Offset: 0x000004DC
		public static Connection Create(Node source, Node target, int sourceIndex = -1, int targetIndex = -1)
		{
			if (source == null || target == null)
			{
				return null;
			}
			if (source is MissingNode)
			{
				return null;
			}
			Connection connection = (Connection)Activator.CreateInstance(source.outConnectionType);
			int sourceIndex2 = connection.SetSourceNode(source, sourceIndex);
			int targetIndex2 = connection.SetTargetNode(target, targetIndex);
			connection.OnValidate(sourceIndex2, targetIndex2);
			connection.OnCreate(sourceIndex2, targetIndex2);
			return connection;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002330 File Offset: 0x00000530
		public Connection Duplicate(Node newSource, Node newTarget)
		{
			if (newSource == null || newTarget == null)
			{
				return null;
			}
			Connection connection = JSONSerializer.Clone<Connection>(this);
			connection._UID = null;
			connection.sourceNode = newSource;
			connection.targetNode = newTarget;
			newSource.outConnections.Add(connection);
			newTarget.inConnections.Add(connection);
			if (newSource.graph != null)
			{
				foreach (Task task in Graph.GetTasksInElement(connection))
				{
					task.Validate(newSource.graph);
				}
			}
			connection.OnValidate(newSource.outConnections.Count - 1, newTarget.inConnections.Count - 1);
			return connection;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023EC File Offset: 0x000005EC
		public int SetSourceNode(Node newSource, int index = -1)
		{
			if (this.sourceNode == newSource)
			{
				return -1;
			}
			if (this.sourceNode != null && this.sourceNode.outConnections.Contains(this))
			{
				int connectionIndex = this.sourceNode.outConnections.IndexOf(this);
				this.sourceNode.OnChildDisconnected(connectionIndex);
				this.sourceNode.outConnections.Remove(this);
			}
			index = ((index == -1) ? newSource.outConnections.Count : index);
			newSource.outConnections.Insert(index, this);
			newSource.OnChildConnected(index);
			this.sourceNode = newSource;
			this.OnValidate(index, (this.targetNode != null) ? this.targetNode.inConnections.IndexOf(this) : -1);
			return index;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024A4 File Offset: 0x000006A4
		public int SetTargetNode(Node newTarget, int index = -1)
		{
			if (this.targetNode == newTarget)
			{
				return -1;
			}
			if (this.targetNode != null && this.targetNode.inConnections.Contains(this))
			{
				int connectionIndex = this.targetNode.inConnections.IndexOf(this);
				this.targetNode.OnParentDisconnected(connectionIndex);
				this.targetNode.inConnections.Remove(this);
			}
			index = ((index == -1) ? newTarget.inConnections.Count : index);
			newTarget.inConnections.Insert(index, this);
			newTarget.OnParentConnected(index);
			this.targetNode = newTarget;
			this.OnValidate((this.sourceNode != null) ? this.sourceNode.outConnections.IndexOf(this) : -1, index);
			return index;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000255A File Offset: 0x0000075A
		public Status Execute(Component agent, IBlackboard blackboard)
		{
			if (!this.isActive)
			{
				return Status.Optional;
			}
			this.status = this.targetNode.Execute(agent, blackboard);
			return this.status;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000257F File Offset: 0x0000077F
		public void Reset(bool recursively = true)
		{
			if (this.status == Status.Resting)
			{
				return;
			}
			this.status = Status.Resting;
			if (recursively)
			{
				this.targetNode.Reset(recursively);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000025A1 File Offset: 0x000007A1
		public virtual void OnCreate(int sourceIndex, int targetIndex)
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000025A3 File Offset: 0x000007A3
		public virtual void OnValidate(int sourceIndex, int targetIndex)
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000025A5 File Offset: 0x000007A5
		public virtual void OnDestroy()
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025A7 File Offset: 0x000007A7
		public override string ToString()
		{
			return base.GetType().FriendlyName(false);
		}

		// Token: 0x04000016 RID: 22
		[SerializeField]
		[fsSerializeAsReference]
		private Node _sourceNode;

		// Token: 0x04000017 RID: 23
		[SerializeField]
		[fsSerializeAsReference]
		private Node _targetNode;

		// Token: 0x04000018 RID: 24
		[SerializeField]
		private string _UID;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		private bool _isDisabled;

		// Token: 0x0400001A RID: 26
		[NonSerialized]
		private Status _status = Status.Resting;
	}
}
