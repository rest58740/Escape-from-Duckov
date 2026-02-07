using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200000B RID: 11
	public class HiderToggleObjects : HiderBehavior
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00004504 File Offset: 0x00002704
		protected override void OnHide()
		{
			GameObject[] array = this.RevealedObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			array = this.HiddenObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004550 File Offset: 0x00002750
		protected override void OnReveal()
		{
			GameObject[] array = this.RevealedObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			array = this.HiddenObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}

		// Token: 0x0400007F RID: 127
		[Tooltip("Objects that will be visible when in Line Of Sight")]
		public GameObject[] RevealedObjects;

		// Token: 0x04000080 RID: 128
		[Tooltip("Objects that will be visible when out of Line Of Sight")]
		public GameObject[] HiddenObjects;
	}
}
