using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000054 RID: 84
	[Category("✫ Utility")]
	[Description("Check for an invoked Signal with agent as the target. If Signal was invoked as global, then the target does not matter.")]
	public class CheckSignal : ConditionTask<Transform>
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00007B55 File Offset: 0x00005D55
		protected override string info
		{
			get
			{
				return this.signalDefinition.ToString();
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007B62 File Offset: 0x00005D62
		protected override string OnInit()
		{
			if (this.signalDefinition.isNoneOrNull)
			{
				return "Missing Definition";
			}
			return null;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007B78 File Offset: 0x00005D78
		protected override void OnEnable()
		{
			this.signalDefinition.value.onInvoke -= this.OnSignalInvoke;
			this.signalDefinition.value.onInvoke += this.OnSignalInvoke;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007BB2 File Offset: 0x00005DB2
		protected override void OnDisable()
		{
			this.signalDefinition.value.onInvoke -= this.OnSignalInvoke;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007BD0 File Offset: 0x00005DD0
		private void OnSignalInvoke(Transform sender, Transform receiver, bool isGlobal, params object[] args)
		{
			if (receiver == base.agent || isGlobal)
			{
				SignalDefinition value = this.signalDefinition.value;
				for (int i = 0; i < args.Length; i++)
				{
					this.argumentsMap[value.parameters[i].ID].value = args[i];
				}
				base.YieldReturn(true);
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007C34 File Offset: 0x00005E34
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x040000FE RID: 254
		public BBParameter<SignalDefinition> signalDefinition;

		// Token: 0x040000FF RID: 255
		[SerializeField]
		private Dictionary<string, BBObjectParameter> argumentsMap = new Dictionary<string, BBObjectParameter>();
	}
}
