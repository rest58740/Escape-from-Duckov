using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000F9 RID: 249
	[Category("SubGraphs")]
	[Color("ffe4e1")]
	public abstract class DTNodeNested<T> : DTNode, IGraphAssignable<T>, IGraphAssignable, IGraphElement where T : Graph
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600050F RID: 1295
		// (set) Token: 0x06000510 RID: 1296
		public abstract T subGraph { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000511 RID: 1297
		public abstract BBParameter subGraphParameter { get; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00011120 File Offset: 0x0000F320
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x00011128 File Offset: 0x0000F328
		public T currentInstance { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x00011131 File Offset: 0x0000F331
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x00011139 File Offset: 0x0000F339
		public Dictionary<Graph, Graph> instances { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00011142 File Offset: 0x0000F342
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x0001114A File Offset: 0x0000F34A
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

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00011153 File Offset: 0x0000F353
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00011160 File Offset: 0x0000F360
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

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001116E File Offset: 0x0000F36E
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x0001117B File Offset: 0x0000F37B
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

		// Token: 0x040002DA RID: 730
		[SerializeField]
		private List<BBMappingParameter> _variablesMap;
	}
}
