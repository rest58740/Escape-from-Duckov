using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000094 RID: 148
	public interface IDictionaryKeyPathProvider<T> : IDictionaryKeyPathProvider
	{
		// Token: 0x06000474 RID: 1140
		string GetPathStringFromKey(T key);

		// Token: 0x06000475 RID: 1141
		T GetKeyFromPathString(string pathStr);

		// Token: 0x06000476 RID: 1142
		int Compare(T x, T y);
	}
}
