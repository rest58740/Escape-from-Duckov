using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000060 RID: 96
	[Name("Set Layer Weight", 0)]
	[Category("Animator")]
	public class MecanimSetLayerWeight : ActionTask<Animator>
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00008741 File Offset: 0x00006941
		protected override string info
		{
			get
			{
				string text = "Set Layer ";
				BBParameter<int> bbparameter = this.layerIndex;
				string text2 = (bbparameter != null) ? bbparameter.ToString() : null;
				string text3 = ", weight ";
				BBParameter<float> bbparameter2 = this.layerWeight;
				return text + text2 + text3 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008776 File Offset: 0x00006976
		protected override void OnExecute()
		{
			this.currentValue = base.agent.GetLayerWeight(this.layerIndex.value);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008794 File Offset: 0x00006994
		protected override void OnUpdate()
		{
			float weight = (this.transitTime > 0f) ? Mathf.Lerp(this.currentValue, this.layerWeight.value, base.elapsedTime / this.transitTime) : this.layerWeight.value;
			base.agent.SetLayerWeight(this.layerIndex.value, weight);
			if (base.elapsedTime >= this.transitTime)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x0400012C RID: 300
		public BBParameter<int> layerIndex;

		// Token: 0x0400012D RID: 301
		[SliderField(0, 1)]
		public BBParameter<float> layerWeight;

		// Token: 0x0400012E RID: 302
		[SliderField(0, 1)]
		public float transitTime;

		// Token: 0x0400012F RID: 303
		private float currentValue;
	}
}
