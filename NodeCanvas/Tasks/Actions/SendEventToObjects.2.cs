using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D2 RID: 210
	[Category("✫ Utility")]
	[Description("Send a Graph Event to multiple gameobjects which should have a GraphOwner component attached.")]
	public class SendEventToObjects<T> : ActionTask
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		protected override string info
		{
			get
			{
				return string.Format("Send Event [{0}]({1}) to {2}", this.eventName, this.eventValue, this.targetObjects);
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000E1F8 File Offset: 0x0000C3F8
		protected override void OnExecute()
		{
			foreach (GameObject gameObject in this.targetObjects.value)
			{
				GraphOwner component = gameObject.GetComponent<GraphOwner>();
				if (component != null)
				{
					component.SendEvent<T>(this.eventName.value, this.eventValue.value, this);
				}
			}
			base.EndAction();
		}

		// Token: 0x04000271 RID: 625
		[RequiredField]
		public BBParameter<List<GameObject>> targetObjects;

		// Token: 0x04000272 RID: 626
		[RequiredField]
		public BBParameter<string> eventName;

		// Token: 0x04000273 RID: 627
		public BBParameter<T> eventValue;
	}
}
