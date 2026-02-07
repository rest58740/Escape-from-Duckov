using System;

namespace System.Reflection
{
	// Token: 0x020008CD RID: 2253
	[Flags]
	public enum TypeAttributes
	{
		// Token: 0x04002F36 RID: 12086
		VisibilityMask = 7,
		// Token: 0x04002F37 RID: 12087
		NotPublic = 0,
		// Token: 0x04002F38 RID: 12088
		Public = 1,
		// Token: 0x04002F39 RID: 12089
		NestedPublic = 2,
		// Token: 0x04002F3A RID: 12090
		NestedPrivate = 3,
		// Token: 0x04002F3B RID: 12091
		NestedFamily = 4,
		// Token: 0x04002F3C RID: 12092
		NestedAssembly = 5,
		// Token: 0x04002F3D RID: 12093
		NestedFamANDAssem = 6,
		// Token: 0x04002F3E RID: 12094
		NestedFamORAssem = 7,
		// Token: 0x04002F3F RID: 12095
		LayoutMask = 24,
		// Token: 0x04002F40 RID: 12096
		AutoLayout = 0,
		// Token: 0x04002F41 RID: 12097
		SequentialLayout = 8,
		// Token: 0x04002F42 RID: 12098
		ExplicitLayout = 16,
		// Token: 0x04002F43 RID: 12099
		ClassSemanticsMask = 32,
		// Token: 0x04002F44 RID: 12100
		Class = 0,
		// Token: 0x04002F45 RID: 12101
		Interface = 32,
		// Token: 0x04002F46 RID: 12102
		Abstract = 128,
		// Token: 0x04002F47 RID: 12103
		Sealed = 256,
		// Token: 0x04002F48 RID: 12104
		SpecialName = 1024,
		// Token: 0x04002F49 RID: 12105
		Import = 4096,
		// Token: 0x04002F4A RID: 12106
		Serializable = 8192,
		// Token: 0x04002F4B RID: 12107
		WindowsRuntime = 16384,
		// Token: 0x04002F4C RID: 12108
		StringFormatMask = 196608,
		// Token: 0x04002F4D RID: 12109
		AnsiClass = 0,
		// Token: 0x04002F4E RID: 12110
		UnicodeClass = 65536,
		// Token: 0x04002F4F RID: 12111
		AutoClass = 131072,
		// Token: 0x04002F50 RID: 12112
		CustomFormatClass = 196608,
		// Token: 0x04002F51 RID: 12113
		CustomFormatMask = 12582912,
		// Token: 0x04002F52 RID: 12114
		BeforeFieldInit = 1048576,
		// Token: 0x04002F53 RID: 12115
		RTSpecialName = 2048,
		// Token: 0x04002F54 RID: 12116
		HasSecurity = 262144,
		// Token: 0x04002F55 RID: 12117
		ReservedMask = 264192
	}
}
