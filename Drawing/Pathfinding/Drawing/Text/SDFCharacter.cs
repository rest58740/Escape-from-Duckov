using System;
using Unity.Mathematics;

namespace Pathfinding.Drawing.Text
{
	// Token: 0x0200005C RID: 92
	internal struct SDFCharacter
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000F647 File Offset: 0x0000D847
		public float2 uvTopLeft
		{
			get
			{
				return this.uvtopleft;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000F64F File Offset: 0x0000D84F
		public float2 uvTopRight
		{
			get
			{
				return new float2(this.uvbottomright.x, this.uvtopleft.y);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000F66C File Offset: 0x0000D86C
		public float2 uvBottomLeft
		{
			get
			{
				return new float2(this.uvtopleft.x, this.uvbottomright.y);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000F689 File Offset: 0x0000D889
		public float2 uvBottomRight
		{
			get
			{
				return this.uvbottomright;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000F691 File Offset: 0x0000D891
		public float2 vertexTopLeft
		{
			get
			{
				return this.vtopleft;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000F699 File Offset: 0x0000D899
		public float2 vertexTopRight
		{
			get
			{
				return new float2(this.vbottomright.x, this.vtopleft.y);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000F6B6 File Offset: 0x0000D8B6
		public float2 vertexBottomLeft
		{
			get
			{
				return new float2(this.vtopleft.x, this.vbottomright.y);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000F6D3 File Offset: 0x0000D8D3
		public float2 vertexBottomRight
		{
			get
			{
				return this.vbottomright;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000F6DC File Offset: 0x0000D8DC
		public SDFCharacter(char codePoint, int x, int y, int width, int height, int originX, int originY, int advance, int textureWidth, int textureHeight, float defaultSize)
		{
			float2 rhs = new float2((float)textureWidth, (float)textureHeight);
			this.codePoint = codePoint;
			float2 @float = new float2((float)x, (float)y) / rhs;
			float2 float2 = new float2((float)(x + width), (float)(y + height)) / rhs;
			this.uvtopleft = new float2(@float.x, 1f - @float.y);
			this.uvbottomright = new float2(float2.x, 1f - float2.y);
			float2 lhs = new float2((float)(-(float)originX), (float)originY);
			this.vtopleft = (lhs + new float2(0f, 0f)) / defaultSize;
			this.vbottomright = (lhs + new float2((float)width, (float)(-(float)height))) / defaultSize;
			this.advance = (float)advance / defaultSize;
		}

		// Token: 0x04000177 RID: 375
		public char codePoint;

		// Token: 0x04000178 RID: 376
		private float2 uvtopleft;

		// Token: 0x04000179 RID: 377
		private float2 uvbottomright;

		// Token: 0x0400017A RID: 378
		private float2 vtopleft;

		// Token: 0x0400017B RID: 379
		private float2 vbottomright;

		// Token: 0x0400017C RID: 380
		public float advance;
	}
}
