using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000113 RID: 275
	internal class PrioritySelector_0 : BTComposite
	{
		// Token: 0x04000311 RID: 785
		[SerializeField]
		public List<BBParameter<float>> priorities;
	}
}
