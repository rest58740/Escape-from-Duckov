using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000074 RID: 116
	[Category("✫ Blackboard/Lists")]
	public class PickRandomListElement<T> : ActionTask
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00009059 File Offset: 0x00007259
		protected override string info
		{
			get
			{
				return string.Format("{0} = Random From {1}", this.saveAs, this.targetList);
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009074 File Offset: 0x00007274
		protected override void OnExecute()
		{
			if (this.targetList.value.Count <= 0)
			{
				base.EndAction(false);
				return;
			}
			this.saveAs.value = this.targetList.value[Random.Range(0, this.targetList.value.Count)];
			base.EndAction(true);
		}

		// Token: 0x04000161 RID: 353
		[RequiredField]
		public BBParameter<List<T>> targetList;

		// Token: 0x04000162 RID: 354
		public BBParameter<T> saveAs;
	}
}
