using System;
using UI_Spline_Renderer;
using Unity.Jobs;
using UnityEngine;

// Token: 0x0200001B RID: 27
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__4342233803106572155
{
	// Token: 0x060000A5 RID: 165 RVA: 0x000051E8 File Offset: 0x000033E8
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<SplineExtrudeJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000521C File Offset: 0x0000341C
	[RuntimeInitializeOnLoadMethod(2)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__4342233803106572155.CreateJobReflectionData();
	}
}
