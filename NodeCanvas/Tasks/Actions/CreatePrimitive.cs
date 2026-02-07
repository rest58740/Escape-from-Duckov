using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200008F RID: 143
	[Category("GameObject")]
	public class CreatePrimitive : ActionTask
	{
		// Token: 0x06000278 RID: 632 RVA: 0x00009D5C File Offset: 0x00007F5C
		protected override void OnExecute()
		{
			GameObject gameObject = GameObject.CreatePrimitive(this.type.value);
			gameObject.name = this.objectName.value;
			gameObject.transform.position = this.position.value;
			gameObject.transform.eulerAngles = this.rotation.value;
			this.saveAs.value = gameObject;
			base.EndAction();
		}

		// Token: 0x0400019D RID: 413
		public BBParameter<string> objectName;

		// Token: 0x0400019E RID: 414
		public BBParameter<Vector3> position;

		// Token: 0x0400019F RID: 415
		public BBParameter<Vector3> rotation;

		// Token: 0x040001A0 RID: 416
		public BBParameter<PrimitiveType> type;

		// Token: 0x040001A1 RID: 417
		[BlackboardOnly]
		public BBParameter<GameObject> saveAs;
	}
}
