using System;

namespace System.Reflection
{
	// Token: 0x02000893 RID: 2195
	[Flags]
	public enum BindingFlags
	{
		// Token: 0x04002E68 RID: 11880
		Default = 0,
		// Token: 0x04002E69 RID: 11881
		IgnoreCase = 1,
		// Token: 0x04002E6A RID: 11882
		DeclaredOnly = 2,
		// Token: 0x04002E6B RID: 11883
		Instance = 4,
		// Token: 0x04002E6C RID: 11884
		Static = 8,
		// Token: 0x04002E6D RID: 11885
		Public = 16,
		// Token: 0x04002E6E RID: 11886
		NonPublic = 32,
		// Token: 0x04002E6F RID: 11887
		FlattenHierarchy = 64,
		// Token: 0x04002E70 RID: 11888
		InvokeMethod = 256,
		// Token: 0x04002E71 RID: 11889
		CreateInstance = 512,
		// Token: 0x04002E72 RID: 11890
		GetField = 1024,
		// Token: 0x04002E73 RID: 11891
		SetField = 2048,
		// Token: 0x04002E74 RID: 11892
		GetProperty = 4096,
		// Token: 0x04002E75 RID: 11893
		SetProperty = 8192,
		// Token: 0x04002E76 RID: 11894
		PutDispProperty = 16384,
		// Token: 0x04002E77 RID: 11895
		PutRefDispProperty = 32768,
		// Token: 0x04002E78 RID: 11896
		ExactBinding = 65536,
		// Token: 0x04002E79 RID: 11897
		SuppressChangeType = 131072,
		// Token: 0x04002E7A RID: 11898
		OptionalParamBinding = 262144,
		// Token: 0x04002E7B RID: 11899
		IgnoreReturn = 16777216,
		// Token: 0x04002E7C RID: 11900
		DoNotWrapExceptions = 33554432
	}
}
