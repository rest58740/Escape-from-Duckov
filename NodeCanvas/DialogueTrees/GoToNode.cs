using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000107 RID: 263
	[Obsolete("Use Jumpers instead")]
	public class GoToNode : DTNode
	{
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00011B08 File Offset: 0x0000FD08
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00011B0B File Offset: 0x0000FD0B
		public override bool requireActorSelection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00011B0E File Offset: 0x0000FD0E
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (this._targetNode == null)
			{
				return base.Error("Target node of GOTO node is null");
			}
			base.DLGTree.EnterNode(this._targetNode);
			return Status.Success;
		}

		// Token: 0x040002EF RID: 751
		[SerializeField]
		private DTNode _targetNode;
	}
}
