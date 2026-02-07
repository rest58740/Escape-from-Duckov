using System;
using Drawing;
using UnityEngine;

// Token: 0x0200006E RID: 110
internal static class $BurstDirectCallInitializer
{
	// Token: 0x060003E5 RID: 997 RVA: 0x0001357F File Offset: 0x0001177F
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		CommandBuilder.Initialize$JobWireMesh_WireMesh_00000105$BurstDirectCall();
		CommandBuilder.Initialize$JobWireMesh_Execute_00000106$BurstDirectCall();
		DrawingData.Initialize$BuilderData_AnyBuffersWrittenTo_000002F7$BurstDirectCall();
		DrawingData.Initialize$BuilderData_ResetAllBuffers_000002F8$BurstDirectCall();
	}
}
