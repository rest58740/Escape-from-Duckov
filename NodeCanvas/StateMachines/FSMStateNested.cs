using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E0 RID: 224
	[Category("SubGraphs")]
	[Color("ffe4e1")]
	public abstract class FSMStateNested<T> : FSMState, IGraphAssignable<T>, IGraphAssignable, IGraphElement where T : Graph
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600041B RID: 1051
		// (set) Token: 0x0600041C RID: 1052
		public abstract T subGraph { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600041D RID: 1053
		public abstract BBParameter subGraphParameter { get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000F6C5 File Offset: 0x0000D8C5
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x0000F6CD File Offset: 0x0000D8CD
		public T currentInstance { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000F6D6 File Offset: 0x0000D8D6
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x0000F6DE File Offset: 0x0000D8DE
		public Dictionary<Graph, Graph> instances { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000F6E7 File Offset: 0x0000D8E7
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x0000F6EF File Offset: 0x0000D8EF
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

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0000F6F8 File Offset: 0x0000D8F8
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x0000F705 File Offset: 0x0000D905
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000F713 File Offset: 0x0000D913
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x0000F720 File Offset: 0x0000D920
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

		// Token: 0x04000299 RID: 665
		[SerializeField]
		private List<BBMappingParameter> _variablesMap;
	}
}
