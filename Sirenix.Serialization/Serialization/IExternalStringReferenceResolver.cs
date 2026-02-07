using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000069 RID: 105
	public interface IExternalStringReferenceResolver
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000375 RID: 885
		// (set) Token: 0x06000376 RID: 886
		IExternalStringReferenceResolver NextResolver { get; set; }

		// Token: 0x06000377 RID: 887
		bool TryResolveReference(string id, out object value);

		// Token: 0x06000378 RID: 888
		bool CanReference(object value, out string id);
	}
}
