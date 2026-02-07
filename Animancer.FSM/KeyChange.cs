using System;

namespace Animancer.FSM
{
	// Token: 0x02000008 RID: 8
	public struct KeyChange<TKey> : IDisposable
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002192 File Offset: 0x00000392
		public static bool IsActive
		{
			get
			{
				return KeyChange<TKey>._Current._StateMachine != null;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000021A1 File Offset: 0x000003A1
		public static IKeyedStateMachine<TKey> StateMachine
		{
			get
			{
				return KeyChange<TKey>._Current._StateMachine;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000021AD File Offset: 0x000003AD
		public static TKey PreviousKey
		{
			get
			{
				return KeyChange<TKey>._Current._PreviousKey;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000021B9 File Offset: 0x000003B9
		public static TKey NextKey
		{
			get
			{
				return KeyChange<TKey>._Current._NextKey;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000021C5 File Offset: 0x000003C5
		internal KeyChange(IKeyedStateMachine<TKey> stateMachine, TKey previousKey, TKey nextKey)
		{
			this = KeyChange<TKey>._Current;
			KeyChange<TKey>._Current._StateMachine = stateMachine;
			KeyChange<TKey>._Current._PreviousKey = previousKey;
			KeyChange<TKey>._Current._NextKey = nextKey;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000021F3 File Offset: 0x000003F3
		public void Dispose()
		{
			KeyChange<TKey>._Current = this;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002200 File Offset: 0x00000400
		public override string ToString()
		{
			if (!KeyChange<TKey>.IsActive)
			{
				return "KeyChange<" + typeof(TKey).FullName + "(Not Currently Active)";
			}
			return "KeyChange<" + typeof(TKey).FullName + string.Format(">({0}={1}", "PreviousKey", KeyChange<TKey>.PreviousKey) + string.Format(", {0}={1})", "NextKey", KeyChange<TKey>.NextKey);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000227E File Offset: 0x0000047E
		public static string CurrentToString()
		{
			return KeyChange<TKey>._Current.ToString();
		}

		// Token: 0x04000006 RID: 6
		[ThreadStatic]
		private static KeyChange<TKey> _Current;

		// Token: 0x04000007 RID: 7
		private IKeyedStateMachine<TKey> _StateMachine;

		// Token: 0x04000008 RID: 8
		private TKey _PreviousKey;

		// Token: 0x04000009 RID: 9
		private TKey _NextKey;
	}
}
