using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.PerkTrees
{
	// Token: 0x02000262 RID: 610
	public class PerkRelationNodeBase : Node
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x00049424 File Offset: 0x00047624
		public override int maxInConnections
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600134C RID: 4940 RVA: 0x00049428 File Offset: 0x00047628
		public override int maxOutConnections
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x0004942C File Offset: 0x0004762C
		public override Type outConnectionType
		{
			get
			{
				return typeof(PerkRelationConnection);
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x00049438 File Offset: 0x00047638
		public override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x0004943B File Offset: 0x0004763B
		public override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001350 RID: 4944 RVA: 0x0004943E File Offset: 0x0004763E
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x00049441 File Offset: 0x00047641
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return 0;
			}
		}
	}
}
