using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace EPOOutline
{
	// Token: 0x02000020 RID: 32
	public static class RenderTargetUtility
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x0000614C File Offset: 0x0000434C
		public static RenderTextureFormat GetRTFormat(bool useHDR)
		{
			if (!useHDR)
			{
				return RenderTextureFormat.ARGB32;
			}
			return RenderTargetUtility.GetHDRTextureFormat();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006158 File Offset: 0x00004358
		public static int GetDepthSliceForEye(StereoTargetEyeMask mask)
		{
			switch (mask)
			{
			case StereoTargetEyeMask.None:
			case StereoTargetEyeMask.Left:
				return 0;
			case StereoTargetEyeMask.Right:
				return 1;
			case StereoTargetEyeMask.Both:
				return -1;
			default:
				throw new ArgumentException("Unknown mode");
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006182 File Offset: 0x00004382
		public static RenderTargetIdentifier ComposeTarget(OutlineParameters parameters, RenderTargetIdentifier target)
		{
			return new RenderTargetIdentifier(target, 0, CubemapFace.Unknown, RenderTargetUtility.GetDepthSliceForEye(parameters.EyeMask));
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006198 File Offset: 0x00004398
		public static RenderTargetUtility.RenderTextureInfo GetTargetInfo(OutlineParameters parameters, int width, int height)
		{
			RenderTextureFormat rtformat = RenderTargetUtility.GetRTFormat(parameters.UseHDR);
			if (XRUtility.IsUsingVR(parameters))
			{
				RenderTextureDescriptor eyeTextureDesc = XRSettings.eyeTextureDesc;
				eyeTextureDesc.colorFormat = rtformat;
				eyeTextureDesc.width = width;
				eyeTextureDesc.height = height;
				eyeTextureDesc.depthBufferBits = 0;
				eyeTextureDesc.msaaSamples = Mathf.Max(parameters.Antialiasing, 1);
				VRTextureUsage vrUsage = (parameters.EyeMask == StereoTargetEyeMask.Both) ? VRTextureUsage.TwoEyes : VRTextureUsage.OneEye;
				eyeTextureDesc.vrUsage = vrUsage;
				return new RenderTargetUtility.RenderTextureInfo(eyeTextureDesc);
			}
			return new RenderTargetUtility.RenderTextureInfo(new RenderTextureDescriptor(width, height, rtformat, 0)
			{
				dimension = TextureDimension.Tex2D,
				msaaSamples = Mathf.Max(parameters.Antialiasing, 1)
			});
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000623C File Offset: 0x0000443C
		public static RTHandle GetRT(OutlineParameters parameters, int width, int height, string name)
		{
			RenderTargetUtility.RenderTextureInfo targetInfo = RenderTargetUtility.GetTargetInfo(parameters, width, height);
			return OutlineEffect.HandleSystem.Alloc(width, height, targetInfo.Descriptor.volumeDepth, DepthBits.None, targetInfo.Descriptor.graphicsFormat, FilterMode.Bilinear, TextureWrapMode.Clamp, targetInfo.Descriptor.dimension, targetInfo.Descriptor.enableRandomWrite, targetInfo.Descriptor.useMipMap, targetInfo.Descriptor.autoGenerateMips, false, 1, 0f, (MSAASamples)targetInfo.Descriptor.msaaSamples, targetInfo.Descriptor.bindMS, targetInfo.Descriptor.useDynamicScale, targetInfo.Descriptor.memoryless, targetInfo.Descriptor.vrUsage, name);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000062FC File Offset: 0x000044FC
		public static RenderTextureFormat GetHDRTextureFormat()
		{
			if (RenderTargetUtility.hdrFormat != null)
			{
				return RenderTargetUtility.hdrFormat.Value;
			}
			if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
			{
				RenderTargetUtility.hdrFormat = new RenderTextureFormat?(RenderTextureFormat.ARGBHalf);
			}
			else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
			{
				RenderTargetUtility.hdrFormat = new RenderTextureFormat?(RenderTextureFormat.ARGBFloat);
			}
			else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB64))
			{
				RenderTargetUtility.hdrFormat = new RenderTextureFormat?(RenderTextureFormat.ARGB64);
			}
			else
			{
				RenderTargetUtility.hdrFormat = new RenderTextureFormat?(RenderTextureFormat.ARGB32);
			}
			return RenderTargetUtility.hdrFormat.Value;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006378 File Offset: 0x00004578
		public static GraphicsFormat GetHDRGraphicsFormat()
		{
			return GraphicsFormatUtility.GetGraphicsFormat(RenderTargetUtility.GetHDRTextureFormat(), RenderTextureReadWrite.Default);
		}

		// Token: 0x040000C0 RID: 192
		private static RenderTextureFormat? hdrFormat;

		// Token: 0x02000031 RID: 49
		public struct RenderTextureInfo
		{
			// Token: 0x0600012B RID: 299 RVA: 0x00007193 File Offset: 0x00005393
			public RenderTextureInfo(RenderTextureDescriptor descriptor)
			{
				this.Descriptor = descriptor;
			}

			// Token: 0x0400010C RID: 268
			public readonly RenderTextureDescriptor Descriptor;
		}
	}
}
