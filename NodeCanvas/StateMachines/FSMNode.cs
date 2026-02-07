using System;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000DC RID: 220
	public abstract class FSMNode : Node
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x0000F283 File Offset: 0x0000D483
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000F286 File Offset: 0x0000D486
		public override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000F289 File Offset: 0x0000D489
		public override int maxInConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0000F28C File Offset: 0x0000D48C
		public override int maxOutConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0000F28F File Offset: 0x0000D48F
		public sealed override Type outConnectionType
		{
			get
			{
				return typeof(FSMConnection);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000F29B File Offset: 0x0000D49B
		public sealed override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Bottom;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000F29E File Offset: 0x0000D49E
		public sealed override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Bottom;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000F2A1 File Offset: 0x0000D4A1
		public FSM FSM
		{
			get
			{
				return (FSM)base.graph;
			}
		}
	}
}
