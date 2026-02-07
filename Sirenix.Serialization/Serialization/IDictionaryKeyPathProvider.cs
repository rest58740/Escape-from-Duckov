using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000093 RID: 147
	public interface IDictionaryKeyPathProvider
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000470 RID: 1136
		string ProviderID { get; }

		// Token: 0x06000471 RID: 1137
		string GetPathStringFromKey(object key);

		// Token: 0x06000472 RID: 1138
		object GetKeyFromPathString(string pathStr);

		// Token: 0x06000473 RID: 1139
		int Compare(object x, object y);
	}
}
