using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000377 RID: 887
	public class QuestRelationNodeBase : Node
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x0006E9AA File Offset: 0x0006CBAA
		public override int maxInConnections
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x0006E9AE File Offset: 0x0006CBAE
		public override int maxOutConnections
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x0006E9B2 File Offset: 0x0006CBB2
		public override Type outConnectionType
		{
			get
			{
				return typeof(QuestRelationConnection);
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x0006E9BE File Offset: 0x0006CBBE
		public override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001F34 RID: 7988 RVA: 0x0006E9C1 File Offset: 0x0006CBC1
		public override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x0006E9C4 File Offset: 0x0006CBC4
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001F36 RID: 7990 RVA: 0x0006E9C7 File Offset: 0x0006CBC7
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return 0;
			}
		}
	}
}
