using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x02000249 RID: 585
	public static class PathPool
	{
		// Token: 0x06000DC7 RID: 3527 RVA: 0x00056C8C File Offset: 0x00054E8C
		public static void Pool(Path path)
		{
			Dictionary<Type, Stack<Path>> obj = PathPool.pool;
			lock (obj)
			{
				if (((IPathInternals)path).Pooled)
				{
					throw new ArgumentException("The path is already pooled.");
				}
				Stack<Path> stack;
				if (!PathPool.pool.TryGetValue(path.GetType(), out stack))
				{
					stack = new Stack<Path>();
					PathPool.pool[path.GetType()] = stack;
				}
				((IPathInternals)path).Pooled = true;
				((IPathInternals)path).OnEnterPool();
				stack.Push(path);
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00056D18 File Offset: 0x00054F18
		public static int GetTotalCreated(Type type)
		{
			int result;
			if (PathPool.totalCreated.TryGetValue(type, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00056D38 File Offset: 0x00054F38
		public static int GetSize(Type type)
		{
			Stack<Path> stack;
			if (PathPool.pool.TryGetValue(type, out stack))
			{
				return stack.Count;
			}
			return 0;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00056D5C File Offset: 0x00054F5C
		public static T GetPath<T>() where T : Path, new()
		{
			Dictionary<Type, Stack<Path>> obj = PathPool.pool;
			T result;
			lock (obj)
			{
				Stack<Path> stack;
				T t;
				if (PathPool.pool.TryGetValue(typeof(T), out stack) && stack.Count > 0)
				{
					t = (stack.Pop() as T);
				}
				else
				{
					t = Activator.CreateInstance<T>();
					if (!PathPool.totalCreated.ContainsKey(typeof(T)))
					{
						PathPool.totalCreated[typeof(T)] = 0;
					}
					Dictionary<Type, int> dictionary = PathPool.totalCreated;
					Type typeFromHandle = typeof(T);
					int num = dictionary[typeFromHandle];
					dictionary[typeFromHandle] = num + 1;
				}
				t.Pooled = false;
				t.Reset();
				result = t;
			}
			return result;
		}

		// Token: 0x04000A94 RID: 2708
		private static readonly Dictionary<Type, Stack<Path>> pool = new Dictionary<Type, Stack<Path>>();

		// Token: 0x04000A95 RID: 2709
		private static readonly Dictionary<Type, int> totalCreated = new Dictionary<Type, int>();
	}
}
