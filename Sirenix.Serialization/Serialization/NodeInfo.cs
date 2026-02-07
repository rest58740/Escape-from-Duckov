using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200006C RID: 108
	public struct NodeInfo
	{
		// Token: 0x0600037E RID: 894 RVA: 0x000186B2 File Offset: 0x000168B2
		public NodeInfo(string name, int id, Type type, bool isArray)
		{
			this.Name = name;
			this.Id = id;
			this.Type = type;
			this.IsArray = isArray;
			this.IsEmpty = false;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000186D8 File Offset: 0x000168D8
		private NodeInfo(bool parameter)
		{
			this.Name = null;
			this.Id = -1;
			this.Type = null;
			this.IsArray = false;
			this.IsEmpty = true;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00018700 File Offset: 0x00016900
		public static bool operator ==(NodeInfo a, NodeInfo b)
		{
			return a.Name == b.Name && a.Id == b.Id && a.Type == b.Type && a.IsArray == b.IsArray && a.IsEmpty == b.IsEmpty;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001875F File Offset: 0x0001695F
		public static bool operator !=(NodeInfo a, NodeInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001876B File Offset: 0x0001696B
		public override bool Equals(object obj)
		{
			return obj != null && obj is NodeInfo && (NodeInfo)obj == this;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00018790 File Offset: 0x00016990
		public override int GetHashCode()
		{
			if (this.IsEmpty)
			{
				return 0;
			}
			return -2128831035 ^ ((this.Name == null) ? 12321 : this.Name.GetHashCode()) * 16777619 ^ this.Id * 16777619 ^ ((this.Type == null) ? 1423 : this.Type.GetHashCode()) * 16777619 ^ (this.IsArray ? 124124 : 43234) * 16777619 ^ (this.IsEmpty ? 872934 : 27323) * 16777619;
		}

		// Token: 0x04000136 RID: 310
		public static readonly NodeInfo Empty = new NodeInfo(true);

		// Token: 0x04000137 RID: 311
		public readonly string Name;

		// Token: 0x04000138 RID: 312
		public readonly int Id;

		// Token: 0x04000139 RID: 313
		public readonly Type Type;

		// Token: 0x0400013A RID: 314
		public readonly bool IsArray;

		// Token: 0x0400013B RID: 315
		public readonly bool IsEmpty;
	}
}
