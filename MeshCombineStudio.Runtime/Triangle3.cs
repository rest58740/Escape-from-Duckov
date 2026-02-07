using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000036 RID: 54
	public struct Triangle3
	{
		// Token: 0x06000126 RID: 294 RVA: 0x0000B160 File Offset: 0x00009360
		public void Calc()
		{
			Vector3 vector = this.a;
			Vector3 vector2 = this.b;
			Vector3 vector3 = this.c;
			Vector3 vector4 = this.b - this.a;
			Vector3 vector5 = this.c - this.a;
			Vector3 vector6 = this.c - this.b;
			float magnitude = vector4.magnitude;
			float magnitude2 = vector5.magnitude;
			float magnitude3 = vector6.magnitude;
			if (magnitude2 > magnitude && magnitude2 > magnitude3)
			{
				this.a = vector;
				this.b = vector3;
				this.c = vector2;
			}
			else if (magnitude3 > magnitude)
			{
				this.a = vector3;
				this.b = vector2;
				this.c = vector;
			}
			this.dirAb = this.b - this.a;
			this.dirAc = this.c - this.a;
			this.dirBc = this.c - this.b;
			this.ab = this.dirAb.magnitude;
			this.ac = this.dirAc.magnitude;
			this.bc = this.dirBc.magnitude;
			float num = (this.ab + this.ac + this.bc) * 0.5f;
			this.area = Mathf.Sqrt(num * (num - this.ab) * (num - this.ac) * (num - this.bc));
			this.h = 2f * this.area / this.ab;
			this.ah = Mathf.Sqrt(this.ac * this.ac - this.h * this.h);
			this.hb = this.ab - this.ah;
			this.h1 = this.a + this.dirAb * (1f / this.ab * this.ah);
		}

		// Token: 0x04000141 RID: 321
		public Vector3 a;

		// Token: 0x04000142 RID: 322
		public Vector3 b;

		// Token: 0x04000143 RID: 323
		public Vector3 c;

		// Token: 0x04000144 RID: 324
		public Vector3 dirAb;

		// Token: 0x04000145 RID: 325
		public Vector3 dirAc;

		// Token: 0x04000146 RID: 326
		public Vector3 dirBc;

		// Token: 0x04000147 RID: 327
		public Vector3 h1;

		// Token: 0x04000148 RID: 328
		public float ab;

		// Token: 0x04000149 RID: 329
		public float ac;

		// Token: 0x0400014A RID: 330
		public float bc;

		// Token: 0x0400014B RID: 331
		public float area;

		// Token: 0x0400014C RID: 332
		public float h;

		// Token: 0x0400014D RID: 333
		public float ah;

		// Token: 0x0400014E RID: 334
		public float hb;
	}
}
