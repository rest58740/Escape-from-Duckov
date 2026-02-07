using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Spline_Renderer.Example
{
	// Token: 0x02000053 RID: 83
	public class UISplineRendererExample : MonoBehaviour
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000CC90 File Offset: 0x0000AE90
		private void Start()
		{
			this.target_interaction.GetComponent<Button>().onClick.AddListener(delegate()
			{
				Debug.Log("Spline Clicked !");
			});
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000CCC6 File Offset: 0x0000AEC6
		private void Update()
		{
			this.UpdateUV();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000CCD0 File Offset: 0x0000AED0
		private void UpdateUV()
		{
			this.target_uvAnimation.uvOffset += new Vector2(0f, Time.deltaTime * 2f);
			this.target_uvAnimation.clipRange = new Vector2(0f, (Mathf.Sin(Time.time) + 1f) * 0.5f);
		}

		// Token: 0x040001DD RID: 477
		public UISplineRenderer target_uvAnimation;

		// Token: 0x040001DE RID: 478
		public UISplineRenderer target_interaction;
	}
}
