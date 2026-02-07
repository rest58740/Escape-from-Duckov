using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000431 RID: 1073
	public class SetAim : ActionTask<AICharacterController>
	{
		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x0008643C File Offset: 0x0008463C
		protected override string info
		{
			get
			{
				if (this.useTransfom && string.IsNullOrEmpty(this.aimTarget.name))
				{
					return "Set aim to null";
				}
				if (!this.useTransfom)
				{
					return "Set aim to " + this.aimPos.name;
				}
				return "Set aim to " + this.aimTarget.name;
			}
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0008649C File Offset: 0x0008469C
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000864A0 File Offset: 0x000846A0
		protected override void OnExecute()
		{
			base.agent.SetTarget(this.aimTarget.value);
			if (!this.useTransfom || !(this.aimTarget.value != null))
			{
				if (!this.useTransfom)
				{
					base.agent.SetAimInput((this.aimPos.value - base.agent.transform.position).normalized, AimTypes.normalAim);
				}
				else
				{
					base.agent.SetAimInput(Vector3.zero, AimTypes.normalAim);
				}
			}
			base.EndAction(true);
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x00086534 File Offset: 0x00084734
		protected override void OnUpdate()
		{
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x00086536 File Offset: 0x00084736
		protected override void OnStop()
		{
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x00086538 File Offset: 0x00084738
		protected override void OnPause()
		{
		}

		// Token: 0x04001A6E RID: 6766
		public bool useTransfom = true;

		// Token: 0x04001A6F RID: 6767
		[ShowIf("useTransfom", 1)]
		public BBParameter<Transform> aimTarget;

		// Token: 0x04001A70 RID: 6768
		[ShowIf("useTransfom", 0)]
		public BBParameter<Vector3> aimPos;

		// Token: 0x04001A71 RID: 6769
		private bool waitingSearchResult;
	}
}
