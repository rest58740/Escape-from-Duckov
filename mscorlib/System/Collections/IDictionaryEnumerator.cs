using System;

namespace System.Collections
{
	// Token: 0x02000A15 RID: 2581
	public interface IDictionaryEnumerator : IEnumerator
	{
		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06005B8E RID: 23438
		object Key { get; }

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06005B8F RID: 23439
		object Value { get; }

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06005B90 RID: 23440
		DictionaryEntry Entry { get; }
	}
}
