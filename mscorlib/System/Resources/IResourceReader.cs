using System;
using System.Collections;

namespace System.Resources
{
	// Token: 0x02000859 RID: 2137
	public interface IResourceReader : IEnumerable, IDisposable
	{
		// Token: 0x06004734 RID: 18228
		void Close();

		// Token: 0x06004735 RID: 18229
		IDictionaryEnumerator GetEnumerator();
	}
}
