using System;
using System.Diagnostics;

namespace Animancer
{
	// Token: 0x02000042 RID: 66
	public class DontAllowFade : Key, IUpdatable, Key.IListItem
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x0000BC46 File Offset: 0x00009E46
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(AnimancerPlayable animancer)
		{
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000BC48 File Offset: 0x00009E48
		private static void Validate(AnimancerNode node)
		{
			if (node != null && node.FadeSpeed != 0f)
			{
				node.Weight = node.TargetWeight;
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000BC68 File Offset: 0x00009E68
		void IUpdatable.Update()
		{
			AnimancerPlayable.LayerList layers = AnimancerPlayable.Current.Layers;
			for (int i = layers.Count - 1; i >= 0; i--)
			{
				AnimancerLayer animancerLayer = layers[i];
				DontAllowFade.Validate(animancerLayer);
				DontAllowFade.Validate(animancerLayer.CurrentState);
			}
		}
	}
}
