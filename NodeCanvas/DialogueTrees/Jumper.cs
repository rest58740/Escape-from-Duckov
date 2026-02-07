using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000101 RID: 257
	[Name("JUMP", 0)]
	[Description("Select a target node to jump to.\nFor your convenience in identifying nodes in the dropdown, please give a Tag name to the nodes you want to use in this way.")]
	[Category("Control")]
	[ParadoxNotion.Design.Icon("Set", false, "")]
	[Color("6ebbff")]
	public class Jumper : DTNode, IHaveNodeReference, IGraphElement
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x000114C9 File Offset: 0x0000F6C9
		INodeReference IHaveNodeReference.targetReference
		{
			get
			{
				return this._targetNode;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x000114D1 File Offset: 0x0000F6D1
		private DTNode target
		{
			get
			{
				NodeReference<DTNode> targetNode = this._targetNode;
				if (targetNode == null)
				{
					return null;
				}
				return targetNode.Get(base.graph);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x000114EA File Offset: 0x0000F6EA
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x000114ED File Offset: 0x0000F6ED
		public override bool requireActorSelection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000114F0 File Offset: 0x0000F6F0
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (this.target == null)
			{
				return base.Error("Target Node of Jumper node is null");
			}
			base.DLGTree.EnterNode(this.target);
			return Status.Success;
		}

		// Token: 0x040002E5 RID: 741
		[fsSerializeAs("_sourceNodeUID")]
		public NodeReference<DTNode> _targetNode;
	}
}
