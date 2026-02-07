using System;
using System.Linq;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200001D RID: 29
	[fsForward("_targetNodeUID")]
	[fsAutoInstance(true)]
	[Serializable]
	public class NodeReference<T> : INodeReference where T : Node
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00005B60 File Offset: 0x00003D60
		Type INodeReference.type
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005B6C File Offset: 0x00003D6C
		Node INodeReference.Get(Graph graph)
		{
			return this.Get(graph);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005B7A File Offset: 0x00003D7A
		void INodeReference.Set(Node target)
		{
			this.Set(target as T);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00005B8D File Offset: 0x00003D8D
		public NodeReference()
		{
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00005B95 File Offset: 0x00003D95
		public NodeReference(T target)
		{
			this.Set(target);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00005BA4 File Offset: 0x00003DA4
		public T Get(Graph graph)
		{
			T t;
			if (this._targetNodeRef == null)
			{
				t = graph.GetAllNodesOfType<T>().FirstOrDefault((T x) => x.UID == this._targetNodeUID);
				this._targetNodeRef = new WeakReference<T>(t);
			}
			this._targetNodeRef.TryGetTarget(ref t);
			return t;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00005BF9 File Offset: 0x00003DF9
		public void Set(T target)
		{
			if (this._targetNodeRef == null)
			{
				this._targetNodeRef = new WeakReference<T>(target);
			}
			this._targetNodeRef.SetTarget(target);
			this._targetNodeUID = ((target != null) ? target.UID : null);
		}

		// Token: 0x0400005A RID: 90
		[SerializeField]
		private string _targetNodeUID;

		// Token: 0x0400005B RID: 91
		[NonSerialized]
		private WeakReference<T> _targetNodeRef;
	}
}
