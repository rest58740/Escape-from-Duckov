using System;
using UnityEngine;

namespace EPOOutline.Demo
{
	// Token: 0x0200000A RID: 10
	public class UsecaseSwitcher : MonoBehaviour
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000026E0 File Offset: 0x000008E0
		private void Start()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(i == 0);
			}
			this.currentSelected = base.transform.GetChild(0);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002730 File Offset: 0x00000930
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				int num = this.currentSelected.GetSiblingIndex();
				base.transform.GetChild(num).gameObject.SetActive(false);
				num++;
				this.currentSelected = base.transform.GetChild(num % base.transform.childCount);
				this.currentSelected.gameObject.SetActive(true);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				int num2 = this.currentSelected.GetSiblingIndex();
				base.transform.GetChild(num2).gameObject.SetActive(false);
				num2--;
				if (num2 < 0)
				{
					num2 = base.transform.childCount - 1;
				}
				this.currentSelected = base.transform.GetChild(num2);
				this.currentSelected.gameObject.SetActive(true);
			}
		}

		// Token: 0x04000026 RID: 38
		private Transform currentSelected;
	}
}
