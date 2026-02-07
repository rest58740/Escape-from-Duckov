using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000F6 RID: 246
	[AddComponentMenu("NodeCanvas/Dialogue Tree Controller")]
	public class DialogueTreeController : GraphOwner<DialogueTree>, IDialogueActor
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00010EB5 File Offset: 0x0000F0B5
		string IDialogueActor.name
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00010EBD File Offset: 0x0000F0BD
		Texture2D IDialogueActor.portrait
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00010EC0 File Offset: 0x0000F0C0
		Sprite IDialogueActor.portraitSprite
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00010EC3 File Offset: 0x0000F0C3
		Color IDialogueActor.dialogueColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00010ECA File Offset: 0x0000F0CA
		Vector3 IDialogueActor.dialoguePosition
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00010ED1 File Offset: 0x0000F0D1
		Transform IDialogueActor.transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00010ED9 File Offset: 0x0000F0D9
		public void StartDialogue()
		{
			this.StartDialogue(this, null);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00010EE3 File Offset: 0x0000F0E3
		public void StartDialogue(Action<bool> callback)
		{
			this.StartDialogue(this, callback);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00010EED File Offset: 0x0000F0ED
		public void StartDialogue(IDialogueActor instigator)
		{
			this.StartDialogue(instigator, null);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00010EF7 File Offset: 0x0000F0F7
		public void StartDialogue(DialogueTree newTree, IDialogueActor instigator, Action<bool> callback)
		{
			this.graph = newTree;
			this.StartDialogue(instigator, callback);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00010F08 File Offset: 0x0000F108
		public void StartDialogue(IDialogueActor instigator, Action<bool> callback)
		{
			this.graph = base.GetInstance(this.graph);
			this.graph.StartGraph((instigator is Component) ? ((Component)instigator) : instigator.transform, this.blackboard, base.updateMode, callback);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00010F55 File Offset: 0x0000F155
		public void PauseDialogue()
		{
			this.graph.Pause();
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00010F62 File Offset: 0x0000F162
		public void StopDialogue()
		{
			this.graph.Stop(true);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00010F70 File Offset: 0x0000F170
		public void SetActorReference(string paramName, IDialogueActor actor)
		{
			if (base.behaviour != null)
			{
				base.behaviour.SetActorReference(paramName, actor);
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00010F8D File Offset: 0x0000F18D
		public void SetActorReferences(Dictionary<string, IDialogueActor> actors)
		{
			if (base.behaviour != null)
			{
				base.behaviour.SetActorReferences(actors);
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00010FA9 File Offset: 0x0000F1A9
		public IDialogueActor GetActorReferenceByName(string paramName)
		{
			if (!(base.behaviour != null))
			{
				return null;
			}
			return base.behaviour.GetActorReferenceByName(paramName);
		}
	}
}
