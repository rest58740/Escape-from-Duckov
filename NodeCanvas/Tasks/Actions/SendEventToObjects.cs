using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D1 RID: 209
	[Category("✫ Utility")]
	[Description("Send a Graph Event to multiple gameobjects which should have a GraphOwner component attached.")]
	public class SendEventToObjects : ActionTask
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000E132 File Offset: 0x0000C332
		protected override string info
		{
			get
			{
				return string.Format("Send Event [{0}] to {1}", this.eventName, this.targetObjects);
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000E14C File Offset: 0x0000C34C
		protected override void OnExecute()
		{
			foreach (GameObject gameObject in this.targetObjects.value)
			{
				if (gameObject != null)
				{
					GraphOwner component = gameObject.GetComponent<GraphOwner>();
					if (component != null)
					{
						component.SendEvent(this.eventName.value, null, this);
					}
				}
			}
			base.EndAction();
		}

		// Token: 0x0400026F RID: 623
		[RequiredField]
		public BBParameter<List<GameObject>> targetObjects;

		// Token: 0x04000270 RID: 624
		[RequiredField]
		public BBParameter<string> eventName;
	}
}
