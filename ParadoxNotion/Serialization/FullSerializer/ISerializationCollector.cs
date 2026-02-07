using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000B1 RID: 177
	public interface ISerializationCollector : ISerializationCollectable
	{
		// Token: 0x060006B6 RID: 1718
		void OnPush(ISerializationCollector parent);

		// Token: 0x060006B7 RID: 1719
		void OnCollect(ISerializationCollectable child, int depth);

		// Token: 0x060006B8 RID: 1720
		void OnPop(ISerializationCollector parent);
	}
}
