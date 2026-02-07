using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000029 RID: 41
	[Category("GameObject")]
	public class IsWithinLayerMask : ConditionTask<Transform>
	{
		// Token: 0x06000098 RID: 152 RVA: 0x0000467A File Offset: 0x0000287A
		protected override bool OnCheck()
		{
			return base.agent.gameObject.IsInLayerMask(this.targetLayers.value);
		}

		// Token: 0x0400007A RID: 122
		public BBParameter<LayerMask> targetLayers;
	}
}
