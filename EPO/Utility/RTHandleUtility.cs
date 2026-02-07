using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline.Utility
{
	// Token: 0x02000025 RID: 37
	public static class RTHandleUtility
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00006D63 File Offset: 0x00004F63
		internal static void RemoveDelegates(RTHandle handle)
		{
			RTHandleUtility.setTextureDelegates.Remove(handle);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006D74 File Offset: 0x00004F74
		public static void SetTexture(this RTHandle handle, Texture texture)
		{
			if (RTHandleUtility.setTextureInfo == null)
			{
				RTHandleUtility.setTextureInfo = typeof(RTHandle).GetMethod("SetTexture", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(Texture)
				}, null);
			}
			RTHandleUtility.parameter[0] = texture;
			RTHandleUtility.setTextureInfo.Invoke(handle, RTHandleUtility.parameter);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006DD8 File Offset: 0x00004FD8
		public static void SetRenderTargetIdentifier(this RTHandle handle, RenderTargetIdentifier identifier)
		{
			Action<RenderTargetIdentifier> action;
			if (!RTHandleUtility.setTextureDelegates.TryGetValue(handle, out action))
			{
				action = (Action<RenderTargetIdentifier>)typeof(RTHandle).GetMethod("SetTexture", BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Standard, new Type[]
				{
					typeof(RenderTargetIdentifier)
				}, null).CreateDelegate(typeof(Action<RenderTargetIdentifier>), handle);
				RTHandleUtility.setTextureDelegates.Add(handle, action);
			}
			action(identifier);
		}

		// Token: 0x040000CC RID: 204
		private static MethodInfo setTextureInfo;

		// Token: 0x040000CD RID: 205
		private static object[] parameter = new object[1];

		// Token: 0x040000CE RID: 206
		private static Dictionary<RTHandle, Action<RenderTargetIdentifier>> setTextureDelegates = new Dictionary<RTHandle, Action<RenderTargetIdentifier>>();
	}
}
