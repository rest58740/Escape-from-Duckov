using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000068 RID: 104
	[Category("✫ Blackboard/Dictionaries")]
	public class GetDictionaryElement<T> : ActionTask
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00008B6E File Offset: 0x00006D6E
		protected override string info
		{
			get
			{
				return string.Format("{0} = {1}[{2}]", this.saveAs, this.dictionary, this.key);
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00008B8C File Offset: 0x00006D8C
		protected override void OnExecute()
		{
			if (this.dictionary.value == null)
			{
				base.EndAction(false);
				return;
			}
			this.saveAs.value = this.dictionary.value[this.key.value];
			base.EndAction();
		}

		// Token: 0x04000144 RID: 324
		[BlackboardOnly]
		[RequiredField]
		public BBParameter<Dictionary<string, T>> dictionary;

		// Token: 0x04000145 RID: 325
		public BBParameter<string> key;

		// Token: 0x04000146 RID: 326
		[BlackboardOnly]
		public BBParameter<T> saveAs;
	}
}
