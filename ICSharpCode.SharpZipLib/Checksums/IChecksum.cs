using System;

namespace ICSharpCode.SharpZipLib.Checksums
{
	// Token: 0x02000038 RID: 56
	public interface IChecksum
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000242 RID: 578
		long Value { get; }

		// Token: 0x06000243 RID: 579
		void Reset();

		// Token: 0x06000244 RID: 580
		void Update(int value);

		// Token: 0x06000245 RID: 581
		void Update(byte[] buffer);

		// Token: 0x06000246 RID: 582
		void Update(byte[] buffer, int offset, int count);
	}
}
