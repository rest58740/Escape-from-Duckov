using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000068 RID: 104
	public interface IExternalIndexReferenceResolver
	{
		// Token: 0x06000373 RID: 883
		bool TryResolveReference(int index, out object value);

		// Token: 0x06000374 RID: 884
		bool CanReference(object value, out int index);
	}
}
