using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000004 RID: 4
	public static class DOTweenModulePhysics
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002228 File Offset: 0x00000428
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this Rigidbody target, Vector3 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, new DOSetter<Vector3>(target.MovePosition), endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000227C File Offset: 0x0000047C
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveX(this Rigidbody target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, new DOSetter<Vector3>(target.MovePosition), new Vector3(endValue, 0f, 0f), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 2, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022E0 File Offset: 0x000004E0
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveY(this Rigidbody target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, new DOSetter<Vector3>(target.MovePosition), new Vector3(0f, endValue, 0f), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 4, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002344 File Offset: 0x00000544
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveZ(this Rigidbody target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, new DOSetter<Vector3>(target.MovePosition), new Vector3(0f, 0f, endValue), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 8, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023A8 File Offset: 0x000005A8
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DORotate(this Rigidbody target, Vector3 endValue, float duration, RotateMode mode = 0)
		{
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.rotation, new DOSetter<Quaternion>(target.MoveRotation), endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(tweenerCore, target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002400 File Offset: 0x00000600
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DOLookAt(this Rigidbody target, Vector3 towards, float duration, AxisConstraint axisConstraint = 0, Vector3? up = null)
		{
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = Extensions.SetSpecialStartupMode<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(TweenSettingsExtensions.SetTarget<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(DOTween.To(() => target.rotation, new DOSetter<Quaternion>(target.MoveRotation), towards, duration), target), 1);
			tweenerCore.plugOptions.axisConstraint = axisConstraint;
			tweenerCore.plugOptions.up = ((up == null) ? Vector3.up : up.Value);
			return tweenerCore;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002480 File Offset: 0x00000680
		public static Sequence DOJump(this Rigidbody target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
		{
			if (numJumps < 1)
			{
				numJumps = 1;
			}
			float startPosY = 0f;
			float offsetY = -1f;
			bool offsetYSet = false;
			Sequence s = DOTween.Sequence();
			Tween yTween = TweenSettingsExtensions.OnStart<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetRelative<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.position, new DOSetter<Vector3>(target.MovePosition), new Vector3(0f, jumpPower, 0f), duration / (float)(numJumps * 2)), 4, snapping), 6)), numJumps * 2, 1), delegate()
			{
				startPosY = target.position.y;
			});
			TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.SetTarget<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(s, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.position, new DOSetter<Vector3>(target.MovePosition), new Vector3(endValue.x, 0f, 0f), duration), 2, snapping), 1)), TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.position, new DOSetter<Vector3>(target.MovePosition), new Vector3(0f, 0f, endValue.z), duration), 8, snapping), 1)), yTween), target), DOTween.defaultEaseType);
			TweenSettingsExtensions.OnUpdate<Tween>(yTween, delegate()
			{
				if (!offsetYSet)
				{
					offsetYSet = true;
					offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
				}
				Vector3 position = target.position;
				position.y += DOVirtual.EasedValue(0f, offsetY, TweenExtensions.ElapsedPercentage(yTween, true), 6);
				target.MovePosition(position);
			});
			return s;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002624 File Offset: 0x00000824
		public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Rigidbody target, Vector3[] path, float duration, PathType pathType = 0, PathMode pathMode = 1, int resolution = 10, Color? gizmoColor = null)
		{
			if (resolution < 1)
			{
				resolution = 1;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = TweenSettingsExtensions.SetUpdate<TweenerCore<Vector3, Path, PathOptions>>(TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Path, PathOptions>>(DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.position, new DOSetter<Vector3>(target.MovePosition), new Path(pathType, path, resolution, gizmoColor), duration), target), 2);
			tweenerCore.plugOptions.isRigidbody = true;
			tweenerCore.plugOptions.mode = pathMode;
			return tweenerCore;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000026A4 File Offset: 0x000008A4
		public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Rigidbody target, Vector3[] path, float duration, PathType pathType = 0, PathMode pathMode = 1, int resolution = 10, Color? gizmoColor = null)
		{
			if (resolution < 1)
			{
				resolution = 1;
			}
			Transform trans = target.transform;
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = TweenSettingsExtensions.SetUpdate<TweenerCore<Vector3, Path, PathOptions>>(TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Path, PathOptions>>(DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => trans.localPosition, delegate(Vector3 x)
			{
				target.MovePosition((trans.parent == null) ? x : trans.parent.TransformPoint(x));
			}, new Path(pathType, path, resolution, gizmoColor), duration), target), 2);
			tweenerCore.plugOptions.isRigidbody = true;
			tweenerCore.plugOptions.mode = pathMode;
			tweenerCore.plugOptions.useLocalPosition = true;
			return tweenerCore;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000273C File Offset: 0x0000093C
		internal static TweenerCore<Vector3, Path, PathOptions> DOPath(this Rigidbody target, Path path, float duration, PathMode pathMode = 1)
		{
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Path, PathOptions>>(DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.position, new DOSetter<Vector3>(target.MovePosition), path, duration), target);
			tweenerCore.plugOptions.isRigidbody = true;
			tweenerCore.plugOptions.mode = pathMode;
			return tweenerCore;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027A4 File Offset: 0x000009A4
		internal static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Rigidbody target, Path path, float duration, PathMode pathMode = 1)
		{
			Transform trans = target.transform;
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Path, PathOptions>>(DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => trans.localPosition, delegate(Vector3 x)
			{
				target.MovePosition((trans.parent == null) ? x : trans.parent.TransformPoint(x));
			}, path, duration), target);
			tweenerCore.plugOptions.isRigidbody = true;
			tweenerCore.plugOptions.mode = pathMode;
			tweenerCore.plugOptions.useLocalPosition = true;
			return tweenerCore;
		}
	}
}
