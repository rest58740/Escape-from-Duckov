using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000093 RID: 147
	[Category("GameObject")]
	[Description("Action will end in Failure if no objects are found")]
	public class FindAllWithTag : ActionTask
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000283 RID: 643 RVA: 0x00009F4A File Offset: 0x0000814A
		protected override string info
		{
			get
			{
				string text = "GetObjects '";
				BBParameter<string> bbparameter = this.searchTag;
				string text2 = (bbparameter != null) ? bbparameter.ToString() : null;
				string text3 = "' as ";
				BBParameter<List<GameObject>> bbparameter2 = this.saveAs;
				return text + text2 + text3 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00009F7F File Offset: 0x0000817F
		protected override void OnExecute()
		{
			this.saveAs.value = GameObject.FindGameObjectsWithTag(this.searchTag.value).ToList<GameObject>();
			base.EndAction(this.saveAs.value.Count != 0);
		}

		// Token: 0x040001A7 RID: 423
		[RequiredField]
		[TagField]
		public BBParameter<string> searchTag = "Untagged";

		// Token: 0x040001A8 RID: 424
		[BlackboardOnly]
		public BBParameter<List<GameObject>> saveAs;
	}
}
