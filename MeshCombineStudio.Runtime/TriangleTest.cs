using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public struct TriangleTest
{
	// Token: 0x06000009 RID: 9 RVA: 0x0000252C File Offset: 0x0000072C
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

	// Token: 0x0600000A RID: 10 RVA: 0x00002720 File Offset: 0x00000920
	private void Swap<T>(ref T v1, ref T v2)
	{
		T t = v1;
		v1 = v2;
		v2 = t;
	}

	// Token: 0x0400000B RID: 11
	public Vector3 a;

	// Token: 0x0400000C RID: 12
	public Vector3 b;

	// Token: 0x0400000D RID: 13
	public Vector3 c;

	// Token: 0x0400000E RID: 14
	public Vector3 dirAb;

	// Token: 0x0400000F RID: 15
	public Vector3 dirAc;

	// Token: 0x04000010 RID: 16
	public Vector3 dirBc;

	// Token: 0x04000011 RID: 17
	public Vector3 h1;

	// Token: 0x04000012 RID: 18
	public float ab;

	// Token: 0x04000013 RID: 19
	public float ac;

	// Token: 0x04000014 RID: 20
	public float bc;

	// Token: 0x04000015 RID: 21
	public float area;

	// Token: 0x04000016 RID: 22
	public float h;

	// Token: 0x04000017 RID: 23
	public float ah;

	// Token: 0x04000018 RID: 24
	public float hb;
}
