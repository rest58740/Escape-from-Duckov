using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000005 RID: 5
	public static class DOTweenModulePhysics2D
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002824 File Offset: 0x00000A24
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOMove(this Rigidbody2D target, Vector2 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.position, new DOSetter<Vector2>(target.MovePosition), endValue, duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002878 File Offset: 0x00000A78
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOMoveX(this Rigidbody2D target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.position, new DOSetter<Vector2>(target.MovePosition), new Vector2(endValue, 0f), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 2, snapping), target);
			return tweenerCore;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000028D4 File Offset: 0x00000AD4
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOMoveY(this Rigidbody2D target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.position, new DOSetter<Vector2>(target.MovePosition), new Vector2(0f, endValue), duration);
			TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, 4, snapping), target);
			return tweenerCore;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002930 File Offset: 0x00000B30
		public static TweenerCore<float, float, FloatOptions> DORotate(this Rigidbody2D target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.rotation, new DOSetter<float>(target.MoveRotation), endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<float, float, FloatOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000297C File Offset: 0x00000B7C
		public static Sequence DOJump(this Rigidbody2D target, Vector2 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
		{
			if (numJumps < 1)
			{
				numJumps = 1;
			}
			float startPosY = 0f;
			float offsetY = -1f;
			bool offsetYSet = false;
			Sequence s = DOTween.Sequence();
			Tween yTween = TweenSettingsExtensions.OnStart<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetRelative<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.position, delegate(Vector2 x)
			{
				target.position = x;
			}, new Vector2(0f, jumpPower), duration / (float)(numJumps * 2)), 4, snapping), 6)), numJumps * 2, 1), delegate()
			{
				startPosY = target.position.y;
			});
			TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.SetTarget<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(s, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetOptions(DOTween.To(() => target.position, delegate(Vector2 x)
			{
				target.position = x;
			}, new Vector2(endValue.x, 0f), duration), 2, snapping), 1)), yTween), target), DOTween.defaultEaseType);
			TweenSettingsExtensions.OnUpdate<Tween>(yTween, delegate()
			{
				if (!offsetYSet)
				{
					offsetYSet = true;
					offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
				}
				Vector3 v = target.position;
				v.y += DOVirtual.EasedValue(0f, offsetY, TweenExtensions.ElapsedPercentage(yTween, true), 6);
				target.MovePosition(v);
			});
			return s;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002ABC File Offset: 0x00000CBC
		public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Rigidbody2D target, Vector2[] path, float duration, PathType pathType = 0, PathMode pathMode = 1, int resolution = 10, Color? gizmoColor = null)
		{
			if (resolution < 1)
			{
				resolution = 1;
			}
			int num = path.Length;
			Vector3[] array = new Vector3[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = path[i];
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = TweenSettingsExtensions.SetUpdate<TweenerCore<Vector3, Path, PathOptions>>(TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Path, PathOptions>>(DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
			{
				target.MovePosition(x);
			}, new Path(pathType, array, resolution, gizmoColor), duration), target), 2);
			tweenerCore.plugOptions.isRigidbody2D = true;
			tweenerCore.plugOptions.mode = pathMode;
			return tweenerCore;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002B60 File Offset: 0x00000D60
		public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Rigidbody2D target, Vector2[] path, float duration, PathType pathType = 0, PathMode pathMode = 1, int resolution = 10, Color? gizmoColor = null)
		{
			if (resolution < 1)
			{
				resolution = 1;
			}
			int num = path.Length;
			Vector3[] array = new Vector3[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = path[i];
			}
			Transform trans = target.transform;
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = TweenSettingsExtensions.SetUpdate<TweenerCore<Vector3, Path, PathOptions>>(TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Path, PathOptions>>(DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => trans.localPosition, delegate(Vector3 x)
			{
				target.MovePosition((trans.parent == null) ? x : trans.parent.TransformPoint(x));
			}, new Path(pathType, array, resolution, gizmoColor), duration), target), 2);
			tweenerCore.plugOptions.isRigidbody2D = true;
			tweenerCore.plugOptions.mode = pathMode;
			tweenerCore.plugOptions.useLocalPosition = true;
			return tweenerCore;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C24 File Offset: 0x00000E24
		internal static TweenerCore<Vector3, Path, PathOptions> DOPath(this Rigidbody2D target, Path path, float duration, PathMode pathMode = 1)
		{
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Path, PathOptions>>(DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
			{
				target.MovePosition(x);
			}, path, duration), target);
			tweenerCore.plugOptions.isRigidbody2D = true;
			tweenerCore.plugOptions.mode = pathMode;
			return tweenerCore;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002C88 File Offset: 0x00000E88
		internal static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Rigidbody2D target, Path path, float duration, PathMode pathMode = 1)
		{
			Transform trans = target.transform;
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = TweenSettingsExtensions.SetTarget<TweenerCore<Vector3, Path, PathOptions>>(DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => trans.localPosition, delegate(Vector3 x)
			{
				target.MovePosition((trans.parent == null) ? x : trans.parent.TransformPoint(x));
			}, path, duration), target);
			tweenerCore.plugOptions.isRigidbody2D = true;
			tweenerCore.plugOptions.mode = pathMode;
			tweenerCore.plugOptions.useLocalPosition = true;
			return tweenerCore;
		}
	}
}
