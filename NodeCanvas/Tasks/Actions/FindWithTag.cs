using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000099 RID: 153
	[Category("GameObject")]
	public class FindWithTag : ActionTask
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000A258 File Offset: 0x00008458
		protected override string info
		{
			get
			{
				string text = "GetObject '";
				string text2 = this.searchTag;
				string text3 = "' as ";
				BBParameter<GameObject> bbparameter = this.saveAs;
				return text + text2 + text3 + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000A281 File Offset: 0x00008481
		protected override void OnExecute()
		{
			this.saveAs.value = GameObject.FindWithTag(this.searchTag);
			base.EndAction();
		}

		// Token: 0x040001B5 RID: 437
		[RequiredField]
		[TagField]
		public string searchTag = "Untagged";

		// Token: 0x040001B6 RID: 438
		[BlackboardOnly]
		public BBParameter<GameObject> saveAs;
	}
}
