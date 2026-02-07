using System;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000348 RID: 840
	public class Condition : MonoBehaviour
	{
		// Token: 0x06001CA5 RID: 7333 RVA: 0x0006872C File Offset: 0x0006692C
		public virtual bool Evaluate()
		{
			return false;
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x0006872F File Offset: 0x0006692F
		public virtual string DisplayText
		{
			get
			{
				return "";
			}
		}
	}
}
