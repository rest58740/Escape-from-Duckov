using System;

namespace Pathfinding
{
	// Token: 0x0200008C RID: 140
	[Serializable]
	public struct PathfindingTag
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x0001720D File Offset: 0x0001540D
		public PathfindingTag(uint value)
		{
			this.value = value;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00017216 File Offset: 0x00015416
		public static implicit operator uint(PathfindingTag tag)
		{
			return tag.value;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001721E File Offset: 0x0001541E
		public static implicit operator PathfindingTag(uint tag)
		{
			return new PathfindingTag(tag);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00017228 File Offset: 0x00015428
		public static PathfindingTag FromName(string tagName)
		{
			AstarPath.FindAstarPath();
			if (AstarPath.active == null)
			{
				throw new InvalidOperationException("There's no AstarPath component in the scene. Cannot get tag names.");
			}
			int num = Array.IndexOf<string>(AstarPath.active.GetTagNames(), tagName);
			if (num == -1)
			{
				throw new ArgumentException("There's no pathfinding tag with the name '" + tagName + "'");
			}
			return new PathfindingTag((uint)num);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00017281 File Offset: 0x00015481
		public override string ToString()
		{
			return this.value.ToString();
		}

		// Token: 0x040002FE RID: 766
		public uint value;
	}
}
