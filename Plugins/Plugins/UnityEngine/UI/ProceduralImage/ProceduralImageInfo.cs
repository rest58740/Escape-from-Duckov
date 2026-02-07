using System;

namespace UnityEngine.UI.ProceduralImage
{
	// Token: 0x02000010 RID: 16
	public struct ProceduralImageInfo
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003204 File Offset: 0x00001404
		public ProceduralImageInfo(float width, float height, float fallOffDistance, float pixelSize, Vector4 radius, float borderWidth)
		{
			this.width = Mathf.Abs(width);
			this.height = Mathf.Abs(height);
			this.fallOffDistance = Mathf.Max(0f, fallOffDistance);
			this.radius = radius;
			this.borderWidth = Mathf.Max(borderWidth, 0f);
			this.pixelSize = Mathf.Max(0f, pixelSize);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003268 File Offset: 0x00001468
		public override int GetHashCode()
		{
			string text = "" + this.width.ToString() + this.height.ToString() + this.fallOffDistance.ToString();
			Vector4 vector = this.radius;
			return (text + vector.ToString() + this.borderWidth.ToString() + this.pixelSize.ToString()).GetHashCode();
		}

		// Token: 0x0400001B RID: 27
		public float width;

		// Token: 0x0400001C RID: 28
		public float height;

		// Token: 0x0400001D RID: 29
		public float fallOffDistance;

		// Token: 0x0400001E RID: 30
		public Vector4 radius;

		// Token: 0x0400001F RID: 31
		public float borderWidth;

		// Token: 0x04000020 RID: 32
		public float pixelSize;
	}
}
