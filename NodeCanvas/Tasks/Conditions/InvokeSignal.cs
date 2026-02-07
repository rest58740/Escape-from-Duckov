using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000007 RID: 7
	[Category("✫ Utility")]
	[Description("Invoke a defined Signal with agent as the target and optionally global.")]
	public class InvokeSignal : ActionTask<Transform>
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000023F5 File Offset: 0x000005F5
		protected override string info
		{
			get
			{
				return this.signalDefinition.ToString();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002402 File Offset: 0x00000602
		protected override string OnInit()
		{
			if (this.signalDefinition.isNoneOrNull)
			{
				return "Missing Definition";
			}
			this.args = new object[this.argumentsMap.Count];
			return null;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002430 File Offset: 0x00000630
		protected override void OnExecute()
		{
			SignalDefinition value = this.signalDefinition.value;
			for (int i = 0; i < value.parameters.Count; i++)
			{
				this.args[i] = this.argumentsMap[value.parameters[i].ID].value;
			}
			value.Invoke(base.agent, base.agent, this.global, this.args);
			base.EndAction();
		}

		// Token: 0x0400000D RID: 13
		public BBParameter<SignalDefinition> signalDefinition;

		// Token: 0x0400000E RID: 14
		public bool global;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private Dictionary<string, BBObjectParameter> argumentsMap = new Dictionary<string, BBObjectParameter>();

		// Token: 0x04000010 RID: 16
		private object[] args;
	}
}
