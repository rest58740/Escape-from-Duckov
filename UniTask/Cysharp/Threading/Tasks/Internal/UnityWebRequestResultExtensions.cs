using System;
using UnityEngine.Networking;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000118 RID: 280
	internal static class UnityWebRequestResultExtensions
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x0000ED74 File Offset: 0x0000CF74
		public static bool IsError(this UnityWebRequest unityWebRequest)
		{
			UnityWebRequest.Result result = unityWebRequest.result;
			return result == 2 || result == 4 || result == 3;
		}
	}
}
