using System;
using Pathfinding.Serialization;

namespace Pathfinding
{
	// Token: 0x020000CD RID: 205
	public interface IGraphInternals
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000678 RID: 1656
		// (set) Token: 0x06000679 RID: 1657
		string SerializedEditorSettings { get; set; }

		// Token: 0x0600067A RID: 1658
		void OnDestroy();

		// Token: 0x0600067B RID: 1659
		void DisposeUnmanagedData();

		// Token: 0x0600067C RID: 1660
		void DestroyAllNodes();

		// Token: 0x0600067D RID: 1661
		IGraphUpdatePromise ScanInternal(bool async);

		// Token: 0x0600067E RID: 1662
		void SerializeExtraInfo(GraphSerializationContext ctx);

		// Token: 0x0600067F RID: 1663
		void DeserializeExtraInfo(GraphSerializationContext ctx);

		// Token: 0x06000680 RID: 1664
		void PostDeserialization(GraphSerializationContext ctx);
	}
}
