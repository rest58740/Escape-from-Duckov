using System;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Core
{
	// Token: 0x02000051 RID: 81
	public static class Extensions
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000FC93 File Offset: 0x0000DE93
		public static T SetSpecialStartupMode<T>(this T t, SpecialStartupMode mode) where T : Tween
		{
			t.specialStartupMode = mode;
			return t;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000FCA2 File Offset: 0x0000DEA2
		public static TweenerCore<T1, T2, TPlugOptions> Blendable<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			t.isBlendable = true;
			return t;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000FCAC File Offset: 0x0000DEAC
		public static TweenerCore<T1, T2, TPlugOptions> NoFrom<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			t.isFromAllowed = false;
			return t;
		}
	}
}
