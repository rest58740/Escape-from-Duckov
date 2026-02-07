using System;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000037 RID: 55
	internal class ImageHelper
	{
		// Token: 0x06000173 RID: 371 RVA: 0x00006304 File Offset: 0x00004504
		public static ImageHelper.ImageFormat GetImageFormat(byte[] bytes)
		{
			byte[] value = new byte[]
			{
				66,
				77
			};
			byte[] value2 = new byte[]
			{
				71,
				73,
				70
			};
			byte[] value3 = new byte[]
			{
				137,
				80,
				78,
				71
			};
			byte[] value4 = new byte[]
			{
				73,
				73,
				42
			};
			byte[] value5 = new byte[]
			{
				77,
				77,
				42
			};
			byte[] value6 = new byte[]
			{
				byte.MaxValue,
				216,
				byte.MaxValue,
				224
			};
			byte[] value7 = new byte[]
			{
				byte.MaxValue,
				216,
				byte.MaxValue,
				225
			};
			if (bytes.StartsWith(value))
			{
				return ImageHelper.ImageFormat.bmp;
			}
			if (bytes.StartsWith(value2))
			{
				return ImageHelper.ImageFormat.gif;
			}
			if (bytes.StartsWith(value3))
			{
				return ImageHelper.ImageFormat.png;
			}
			if (bytes.StartsWith(value4))
			{
				return ImageHelper.ImageFormat.tiff;
			}
			if (bytes.StartsWith(value5))
			{
				return ImageHelper.ImageFormat.tiff;
			}
			if (bytes.StartsWith(value6))
			{
				return ImageHelper.ImageFormat.jpg;
			}
			if (bytes.StartsWith(value7))
			{
				return ImageHelper.ImageFormat.jpg;
			}
			return ImageHelper.ImageFormat.unknown;
		}

		// Token: 0x020000A8 RID: 168
		public enum ImageFormat
		{
			// Token: 0x0400030B RID: 779
			bmp,
			// Token: 0x0400030C RID: 780
			jpg,
			// Token: 0x0400030D RID: 781
			gif,
			// Token: 0x0400030E RID: 782
			tiff,
			// Token: 0x0400030F RID: 783
			png,
			// Token: 0x04000310 RID: 784
			unknown
		}
	}
}
