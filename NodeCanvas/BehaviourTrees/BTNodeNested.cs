using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200010D RID: 269
	[Category("SubGraphs")]
	[Color("ffe4e1")]
	public abstract class BTNodeNested<T> : BTNode, IGraphAssignable<T>, IGraphAssignable, IGraphElement where T : Graph
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060005B1 RID: 1457
		// (set) Token: 0x060005B2 RID: 1458
		public abstract T subGraph { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060005B3 RID: 1459
		public abstract BBParameter subGraphParameter { get; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00012515 File Offset: 0x00010715
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0001251D File Offset: 0x0001071D
		public T currentInstance { get; set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00012526 File Offset: 0x00010726
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0001252E File Offset: 0x0001072E
		public Dictionary<Graph, Graph> instances { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00012537 File Offset: 0x00010737
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x0001253F File Offset: 0x0001073F
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

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00012548 File Offset: 0x00010748
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x00012555 File Offset: 0x00010755
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

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00012563 File Offset: 0x00010763
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x00012570 File Offset: 0x00010770
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

		// Token: 0x04000306 RID: 774
		[SerializeField]
		private List<BBMappingParameter> _variablesMap;
	}
}
