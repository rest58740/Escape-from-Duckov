using System;
using System.Collections.Generic;

namespace Sirenix.Serialization
{
	// Token: 0x02000091 RID: 145
	public abstract class BaseDictionaryKeyPathProvider<T> : IDictionaryKeyPathProvider<T>, IDictionaryKeyPathProvider, IComparer<T>
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000460 RID: 1120
		public abstract string ProviderID { get; }

		// Token: 0x06000461 RID: 1121
		public abstract T GetKeyFromPathString(string pathStr);

		// Token: 0x06000462 RID: 1122
		public abstract string GetPathStringFromKey(T key);

		// Token: 0x06000463 RID: 1123
		public abstract int Compare(T x, T y);

		// Token: 0x06000464 RID: 1124 RVA: 0x0001EF4A File Offset: 0x0001D14A
		int IDictionaryKeyPathProvider.Compare(object x, object y)
		{
			return this.Compare((T)((object)x), (T)((object)y));
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001EF5E File Offset: 0x0001D15E
		object IDictionaryKeyPathProvider.GetKeyFromPathString(string pathStr)
		{
			return this.GetKeyFromPathString(pathStr);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001EF6C File Offset: 0x0001D16C
		string IDictionaryKeyPathProvider.GetPathStringFromKey(object key)
		{
			return this.GetPathStringFromKey((T)((object)key));
		}
	}
}
