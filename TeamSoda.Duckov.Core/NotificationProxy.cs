using System;
using Duckov.UI;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class NotificationProxy : MonoBehaviour
{
	// Token: 0x06000670 RID: 1648 RVA: 0x0001D217 File Offset: 0x0001B417
	public void Notify()
	{
		NotificationText.Push(LocalizationManager.ToPlainText(this.notification));
	}

	// Token: 0x04000635 RID: 1589
	[LocalizationKey("Default")]
	public string notification;
}
