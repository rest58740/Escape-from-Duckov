using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000397 RID: 919
	public class CopyTextButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06002009 RID: 8201 RVA: 0x00070F5A File Offset: 0x0006F15A
		public void OnPointerClick(PointerEventData eventData)
		{
			GUIUtility.systemCopyBuffer = this.text;
		}

		// Token: 0x040015F1 RID: 5617
		[SerializeField]
		private string text;
	}
}
