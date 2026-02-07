using System;
using Saves;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class SaveDataBoolProxy : MonoBehaviour
{
	// Token: 0x060005ED RID: 1517 RVA: 0x0001AB38 File Offset: 0x00018D38
	public void Save()
	{
		SavesSystem.Save<bool>(this.key, this.value);
		Debug.Log(string.Format("SetSaveData:{0} to {1}", this.key, this.value));
	}

	// Token: 0x0400057A RID: 1402
	public string key;

	// Token: 0x0400057B RID: 1403
	public bool value;
}
