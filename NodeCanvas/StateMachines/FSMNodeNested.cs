using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000DD RID: 221
	[Category("SubGraphs")]
	[Color("ffe4e1")]
	public abstract class FSMNodeNested<T> : FSMNode, IGraphAssignable<T>, IGraphAssignable, IGraphElement where T : Graph
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003EB RID: 1003
		// (set) Token: 0x060003EC RID: 1004
		public abstract T subGraph { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003ED RID: 1005
		public abstract BBParameter subGraphParameter { get; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000F2B6 File Offset: 0x0000D4B6
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x0000F2BE File Offset: 0x0000D4BE
		public T currentInstance { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000F2C7 File Offset: 0x0000D4C7
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0000F2CF File Offset: 0x0000D4CF
		public Dictionary<Graph, Graph> instances { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000F2D8 File Offset: 0x0000D4D8
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0000F2E0 File Offset: 0x0000D4E0
		public List<BBMappingParameter> variablesMap
		{
			get
			{
				return this._variablesMap;
			}
			set
			{
				this._variablesMap = value;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000F2E9 File Offset: 0x0000D4E9
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000F2F6 File Offset: 0x0000D4F6
		Graph IGraphAssignable.subGraph
		{
			get
			{
				return this.subGraph;
			}
			set
			{
				this.subGraph = (T)((object)value);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000F304 File Offset: 0x0000D504
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0000F311 File Offset: 0x0000D511
		Graph IGraphAssignable.currentInstance
		{
			get
			{
				return this.currentInstance;
			}
			set
			{
				this.currentInstance = (T)((object)value);
			}
		}

		// Token: 0x04000294 RID: 660
		[SerializeField]
		private List<BBMappingParameter> _variablesMap;
	}
}
