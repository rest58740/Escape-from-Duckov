using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200000E RID: 14
	public enum BinaryEntryType : byte
	{
		// Token: 0x04000021 RID: 33
		Invalid,
		// Token: 0x04000022 RID: 34
		NamedStartOfReferenceNode,
		// Token: 0x04000023 RID: 35
		UnnamedStartOfReferenceNode,
		// Token: 0x04000024 RID: 36
		NamedStartOfStructNode,
		// Token: 0x04000025 RID: 37
		UnnamedStartOfStructNode,
		// Token: 0x04000026 RID: 38
		EndOfNode,
		// Token: 0x04000027 RID: 39
		StartOfArray,
		// Token: 0x04000028 RID: 40
		EndOfArray,
		// Token: 0x04000029 RID: 41
		PrimitiveArray,
		// Token: 0x0400002A RID: 42
		NamedInternalReference,
		// Token: 0x0400002B RID: 43
		UnnamedInternalReference,
		// Token: 0x0400002C RID: 44
		NamedExternalReferenceByIndex,
		// Token: 0x0400002D RID: 45
		UnnamedExternalReferenceByIndex,
		// Token: 0x0400002E RID: 46
		NamedExternalReferenceByGuid,
		// Token: 0x0400002F RID: 47
		UnnamedExternalReferenceByGuid,
		// Token: 0x04000030 RID: 48
		NamedSByte,
		// Token: 0x04000031 RID: 49
		UnnamedSByte,
		// Token: 0x04000032 RID: 50
		NamedByte,
		// Token: 0x04000033 RID: 51
		UnnamedByte,
		// Token: 0x04000034 RID: 52
		NamedShort,
		// Token: 0x04000035 RID: 53
		UnnamedShort,
		// Token: 0x04000036 RID: 54
		NamedUShort,
		// Token: 0x04000037 RID: 55
		UnnamedUShort,
		// Token: 0x04000038 RID: 56
		NamedInt,
		// Token: 0x04000039 RID: 57
		UnnamedInt,
		// Token: 0x0400003A RID: 58
		NamedUInt,
		// Token: 0x0400003B RID: 59
		UnnamedUInt,
		// Token: 0x0400003C RID: 60
		NamedLong,
		// Token: 0x0400003D RID: 61
		UnnamedLong,
		// Token: 0x0400003E RID: 62
		NamedULong,
		// Token: 0x0400003F RID: 63
		UnnamedULong,
		// Token: 0x04000040 RID: 64
		NamedFloat,
		// Token: 0x04000041 RID: 65
		UnnamedFloat,
		// Token: 0x04000042 RID: 66
		NamedDouble,
		// Token: 0x04000043 RID: 67
		UnnamedDouble,
		// Token: 0x04000044 RID: 68
		NamedDecimal,
		// Token: 0x04000045 RID: 69
		UnnamedDecimal,
		// Token: 0x04000046 RID: 70
		NamedChar,
		// Token: 0x04000047 RID: 71
		UnnamedChar,
		// Token: 0x04000048 RID: 72
		NamedString,
		// Token: 0x04000049 RID: 73
		UnnamedString,
		// Token: 0x0400004A RID: 74
		NamedGuid,
		// Token: 0x0400004B RID: 75
		UnnamedGuid,
		// Token: 0x0400004C RID: 76
		NamedBoolean,
		// Token: 0x0400004D RID: 77
		UnnamedBoolean,
		// Token: 0x0400004E RID: 78
		NamedNull,
		// Token: 0x0400004F RID: 79
		UnnamedNull,
		// Token: 0x04000050 RID: 80
		TypeName,
		// Token: 0x04000051 RID: 81
		TypeID,
		// Token: 0x04000052 RID: 82
		EndOfStream,
		// Token: 0x04000053 RID: 83
		NamedExternalReferenceByString,
		// Token: 0x04000054 RID: 84
		UnnamedExternalReferenceByString
	}
}
