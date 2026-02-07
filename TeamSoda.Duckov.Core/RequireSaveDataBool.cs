using System;
using Duckov.Quests;
using Saves;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class RequireSaveDataBool : Condition
{
	// Token: 0x060009D2 RID: 2514 RVA: 0x0002B5AC File Offset: 0x000297AC
	public override bool Evaluate()
	{
		bool flag = SavesSystem.Load<bool>(this.key);
		Debug.Log(string.Format("Load bool:{0}  value:{1}", this.key, flag));
		return flag == this.requireValue;
	}

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	private string key;

	// Token: 0x040008C5 RID: 2245
	[SerializeField]
	private bool requireValue;
}
