using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200003C RID: 60
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-skewinghandle-sd/")]
	public class SkewingHandleSD : MonoBehaviour
	{
		// Token: 0x060001EE RID: 494 RVA: 0x00008ACA File Offset: 0x00006CCA
		public bool IsAttachedToSelf()
		{
			return this.volumetricLightBeam != null && this.volumetricLightBeam.gameObject == base.gameObject;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008AF2 File Offset: 0x00006CF2
		public bool CanSetSkewingVector()
		{
			return this.volumetricLightBeam != null && this.volumetricLightBeam.canHaveMeshSkewing;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008B0F File Offset: 0x00006D0F
		public bool CanUpdateEachFrame()
		{
			return this.CanSetSkewingVector() && this.volumetricLightBeam.trackChangesDuringPlaytime;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008B26 File Offset: 0x00006D26
		private bool ShouldUpdateEachFrame()
		{
			return this.shouldUpdateEachFrame && this.CanUpdateEachFrame();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008B38 File Offset: 0x00006D38
		private void OnEnable()
		{
			if (this.CanSetSkewingVector())
			{
				this.SetSkewingVector();
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008B48 File Offset: 0x00006D48
		private void Start()
		{
			if (Application.isPlaying && this.ShouldUpdateEachFrame())
			{
				base.StartCoroutine(this.CoUpdate());
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008B66 File Offset: 0x00006D66
		private IEnumerator CoUpdate()
		{
			while (this.ShouldUpdateEachFrame())
			{
				this.SetSkewingVector();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008B78 File Offset: 0x00006D78
		private void SetSkewingVector()
		{
			Vector3 skewingLocalForwardDirection = this.volumetricLightBeam.transform.InverseTransformPoint(base.transform.position);
			this.volumetricLightBeam.skewingLocalForwardDirection = skewingLocalForwardDirection;
		}

		// Token: 0x04000142 RID: 322
		public const string ClassName = "SkewingHandleSD";

		// Token: 0x04000143 RID: 323
		public VolumetricLightBeamSD volumetricLightBeam;

		// Token: 0x04000144 RID: 324
		public bool shouldUpdateEachFrame;
	}
}
