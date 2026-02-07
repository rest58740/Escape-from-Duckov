using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000016 RID: 22
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct LocalMinima
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000057B5 File Offset: 0x000039B5
		public LocalMinima(Vertex vertex, PathType polytype, bool isOpen = false)
		{
			this.vertex = vertex;
			this.polytype = polytype;
			this.isOpen = isOpen;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000057CC File Offset: 0x000039CC
		public static bool operator ==(LocalMinima lm1, LocalMinima lm2)
		{
			return lm1.vertex == lm2.vertex;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000057DC File Offset: 0x000039DC
		public static bool operator !=(LocalMinima lm1, LocalMinima lm2)
		{
			return !(lm1 == lm2);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000057E8 File Offset: 0x000039E8
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is LocalMinima)
			{
				LocalMinima lm = (LocalMinima)obj;
				return this == lm;
			}
			return false;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005812 File Offset: 0x00003A12
		public override int GetHashCode()
		{
			return this.vertex.GetHashCode();
		}

		// Token: 0x0400003A RID: 58
		public readonly Vertex vertex;

		// Token: 0x0400003B RID: 59
		public readonly PathType polytype;

		// Token: 0x0400003C RID: 60
		public readonly bool isOpen;
	}
}
