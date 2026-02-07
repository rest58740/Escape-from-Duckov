using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class AISpecialAttachmentBase : MonoBehaviour
{
	// Token: 0x06000502 RID: 1282 RVA: 0x00016BA2 File Offset: 0x00014DA2
	public void Init(AICharacterController _ai, CharacterMainControl _character)
	{
		this.aiCharacterController = _ai;
		this.character = _character;
		this.OnInited();
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00016BB8 File Offset: 0x00014DB8
	protected virtual void OnInited()
	{
	}

	// Token: 0x04000439 RID: 1081
	public AICharacterController aiCharacterController;

	// Token: 0x0400043A RID: 1082
	public CharacterMainControl character;
}
