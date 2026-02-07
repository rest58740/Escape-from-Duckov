using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.CustomPlugins;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000016 RID: 22
	public static class ShortcutExtensions
	{
		// Token: 0x060000AE RID: 174 RVA: 0x000049F8 File Offset: 0x00002BF8
		public static TweenerCore<float, float, FloatOptions> DOAspect(this Camera target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.aspect, delegate(float x)
			{
				target.aspect = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004A40 File Offset: 0x00002C40
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Camera target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.backgroundColor, delegate(Color x)
			{
				target.backgroundColor = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004A88 File Offset: 0x00002C88
		public static TweenerCore<float, float, FloatOptions> DOFarClipPlane(this Camera target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.farClipPlane, delegate(float x)
			{
				target.farClipPlane = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public static TweenerCore<float, float, FloatOptions> DOFieldOfView(this Camera target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.fieldOfView, delegate(float x)
			{
				target.fieldOfView = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004B18 File Offset: 0x00002D18
		public static TweenerCore<float, float, FloatOptions> DONearClipPlane(this Camera target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.nearClipPlane, delegate(float x)
			{
				target.nearClipPlane = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004B60 File Offset: 0x00002D60
		public static TweenerCore<float, float, FloatOptions> DOOrthoSize(this Camera target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.orthographicSize, delegate(float x)
			{
				target.orthographicSize = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public static TweenerCore<Rect, Rect, RectOptions> DOPixelRect(this Camera target, Rect endValue, float duration)
		{
			TweenerCore<Rect, Rect, RectOptions> tweenerCore = DOTween.To(() => target.pixelRect, delegate(Rect x)
			{
				target.pixelRect = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004BF0 File Offset: 0x00002DF0
		public static TweenerCore<Rect, Rect, RectOptions> DORect(this Camera target, Rect endValue, float duration)
		{
			TweenerCore<Rect, Rect, RectOptions> tweenerCore = DOTween.To(() => target.rect, delegate(Rect x)
			{
				target.rect = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004C38 File Offset: 0x00002E38
		public static Tweener DOShakePosition(this Camera target, float duration, float strength = 3f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.transform.localPosition, delegate(Vector3 x)
			{
				target.transform.localPosition = x;
			}, duration, strength, vibrato, randomness, true, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetCameraShakePosition);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004CA4 File Offset: 0x00002EA4
		public static Tweener DOShakePosition(this Camera target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.transform.localPosition, delegate(Vector3 x)
			{
				target.transform.localPosition = x;
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetCameraShakePosition);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004D10 File Offset: 0x00002F10
		public static Tweener DOShakeRotation(this Camera target, float duration, float strength = 90f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.transform.localEulerAngles, delegate(Vector3 x)
			{
				target.transform.localRotation = Quaternion.Euler(x);
			}, duration, strength, vibrato, randomness, false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004D7C File Offset: 0x00002F7C
		public static Tweener DOShakeRotation(this Camera target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.transform.localEulerAngles, delegate(Vector3 x)
			{
				target.transform.localRotation = Quaternion.Euler(x);
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004DE8 File Offset: 0x00002FE8
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Light target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004E30 File Offset: 0x00003030
		public static TweenerCore<float, float, FloatOptions> DOIntensity(this Light target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.intensity, delegate(float x)
			{
				target.intensity = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004E78 File Offset: 0x00003078
		public static TweenerCore<float, float, FloatOptions> DOShadowStrength(this Light target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.shadowStrength, delegate(float x)
			{
				target.shadowStrength = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004EC0 File Offset: 0x000030C0
		public static Tweener DOColor(this LineRenderer target, Color2 startValue, Color2 endValue, float duration)
		{
			return DOTween.To(() => startValue, delegate(Color2 x)
			{
				target.SetColors(x.ca, x.cb);
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004F0C File Offset: 0x0000310C
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Material target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004F54 File Offset: 0x00003154
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Material target, Color endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.GetColor(property), delegate(Color x)
			{
				target.SetColor(property, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004FCC File Offset: 0x000031CC
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this Material target, Color endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.GetColor(propertyID), delegate(Color x)
			{
				target.SetColor(propertyID, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005044 File Offset: 0x00003244
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this Material target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000508C File Offset: 0x0000328C
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this Material target, float endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.GetColor(property), delegate(Color x)
			{
				target.SetColor(property, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005104 File Offset: 0x00003304
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this Material target, float endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.GetColor(propertyID), delegate(Color x)
			{
				target.SetColor(propertyID, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000517C File Offset: 0x0000337C
		public static TweenerCore<float, float, FloatOptions> DOFloat(this Material target, float endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.GetFloat(property), delegate(float x)
			{
				target.SetFloat(property, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000051F4 File Offset: 0x000033F4
		public static TweenerCore<float, float, FloatOptions> DOFloat(this Material target, float endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.GetFloat(propertyID), delegate(float x)
			{
				target.SetFloat(propertyID, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000526C File Offset: 0x0000346C
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOOffset(this Material target, Vector2 endValue, float duration)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.mainTextureOffset, delegate(Vector2 x)
			{
				target.mainTextureOffset = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000052B4 File Offset: 0x000034B4
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOOffset(this Material target, Vector2 endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureOffset(property), delegate(Vector2 x)
			{
				target.SetTextureOffset(property, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000532C File Offset: 0x0000352C
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOTiling(this Material target, Vector2 endValue, float duration)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.mainTextureScale, delegate(Vector2 x)
			{
				target.mainTextureScale = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005374 File Offset: 0x00003574
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOTiling(this Material target, Vector2 endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureScale(property), delegate(Vector2 x)
			{
				target.SetTextureScale(property, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000053EC File Offset: 0x000035EC
		public static TweenerCore<Vector4, Vector4, VectorOptions> DOVector(this Material target, Vector4 endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			TweenerCore<Vector4, Vector4, VectorOptions> tweenerCore = DOTween.To(() => target.GetVector(property), delegate(Vector4 x)
			{
				target.SetVector(property, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005464 File Offset: 0x00003664
		public static TweenerCore<Vector4, Vector4, VectorOptions> DOVector(this Material target, Vector4 endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			TweenerCore<Vector4, Vector4, VectorOptions> tweenerCore = DOTween.To(() => target.GetVector(propertyID), delegate(Vector4 x)
			{
				target.SetVector(propertyID, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000054DC File Offset: 0x000036DC
		public static Tweener DOResize(this TrailRenderer target, float toStartWidth, float toEndWidth, float duration)
		{
			return DOTween.To(() => new Vector2(target.startWidth, target.endWidth), delegate(Vector2 x)
			{
				target.startWidth = x.x;
				target.endWidth = x.y;
			}, new Vector2(toStartWidth, toEndWidth), duration).SetTarget(target);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005528 File Offset: 0x00003728
		public static TweenerCore<float, float, FloatOptions> DOTime(this TrailRenderer target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.time, delegate(float x)
			{
				target.time = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005570 File Offset: 0x00003770
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this Transform target, Vector3 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, endValue, duration);
			tweenerCore.SetOptions(snapping).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000055BC File Offset: 0x000037BC
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveX(this Transform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(endValue, 0f, 0f), duration);
			tweenerCore.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005618 File Offset: 0x00003818
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveY(this Transform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(0f, endValue, 0f), duration);
			tweenerCore.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005674 File Offset: 0x00003874
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveZ(this Transform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(0f, 0f, endValue), duration);
			tweenerCore.SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000056D0 File Offset: 0x000038D0
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMove(this Transform target, Vector3 endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, endValue, duration);
			tweenerCore.SetOptions(snapping).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000571C File Offset: 0x0000391C
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveX(this Transform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(endValue, 0f, 0f), duration);
			tweenerCore.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005778 File Offset: 0x00003978
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveY(this Transform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(0f, endValue, 0f), duration);
			tweenerCore.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000057D4 File Offset: 0x000039D4
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveZ(this Transform target, float endValue, float duration, bool snapping = false)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(0f, 0f, endValue), duration);
			tweenerCore.SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005830 File Offset: 0x00003A30
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DORotate(this Transform target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.rotation, delegate(Quaternion x)
			{
				target.rotation = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005884 File Offset: 0x00003A84
		public static TweenerCore<Quaternion, Quaternion, NoOptions> DORotateQuaternion(this Transform target, Quaternion endValue, float duration)
		{
			TweenerCore<Quaternion, Quaternion, NoOptions> tweenerCore = DOTween.To<Quaternion, Quaternion, NoOptions>(PureQuaternionPlugin.Plug(), () => target.rotation, delegate(Quaternion x)
			{
				target.rotation = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000058D0 File Offset: 0x00003AD0
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DOLocalRotate(this Transform target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.localRotation, delegate(Quaternion x)
			{
				target.localRotation = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005924 File Offset: 0x00003B24
		public static TweenerCore<Quaternion, Quaternion, NoOptions> DOLocalRotateQuaternion(this Transform target, Quaternion endValue, float duration)
		{
			TweenerCore<Quaternion, Quaternion, NoOptions> tweenerCore = DOTween.To<Quaternion, Quaternion, NoOptions>(PureQuaternionPlugin.Plug(), () => target.localRotation, delegate(Quaternion x)
			{
				target.localRotation = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005970 File Offset: 0x00003B70
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this Transform target, Vector3 endValue, float duration)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000059B8 File Offset: 0x00003BB8
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this Transform target, float endValue, float duration)
		{
			Vector3 endValue2 = new Vector3(endValue, endValue, endValue);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, endValue2, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005A08 File Offset: 0x00003C08
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleX(this Transform target, float endValue, float duration)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, new Vector3(endValue, 0f, 0f), duration);
			tweenerCore.SetOptions(AxisConstraint.X, false).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005A64 File Offset: 0x00003C64
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleY(this Transform target, float endValue, float duration)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, new Vector3(0f, endValue, 0f), duration);
			tweenerCore.SetOptions(AxisConstraint.Y, false).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleZ(this Transform target, float endValue, float duration)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, new Vector3(0f, 0f, endValue), duration);
			tweenerCore.SetOptions(AxisConstraint.Z, false).SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005B1C File Offset: 0x00003D1C
		public static Tweener DOLookAt(this Transform target, Vector3 towards, float duration, AxisConstraint axisConstraint = AxisConstraint.None, Vector3? up = null)
		{
			return target.LookAt(towards, duration, axisConstraint, up, false);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005B2A File Offset: 0x00003D2A
		public static Tweener DODynamicLookAt(this Transform target, Vector3 towards, float duration, AxisConstraint axisConstraint = AxisConstraint.None, Vector3? up = null)
		{
			return target.LookAt(towards, duration, axisConstraint, up, true);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005B38 File Offset: 0x00003D38
		private static Tweener LookAt(this Transform target, Vector3 towards, float duration, AxisConstraint axisConstraint, Vector3? up, bool dynamic)
		{
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.rotation, delegate(Quaternion x)
			{
				target.rotation = x;
			}, towards, duration).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetLookAt);
			tweenerCore.plugOptions.axisConstraint = axisConstraint;
			tweenerCore.plugOptions.up = ((up == null) ? Vector3.up : up.Value);
			if (dynamic)
			{
				tweenerCore.plugOptions.dynamicLookAt = true;
				tweenerCore.plugOptions.dynamicLookAtWorldPosition = towards;
			}
			else
			{
				tweenerCore.plugOptions.dynamicLookAt = false;
			}
			return tweenerCore;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005BDC File Offset: 0x00003DDC
		public static Tweener DOPunchPosition(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f, bool snapping = false)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOPunchPosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, punch, duration, vibrato, elasticity).SetTarget(target).SetOptions(snapping);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005C48 File Offset: 0x00003E48
		public static Tweener DOPunchScale(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOPunchScale: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, punch, duration, vibrato, elasticity).SetTarget(target);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005CAC File Offset: 0x00003EAC
		public static Tweener DOPunchRotation(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOPunchRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => target.localEulerAngles, delegate(Vector3 x)
			{
				target.localRotation = Quaternion.Euler(x);
			}, punch, duration, vibrato, elasticity).SetTarget(target);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005D10 File Offset: 0x00003F10
		public static Tweener DOShakePosition(this Transform target, float duration, float strength = 1f, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, duration, strength, vibrato, randomness, false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake).SetOptions(snapping);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005D84 File Offset: 0x00003F84
		public static Tweener DOShakePosition(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake).SetOptions(snapping);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005DF8 File Offset: 0x00003FF8
		public static Tweener DOShakeRotation(this Transform target, float duration, float strength = 90f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localEulerAngles, delegate(Vector3 x)
			{
				target.localRotation = Quaternion.Euler(x);
			}, duration, strength, vibrato, randomness, false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005E64 File Offset: 0x00004064
		public static Tweener DOShakeRotation(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localEulerAngles, delegate(Vector3 x)
			{
				target.localRotation = Quaternion.Euler(x);
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005ED0 File Offset: 0x000040D0
		public static Tweener DOShakeScale(this Transform target, float duration, float strength = 1f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				Debug.Log(Debugger.logPriority);
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeScale: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, duration, strength, vibrato, randomness, false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005F4C File Offset: 0x0000414C
		public static Tweener DOShakeScale(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeScale: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005FB8 File Offset: 0x000041B8
		public static Sequence DOJump(this Transform target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
		{
			if (numJumps < 1)
			{
				numJumps = 1;
			}
			float startPosY = target.position.y;
			float offsetY = -1f;
			bool offsetYSet = false;
			Sequence s = DOTween.Sequence();
			Tween yTween = DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(0f, jumpPower, 0f), duration / (float)(numJumps * 2)).SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad).SetRelative<Tweener>().SetLoops(numJumps * 2, LoopType.Yoyo).OnStart(delegate
			{
				startPosY = target.position.y;
			});
			s.Append(DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(endValue.x, 0f, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)).Join(DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(0f, 0f, endValue.z), duration).SetOptions(AxisConstraint.Z, snapping).SetEase(Ease.Linear)).Join(yTween).SetTarget(target).SetEase(DOTween.defaultEaseType);
			yTween.OnUpdate(delegate
			{
				if (!offsetYSet)
				{
					offsetYSet = true;
					offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
				}
				Vector3 position = target.position;
				position.y += DOVirtual.EasedValue(0f, offsetY, yTween.ElapsedPercentage(true), Ease.OutQuad);
				target.position = position;
			});
			return s;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006158 File Offset: 0x00004358
		public static Sequence DOLocalJump(this Transform target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
		{
			if (numJumps < 1)
			{
				numJumps = 1;
			}
			float startPosY = target.localPosition.y;
			float offsetY = -1f;
			bool offsetYSet = false;
			Sequence s = DOTween.Sequence();
			Tween yTween = DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(0f, jumpPower, 0f), duration / (float)(numJumps * 2)).SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad).SetRelative<Tweener>().SetLoops(numJumps * 2, LoopType.Yoyo).OnStart(delegate
			{
				startPosY = target.localPosition.y;
			});
			s.Append(DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(endValue.x, 0f, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)).Join(DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(0f, 0f, endValue.z), duration).SetOptions(AxisConstraint.Z, snapping).SetEase(Ease.Linear)).Join(yTween).SetTarget(target).SetEase(DOTween.defaultEaseType);
			yTween.OnUpdate(delegate
			{
				if (!offsetYSet)
				{
					offsetYSet = true;
					offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
				}
				Vector3 localPosition = target.localPosition;
				localPosition.y += DOVirtual.EasedValue(0f, offsetY, yTween.ElapsedPercentage(true), Ease.OutQuad);
				target.localPosition = localPosition;
			});
			return s;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000062F8 File Offset: 0x000044F8
		public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Transform target, Vector3[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
		{
			if (resolution < 1)
			{
				resolution = 1;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Path(pathType, path, resolution, gizmoColor), duration).SetTarget(target);
			tweenerCore.plugOptions.mode = pathMode;
			return tweenerCore;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006360 File Offset: 0x00004560
		public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Transform target, Vector3[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
		{
			if (resolution < 1)
			{
				resolution = 1;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Path(pathType, path, resolution, gizmoColor), duration).SetTarget(target);
			tweenerCore.plugOptions.mode = pathMode;
			tweenerCore.plugOptions.useLocalPosition = true;
			return tweenerCore;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000063D4 File Offset: 0x000045D4
		public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Transform target, Path path, float duration, PathMode pathMode = PathMode.Full3D)
		{
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, path, duration).SetTarget(target);
			tweenerCore.plugOptions.mode = pathMode;
			return tweenerCore;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000642C File Offset: 0x0000462C
		public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Transform target, Path path, float duration, PathMode pathMode = PathMode.Full3D)
		{
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, path, duration).SetTarget(target);
			tweenerCore.plugOptions.mode = pathMode;
			tweenerCore.plugOptions.useLocalPosition = true;
			return tweenerCore;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006490 File Offset: 0x00004690
		public static TweenerCore<float, float, FloatOptions> DOTimeScale(this Tween target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.timeScale, delegate(float x)
			{
				target.timeScale = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000064D8 File Offset: 0x000046D8
		public static Tweener DOBlendableColor(this Light target, Color endValue, float duration)
		{
			endValue -= target.color;
			Color to = new Color(0f, 0f, 0f, 0f);
			return DOTween.To(() => to, delegate(Color x)
			{
				Color b = x - to;
				to = x;
				target.color += b;
			}, endValue, duration).Blendable<Color, Color, ColorOptions>().SetTarget(target);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006554 File Offset: 0x00004754
		public static Tweener DOBlendableColor(this Material target, Color endValue, float duration)
		{
			endValue -= target.color;
			Color to = new Color(0f, 0f, 0f, 0f);
			return DOTween.To(() => to, delegate(Color x)
			{
				Color b = x - to;
				to = x;
				target.color += b;
			}, endValue, duration).Blendable<Color, Color, ColorOptions>().SetTarget(target);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000065D0 File Offset: 0x000047D0
		public static Tweener DOBlendableColor(this Material target, Color endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			endValue -= target.GetColor(property);
			Color to = new Color(0f, 0f, 0f, 0f);
			return DOTween.To(() => to, delegate(Color x)
			{
				Color b = x - to;
				to = x;
				target.SetColor(property, target.GetColor(property) + b);
			}, endValue, duration).Blendable<Color, Color, ColorOptions>().SetTarget(target);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006680 File Offset: 0x00004880
		public static Tweener DOBlendableColor(this Material target, Color endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			endValue -= target.GetColor(propertyID);
			Color to = new Color(0f, 0f, 0f, 0f);
			return DOTween.To(() => to, delegate(Color x)
			{
				Color b = x - to;
				to = x;
				target.SetColor(propertyID, target.GetColor(propertyID) + b);
			}, endValue, duration).Blendable<Color, Color, ColorOptions>().SetTarget(target);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006730 File Offset: 0x00004930
		public static Tweener DOBlendableMoveBy(this Transform target, Vector3 byValue, float duration, bool snapping = false)
		{
			Vector3 to = Vector3.zero;
			return DOTween.To(() => to, delegate(Vector3 x)
			{
				Vector3 b = x - to;
				to = x;
				target.position += b;
			}, byValue, duration).Blendable<Vector3, Vector3, VectorOptions>().SetOptions(snapping).SetTarget(target);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000678C File Offset: 0x0000498C
		public static Tweener DOBlendableLocalMoveBy(this Transform target, Vector3 byValue, float duration, bool snapping = false)
		{
			Vector3 to = Vector3.zero;
			return DOTween.To(() => to, delegate(Vector3 x)
			{
				Vector3 b = x - to;
				to = x;
				target.localPosition += b;
			}, byValue, duration).Blendable<Vector3, Vector3, VectorOptions>().SetOptions(snapping).SetTarget(target);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000067E8 File Offset: 0x000049E8
		public static Tweener DOBlendableRotateBy(this Transform target, Vector3 byValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			Quaternion to = Quaternion.identity;
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => to, delegate(Quaternion x)
			{
				Quaternion rhs = x * Quaternion.Inverse(to);
				to = x;
				Quaternion rotation = target.rotation;
				target.rotation = rotation * Quaternion.Inverse(rotation) * rhs * rotation;
			}, byValue, duration).Blendable<Quaternion, Vector3, QuaternionOptions>().SetTarget(target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006848 File Offset: 0x00004A48
		public static Tweener DOBlendableLocalRotateBy(this Transform target, Vector3 byValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			Quaternion to = Quaternion.identity;
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => to, delegate(Quaternion x)
			{
				Quaternion rhs = x * Quaternion.Inverse(to);
				to = x;
				Quaternion localRotation = target.localRotation;
				target.localRotation = localRotation * Quaternion.Inverse(localRotation) * rhs * localRotation;
			}, byValue, duration).Blendable<Quaternion, Vector3, QuaternionOptions>().SetTarget(target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000068A8 File Offset: 0x00004AA8
		public static Tweener DOBlendablePunchRotation(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOBlendablePunchRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			Vector3 to = Vector3.zero;
			return DOTween.Punch(() => to, delegate(Vector3 v)
			{
				Quaternion rotation = Quaternion.Euler(to.x, to.y, to.z);
				Quaternion rhs = Quaternion.Euler(v.x, v.y, v.z) * Quaternion.Inverse(rotation);
				to = v;
				Quaternion rotation2 = target.rotation;
				target.rotation = rotation2 * Quaternion.Inverse(rotation2) * rhs * rotation2;
			}, punch, duration, vibrato, elasticity).Blendable<Vector3, Vector3[], Vector3ArrayOptions>().SetTarget(target);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000691C File Offset: 0x00004B1C
		public static Tweener DOBlendableScaleBy(this Transform target, Vector3 byValue, float duration)
		{
			Vector3 to = Vector3.zero;
			return DOTween.To(() => to, delegate(Vector3 x)
			{
				Vector3 b = x - to;
				to = x;
				target.localScale += b;
			}, byValue, duration).Blendable<Vector3, Vector3, VectorOptions>().SetTarget(target);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006970 File Offset: 0x00004B70
		public static int DOComplete(this Component target, bool withCallbacks = false)
		{
			return DOTween.Complete(target, withCallbacks);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006970 File Offset: 0x00004B70
		public static int DOComplete(this Material target, bool withCallbacks = false)
		{
			return DOTween.Complete(target, withCallbacks);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00006979 File Offset: 0x00004B79
		public static int DOKill(this Component target, bool complete = false)
		{
			return DOTween.Kill(target, complete);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006979 File Offset: 0x00004B79
		public static int DOKill(this Material target, bool complete = false)
		{
			return DOTween.Kill(target, complete);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006982 File Offset: 0x00004B82
		public static int DOFlip(this Component target)
		{
			return DOTween.Flip(target);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006982 File Offset: 0x00004B82
		public static int DOFlip(this Material target)
		{
			return DOTween.Flip(target);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000698A File Offset: 0x00004B8A
		public static int DOGoto(this Component target, float to, bool andPlay = false)
		{
			return DOTween.Goto(target, to, andPlay);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000698A File Offset: 0x00004B8A
		public static int DOGoto(this Material target, float to, bool andPlay = false)
		{
			return DOTween.Goto(target, to, andPlay);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006994 File Offset: 0x00004B94
		public static int DOPause(this Component target)
		{
			return DOTween.Pause(target);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006994 File Offset: 0x00004B94
		public static int DOPause(this Material target)
		{
			return DOTween.Pause(target);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000699C File Offset: 0x00004B9C
		public static int DOPlay(this Component target)
		{
			return DOTween.Play(target);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000699C File Offset: 0x00004B9C
		public static int DOPlay(this Material target)
		{
			return DOTween.Play(target);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000069A4 File Offset: 0x00004BA4
		public static int DOPlayBackwards(this Component target)
		{
			return DOTween.PlayBackwards(target);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000069A4 File Offset: 0x00004BA4
		public static int DOPlayBackwards(this Material target)
		{
			return DOTween.PlayBackwards(target);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000069AC File Offset: 0x00004BAC
		public static int DOPlayForward(this Component target)
		{
			return DOTween.PlayForward(target);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000069AC File Offset: 0x00004BAC
		public static int DOPlayForward(this Material target)
		{
			return DOTween.PlayForward(target);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000069B4 File Offset: 0x00004BB4
		public static int DORestart(this Component target, bool includeDelay = true)
		{
			return DOTween.Restart(target, includeDelay, -1f);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000069B4 File Offset: 0x00004BB4
		public static int DORestart(this Material target, bool includeDelay = true)
		{
			return DOTween.Restart(target, includeDelay, -1f);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000069C2 File Offset: 0x00004BC2
		public static int DORewind(this Component target, bool includeDelay = true)
		{
			return DOTween.Rewind(target, includeDelay);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000069C2 File Offset: 0x00004BC2
		public static int DORewind(this Material target, bool includeDelay = true)
		{
			return DOTween.Rewind(target, includeDelay);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000069CB File Offset: 0x00004BCB
		public static int DOSmoothRewind(this Component target)
		{
			return DOTween.SmoothRewind(target);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000069CB File Offset: 0x00004BCB
		public static int DOSmoothRewind(this Material target)
		{
			return DOTween.SmoothRewind(target);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000069D3 File Offset: 0x00004BD3
		public static int DOTogglePause(this Component target)
		{
			return DOTween.TogglePause(target);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000069D3 File Offset: 0x00004BD3
		public static int DOTogglePause(this Material target)
		{
			return DOTween.TogglePause(target);
		}
	}
}
