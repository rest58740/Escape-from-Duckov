using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200005D RID: 93
	[Name("Set Parameter Float", 0)]
	[Category("Animator")]
	[Description("You can either use a parameter name OR hashID. Leave the parameter name empty or none to use hashID instead.")]
	public class MecanimSetFloat : ActionTask<Animator>
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00008440 File Offset: 0x00006640
		protected override string info
		{
			get
			{
				return string.Format("Mec.SetFloat {0} to {1}", (string.IsNullOrEmpty(this.parameter.value) && !this.parameter.useBlackboard) ? this.parameterHashID.ToString() : this.parameter.ToString(), this.setTo);
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008494 File Offset: 0x00006694
		protected override void OnExecute()
		{
			if (this.transitTime <= 0f)
			{
				this.Set(this.setTo.value);
				base.EndAction();
				return;
			}
			this.currentValue = this.Get();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000084C7 File Offset: 0x000066C7
		protected override void OnUpdate()
		{
			this.Set(Mathf.Lerp(this.currentValue, this.setTo.value, base.elapsedTime / this.transitTime));
			if (base.elapsedTime >= this.transitTime)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008508 File Offset: 0x00006708
		private float Get()
		{
			if (!string.IsNullOrEmpty(this.parameter.value))
			{
				return base.agent.GetFloat(this.parameter.value);
			}
			return base.agent.GetFloat(this.parameterHashID.value);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008554 File Offset: 0x00006754
		private void Set(float newValue)
		{
			if (!string.IsNullOrEmpty(this.parameter.value))
			{
				base.agent.SetFloat(this.parameter.value, newValue);
				return;
			}
			base.agent.SetFloat(this.parameterHashID.value, newValue);
		}

		// Token: 0x04000121 RID: 289
		public BBParameter<string> parameter;

		// Token: 0x04000122 RID: 290
		public BBParameter<int> parameterHashID;

		// Token: 0x04000123 RID: 291
		public BBParameter<float> setTo;

		// Token: 0x04000124 RID: 292
		[SliderField(0, 1)]
		public float transitTime = 0.25f;

		// Token: 0x04000125 RID: 293
		private float currentValue;
	}
}
