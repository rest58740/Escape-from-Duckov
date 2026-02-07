using System;
using Pathfinding.Drawing;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000064 RID: 100
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__17700261659878220535
{
	// Token: 0x060002B0 RID: 688 RVA: 0x00010BE4 File Offset: 0x0000EDE4
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<GeometryBuilderJob>();
			IJobExtensions.EarlyJobInit<PersistentFilterJob>();
			IJobExtensions.EarlyJobInit<StreamSplitter>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00010C24 File Offset: 0x0000EE24
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__17700261659878220535.CreateJobReflectionData();
	}
}
