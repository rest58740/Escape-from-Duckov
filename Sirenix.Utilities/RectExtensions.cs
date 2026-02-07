using System;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x0200000E RID: 14
	public static class RectExtensions
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000036EC File Offset: 0x000018EC
		public static Rect TakeFromDir(this Rect rect, float width, Direction direction)
		{
			switch (direction)
			{
			case Direction.Left:
				return ref rect.TakeFromLeft(width);
			case Direction.Right:
				return ref rect.TakeFromRight(width);
			case Direction.Top:
				return ref rect.TakeFromTop(width);
			case Direction.Bottom:
				return ref rect.TakeFromBottom(width);
			default:
				throw new NotImplementedException(direction.ToString());
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003744 File Offset: 0x00001944
		public static Rect TakeFromLeft(this Rect rect, float width)
		{
			float num = Math.Min(rect.width, width);
			Rect result = rect;
			result.width = num;
			rect.x += num;
			rect.width -= num;
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000378C File Offset: 0x0000198C
		public static Rect TakeFromRight(this Rect rect, float width)
		{
			float num = Math.Min(rect.width, width);
			Rect result = rect.AlignRight(num);
			rect.width -= num;
			return result;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000037C4 File Offset: 0x000019C4
		public static Rect TakeFromTop(this Rect rect, float height)
		{
			float num = Math.Min(rect.height, height);
			Rect result = rect;
			result.height = num;
			rect.y += num;
			rect.height -= num;
			return result;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000380C File Offset: 0x00001A0C
		public static Rect TakeFromBottom(this Rect rect, float height)
		{
			float height2 = Math.Min(rect.height, height);
			Rect result = rect.AlignBottom(height2);
			rect.height -= height;
			return result;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003842 File Offset: 0x00001A42
		public static Rect SetWidth(this Rect rect, float width)
		{
			rect.width = width;
			return rect;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000384D File Offset: 0x00001A4D
		public static Rect SetHeight(this Rect rect, float height)
		{
			rect.height = height;
			return rect;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003858 File Offset: 0x00001A58
		public static Rect SetSize(this Rect rect, float width, float height)
		{
			rect.width = width;
			rect.height = height;
			return rect;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000386B File Offset: 0x00001A6B
		public static Rect SetSize(this Rect rect, float widthAndHeight)
		{
			rect.width = widthAndHeight;
			rect.height = widthAndHeight;
			return rect;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000387E File Offset: 0x00001A7E
		public static Rect SetSize(this Rect rect, Vector2 size)
		{
			rect.size = size;
			return rect;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003889 File Offset: 0x00001A89
		public static Rect HorizontalPadding(this Rect rect, float padding)
		{
			rect.x += padding;
			rect.width -= padding * 2f;
			return rect;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000038B0 File Offset: 0x00001AB0
		public static Rect HorizontalPadding(this Rect rect, float left, float right)
		{
			rect.x += left;
			rect.width -= left + right;
			return rect;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000038D3 File Offset: 0x00001AD3
		public static Rect VerticalPadding(this Rect rect, float padding)
		{
			rect.y += padding;
			rect.height -= padding * 2f;
			return rect;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000038FA File Offset: 0x00001AFA
		public static Rect VerticalPadding(this Rect rect, float top, float bottom)
		{
			rect.y += top;
			rect.height -= top + bottom;
			return rect;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000391D File Offset: 0x00001B1D
		public static Rect Padding(this Rect rect, float padding)
		{
			rect.position += new Vector2(padding, padding);
			rect.size -= new Vector2(padding, padding) * 2f;
			return rect;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000395C File Offset: 0x00001B5C
		public static Rect Padding(this Rect rect, float horizontal, float vertical)
		{
			rect.position += new Vector2(horizontal, vertical);
			rect.size -= new Vector2(horizontal, vertical) * 2f;
			return rect;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000399B File Offset: 0x00001B9B
		public static Rect Padding(this Rect rect, float left, float right, float top, float bottom)
		{
			rect.position += new Vector2(left, top);
			rect.size -= new Vector2(left + right, top + bottom);
			return rect;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003842 File Offset: 0x00001A42
		public static Rect AlignLeft(this Rect rect, float width)
		{
			rect.width = width;
			return rect;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000039D5 File Offset: 0x00001BD5
		public static Rect AlignCenter(this Rect rect, float width)
		{
			rect.x = rect.x + rect.width * 0.5f - width * 0.5f;
			rect.width = width;
			return rect;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003A04 File Offset: 0x00001C04
		public static Rect AlignCenter(this Rect rect, float width, float height)
		{
			rect.x = rect.x + rect.width * 0.5f - width * 0.5f;
			rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
			rect.width = width;
			rect.height = height;
			return rect;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003A6A File Offset: 0x00001C6A
		public static Rect AlignRight(this Rect rect, float width)
		{
			rect.x = rect.x + rect.width - width;
			rect.width = width;
			return rect;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003A90 File Offset: 0x00001C90
		public static Rect AlignRight(this Rect rect, float width, bool clamp)
		{
			if (clamp)
			{
				rect.xMin = Mathf.Max(rect.xMax - width, rect.xMin);
				return rect;
			}
			rect.x = rect.x + rect.width - width;
			rect.width = width;
			return rect;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000384D File Offset: 0x00001A4D
		public static Rect AlignTop(this Rect rect, float height)
		{
			rect.height = height;
			return rect;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003ADF File Offset: 0x00001CDF
		public static Rect AlignMiddle(this Rect rect, float height)
		{
			rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
			rect.height = height;
			return rect;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003B0E File Offset: 0x00001D0E
		public static Rect AlignBottom(this Rect rect, float height)
		{
			rect.y = rect.y + rect.height - height;
			rect.height = height;
			return rect;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000039D5 File Offset: 0x00001BD5
		public static Rect AlignCenterX(this Rect rect, float width)
		{
			rect.x = rect.x + rect.width * 0.5f - width * 0.5f;
			rect.width = width;
			return rect;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003ADF File Offset: 0x00001CDF
		public static Rect AlignCenterY(this Rect rect, float height)
		{
			rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
			rect.height = height;
			return rect;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003B34 File Offset: 0x00001D34
		public static Rect AlignCenterXY(this Rect rect, float size)
		{
			rect.y = rect.y + rect.height * 0.5f - size * 0.5f;
			rect.x = rect.x + rect.width * 0.5f - size * 0.5f;
			rect.height = size;
			rect.width = size;
			return rect;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003B9C File Offset: 0x00001D9C
		public static Rect AlignCenterXY(this Rect rect, float width, float height)
		{
			rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
			rect.x = rect.x + rect.width * 0.5f - width * 0.5f;
			rect.width = width;
			rect.height = height;
			return rect;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003C04 File Offset: 0x00001E04
		public static Rect Expand(this Rect rect, float expand)
		{
			rect.x -= expand;
			rect.y -= expand;
			rect.height += expand * 2f;
			rect.width += expand * 2f;
			return rect;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003C5A File Offset: 0x00001E5A
		public static Rect Expand(this Rect rect, float horizontal, float vertical)
		{
			rect.position -= new Vector2(horizontal, vertical);
			rect.size += new Vector2(horizontal, vertical) * 2f;
			return rect;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003C99 File Offset: 0x00001E99
		public static Rect Expand(this Rect rect, float left, float right, float top, float bottom)
		{
			rect.position -= new Vector2(left, top);
			rect.size += new Vector2(left + right, top + bottom);
			return rect;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003CD4 File Offset: 0x00001ED4
		public static Rect Split(this Rect rect, int index, int count)
		{
			int num = (int)rect.width;
			int num2 = num / count;
			int num3 = num - num2 * count;
			float num4 = rect.x + (float)(num2 * index);
			if (index < num3)
			{
				num4 += (float)index;
				num2++;
			}
			else
			{
				num4 += (float)num3;
			}
			rect.x = num4;
			rect.width = (float)num2;
			return rect;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003D28 File Offset: 0x00001F28
		public static Rect SplitVertical(this Rect rect, int index, int count)
		{
			float num = rect.height / (float)count;
			rect.height = num;
			rect.y += num * (float)index;
			return rect;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003D5C File Offset: 0x00001F5C
		public static Rect SplitGrid(this Rect rect, float width, float height, int index)
		{
			int num = (int)(rect.width / width);
			num = ((num > 0) ? num : 1);
			int num2 = index % num;
			int num3 = index / num;
			rect.x += (float)num2 * width;
			rect.y += (float)num3 * height;
			rect.width = width;
			rect.height = height;
			return rect;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003DBC File Offset: 0x00001FBC
		public static Rect SplitTableGrid(this Rect rect, int columnCount, float rowHeight, int index)
		{
			int num = index % columnCount;
			int num2 = index / columnCount;
			float num3 = rect.width / (float)columnCount;
			rect.x += (float)num * num3;
			rect.y += (float)num2 * rowHeight;
			rect.width = num3;
			rect.height = rowHeight;
			return rect;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003E11 File Offset: 0x00002011
		public static Rect SetCenterX(this Rect rect, float x)
		{
			rect.center = new Vector2(x, rect.center.y);
			return rect;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003E2D File Offset: 0x0000202D
		public static Rect SetCenterY(this Rect rect, float y)
		{
			rect.center = new Vector2(rect.center.x, y);
			return rect;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003E49 File Offset: 0x00002049
		public static Rect SetCenter(this Rect rect, float x, float y)
		{
			rect.center = new Vector2(x, y);
			return rect;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003E5A File Offset: 0x0000205A
		public static Rect SetCenter(this Rect rect, Vector2 center)
		{
			rect.center = center;
			return rect;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003E65 File Offset: 0x00002065
		public static Rect SetPosition(this Rect rect, Vector2 position)
		{
			rect.position = position;
			return rect;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003E70 File Offset: 0x00002070
		public static Rect ResetPosition(this Rect rect)
		{
			rect.position = Vector2.zero;
			return rect;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003E7F File Offset: 0x0000207F
		public static Rect AddPosition(this Rect rect, Vector2 move)
		{
			rect.position += move;
			return rect;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003E95 File Offset: 0x00002095
		public static Rect AddPosition(this Rect rect, float x, float y)
		{
			rect.x += x;
			rect.y += y;
			return rect;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003EB6 File Offset: 0x000020B6
		public static Rect SetX(this Rect rect, float x)
		{
			rect.x = x;
			return rect;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003EC1 File Offset: 0x000020C1
		public static Rect AddX(this Rect rect, float x)
		{
			rect.x += x;
			return rect;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003ED3 File Offset: 0x000020D3
		public static Rect SubX(this Rect rect, float x)
		{
			rect.x -= x;
			return rect;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003EE5 File Offset: 0x000020E5
		public static Rect SetY(this Rect rect, float y)
		{
			rect.y = y;
			return rect;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003EF0 File Offset: 0x000020F0
		public static Rect AddY(this Rect rect, float y)
		{
			rect.y += y;
			return rect;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003F02 File Offset: 0x00002102
		public static Rect SubY(this Rect rect, float y)
		{
			rect.y -= y;
			return rect;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003F14 File Offset: 0x00002114
		public static Rect SetMin(this Rect rect, Vector2 min)
		{
			rect.min = min;
			return rect;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003F1F File Offset: 0x0000211F
		public static Rect AddMin(this Rect rect, Vector2 value)
		{
			rect.min += value;
			return rect;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003F35 File Offset: 0x00002135
		public static Rect SubMin(this Rect rect, Vector2 value)
		{
			rect.min -= value;
			return rect;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003F4B File Offset: 0x0000214B
		public static Rect SetMax(this Rect rect, Vector2 max)
		{
			rect.max = max;
			return rect;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003F56 File Offset: 0x00002156
		public static Rect AddMax(this Rect rect, Vector2 value)
		{
			rect.max += value;
			return rect;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003F6C File Offset: 0x0000216C
		public static Rect SubMax(this Rect rect, Vector2 value)
		{
			rect.max -= value;
			return rect;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003F82 File Offset: 0x00002182
		public static Rect SetXMin(this Rect rect, float xMin)
		{
			rect.xMin = xMin;
			return rect;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003F8D File Offset: 0x0000218D
		public static Rect AddXMin(this Rect rect, float value)
		{
			rect.xMin += value;
			return rect;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003F9F File Offset: 0x0000219F
		public static Rect SubXMin(this Rect rect, float value)
		{
			rect.xMin -= value;
			return rect;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003FB1 File Offset: 0x000021B1
		public static Rect SetXMax(this Rect rect, float xMax)
		{
			rect.xMax = xMax;
			return rect;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003FBC File Offset: 0x000021BC
		public static Rect AddXMax(this Rect rect, float value)
		{
			rect.xMax += value;
			return rect;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003FCE File Offset: 0x000021CE
		public static Rect SubXMax(this Rect rect, float value)
		{
			rect.xMax -= value;
			return rect;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003FE0 File Offset: 0x000021E0
		public static Rect SetYMin(this Rect rect, float yMin)
		{
			rect.yMin = yMin;
			return rect;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003FEB File Offset: 0x000021EB
		public static Rect AddYMin(this Rect rect, float value)
		{
			rect.yMin += value;
			return rect;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003FFD File Offset: 0x000021FD
		public static Rect SubYMin(this Rect rect, float value)
		{
			rect.yMin -= value;
			return rect;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000400F File Offset: 0x0000220F
		public static Rect SetYMax(this Rect rect, float yMax)
		{
			rect.yMax = yMax;
			return rect;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000401A File Offset: 0x0000221A
		public static Rect AddYMax(this Rect rect, float value)
		{
			rect.yMax += value;
			return rect;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000402C File Offset: 0x0000222C
		public static Rect SubYMax(this Rect rect, float value)
		{
			rect.yMax -= value;
			return rect;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000403E File Offset: 0x0000223E
		public static Rect MinWidth(this Rect rect, float minWidth)
		{
			rect.width = Mathf.Max(rect.width, minWidth);
			return rect;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004055 File Offset: 0x00002255
		public static Rect MaxWidth(this Rect rect, float maxWidth)
		{
			rect.width = Mathf.Min(rect.width, maxWidth);
			return rect;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000406C File Offset: 0x0000226C
		public static Rect MinHeight(this Rect rect, float minHeight)
		{
			rect.height = Mathf.Max(rect.height, minHeight);
			return rect;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004083 File Offset: 0x00002283
		public static Rect MaxHeight(this Rect rect, float maxHeight)
		{
			rect.height = Mathf.Min(rect.height, maxHeight);
			return rect;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000409C File Offset: 0x0000229C
		public static Rect ExpandTo(this Rect rect, Vector2 pos)
		{
			if (pos.x < rect.xMin)
			{
				rect.xMin = pos.x;
			}
			else if (pos.x > rect.xMax)
			{
				rect.xMax = pos.x;
			}
			if (pos.y < rect.yMin)
			{
				rect.yMin = pos.y;
			}
			else if (pos.y > rect.yMax)
			{
				rect.yMax = pos.y;
			}
			return rect;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004120 File Offset: 0x00002320
		public static bool IsPlaceholder(this Rect rect)
		{
			return rect.x == 0f && rect.y == 0f && ((rect.width == 1f && rect.height == 1f) || (rect.width == 0f && rect.height == 0f));
		}
	}
}
