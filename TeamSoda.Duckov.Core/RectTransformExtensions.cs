using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
public static class RectTransformExtensions
{
	// Token: 0x06000556 RID: 1366 RVA: 0x000183AE File Offset: 0x000165AE
	public static Camera GetUICamera()
	{
		return null;
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x000183B4 File Offset: 0x000165B4
	public static bool MatchWorldPosition(this RectTransform rectTransform, Vector3 worldPosition, Vector3 worldSpaceOffset = default(Vector3))
	{
		RectTransform rectTransform2 = rectTransform.parent as RectTransform;
		if (rectTransform2 == null)
		{
			return false;
		}
		worldPosition += worldSpaceOffset;
		Camera main = Camera.main;
		if (main == null)
		{
			return false;
		}
		Vector2 vector = RectTransformUtility.WorldToScreenPoint(main, worldPosition);
		Vector2 vector2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform2, vector, RectTransformExtensions.GetUICamera(), ref vector2);
		Vector3 vector3 = worldPosition - main.transform.position;
		bool result = Vector3.Dot(main.transform.forward, vector3) > 0f;
		rectTransform.localPosition = vector2;
		return result;
	}
}
