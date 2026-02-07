using System;
using System.Collections.Generic;
using UnityEngine;

namespace DG.DemiLib
{
	// Token: 0x0200000B RID: 11
	public interface IEditorGUINode
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000016 RID: 22
		// (set) Token: 0x06000017 RID: 23
		string id { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000018 RID: 24
		// (set) Token: 0x06000019 RID: 25
		Vector2 guiPosition { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001A RID: 26
		List<string> connectedNodesIds { get; }
	}
}
