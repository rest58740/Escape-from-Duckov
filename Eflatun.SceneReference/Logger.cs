using System;
using UnityEngine;

namespace Eflatun.SceneReference
{
	// Token: 0x02000005 RID: 5
	internal static class Logger
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		internal static void Debug(string msg)
		{
			UnityEngine.Debug.Log("[Eflatun.SceneReference] " + msg);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020D2 File Offset: 0x000002D2
		internal static void Warn(string msg)
		{
			UnityEngine.Debug.LogWarning("[Eflatun.SceneReference] " + msg);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020E4 File Offset: 0x000002E4
		internal static void Error(string msg)
		{
			UnityEngine.Debug.LogError("[Eflatun.SceneReference] " + msg);
		}
	}
}
