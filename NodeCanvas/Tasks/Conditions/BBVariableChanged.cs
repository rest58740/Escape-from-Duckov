using System;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000019 RID: 25
	[Name("On Variable Changed", 0)]
	[Category("✫ Blackboard")]
	public class BBVariableChanged : ConditionTask
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002ABA File Offset: 0x00000CBA
		protected override string info
		{
			get
			{
				BBObjectParameter bbobjectParameter = this.targetVariable;
				return ((bbobjectParameter != null) ? bbobjectParameter.ToString() : null) + " Changed.";
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002AD8 File Offset: 0x00000CD8
		protected override string OnInit()
		{
			if (this.targetVariable.isNone)
			{
				return "Blackboard Variable not set.";
			}
			return null;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002AEE File Offset: 0x00000CEE
		protected override void OnEnable()
		{
			this.targetVariable.varRef.onValueChanged += new Action<object>(this.OnValueChanged);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002B0C File Offset: 0x00000D0C
		protected override void OnDisable()
		{
			this.targetVariable.varRef.onValueChanged -= new Action<object>(this.OnValueChanged);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002B2A File Offset: 0x00000D2A
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002B2D File Offset: 0x00000D2D
		private void OnValueChanged(object varValue)
		{
			base.YieldReturn(true);
		}

		// Token: 0x04000037 RID: 55
		[BlackboardOnly]
		public BBObjectParameter targetVariable;
	}
}
