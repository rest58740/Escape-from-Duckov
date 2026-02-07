using System;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000FC RID: 252
	public interface IStatement
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600052A RID: 1322
		string text { get; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600052B RID: 1323
		AudioClip audio { get; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600052C RID: 1324
		string meta { get; }
	}
}
