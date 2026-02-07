using System;
using UnityEngine;
using UnityEngine.XR;

namespace EPOOutline
{
	// Token: 0x0200001F RID: 31
	public static class XRUtility
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00006119 File Offset: 0x00004319
		public static bool IsXRActive
		{
			get
			{
				return XRSettings.enabled && XRSettings.isDeviceActive;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00006129 File Offset: 0x00004329
		public static RenderTextureDescriptor VRRenderTextureDescriptor
		{
			get
			{
				return XRSettings.eyeTextureDesc;
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006130 File Offset: 0x00004330
		public static bool IsUsingVR(OutlineParameters parameters)
		{
			return XRUtility.IsXRActive && !parameters.IsEditorCamera && parameters.EyeMask > StereoTargetEyeMask.None;
		}
	}
}
