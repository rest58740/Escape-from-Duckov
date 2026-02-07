using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000062 RID: 98
	public enum EntryType : byte
	{
		// Token: 0x0400011C RID: 284
		Invalid,
		// Token: 0x0400011D RID: 285
		String,
		// Token: 0x0400011E RID: 286
		Guid,
		// Token: 0x0400011F RID: 287
		Integer,
		// Token: 0x04000120 RID: 288
		FloatingPoint,
		// Token: 0x04000121 RID: 289
		Boolean,
		// Token: 0x04000122 RID: 290
		Null,
		// Token: 0x04000123 RID: 291
		StartOfNode,
		// Token: 0x04000124 RID: 292
		EndOfNode,
		// Token: 0x04000125 RID: 293
		InternalReference,
		// Token: 0x04000126 RID: 294
		ExternalReferenceByIndex,
		// Token: 0x04000127 RID: 295
		ExternalReferenceByGuid,
		// Token: 0x04000128 RID: 296
		StartOfArray,
		// Token: 0x04000129 RID: 297
		EndOfArray,
		// Token: 0x0400012A RID: 298
		PrimitiveArray,
		// Token: 0x0400012B RID: 299
		EndOfStream,
		// Token: 0x0400012C RID: 300
		ExternalReferenceByString
	}
}
