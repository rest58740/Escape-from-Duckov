using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000063 RID: 99
	internal interface ITaggedDataFactory
	{
		// Token: 0x06000441 RID: 1089
		ITaggedData Create(short tag, byte[] data, int offset, int count);
	}
}
