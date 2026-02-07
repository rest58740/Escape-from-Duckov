using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000030 RID: 48
	public interface IArchiveStorage
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001E3 RID: 483
		FileUpdateMode UpdateMode { get; }

		// Token: 0x060001E4 RID: 484
		Stream GetTemporaryOutput();

		// Token: 0x060001E5 RID: 485
		Stream ConvertTemporaryToFinal();

		// Token: 0x060001E6 RID: 486
		Stream MakeTemporaryCopy(Stream stream);

		// Token: 0x060001E7 RID: 487
		Stream OpenForDirectUpdate(Stream stream);

		// Token: 0x060001E8 RID: 488
		void Dispose();
	}
}
