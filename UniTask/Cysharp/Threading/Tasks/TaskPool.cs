using System;
using System.Collections.Generic;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000031 RID: 49
	public static class TaskPool
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x00003C3C File Offset: 0x00001E3C
		static TaskPool()
		{
			try
			{
				string environmentVariable = Environment.GetEnvironmentVariable("UNITASK_MAX_POOLSIZE");
				int maxPoolSize;
				if (environmentVariable != null && int.TryParse(environmentVariable, out maxPoolSize))
				{
					TaskPool.MaxPoolSize = maxPoolSize;
					return;
				}
			}
			catch
			{
			}
			TaskPool.MaxPoolSize = int.MaxValue;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003C94 File Offset: 0x00001E94
		public static void SetMaxPoolSize(int maxPoolSize)
		{
			TaskPool.MaxPoolSize = maxPoolSize;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003C9C File Offset: 0x00001E9C
		public static IEnumerable<ValueTuple<Type, int>> GetCacheSizeInfo()
		{
			Dictionary<Type, Func<int>> obj = TaskPool.sizes;
			lock (obj)
			{
				foreach (KeyValuePair<Type, Func<int>> keyValuePair in TaskPool.sizes)
				{
					yield return new ValueTuple<Type, int>(keyValuePair.Key, keyValuePair.Value());
				}
				Dictionary<Type, Func<int>>.Enumerator enumerator = default(Dictionary<Type, Func<int>>.Enumerator);
			}
			obj = null;
			yield break;
			yield break;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003CA8 File Offset: 0x00001EA8
		public static void RegisterSizeGetter(Type type, Func<int> getSize)
		{
			Dictionary<Type, Func<int>> obj = TaskPool.sizes;
			lock (obj)
			{
				TaskPool.sizes[type] = getSize;
			}
		}

		// Token: 0x0400006B RID: 107
		internal static int MaxPoolSize;

		// Token: 0x0400006C RID: 108
		private static Dictionary<Type, Func<int>> sizes = new Dictionary<Type, Func<int>>();
	}
}
