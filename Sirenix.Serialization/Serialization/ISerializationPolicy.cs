using System;
using System.Reflection;

namespace Sirenix.Serialization
{
	// Token: 0x0200006B RID: 107
	public interface ISerializationPolicy
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600037B RID: 891
		string ID { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600037C RID: 892
		bool AllowNonSerializableTypes { get; }

		// Token: 0x0600037D RID: 893
		bool ShouldSerializeMember(MemberInfo member);
	}
}
