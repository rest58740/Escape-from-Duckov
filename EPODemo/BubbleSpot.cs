using System;
using System.Collections;
using UnityEngine;

namespace EPOOutline.Demo
{
	// Token: 0x02000003 RID: 3
	public class BubbleSpot : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BE File Offset: 0x000002BE
		private IEnumerator Start()
		{
			this.Hide(0f);
			if (!this.visibleFromBegining)
			{
				yield break;
			}
			yield return new WaitForSeconds(this.showDelay);
			this.Show();
			yield return new WaitForSeconds(this.showDuration);
			this.Hide();
			yield break;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020CD File Offset: 0x000002CD
		private void Reset()
		{
			this.targetCamera = Camera.main;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020DC File Offset: 0x000002DC
		private void OnTriggerEnter(Collider other)
		{
			if (!other.GetComponent<Character>())
			{
				return;
			}
			int num = this.playersInside;
			this.playersInside = num + 1;
			if (num == 0)
			{
				this.Show();
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002110 File Offset: 0x00000310
		private void OnTriggerExit(Collider other)
		{
			if (!other.GetComponent<Character>())
			{
				return;
			}
			int num = this.playersInside - 1;
			this.playersInside = num;
			if (num == 0)
			{
				this.Hide();
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002144 File Offset: 0x00000344
		private void Show()
		{
			if (this.wasShown && this.once)
			{
				return;
			}
			this.wasShown = true;
			this.Show(0.5f);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002169 File Offset: 0x00000369
		private void Hide()
		{
			this.Hide(0.15f);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002176 File Offset: 0x00000376
		private void Hide(float duration)
		{
			this.bubble.gameObject.SetActive(false);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002189 File Offset: 0x00000389
		private void Show(float duration)
		{
			this.bubble.gameObject.SetActive(true);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000219C File Offset: 0x0000039C
		private void Update()
		{
			if (this.trackPosition)
			{
				base.transform.position = this.trackPosition.position + this.trackShift;
			}
			this.bubble.transform.position = this.targetCamera.WorldToScreenPoint(base.transform.position);
		}

		// Token: 0x04000001 RID: 1
		[SerializeField]
		private Transform trackPosition;

		// Token: 0x04000002 RID: 2
		[SerializeField]
		private Vector3 trackShift;

		// Token: 0x04000003 RID: 3
		[SerializeField]
		private Camera targetCamera;

		// Token: 0x04000004 RID: 4
		[SerializeField]
		private Transform bubble;

		// Token: 0x04000005 RID: 5
		[SerializeField]
		private bool visibleFromBegining;

		// Token: 0x04000006 RID: 6
		[SerializeField]
		private float showDelay;

		// Token: 0x04000007 RID: 7
		[SerializeField]
		private float showDuration = 5f;

		// Token: 0x04000008 RID: 8
		[SerializeField]
		private bool once;

		// Token: 0x04000009 RID: 9
		private bool wasShown;

		// Token: 0x0400000A RID: 10
		private int playersInside;
	}
}
