using System;
using Pathfinding.Drawing;
using UnityEngine;

// Token: 0x02000065 RID: 101
internal static class $BurstDirectCallInitializer
{
	// Token: 0x060002B2 RID: 690 RVA: 0x00010C2B File Offset: 0x0000EE2B
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		CommandBuilder.Initialize$JobWireMesh_WireMesh_000000D1$BurstDirectCall();
		CommandBuilder.Initialize$JobWireMesh_Execute_000000D2$BurstDirectCall();
		DrawingData.Initialize$BuilderData_AnyBuffersWrittenTo_000001E7$BurstDirectCall();
		DrawingData.Initialize$BuilderData_ResetAllBuffers_000001E8$BurstDirectCall();
	}
}
