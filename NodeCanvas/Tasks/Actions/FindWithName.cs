using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000098 RID: 152
	[Category("GameObject")]
	public class FindWithName : ActionTask
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000A1F8 File Offset: 0x000083F8
		protected override string info
		{
			get
			{
				string text = "Find Object ";
				BBParameter<string> bbparameter = this.gameObjectName;
				string text2 = (bbparameter != null) ? bbparameter.ToString() : null;
				string text3 = " as ";
				BBParameter<GameObject> bbparameter2 = this.saveAs;
				return text + text2 + text3 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A22D File Offset: 0x0000842D
		protected override void OnExecute()
		{
			this.saveAs.value = GameObject.Find(this.gameObjectName.value);
			base.EndAction();
		}

		// Token: 0x040001B3 RID: 435
		[RequiredField]
		public BBParameter<string> gameObjectName;

		// Token: 0x040001B4 RID: 436
		[BlackboardOnly]
		public BBParameter<GameObject> saveAs;
	}
}
