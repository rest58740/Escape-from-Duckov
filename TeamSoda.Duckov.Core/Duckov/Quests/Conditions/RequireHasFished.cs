using System;
using Saves;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200037D RID: 893
	public class RequireHasFished : Condition
	{
		// Token: 0x06001F47 RID: 8007 RVA: 0x0006EBA6 File Offset: 0x0006CDA6
		public override bool Evaluate()
		{
			return RequireHasFished.GetHasFished();
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0006EBAD File Offset: 0x0006CDAD
		public static void SetHasFished()
		{
			SavesSystem.Save<bool>("HasFished", true);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0006EBBA File Offset: 0x0006CDBA
		public static bool GetHasFished()
		{
			return SavesSystem.Load<bool>("HasFished");
		}
	}
}
