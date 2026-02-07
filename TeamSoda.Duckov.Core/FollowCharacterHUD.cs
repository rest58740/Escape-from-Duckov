using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class FollowCharacterHUD : MonoBehaviour
{
	// Token: 0x06000678 RID: 1656 RVA: 0x0001D489 File Offset: 0x0001B689
	private void Awake()
	{
		GameCamera.OnCameraPosUpdate = (Action<GameCamera, CharacterMainControl>)Delegate.Combine(GameCamera.OnCameraPosUpdate, new Action<GameCamera, CharacterMainControl>(this.UpdatePos));
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0001D4AB File Offset: 0x0001B6AB
	private void OnDestroy()
	{
		GameCamera.OnCameraPosUpdate = (Action<GameCamera, CharacterMainControl>)Delegate.Remove(GameCamera.OnCameraPosUpdate, new Action<GameCamera, CharacterMainControl>(this.UpdatePos));
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0001D4D0 File Offset: 0x0001B6D0
	private void UpdatePos(GameCamera gameCamera, CharacterMainControl target)
	{
		Camera renderCamera = gameCamera.renderCamera;
		Vector3 forward = renderCamera.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		Vector3 right = renderCamera.transform.right;
		right.y = 0f;
		right.Normalize();
		Vector3 vector = target.transform.position + forward * this.offset.y + right * this.offset.x;
		this.worldPos = Vector3.SmoothDamp(this.worldPos, vector, ref this.velocityTemp, this.smoothTime);
		if (Vector3.Distance(this.worldPos, vector) > this.maxDistance)
		{
			this.worldPos = (this.worldPos - vector).normalized * this.maxDistance + vector;
		}
		Vector3 position = renderCamera.WorldToScreenPoint(this.worldPos);
		base.transform.position = position;
		if (target.gameObject.activeInHierarchy != base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(target.gameObject.activeInHierarchy);
		}
	}

	// Token: 0x0400063F RID: 1599
	public float maxDistance = 2f;

	// Token: 0x04000640 RID: 1600
	public float smoothTime;

	// Token: 0x04000641 RID: 1601
	private Vector3 worldPos;

	// Token: 0x04000642 RID: 1602
	private Vector3 velocityTemp;

	// Token: 0x04000643 RID: 1603
	public Vector3 offset;
}
