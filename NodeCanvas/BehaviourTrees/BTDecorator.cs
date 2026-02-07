using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200010F RID: 271
	public abstract class BTDecorator : BTNode
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x000125A1 File Offset: 0x000107A1
		public sealed override int maxOutConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x000125A4 File Offset: 0x000107A4
		public sealed override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Right;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000125A7 File Offset: 0x000107A7
		protected Connection decoratedConnection
		{
			get
			{
				if (base.outConnections.Count <= 0)
				{
					return null;
				}
				return base.outConnections[0];
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x000125C8 File Offset: 0x000107C8
		protected Node decoratedNode
		{
			get
			{
				Connection decoratedConnection = this.decoratedConnection;
				if (decoratedConnection == null)
				{
					return null;
				}
				return decoratedConnection.targetNode;
			}
		}
	}
}
