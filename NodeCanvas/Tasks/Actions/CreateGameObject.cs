using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200008E RID: 142
	[Category("GameObject")]
	public class CreateGameObject : ActionTask
	{
		// Token: 0x06000276 RID: 630 RVA: 0x00009CF8 File Offset: 0x00007EF8
		protected override void OnExecute()
		{
			GameObject gameObject = new GameObject(this.objectName.value);
			gameObject.transform.position = this.position.value;
			gameObject.transform.eulerAngles = this.rotation.value;
			this.saveAs.value = gameObject;
			base.EndAction();
		}

		// Token: 0x04000199 RID: 409
		public BBParameter<string> objectName;

		// Token: 0x0400019A RID: 410
		public BBParameter<Vector3> position;

		// Token: 0x0400019B RID: 411
		public BBParameter<Vector3> rotation;

		// Token: 0x0400019C RID: 412
		[BlackboardOnly]
		public BBParameter<GameObject> saveAs;
	}
}
