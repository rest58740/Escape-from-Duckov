using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000005 RID: 5
	public static class DOTweenProShortcuts
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000035B2 File Offset: 0x000017B2
		static DOTweenProShortcuts()
		{
			new SpiralPlugin();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000035BC File Offset: 0x000017BC
		public static Tweener DOSpiral(this Transform target, float duration, Vector3? axis = null, SpiralMode mode = 0, float speed = 1f, float frequency = 10f, float depth = 0f, bool snapping = false)
		{
			if (Mathf.Approximately(speed, 0f))
			{
				speed = 1f;
			}
			if (axis != null)
			{
				Vector3? vector = axis;
				Vector3 zero = Vector3.zero;
				if (vector == null || (vector != null && !(vector.GetValueOrDefault() == zero)))
				{
					goto IL_66;
				}
			}
			axis = new Vector3?(Vector3.forward);
			IL_66:
			TweenerCore<Vector3, Vector3, SpiralOptions> tweenerCore = TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Vector3, SpiralOptions>>(DOTween.To<Vector3, Vector3, SpiralOptions>(SpiralPlugin.Get(), () => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, axis.Value, duration), target);
			tweenerCore.plugOptions.mode = mode;
			tweenerCore.plugOptions.speed = speed;
			tweenerCore.plugOptions.frequency = frequency;
			tweenerCore.plugOptions.depth = depth;
			tweenerCore.plugOptions.snapping = snapping;
			return tweenerCore;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000036A4 File Offset: 0x000018A4
		public static Tweener DOSpiral(this Rigidbody target, float duration, Vector3? axis = null, SpiralMode mode = 0, float speed = 1f, float frequency = 10f, float depth = 0f, bool snapping = false)
		{
			if (Mathf.Approximately(speed, 0f))
			{
				speed = 1f;
			}
			if (axis != null)
			{
				Vector3? vector = axis;
				Vector3 zero = Vector3.zero;
				if (vector == null || (vector != null && !(vector.GetValueOrDefault() == zero)))
				{
					goto IL_66;
				}
			}
			axis = new Vector3?(Vector3.forward);
			IL_66:
			TweenerCore<Vector3, Vector3, SpiralOptions> tweenerCore = TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Vector3, SpiralOptions>>(DOTween.To<Vector3, Vector3, SpiralOptions>(SpiralPlugin.Get(), () => target.position, new DOSetter<Vector3>(target.MovePosition), axis.Value, duration), target);
			tweenerCore.plugOptions.mode = mode;
			tweenerCore.plugOptions.speed = speed;
			tweenerCore.plugOptions.frequency = frequency;
			tweenerCore.plugOptions.depth = depth;
			tweenerCore.plugOptions.snapping = snapping;
			return tweenerCore;
		}
	}
}
