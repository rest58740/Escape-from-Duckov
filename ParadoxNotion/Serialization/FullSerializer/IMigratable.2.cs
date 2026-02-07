using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000B0 RID: 176
	public interface IMigratable<T> : IMigratable
	{
		// Token: 0x060006B5 RID: 1717
		void Migrate(T model);
	}
}
