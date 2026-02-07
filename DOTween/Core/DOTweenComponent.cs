using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x0200004F RID: 79
	[AddComponentMenu("")]
	public class DOTweenComponent : MonoBehaviour, IDOTweenInit
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000F75C File Offset: 0x0000D95C
		private void Awake()
		{
			if (!(DOTween.instance == null))
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("Duplicate DOTweenComponent instance found in scene: destroying it", null);
				}
				Object.Destroy(base.gameObject);
				return;
			}
			DOTween.instance = this;
			this.inspectorUpdater = 0;
			this._unscaledTime = Time.realtimeSinceStartup;
			Type looseScriptType = DOTweenUtils.GetLooseScriptType("DG.Tweening.DOTweenModuleUtils");
			if (looseScriptType == null)
			{
				Debugger.LogError("Couldn't load Modules system", null);
				return;
			}
			looseScriptType.GetMethod("Init", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000F7DE File Offset: 0x0000D9DE
		private void Start()
		{
			if (DOTween.instance != this)
			{
				this._duplicateToDestroy = true;
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000F800 File Offset: 0x0000DA00
		private void Update()
		{
			this._unscaledDeltaTime = Time.realtimeSinceStartup - this._unscaledTime;
			if (DOTween.useSmoothDeltaTime && this._unscaledDeltaTime > DOTween.maxSmoothUnscaledTime)
			{
				this._unscaledDeltaTime = DOTween.maxSmoothUnscaledTime;
			}
			if (TweenManager.hasActiveDefaultTweens)
			{
				TweenManager.Update(UpdateType.Normal, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, this._unscaledDeltaTime * DOTween.timeScale);
			}
			this._unscaledTime = Time.realtimeSinceStartup;
			if (TweenManager.isUnityEditor)
			{
				this.inspectorUpdater++;
				if (DOTween.showUnityEditorReport && TweenManager.hasActiveTweens)
				{
					if (TweenManager.totActiveTweeners > DOTween.maxActiveTweenersReached)
					{
						DOTween.maxActiveTweenersReached = TweenManager.totActiveTweeners;
					}
					if (TweenManager.totActiveSequences > DOTween.maxActiveSequencesReached)
					{
						DOTween.maxActiveSequencesReached = TweenManager.totActiveSequences;
					}
				}
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000F8CA File Offset: 0x0000DACA
		private void LateUpdate()
		{
			if (TweenManager.hasActiveLateTweens)
			{
				TweenManager.Update(UpdateType.Late, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, this._unscaledDeltaTime * DOTween.timeScale);
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000F900 File Offset: 0x0000DB00
		private void FixedUpdate()
		{
			if (TweenManager.hasActiveFixedTweens && Time.timeScale > 0f)
			{
				TweenManager.Update(UpdateType.Fixed, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) / Time.timeScale * DOTween.timeScale);
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000F960 File Offset: 0x0000DB60
		private void OnDrawGizmos()
		{
			if (!DOTween.drawGizmos || !TweenManager.isUnityEditor)
			{
				return;
			}
			int count = DOTween.GizmosDelegates.Count;
			if (count == 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				DOTween.GizmosDelegates[i]();
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000F9A8 File Offset: 0x0000DBA8
		private void OnDestroy()
		{
			if (this._duplicateToDestroy)
			{
				return;
			}
			if (DOTween.showUnityEditorReport)
			{
				Debugger.LogReport("Max overall simultaneous active Tweeners/Sequences: " + DOTween.maxActiveTweenersReached.ToString() + "/" + DOTween.maxActiveSequencesReached.ToString());
			}
			if (DOTween.useSafeMode)
			{
				int totErrors = DOTween.safeModeReport.GetTotErrors();
				if (totErrors > 0)
				{
					string text = string.Format("DOTween's safe mode captured {0} errors. This is usually ok (it's what safe mode is there for) but if your game is encountering issues you should set Log Behaviour to Default in DOTween Utility Panel in order to get detailed warnings when an error is captured (consider that these errors are always on the user side).", totErrors);
					if (DOTween.safeModeReport.totMissingTargetOrFieldErrors > 0)
					{
						text = text + "\n- " + DOTween.safeModeReport.totMissingTargetOrFieldErrors.ToString() + " missing target or field errors";
					}
					if (DOTween.safeModeReport.totStartupErrors > 0)
					{
						text = text + "\n- " + DOTween.safeModeReport.totStartupErrors.ToString() + " startup errors";
					}
					if (DOTween.safeModeReport.totCallbackErrors > 0)
					{
						text = text + "\n- " + DOTween.safeModeReport.totCallbackErrors.ToString() + " errors inside callbacks (these might be important)";
					}
					if (DOTween.safeModeReport.totUnsetErrors > 0)
					{
						text = text + "\n- " + DOTween.safeModeReport.totUnsetErrors.ToString() + " undetermined errors (these might be important)";
					}
					Debugger.LogSafeModeReport(text);
				}
			}
			if (DOTween.instance == this)
			{
				DOTween.instance = null;
			}
			DOTween.Clear(true, this._isQuitting);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000FAFF File Offset: 0x0000DCFF
		public void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				this._paused = true;
				this._pausedTime = Time.realtimeSinceStartup;
				return;
			}
			if (this._paused)
			{
				this._paused = false;
				this._unscaledTime += Time.realtimeSinceStartup - this._pausedTime;
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000FB3F File Offset: 0x0000DD3F
		private void OnApplicationQuit()
		{
			this._isQuitting = true;
			DOTween.isQuitting = true;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000FB4E File Offset: 0x0000DD4E
		public IDOTweenInit SetCapacity(int tweenersCapacity, int sequencesCapacity)
		{
			TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
			return this;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000FB58 File Offset: 0x0000DD58
		internal IEnumerator WaitForCompletion(Tween t)
		{
			while (t.active && !t.isComplete)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000FB67 File Offset: 0x0000DD67
		internal IEnumerator WaitForRewind(Tween t)
		{
			while (t.active && (!t.playedOnce || t.position * (float)(t.completedLoops + 1) > 0f))
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000FB76 File Offset: 0x0000DD76
		internal IEnumerator WaitForKill(Tween t)
		{
			while (t.active)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000FB85 File Offset: 0x0000DD85
		internal IEnumerator WaitForElapsedLoops(Tween t, int elapsedLoops)
		{
			while (t.active && t.completedLoops < elapsedLoops)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000FB9B File Offset: 0x0000DD9B
		internal IEnumerator WaitForPosition(Tween t, float position)
		{
			while (t.active && t.position * (float)(t.completedLoops + 1) < position)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000FBB1 File Offset: 0x0000DDB1
		internal IEnumerator WaitForStart(Tween t)
		{
			while (t.active && !t.playedOnce)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
		internal static void Create()
		{
			if (DOTween.instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[DOTween]");
			Object.DontDestroyOnLoad(gameObject);
			DOTween.instance = gameObject.AddComponent<DOTweenComponent>();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000FBEA File Offset: 0x0000DDEA
		internal static void DestroyInstance()
		{
			if (DOTween.instance != null)
			{
				Object.Destroy(DOTween.instance.gameObject);
			}
			DOTween.instance = null;
		}

		// Token: 0x04000149 RID: 329
		public int inspectorUpdater;

		// Token: 0x0400014A RID: 330
		private float _unscaledTime;

		// Token: 0x0400014B RID: 331
		private float _unscaledDeltaTime;

		// Token: 0x0400014C RID: 332
		private bool _paused;

		// Token: 0x0400014D RID: 333
		private float _pausedTime;

		// Token: 0x0400014E RID: 334
		private bool _isQuitting;

		// Token: 0x0400014F RID: 335
		private bool _duplicateToDestroy;
	}
}
