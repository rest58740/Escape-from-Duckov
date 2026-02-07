using System;

// Token: 0x0200009A RID: 154
public static class CharacterMainControlExtensions
{
	// Token: 0x06000554 RID: 1364 RVA: 0x00018310 File Offset: 0x00016510
	public static bool IsMainCharacter(this CharacterMainControl character)
	{
		if (character == null)
		{
			return false;
		}
		LevelManager instance = LevelManager.Instance;
		return ((instance != null) ? instance.MainCharacter : null) == character;
	}
}
