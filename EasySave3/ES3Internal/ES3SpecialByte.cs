using System;

namespace ES3Internal
{
	// Token: 0x020000E5 RID: 229
	internal enum ES3SpecialByte : byte
	{
		// Token: 0x0400015A RID: 346
		Null,
		// Token: 0x0400015B RID: 347
		Bool,
		// Token: 0x0400015C RID: 348
		Byte,
		// Token: 0x0400015D RID: 349
		Sbyte,
		// Token: 0x0400015E RID: 350
		Char,
		// Token: 0x0400015F RID: 351
		Decimal,
		// Token: 0x04000160 RID: 352
		Double,
		// Token: 0x04000161 RID: 353
		Float,
		// Token: 0x04000162 RID: 354
		Int,
		// Token: 0x04000163 RID: 355
		Uint,
		// Token: 0x04000164 RID: 356
		Long,
		// Token: 0x04000165 RID: 357
		Ulong,
		// Token: 0x04000166 RID: 358
		Short,
		// Token: 0x04000167 RID: 359
		Ushort,
		// Token: 0x04000168 RID: 360
		String,
		// Token: 0x04000169 RID: 361
		ByteArray,
		// Token: 0x0400016A RID: 362
		Collection = 128,
		// Token: 0x0400016B RID: 363
		Dictionary,
		// Token: 0x0400016C RID: 364
		CollectionItem,
		// Token: 0x0400016D RID: 365
		Object = 254,
		// Token: 0x0400016E RID: 366
		Terminator
	}
}
