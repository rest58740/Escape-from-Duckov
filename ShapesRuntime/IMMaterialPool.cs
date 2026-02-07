using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shapes
{
	// Token: 0x02000020 RID: 32
	internal static class IMMaterialPool
	{
		// Token: 0x06000B33 RID: 2867 RVA: 0x00015C6F File Offset: 0x00013E6F
		static IMMaterialPool()
		{
			SceneManager.sceneUnloaded += delegate(Scene scene)
			{
				IMMaterialPool.FlushAllMaterials();
			};
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00015C90 File Offset: 0x00013E90
		internal static Material GetMaterial(ref RenderState state)
		{
			Material result;
			if (!IMMaterialPool.pool.TryGetValue(state, ref result))
			{
				IMMaterialPool.pool.Add(state, result = state.CreateMaterial());
			}
			return result;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00015CCC File Offset: 0x00013ECC
		private static void FlushAllMaterials()
		{
			foreach (Material material in IMMaterialPool.pool.Values)
			{
				if (material != null)
				{
					material.DestroyBranched();
				}
			}
			IMMaterialPool.pool.Clear();
		}

		// Token: 0x04000100 RID: 256
		public static Dictionary<RenderState, Material> pool = new Dictionary<RenderState, Material>();
	}
}
