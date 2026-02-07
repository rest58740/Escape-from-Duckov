using System;

namespace System.Security.AccessControl
{
	// Token: 0x0200052A RID: 1322
	[Flags]
	public enum FileSystemRights
	{
		// Token: 0x0400248E RID: 9358
		ListDirectory = 1,
		// Token: 0x0400248F RID: 9359
		ReadData = 1,
		// Token: 0x04002490 RID: 9360
		CreateFiles = 2,
		// Token: 0x04002491 RID: 9361
		WriteData = 2,
		// Token: 0x04002492 RID: 9362
		AppendData = 4,
		// Token: 0x04002493 RID: 9363
		CreateDirectories = 4,
		// Token: 0x04002494 RID: 9364
		ReadExtendedAttributes = 8,
		// Token: 0x04002495 RID: 9365
		WriteExtendedAttributes = 16,
		// Token: 0x04002496 RID: 9366
		ExecuteFile = 32,
		// Token: 0x04002497 RID: 9367
		Traverse = 32,
		// Token: 0x04002498 RID: 9368
		DeleteSubdirectoriesAndFiles = 64,
		// Token: 0x04002499 RID: 9369
		ReadAttributes = 128,
		// Token: 0x0400249A RID: 9370
		WriteAttributes = 256,
		// Token: 0x0400249B RID: 9371
		Write = 278,
		// Token: 0x0400249C RID: 9372
		Delete = 65536,
		// Token: 0x0400249D RID: 9373
		ReadPermissions = 131072,
		// Token: 0x0400249E RID: 9374
		Read = 131209,
		// Token: 0x0400249F RID: 9375
		ReadAndExecute = 131241,
		// Token: 0x040024A0 RID: 9376
		Modify = 197055,
		// Token: 0x040024A1 RID: 9377
		ChangePermissions = 262144,
		// Token: 0x040024A2 RID: 9378
		TakeOwnership = 524288,
		// Token: 0x040024A3 RID: 9379
		Synchronize = 1048576,
		// Token: 0x040024A4 RID: 9380
		FullControl = 2032127
	}
}
