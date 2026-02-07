using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000016 RID: 22
	[Category("✫ Blackboard/Dictionaries")]
	public class TryGetValue<T> : ConditionTask
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004C RID: 76 RVA: 0x000029C5 File Offset: 0x00000BC5
		protected override string info
		{
			get
			{
				return string.Format("{0}.TryGetValue({1} as {2})", this.targetDictionary, this.key, this.saveValueAs);
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000029E4 File Offset: 0x00000BE4
		protected override bool OnCheck()
		{
			if (this.targetDictionary.value == null)
			{
				return false;
			}
			T value;
			if (this.targetDictionary.value.TryGetValue(this.key.value, ref value))
			{
				this.saveValueAs.value = value;
				return true;
			}
			return false;
		}

		// Token: 0x04000031 RID: 49
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<Dictionary<string, T>> targetDictionary;

		// Token: 0x04000032 RID: 50
		[RequiredField]
		public BBParameter<string> key;

		// Token: 0x04000033 RID: 51
		[BlackboardOnly]
		public BBParameter<T> saveValueAs;
	}
}
