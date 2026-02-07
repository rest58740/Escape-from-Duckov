using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200006A RID: 106
public class CheatingManager : MonoBehaviour
{
	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000427 RID: 1063 RVA: 0x000124BC File Offset: 0x000106BC
	public static CheatingManager Instance
	{
		get
		{
			return CheatingManager._instance;
		}
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x000124C3 File Offset: 0x000106C3
	private void Awake()
	{
		CheatingManager._instance = this;
		CheatMode.Activate();
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x000124D0 File Offset: 0x000106D0
	private void Update()
	{
		if (!CheatMode.Active)
		{
			return;
		}
		if (!CharacterMainControl.Main)
		{
			return;
		}
		if (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.equalsKey.wasPressedThisFrame)
		{
			this.ToggleInvincible();
		}
		if (Keyboard.current != null && Keyboard.current.numpadMultiplyKey.wasPressedThisFrame)
		{
			this.typing = !this.typing;
			if (this.typing)
			{
				this.typingID = 0;
				this.LogCurrentTypingID();
			}
			else
			{
				this.LockItem();
			}
		}
		this.UpdateTyping();
		if (Keyboard.current != null && this.typing && Keyboard.current.backspaceKey.wasPressedThisFrame && this.typingID > 0)
		{
			this.typingID /= 10;
			this.LogCurrentTypingID();
		}
		if (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed && Mouse.current.backButton.wasPressedThisFrame)
		{
			this.CheatMove();
		}
		if (Keyboard.current != null && Keyboard.current.leftAltKey.isPressed && Keyboard.current.sKey.wasPressedThisFrame)
		{
			SleepView.Instance.Open(null);
		}
		if (Keyboard.current != null && Keyboard.current.numpadPlusKey.wasPressedThisFrame)
		{
			if (this.typing)
			{
				this.LockItem();
				this.typing = false;
			}
			this.CreateItem(this.lockedItem, -1);
		}
		if (Keyboard.current != null && Keyboard.current.numpadMinusKey.wasPressedThisFrame)
		{
			int displayingItemID = ItemHoveringUI.DisplayingItemID;
			if (displayingItemID > 0)
			{
				this.SetTypedItem(displayingItemID);
				this.CreateItem(this.lockedItem, -1);
			}
		}
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0001267C File Offset: 0x0001087C
	private void UpdateTyping()
	{
		if (Keyboard.current != null && Keyboard.current.numpad0Key.wasPressedThisFrame)
		{
			this.TypeOne(0);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad1Key.wasPressedThisFrame)
		{
			this.TypeOne(1);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad2Key.wasPressedThisFrame)
		{
			this.TypeOne(2);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad3Key.wasPressedThisFrame)
		{
			this.TypeOne(3);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad4Key.wasPressedThisFrame)
		{
			this.TypeOne(4);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad5Key.wasPressedThisFrame)
		{
			this.TypeOne(5);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad6Key.wasPressedThisFrame)
		{
			this.TypeOne(6);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad7Key.wasPressedThisFrame)
		{
			this.TypeOne(7);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad8Key.wasPressedThisFrame)
		{
			this.TypeOne(8);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad9Key.wasPressedThisFrame)
		{
			this.TypeOne(9);
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x000127CC File Offset: 0x000109CC
	private void LogCurrentTypingID()
	{
		if (this.typingID <= 0)
		{
			CharacterMainControl.Main.PopText("_", 999f);
			return;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.typingID);
		if (metaData.id > 0)
		{
			CharacterMainControl.Main.PopText(string.Format(" {0}_  ({1})", this.typingID, metaData.DisplayName), 999f);
			return;
		}
		CharacterMainControl.Main.PopText(string.Format("{0}_", this.typingID), 999f);
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0001285C File Offset: 0x00010A5C
	private void TypeOne(int i)
	{
		this.typingID = this.typingID * 10 + i;
		this.LogCurrentTypingID();
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00012875 File Offset: 0x00010A75
	private void SetTypedItem(int id)
	{
		this.typingID = id;
		this.LockItem();
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00012884 File Offset: 0x00010A84
	private void LockItem()
	{
		this.typing = false;
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.typingID);
		if (metaData.id <= 0)
		{
			CharacterMainControl.Main.PopText("没有这个物品。", 999f);
			return;
		}
		this.lockedItem = this.typingID;
		CharacterMainControl.Main.PopText(metaData.DisplayName + " 已选定", 999f);
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x000128F0 File Offset: 0x00010AF0
	public void CreateItem(int id, int quantity = -1)
	{
		if (ItemAssetsCollection.GetMetaData(id).id <= 0)
		{
			CharacterMainControl.Main.PopText("没有这个物品。", 999f);
			return;
		}
		this.CreateItemAsync(id, quantity).Forget();
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00012930 File Offset: 0x00010B30
	private UniTaskVoid CreateItemAsync(int id, int quantity = -1)
	{
		CheatingManager.<CreateItemAsync>d__15 <CreateItemAsync>d__;
		<CreateItemAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateItemAsync>d__.id = id;
		<CreateItemAsync>d__.quantity = quantity;
		<CreateItemAsync>d__.<>1__state = -1;
		<CreateItemAsync>d__.<>t__builder.Start<CheatingManager.<CreateItemAsync>d__15>(ref <CreateItemAsync>d__);
		return <CreateItemAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0001297B File Offset: 0x00010B7B
	private void ToggleTypeing()
	{
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00012980 File Offset: 0x00010B80
	public void ToggleInvincible()
	{
		this.isInvincible = !this.isInvincible;
		CharacterMainControl.Main.Health.SetInvincible(this.isInvincible);
		CharacterMainControl.Main.PopText(this.isInvincible ? "我无敌了" : "我不无敌了", -1f);
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x000129D4 File Offset: 0x00010BD4
	public void CheatMove()
	{
		Vector2 vector = Mouse.current.position.ReadValue();
		Ray ray = LevelManager.Instance.GameCamera.renderCamera.ScreenPointToRay(vector);
		LayerMask layerMask = GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, ref raycastHit, 100f, layerMask, 1))
		{
			CharacterMainControl.Main.SetPosition(raycastHit.point);
		}
	}

	// Token: 0x0400031C RID: 796
	private static CheatingManager _instance;

	// Token: 0x0400031D RID: 797
	private bool isInvincible;

	// Token: 0x0400031E RID: 798
	private bool typing;

	// Token: 0x0400031F RID: 799
	private int typingID;

	// Token: 0x04000320 RID: 800
	private int lockedItem;
}
