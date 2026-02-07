using System;
using System.IO;
using Saves;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001AC RID: 428
public class CopySaveFileButton : MonoBehaviour
{
	// Token: 0x06000CE4 RID: 3300 RVA: 0x00036E77 File Offset: 0x00035077
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.Apply));
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x00036E98 File Offset: 0x00035098
	public void Apply()
	{
		string text = Path.Combine(Application.streamingAssetsPath, this.fromFile);
		if (!File.Exists(text))
		{
			return;
		}
		ES3.CacheFile(text);
		ES3.DeleteFile(SavesSystem.CurrentFilePath);
		ES3.CopyFile(text, SavesSystem.CurrentFilePath);
		ES3.StoreCachedFile(SavesSystem.CurrentFilePath);
		SavesSystem.SetFile(SavesSystem.CurrentSlot);
	}

	// Token: 0x04000B3F RID: 2879
	[SerializeField]
	private string fromFile = "Saves/Backup.sav";

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private Button button;
}
