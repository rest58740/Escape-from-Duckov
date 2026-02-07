using System;
using System.Collections.Generic;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x02000056 RID: 86
	internal static class TweenManager
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000FE40 File Offset: 0x0000E040
		static TweenManager()
		{
			TweenManager.isUnityEditor = Application.isEditor;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		internal static TweenerCore<T1, T2, TPlugOptions> GetTweener<T1, T2, TPlugOptions>() where TPlugOptions : struct, IPlugOptions
		{
			if (TweenManager.totPooledTweeners > 0)
			{
				Type typeFromHandle = typeof(T1);
				Type typeFromHandle2 = typeof(T2);
				Type typeFromHandle3 = typeof(TPlugOptions);
				for (int i = TweenManager._maxPooledTweenerId; i > TweenManager._minPooledTweenerId - 1; i--)
				{
					Tween tween = TweenManager._pooledTweeners[i];
					if (tween != null && tween.typeofT1 == typeFromHandle && tween.typeofT2 == typeFromHandle2 && tween.typeofTPlugOptions == typeFromHandle3)
					{
						TweenerCore<T1, T2, TPlugOptions> tweenerCore = (TweenerCore<T1, T2, TPlugOptions>)tween;
						TweenManager.AddActiveTween(tweenerCore);
						TweenManager._pooledTweeners[i] = null;
						if (TweenManager._maxPooledTweenerId != TweenManager._minPooledTweenerId)
						{
							if (i == TweenManager._maxPooledTweenerId)
							{
								TweenManager._maxPooledTweenerId--;
							}
							else if (i == TweenManager._minPooledTweenerId)
							{
								TweenManager._minPooledTweenerId++;
							}
						}
						TweenManager.totPooledTweeners--;
						return tweenerCore;
					}
				}
				if (TweenManager.totTweeners >= TweenManager.maxTweeners)
				{
					TweenManager._pooledTweeners[TweenManager._maxPooledTweenerId] = null;
					TweenManager._maxPooledTweenerId--;
					TweenManager.totPooledTweeners--;
					TweenManager.totTweeners--;
				}
			}
			else if (TweenManager.totTweeners >= TweenManager.maxTweeners - 1)
			{
				int num = TweenManager.maxTweeners;
				int num2 = TweenManager.maxSequences;
				TweenManager.IncreaseCapacities(TweenManager.CapacityIncreaseMode.TweenersOnly);
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup".Replace("#0", num.ToString() + "/" + num2.ToString()).Replace("#1", TweenManager.maxTweeners.ToString() + "/" + TweenManager.maxSequences.ToString()), null);
				}
			}
			TweenerCore<T1, T2, TPlugOptions> tweenerCore2 = new TweenerCore<T1, T2, TPlugOptions>();
			TweenManager.totTweeners++;
			TweenManager.AddActiveTween(tweenerCore2);
			return tweenerCore2;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00010080 File Offset: 0x0000E280
		internal static Sequence GetSequence()
		{
			if (TweenManager.totPooledSequences > 0)
			{
				Sequence sequence = (Sequence)TweenManager._PooledSequences.Pop();
				TweenManager.AddActiveTween(sequence);
				TweenManager.totPooledSequences--;
				return sequence;
			}
			if (TweenManager.totSequences >= TweenManager.maxSequences - 1)
			{
				int num = TweenManager.maxTweeners;
				int num2 = TweenManager.maxSequences;
				TweenManager.IncreaseCapacities(TweenManager.CapacityIncreaseMode.SequencesOnly);
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup".Replace("#0", num.ToString() + "/" + num2.ToString()).Replace("#1", TweenManager.maxTweeners.ToString() + "/" + TweenManager.maxSequences.ToString()), null);
				}
			}
			Sequence sequence2 = new Sequence();
			TweenManager.totSequences++;
			TweenManager.AddActiveTween(sequence2);
			return sequence2;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0001014C File Offset: 0x0000E34C
		internal static void SetUpdateType(Tween t, UpdateType updateType, bool isIndependentUpdate)
		{
			if (!t.active || t.updateType == updateType)
			{
				t.updateType = updateType;
				t.isIndependentUpdate = isIndependentUpdate;
				return;
			}
			if (t.updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens--;
				TweenManager.hasActiveDefaultTweens = (TweenManager.totActiveDefaultTweens > 0);
			}
			else
			{
				UpdateType updateType2 = t.updateType;
				if (updateType2 != UpdateType.Late)
				{
					if (updateType2 == UpdateType.Fixed)
					{
						TweenManager.totActiveFixedTweens--;
						TweenManager.hasActiveFixedTweens = (TweenManager.totActiveFixedTweens > 0);
					}
					else
					{
						TweenManager.totActiveManualTweens--;
						TweenManager.hasActiveManualTweens = (TweenManager.totActiveManualTweens > 0);
					}
				}
				else
				{
					TweenManager.totActiveLateTweens--;
					TweenManager.hasActiveLateTweens = (TweenManager.totActiveLateTweens > 0);
				}
			}
			t.updateType = updateType;
			t.isIndependentUpdate = isIndependentUpdate;
			if (updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens++;
				TweenManager.hasActiveDefaultTweens = true;
				return;
			}
			if (updateType == UpdateType.Late)
			{
				TweenManager.totActiveLateTweens++;
				TweenManager.hasActiveLateTweens = true;
				return;
			}
			if (updateType == UpdateType.Fixed)
			{
				TweenManager.totActiveFixedTweens++;
				TweenManager.hasActiveFixedTweens = true;
				return;
			}
			TweenManager.totActiveManualTweens++;
			TweenManager.hasActiveManualTweens = true;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0001025E File Offset: 0x0000E45E
		internal static void AddActiveTweenToSequence(Tween t)
		{
			TweenManager.RemoveActiveTween(t);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00010268 File Offset: 0x0000E468
		internal static int DespawnAll()
		{
			int result = TweenManager.totActiveTweens;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null)
				{
					TweenManager.Despawn(tween, false);
				}
			}
			TweenManager.ClearTweenArray(TweenManager._activeTweens);
			TweenManager.hasActiveTweens = (TweenManager.hasActiveDefaultTweens = (TweenManager.hasActiveLateTweens = (TweenManager.hasActiveFixedTweens = (TweenManager.hasActiveManualTweens = false))));
			TweenManager.totActiveTweens = (TweenManager.totActiveDefaultTweens = (TweenManager.totActiveLateTweens = (TweenManager.totActiveFixedTweens = (TweenManager.totActiveManualTweens = 0))));
			TweenManager.totActiveTweeners = (TweenManager.totActiveSequences = 0);
			TweenManager._maxActiveLookupId = (TweenManager._reorganizeFromId = -1);
			TweenManager._requiresActiveReorganization = false;
			TweenManager._TweenLinks.Clear();
			TweenManager._totTweenLinks = 0;
			if (TweenManager.isUpdateLoop)
			{
				TweenManager._despawnAllCalledFromUpdateLoopCallback = true;
			}
			return result;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00010324 File Offset: 0x0000E524
		internal static void Despawn(Tween t, bool modifyActiveLists = true)
		{
			if (t.onKill != null)
			{
				Tween.OnTweenCallback(t.onKill, t);
			}
			if (modifyActiveLists)
			{
				TweenManager.RemoveActiveTween(t);
			}
			if (t.isRecyclable)
			{
				TweenType tweenType = t.tweenType;
				if (tweenType != TweenType.Tweener)
				{
					if (tweenType == TweenType.Sequence)
					{
						TweenManager._PooledSequences.Push(t);
						TweenManager.totPooledSequences++;
						Sequence sequence = (Sequence)t;
						int count = sequence.sequencedTweens.Count;
						for (int i = 0; i < count; i++)
						{
							TweenManager.Despawn(sequence.sequencedTweens[i], false);
						}
					}
				}
				else
				{
					if (TweenManager._maxPooledTweenerId == -1)
					{
						TweenManager._maxPooledTweenerId = TweenManager.maxTweeners - 1;
						TweenManager._minPooledTweenerId = TweenManager.maxTweeners - 1;
					}
					if (TweenManager._maxPooledTweenerId < TweenManager.maxTweeners - 1)
					{
						TweenManager._pooledTweeners[TweenManager._maxPooledTweenerId + 1] = t;
						TweenManager._maxPooledTweenerId++;
						if (TweenManager._minPooledTweenerId > TweenManager._maxPooledTweenerId)
						{
							TweenManager._minPooledTweenerId = TweenManager._maxPooledTweenerId;
						}
					}
					else
					{
						int j = TweenManager._maxPooledTweenerId;
						while (j > -1)
						{
							if (TweenManager._pooledTweeners[j] == null)
							{
								TweenManager._pooledTweeners[j] = t;
								if (j < TweenManager._minPooledTweenerId)
								{
									TweenManager._minPooledTweenerId = j;
								}
								if (TweenManager._maxPooledTweenerId < TweenManager._minPooledTweenerId)
								{
									TweenManager._maxPooledTweenerId = TweenManager._minPooledTweenerId;
									break;
								}
								break;
							}
							else
							{
								j--;
							}
						}
					}
					TweenManager.totPooledTweeners++;
				}
			}
			else
			{
				TweenType tweenType = t.tweenType;
				if (tweenType != TweenType.Tweener)
				{
					if (tweenType == TweenType.Sequence)
					{
						TweenManager.totSequences--;
						Sequence sequence2 = (Sequence)t;
						int count2 = sequence2.sequencedTweens.Count;
						for (int k = 0; k < count2; k++)
						{
							TweenManager.Despawn(sequence2.sequencedTweens[k], false);
						}
					}
				}
				else
				{
					TweenManager.totTweeners--;
				}
			}
			t.active = false;
			t.Reset();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000104E8 File Offset: 0x0000E6E8
		internal static void PurgeAll(bool isApplicationQuitting)
		{
			if (!isApplicationQuitting)
			{
				for (int i = 0; i < TweenManager.maxActive; i++)
				{
					Tween tween = TweenManager._activeTweens[i];
					if (tween != null && tween.active)
					{
						tween.active = false;
						if (tween.onKill != null)
						{
							Tween.OnTweenCallback(tween.onKill, tween);
						}
					}
				}
			}
			TweenManager.ClearTweenArray(TweenManager._activeTweens);
			TweenManager.hasActiveTweens = (TweenManager.hasActiveDefaultTweens = (TweenManager.hasActiveLateTweens = (TweenManager.hasActiveFixedTweens = (TweenManager.hasActiveManualTweens = false))));
			TweenManager.totActiveTweens = (TweenManager.totActiveDefaultTweens = (TweenManager.totActiveLateTweens = (TweenManager.totActiveFixedTweens = (TweenManager.totActiveManualTweens = 0))));
			TweenManager.totActiveTweeners = (TweenManager.totActiveSequences = 0);
			TweenManager._maxActiveLookupId = (TweenManager._reorganizeFromId = -1);
			TweenManager._requiresActiveReorganization = false;
			TweenManager.PurgePools();
			TweenManager.ResetCapacities();
			TweenManager.totTweeners = (TweenManager.totSequences = 0);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000105B4 File Offset: 0x0000E7B4
		internal static void PurgePools()
		{
			TweenManager.totTweeners -= TweenManager.totPooledTweeners;
			TweenManager.totSequences -= TweenManager.totPooledSequences;
			TweenManager.ClearTweenArray(TweenManager._pooledTweeners);
			TweenManager._PooledSequences.Clear();
			TweenManager.totPooledTweeners = (TweenManager.totPooledSequences = 0);
			TweenManager._minPooledTweenerId = (TweenManager._maxPooledTweenerId = -1);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00010610 File Offset: 0x0000E810
		internal static void AddTweenLink(Tween t, TweenLink tweenLink)
		{
			TweenManager._totTweenLinks++;
			if (TweenManager._TweenLinks.ContainsKey(t))
			{
				TweenManager._TweenLinks[t] = tweenLink;
			}
			else
			{
				TweenManager._TweenLinks.Add(t, tweenLink);
			}
			if (tweenLink.lastSeenActive)
			{
				LinkBehaviour behaviour = tweenLink.behaviour;
				if (behaviour - LinkBehaviour.PauseOnDisablePlayOnEnable <= 3)
				{
					TweenManager.Play(t);
					return;
				}
			}
			else
			{
				LinkBehaviour behaviour = tweenLink.behaviour;
				if (behaviour <= LinkBehaviour.PauseOnDisableRestartOnEnable)
				{
					TweenManager.Pause(t);
				}
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0001067F File Offset: 0x0000E87F
		private static void RemoveTweenLink(Tween t)
		{
			if (!TweenManager._TweenLinks.ContainsKey(t))
			{
				return;
			}
			TweenManager._TweenLinks.Remove(t);
			TweenManager._totTweenLinks--;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000106A7 File Offset: 0x0000E8A7
		internal static void ResetCapacities()
		{
			TweenManager.SetCapacities(200, 50);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000106B8 File Offset: 0x0000E8B8
		internal static void SetCapacities(int tweenersCapacity, int sequencesCapacity)
		{
			if (tweenersCapacity < sequencesCapacity)
			{
				tweenersCapacity = sequencesCapacity;
			}
			TweenManager.maxActive = tweenersCapacity + sequencesCapacity;
			TweenManager.maxTweeners = tweenersCapacity;
			TweenManager.maxSequences = sequencesCapacity;
			Array.Resize<Tween>(ref TweenManager._activeTweens, TweenManager.maxActive);
			Array.Resize<Tween>(ref TweenManager._pooledTweeners, tweenersCapacity);
			TweenManager._KillList.Capacity = TweenManager.maxActive;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0001070C File Offset: 0x0000E90C
		internal static int Validate()
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			int num = 0;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (!tween.Validate())
				{
					num++;
					TweenManager.MarkForKilling(tween, false);
				}
			}
			if (num > 0)
			{
				TweenManager.DespawnActiveTweens(TweenManager._KillList);
				TweenManager._KillList.Clear();
			}
			return num;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00010770 File Offset: 0x0000E970
		internal static void Update(UpdateType updateType, float deltaTime, float independentTime)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			TweenManager.isUpdateLoop = true;
			bool flag = false;
			int num = TweenManager._maxActiveLookupId + 1;
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.updateType == updateType && TweenManager.Update(tween, deltaTime, independentTime, false))
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (TweenManager._despawnAllCalledFromUpdateLoopCallback)
				{
					TweenManager._despawnAllCalledFromUpdateLoopCallback = false;
				}
				else
				{
					TweenManager.DespawnActiveTweens(TweenManager._KillList);
				}
				TweenManager._KillList.Clear();
			}
			TweenManager.isUpdateLoop = false;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000107F4 File Offset: 0x0000E9F4
		internal static bool Update(Tween t, float deltaTime, float independentTime, bool isSingleTweenManualUpdate)
		{
			if (TweenManager._totTweenLinks > 0)
			{
				TweenManager.EvaluateTweenLink(t);
			}
			if (!t.active)
			{
				TweenManager.MarkForKilling(t, isSingleTweenManualUpdate);
				return true;
			}
			if (!t.isPlaying)
			{
				return false;
			}
			t.creationLocked = true;
			float num = (t.isIndependentUpdate ? independentTime : deltaTime) * t.timeScale;
			if (num < 1E-06f && num > -1E-06f)
			{
				return false;
			}
			if (!t.delayComplete)
			{
				num = t.UpdateDelay(t.elapsedDelay + num);
				if (num <= -1f)
				{
					TweenManager.MarkForKilling(t, isSingleTweenManualUpdate);
					return true;
				}
				if (num <= 0f)
				{
					return false;
				}
				if (t.playedOnce && t.onPlay != null)
				{
					Tween.OnTweenCallback(t.onPlay, t);
				}
			}
			if (!t.startupDone && !t.Startup())
			{
				TweenManager.MarkForKilling(t, isSingleTweenManualUpdate);
				return true;
			}
			float num2 = t.position;
			bool flag = num2 >= t.duration;
			int num3 = t.completedLoops;
			if (t.duration <= 0f)
			{
				num2 = 0f;
				num3 = ((t.loops == -1) ? (t.completedLoops + 1) : t.loops);
			}
			else
			{
				if (t.isBackwards)
				{
					num2 -= num;
					while (num2 < 0f && num3 > -1)
					{
						num2 += t.duration;
						num3--;
					}
					if (num3 < 0 || (flag && num3 < 1))
					{
						num2 = 0f;
						num3 = (flag ? 1 : 0);
					}
				}
				else
				{
					num2 += num;
					while (num2 >= t.duration && (t.loops == -1 || num3 < t.loops))
					{
						num2 -= t.duration;
						num3++;
					}
				}
				if (flag)
				{
					num3--;
				}
				if (t.loops != -1 && num3 >= t.loops)
				{
					num2 = t.duration;
				}
			}
			if (Tween.DoGoto(t, num2, num3, UpdateMode.Update))
			{
				TweenManager.MarkForKilling(t, isSingleTweenManualUpdate);
				return true;
			}
			return false;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000109B4 File Offset: 0x0000EBB4
		internal static int FilteredOperation(OperationType operationType, FilterType filterType, object id, bool optionalBool, float optionalFloat, object optionalObj = null, object[] optionalArray = null)
		{
			int num = 0;
			bool flag = false;
			int num2 = (optionalArray == null) ? 0 : optionalArray.Length;
			bool flag2 = false;
			string b = null;
			bool flag3 = false;
			int num3 = 0;
			if (filterType - FilterType.TargetOrId <= 1)
			{
				if (id is string)
				{
					flag2 = true;
					b = (string)id;
				}
				else if (id is int)
				{
					flag3 = true;
					num3 = (int)id;
				}
			}
			for (int i = TweenManager._maxActiveLookupId; i > -1; i--)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.active)
				{
					bool flag4 = false;
					switch (filterType)
					{
					case FilterType.All:
						flag4 = true;
						break;
					case FilterType.TargetOrId:
						if (flag2)
						{
							flag4 = (tween.stringId != null && tween.stringId == b);
						}
						else if (flag3)
						{
							flag4 = (tween.intId == num3);
						}
						else
						{
							flag4 = ((tween.id != null && id.Equals(tween.id)) || (tween.target != null && id.Equals(tween.target)));
						}
						break;
					case FilterType.TargetAndId:
						if (flag2)
						{
							flag4 = (tween.target != null && tween.stringId == b && optionalObj != null && optionalObj.Equals(tween.target));
						}
						else if (flag3)
						{
							flag4 = (tween.target != null && tween.intId == num3 && optionalObj != null && optionalObj.Equals(tween.target));
						}
						else
						{
							flag4 = (tween.id != null && tween.target != null && optionalObj != null && id.Equals(tween.id) && optionalObj.Equals(tween.target));
						}
						break;
					case FilterType.AllExceptTargetsOrIds:
						flag4 = true;
						for (int j = 0; j < num2; j++)
						{
							object obj = optionalArray[j];
							if (obj is string)
							{
								flag2 = true;
								b = (string)obj;
							}
							else if (obj is int)
							{
								flag3 = true;
								num3 = (int)obj;
							}
							if (flag2 && tween.stringId == b)
							{
								flag4 = false;
								break;
							}
							if (flag3 && tween.intId == num3)
							{
								flag4 = false;
								break;
							}
							if ((tween.id != null && obj.Equals(tween.id)) || (tween.target != null && obj.Equals(tween.target)))
							{
								flag4 = false;
								break;
							}
						}
						break;
					}
					if (flag4)
					{
						switch (operationType)
						{
						case OperationType.Complete:
						{
							bool autoKill = tween.autoKill;
							if (!tween.startupDone)
							{
								TweenManager.ForceInit(tween, false);
							}
							if (TweenManager.Complete(tween, false, (optionalFloat > 0f) ? UpdateMode.Update : UpdateMode.Goto))
							{
								num += ((!optionalBool) ? 1 : (autoKill ? 1 : 0));
								if (autoKill)
								{
									if (TweenManager.isUpdateLoop)
									{
										tween.active = false;
									}
									else
									{
										flag = true;
										TweenManager._KillList.Add(tween);
									}
								}
							}
							break;
						}
						case OperationType.Despawn:
							num++;
							tween.active = false;
							if (!TweenManager.isUpdateLoop)
							{
								TweenManager.Despawn(tween, false);
								flag = true;
								TweenManager._KillList.Add(tween);
							}
							break;
						case OperationType.Flip:
							if (TweenManager.Flip(tween))
							{
								num++;
							}
							break;
						case OperationType.Goto:
							if (!tween.startupDone)
							{
								TweenManager.ForceInit(tween, false);
							}
							TweenManager.Goto(tween, optionalFloat, optionalBool, UpdateMode.Goto);
							num++;
							break;
						case OperationType.Pause:
							if (TweenManager.Pause(tween))
							{
								num++;
							}
							break;
						case OperationType.Play:
							if (TweenManager.Play(tween))
							{
								num++;
							}
							break;
						case OperationType.PlayForward:
							if (TweenManager.PlayForward(tween))
							{
								num++;
							}
							break;
						case OperationType.PlayBackwards:
							if (TweenManager.PlayBackwards(tween))
							{
								num++;
							}
							break;
						case OperationType.Rewind:
							if (TweenManager.Rewind(tween, optionalBool))
							{
								num++;
							}
							break;
						case OperationType.SmoothRewind:
							if (TweenManager.SmoothRewind(tween))
							{
								num++;
							}
							break;
						case OperationType.Restart:
							if (TweenManager.Restart(tween, optionalBool, optionalFloat))
							{
								num++;
							}
							break;
						case OperationType.TogglePause:
							if (TweenManager.TogglePause(tween))
							{
								num++;
							}
							break;
						case OperationType.IsTweening:
							if ((!tween.isComplete || !tween.autoKill) && (!optionalBool || tween.isPlaying))
							{
								num++;
							}
							break;
						}
					}
				}
			}
			if (flag)
			{
				for (int k = TweenManager._KillList.Count - 1; k > -1; k--)
				{
					Tween tween2 = TweenManager._KillList[k];
					if (tween2.activeId != -1)
					{
						TweenManager.RemoveActiveTween(tween2);
					}
				}
				TweenManager._KillList.Clear();
			}
			return num;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00010E4C File Offset: 0x0000F04C
		internal static bool Complete(Tween t, bool modifyActiveLists = true, UpdateMode updateMode = UpdateMode.Goto)
		{
			if (t.loops == -1)
			{
				return false;
			}
			if (!t.isComplete)
			{
				Tween.DoGoto(t, t.duration, t.loops, updateMode);
				t.isPlaying = false;
				if (t.autoKill)
				{
					if (TweenManager.isUpdateLoop)
					{
						t.active = false;
					}
					else
					{
						TweenManager.Despawn(t, modifyActiveLists);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00010EA9 File Offset: 0x0000F0A9
		internal static bool Flip(Tween t)
		{
			t.isBackwards = !t.isBackwards;
			return true;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010EBB File Offset: 0x0000F0BB
		internal static void ForceInit(Tween t, bool isSequenced = false)
		{
			if (t.startupDone)
			{
				return;
			}
			if (!t.Startup() && !isSequenced)
			{
				if (TweenManager.isUpdateLoop)
				{
					t.active = false;
					return;
				}
				TweenManager.RemoveActiveTween(t);
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00010EE8 File Offset: 0x0000F0E8
		internal static bool Goto(Tween t, float to, bool andPlay = false, UpdateMode updateMode = UpdateMode.Goto)
		{
			bool isPlaying = t.isPlaying;
			t.isPlaying = andPlay;
			t.delayComplete = true;
			t.elapsedDelay = t.delay;
			int num = (t.duration <= 0f) ? 1 : Mathf.FloorToInt(to / t.duration);
			float num2 = to % t.duration;
			if (t.loops != -1 && num >= t.loops)
			{
				num = t.loops;
				num2 = t.duration;
			}
			else if (num2 >= t.duration)
			{
				num2 = 0f;
			}
			bool flag = Tween.DoGoto(t, num2, num, updateMode);
			if (!andPlay && isPlaying && !flag && t.onPause != null)
			{
				Tween.OnTweenCallback(t.onPause, t);
			}
			return flag;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00010F99 File Offset: 0x0000F199
		internal static bool Pause(Tween t)
		{
			if (t.isPlaying)
			{
				t.isPlaying = false;
				if (t.onPause != null)
				{
					Tween.OnTweenCallback(t.onPause, t);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00010FC4 File Offset: 0x0000F1C4
		internal static bool Play(Tween t)
		{
			if (!t.isPlaying && ((!t.isBackwards && !t.isComplete) || (t.isBackwards && (t.completedLoops > 0 || t.position > 0f))))
			{
				t.isPlaying = true;
				if (t.playedOnce && t.delayComplete && t.onPlay != null)
				{
					Tween.OnTweenCallback(t.onPlay, t);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00011038 File Offset: 0x0000F238
		internal static bool PlayBackwards(Tween t)
		{
			if (t.completedLoops == 0 && t.position <= 0f)
			{
				TweenManager.ManageOnRewindCallbackWhenAlreadyRewinded(t, true);
				t.isBackwards = true;
				t.isPlaying = false;
				return false;
			}
			if (!t.isBackwards)
			{
				t.isBackwards = true;
				TweenManager.Play(t);
				return true;
			}
			return TweenManager.Play(t);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0001108F File Offset: 0x0000F28F
		internal static bool PlayForward(Tween t)
		{
			if (t.isComplete)
			{
				t.isBackwards = false;
				t.isPlaying = false;
				return false;
			}
			if (t.isBackwards)
			{
				t.isBackwards = false;
				TweenManager.Play(t);
				return true;
			}
			return TweenManager.Play(t);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000110C8 File Offset: 0x0000F2C8
		internal static bool Restart(Tween t, bool includeDelay = true, float changeDelayTo = -1f)
		{
			bool flag = !t.isPlaying;
			t.isBackwards = false;
			if (changeDelayTo >= 0f && t.tweenType == TweenType.Tweener)
			{
				t.delay = changeDelayTo;
			}
			TweenManager.Rewind(t, includeDelay);
			t.isPlaying = true;
			if (flag && t.playedOnce && t.delayComplete && t.onPlay != null)
			{
				Tween.OnTweenCallback(t.onPlay, t);
			}
			return true;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00011134 File Offset: 0x0000F334
		internal static bool Rewind(Tween t, bool includeDelay = true)
		{
			bool isPlaying = t.isPlaying;
			t.isPlaying = false;
			bool result = false;
			if (t.delay > 0f)
			{
				if (includeDelay)
				{
					result = (t.delay > 0f && t.elapsedDelay > 0f);
					t.elapsedDelay = 0f;
					t.delayComplete = false;
				}
				else
				{
					result = (t.elapsedDelay < t.delay);
					t.elapsedDelay = t.delay;
					t.delayComplete = true;
				}
			}
			if (t.position > 0f || t.completedLoops > 0 || !t.startupDone)
			{
				result = true;
				if (!Tween.DoGoto(t, 0f, 0, UpdateMode.Goto) && isPlaying && t.onPause != null)
				{
					Tween.OnTweenCallback(t.onPause, t);
				}
			}
			else
			{
				TweenManager.ManageOnRewindCallbackWhenAlreadyRewinded(t, false);
			}
			return result;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00011208 File Offset: 0x0000F408
		internal static bool SmoothRewind(Tween t)
		{
			bool result = false;
			if (t.delay > 0f)
			{
				result = (t.elapsedDelay < t.delay);
				t.elapsedDelay = t.delay;
				t.delayComplete = true;
			}
			if (t.position > 0f || t.completedLoops > 0 || !t.startupDone)
			{
				result = true;
				if (t.loopType == LoopType.Incremental)
				{
					t.PlayBackwards();
				}
				else
				{
					t.Goto(t.ElapsedDirectionalPercentage() * t.duration, false);
					t.PlayBackwards();
				}
			}
			else
			{
				t.isPlaying = false;
				TweenManager.ManageOnRewindCallbackWhenAlreadyRewinded(t, true);
			}
			return result;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000112A2 File Offset: 0x0000F4A2
		internal static bool TogglePause(Tween t)
		{
			if (t.isPlaying)
			{
				return TweenManager.Pause(t);
			}
			return TweenManager.Play(t);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000112B9 File Offset: 0x0000F4B9
		internal static int TotalPooledTweens()
		{
			return TweenManager.totPooledTweeners + TweenManager.totPooledSequences;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000112C8 File Offset: 0x0000F4C8
		internal static int TotalPlayingTweens()
		{
			if (!TweenManager.hasActiveTweens)
			{
				return 0;
			}
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			int num = 0;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.isPlaying)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00011318 File Offset: 0x0000F518
		internal static List<Tween> GetActiveTweens(bool playing, List<Tween> fillableList = null)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			if (fillableList == null)
			{
				fillableList = new List<Tween>(num);
			}
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween.isPlaying == playing)
				{
					fillableList.Add(tween);
				}
			}
			if (fillableList.Count > 0)
			{
				return fillableList;
			}
			return null;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0001137C File Offset: 0x0000F57C
		internal static List<Tween> GetTweensById(object id, bool playingOnly, List<Tween> fillableList = null)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			if (fillableList == null)
			{
				fillableList = new List<Tween>(num);
			}
			bool flag = false;
			string b = null;
			bool flag2 = false;
			int num2 = 0;
			if (id is string)
			{
				flag = true;
				b = (string)id;
			}
			else if (id is int)
			{
				flag2 = true;
				num2 = (int)id;
			}
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null)
				{
					if (flag)
					{
						if (tween.stringId == null)
						{
							goto IL_C1;
						}
						if (tween.stringId != b)
						{
							goto IL_C1;
						}
					}
					else if (flag2)
					{
						if (tween.intId != num2)
						{
							goto IL_C1;
						}
					}
					else if (tween.id == null || !object.Equals(id, tween.id))
					{
						goto IL_C1;
					}
					if (!playingOnly || tween.isPlaying)
					{
						fillableList.Add(tween);
					}
				}
				IL_C1:;
			}
			if (fillableList.Count > 0)
			{
				return fillableList;
			}
			return null;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00011464 File Offset: 0x0000F664
		internal static List<Tween> GetTweensByTarget(object target, bool playingOnly, List<Tween> fillableList = null)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			if (fillableList == null)
			{
				fillableList = new List<Tween>(num);
			}
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween.target == target && (!playingOnly || tween.isPlaying))
				{
					fillableList.Add(tween);
				}
			}
			if (fillableList.Count > 0)
			{
				return fillableList;
			}
			return null;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000114D3 File Offset: 0x0000F6D3
		private static void MarkForKilling(Tween t, bool isSingleTweenManualUpdate = false)
		{
			if (isSingleTweenManualUpdate && !TweenManager.isUpdateLoop)
			{
				TweenManager.Despawn(t, true);
				return;
			}
			t.active = false;
			TweenManager._KillList.Add(t);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000114FC File Offset: 0x0000F6FC
		private static void EvaluateTweenLink(Tween t)
		{
			TweenLink tweenLink;
			if (!TweenManager._TweenLinks.TryGetValue(t, out tweenLink))
			{
				return;
			}
			if (tweenLink.target == null)
			{
				t.active = false;
				return;
			}
			bool activeInHierarchy = tweenLink.target.activeInHierarchy;
			bool flag = !tweenLink.lastSeenActive && activeInHierarchy;
			bool flag2 = tweenLink.lastSeenActive && !activeInHierarchy;
			tweenLink.lastSeenActive = activeInHierarchy;
			switch (tweenLink.behaviour)
			{
			case LinkBehaviour.PauseOnDisable:
				if (flag2 && t.isPlaying)
				{
					TweenManager.Pause(t);
					return;
				}
				break;
			case LinkBehaviour.PauseOnDisablePlayOnEnable:
				if (flag2)
				{
					TweenManager.Pause(t);
					return;
				}
				if (flag)
				{
					TweenManager.Play(t);
					return;
				}
				break;
			case LinkBehaviour.PauseOnDisableRestartOnEnable:
				if (flag2)
				{
					TweenManager.Pause(t);
					return;
				}
				if (flag)
				{
					TweenManager.Restart(t, true, -1f);
					return;
				}
				break;
			case LinkBehaviour.PlayOnEnable:
				if (flag)
				{
					TweenManager.Play(t);
					return;
				}
				break;
			case LinkBehaviour.RestartOnEnable:
				if (flag)
				{
					TweenManager.Restart(t, true, -1f);
				}
				break;
			case LinkBehaviour.KillOnDisable:
				if (!activeInHierarchy)
				{
					t.active = false;
					return;
				}
				break;
			case LinkBehaviour.KillOnDestroy:
				break;
			case LinkBehaviour.CompleteOnDisable:
				if (flag2 && !t.isComplete)
				{
					t.Complete();
					return;
				}
				break;
			case LinkBehaviour.CompleteAndKillOnDisable:
				if (!activeInHierarchy)
				{
					if (!t.isComplete)
					{
						t.Complete();
					}
					t.active = false;
					return;
				}
				break;
			case LinkBehaviour.RewindOnDisable:
				if (flag2)
				{
					t.Rewind(false);
					return;
				}
				break;
			case LinkBehaviour.RewindAndKillOnDisable:
				if (!activeInHierarchy)
				{
					t.Rewind(false);
					t.active = false;
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00011658 File Offset: 0x0000F858
		private static void AddActiveTween(Tween t)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens < 0)
			{
				Debugger.LogAddActiveTweenError("totActiveTweens < 0", t);
				TweenManager.totActiveTweens = 0;
			}
			t.active = true;
			t.updateType = DOTween.defaultUpdateType;
			t.isIndependentUpdate = DOTween.defaultTimeScaleIndependent;
			t.activeId = (TweenManager._maxActiveLookupId = TweenManager.totActiveTweens);
			TweenManager._activeTweens[TweenManager.totActiveTweens] = t;
			if (t.updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens++;
				TweenManager.hasActiveDefaultTweens = true;
			}
			else
			{
				UpdateType updateType = t.updateType;
				if (updateType != UpdateType.Late)
				{
					if (updateType == UpdateType.Fixed)
					{
						TweenManager.totActiveFixedTweens++;
						TweenManager.hasActiveFixedTweens = true;
					}
					else
					{
						TweenManager.totActiveManualTweens++;
						TweenManager.hasActiveManualTweens = true;
					}
				}
				else
				{
					TweenManager.totActiveLateTweens++;
					TweenManager.hasActiveLateTweens = true;
				}
			}
			TweenManager.totActiveTweens++;
			if (t.tweenType == TweenType.Tweener)
			{
				TweenManager.totActiveTweeners++;
			}
			else
			{
				TweenManager.totActiveSequences++;
			}
			TweenManager.hasActiveTweens = true;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00011760 File Offset: 0x0000F960
		private static void ReorganizeActiveTweens()
		{
			if (TweenManager.totActiveTweens <= 0)
			{
				TweenManager._maxActiveLookupId = -1;
				TweenManager._requiresActiveReorganization = false;
				TweenManager._reorganizeFromId = -1;
				return;
			}
			if (TweenManager._reorganizeFromId == TweenManager._maxActiveLookupId)
			{
				TweenManager._maxActiveLookupId--;
				TweenManager._requiresActiveReorganization = false;
				TweenManager._reorganizeFromId = -1;
				return;
			}
			int num = 1;
			int num2 = TweenManager._maxActiveLookupId + 1;
			TweenManager._maxActiveLookupId = TweenManager._reorganizeFromId - 1;
			for (int i = TweenManager._reorganizeFromId + 1; i < num2; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween == null)
				{
					num++;
				}
				else
				{
					tween.activeId = (TweenManager._maxActiveLookupId = i - num);
					TweenManager._activeTweens[i - num] = tween;
					TweenManager._activeTweens[i] = null;
				}
			}
			TweenManager._requiresActiveReorganization = false;
			TweenManager._reorganizeFromId = -1;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00011814 File Offset: 0x0000FA14
		private static void DespawnActiveTweens(List<Tween> tweens)
		{
			for (int i = tweens.Count - 1; i > -1; i--)
			{
				TweenManager.Despawn(tweens[i], true);
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00011844 File Offset: 0x0000FA44
		private static void RemoveActiveTween(Tween t)
		{
			int activeId = t.activeId;
			if (TweenManager._totTweenLinks > 0)
			{
				TweenManager.RemoveTweenLink(t);
			}
			t.activeId = -1;
			TweenManager._requiresActiveReorganization = true;
			if (TweenManager._reorganizeFromId == -1 || TweenManager._reorganizeFromId > activeId)
			{
				TweenManager._reorganizeFromId = activeId;
			}
			TweenManager._activeTweens[activeId] = null;
			if (t.updateType == UpdateType.Normal)
			{
				if (TweenManager.totActiveDefaultTweens > 0)
				{
					TweenManager.totActiveDefaultTweens--;
					TweenManager.hasActiveDefaultTweens = (TweenManager.totActiveDefaultTweens > 0);
				}
				else
				{
					Debugger.LogRemoveActiveTweenError("totActiveDefaultTweens < 0", t);
				}
			}
			else
			{
				UpdateType updateType = t.updateType;
				if (updateType != UpdateType.Late)
				{
					if (updateType == UpdateType.Fixed)
					{
						if (TweenManager.totActiveFixedTweens > 0)
						{
							TweenManager.totActiveFixedTweens--;
							TweenManager.hasActiveFixedTweens = (TweenManager.totActiveFixedTweens > 0);
						}
						else
						{
							Debugger.LogRemoveActiveTweenError("totActiveFixedTweens < 0", t);
						}
					}
					else if (TweenManager.totActiveManualTweens > 0)
					{
						TweenManager.totActiveManualTweens--;
						TweenManager.hasActiveManualTweens = (TweenManager.totActiveManualTweens > 0);
					}
					else
					{
						Debugger.LogRemoveActiveTweenError("totActiveManualTweens < 0", t);
					}
				}
				else if (TweenManager.totActiveLateTweens > 0)
				{
					TweenManager.totActiveLateTweens--;
					TweenManager.hasActiveLateTweens = (TweenManager.totActiveLateTweens > 0);
				}
				else
				{
					Debugger.LogRemoveActiveTweenError("totActiveLateTweens < 0", t);
				}
			}
			TweenManager.totActiveTweens--;
			TweenManager.hasActiveTweens = (TweenManager.totActiveTweens > 0);
			if (t.tweenType == TweenType.Tweener)
			{
				TweenManager.totActiveTweeners--;
			}
			else
			{
				TweenManager.totActiveSequences--;
			}
			if (TweenManager.totActiveTweens < 0)
			{
				TweenManager.totActiveTweens = 0;
				Debugger.LogRemoveActiveTweenError("totActiveTweens < 0", t);
			}
			if (TweenManager.totActiveTweeners < 0)
			{
				TweenManager.totActiveTweeners = 0;
				Debugger.LogRemoveActiveTweenError("totActiveTweeners < 0", t);
			}
			if (TweenManager.totActiveSequences < 0)
			{
				TweenManager.totActiveSequences = 0;
				Debugger.LogRemoveActiveTweenError("totActiveSequences < 0", t);
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000119F4 File Offset: 0x0000FBF4
		private static void ClearTweenArray(Tween[] tweens)
		{
			int num = tweens.Length;
			for (int i = 0; i < num; i++)
			{
				tweens[i] = null;
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00011A18 File Offset: 0x0000FC18
		private static void IncreaseCapacities(TweenManager.CapacityIncreaseMode increaseMode)
		{
			int num = 0;
			int num2 = Mathf.Max((int)((float)TweenManager.maxTweeners * 1.5f), 200);
			int num3 = Mathf.Max((int)((float)TweenManager.maxSequences * 1.5f), 50);
			if (increaseMode != TweenManager.CapacityIncreaseMode.TweenersOnly)
			{
				if (increaseMode != TweenManager.CapacityIncreaseMode.SequencesOnly)
				{
					num += num2 + num3;
					TweenManager.maxTweeners += num2;
					TweenManager.maxSequences += num3;
					Array.Resize<Tween>(ref TweenManager._pooledTweeners, TweenManager.maxTweeners);
				}
				else
				{
					num += num3;
					TweenManager.maxSequences += num3;
				}
			}
			else
			{
				num += num2;
				TweenManager.maxTweeners += num2;
				Array.Resize<Tween>(ref TweenManager._pooledTweeners, TweenManager.maxTweeners);
			}
			TweenManager.maxActive = TweenManager.maxTweeners + TweenManager.maxSequences;
			Array.Resize<Tween>(ref TweenManager._activeTweens, TweenManager.maxActive);
			if (num > 0)
			{
				TweenManager._KillList.Capacity += num;
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00011AF3 File Offset: 0x0000FCF3
		private static void ManageOnRewindCallbackWhenAlreadyRewinded(Tween t, bool isPlayBackwardsOrSmoothRewind)
		{
			if (t.onRewind == null)
			{
				return;
			}
			if (isPlayBackwardsOrSmoothRewind)
			{
				if (DOTween.rewindCallbackMode == RewindCallbackMode.FireAlways)
				{
					t.onRewind();
					return;
				}
			}
			else if (DOTween.rewindCallbackMode != RewindCallbackMode.FireIfPositionChanged)
			{
				t.onRewind();
			}
		}

		// Token: 0x04000174 RID: 372
		private const int _DefaultMaxTweeners = 200;

		// Token: 0x04000175 RID: 373
		private const int _DefaultMaxSequences = 50;

		// Token: 0x04000176 RID: 374
		private const string _MaxTweensReached = "Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup";

		// Token: 0x04000177 RID: 375
		private const float _EpsilonVsTimeCheck = 1E-06f;

		// Token: 0x04000178 RID: 376
		internal static bool isUnityEditor;

		// Token: 0x04000179 RID: 377
		internal static bool isDebugBuild;

		// Token: 0x0400017A RID: 378
		internal static int maxActive = 250;

		// Token: 0x0400017B RID: 379
		internal static int maxTweeners = 200;

		// Token: 0x0400017C RID: 380
		internal static int maxSequences = 50;

		// Token: 0x0400017D RID: 381
		internal static bool hasActiveTweens;

		// Token: 0x0400017E RID: 382
		internal static bool hasActiveDefaultTweens;

		// Token: 0x0400017F RID: 383
		internal static bool hasActiveLateTweens;

		// Token: 0x04000180 RID: 384
		internal static bool hasActiveFixedTweens;

		// Token: 0x04000181 RID: 385
		internal static bool hasActiveManualTweens;

		// Token: 0x04000182 RID: 386
		internal static int totActiveTweens;

		// Token: 0x04000183 RID: 387
		internal static int totActiveDefaultTweens;

		// Token: 0x04000184 RID: 388
		internal static int totActiveLateTweens;

		// Token: 0x04000185 RID: 389
		internal static int totActiveFixedTweens;

		// Token: 0x04000186 RID: 390
		internal static int totActiveManualTweens;

		// Token: 0x04000187 RID: 391
		internal static int totActiveTweeners;

		// Token: 0x04000188 RID: 392
		internal static int totActiveSequences;

		// Token: 0x04000189 RID: 393
		internal static int totPooledTweeners;

		// Token: 0x0400018A RID: 394
		internal static int totPooledSequences;

		// Token: 0x0400018B RID: 395
		internal static int totTweeners;

		// Token: 0x0400018C RID: 396
		internal static int totSequences;

		// Token: 0x0400018D RID: 397
		internal static bool isUpdateLoop;

		// Token: 0x0400018E RID: 398
		internal static Tween[] _activeTweens = new Tween[250];

		// Token: 0x0400018F RID: 399
		private static Tween[] _pooledTweeners = new Tween[200];

		// Token: 0x04000190 RID: 400
		private static readonly Stack<Tween> _PooledSequences = new Stack<Tween>();

		// Token: 0x04000191 RID: 401
		private static readonly List<Tween> _KillList = new List<Tween>(250);

		// Token: 0x04000192 RID: 402
		private static readonly Dictionary<Tween, TweenLink> _TweenLinks = new Dictionary<Tween, TweenLink>(250);

		// Token: 0x04000193 RID: 403
		private static int _totTweenLinks;

		// Token: 0x04000194 RID: 404
		private static int _maxActiveLookupId = -1;

		// Token: 0x04000195 RID: 405
		private static bool _requiresActiveReorganization;

		// Token: 0x04000196 RID: 406
		private static int _reorganizeFromId = -1;

		// Token: 0x04000197 RID: 407
		private static int _minPooledTweenerId = -1;

		// Token: 0x04000198 RID: 408
		private static int _maxPooledTweenerId = -1;

		// Token: 0x04000199 RID: 409
		private static bool _despawnAllCalledFromUpdateLoopCallback;

		// Token: 0x020000C4 RID: 196
		internal enum CapacityIncreaseMode
		{
			// Token: 0x04000281 RID: 641
			TweenersAndSequences,
			// Token: 0x04000282 RID: 642
			TweenersOnly,
			// Token: 0x04000283 RID: 643
			SequencesOnly
		}
	}
}
