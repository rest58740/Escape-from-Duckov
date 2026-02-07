using System;
using TMPro;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200007C RID: 124
	[ExecuteAlways]
	public class ShapesTextPool : ShapesObjPool<TextMeshProShapes, ShapesTextPool>
	{
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0001CAE2 File Offset: 0x0001ACE2
		public override string PoolTypeName
		{
			get
			{
				return "Text";
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0001CAE9 File Offset: 0x0001ACE9
		public override void OnCreatedNewComponent(TextMeshProShapes comp)
		{
			comp.textWrappingMode = TextWrappingModes.NoWrap;
			comp.overflowMode = TextOverflowModes.Overflow;
			comp.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
