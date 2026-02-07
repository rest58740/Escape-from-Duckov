using System;
using System.Collections.Generic;
using UnityEngine;

namespace EPOOutline
{
	// Token: 0x02000019 RID: 25
	public static class RendererFilteringUtility
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00006008 File Offset: 0x00004208
		public static void Filter(Camera camera, OutlineParameters parameters)
		{
			RendererFilteringUtility.filteredOutlinables.Clear();
			int num = parameters.Mask.value & camera.cullingMask;
			foreach (Outlinable outlinable in parameters.OutlinablesToRender)
			{
				long num2 = 1L << outlinable.OutlineLayer;
				if ((parameters.OutlineLayerMask & num2) != 0L)
				{
					GameObject gameObject = outlinable.gameObject;
					if (gameObject.activeInHierarchy && (1 << gameObject.layer & num) != 0)
					{
						RendererFilteringUtility.filteredOutlinables.Add(outlinable);
					}
				}
			}
			parameters.OutlinablesToRender.Clear();
			parameters.OutlinablesToRender.AddRange(RendererFilteringUtility.filteredOutlinables);
		}

		// Token: 0x040000AF RID: 175
		private static List<Outlinable> filteredOutlinables = new List<Outlinable>();
	}
}
