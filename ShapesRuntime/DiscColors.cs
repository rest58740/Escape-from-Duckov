using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000016 RID: 22
	public struct DiscColors
	{
		// Token: 0x06000286 RID: 646 RVA: 0x00007546 File Offset: 0x00005746
		internal DiscColors(Color innerStart, Color outerStart, Color innerEnd, Color outerEnd)
		{
			this.innerStart = innerStart;
			this.outerStart = outerStart;
			this.innerEnd = innerEnd;
			this.outerEnd = outerEnd;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00007565 File Offset: 0x00005765
		public static DiscColors Flat(Color color)
		{
			return new DiscColors(color, color, color, color);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00007570 File Offset: 0x00005770
		public static DiscColors Radial(Color inner, Color outer)
		{
			return new DiscColors(inner, outer, inner, outer);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000757B File Offset: 0x0000577B
		public static DiscColors Angular(Color start, Color end)
		{
			return new DiscColors(start, start, end, end);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00007586 File Offset: 0x00005786
		public static DiscColors Bilinear(Color innerStart, Color outerStart, Color innerEnd, Color outerEnd)
		{
			return new DiscColors(innerStart, outerStart, innerEnd, outerEnd);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00007591 File Offset: 0x00005791
		public static implicit operator DiscColors(Color flatColor)
		{
			return DiscColors.Flat(flatColor);
		}

		// Token: 0x04000098 RID: 152
		public Color innerStart;

		// Token: 0x04000099 RID: 153
		public Color outerStart;

		// Token: 0x0400009A RID: 154
		public Color innerEnd;

		// Token: 0x0400009B RID: 155
		public Color outerEnd;
	}
}
