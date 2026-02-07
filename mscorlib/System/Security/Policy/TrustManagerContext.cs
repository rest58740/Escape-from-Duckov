using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000422 RID: 1058
	[ComVisible(true)]
	public class TrustManagerContext
	{
		// Token: 0x06002B51 RID: 11089 RVA: 0x0009CB5A File Offset: 0x0009AD5A
		public TrustManagerContext() : this(TrustManagerUIContext.Run)
		{
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x0009CB63 File Offset: 0x0009AD63
		public TrustManagerContext(TrustManagerUIContext uiContext)
		{
			this._ignorePersistedDecision = false;
			this._noPrompt = false;
			this._keepAlive = false;
			this._persist = false;
			this._ui = uiContext;
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x0009CB8E File Offset: 0x0009AD8E
		// (set) Token: 0x06002B54 RID: 11092 RVA: 0x0009CB96 File Offset: 0x0009AD96
		public virtual bool IgnorePersistedDecision
		{
			get
			{
				return this._ignorePersistedDecision;
			}
			set
			{
				this._ignorePersistedDecision = value;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002B55 RID: 11093 RVA: 0x0009CB9F File Offset: 0x0009AD9F
		// (set) Token: 0x06002B56 RID: 11094 RVA: 0x0009CBA7 File Offset: 0x0009ADA7
		public virtual bool KeepAlive
		{
			get
			{
				return this._keepAlive;
			}
			set
			{
				this._keepAlive = value;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06002B57 RID: 11095 RVA: 0x0009CBB0 File Offset: 0x0009ADB0
		// (set) Token: 0x06002B58 RID: 11096 RVA: 0x0009CBB8 File Offset: 0x0009ADB8
		public virtual bool NoPrompt
		{
			get
			{
				return this._noPrompt;
			}
			set
			{
				this._noPrompt = value;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06002B59 RID: 11097 RVA: 0x0009CBC1 File Offset: 0x0009ADC1
		// (set) Token: 0x06002B5A RID: 11098 RVA: 0x0009CBC9 File Offset: 0x0009ADC9
		public virtual bool Persist
		{
			get
			{
				return this._persist;
			}
			set
			{
				this._persist = value;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x0009CBD2 File Offset: 0x0009ADD2
		// (set) Token: 0x06002B5C RID: 11100 RVA: 0x0009CBDA File Offset: 0x0009ADDA
		public virtual ApplicationIdentity PreviousApplicationIdentity
		{
			get
			{
				return this._previousId;
			}
			set
			{
				this._previousId = value;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x0009CBE3 File Offset: 0x0009ADE3
		// (set) Token: 0x06002B5E RID: 11102 RVA: 0x0009CBEB File Offset: 0x0009ADEB
		public virtual TrustManagerUIContext UIContext
		{
			get
			{
				return this._ui;
			}
			set
			{
				this._ui = value;
			}
		}

		// Token: 0x04001FBC RID: 8124
		private bool _ignorePersistedDecision;

		// Token: 0x04001FBD RID: 8125
		private bool _noPrompt;

		// Token: 0x04001FBE RID: 8126
		private bool _keepAlive;

		// Token: 0x04001FBF RID: 8127
		private bool _persist;

		// Token: 0x04001FC0 RID: 8128
		private ApplicationIdentity _previousId;

		// Token: 0x04001FC1 RID: 8129
		private TrustManagerUIContext _ui;
	}
}
