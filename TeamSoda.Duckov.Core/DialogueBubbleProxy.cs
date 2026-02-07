using System;
using Cysharp.Threading.Tasks;
using Duckov.UI.DialogueBubbles;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class DialogueBubbleProxy : MonoBehaviour
{
	// Token: 0x06000D37 RID: 3383 RVA: 0x00037D48 File Offset: 0x00035F48
	public void Pop()
	{
		if (this.byMainCharacter && CharacterMainControl.Main)
		{
			CharacterMainControl.Main.PopText(LocalizationManager.ToPlainText(this.textKey), -1f);
			return;
		}
		DialogueBubblesManager.Show(LocalizationManager.ToPlainText(this.textKey), base.transform, this.yOffset, false, false, -1f, this.duration).Forget();
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x00037DB4 File Offset: 0x00035FB4
	public void Pop(string text, float speed = -1f)
	{
		if (this.byMainCharacter && CharacterMainControl.Main)
		{
			CharacterMainControl.Main.PopText(LocalizationManager.ToPlainText(this.textKey), -1f);
			return;
		}
		DialogueBubblesManager.Show(text, base.transform, this.yOffset, false, false, speed, 2f).Forget();
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x00037E0F File Offset: 0x0003600F
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(base.transform.position + Vector3.up * this.yOffset, Vector3.one * 0.2f);
	}

	// Token: 0x04000B7E RID: 2942
	[LocalizationKey("Dialogues")]
	public string textKey;

	// Token: 0x04000B7F RID: 2943
	public float yOffset;

	// Token: 0x04000B80 RID: 2944
	public float duration = 2f;

	// Token: 0x04000B81 RID: 2945
	public bool byMainCharacter;
}
