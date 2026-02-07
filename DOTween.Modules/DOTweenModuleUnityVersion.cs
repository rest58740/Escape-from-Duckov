using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000008 RID: 8
	public static class DOTweenModuleUnityVersion
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00003DF4 File Offset: 0x00001FF4
		public static Sequence DOGradientColor(this Material target, Gradient gradient, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			GradientColorKey[] colorKeys = gradient.colorKeys;
			int num = colorKeys.Length;
			for (int i = 0; i < num; i++)
			{
				GradientColorKey gradientColorKey = colorKeys[i];
				if (i == 0 && gradientColorKey.time <= 0f)
				{
					target.color = gradientColorKey.color;
				}
				else
				{
					float num2 = (i == num - 1) ? (duration - TweenExtensions.Duration(sequence, false)) : (duration * ((i == 0) ? gradientColorKey.time : (gradientColorKey.time - colorKeys[i - 1].time)));
					TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(ShortcutExtensions.DOColor(target, gradientColorKey.color, num2), 1));
				}
			}
			TweenSettingsExtensions.SetTarget<Sequence>(sequence, target);
			return sequence;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003EAC File Offset: 0x000020AC
		public static Sequence DOGradientColor(this Material target, Gradient gradient, string property, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			GradientColorKey[] colorKeys = gradient.colorKeys;
			int num = colorKeys.Length;
			for (int i = 0; i < num; i++)
			{
				GradientColorKey gradientColorKey = colorKeys[i];
				if (i == 0 && gradientColorKey.time <= 0f)
				{
					target.SetColor(property, gradientColorKey.color);
				}
				else
				{
					float num2 = (i == num - 1) ? (duration - TweenExtensions.Duration(sequence, false)) : (duration * ((i == 0) ? gradientColorKey.time : (gradientColorKey.time - colorKeys[i - 1].time)));
					TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(ShortcutExtensions.DOColor(target, gradientColorKey.color, property, num2), 1));
				}
			}
			TweenSettingsExtensions.SetTarget<Sequence>(sequence, target);
			return sequence;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003F63 File Offset: 0x00002163
		public static CustomYieldInstruction WaitForCompletion(this Tween t, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForCompletion(t);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003F83 File Offset: 0x00002183
		public static CustomYieldInstruction WaitForRewind(this Tween t, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForRewind(t);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003FA3 File Offset: 0x000021A3
		public static CustomYieldInstruction WaitForKill(this Tween t, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForKill(t);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003FC3 File Offset: 0x000021C3
		public static CustomYieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForElapsedLoops(t, elapsedLoops);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003FE4 File Offset: 0x000021E4
		public static CustomYieldInstruction WaitForPosition(this Tween t, float position, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForPosition(t, position);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004005 File Offset: 0x00002205
		public static CustomYieldInstruction WaitForStart(this Tween t, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForStart(t);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004028 File Offset: 0x00002228
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOOffset(this Material target, Vector2 endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureOffset(propertyID), delegate(Vector2 x)
			{
				target.SetTextureOffset(propertyID, x);
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Vector2, Vector2, VectorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000040A0 File Offset: 0x000022A0
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOTiling(this Material target, Vector2 endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureScale(propertyID), delegate(Vector2 x)
			{
				target.SetTextureScale(propertyID, x);
			}, endValue, duration);
			TweenSettingsExtensions.SetTarget<TweenerCore<Vector2, Vector2, VectorOptions>>(tweenerCore, target);
			return tweenerCore;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004118 File Offset: 0x00002318
		public static Task AsyncWaitForCompletion(this Tween t)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForCompletion>d__10 <AsyncWaitForCompletion>d__;
			<AsyncWaitForCompletion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForCompletion>d__.t = t;
			<AsyncWaitForCompletion>d__.<>1__state = -1;
			<AsyncWaitForCompletion>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForCompletion>d__10>(ref <AsyncWaitForCompletion>d__);
			return <AsyncWaitForCompletion>d__.<>t__builder.Task;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000415C File Offset: 0x0000235C
		public static Task AsyncWaitForRewind(this Tween t)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForRewind>d__11 <AsyncWaitForRewind>d__;
			<AsyncWaitForRewind>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForRewind>d__.t = t;
			<AsyncWaitForRewind>d__.<>1__state = -1;
			<AsyncWaitForRewind>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForRewind>d__11>(ref <AsyncWaitForRewind>d__);
			return <AsyncWaitForRewind>d__.<>t__builder.Task;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000041A0 File Offset: 0x000023A0
		public static Task AsyncWaitForKill(this Tween t)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForKill>d__12 <AsyncWaitForKill>d__;
			<AsyncWaitForKill>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForKill>d__.t = t;
			<AsyncWaitForKill>d__.<>1__state = -1;
			<AsyncWaitForKill>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForKill>d__12>(ref <AsyncWaitForKill>d__);
			return <AsyncWaitForKill>d__.<>t__builder.Task;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000041E4 File Offset: 0x000023E4
		public static Task AsyncWaitForElapsedLoops(this Tween t, int elapsedLoops)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForElapsedLoops>d__13 <AsyncWaitForElapsedLoops>d__;
			<AsyncWaitForElapsedLoops>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForElapsedLoops>d__.t = t;
			<AsyncWaitForElapsedLoops>d__.elapsedLoops = elapsedLoops;
			<AsyncWaitForElapsedLoops>d__.<>1__state = -1;
			<AsyncWaitForElapsedLoops>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForElapsedLoops>d__13>(ref <AsyncWaitForElapsedLoops>d__);
			return <AsyncWaitForElapsedLoops>d__.<>t__builder.Task;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004230 File Offset: 0x00002430
		public static Task AsyncWaitForPosition(this Tween t, float position)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForPosition>d__14 <AsyncWaitForPosition>d__;
			<AsyncWaitForPosition>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForPosition>d__.t = t;
			<AsyncWaitForPosition>d__.position = position;
			<AsyncWaitForPosition>d__.<>1__state = -1;
			<AsyncWaitForPosition>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForPosition>d__14>(ref <AsyncWaitForPosition>d__);
			return <AsyncWaitForPosition>d__.<>t__builder.Task;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000427C File Offset: 0x0000247C
		public static Task AsyncWaitForStart(this Tween t)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForStart>d__15 <AsyncWaitForStart>d__;
			<AsyncWaitForStart>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForStart>d__.t = t;
			<AsyncWaitForStart>d__.<>1__state = -1;
			<AsyncWaitForStart>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForStart>d__15>(ref <AsyncWaitForStart>d__);
			return <AsyncWaitForStart>d__.<>t__builder.Task;
		}
	}
}
