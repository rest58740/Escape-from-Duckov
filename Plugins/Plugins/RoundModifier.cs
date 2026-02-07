using System;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000009 RID: 9
[ModifierID("Round")]
public class RoundModifier : ProceduralImageModifier
{
	// Token: 0x06000030 RID: 48 RVA: 0x000027DD File Offset: 0x000009DD
	public override Vector4 CalculateRadius(Rect imageRect)
	{
		float num = Mathf.Min(imageRect.width, imageRect.height) * 0.5f;
		return new Vector4(num, num, num, num);
	}
}
