using System;
using System.Collections;
using UnityEngine;

namespace EPOOutline.Demo
{
	// Token: 0x02000007 RID: 7
	public class Doughnut : MonoBehaviour, ICollectable
	{
		// Token: 0x06000019 RID: 25 RVA: 0x0000252F File Offset: 0x0000072F
		private void Start()
		{
			this.outlinable = base.GetComponent<Outlinable>();
			this.amplitudeShift = UnityEngine.Random.Range(0f, 10f);
			this.initialPosition = base.transform.position;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002564 File Offset: 0x00000764
		private void Update()
		{
			if (!this.isCollected)
			{
				base.transform.position = this.initialPosition + Vector3.up * Mathf.Sin(Time.time * this.moveSpeed + this.amplitudeShift);
			}
			base.transform.Rotate(Vector3.up * this.rotationSpeed * Time.smoothDeltaTime, Space.World);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025D7 File Offset: 0x000007D7
		public void Collect(GameObject collector)
		{
			if (this.isCollected)
			{
				return;
			}
			this.isCollected = true;
			base.StartCoroutine(this.AnimateCollection(collector));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025F7 File Offset: 0x000007F7
		private IEnumerator AnimateCollection(GameObject collector)
		{
			AudioSource.PlayClipAtPoint(this.eatSound, base.transform.position, 10f);
			float duration = 0.2f;
			float collectionRadius = 1.5f;
			float collectionAngle = UnityEngine.Random.Range(0f, 360f);
			float timeLeft = duration;
			while (collector != null && timeLeft > 0f)
			{
				timeLeft -= Time.smoothDeltaTime;
				Vector3 b = Quaternion.Euler(0f, collectionAngle, 0f) * Vector3.right;
				Vector3 b2 = collector.transform.position + b + Vector3.up * 4.5f;
				base.transform.position = Vector3.Lerp(base.transform.position, b2, Time.smoothDeltaTime * 5f);
				collectionAngle += Time.smoothDeltaTime * 360f;
				collectionRadius = Mathf.MoveTowards(collectionRadius, 0f, Time.smoothDeltaTime * 3.5f);
				yield return null;
			}
			timeLeft = duration;
			Vector3 initialScale = base.transform.localScale;
			while (timeLeft >= 0f)
			{
				timeLeft -= Time.smoothDeltaTime;
				base.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, 1f - timeLeft / duration);
				yield return null;
			}
			base.transform.localScale = Vector3.zero;
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x0400001B RID: 27
		[SerializeField]
		private float rotationSpeed = 30f;

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private AudioClip eatSound;

		// Token: 0x0400001D RID: 29
		[SerializeField]
		private float moveAmplitude = 0.25f;

		// Token: 0x0400001E RID: 30
		[SerializeField]
		private float moveSpeed = 0.2f;

		// Token: 0x0400001F RID: 31
		private Outlinable outlinable;

		// Token: 0x04000020 RID: 32
		private Vector3 initialPosition;

		// Token: 0x04000021 RID: 33
		private float amplitudeShift;

		// Token: 0x04000022 RID: 34
		private bool isCollected;
	}
}
