using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class RectStructRefTest : MonoBehaviour
{
	// Token: 0x060004D2 RID: 1234 RVA: 0x0001630C File Offset: 0x0001450C
	private void Test()
	{
		Rect rect;
		rect..ctor(Vector2.up, Vector2.one);
		Debug.Log("original: " + rect.size.ToString());
		Rect rect2 = rect;
		rect2.xMax = 20f;
		Debug.Log("Changed");
		Debug.Log("rect: " + rect.size.ToString());
		Debug.Log("rect2: " + rect2.size.ToString());
	}
}
