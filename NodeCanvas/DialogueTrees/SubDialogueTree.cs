using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000106 RID: 262
	[Name("Sub Dialogue Tree", 0)]
	[Description("Execute a Sub Dialogue Tree. When that Dialogue Tree is finished, this node will continue either in Success or Failure if it has any connections. Useful for making reusable and self-contained Dialogue Trees.")]
	[DropReferenceType(typeof(DialogueTree))]
	[ParadoxNotion.Design.Icon("Dialogue", false, "")]
	public class SubDialogueTree : DTNodeNested<DialogueTree>, IUpdatable, IGraphElement
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00011961 File Offset: 0x0000FB61
		public override int maxOutConnections
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00011964 File Offset: 0x0000FB64
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x00011971 File Offset: 0x0000FB71
		public override DialogueTree subGraph
		{
			get
			{
				return this._subTree.value;
			}
			set
			{
				this._subTree.value = value;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001197F File Offset: 0x0000FB7F
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._subTree;
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00011988 File Offset: 0x0000FB88
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (this.subGraph == null)
			{
				return base.Error("No Sub Dialogue Tree assigned!");
			}
			base.currentInstance = (DialogueTree)this.CheckInstance();
			this.TryWriteAndBindMappedVariables();
			this.TryWriteMappedActorParameters();
			base.currentInstance.StartGraph((base.finalActor is Component) ? ((Component)base.finalActor) : base.finalActor.transform, bb.parent, Graph.UpdateMode.Manual, new Action<bool>(this.OnSubDialogueFinish));
			return Status.Running;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00011A10 File Offset: 0x0000FC10
		private void OnSubDialogueFinish(bool success)
		{
			this.TryReadAndUnbindMappedVariables();
			base.status = (success ? Status.Success : Status.Failure);
			base.DLGTree.Continue(success ? 0 : 1);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00011A37 File Offset: 0x0000FC37
		void IUpdatable.Update()
		{
			if (base.currentInstance != null && base.status == Status.Running)
			{
				base.currentInstance.UpdateGraph(base.graph.deltaTime);
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00011A68 File Offset: 0x0000FC68
		private void TryWriteMappedActorParameters()
		{
			if (this._actorParametersMap == null)
			{
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in this._actorParametersMap)
			{
				DialogueTree.ActorParameter parameterByID = base.currentInstance.GetParameterByID(keyValuePair.Key);
				DialogueTree.ActorParameter parameterByID2 = base.DLGTree.GetParameterByID(keyValuePair.Value);
				if (parameterByID != null && parameterByID2 != null)
				{
					base.currentInstance.SetActorReference(parameterByID.name, parameterByID2.actor);
				}
			}
		}

		// Token: 0x040002ED RID: 749
		[SerializeField]
		[ExposeField]
		private BBParameter<DialogueTree> _subTree;

		// Token: 0x040002EE RID: 750
		[fsSerializeAs("actorParametersMap")]
		private Dictionary<string, string> _actorParametersMap;
	}
}
