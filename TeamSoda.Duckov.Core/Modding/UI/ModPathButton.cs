using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Modding.UI
{
	// Token: 0x0200027E RID: 638
	public class ModPathButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001443 RID: 5187 RVA: 0x0004BFF3 File Offset: 0x0004A1F3
		private void OnEnable()
		{
			this.pathText.text = ModManager.DefaultModFolderPath;
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0004C005 File Offset: 0x0004A205
		public void OnPointerClick(PointerEventData eventData)
		{
			GUIUtility.systemCopyBuffer = ModManager.DefaultModFolderPath;
		}

		// Token: 0x04000F18 RID: 3864
		[SerializeField]
		private TextMeshProUGUI pathText;
	}
}
