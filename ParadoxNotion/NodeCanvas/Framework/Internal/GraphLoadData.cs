using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000032 RID: 50
	public struct GraphLoadData
	{
		// Token: 0x040000A4 RID: 164
		public GraphSource source;

		// Token: 0x040000A5 RID: 165
		public string json;

		// Token: 0x040000A6 RID: 166
		public List<Object> references;

		// Token: 0x040000A7 RID: 167
		public Component agent;

		// Token: 0x040000A8 RID: 168
		public IBlackboard parentBlackboard;

		// Token: 0x040000A9 RID: 169
		public bool preInitializeSubGraphs;
	}
}
