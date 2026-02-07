using System;

namespace System.Threading
{
	// Token: 0x02000281 RID: 641
	public sealed class AsyncLocal<T> : IAsyncLocal
	{
		// Token: 0x06001D5A RID: 7514 RVA: 0x0000259F File Offset: 0x0000079F
		public AsyncLocal()
		{
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x0006DAA5 File Offset: 0x0006BCA5
		public AsyncLocal(Action<AsyncLocalValueChangedArgs<T>> valueChangedHandler)
		{
			this.m_valueChangedHandler = valueChangedHandler;
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001D5C RID: 7516 RVA: 0x0006DAB4 File Offset: 0x0006BCB4
		// (set) Token: 0x06001D5D RID: 7517 RVA: 0x0006DADB File Offset: 0x0006BCDB
		public T Value
		{
			get
			{
				object localValue = ExecutionContext.GetLocalValue(this);
				if (localValue != null)
				{
					return (T)((object)localValue);
				}
				return default(T);
			}
			set
			{
				ExecutionContext.SetLocalValue(this, value, this.m_valueChangedHandler != null);
			}
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x0006DAF4 File Offset: 0x0006BCF4
		void IAsyncLocal.OnValueChanged(object previousValueObj, object currentValueObj, bool contextChanged)
		{
			T previousValue = (previousValueObj == null) ? default(T) : ((T)((object)previousValueObj));
			T currentValue = (currentValueObj == null) ? default(T) : ((T)((object)currentValueObj));
			this.m_valueChangedHandler(new AsyncLocalValueChangedArgs<T>(previousValue, currentValue, contextChanged));
		}

		// Token: 0x04001A24 RID: 6692
		private readonly Action<AsyncLocalValueChangedArgs<T>> m_valueChangedHandler;
	}
}
