using System;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x0200002D RID: 45
	[Flags]
	public enum ZipEntryTimestamp
	{
		// Token: 0x040000CB RID: 203
		None = 0,
		// Token: 0x040000CC RID: 204
		DOS = 1,
		// Token: 0x040000CD RID: 205
		Windows = 2,
		// Token: 0x040000CE RID: 206
		Unix = 4,
		// Token: 0x040000CF RID: 207
		InfoZip1 = 8
	}
}
