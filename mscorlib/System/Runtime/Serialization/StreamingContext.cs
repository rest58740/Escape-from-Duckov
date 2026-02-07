using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000673 RID: 1651
	[ComVisible(true)]
	[Serializable]
	public readonly struct StreamingContext
	{
		// Token: 0x06003DB5 RID: 15797 RVA: 0x000D567A File Offset: 0x000D387A
		public StreamingContext(StreamingContextStates state)
		{
			this = new StreamingContext(state, null);
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x000D5684 File Offset: 0x000D3884
		public StreamingContext(StreamingContextStates state, object additional)
		{
			this.m_state = state;
			this.m_additionalContext = additional;
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06003DB7 RID: 15799 RVA: 0x000D5694 File Offset: 0x000D3894
		public object Context
		{
			get
			{
				return this.m_additionalContext;
			}
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x000D569C File Offset: 0x000D389C
		public override bool Equals(object obj)
		{
			return obj is StreamingContext && (((StreamingContext)obj).m_additionalContext == this.m_additionalContext && ((StreamingContext)obj).m_state == this.m_state);
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x000D56D1 File Offset: 0x000D38D1
		public override int GetHashCode()
		{
			return (int)this.m_state;
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x000D56D1 File Offset: 0x000D38D1
		public StreamingContextStates State
		{
			get
			{
				return this.m_state;
			}
		}

		// Token: 0x04002795 RID: 10133
		internal readonly object m_additionalContext;

		// Token: 0x04002796 RID: 10134
		internal readonly StreamingContextStates m_state;
	}
}
