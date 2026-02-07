using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000013 RID: 19
	[Category("✫ Blackboard")]
	[Obsolete("Use CheckVariable(T)")]
	public class CheckUnityObject : ConditionTask
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002894 File Offset: 0x00000A94
		protected override string info
		{
			get
			{
				BBParameter<Object> bbparameter = this.valueA;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string text2 = " == ";
				BBParameter<Object> bbparameter2 = this.valueB;
				return text + text2 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000028C4 File Offset: 0x00000AC4
		protected override bool OnCheck()
		{
			return this.valueA.value == this.valueB.value;
		}

		// Token: 0x04000029 RID: 41
		[BlackboardOnly]
		public BBParameter<Object> valueA;

		// Token: 0x0400002A RID: 42
		public BBParameter<Object> valueB;
	}
}
