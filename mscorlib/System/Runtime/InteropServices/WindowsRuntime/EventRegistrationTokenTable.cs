using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x02000789 RID: 1929
	public sealed class EventRegistrationTokenTable<T> where T : class
	{
		// Token: 0x0600448E RID: 17550 RVA: 0x000E3BC4 File Offset: 0x000E1DC4
		public EventRegistrationTokenTable()
		{
			if (!typeof(Delegate).IsAssignableFrom(typeof(T)))
			{
				throw new InvalidOperationException(Environment.GetResourceString("Type '{0}' is not a delegate type.  EventTokenTable may only be used with delegate types.", new object[]
				{
					typeof(T)
				}));
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x0600448F RID: 17551 RVA: 0x000E3C20 File Offset: 0x000E1E20
		// (set) Token: 0x06004490 RID: 17552 RVA: 0x000E3C2C File Offset: 0x000E1E2C
		public T InvocationList
		{
			get
			{
				return this.m_invokeList;
			}
			set
			{
				Dictionary<EventRegistrationToken, T> tokens = this.m_tokens;
				lock (tokens)
				{
					this.m_tokens.Clear();
					this.m_invokeList = default(T);
					if (value != null)
					{
						this.AddEventHandlerNoLock(value);
					}
				}
			}
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x000E3C94 File Offset: 0x000E1E94
		public EventRegistrationToken AddEventHandler(T handler)
		{
			if (handler == null)
			{
				return new EventRegistrationToken(0UL);
			}
			Dictionary<EventRegistrationToken, T> tokens = this.m_tokens;
			EventRegistrationToken result;
			lock (tokens)
			{
				result = this.AddEventHandlerNoLock(handler);
			}
			return result;
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x000E3CE8 File Offset: 0x000E1EE8
		private EventRegistrationToken AddEventHandlerNoLock(T handler)
		{
			EventRegistrationToken preferredToken = EventRegistrationTokenTable<T>.GetPreferredToken(handler);
			while (this.m_tokens.ContainsKey(preferredToken))
			{
				preferredToken = new EventRegistrationToken(preferredToken.Value + 1UL);
			}
			this.m_tokens[preferredToken] = handler;
			Delegate @delegate = (Delegate)((object)this.m_invokeList);
			@delegate = Delegate.Combine(@delegate, (Delegate)((object)handler));
			this.m_invokeList = (T)((object)@delegate);
			return preferredToken;
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x000E3D60 File Offset: 0x000E1F60
		[FriendAccessAllowed]
		internal T ExtractHandler(EventRegistrationToken token)
		{
			T result = default(T);
			Dictionary<EventRegistrationToken, T> tokens = this.m_tokens;
			lock (tokens)
			{
				if (this.m_tokens.TryGetValue(token, out result))
				{
					this.RemoveEventHandlerNoLock(token);
				}
			}
			return result;
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x000E3DBC File Offset: 0x000E1FBC
		private static EventRegistrationToken GetPreferredToken(T handler)
		{
			Delegate[] invocationList = ((Delegate)((object)handler)).GetInvocationList();
			uint hashCode;
			if (invocationList.Length == 1)
			{
				hashCode = (uint)invocationList[0].Method.GetHashCode();
			}
			else
			{
				hashCode = (uint)handler.GetHashCode();
			}
			return new EventRegistrationToken((ulong)typeof(T).MetadataToken << 32 | (ulong)hashCode);
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x000E3E1C File Offset: 0x000E201C
		public void RemoveEventHandler(EventRegistrationToken token)
		{
			if (token.Value == 0UL)
			{
				return;
			}
			Dictionary<EventRegistrationToken, T> tokens = this.m_tokens;
			lock (tokens)
			{
				this.RemoveEventHandlerNoLock(token);
			}
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x000E3E68 File Offset: 0x000E2068
		public void RemoveEventHandler(T handler)
		{
			if (handler == null)
			{
				return;
			}
			Dictionary<EventRegistrationToken, T> tokens = this.m_tokens;
			lock (tokens)
			{
				EventRegistrationToken preferredToken = EventRegistrationTokenTable<T>.GetPreferredToken(handler);
				T t;
				if (this.m_tokens.TryGetValue(preferredToken, out t) && t == handler)
				{
					this.RemoveEventHandlerNoLock(preferredToken);
				}
				else
				{
					foreach (KeyValuePair<EventRegistrationToken, T> keyValuePair in this.m_tokens)
					{
						if (keyValuePair.Value == (T)((object)handler))
						{
							this.RemoveEventHandlerNoLock(keyValuePair.Key);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x000E3F44 File Offset: 0x000E2144
		private void RemoveEventHandlerNoLock(EventRegistrationToken token)
		{
			T t;
			if (this.m_tokens.TryGetValue(token, out t))
			{
				this.m_tokens.Remove(token);
				Delegate @delegate = (Delegate)((object)this.m_invokeList);
				@delegate = Delegate.Remove(@delegate, (Delegate)((object)t));
				this.m_invokeList = (T)((object)@delegate);
			}
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x000E3FA1 File Offset: 0x000E21A1
		public static EventRegistrationTokenTable<T> GetOrCreateEventRegistrationTokenTable(ref EventRegistrationTokenTable<T> refEventTable)
		{
			if (refEventTable == null)
			{
				Interlocked.CompareExchange<EventRegistrationTokenTable<T>>(ref refEventTable, new EventRegistrationTokenTable<T>(), null);
			}
			return refEventTable;
		}

		// Token: 0x04002C25 RID: 11301
		private Dictionary<EventRegistrationToken, T> m_tokens = new Dictionary<EventRegistrationToken, T>();

		// Token: 0x04002C26 RID: 11302
		private volatile T m_invokeList;
	}
}
