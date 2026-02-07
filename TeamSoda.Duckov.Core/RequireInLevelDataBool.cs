using System;
using Duckov.Quests;
using Duckov.Scenes;

// Token: 0x02000122 RID: 290
public class RequireInLevelDataBool : Condition
{
	// Token: 0x060009CF RID: 2511 RVA: 0x0002B500 File Offset: 0x00029700
	public override bool Evaluate()
	{
		if (!MultiSceneCore.Instance)
		{
			return false;
		}
		if (!this.keyHashInited)
		{
			this.InitKeyHash();
		}
		object obj;
		return !this.isEmptyString && (MultiSceneCore.Instance.inLevelData.TryGetValue(this.keyHash, out obj) && obj is bool) && (bool)obj;
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0002B55C File Offset: 0x0002975C
	private void InitKeyHash()
	{
		if (this.keyString == "")
		{
			this.isEmptyString = true;
		}
		this.keyHash = this.keyString.GetHashCode();
		this.keyHashInited = true;
	}

	// Token: 0x040008C0 RID: 2240
	public string keyString = "";

	// Token: 0x040008C1 RID: 2241
	private int keyHash = -1;

	// Token: 0x040008C2 RID: 2242
	private bool keyHashInited;

	// Token: 0x040008C3 RID: 2243
	private bool isEmptyString;
}
