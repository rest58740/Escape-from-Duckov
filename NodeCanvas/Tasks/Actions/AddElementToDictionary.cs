using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000067 RID: 103
	[Category("✫ Blackboard/Dictionaries")]
	public class AddElementToDictionary<T> : ActionTask
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00008AF8 File Offset: 0x00006CF8
		protected override string info
		{
			get
			{
				return string.Format("{0}[{1}] = {2}", this.dictionary, this.key, this.value);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00008B18 File Offset: 0x00006D18
		protected override void OnExecute()
		{
			if (this.dictionary.value == null)
			{
				base.EndAction(false);
				return;
			}
			this.dictionary.value[this.key.value] = this.value.value;
			base.EndAction();
		}

		// Token: 0x04000141 RID: 321
		[BlackboardOnly]
		[RequiredField]
		public BBParameter<Dictionary<string, T>> dictionary;

		// Token: 0x04000142 RID: 322
		public BBParameter<string> key;

		// Token: 0x04000143 RID: 323
		public BBParameter<T> value;
	}
}
