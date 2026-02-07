using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000010 RID: 16
	public interface IGraphElement
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000138 RID: 312
		string name { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000139 RID: 313
		string UID { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600013A RID: 314
		Graph graph { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600013B RID: 315
		Status status { get; }
	}
}
