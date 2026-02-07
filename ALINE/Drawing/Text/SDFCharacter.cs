using System;
using Unity.Mathematics;

namespace Drawing.Text
{
	// Token: 0x0200005A RID: 90
	internal struct SDFCharacter
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0001124B File Offset: 0x0000F44B
		public float2 uvTopLeft
		{
			get
			{
				return this.uvtopleft;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00011253 File Offset: 0x0000F453
		public float2 uvTopRight
		{
			get
			{
				return new float2(this.uvbottomright.x, this.uvtopleft.y);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00011270 File Offset: 0x0000F470
		public float2 uvBottomLeft
		{
			get
			{
				return new float2(this.uvtopleft.x, this.uvbottomright.y);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0001128D File Offset: 0x0000F48D
		public float2 uvBottomRight
		{
			get
			{
				return this.uvbottomright;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00011295 File Offset: 0x0000F495
		public float2 vertexTopLeft
		{
			get
			{
				return this.vtopleft;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001129D File Offset: 0x0000F49D
		public float2 vertexTopRight
		{
			get
			{
				return new float2(this.vbottomright.x, this.vtopleft.y);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000112BA File Offset: 0x0000F4BA
		public float2 vertexBottomLeft
		{
			get
			{
				return new float2(this.vtopleft.x, this.vbottomright.y);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x000112D7 File Offset: 0x0000F4D7
		public float2 vertexBottomRight
		{
			get
			{
				return this.vbottomright;
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000112E0 File Offset: 0x0000F4E0
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

		// Token: 0x0400016C RID: 364
		public char codePoint;

		// Token: 0x0400016D RID: 365
		private float2 uvtopleft;

		// Token: 0x0400016E RID: 366
		private float2 uvbottomright;

		// Token: 0x0400016F RID: 367
		private float2 vtopleft;

		// Token: 0x04000170 RID: 368
		private float2 vbottomright;

		// Token: 0x04000171 RID: 369
		public float advance;
	}
}
