using System;
using System.Collections.Generic;
using Unity.Jobs;

namespace Pathfinding
{
	// Token: 0x0200009C RID: 156
	public interface IGraphUpdatePromise
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x000059E1 File Offset: 0x00003BE1
		float Progress
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000035D8 File Offset: 0x000017D8
		IEnumerator<JobHandle> Prepare()
		{
			return null;
		}

		// Token: 0x060004E7 RID: 1255
		void Apply(IGraphUpdateContext context);
	}
}
