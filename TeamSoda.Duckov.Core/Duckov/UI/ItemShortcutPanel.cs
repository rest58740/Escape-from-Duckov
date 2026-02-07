using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C9 RID: 969
	public class ItemShortcutPanel : MonoBehaviour
	{
		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0007A24C File Offset: 0x0007844C
		// (set) Token: 0x060022E8 RID: 8936 RVA: 0x0007A254 File Offset: 0x00078454
		public Inventory Target { get; private set; }

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x0007A25D File Offset: 0x0007845D
		// (set) Token: 0x060022EA RID: 8938 RVA: 0x0007A265 File Offset: 0x00078465
		public CharacterMainControl Character { get; internal set; }

		// Token: 0x060022EB RID: 8939 RVA: 0x0007A26E File Offset: 0x0007846E
		private void Awake()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.Initialize();
			}
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0007A28E File Offset: 0x0007848E
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x0007A2A1 File Offset: 0x000784A1
		private void OnLevelInitialized()
		{
			this.Initialize();
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x0007A2AC File Offset: 0x000784AC
		private void Initialize()
		{
			LevelManager instance = LevelManager.Instance;
			this.Character = ((instance != null) ? instance.MainCharacter : null);
			if (this.Character == null)
			{
				return;
			}
			LevelManager instance2 = LevelManager.Instance;
			Inventory target;
			if (instance2 == null)
			{
				target = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance2.MainCharacter;
				if (mainCharacter == null)
				{
					target = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					target = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			this.Target = target;
			if (this.Target == null)
			{
				return;
			}
			for (int i = 0; i < this.buttons.Length; i++)
			{
				ItemShortcutButton itemShortcutButton = this.buttons[i];
				if (!(itemShortcutButton == null))
				{
					itemShortcutButton.Initialize(this, i);
				}
			}
			this.initialized = true;
		}

		// Token: 0x04001796 RID: 6038
		[SerializeField]
		private ItemShortcutButton[] buttons;

		// Token: 0x04001799 RID: 6041
		private bool initialized;
	}
}
