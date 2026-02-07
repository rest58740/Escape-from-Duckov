using System;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000006 RID: 6
	[AddComponentMenu("DOTween/DOTween Path")]
	public class DOTweenPath : ABSAnimationComponent
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000006 RID: 6 RVA: 0x000021CC File Offset: 0x000003CC
		// (remove) Token: 0x06000007 RID: 7 RVA: 0x00002200 File Offset: 0x00000400
		public static event Action<DOTweenPath> OnReset;

		// Token: 0x06000008 RID: 8 RVA: 0x00002233 File Offset: 0x00000433
		private static void Dispatch_OnReset(DOTweenPath path)
		{
			if (DOTweenPath.OnReset != null)
			{
				DOTweenPath.OnReset(path);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002248 File Offset: 0x00000448
		private void Awake()
		{
			if (this.path == null || this.wps.Count < 1 || this.inspectorMode == DOTweenInspectorMode.OnlyPath)
			{
				return;
			}
			if (DOTweenPath._miCreateTween == null)
			{
				DOTweenPath._miCreateTween = DOTweenUtils.GetLooseScriptType("DG.Tweening.DOTweenModuleUtils+Physics").GetMethod("CreateDOTweenPathTween", BindingFlags.Static | BindingFlags.Public);
			}
			this.path.AssignDecoder(this.path.type);
			if (TweenManager.isUnityEditor)
			{
				DOTween.GizmosDelegates.Add(new TweenCallback(this.path.Draw));
				this.path.gizmoColor = this.pathColor;
			}
			if (this.isLocal)
			{
				Transform transform = base.transform;
				if (transform.parent != null)
				{
					transform = transform.parent;
					Vector3 position = transform.position;
					int num = this.path.wps.Length;
					for (int i = 0; i < num; i++)
					{
						this.path.wps[i] = this.path.wps[i] - position;
					}
					num = this.path.controlPoints.Length;
					for (int j = 0; j < num; j++)
					{
						ControlPoint controlPoint = this.path.controlPoints[j];
						controlPoint.a -= position;
						controlPoint.b -= position;
						this.path.controlPoints[j] = controlPoint;
					}
				}
			}
			if (this.relative)
			{
				this.ReEvaluateRelativeTween();
			}
			if (this.pathMode == 1 && base.GetComponent<SpriteRenderer>() != null)
			{
				this.pathMode = 2;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = (TweenerCore<Vector3, Path, PathOptions>)DOTweenPath._miCreateTween.Invoke(null, new object[]
			{
				this,
				this.tweenRigidbody,
				this.isLocal,
				this.path,
				this.duration,
				this.pathMode
			});
			TweenSettingsExtensions.SetOptions(tweenerCore, this.isClosedPath, 0, this.lockRotation);
			switch (this.orientType)
			{
			case 1:
				if (this.assignForwardAndUp)
				{
					TweenSettingsExtensions.SetLookAt(tweenerCore, this.lookAhead, new Vector3?(this.forwardDirection), new Vector3?(this.upDirection));
				}
				else
				{
					TweenSettingsExtensions.SetLookAt(tweenerCore, this.lookAhead, null, null);
				}
				break;
			case 2:
				if (this.lookAtTransform != null)
				{
					if (this.assignForwardAndUp)
					{
						TweenSettingsExtensions.SetLookAt(tweenerCore, this.lookAtTransform, new Vector3?(this.forwardDirection), new Vector3?(this.upDirection));
					}
					else
					{
						TweenSettingsExtensions.SetLookAt(tweenerCore, this.lookAtTransform, null, null);
					}
				}
				break;
			case 3:
				if (this.assignForwardAndUp)
				{
					TweenSettingsExtensions.SetLookAt(tweenerCore, this.lookAtPosition, new Vector3?(this.forwardDirection), new Vector3?(this.upDirection));
				}
				else
				{
					TweenSettingsExtensions.SetLookAt(tweenerCore, this.lookAtPosition, null, null);
				}
				break;
			}
			TweenSettingsExtensions.OnKill<TweenerCore<Vector3, Path, PathOptions>>(TweenSettingsExtensions.SetUpdate<TweenerCore<Vector3, Path, PathOptions>>(TweenSettingsExtensions.SetAutoKill<TweenerCore<Vector3, Path, PathOptions>>(TweenSettingsExtensions.SetLoops<TweenerCore<Vector3, Path, PathOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, this.delay), this.loops, this.loopType), this.autoKill), this.updateType), delegate()
			{
				this.tween = null;
			});
			if (this.isSpeedBased)
			{
				TweenSettingsExtensions.SetSpeedBased<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore);
			}
			if (this.easeType == 37)
			{
				TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, this.easeCurve);
			}
			else
			{
				TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, this.easeType);
			}
			if (!string.IsNullOrEmpty(this.id))
			{
				TweenSettingsExtensions.SetId<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, this.id);
			}
			if (this.hasOnStart)
			{
				if (this.onStart != null)
				{
					TweenSettingsExtensions.OnStart<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, new TweenCallback(this.onStart.Invoke));
				}
			}
			else
			{
				this.onStart = null;
			}
			if (this.hasOnPlay)
			{
				if (this.onPlay != null)
				{
					TweenSettingsExtensions.OnPlay<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, new TweenCallback(this.onPlay.Invoke));
				}
			}
			else
			{
				this.onPlay = null;
			}
			if (this.hasOnUpdate)
			{
				if (this.onUpdate != null)
				{
					TweenSettingsExtensions.OnUpdate<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, new TweenCallback(this.onUpdate.Invoke));
				}
			}
			else
			{
				this.onUpdate = null;
			}
			if (this.hasOnStepComplete)
			{
				if (this.onStepComplete != null)
				{
					TweenSettingsExtensions.OnStepComplete<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, new TweenCallback(this.onStepComplete.Invoke));
				}
			}
			else
			{
				this.onStepComplete = null;
			}
			if (this.hasOnComplete)
			{
				if (this.onComplete != null)
				{
					TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, new TweenCallback(this.onComplete.Invoke));
				}
			}
			else
			{
				this.onComplete = null;
			}
			if (this.hasOnRewind)
			{
				if (this.onRewind != null)
				{
					TweenSettingsExtensions.OnRewind<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore, new TweenCallback(this.onRewind.Invoke));
				}
			}
			else
			{
				this.onRewind = null;
			}
			if (this.autoPlay)
			{
				TweenExtensions.Play<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore);
			}
			else
			{
				TweenExtensions.Pause<TweenerCore<Vector3, Path, PathOptions>>(tweenerCore);
			}
			this.tween = tweenerCore;
			if (this.hasOnTweenCreated && this.onTweenCreated != null)
			{
				this.onTweenCreated.Invoke();
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002786 File Offset: 0x00000986
		private void Reset()
		{
			this.path = new Path(this.pathType, this.wps.ToArray(), 10, new Color?(this.pathColor));
			DOTweenPath.Dispatch_OnReset(this);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000027B7 File Offset: 0x000009B7
		private void OnDestroy()
		{
			if (this.tween != null && this.tween.active)
			{
				TweenExtensions.Kill(this.tween, false);
			}
			this.tween = null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000027E1 File Offset: 0x000009E1
		public override void DOPlay()
		{
			TweenExtensions.Play<Tween>(this.tween);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000027EF File Offset: 0x000009EF
		public void DOPlayById(string id)
		{
			DOTween.Play(base.gameObject, id);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000027FE File Offset: 0x000009FE
		public void DOPlayAllById(string id)
		{
			DOTween.Play(id);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002807 File Offset: 0x00000A07
		public override void DOPlayBackwards()
		{
			TweenExtensions.PlayBackwards(this.tween);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002814 File Offset: 0x00000A14
		public override void DOPlayForward()
		{
			TweenExtensions.PlayForward(this.tween);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002821 File Offset: 0x00000A21
		public override void DOPause()
		{
			TweenExtensions.Pause<Tween>(this.tween);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000282F File Offset: 0x00000A2F
		public override void DOTogglePause()
		{
			TweenExtensions.TogglePause(this.tween);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000283C File Offset: 0x00000A3C
		public override void DORewind()
		{
			TweenExtensions.Rewind(this.tween, true);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000284A File Offset: 0x00000A4A
		public override void DORestart()
		{
			this.DORestart(false);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002854 File Offset: 0x00000A54
		public override void DORestart(bool fromHere)
		{
			if (this.tween == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(this.tween);
				}
				return;
			}
			if (fromHere && this.relative && !this.isLocal)
			{
				this.ReEvaluateRelativeTween();
			}
			TweenExtensions.Restart(this.tween, true, -1f);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000028A7 File Offset: 0x00000AA7
		public override void DOComplete()
		{
			TweenExtensions.Complete(this.tween);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000028B4 File Offset: 0x00000AB4
		public override void DOKill()
		{
			TweenExtensions.Kill(this.tween, false);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000028C2 File Offset: 0x00000AC2
		public void DOKillAllById(string id)
		{
			DOTween.Kill(id, false);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000028CC File Offset: 0x00000ACC
		public Tween GetTween()
		{
			if (this.tween == null || !this.tween.active)
			{
				if (Debugger.logPriority > 1)
				{
					if (this.tween == null)
					{
						Debugger.LogNullTween(this.tween);
					}
					else
					{
						Debugger.LogInvalidTween(this.tween);
					}
				}
				return null;
			}
			return this.tween;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002920 File Offset: 0x00000B20
		public Vector3[] GetDrawPoints()
		{
			if (this.path.wps == null || this.path.nonLinearDrawWps == null)
			{
				Debugger.LogWarning("Draw points not ready yet. Returning NULL", null);
				return null;
			}
			if (this.pathType == null)
			{
				return this.path.wps;
			}
			return this.path.nonLinearDrawWps;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002974 File Offset: 0x00000B74
		internal Vector3[] GetFullWps()
		{
			int count = this.wps.Count;
			int num = count + 1;
			if (this.isClosedPath)
			{
				num++;
			}
			Vector3[] array = new Vector3[num];
			array[0] = base.transform.position;
			for (int i = 0; i < count; i++)
			{
				array[i + 1] = this.wps[i];
			}
			if (this.isClosedPath)
			{
				array[num - 1] = array[0];
			}
			return array;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000029F0 File Offset: 0x00000BF0
		private void ReEvaluateRelativeTween()
		{
			Vector3 position = base.transform.position;
			if (position == this.lastSrcPosition)
			{
				return;
			}
			Vector3 b = position - this.lastSrcPosition;
			int num = this.path.wps.Length;
			for (int i = 0; i < num; i++)
			{
				this.path.wps[i] = this.path.wps[i] + b;
			}
			num = this.path.controlPoints.Length;
			for (int j = 0; j < num; j++)
			{
				ControlPoint controlPoint = this.path.controlPoints[j];
				controlPoint.a += b;
				controlPoint.b += b;
				this.path.controlPoints[j] = controlPoint;
			}
			this.lastSrcPosition = position;
		}

		// Token: 0x04000012 RID: 18
		public float delay;

		// Token: 0x04000013 RID: 19
		public float duration = 1f;

		// Token: 0x04000014 RID: 20
		public Ease easeType = 6;

		// Token: 0x04000015 RID: 21
		public AnimationCurve easeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000016 RID: 22
		public int loops = 1;

		// Token: 0x04000017 RID: 23
		public string id = "";

		// Token: 0x04000018 RID: 24
		public LoopType loopType;

		// Token: 0x04000019 RID: 25
		public OrientType orientType;

		// Token: 0x0400001A RID: 26
		public Transform lookAtTransform;

		// Token: 0x0400001B RID: 27
		public Vector3 lookAtPosition;

		// Token: 0x0400001C RID: 28
		public float lookAhead = 0.01f;

		// Token: 0x0400001D RID: 29
		public bool autoPlay = true;

		// Token: 0x0400001E RID: 30
		public bool autoKill = true;

		// Token: 0x0400001F RID: 31
		public bool relative;

		// Token: 0x04000020 RID: 32
		public bool isLocal;

		// Token: 0x04000021 RID: 33
		public bool isClosedPath;

		// Token: 0x04000022 RID: 34
		public int pathResolution = 10;

		// Token: 0x04000023 RID: 35
		public PathMode pathMode = 1;

		// Token: 0x04000024 RID: 36
		public AxisConstraint lockRotation;

		// Token: 0x04000025 RID: 37
		public bool assignForwardAndUp;

		// Token: 0x04000026 RID: 38
		public Vector3 forwardDirection = Vector3.forward;

		// Token: 0x04000027 RID: 39
		public Vector3 upDirection = Vector3.up;

		// Token: 0x04000028 RID: 40
		public bool tweenRigidbody;

		// Token: 0x04000029 RID: 41
		public List<Vector3> wps = new List<Vector3>();

		// Token: 0x0400002A RID: 42
		public List<Vector3> fullWps = new List<Vector3>();

		// Token: 0x0400002B RID: 43
		public Path path;

		// Token: 0x0400002C RID: 44
		public DOTweenInspectorMode inspectorMode;

		// Token: 0x0400002D RID: 45
		public PathType pathType;

		// Token: 0x0400002E RID: 46
		public HandlesType handlesType;

		// Token: 0x0400002F RID: 47
		public bool livePreview = true;

		// Token: 0x04000030 RID: 48
		public HandlesDrawMode handlesDrawMode;

		// Token: 0x04000031 RID: 49
		public float perspectiveHandleSize = 0.5f;

		// Token: 0x04000032 RID: 50
		public bool showIndexes = true;

		// Token: 0x04000033 RID: 51
		public bool showWpLength;

		// Token: 0x04000034 RID: 52
		public Color pathColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04000035 RID: 53
		public Vector3 lastSrcPosition;

		// Token: 0x04000036 RID: 54
		public Quaternion lastSrcRotation;

		// Token: 0x04000037 RID: 55
		public bool wpsDropdown;

		// Token: 0x04000038 RID: 56
		public float dropToFloorOffset;

		// Token: 0x04000039 RID: 57
		private static MethodInfo _miCreateTween;
	}
}
