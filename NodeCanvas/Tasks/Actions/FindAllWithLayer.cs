using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000091 RID: 145
	[Category("GameObject")]
	[Description("Action will end in Failure if no objects are found")]
	public class FindAllWithLayer : ActionTask
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00009E1D File Offset: 0x0000801D
		protected override string info
		{
			get
			{
				string text = "GetObjects in '";
				BBParameter<LayerMask> bbparameter = this.targetLayers;
				string text2 = (bbparameter != null) ? bbparameter.ToString() : null;
				string text3 = "' as ";
				BBParameter<List<GameObject>> bbparameter2 = this.saveAs;
				return text + text2 + text3 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00009E52 File Offset: 0x00008052
		protected override void OnExecute()
		{
			this.saveAs.value = ObjectUtils.FindGameObjectsWithinLayerMask(this.targetLayers.value, null).ToList<GameObject>();
			base.EndAction(this.saveAs.value.Count != 0);
		}

		// Token: 0x040001A3 RID: 419
		[RequiredField]
		public BBParameter<LayerMask> targetLayers;

		// Token: 0x040001A4 RID: 420
		[BlackboardOnly]
		public BBParameter<List<GameObject>> saveAs;
	}
}
