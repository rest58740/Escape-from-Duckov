using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x02000052 RID: 82
	public interface INameTransform
	{
		// Token: 0x060003C6 RID: 966
		string TransformFile(string name);

		// Token: 0x060003C7 RID: 967
		string TransformDirectory(string name);
	}
}
