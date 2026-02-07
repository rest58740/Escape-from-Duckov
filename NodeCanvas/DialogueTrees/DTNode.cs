using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000F8 RID: 248
	public abstract class DTNode : Node
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00010FD8 File Offset: 0x0000F1D8
		public override string name
		{
			get
			{
				if (!this.requireActorSelection)
				{
					return base.name;
				}
				if (this.DLGTree.definedActorParameterNames.Contains(this.actorName))
				{
					return string.Format("{0}", this.actorName);
				}
				return string.Format("<color=#d63e3e>* {0} *</color>", this._actorName);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0001102D File Offset: 0x0000F22D
		public virtual bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00011030 File Offset: 0x0000F230
		public override int maxInConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00011033 File Offset: 0x0000F233
		public override int maxOutConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00011036 File Offset: 0x0000F236
		public sealed override Type outConnectionType
		{
			get
			{
				return typeof(DTConnection);
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00011042 File Offset: 0x0000F242
		public sealed override bool allowAsPrime
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00011045 File Offset: 0x0000F245
		public sealed override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00011048 File Offset: 0x0000F248
		public sealed override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Right;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001104B File Offset: 0x0000F24B
		public sealed override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Bottom;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0001104E File Offset: 0x0000F24E
		protected DialogueTree DLGTree
		{
			get
			{
				return (DialogueTree)base.graph;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001105C File Offset: 0x0000F25C
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0001108C File Offset: 0x0000F28C
		public string actorName
		{
			get
			{
				DialogueTree.ActorParameter parameterByID = this.DLGTree.GetParameterByID(this._actorParameterID);
				if (parameterByID == null)
				{
					return this._actorName;
				}
				return parameterByID.name;
			}
			private set
			{
				if (this._actorName != value && !string.IsNullOrEmpty(value))
				{
					this._actorName = value;
					DialogueTree.ActorParameter parameterByName = this.DLGTree.GetParameterByName(value);
					this._actorParameterID = ((parameterByName != null) ? parameterByName.ID : null);
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x000110D8 File Offset: 0x0000F2D8
		public IDialogueActor finalActor
		{
			get
			{
				IDialogueActor actorReferenceByID = this.DLGTree.GetActorReferenceByID(this._actorParameterID);
				if (actorReferenceByID == null)
				{
					return this.DLGTree.GetActorReferenceByName(this._actorName);
				}
				return actorReferenceByID;
			}
		}

		// Token: 0x040002D8 RID: 728
		[SerializeField]
		private string _actorName = "SELF";

		// Token: 0x040002D9 RID: 729
		[SerializeField]
		private string _actorParameterID;
	}
}
