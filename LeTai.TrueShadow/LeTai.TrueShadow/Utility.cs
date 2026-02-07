using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LeTai
{
	// Token: 0x02000006 RID: 6
	public static class Utility
	{
		// Token: 0x0600001F RID: 31 RVA: 0x0000257C File Offset: 0x0000077C
		public static void LogList<T>(IEnumerable<T> list, Func<T, object> getData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (T arg in list)
			{
				stringBuilder.Append(num.ToString() + ":    ");
				stringBuilder.Append(getData(arg).ToString());
				stringBuilder.Append("\n");
				num++;
			}
			Debug.Log(stringBuilder.ToString());
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000260C File Offset: 0x0000080C
		public static int SimplePingPong(int t, int max)
		{
			if (t > max)
			{
				return 2 * max - t;
			}
			return t;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000261C File Offset: 0x0000081C
		public static void SafeDestroy(UnityEngine.Object obj)
		{
			if (obj != null)
			{
				if (Application.isPlaying)
				{
					GameObject gameObject = obj as GameObject;
					if (gameObject != null)
					{
						gameObject.transform.parent = null;
					}
					UnityEngine.Object.Destroy(obj);
					return;
				}
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}
}
