using System;
using Duckov.UI;
using Saves;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000139 RID: 313
public class BaseSceneUtilities : MonoBehaviour
{
	// Token: 0x06000A46 RID: 2630 RVA: 0x0002C95F File Offset: 0x0002AB5F
	private void Save()
	{
		LevelManager.Instance.SaveMainCharacter();
		SavesSystem.CollectSaveData();
		SavesSystem.SaveFile(true);
		this.lastTimeSaved = Time.realtimeSinceStartup;
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0002C981 File Offset: 0x0002AB81
	private float TimeSinceLastSave
	{
		get
		{
			return Time.realtimeSinceStartup - this.lastTimeSaved;
		}
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x0002C98F File Offset: 0x0002AB8F
	private void Awake()
	{
		this.lastTimeSaved = Time.realtimeSinceStartup;
		ManagedUIElement.onOpen += this.OnViewOpenClose;
		ManagedUIElement.onClose += this.OnViewOpenClose;
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x0002C9BE File Offset: 0x0002ABBE
	private void OnDestroy()
	{
		ManagedUIElement.onOpen -= this.OnViewOpenClose;
		ManagedUIElement.onClose -= this.OnViewOpenClose;
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x0002C9E4 File Offset: 0x0002ABE4
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (Keyboard.current.altKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			this.TrySetDirty();
		}
		if (this.saveCoolTimer > 0f)
		{
			this.saveCoolTimer -= Time.unscaledDeltaTime;
		}
		if (this.dirty && this.saveCoolTimer <= 0f)
		{
			this.Save();
			this.dirty = false;
			this.saveCoolTimer = this.saveCoolTime;
		}
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x0002CA6E File Offset: 0x0002AC6E
	private void OnViewOpenClose(ManagedUIElement uiElement)
	{
		this.TrySetDirty();
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x0002CA76 File Offset: 0x0002AC76
	private void TrySetDirty()
	{
		if (this.saveCoolTime - this.saveCoolTimer < 0.5f)
		{
			return;
		}
		this.dirty = true;
	}

	// Token: 0x0400091F RID: 2335
	[SerializeField]
	private float saveCoolTime = 5f;

	// Token: 0x04000920 RID: 2336
	private float saveCoolTimer = 5f;

	// Token: 0x04000921 RID: 2337
	private bool dirty;

	// Token: 0x04000922 RID: 2338
	private float lastTimeSaved = float.MinValue;
}
