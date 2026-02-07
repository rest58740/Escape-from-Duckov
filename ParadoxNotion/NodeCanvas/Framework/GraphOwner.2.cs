using System;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200000E RID: 14
	public abstract class GraphOwner<T> : GraphOwner where T : Graph
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00004E79 File Offset: 0x00003079
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00004E86 File Offset: 0x00003086
		public sealed override Graph graph
		{
			get
			{
				return this._graph;
			}
			set
			{
				this._graph = (T)((object)value);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004E94 File Offset: 0x00003094
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00004EA1 File Offset: 0x000030A1
		public T behaviour
		{
			get
			{
				return (T)((object)this.graph);
			}
			set
			{
				this.graph = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004EAF File Offset: 0x000030AF
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00004ECC File Offset: 0x000030CC
		public sealed override IBlackboard blackboard
		{
			get
			{
				if (!(this._blackboard != null))
				{
					return null;
				}
				return this._blackboard as IBlackboard;
			}
			set
			{
				if (this._blackboard != value)
				{
					this._blackboard = (Object)value;
					Graph graph = this.graph;
					if (graph == null)
					{
						return;
					}
					graph.UpdateReferences(this, value, false);
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00004EF6 File Offset: 0x000030F6
		public sealed override Type graphType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004F02 File Offset: 0x00003102
		public void StartBehaviour(T newGraph)
		{
			this.StartBehaviour(newGraph, base.updateMode, null);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004F12 File Offset: 0x00003112
		public void StartBehaviour(T newGraph, Action<bool> callback)
		{
			this.StartBehaviour(newGraph, base.updateMode, callback);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004F22 File Offset: 0x00003122
		public void StartBehaviour(T newGraph, Graph.UpdateMode updateMode, Action<bool> callback = null)
		{
			this.SwitchBehaviour(newGraph, updateMode, callback);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004F2D File Offset: 0x0000312D
		public void SwitchBehaviour(T newGraph)
		{
			this.SwitchBehaviour(newGraph, base.updateMode, null);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004F3D File Offset: 0x0000313D
		public void SwitchBehaviour(T newGraph, Action<bool> callback)
		{
			this.SwitchBehaviour(newGraph, base.updateMode, callback);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004F4D File Offset: 0x0000314D
		public void SwitchBehaviour(T newGraph, Graph.UpdateMode updateMode, Action<bool> callback = null)
		{
			base.StopBehaviour(true);
			this.graph = newGraph;
			base.StartBehaviour(updateMode, callback);
		}

		// Token: 0x04000047 RID: 71
		[SerializeField]
		private T _graph;

		// Token: 0x04000048 RID: 72
		[SerializeField]
		private Object _blackboard;
	}
}
