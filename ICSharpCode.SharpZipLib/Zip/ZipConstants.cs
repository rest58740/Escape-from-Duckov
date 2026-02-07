using System;
using System.Text;
using System.Threading;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000017 RID: 23
	public sealed class ZipConstants
	{
		// Token: 0x060000DE RID: 222 RVA: 0x00007ABC File Offset: 0x00005CBC
		private ZipConstants()
		{
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00007AE0 File Offset: 0x00005CE0
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00007AE8 File Offset: 0x00005CE8
		public static int DefaultCodePage
		{
			get
			{
				return ZipConstants.defaultCodePage;
			}
			set
			{
				ZipConstants.defaultCodePage = value;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007AF0 File Offset: 0x00005CF0
		public static string ConvertToString(byte[] data, int count)
		{
			if (data == null)
			{
				return string.Empty;
			}
			return Encoding.GetEncoding(ZipConstants.DefaultCodePage).GetString(data, 0, count);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007B10 File Offset: 0x00005D10
		public static string ConvertToString(byte[] data)
		{
			if (data == null)
			{
				return string.Empty;
			}
			return ZipConstants.ConvertToString(data, data.Length);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00007B28 File Offset: 0x00005D28
		public static string ConvertToStringExt(int flags, byte[] data, int count)
		{
			if (data == null)
			{
				return string.Empty;
			}
			if ((flags & 2048) != 0)
			{
				return Encoding.UTF8.GetString(data, 0, count);
			}
			return ZipConstants.ConvertToString(data, count);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007B58 File Offset: 0x00005D58
		public static string ConvertToStringExt(int flags, byte[] data)
		{
			if (data == null)
			{
				return string.Empty;
			}
			if ((flags & 2048) != 0)
			{
				return Encoding.UTF8.GetString(data, 0, data.Length);
			}
			return ZipConstants.ConvertToString(data, data.Length);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00007B8C File Offset: 0x00005D8C
		public static byte[] ConvertToArray(string str)
		{
			if (str == null)
			{
				return new byte[0];
			}
			return Encoding.GetEncoding(ZipConstants.DefaultCodePage).GetBytes(str);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007BAC File Offset: 0x00005DAC
		public static byte[] ConvertToArray(int flags, string str)
		{
			if (str == null)
			{
				return new byte[0];
			}
			if ((flags & 2048) != 0)
			{
				return Encoding.UTF8.GetBytes(str);
			}
			return ZipConstants.ConvertToArray(str);
		}

		// Token: 0x040000F9 RID: 249
		public const int VersionMadeBy = 51;

		// Token: 0x040000FA RID: 250
		[Obsolete("Use VersionMadeBy instead")]
		public const int VERSION_MADE_BY = 51;

		// Token: 0x040000FB RID: 251
		public const int VersionStrongEncryption = 50;

		// Token: 0x040000FC RID: 252
		[Obsolete("Use VersionStrongEncryption instead")]
		public const int VERSION_STRONG_ENCRYPTION = 50;

		// Token: 0x040000FD RID: 253
		public const int VERSION_AES = 51;

		// Token: 0x040000FE RID: 254
		public const int VersionZip64 = 45;

		// Token: 0x040000FF RID: 255
		public const int LocalHeaderBaseSize = 30;

		// Token: 0x04000100 RID: 256
		[Obsolete("Use LocalHeaderBaseSize instead")]
		public const int LOCHDR = 30;

		// Token: 0x04000101 RID: 257
		public const int Zip64DataDescriptorSize = 24;

		// Token: 0x04000102 RID: 258
		public const int DataDescriptorSize = 16;

		// Token: 0x04000103 RID: 259
		[Obsolete("Use DataDescriptorSize instead")]
		public const int EXTHDR = 16;

		// Token: 0x04000104 RID: 260
		public const int CentralHeaderBaseSize = 46;

		// Token: 0x04000105 RID: 261
		[Obsolete("Use CentralHeaderBaseSize instead")]
		public const int CENHDR = 46;

		// Token: 0x04000106 RID: 262
		public const int EndOfCentralRecordBaseSize = 22;

		// Token: 0x04000107 RID: 263
		[Obsolete("Use EndOfCentralRecordBaseSize instead")]
		public const int ENDHDR = 22;

		// Token: 0x04000108 RID: 264
		public const int CryptoHeaderSize = 12;

		// Token: 0x04000109 RID: 265
		[Obsolete("Use CryptoHeaderSize instead")]
		public const int CRYPTO_HEADER_SIZE = 12;

		// Token: 0x0400010A RID: 266
		public const int LocalHeaderSignature = 67324752;

		// Token: 0x0400010B RID: 267
		[Obsolete("Use LocalHeaderSignature instead")]
		public const int LOCSIG = 67324752;

		// Token: 0x0400010C RID: 268
		public const int SpanningSignature = 134695760;

		// Token: 0x0400010D RID: 269
		[Obsolete("Use SpanningSignature instead")]
		public const int SPANNINGSIG = 134695760;

		// Token: 0x0400010E RID: 270
		public const int SpanningTempSignature = 808471376;

		// Token: 0x0400010F RID: 271
		[Obsolete("Use SpanningTempSignature instead")]
		public const int SPANTEMPSIG = 808471376;

		// Token: 0x04000110 RID: 272
		public const int DataDescriptorSignature = 134695760;

		// Token: 0x04000111 RID: 273
		[Obsolete("Use DataDescriptorSignature instead")]
		public const int EXTSIG = 134695760;

		// Token: 0x04000112 RID: 274
		[Obsolete("Use CentralHeaderSignature instead")]
		public const int CENSIG = 33639248;

		// Token: 0x04000113 RID: 275
		public const int CentralHeaderSignature = 33639248;

		// Token: 0x04000114 RID: 276
		public const int Zip64CentralFileHeaderSignature = 101075792;

		// Token: 0x04000115 RID: 277
		[Obsolete("Use Zip64CentralFileHeaderSignature instead")]
		public const int CENSIG64 = 101075792;

		// Token: 0x04000116 RID: 278
		public const int Zip64CentralDirLocatorSignature = 117853008;

		// Token: 0x04000117 RID: 279
		public const int ArchiveExtraDataSignature = 117853008;

		// Token: 0x04000118 RID: 280
		public const int CentralHeaderDigitalSignature = 84233040;

		// Token: 0x04000119 RID: 281
		[Obsolete("Use CentralHeaderDigitalSignaure instead")]
		public const int CENDIGITALSIG = 84233040;

		// Token: 0x0400011A RID: 282
		public const int EndOfCentralDirectorySignature = 101010256;

		// Token: 0x0400011B RID: 283
		[Obsolete("Use EndOfCentralDirectorySignature instead")]
		public const int ENDSIG = 101010256;

		// Token: 0x0400011C RID: 284
		private static int defaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage;
	}
}
