using System;
using Duckov.Buildings;
using Duckov.Quests;

// Token: 0x0200011F RID: 287
public class Conditon_BuildingConstructed : Condition
{
	// Token: 0x060009C9 RID: 2505 RVA: 0x0002B478 File Offset: 0x00029678
	public override bool Evaluate()
	{
		bool flag = BuildingManager.Any(this.buildingID, false);
		if (this.not)
		{
			flag = !flag;
		}
		return flag;
	}

	// Token: 0x040008BA RID: 2234
	public string buildingID;

	// Token: 0x040008BB RID: 2235
	public bool not;
}
