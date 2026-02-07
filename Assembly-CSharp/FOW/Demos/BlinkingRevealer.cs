using System;
using UnityEngine;

namespace FOW.Demos
{
	// Token: 0x02000060 RID: 96
	public class BlinkingRevealer : MonoBehaviour
	{
		// Token: 0x0600033B RID: 827 RVA: 0x0000E0FB File Offset: 0x0000C2FB
		private void Awake()
		{
			if (this.RandomOffset)
			{
				this.BlinkCycleTime += UnityEngine.Random.Range(0f, this.BlinkCycleTime * 0.5f);
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000E128 File Offset: 0x0000C328
		private void Update()
		{
			if (Time.time % this.BlinkCycleTime < this.BlinkCycleTime / 2f)
			{
				if (!base.transform.GetChild(0).gameObject.activeInHierarchy)
				{
					base.transform.GetChild(0).gameObject.SetActive(true);
					return;
				}
			}
			else if (base.transform.GetChild(0).gameObject.activeInHierarchy)
			{
				base.transform.GetChild(0).gameObject.SetActive(false);
			}
		}

		// Token: 0x04000225 RID: 549
		public float BlinkCycleTime = 5f;

		// Token: 0x04000226 RID: 550
		public bool RandomOffset = true;
	}
}
