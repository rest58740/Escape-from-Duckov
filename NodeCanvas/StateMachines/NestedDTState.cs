using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E9 RID: 233
	[Name("Sub Dialogue", 0)]
	[Description("Execute the assigned Dialogue Tree OnEnter and stop it OnExit. Optionaly an event can be sent for whether the dialogue ended in Success or Failure. This can be controled by using the 'Finish' Dialogue Node inside the Dialogue Tree. Use a 'CheckEvent' condition to make use of those events. The 'Instigator' Actor of the Dialogue Tree will be set to this graph agent.")]
	[DropReferenceType(typeof(DialogueTree))]
	[ParadoxNotion.Design.Icon("Dialogue", false, "")]
	public class NestedDTState : FSMStateNested<DialogueTree>
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000FC97 File Offset: 0x0000DE97
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0000FCA4 File Offset: 0x0000DEA4
		public override DialogueTree subGraph
		{
			get
			{
				return this._nestedDLG.value;
			}
			set
			{
				this._nestedDLG.value = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0000FCB2 File Offset: 0x0000DEB2
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._nestedDLG;
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000FCBA File Offset: 0x0000DEBA
		protected override void OnEnter()
		{
			if (this.subGraph == null)
			{
				base.Finish(false);
				return;
			}
			this.TryStartSubGraph(base.graphAgent, new Action<bool>(this.OnDialogueFinished));
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000FCEB File Offset: 0x0000DEEB
		protected override void OnUpdate()
		{
			base.currentInstance.UpdateGraph(base.graph.deltaTime);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000FD03 File Offset: 0x0000DF03
		protected override void OnExit()
		{
			if (base.currentInstance != null)
			{
				base.currentInstance.Stop(true);
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000FD20 File Offset: 0x0000DF20
		private void OnDialogueFinished(bool success)
		{
			if (base.status == Status.Running)
			{
				if (!string.IsNullOrEmpty(this.successEvent) && success)
				{
					base.SendEvent(this.successEvent);
				}
				if (!string.IsNullOrEmpty(this.failureEvent) && !success)
				{
					base.SendEvent(this.failureEvent);
				}
				base.Finish(success);
			}
		}

		// Token: 0x040002A6 RID: 678
		[SerializeField]
		[ExposeField]
		[Name("Sub Tree", 0)]
		private BBParameter<DialogueTree> _nestedDLG;

		// Token: 0x040002A7 RID: 679
		[DimIfDefault]
		[Tooltip("The event to send when the Dialogue Tree finished in Success.")]
		public string successEvent;

		// Token: 0x040002A8 RID: 680
		[DimIfDefault]
		[Tooltip("The event to send when the Dialogue Tree finish in Failure.")]
		public string failureEvent;
	}
}
