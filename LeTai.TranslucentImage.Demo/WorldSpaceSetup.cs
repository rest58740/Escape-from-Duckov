using System;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x0200000F RID: 15
	public class WorldSpaceSetup : MonoBehaviour
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000029C4 File Offset: 0x00000BC4
		public void Toggle()
		{
			this.sceneCamera.cullingMask ^= 1 << LayerMask.NameToLayer("UI");
			this.uiCamera.gameObject.SetActive(!this.uiCamera.gameObject.activeSelf);
		}

		// Token: 0x0400002B RID: 43
		public Camera sceneCamera;

		// Token: 0x0400002C RID: 44
		public Camera uiCamera;
	}
}
