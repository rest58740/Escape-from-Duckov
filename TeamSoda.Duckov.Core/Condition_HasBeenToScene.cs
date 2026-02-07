using System;
using Duckov.Quests;
using Duckov.Scenes;

// Token: 0x0200011B RID: 283
public class Condition_HasBeenToScene : Condition
{
	// Token: 0x060009C0 RID: 2496 RVA: 0x0002B349 File Offset: 0x00029549
	public override bool Evaluate()
	{
		return MultiSceneCore.GetVisited(this.sceneID);
	}

	// Token: 0x040008B5 RID: 2229
	[SceneID]
	public string sceneID;
}
