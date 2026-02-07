using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000754 RID: 1876
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibImporterFlags
	{
		// Token: 0x04002C0F RID: 11279
		PrimaryInteropAssembly = 1,
		// Token: 0x04002C10 RID: 11280
		UnsafeInterfaces = 2,
		// Token: 0x04002C11 RID: 11281
		SafeArrayAsSystemArray = 4,
		// Token: 0x04002C12 RID: 11282
		TransformDispRetVals = 8,
		// Token: 0x04002C13 RID: 11283
		None = 0,
		// Token: 0x04002C14 RID: 11284
		PreventClassMembers = 16,
		// Token: 0x04002C15 RID: 11285
		ImportAsAgnostic = 2048,
		// Token: 0x04002C16 RID: 11286
		ImportAsItanium = 1024,
		// Token: 0x04002C17 RID: 11287
		ImportAsX64 = 512,
		// Token: 0x04002C18 RID: 11288
		ImportAsX86 = 256,
		// Token: 0x04002C19 RID: 11289
		ReflectionOnlyLoading = 4096,
		// Token: 0x04002C1A RID: 11290
		SerializableValueClasses = 32,
		// Token: 0x04002C1B RID: 11291
		NoDefineVersionResource = 8192,
		// Token: 0x04002C1C RID: 11292
		ImportAsArm = 16384
	}
}
