using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000044 RID: 68
	internal static class TexturePlacement
	{
		// Token: 0x06000C31 RID: 3121 RVA: 0x000187BA File Offset: 0x000169BA
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		internal static ValueTuple<Rect, Rect> Fit(Texture texture, Rect rect, TextureFillMode mode)
		{
			switch (mode)
			{
			case TextureFillMode.StretchToFill:
				return TexturePlacement.StretchToFill(rect);
			case TextureFillMode.ScaleToFit:
				return TexturePlacement.ScaleToFit(texture, rect);
			case TextureFillMode.ScaleAndCropToFill:
				return TexturePlacement.ScaleAndCropToFill(texture, rect);
			default:
				throw new ArgumentOutOfRangeException("mode", mode, null);
			}
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000187F8 File Offset: 0x000169F8
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		internal static ValueTuple<Rect, Rect> Size(Texture texture, Vector2 c, float size, TextureSizeMode mode)
		{
			float num = (float)texture.width / (float)texture.height;
			switch (mode)
			{
			case TextureSizeMode.Width:
				return TexturePlacement.FitWidth(c, size, num);
			case TextureSizeMode.Height:
				return TexturePlacement.FitHeight(c, size, num);
			case TextureSizeMode.LongestSide:
				if (num >= 1f)
				{
					return TexturePlacement.FitWidth(c, size, num);
				}
				return TexturePlacement.FitHeight(c, size, num);
			case TextureSizeMode.ShortestSide:
				if (num >= 1f)
				{
					return TexturePlacement.FitHeight(c, size, num);
				}
				return TexturePlacement.FitWidth(c, size, num);
			case TextureSizeMode.PixelsPerMeter:
				return TexturePlacement.TexelSized(texture, c, size);
			case TextureSizeMode.Radius:
				return TexturePlacement.FitRadius(texture, c, size);
			default:
				throw new ArgumentOutOfRangeException("mode", mode, null);
			}
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x0001889E File Offset: 0x00016A9E
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		private static ValueTuple<Rect, Rect> FitWidth(Vector2 c, float w, float aspect)
		{
			return TexturePlacement.SimpleRect(c, w, w / aspect);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x000188AA File Offset: 0x00016AAA
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		private static ValueTuple<Rect, Rect> FitHeight(Vector2 c, float h, float aspect)
		{
			return TexturePlacement.SimpleRect(c, h * aspect, h);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000188B8 File Offset: 0x00016AB8
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		private static ValueTuple<Rect, Rect> FitRadius(Texture tex, Vector2 c, float r)
		{
			Vector2 vector = new Vector2((float)tex.width, (float)tex.height).normalized * (r * 2f);
			return TexturePlacement.SimpleRect(c, vector.x, vector.y);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000188FF File Offset: 0x00016AFF
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		private static ValueTuple<Rect, Rect> SimpleRect(Vector2 c, float w, float h)
		{
			return new ValueTuple<Rect, Rect>(TexturePlacement.RectCnt(c.x, c.y, w, h), TexturePlacement.fitUvs);
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x0001891E File Offset: 0x00016B1E
		private static Rect RectCnt(float cx, float cy, float w, float h)
		{
			return new Rect(cx - w / 2f, cy - h / 2f, w, h);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00018939 File Offset: 0x00016B39
		private static Rect RectCnt(Vector2 c, float w, float h)
		{
			return new Rect(c.x - w / 2f, c.y - h / 2f, w, h);
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0001895E File Offset: 0x00016B5E
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		private static ValueTuple<Rect, Rect> StretchToFill(Rect rect)
		{
			return new ValueTuple<Rect, Rect>(rect, TexturePlacement.fitUvs);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0001896C File Offset: 0x00016B6C
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		private static ValueTuple<Rect, Rect> ScaleToFit(Texture texture, Rect rect)
		{
			float a = rect.width / (float)texture.width;
			float b = rect.height / (float)texture.height;
			float num = Mathf.Min(a, b);
			return new ValueTuple<Rect, Rect>(TexturePlacement.RectCnt(rect.center, (float)texture.width * num, (float)texture.height * num), TexturePlacement.fitUvs);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000189C8 File Offset: 0x00016BC8
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		private static ValueTuple<Rect, Rect> ScaleAndCropToFill(Texture texture, Rect rect)
		{
			float num = rect.width / (float)texture.width;
			float num2 = rect.height / (float)texture.height;
			float num3 = Mathf.Max(num, num2);
			return new ValueTuple<Rect, Rect>(rect, TexturePlacement.RectCnt(0.5f, 0.5f, num / num3, num2 / num3));
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00018A18 File Offset: 0x00016C18
		[return: TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})]
		private static ValueTuple<Rect, Rect> TexelSized(Texture texture, Vector2 center, float pixelsPerMeter)
		{
			float w = (float)texture.width / pixelsPerMeter;
			float h = (float)texture.height / pixelsPerMeter;
			return TexturePlacement.SimpleRect(center, w, h);
		}

		// Token: 0x040001B7 RID: 439
		private static readonly Rect fitUvs = new Rect(0f, 0f, 1f, 1f);
	}
}
