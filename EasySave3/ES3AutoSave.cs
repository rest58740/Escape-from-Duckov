using System;
using System.Collections.Generic;
using ES3Internal;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class ES3AutoSave : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06000009 RID: 9 RVA: 0x0000217A File Offset: 0x0000037A
	public void Reset()
	{
		this.saveLayer = false;
		this.saveTag = false;
		this.saveName = false;
		this.saveHideFlags = false;
		this.saveActive = false;
		this.saveChildren = false;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000021A6 File Offset: 0x000003A6
	public void Awake()
	{
		if (ES3AutoSaveMgr.Current == null)
		{
			ES3Debug.LogWarning("<b>No GameObjects in this scene will be autosaved</b> because there is no Easy Save 3 Manager. To add a manager to this scene, exit playmode and go to Assets > Easy Save 3 > Add Manager to Scene.", this, 0);
			return;
		}
		ES3AutoSaveMgr.AddAutoSave(this);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000021C8 File Offset: 0x000003C8
	public void OnApplicationQuit()
	{
		this.isQuitting = true;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000021D1 File Offset: 0x000003D1
	public void OnDestroy()
	{
		if (!this.isQuitting)
		{
			ES3AutoSaveMgr.RemoveAutoSave(this);
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000021E1 File Offset: 0x000003E1
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000021E3 File Offset: 0x000003E3
	public void OnAfterDeserialize()
	{
		this.componentsToSave.RemoveAll((Component c) => c == null || c.GetType() == typeof(Component));
	}

	// Token: 0x04000003 RID: 3
	public bool saveLayer = true;

	// Token: 0x04000004 RID: 4
	public bool saveTag = true;

	// Token: 0x04000005 RID: 5
	public bool saveName = true;

	// Token: 0x04000006 RID: 6
	public bool saveHideFlags = true;

	// Token: 0x04000007 RID: 7
	public bool saveActive = true;

	// Token: 0x04000008 RID: 8
	public bool saveChildren;

	// Token: 0x04000009 RID: 9
	private bool isQuitting;

	// Token: 0x0400000A RID: 10
	public List<Component> componentsToSave = new List<Component>();
}
