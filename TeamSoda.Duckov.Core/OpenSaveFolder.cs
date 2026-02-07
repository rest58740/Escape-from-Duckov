using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001CE RID: 462
public class OpenSaveFolder : MonoBehaviour
{
	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x0003A26D File Offset: 0x0003846D
	private string filePath
	{
		get
		{
			return Application.persistentDataPath;
		}
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x0003A274 File Offset: 0x00038474
	private void Update()
	{
		if (Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.lKey.isPressed)
		{
			this.OpenFolder();
		}
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x0003A29E File Offset: 0x0003849E
	public void OpenFolder()
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = this.filePath,
			UseShellExecute = true
		});
	}
}
