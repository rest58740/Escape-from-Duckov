using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Animancer
{
	// Token: 0x02000011 RID: 17
	public struct AnimancerEvent : IEquatable<AnimancerEvent>
	{
		// Token: 0x06000169 RID: 361 RVA: 0x00004440 File Offset: 0x00002640
		private static void Dummy()
		{
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00004442 File Offset: 0x00002642
		public static bool IsNullOrDummy(Action callback)
		{
			return callback == null || callback == AnimancerEvent.DummyCallback;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00004454 File Offset: 0x00002654
		public AnimancerEvent(float normalizedTime, Action callback)
		{
			this.normalizedTime = normalizedTime;
			this.callback = callback;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00004464 File Offset: 0x00002664
		public override string ToString()
		{
			StringBuilder stringBuilder = ObjectPool.AcquireStringBuilder();
			stringBuilder.Append("AnimancerEvent(");
			this.AppendDetails(stringBuilder);
			stringBuilder.Append(')');
			return stringBuilder.ReleaseToString();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000449C File Offset: 0x0000269C
		public void AppendDetails(StringBuilder text)
		{
			text.Append("NormalizedTime: ").Append(this.normalizedTime).Append(", Callback: ");
			if (this.callback == null)
			{
				text.Append("null");
				return;
			}
			if (this.callback.Target == null)
			{
				text.Append(this.callback.Method.DeclaringType.FullName).Append('.').Append(this.callback.Method.Name);
				return;
			}
			text.Append("(Target: '").Append(this.callback.Target).Append("', Method: ").Append(this.callback.Method.DeclaringType.FullName).Append('.').Append(this.callback.Method.Name).Append(')');
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00004588 File Offset: 0x00002788
		public static AnimancerState CurrentState
		{
			get
			{
				return AnimancerEvent._CurrentState;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000458F File Offset: 0x0000278F
		public static ref readonly AnimancerEvent CurrentEvent
		{
			get
			{
				return ref AnimancerEvent._CurrentEvent;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00004598 File Offset: 0x00002798
		public void Invoke(AnimancerState state)
		{
			AnimancerState currentState = AnimancerEvent._CurrentState;
			AnimancerEvent currentEvent = AnimancerEvent._CurrentEvent;
			AnimancerEvent._CurrentState = state;
			AnimancerEvent._CurrentEvent = this;
			try
			{
				this.callback();
			}
			catch (Exception exception)
			{
				object obj;
				if (state == null)
				{
					obj = null;
				}
				else
				{
					AnimancerPlayable root = state.Root;
					obj = ((root != null) ? root.Component : null);
				}
				UnityEngine.Debug.LogException(exception, obj as UnityEngine.Object);
			}
			AnimancerEvent._CurrentState = currentState;
			AnimancerEvent._CurrentEvent = currentEvent;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00004610 File Offset: 0x00002810
		public static float GetFadeOutDuration()
		{
			return AnimancerEvent.GetFadeOutDuration(AnimancerEvent.CurrentState, AnimancerPlayable.DefaultFadeDuration);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00004621 File Offset: 0x00002821
		public static float GetFadeOutDuration(float minDuration)
		{
			return AnimancerEvent.GetFadeOutDuration(AnimancerEvent.CurrentState, minDuration);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00004630 File Offset: 0x00002830
		public static float GetFadeOutDuration(AnimancerState state, float minDuration)
		{
			if (state == null)
			{
				return minDuration;
			}
			float time = state.Time;
			float effectiveSpeed = state.EffectiveSpeed;
			if (effectiveSpeed == 0f)
			{
				return minDuration;
			}
			if (state.IsLooping)
			{
				float num = time - effectiveSpeed * Time.deltaTime;
				float num2 = 1f / state.Length;
				if (Math.Floor((double)(time * num2)) != Math.Floor((double)(num * num2)))
				{
					return minDuration;
				}
			}
			float val;
			if (effectiveSpeed > 0f)
			{
				val = (state.Length - time) / effectiveSpeed;
			}
			else
			{
				val = time / -effectiveSpeed;
			}
			return Math.Max(minDuration, val);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000046B3 File Offset: 0x000028B3
		public static bool operator ==(AnimancerEvent a, AnimancerEvent b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000046BD File Offset: 0x000028BD
		public static bool operator !=(AnimancerEvent a, AnimancerEvent b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000046CC File Offset: 0x000028CC
		public bool Equals(AnimancerEvent other)
		{
			return this.callback == other.callback && (this.normalizedTime == other.normalizedTime || (float.IsNaN(this.normalizedTime) && float.IsNaN(other.normalizedTime)));
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00004718 File Offset: 0x00002918
		public override bool Equals(object obj)
		{
			if (obj is AnimancerEvent)
			{
				AnimancerEvent other = (AnimancerEvent)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00004740 File Offset: 0x00002940
		public override int GetHashCode()
		{
			int num = -78069441;
			num = num * -1521134295 + this.normalizedTime.GetHashCode();
			if (this.callback != null)
			{
				num = num * -1521134295 + this.callback.GetHashCode();
			}
			return num;
		}

		// Token: 0x0400001F RID: 31
		public float normalizedTime;

		// Token: 0x04000020 RID: 32
		public Action callback;

		// Token: 0x04000021 RID: 33
		public const float AlmostOne = 0.99999994f;

		// Token: 0x04000022 RID: 34
		public static readonly Action DummyCallback = new Action(AnimancerEvent.Dummy);

		// Token: 0x04000023 RID: 35
		private static AnimancerState _CurrentState;

		// Token: 0x04000024 RID: 36
		private static AnimancerEvent _CurrentEvent;

		// Token: 0x02000085 RID: 133
		public class Sequence : IEnumerable<AnimancerEvent>, IEnumerable, ICopyable<AnimancerEvent.Sequence>
		{
			// Token: 0x17000177 RID: 375
			// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0000F516 File Offset: 0x0000D716
			// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0000F51E File Offset: 0x0000D71E
			public int Count { get; private set; }

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x060005CA RID: 1482 RVA: 0x0000F527 File Offset: 0x0000D727
			public bool IsEmpty
			{
				get
				{
					return this._EndEvent.callback == null && float.IsNaN(this._EndEvent.normalizedTime) && this.Count == 0;
				}
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x060005CB RID: 1483 RVA: 0x0000F553 File Offset: 0x0000D753
			// (set) Token: 0x060005CC RID: 1484 RVA: 0x0000F560 File Offset: 0x0000D760
			public int Capacity
			{
				get
				{
					return this._Events.Length;
				}
				set
				{
					if (value < this.Count)
					{
						throw new ArgumentOutOfRangeException("value", "Capacity cannot be set lower than Count");
					}
					if (value == this._Events.Length)
					{
						return;
					}
					if (value > 0)
					{
						AnimancerEvent[] array = new AnimancerEvent[value];
						if (this.Count > 0)
						{
							Array.Copy(this._Events, 0, array, 0, this.Count);
						}
						this._Events = array;
						return;
					}
					this._Events = Array.Empty<AnimancerEvent>();
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x060005CD RID: 1485 RVA: 0x0000F5CD File Offset: 0x0000D7CD
			// (set) Token: 0x060005CE RID: 1486 RVA: 0x0000F5D5 File Offset: 0x0000D7D5
			public int Version
			{
				get
				{
					return this._Version;
				}
				private set
				{
					this._Version = value;
				}
			}

			// Token: 0x060005CF RID: 1487 RVA: 0x0000F5DE File Offset: 0x0000D7DE
			[Conditional("UNITY_ASSERTIONS")]
			public void SetShouldNotModifyReason(string reason)
			{
			}

			// Token: 0x060005D0 RID: 1488 RVA: 0x0000F5E0 File Offset: 0x0000D7E0
			[Conditional("UNITY_ASSERTIONS")]
			public void OnSequenceModified()
			{
			}

			// Token: 0x1700017B RID: 379
			// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0000F5E2 File Offset: 0x0000D7E2
			// (set) Token: 0x060005D2 RID: 1490 RVA: 0x0000F5EA File Offset: 0x0000D7EA
			public AnimancerEvent EndEvent
			{
				get
				{
					return this._EndEvent;
				}
				set
				{
					this._EndEvent = value;
				}
			}

			// Token: 0x1700017C RID: 380
			// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0000F5F3 File Offset: 0x0000D7F3
			// (set) Token: 0x060005D4 RID: 1492 RVA: 0x0000F600 File Offset: 0x0000D800
			public Action OnEnd
			{
				get
				{
					return this._EndEvent.callback;
				}
				set
				{
					this._EndEvent.callback = value;
				}
			}

			// Token: 0x1700017D RID: 381
			// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0000F60E File Offset: 0x0000D80E
			// (set) Token: 0x060005D6 RID: 1494 RVA: 0x0000F61B File Offset: 0x0000D81B
			public float NormalizedEndTime
			{
				get
				{
					return this._EndEvent.normalizedTime;
				}
				set
				{
					this._EndEvent.normalizedTime = value;
				}
			}

			// Token: 0x060005D7 RID: 1495 RVA: 0x0000F629 File Offset: 0x0000D829
			public static float GetDefaultNormalizedStartTime(float speed)
			{
				return (float)((speed < 0f) ? 1 : 0);
			}

			// Token: 0x060005D8 RID: 1496 RVA: 0x0000F638 File Offset: 0x0000D838
			public static float GetDefaultNormalizedEndTime(float speed)
			{
				return (float)((speed < 0f) ? 0 : 1);
			}

			// Token: 0x1700017E RID: 382
			// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0000F647 File Offset: 0x0000D847
			public ref string[] Names
			{
				get
				{
					return ref this._Names;
				}
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x0000F64F File Offset: 0x0000D84F
			public string GetName(int index)
			{
				if (this._Names == null || this._Names.Length <= index)
				{
					return null;
				}
				return this._Names[index];
			}

			// Token: 0x060005DB RID: 1499 RVA: 0x0000F670 File Offset: 0x0000D870
			public void SetName(int index, string name)
			{
				if (this._Names == null)
				{
					this._Names = new string[this.Capacity];
				}
				else if (this._Names.Length <= index)
				{
					string[] array = new string[this.Capacity];
					Array.Copy(this._Names, array, this._Names.Length);
					this._Names = array;
				}
				this._Names[index] = name;
			}

			// Token: 0x060005DC RID: 1500 RVA: 0x0000F6D4 File Offset: 0x0000D8D4
			public int IndexOf(string name, int startIndex = 0)
			{
				if (this._Names == null)
				{
					return -1;
				}
				int num = Mathf.Min(this.Count, this._Names.Length);
				while (startIndex < num)
				{
					if (this._Names[startIndex] == name)
					{
						return startIndex;
					}
					startIndex++;
				}
				return -1;
			}

			// Token: 0x060005DD RID: 1501 RVA: 0x0000F71D File Offset: 0x0000D91D
			public int IndexOfRequired(string name, int startIndex = 0)
			{
				startIndex = this.IndexOf(name, startIndex);
				if (startIndex >= 0)
				{
					return startIndex;
				}
				throw new ArgumentException("No event exists with the name '" + name + "'.");
			}

			// Token: 0x060005DE RID: 1502 RVA: 0x0000F744 File Offset: 0x0000D944
			public Sequence()
			{
				this._Events = Array.Empty<AnimancerEvent>();
			}

			// Token: 0x060005DF RID: 1503 RVA: 0x0000F768 File Offset: 0x0000D968
			public Sequence(int capacity)
			{
				this._Events = ((capacity > 0) ? new AnimancerEvent[capacity] : Array.Empty<AnimancerEvent>());
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x0000F798 File Offset: 0x0000D998
			public Sequence(AnimancerEvent.Sequence copyFrom)
			{
				this._Events = Array.Empty<AnimancerEvent>();
				if (copyFrom != null)
				{
					this.CopyFrom(copyFrom);
				}
			}

			// Token: 0x1700017F RID: 383
			public AnimancerEvent this[int index]
			{
				get
				{
					return this._Events[index];
				}
			}

			// Token: 0x17000180 RID: 384
			public AnimancerEvent this[string name]
			{
				get
				{
					return this[this.IndexOfRequired(name, 0)];
				}
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x0000F7E4 File Offset: 0x0000D9E4
			[Conditional("UNITY_ASSERTIONS")]
			public void AssertNormalizedTimes(AnimancerState state)
			{
				if (this.Count == 0 || (this._Events[0].normalizedTime >= 0f && this._Events[this.Count - 1].normalizedTime < 1f))
				{
					return;
				}
				throw new ArgumentOutOfRangeException("normalizedTime", "Events on looping animations are triggered every loop and must be" + string.Format(" within the range of 0 <= {0} < 1.\n{1}\n{2}", "normalizedTime", state, this.DeepToString(true)));
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x0000F85C File Offset: 0x0000DA5C
			[Conditional("UNITY_ASSERTIONS")]
			public void AssertNormalizedTimes(AnimancerState state, bool isLooping)
			{
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x0000F860 File Offset: 0x0000DA60
			public string DeepToString(bool multiLine = true)
			{
				StringBuilder stringBuilder = ObjectPool.AcquireStringBuilder().Append(this.ToString()).Append('[').Append(this.Count).Append(']');
				stringBuilder.Append(multiLine ? "\n{" : " {");
				for (int i = 0; i < this.Count; i++)
				{
					if (multiLine)
					{
						stringBuilder.Append("\n   ");
					}
					else if (i > 0)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(" [");
					stringBuilder.Append(i).Append("] ");
					this[i].AppendDetails(stringBuilder);
					string name = this.GetName(i);
					if (name != null)
					{
						stringBuilder.Append(", Name: '").Append(name).Append('\'');
					}
				}
				if (multiLine)
				{
					stringBuilder.Append("\n    [End] ");
				}
				else
				{
					if (this.Count > 0)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(" [End] ");
				}
				this._EndEvent.AppendDetails(stringBuilder);
				if (multiLine)
				{
					stringBuilder.Append("\n}\n");
				}
				else
				{
					stringBuilder.Append(" }");
				}
				return stringBuilder.ReleaseToString();
			}

			// Token: 0x060005E6 RID: 1510 RVA: 0x0000F98C File Offset: 0x0000DB8C
			public FastEnumerator<AnimancerEvent> GetEnumerator()
			{
				return new FastEnumerator<AnimancerEvent>(this._Events, this.Count);
			}

			// Token: 0x060005E7 RID: 1511 RVA: 0x0000F99F File Offset: 0x0000DB9F
			IEnumerator<AnimancerEvent> IEnumerable<AnimancerEvent>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060005E8 RID: 1512 RVA: 0x0000F9AC File Offset: 0x0000DBAC
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060005E9 RID: 1513 RVA: 0x0000F9B9 File Offset: 0x0000DBB9
			public int IndexOf(AnimancerEvent animancerEvent)
			{
				return this.IndexOf(this.Count / 2, animancerEvent);
			}

			// Token: 0x060005EA RID: 1514 RVA: 0x0000F9CA File Offset: 0x0000DBCA
			public int IndexOfRequired(AnimancerEvent animancerEvent)
			{
				return this.IndexOfRequired(this.Count / 2, animancerEvent);
			}

			// Token: 0x060005EB RID: 1515 RVA: 0x0000F9DC File Offset: 0x0000DBDC
			public int IndexOf(int indexHint, AnimancerEvent animancerEvent)
			{
				if (this.Count == 0)
				{
					return -1;
				}
				if (indexHint >= this.Count)
				{
					indexHint = this.Count - 1;
				}
				AnimancerEvent animancerEvent2 = this._Events[indexHint];
				if (animancerEvent2 == animancerEvent)
				{
					return indexHint;
				}
				if (animancerEvent2.normalizedTime <= animancerEvent.normalizedTime)
				{
					while (animancerEvent2.normalizedTime == animancerEvent.normalizedTime)
					{
						indexHint--;
						if (indexHint < 0)
						{
							IL_F6:
							while (++indexHint < this.Count)
							{
								animancerEvent2 = this._Events[indexHint];
								if (animancerEvent2.normalizedTime > animancerEvent.normalizedTime)
								{
									return -1;
								}
								if (animancerEvent2.normalizedTime == animancerEvent.normalizedTime && animancerEvent2.callback == animancerEvent.callback)
								{
									return indexHint;
								}
							}
							return -1;
						}
						animancerEvent2 = this._Events[indexHint];
					}
					goto IL_F6;
				}
				while (--indexHint >= 0)
				{
					animancerEvent2 = this._Events[indexHint];
					if (animancerEvent2.normalizedTime < animancerEvent.normalizedTime)
					{
						return -1;
					}
					if (animancerEvent2.normalizedTime == animancerEvent.normalizedTime && animancerEvent2.callback == animancerEvent.callback)
					{
						return indexHint;
					}
				}
				return -1;
			}

			// Token: 0x060005EC RID: 1516 RVA: 0x0000FAEE File Offset: 0x0000DCEE
			public int IndexOfRequired(int indexHint, AnimancerEvent animancerEvent)
			{
				indexHint = this.IndexOf(indexHint, animancerEvent);
				if (indexHint >= 0)
				{
					return indexHint;
				}
				throw new ArgumentException(string.Format("Event not found in {0} '{1}'.", "Sequence", animancerEvent));
			}

			// Token: 0x060005ED RID: 1517 RVA: 0x0000FB1C File Offset: 0x0000DD1C
			public int Add(AnimancerEvent animancerEvent)
			{
				int num = this.Insert(animancerEvent.normalizedTime);
				this._Events[num] = animancerEvent;
				return num;
			}

			// Token: 0x060005EE RID: 1518 RVA: 0x0000FB44 File Offset: 0x0000DD44
			public int Add(float normalizedTime, Action callback)
			{
				return this.Add(new AnimancerEvent(normalizedTime, callback));
			}

			// Token: 0x060005EF RID: 1519 RVA: 0x0000FB53 File Offset: 0x0000DD53
			public int Add(int indexHint, AnimancerEvent animancerEvent)
			{
				indexHint = this.Insert(indexHint, animancerEvent.normalizedTime);
				this._Events[indexHint] = animancerEvent;
				return indexHint;
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x0000FB72 File Offset: 0x0000DD72
			public int Add(int indexHint, float normalizedTime, Action callback)
			{
				return this.Add(indexHint, new AnimancerEvent(normalizedTime, callback));
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x0000FB84 File Offset: 0x0000DD84
			public void AddRange(IEnumerable<AnimancerEvent> enumerable)
			{
				foreach (AnimancerEvent animancerEvent in enumerable)
				{
					this.Add(animancerEvent);
				}
			}

			// Token: 0x060005F2 RID: 1522 RVA: 0x0000FBD0 File Offset: 0x0000DDD0
			public void AddCallback(int index, Action callback)
			{
				AnimancerEvent[] events = this._Events;
				events[index].callback = (Action)Delegate.Combine(events[index].callback, callback);
				int version = this.Version;
				this.Version = version + 1;
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x0000FC0C File Offset: 0x0000DE0C
			public void AddCallback(string name, Action callback)
			{
				this.AddCallback(this.IndexOfRequired(name, 0), callback);
			}

			// Token: 0x060005F4 RID: 1524 RVA: 0x0000FC20 File Offset: 0x0000DE20
			public void RemoveCallback(int index, Action callback)
			{
				ref AnimancerEvent ptr = ref this._Events[index];
				ref AnimancerEvent ptr2 = ref ptr;
				ptr2.callback = (Action)Delegate.Remove(ptr2.callback, callback);
				if (ptr.callback == null)
				{
					ptr.callback = AnimancerEvent.DummyCallback;
				}
				int version = this.Version;
				this.Version = version + 1;
			}

			// Token: 0x060005F5 RID: 1525 RVA: 0x0000FC71 File Offset: 0x0000DE71
			public void RemoveCallback(string name, Action callback)
			{
				this.RemoveCallback(this.IndexOfRequired(name, 0), callback);
			}

			// Token: 0x060005F6 RID: 1526 RVA: 0x0000FC84 File Offset: 0x0000DE84
			public void SetCallback(int index, Action callback)
			{
				this._Events[index].callback = callback;
				int version = this.Version;
				this.Version = version + 1;
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x0000FCB3 File Offset: 0x0000DEB3
			public void SetCallback(string name, Action callback)
			{
				this.SetCallback(this.IndexOfRequired(name, 0), callback);
			}

			// Token: 0x060005F8 RID: 1528 RVA: 0x0000FCC4 File Offset: 0x0000DEC4
			[Conditional("UNITY_ASSERTIONS")]
			private static void AssertCallbackUniqueness(Action oldCallback, Action newCallback, string target)
			{
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x0000FCC6 File Offset: 0x0000DEC6
			[Conditional("UNITY_ASSERTIONS")]
			private void AssertEventUniqueness(int index, AnimancerEvent newEvent)
			{
			}

			// Token: 0x060005FA RID: 1530 RVA: 0x0000FCC8 File Offset: 0x0000DEC8
			public int SetNormalizedTime(int index, float normalizedTime)
			{
				AnimancerEvent animancerEvent = this._Events[index];
				if (animancerEvent.normalizedTime == normalizedTime)
				{
					return index;
				}
				int i = index;
				if (animancerEvent.normalizedTime < normalizedTime)
				{
					while (i < this.Count - 1)
					{
						if (this._Events[i + 1].normalizedTime >= normalizedTime)
						{
							break;
						}
						i++;
					}
				}
				else
				{
					while (i > 0 && this._Events[i - 1].normalizedTime > normalizedTime)
					{
						i--;
					}
				}
				if (index != i)
				{
					string name = this.GetName(index);
					this.Remove(index);
					index = i;
					this.Insert(index);
					if (!string.IsNullOrEmpty(name))
					{
						this.SetName(index, name);
					}
				}
				animancerEvent.normalizedTime = normalizedTime;
				this._Events[index] = animancerEvent;
				int version = this.Version;
				this.Version = version + 1;
				return index;
			}

			// Token: 0x060005FB RID: 1531 RVA: 0x0000FD92 File Offset: 0x0000DF92
			public int SetNormalizedTime(string name, float normalizedTime)
			{
				return this.SetNormalizedTime(this.IndexOfRequired(name, 0), normalizedTime);
			}

			// Token: 0x060005FC RID: 1532 RVA: 0x0000FDA3 File Offset: 0x0000DFA3
			public int SetNormalizedTime(AnimancerEvent animancerEvent, float normalizedTime)
			{
				return this.SetNormalizedTime(this.IndexOfRequired(animancerEvent), normalizedTime);
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
			private int Insert(float normalizedTime)
			{
				int num = this.Count;
				while (num > 0 && this._Events[num - 1].normalizedTime > normalizedTime)
				{
					num--;
				}
				this.Insert(num);
				return num;
			}

			// Token: 0x060005FE RID: 1534 RVA: 0x0000FDF0 File Offset: 0x0000DFF0
			private int Insert(int indexHint, float normalizedTime)
			{
				if (this.Count == 0)
				{
					this.Count = 0;
				}
				else
				{
					if (indexHint >= this.Count)
					{
						indexHint = this.Count - 1;
					}
					if (this._Events[indexHint].normalizedTime > normalizedTime)
					{
						while (indexHint > 0)
						{
							if (this._Events[indexHint - 1].normalizedTime <= normalizedTime)
							{
								break;
							}
							indexHint--;
						}
					}
					else
					{
						while (indexHint < this.Count && this._Events[indexHint].normalizedTime <= normalizedTime)
						{
							indexHint++;
						}
					}
				}
				this.Insert(indexHint);
				return indexHint;
			}

			// Token: 0x060005FF RID: 1535 RVA: 0x0000FE84 File Offset: 0x0000E084
			private void Insert(int index)
			{
				int num = this._Events.Length;
				if (this.Count == num)
				{
					if (num == 0)
					{
						num = 8;
						this._Events = new AnimancerEvent[8];
					}
					else
					{
						num *= 2;
						if (num < 8)
						{
							num = 8;
						}
						AnimancerEvent[] array = new AnimancerEvent[num];
						Array.Copy(this._Events, 0, array, 0, index);
						if (this.Count > index)
						{
							Array.Copy(this._Events, index, array, index + 1, this.Count - index);
						}
						this._Events = array;
					}
				}
				else if (this.Count > index)
				{
					Array.Copy(this._Events, index, this._Events, index + 1, this.Count - index);
				}
				if (this._Names != null)
				{
					if (this._Names.Length < num)
					{
						string[] array2 = new string[num];
						Array.Copy(this._Names, 0, array2, 0, Math.Min(this._Names.Length, index));
						if (index <= this.Count && index < this._Names.Length)
						{
							Array.Copy(this._Names, index, array2, index + 1, this.Count - index);
						}
						this._Names = array2;
					}
					else
					{
						if (this.Count > index)
						{
							Array.Copy(this._Names, index, this._Names, index + 1, this.Count - index);
						}
						this._Names[index] = null;
					}
				}
				int num2 = this.Count;
				this.Count = num2 + 1;
				num2 = this.Version;
				this.Version = num2 + 1;
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x0000FFE4 File Offset: 0x0000E1E4
			public void Remove(int index)
			{
				int num = this.Count;
				this.Count = num - 1;
				if (index < this.Count)
				{
					Array.Copy(this._Events, index + 1, this._Events, index, this.Count - index);
					if (this._Names != null)
					{
						int num2 = Mathf.Min(this.Count + 1, this._Names.Length);
						if (index + 1 < num2)
						{
							Array.Copy(this._Names, index + 1, this._Names, index, num2 - index - 1);
						}
						this._Names[num2 - 1] = null;
					}
				}
				else if (this._Names != null && index < this._Names.Length)
				{
					this._Names[index] = null;
				}
				this._Events[this.Count] = default(AnimancerEvent);
				num = this.Version;
				this.Version = num + 1;
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x000100B8 File Offset: 0x0000E2B8
			public bool Remove(string name)
			{
				int num = this.IndexOf(name, 0);
				if (num >= 0)
				{
					this.Remove(num);
					return true;
				}
				return false;
			}

			// Token: 0x06000602 RID: 1538 RVA: 0x000100DC File Offset: 0x0000E2DC
			public bool Remove(AnimancerEvent animancerEvent)
			{
				int num = this.IndexOf(animancerEvent);
				if (num >= 0)
				{
					this.Remove(num);
					return true;
				}
				return false;
			}

			// Token: 0x06000603 RID: 1539 RVA: 0x00010100 File Offset: 0x0000E300
			public void Clear()
			{
				if (this._Names != null)
				{
					Array.Clear(this._Names, 0, this._Names.Length);
				}
				Array.Clear(this._Events, 0, this.Count);
				this.Count = 0;
				int version = this.Version;
				this.Version = version + 1;
				this._EndEvent = new AnimancerEvent(float.NaN, null);
			}

			// Token: 0x06000604 RID: 1540 RVA: 0x00010164 File Offset: 0x0000E364
			public void CopyFrom(AnimancerEvent.Sequence copyFrom)
			{
				if (copyFrom == null)
				{
					if (this._Names != null)
					{
						Array.Clear(this._Names, 0, this._Names.Length);
					}
					Array.Clear(this._Events, 0, this.Count);
					this.Count = 0;
					this.Capacity = 0;
					this._EndEvent = default(AnimancerEvent);
					return;
				}
				AnimancerUtilities.CopyExactArray<string>(copyFrom._Names, ref this._Names);
				int count = copyFrom.Count;
				if (this.Count > count)
				{
					Array.Clear(this._Events, this.Count, count - this.Count);
				}
				else if (this._Events.Length < count)
				{
					this.Capacity = count;
				}
				this.Count = count;
				Array.Copy(copyFrom._Events, 0, this._Events, 0, count);
				this._EndEvent = copyFrom._EndEvent;
			}

			// Token: 0x06000605 RID: 1541 RVA: 0x00010234 File Offset: 0x0000E434
			public void AddAllEvents(AnimationClip animation)
			{
				if (animation == null)
				{
					return;
				}
				float length = animation.length;
				AnimationEvent[] events = animation.events;
				this.Capacity += events.Length;
				int num = -1;
				foreach (AnimationEvent animationEvent in events)
				{
					num = this.Add(num + 1, new AnimancerEvent(animationEvent.time / length, AnimancerEvent.DummyCallback));
					this.SetName(num, animationEvent.functionName);
				}
			}

			// Token: 0x06000606 RID: 1542 RVA: 0x000102A9 File Offset: 0x0000E4A9
			public void CopyTo(AnimancerEvent[] array, int index)
			{
				Array.Copy(this._Events, 0, array, index, this.Count);
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x000102C0 File Offset: 0x0000E4C0
			public bool ContentsAreEqual(AnimancerEvent.Sequence other)
			{
				if (this._EndEvent != other._EndEvent)
				{
					return false;
				}
				if (this.Count != other.Count)
				{
					return false;
				}
				for (int i = this.Count - 1; i >= 0; i--)
				{
					if (this[i] != other[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x0400011B RID: 283
			internal const string IndexOutOfRangeError = "index must be within the range of 0 <= index < Count";

			// Token: 0x0400011C RID: 284
			private AnimancerEvent[] _Events;

			// Token: 0x0400011E RID: 286
			public const int DefaultCapacity = 8;

			// Token: 0x0400011F RID: 287
			private int _Version;

			// Token: 0x04000120 RID: 288
			private AnimancerEvent _EndEvent = new AnimancerEvent(float.NaN, null);

			// Token: 0x04000121 RID: 289
			private string[] _Names;

			// Token: 0x020000BC RID: 188
			[Serializable]
			public class Serializable : ICopyable<AnimancerEvent.Sequence.Serializable>
			{
				// Token: 0x170001A3 RID: 419
				// (get) Token: 0x06000724 RID: 1828 RVA: 0x00012896 File Offset: 0x00010A96
				public ref float[] NormalizedTimes
				{
					get
					{
						return ref this._NormalizedTimes;
					}
				}

				// Token: 0x170001A4 RID: 420
				// (get) Token: 0x06000725 RID: 1829 RVA: 0x0001289E File Offset: 0x00010A9E
				public ref UnityEvent[] Callbacks
				{
					get
					{
						return ref this._Callbacks;
					}
				}

				// Token: 0x170001A5 RID: 421
				// (get) Token: 0x06000726 RID: 1830 RVA: 0x000128A6 File Offset: 0x00010AA6
				public ref string[] Names
				{
					get
					{
						return ref this._Names;
					}
				}

				// Token: 0x170001A6 RID: 422
				// (get) Token: 0x06000727 RID: 1831 RVA: 0x000128AE File Offset: 0x00010AAE
				// (set) Token: 0x06000728 RID: 1832 RVA: 0x000128D8 File Offset: 0x00010AD8
				public AnimancerEvent.Sequence Events
				{
					get
					{
						if (this._Events == null)
						{
							this.GetEventsOptional();
							if (this._Events == null)
							{
								this._Events = new AnimancerEvent.Sequence();
							}
						}
						return this._Events;
					}
					set
					{
						this._Events = value;
					}
				}

				// Token: 0x06000729 RID: 1833 RVA: 0x000128E4 File Offset: 0x00010AE4
				public AnimancerEvent.Sequence GetEventsOptional()
				{
					if (this._Events != null || this._NormalizedTimes == null)
					{
						return this._Events;
					}
					int num = this._NormalizedTimes.Length;
					if (num == 0)
					{
						return null;
					}
					int num2 = (this._Callbacks != null) ? this._Callbacks.Length : 0;
					Action callback = (num2 >= num--) ? AnimancerEvent.Sequence.Serializable.GetInvoker(this._Callbacks[num]) : null;
					AnimancerEvent endEvent = new AnimancerEvent(this._NormalizedTimes[num], callback);
					this._Events = new AnimancerEvent.Sequence(num)
					{
						EndEvent = endEvent,
						Count = num,
						_Names = this._Names
					};
					for (int i = 0; i < num; i++)
					{
						callback = ((i < num2) ? AnimancerEvent.Sequence.Serializable.GetInvoker(this._Callbacks[i]) : AnimancerEvent.DummyCallback);
						this._Events._Events[i] = new AnimancerEvent(this._NormalizedTimes[i], callback);
					}
					return this._Events;
				}

				// Token: 0x0600072A RID: 1834 RVA: 0x000129CB File Offset: 0x00010BCB
				public static implicit operator AnimancerEvent.Sequence(AnimancerEvent.Sequence.Serializable serializable)
				{
					if (serializable == null)
					{
						return null;
					}
					return serializable.GetEventsOptional();
				}

				// Token: 0x170001A7 RID: 423
				// (get) Token: 0x0600072B RID: 1835 RVA: 0x000129D8 File Offset: 0x00010BD8
				internal AnimancerEvent.Sequence InitializedEvents
				{
					get
					{
						return this._Events;
					}
				}

				// Token: 0x0600072C RID: 1836 RVA: 0x000129E0 File Offset: 0x00010BE0
				public static Action GetInvoker(UnityEvent callback)
				{
					if (!AnimancerEvent.Sequence.Serializable.HasPersistentCalls(callback))
					{
						return AnimancerEvent.DummyCallback;
					}
					return new Action(callback.Invoke);
				}

				// Token: 0x0600072D RID: 1837 RVA: 0x000129FC File Offset: 0x00010BFC
				public static bool HasPersistentCalls(UnityEvent callback)
				{
					return callback != null && callback.GetPersistentEventCount() > 0;
				}

				// Token: 0x0600072E RID: 1838 RVA: 0x00012A0C File Offset: 0x00010C0C
				public float GetNormalizedEndTime(float speed = 1f)
				{
					if (this._NormalizedTimes.IsNullOrEmpty<float>())
					{
						return AnimancerEvent.Sequence.GetDefaultNormalizedEndTime(speed);
					}
					return this._NormalizedTimes[this._NormalizedTimes.Length - 1];
				}

				// Token: 0x0600072F RID: 1839 RVA: 0x00012A33 File Offset: 0x00010C33
				public void SetNormalizedEndTime(float normalizedTime)
				{
					if (this._NormalizedTimes.IsNullOrEmpty<float>())
					{
						this._NormalizedTimes = new float[]
						{
							normalizedTime
						};
						return;
					}
					this._NormalizedTimes[this._NormalizedTimes.Length - 1] = normalizedTime;
				}

				// Token: 0x06000730 RID: 1840 RVA: 0x00012A68 File Offset: 0x00010C68
				public void CopyFrom(AnimancerEvent.Sequence.Serializable copyFrom)
				{
					if (copyFrom == null)
					{
						this._NormalizedTimes = null;
						this._Callbacks = null;
						this._Names = null;
						return;
					}
					AnimancerUtilities.CopyExactArray<float>(copyFrom._NormalizedTimes, ref this._NormalizedTimes);
					AnimancerUtilities.CopyExactArray<UnityEvent>(copyFrom._Callbacks, ref this._Callbacks);
					AnimancerUtilities.CopyExactArray<string>(copyFrom._Names, ref this._Names);
				}

				// Token: 0x0400019A RID: 410
				[SerializeField]
				private float[] _NormalizedTimes;

				// Token: 0x0400019B RID: 411
				[SerializeField]
				private UnityEvent[] _Callbacks;

				// Token: 0x0400019C RID: 412
				[SerializeField]
				private string[] _Names;

				// Token: 0x0400019D RID: 413
				private AnimancerEvent.Sequence _Events;
			}
		}
	}
}
