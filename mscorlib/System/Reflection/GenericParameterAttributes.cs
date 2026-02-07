using System;

namespace System.Reflection
{
	// Token: 0x020008A0 RID: 2208
	[Flags]
	public enum GenericParameterAttributes
	{
		// Token: 0x04002EA6 RID: 11942
		None = 0,
		// Token: 0x04002EA7 RID: 11943
		VarianceMask = 3,
		// Token: 0x04002EA8 RID: 11944
		Covariant = 1,
		// Token: 0x04002EA9 RID: 11945
		Contravariant = 2,
		// Token: 0x04002EAA RID: 11946
		SpecialConstraintMask = 28,
		// Token: 0x04002EAB RID: 11947
		ReferenceTypeConstraint = 4,
		// Token: 0x04002EAC RID: 11948
		NotNullableValueTypeConstraint = 8,
		// Token: 0x04002EAD RID: 11949
		DefaultConstructorConstraint = 16
	}
}
