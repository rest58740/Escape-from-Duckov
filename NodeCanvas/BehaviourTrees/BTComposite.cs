using System;
using ParadoxNotion;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200010E RID: 270
	public abstract class BTComposite : BTNode
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00012586 File Offset: 0x00010786
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00012593 File Offset: 0x00010793
		public sealed override int maxOutConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00012596 File Offset: 0x00010796
		public sealed override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Right;
			}
		}
	}
}
