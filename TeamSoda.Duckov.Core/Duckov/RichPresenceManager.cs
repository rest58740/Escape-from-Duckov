using System;
using Duckov.Scenes;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023F RID: 575
	public class RichPresenceManager : MonoBehaviour
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00046A85 File Offset: 0x00044C85
		public bool isPlaying
		{
			get
			{
				return !this.isMainMenu;
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00046A90 File Offset: 0x00044C90
		private void InvokeChangeEvent()
		{
			Action<RichPresenceManager> onInstanceChanged = RichPresenceManager.OnInstanceChanged;
			if (onInstanceChanged == null)
			{
				return;
			}
			onInstanceChanged(this);
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00046AA4 File Offset: 0x00044CA4
		private void Awake()
		{
			MainMenu.OnMainMenuAwake = (Action)Delegate.Combine(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Combine(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			MultiSceneCore.OnInstanceAwake += this.OnMultiSceneCoreInstanceAwake;
			MultiSceneCore.OnInstanceDestroy += this.OnMultiSceneCoreInstanceDestroy;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00046B14 File Offset: 0x00044D14
		private void OnDestroy()
		{
			MainMenu.OnMainMenuAwake = (Action)Delegate.Remove(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Remove(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			MultiSceneCore.OnInstanceAwake -= this.OnMultiSceneCoreInstanceAwake;
			MultiSceneCore.OnInstanceDestroy -= this.OnMultiSceneCoreInstanceDestroy;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00046B83 File Offset: 0x00044D83
		private void OnMainMenuAwake()
		{
			this.isMainMenu = true;
			this.InvokeChangeEvent();
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00046B92 File Offset: 0x00044D92
		private void OnMainMenuDestroy()
		{
			this.isMainMenu = false;
			this.InvokeChangeEvent();
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00046BA1 File Offset: 0x00044DA1
		private void OnMultiSceneCoreInstanceAwake(MultiSceneCore core)
		{
			this.levelDisplayNameRaw = core.DisplaynameRaw;
			this.isInLevel = true;
			this.InvokeChangeEvent();
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00046BBC File Offset: 0x00044DBC
		private void OnMultiSceneCoreInstanceDestroy(MultiSceneCore core)
		{
			this.isInLevel = false;
			this.InvokeChangeEvent();
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00046BCB File Offset: 0x00044DCB
		internal string GetSteamDisplay()
		{
			if (Application.isEditor)
			{
				return "#Status_UnityEditor";
			}
			if (!this.isMainMenu)
			{
				return "#Status_Playing";
			}
			return "#Status_MainMenu";
		}

		// Token: 0x04000E0D RID: 3597
		public bool isMainMenu = true;

		// Token: 0x04000E0E RID: 3598
		public bool isInLevel;

		// Token: 0x04000E0F RID: 3599
		public string levelDisplayNameRaw;

		// Token: 0x04000E10 RID: 3600
		public static Action<RichPresenceManager> OnInstanceChanged;
	}
}
