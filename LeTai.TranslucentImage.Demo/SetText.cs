using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x0200000A RID: 10
	public class SetText : MonoBehaviour
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000025F3 File Offset: 0x000007F3
		private void Awake()
		{
			this.text = base.GetComponent<Text>();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002601 File Offset: 0x00000801
		public void Set(float value)
		{
			this.text.text = value.ToString(this.format);
		}

		// Token: 0x0400001F RID: 31
		public string format = "0.0";

		// Token: 0x04000020 RID: 32
		private Text text;
	}
}
