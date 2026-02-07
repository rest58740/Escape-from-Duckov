using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000067 RID: 103
	public interface IExternalGuidReferenceResolver
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600036F RID: 879
		// (set) Token: 0x06000370 RID: 880
		IExternalGuidReferenceResolver NextResolver { get; set; }

		// Token: 0x06000371 RID: 881
		bool TryResolveReference(Guid guid, out object value);

		// Token: 0x06000372 RID: 882
		bool CanReference(object value, out Guid guid);
	}
}
