using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class PaperBox : MonoBehaviour
{
	// Token: 0x060005DE RID: 1502 RVA: 0x0001A61C File Offset: 0x0001881C
	private void Update()
	{
		if (!this.character)
		{
			return;
		}
		if (!this.setActiveWhileStandStill)
		{
			return;
		}
		bool flag = this.character.Velocity.magnitude < 0.2f;
		if (this.setActiveWhileStandStill.gameObject.activeSelf != flag)
		{
			this.setActiveWhileStandStill.gameObject.SetActive(flag);
		}
	}

	// Token: 0x04000569 RID: 1385
	[HideInInspector]
	public CharacterMainControl character;

	// Token: 0x0400056A RID: 1386
	public Transform setActiveWhileStandStill;
}
