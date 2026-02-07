using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x0200005F RID: 95
	public class CinemachineDebug
	{
		// Token: 0x060003BC RID: 956 RVA: 0x00016F25 File Offset: 0x00015125
		public static void ReleaseScreenPos(UnityEngine.Object client)
		{
			if (CinemachineDebug.mClients != null && CinemachineDebug.mClients.Contains(client))
			{
				CinemachineDebug.mClients.Remove(client);
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00016F48 File Offset: 0x00015148
		public static Rect GetScreenPos(UnityEngine.Object client, string text, GUIStyle style)
		{
			if (CinemachineDebug.mClients == null)
			{
				CinemachineDebug.mClients = new HashSet<UnityEngine.Object>();
			}
			if (!CinemachineDebug.mClients.Contains(client))
			{
				CinemachineDebug.mClients.Add(client);
			}
			Vector2 zero = Vector2.zero;
			Vector2 vector = style.CalcSize(new GUIContent(text));
			if (CinemachineDebug.mClients != null)
			{
				using (HashSet<UnityEngine.Object>.Enumerator enumerator = CinemachineDebug.mClients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == client)
						{
							break;
						}
						zero.y += vector.y;
					}
				}
			}
			return new Rect(zero, vector);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00016FF8 File Offset: 0x000151F8
		public static StringBuilder SBFromPool()
		{
			if (CinemachineDebug.mAvailableStringBuilders == null || CinemachineDebug.mAvailableStringBuilders.Count == 0)
			{
				return new StringBuilder();
			}
			StringBuilder stringBuilder = CinemachineDebug.mAvailableStringBuilders[CinemachineDebug.mAvailableStringBuilders.Count - 1];
			CinemachineDebug.mAvailableStringBuilders.RemoveAt(CinemachineDebug.mAvailableStringBuilders.Count - 1);
			stringBuilder.Length = 0;
			return stringBuilder;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00017051 File Offset: 0x00015251
		public static void ReturnToPool(StringBuilder sb)
		{
			if (CinemachineDebug.mAvailableStringBuilders == null)
			{
				CinemachineDebug.mAvailableStringBuilders = new List<StringBuilder>();
			}
			CinemachineDebug.mAvailableStringBuilders.Add(sb);
		}

		// Token: 0x0400028B RID: 651
		private static HashSet<UnityEngine.Object> mClients;

		// Token: 0x0400028C RID: 652
		public static CinemachineDebug.OnGUIDelegate OnGUIHandlers;

		// Token: 0x0400028D RID: 653
		private static List<StringBuilder> mAvailableStringBuilders;

		// Token: 0x020000E8 RID: 232
		// (Invoke) Token: 0x06000576 RID: 1398
		public delegate void OnGUIDelegate();
	}
}
