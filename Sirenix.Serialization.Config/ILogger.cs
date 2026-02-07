using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000007 RID: 7
	public interface ILogger
	{
		// Token: 0x06000014 RID: 20
		void LogWarning(string warning);

		// Token: 0x06000015 RID: 21
		void LogError(string error);

		// Token: 0x06000016 RID: 22
		void LogException(Exception exception);
	}
}
