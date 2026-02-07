using System;
using TMPro;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000247 RID: 583
	public class GameVersionDisplay : MonoBehaviour
	{
		// Token: 0x0600125A RID: 4698 RVA: 0x00047240 File Offset: 0x00045440
		private void Start()
		{
			this.text.text = string.Format("v{0}", GameMetaData.Instance.Version);
		}

		// Token: 0x04000E2A RID: 3626
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
