using System;
using Drawing;
using Drawing.Examples;
using Unity.Jobs;
using UnityEngine;

// Token: 0x0200006D RID: 109
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__3150089336157158032
{
	// Token: 0x060003E3 RID: 995 RVA: 0x00013534 File Offset: 0x00011734
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<GeometryBuilderJob>();
			IJobExtensions.EarlyJobInit<PersistentFilterJob>();
			IJobExtensions.EarlyJobInit<StreamSplitter>();
			IJobExtensions.EarlyJobInit<BurstExample.DrawingJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00013578 File Offset: 0x00011778
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__3150089336157158032.CreateJobReflectionData();
	}
}
