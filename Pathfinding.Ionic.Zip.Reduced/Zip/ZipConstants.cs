using System;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000026 RID: 38
	internal static class ZipConstants
	{
		// Token: 0x04000063 RID: 99
		public const uint PackedToRemovableMedia = 808471376U;

		// Token: 0x04000064 RID: 100
		public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792U;

		// Token: 0x04000065 RID: 101
		public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008U;

		// Token: 0x04000066 RID: 102
		public const uint EndOfCentralDirectorySignature = 101010256U;

		// Token: 0x04000067 RID: 103
		public const int ZipEntrySignature = 67324752;

		// Token: 0x04000068 RID: 104
		public const int ZipEntryDataDescriptorSignature = 134695760;

		// Token: 0x04000069 RID: 105
		public const int SplitArchiveSignature = 134695760;

		// Token: 0x0400006A RID: 106
		public const int ZipDirEntrySignature = 33639248;

		// Token: 0x0400006B RID: 107
		public const int AesKeySize = 192;

		// Token: 0x0400006C RID: 108
		public const int AesBlockSize = 128;

		// Token: 0x0400006D RID: 109
		public const ushort AesAlgId128 = 26126;

		// Token: 0x0400006E RID: 110
		public const ushort AesAlgId192 = 26127;

		// Token: 0x0400006F RID: 111
		public const ushort AesAlgId256 = 26128;
	}
}
