using System;

namespace System.Reflection
{
	// Token: 0x020008AC RID: 2220
	[Flags]
	public enum MethodAttributes
	{
		// Token: 0x04002EC5 RID: 11973
		MemberAccessMask = 7,
		// Token: 0x04002EC6 RID: 11974
		PrivateScope = 0,
		// Token: 0x04002EC7 RID: 11975
		Private = 1,
		// Token: 0x04002EC8 RID: 11976
		FamANDAssem = 2,
		// Token: 0x04002EC9 RID: 11977
		Assembly = 3,
		// Token: 0x04002ECA RID: 11978
		Family = 4,
		// Token: 0x04002ECB RID: 11979
		FamORAssem = 5,
		// Token: 0x04002ECC RID: 11980
		Public = 6,
		// Token: 0x04002ECD RID: 11981
		Static = 16,
		// Token: 0x04002ECE RID: 11982
		Final = 32,
		// Token: 0x04002ECF RID: 11983
		Virtual = 64,
		// Token: 0x04002ED0 RID: 11984
		HideBySig = 128,
		// Token: 0x04002ED1 RID: 11985
		CheckAccessOnOverride = 512,
		// Token: 0x04002ED2 RID: 11986
		VtableLayoutMask = 256,
		// Token: 0x04002ED3 RID: 11987
		ReuseSlot = 0,
		// Token: 0x04002ED4 RID: 11988
		NewSlot = 256,
		// Token: 0x04002ED5 RID: 11989
		Abstract = 1024,
		// Token: 0x04002ED6 RID: 11990
		SpecialName = 2048,
		// Token: 0x04002ED7 RID: 11991
		PinvokeImpl = 8192,
		// Token: 0x04002ED8 RID: 11992
		UnmanagedExport = 8,
		// Token: 0x04002ED9 RID: 11993
		RTSpecialName = 4096,
		// Token: 0x04002EDA RID: 11994
		HasSecurity = 16384,
		// Token: 0x04002EDB RID: 11995
		RequireSecObject = 32768,
		// Token: 0x04002EDC RID: 11996
		ReservedMask = 53248
	}
}
