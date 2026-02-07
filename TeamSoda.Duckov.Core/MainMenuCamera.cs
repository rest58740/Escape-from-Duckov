using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000186 RID: 390
public class MainMenuCamera : MonoBehaviour
{
	// Token: 0x06000BF8 RID: 3064 RVA: 0x00033100 File Offset: 0x00031300
	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		float num = (float)Screen.width;
		float num2 = (float)Screen.height;
		float num3 = mousePosition.x / num;
		float num4 = mousePosition.y / num2;
		base.transform.localRotation = quaternion.Euler(0f, Mathf.Lerp(this.yawRange.x, this.yawRange.y, num3) * 0.017453292f, 0f, 4);
		if (this.pitchRoot)
		{
			this.pitchRoot.localRotation = quaternion.Euler(Mathf.Lerp(this.pitchRange.x, this.pitchRange.y, num4) * 0.017453292f, 0f, 0f, 4);
		}
		base.transform.localPosition = new Vector3(Mathf.Lerp(this.posRangeX.x, this.posRangeX.y, num3), Mathf.Lerp(this.posRangeY.x, this.posRangeY.y, num4), 0f);
	}

	// Token: 0x04000A40 RID: 2624
	public Vector2 yawRange;

	// Token: 0x04000A41 RID: 2625
	public Vector2 pitchRange;

	// Token: 0x04000A42 RID: 2626
	public Transform pitchRoot;

	// Token: 0x04000A43 RID: 2627
	[FormerlySerializedAs("posRange")]
	public Vector2 posRangeX;

	// Token: 0x04000A44 RID: 2628
	public Vector2 posRangeY;
}
