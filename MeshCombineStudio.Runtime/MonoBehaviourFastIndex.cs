using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000029 RID: 41
	public class MonoBehaviourFastIndex : MonoBehaviour, IFastIndex
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000081D7 File Offset: 0x000063D7
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000081DF File Offset: 0x000063DF
		public IFastIndexList List { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000081E8 File Offset: 0x000063E8
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000081F0 File Offset: 0x000063F0
		public int ListIndex { get; set; }

		// Token: 0x060000C5 RID: 197 RVA: 0x000081F9 File Offset: 0x000063F9
		public MonoBehaviourFastIndex()
		{
			this.ListIndex = -1;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00008208 File Offset: 0x00006408
		public void RemoveFromList()
		{
			if (this.List != null)
			{
				this.List.Remove(this);
			}
		}
	}
}
