using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000CD RID: 205
	[Name("Control Graph Owner", 0)]
	[Category("✫ Utility")]
	[Description("Start, Resume, Pause, Stop a GraphOwner's behaviour")]
	public class GraphOwnerControl : ActionTask<GraphOwner>
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000DE07 File Offset: 0x0000C007
		protected override string info
		{
			get
			{
				return base.agentInfo + "." + this.control.ToString();
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000DE2C File Offset: 0x0000C02C
		protected override void OnExecute()
		{
			if (this.control == GraphOwnerControl.Control.StartBehaviour)
			{
				if (this.waitActionFinish)
				{
					base.agent.StartBehaviour(delegate(bool s)
					{
						base.EndAction(s);
					});
					return;
				}
				base.agent.StartBehaviour();
				base.EndAction();
				return;
			}
			else
			{
				if (base.agent == base.ownerSystemAgent)
				{
					base.StartCoroutine(this.YieldDo());
					return;
				}
				this.Do();
				return;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000DE9A File Offset: 0x0000C09A
		private IEnumerator YieldDo()
		{
			yield return null;
			this.Do();
			yield break;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		private void Do()
		{
			if (this.control == GraphOwnerControl.Control.StopBehaviour)
			{
				base.EndAction(default(bool?));
				base.agent.StopBehaviour(true);
			}
			if (this.control == GraphOwnerControl.Control.PauseBehaviour)
			{
				base.EndAction(default(bool?));
				base.agent.PauseBehaviour();
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000DF00 File Offset: 0x0000C100
		protected override void OnStop()
		{
			if (this.waitActionFinish && this.control == GraphOwnerControl.Control.StartBehaviour)
			{
				base.agent.StopBehaviour(true);
			}
		}

		// Token: 0x04000266 RID: 614
		public GraphOwnerControl.Control control;

		// Token: 0x04000267 RID: 615
		public bool waitActionFinish = true;

		// Token: 0x02000147 RID: 327
		public enum Control
		{
			// Token: 0x040003B2 RID: 946
			StartBehaviour,
			// Token: 0x040003B3 RID: 947
			StopBehaviour,
			// Token: 0x040003B4 RID: 948
			PauseBehaviour
		}
	}
}
