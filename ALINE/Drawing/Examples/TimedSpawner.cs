using System;
using System.Collections;
using UnityEngine;

namespace Drawing.Examples
{
	// Token: 0x02000061 RID: 97
	public class TimedSpawner : MonoBehaviour
	{
		// Token: 0x060003C8 RID: 968 RVA: 0x00012DBA File Offset: 0x00010FBA
		private IEnumerator Start()
		{
			for (;;)
			{
				GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.prefab, base.transform.position + UnityEngine.Random.insideUnitSphere * 0.01f, UnityEngine.Random.rotation);
				base.StartCoroutine(this.DestroyAfter(go, this.lifeTime));
				yield return new WaitForSeconds(this.interval);
			}
			yield break;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00012DC9 File Offset: 0x00010FC9
		private IEnumerator DestroyAfter(GameObject go, float delay)
		{
			yield return new WaitForSeconds(delay);
			UnityEngine.Object.Destroy(go);
			yield break;
		}

		// Token: 0x0400018E RID: 398
		public float interval = 1f;

		// Token: 0x0400018F RID: 399
		public float lifeTime = 5f;

		// Token: 0x04000190 RID: 400
		public GameObject prefab;
	}
}
