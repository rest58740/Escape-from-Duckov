using System;
using System.Collections;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000088 RID: 136
	public class CameraFader : MonoBehaviour
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00009986 File Offset: 0x00007B86
		private Texture2D blackTexture
		{
			get
			{
				if (this._blackTexture == null)
				{
					this._blackTexture = new Texture2D(1, 1);
					this._blackTexture.SetPixel(1, 1, Color.black);
					this._blackTexture.Apply();
				}
				return this._blackTexture;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600025F RID: 607 RVA: 0x000099C6 File Offset: 0x00007BC6
		public static CameraFader current
		{
			get
			{
				if (CameraFader._current == null)
				{
					CameraFader._current = Object.FindAnyObjectByType<CameraFader>();
				}
				if (CameraFader._current == null)
				{
					CameraFader._current = new GameObject("_CameraFader").AddComponent<CameraFader>();
				}
				return CameraFader._current;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009A05 File Offset: 0x00007C05
		public void FadeIn(float time)
		{
			base.StartCoroutine(this.CoroutineFadeIn(time));
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009A15 File Offset: 0x00007C15
		public void FadeOut(float time)
		{
			base.StartCoroutine(this.CoroutineFadeOut(time));
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009A25 File Offset: 0x00007C25
		private IEnumerator CoroutineFadeIn(float time)
		{
			this.alpha = 1f;
			if (time <= 0f)
			{
				this.alpha = 0f;
			}
			while (this.alpha > 0f)
			{
				yield return null;
				this.alpha -= 1f / time * Time.deltaTime;
			}
			yield break;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009A3B File Offset: 0x00007C3B
		private IEnumerator CoroutineFadeOut(float time)
		{
			this.alpha = 0f;
			if (time <= 0f)
			{
				this.alpha = 1f;
			}
			while (this.alpha < 1f)
			{
				yield return null;
				this.alpha += 1f / time * Time.deltaTime;
			}
			yield break;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009A54 File Offset: 0x00007C54
		private void OnGUI()
		{
			if (this.alpha <= 0f)
			{
				return;
			}
			GUI.color = new Color(1f, 1f, 1f, this.alpha);
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackTexture);
			GUI.color = Color.white;
		}

		// Token: 0x0400018E RID: 398
		private static CameraFader _current;

		// Token: 0x0400018F RID: 399
		private float alpha;

		// Token: 0x04000190 RID: 400
		private Texture2D _blackTexture;
	}
}
