using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib.Tar
{
	// Token: 0x02000047 RID: 71
	[Serializable]
	public class TarException : SharpZipBaseException
	{
		// Token: 0x06000349 RID: 841 RVA: 0x00014854 File Offset: 0x00012A54
		protected TarException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00014860 File Offset: 0x00012A60
		public TarException()
		{
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00014868 File Offset: 0x00012A68
		public TarException(string message) : base(message)
		{
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00014874 File Offset: 0x00012A74
		public TarException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
