using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LeTai.Asset.TranslucentImage.UniversalRP
{
	// Token: 0x02000005 RID: 5
	internal class URPRendererInternal
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000227A File Offset: 0x0000047A
		public void CacheRenderer(ScriptableRenderer renderer)
		{
			if (this.renderer == renderer)
			{
				return;
			}
			this.renderer = renderer;
			this.<CacheRenderer>g__CacheBackBufferGetter|3_0(renderer);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002294 File Offset: 0x00000494
		public RenderTargetIdentifier GetBackBuffer()
		{
			return this.getBackBufferDelegate().nameID;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022A6 File Offset: 0x000004A6
		public RenderTargetIdentifier GetAfterPostColor()
		{
			return this.getAfterPostColorDelegate().nameID;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022C0 File Offset: 0x000004C0
		[CompilerGenerated]
		private void <CacheRenderer>g__CacheBackBufferGetter|3_0(object rd)
		{
			object value = rd.GetType().GetField("m_ColorBufferSystem", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(rd);
			MethodInfo methodInfo = value.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).First((MethodInfo m) => m.Name == "PeekBackBuffer" && m.GetParameters().Length == 0);
			this.getBackBufferDelegate = (Func<RTHandle>)methodInfo.CreateDelegate(typeof(Func<RTHandle>), value);
		}

		// Token: 0x04000009 RID: 9
		private ScriptableRenderer renderer;

		// Token: 0x0400000A RID: 10
		private Func<RTHandle> getBackBufferDelegate;

		// Token: 0x0400000B RID: 11
		private Func<RTHandle> getAfterPostColorDelegate;
	}
}
