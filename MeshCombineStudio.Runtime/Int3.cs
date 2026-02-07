using System;

namespace MeshCombineStudio
{
	// Token: 0x02000039 RID: 57
	public struct Int3
	{
		// Token: 0x06000129 RID: 297 RVA: 0x0000B373 File Offset: 0x00009573
		public Int3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000B38C File Offset: 0x0000958C
		public static Int3 operator +(Int3 a, Int3 b)
		{
			Int3 result;
			result.x = a.x + b.x;
			result.y = a.y + b.y;
			result.z = a.z + b.z;
			return result;
		}

		// Token: 0x04000153 RID: 339
		public int x;

		// Token: 0x04000154 RID: 340
		public int y;

		// Token: 0x04000155 RID: 341
		public int z;
	}
}
