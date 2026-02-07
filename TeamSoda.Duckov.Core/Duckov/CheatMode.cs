using System;
using System.IO;
using Saves;

namespace Duckov
{
	// Token: 0x02000249 RID: 585
	public class CheatMode
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x00047300 File Offset: 0x00045500
		// (set) Token: 0x06001264 RID: 4708 RVA: 0x00047307 File Offset: 0x00045507
		public static bool Active
		{
			get
			{
				return CheatMode._acitive;
			}
			private set
			{
				CheatMode._acitive = value;
				Action<bool> onCheatModeStatusChanged = CheatMode.OnCheatModeStatusChanged;
				if (onCheatModeStatusChanged == null)
				{
					return;
				}
				onCheatModeStatusChanged(value);
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06001265 RID: 4709 RVA: 0x00047320 File Offset: 0x00045520
		// (remove) Token: 0x06001266 RID: 4710 RVA: 0x00047354 File Offset: 0x00045554
		public static event Action<bool> OnCheatModeStatusChanged;

		// Token: 0x06001267 RID: 4711 RVA: 0x00047387 File Offset: 0x00045587
		public static void Activate()
		{
			if (!CheatMode.CheatFileExists())
			{
				return;
			}
			CheatMode.Active = true;
			SavesSystem.Save<bool>("Cheated", true);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x000473A2 File Offset: 0x000455A2
		public static void Deactivate()
		{
			CheatMode.Active = false;
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x000473AA File Offset: 0x000455AA
		private bool Cheated
		{
			get
			{
				return SavesSystem.Load<bool>("Cheated");
			}
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x000473B6 File Offset: 0x000455B6
		private static bool CheatFileExists()
		{
			return File.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "WWSSADADBA"));
		}

		// Token: 0x04000E2D RID: 3629
		private static bool _acitive;
	}
}
