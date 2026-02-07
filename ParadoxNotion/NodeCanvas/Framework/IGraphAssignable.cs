using System;
using System.Collections.Generic;
using NodeCanvas.Framework.Internal;

namespace NodeCanvas.Framework
{
	// Token: 0x02000013 RID: 19
	public interface IGraphAssignable : IGraphElement
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000140 RID: 320
		// (set) Token: 0x06000141 RID: 321
		Graph subGraph { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000142 RID: 322
		// (set) Token: 0x06000143 RID: 323
		Graph currentInstance { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000144 RID: 324
		// (set) Token: 0x06000145 RID: 325
		Dictionary<Graph, Graph> instances { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000146 RID: 326
		BBParameter subGraphParameter { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000147 RID: 327
		// (set) Token: 0x06000148 RID: 328
		List<BBMappingParameter> variablesMap { get; set; }
	}
}
