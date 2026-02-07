using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200010C RID: 268
	public abstract class BTNode : Node
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00012440 File Offset: 0x00010640
		public sealed override Type outConnectionType
		{
			get
			{
				return typeof(BTConnection);
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001244C File Offset: 0x0001064C
		public sealed override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0001244F File Offset: 0x0001064F
		public sealed override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00012452 File Offset: 0x00010652
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Bottom;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00012455 File Offset: 0x00010655
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00012458 File Offset: 0x00010658
		public override int maxInConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001245B File Offset: 0x0001065B
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00012460 File Offset: 0x00010660
		public T AddChild<T>(int childIndex) where T : BTNode
		{
			if (base.outConnections.Count >= this.maxOutConnections && this.maxOutConnections != -1)
			{
				return default(T);
			}
			T t = base.graph.AddNode<T>();
			base.graph.ConnectNodes(this, t, childIndex, -1);
			return t;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000124B8 File Offset: 0x000106B8
		public T AddChild<T>() where T : BTNode
		{
			if (base.outConnections.Count >= this.maxOutConnections && this.maxOutConnections != -1)
			{
				return default(T);
			}
			T t = base.graph.AddNode<T>();
			base.graph.ConnectNodes(this, t, -1, -1);
			return t;
		}
	}
}
