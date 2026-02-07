using System;
using UnityEngine;

// Token: 0x020000CB RID: 203
public class LittleMoveHUD : MonoBehaviour
{
	// Token: 0x0600067C RID: 1660 RVA: 0x0001D610 File Offset: 0x0001B810
	private void LateUpdate()
	{
		if (!this.character)
		{
			if (LevelManager.Instance)
			{
				this.character = LevelManager.Instance.MainCharacter;
			}
			if (!this.character)
			{
				return;
			}
		}
		if (!this.camera)
		{
			this.camera = Camera.main;
			if (!this.camera)
			{
				return;
			}
		}
		Vector3 vector = this.character.transform.position + this.offset;
		this.worldPos = Vector3.SmoothDamp(this.worldPos, vector, ref this.velocityTemp, this.smoothTime);
		if (Vector3.Distance(this.worldPos, vector) > this.maxDistance)
		{
			this.worldPos = (this.worldPos - vector).normalized * this.maxDistance + vector;
		}
		Vector3 position = this.camera.WorldToScreenPoint(this.worldPos);
		base.transform.position = position;
	}

	// Token: 0x04000644 RID: 1604
	private Camera camera;

	// Token: 0x04000645 RID: 1605
	private CharacterMainControl character;

	// Token: 0x04000646 RID: 1606
	public float maxDistance = 2f;

	// Token: 0x04000647 RID: 1607
	public float smoothTime;

	// Token: 0x04000648 RID: 1608
	private Vector3 worldPos;

	// Token: 0x04000649 RID: 1609
	private Vector3 velocityTemp;

	// Token: 0x0400064A RID: 1610
	public Vector3 offset;
}
