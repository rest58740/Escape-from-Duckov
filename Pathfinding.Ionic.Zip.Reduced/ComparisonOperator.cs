using System;
using System.ComponentModel;

namespace Pathfinding.Ionic
{
	// Token: 0x02000018 RID: 24
	internal enum ComparisonOperator
	{
		// Token: 0x04000038 RID: 56
		[Description(">")]
		GreaterThan,
		// Token: 0x04000039 RID: 57
		[Description(">=")]
		GreaterThanOrEqualTo,
		// Token: 0x0400003A RID: 58
		[Description("<")]
		LesserThan,
		// Token: 0x0400003B RID: 59
		[Description("<=")]
		LesserThanOrEqualTo,
		// Token: 0x0400003C RID: 60
		[Description("=")]
		EqualTo,
		// Token: 0x0400003D RID: 61
		[Description("!=")]
		NotEqualTo
	}
}
