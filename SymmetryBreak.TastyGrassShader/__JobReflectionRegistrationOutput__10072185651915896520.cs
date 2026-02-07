using System;
using SymmetryBreakStudio.TastyGrassShader;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000028 RID: 40
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__10072185651915896520
{
	// Token: 0x0600009D RID: 157 RVA: 0x000066D8 File Offset: 0x000048D8
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForExtensions.EarlyJobInit<TgsManager.TgsPreRenderJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0000670C File Offset: 0x0000490C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__10072185651915896520.CreateJobReflectionData();
	}
}
