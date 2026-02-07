using System;
using Cysharp.Threading.Tasks;
using Duckov.Economy;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Consoles
{
	// Token: 0x0200031B RID: 795
	[CreateAssetMenu(menuName = "Duckov/Console/Give")]
	public class Give : DCommand
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x0005F67C File Offset: 0x0005D87C
		public override string CommandWord
		{
			get
			{
				return "give";
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0005F684 File Offset: 0x0005D884
		public override string Execute(DConsole console, string[] args)
		{
			if (args.Length < 1)
			{
				return "Failed giving. Args not enough";
			}
			string text = args[0];
			int num = -1;
			if (!int.TryParse(text, out num))
			{
				num = ItemAssetsCollection.TryGetIDByName(text, true);
				if (num < 0)
				{
					return "Failed finding item type: " + text;
				}
			}
			int num2 = 1;
			int num3;
			if (args.Length >= 2 && int.TryParse(args[1], out num3))
			{
				num2 = num3;
			}
			if (num2 < 0)
			{
				return "Amount error.";
			}
			if (!LevelManager.LevelInited)
			{
				return "This command is in-game only. Abort.";
			}
			Cost cost = new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(num, (long)num2)
			});
			cost.Return(false, true, 1, null).Forget();
			return string.Format("Giving {0} x{1}", text, num2);
		}
	}
}
