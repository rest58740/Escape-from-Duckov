using System;
using UnityEngine;

namespace ECM2.Walkthrough.EX74
{
	// Token: 0x02000068 RID: 104
	public class OneWayPlatform : MonoBehaviour
	{
		// Token: 0x06000364 RID: 868 RVA: 0x0000F168 File Offset: 0x0000D368
		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player"))
			{
				return;
			}
			Character component = other.GetComponent<Character>();
			if (component)
			{
				component.IgnoreCollision(this.platformCollider, true);
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000F1A0 File Offset: 0x0000D3A0
		private void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("Player"))
			{
				return;
			}
			Character component = other.GetComponent<Character>();
			if (component)
			{
				component.IgnoreCollision(this.platformCollider, false);
			}
		}

		// Token: 0x0400025B RID: 603
		public Collider platformCollider;
	}
}
