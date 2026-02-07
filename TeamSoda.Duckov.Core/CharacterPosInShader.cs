using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class CharacterPosInShader : MonoBehaviour
{
	// Token: 0x060008C0 RID: 2240 RVA: 0x000277CF File Offset: 0x000259CF
	private void Update()
	{
		if (!CharacterMainControl.Main)
		{
			return;
		}
		Shader.SetGlobalVector(this.characterPosHash, CharacterMainControl.Main.transform.position);
	}

	// Token: 0x04000809 RID: 2057
	private int characterPosHash = Shader.PropertyToID("CharacterPos");
}
