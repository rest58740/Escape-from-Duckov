using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200059E RID: 1438
	[AttributeUsage(AttributeTargets.Class)]
	[ComVisible(true)]
	[Serializable]
	public class SynchronizationAttribute : ContextAttribute, IContributeClientContextSink, IContributeServerContextSink
	{
		// Token: 0x060037E1 RID: 14305 RVA: 0x000C8D4C File Offset: 0x000C6F4C
		public SynchronizationAttribute() : this(8, false)
		{
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x000C8D56 File Offset: 0x000C6F56
		public SynchronizationAttribute(bool reEntrant) : this(8, reEntrant)
		{
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x000C8D60 File Offset: 0x000C6F60
		public SynchronizationAttribute(int flag) : this(flag, false)
		{
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000C8D6C File Offset: 0x000C6F6C
		public SynchronizationAttribute(int flag, bool reEntrant) : base("Synchronization")
		{
			if (flag != 1 && flag != 4 && flag != 8 && flag != 2)
			{
				throw new ArgumentException("flag");
			}
			this._bReEntrant = reEntrant;
			this._flavor = flag;
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060037E5 RID: 14309 RVA: 0x000C8DB9 File Offset: 0x000C6FB9
		public virtual bool IsReEntrant
		{
			get
			{
				return this._bReEntrant;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x000C8DC1 File Offset: 0x000C6FC1
		// (set) Token: 0x060037E7 RID: 14311 RVA: 0x000C8DCC File Offset: 0x000C6FCC
		public virtual bool Locked
		{
			get
			{
				return this._lockCount > 0;
			}
			set
			{
				SynchronizationAttribute obj;
				if (value)
				{
					this.AcquireLock();
					obj = this;
					lock (obj)
					{
						if (this._lockCount > 1)
						{
							this.ReleaseLock();
						}
						return;
					}
				}
				obj = this;
				lock (obj)
				{
					while (this._lockCount > 0 && this._ownerThread == Thread.CurrentThread)
					{
						this.ReleaseLock();
					}
				}
			}
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x000C8E5C File Offset: 0x000C705C
		internal void AcquireLock()
		{
			this._mutex.WaitOne();
			lock (this)
			{
				this._ownerThread = Thread.CurrentThread;
				this._lockCount++;
			}
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x000C8EB8 File Offset: 0x000C70B8
		internal void ReleaseLock()
		{
			lock (this)
			{
				if (this._lockCount > 0 && this._ownerThread == Thread.CurrentThread)
				{
					this._lockCount--;
					this._mutex.ReleaseMutex();
					if (this._lockCount == 0)
					{
						this._ownerThread = null;
					}
				}
			}
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x000C8F2C File Offset: 0x000C712C
		[SecurityCritical]
		[ComVisible(true)]
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (this._flavor != 1)
			{
				ctorMsg.ContextProperties.Add(this);
			}
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x000C8F44 File Offset: 0x000C7144
		[SecurityCritical]
		public virtual IMessageSink GetClientContextSink(IMessageSink nextSink)
		{
			return new SynchronizedClientContextSink(nextSink, this);
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x000C8F4D File Offset: 0x000C714D
		[SecurityCritical]
		public virtual IMessageSink GetServerContextSink(IMessageSink nextSink)
		{
			return new SynchronizedServerContextSink(nextSink, this);
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x000C8F58 File Offset: 0x000C7158
		[SecurityCritical]
		[ComVisible(true)]
		public override bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			SynchronizationAttribute synchronizationAttribute = ctx.GetProperty("Synchronization") as SynchronizationAttribute;
			int flavor = this._flavor;
			switch (flavor)
			{
			case 1:
				return synchronizationAttribute == null;
			case 2:
				return true;
			case 3:
				break;
			case 4:
				return synchronizationAttribute != null;
			default:
				if (flavor == 8)
				{
					return false;
				}
				break;
			}
			return false;
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x000C8FAC File Offset: 0x000C71AC
		internal static void ExitContext()
		{
			if (Thread.CurrentContext.IsDefaultContext)
			{
				return;
			}
			SynchronizationAttribute synchronizationAttribute = Thread.CurrentContext.GetProperty("Synchronization") as SynchronizationAttribute;
			if (synchronizationAttribute == null)
			{
				return;
			}
			synchronizationAttribute.Locked = false;
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x000C8FE8 File Offset: 0x000C71E8
		internal static void EnterContext()
		{
			if (Thread.CurrentContext.IsDefaultContext)
			{
				return;
			}
			SynchronizationAttribute synchronizationAttribute = Thread.CurrentContext.GetProperty("Synchronization") as SynchronizationAttribute;
			if (synchronizationAttribute == null)
			{
				return;
			}
			synchronizationAttribute.Locked = true;
		}

		// Token: 0x040025B7 RID: 9655
		public const int NOT_SUPPORTED = 1;

		// Token: 0x040025B8 RID: 9656
		public const int SUPPORTED = 2;

		// Token: 0x040025B9 RID: 9657
		public const int REQUIRED = 4;

		// Token: 0x040025BA RID: 9658
		public const int REQUIRES_NEW = 8;

		// Token: 0x040025BB RID: 9659
		private bool _bReEntrant;

		// Token: 0x040025BC RID: 9660
		private int _flavor;

		// Token: 0x040025BD RID: 9661
		[NonSerialized]
		private int _lockCount;

		// Token: 0x040025BE RID: 9662
		[NonSerialized]
		private Mutex _mutex = new Mutex(false);

		// Token: 0x040025BF RID: 9663
		[NonSerialized]
		private Thread _ownerThread;
	}
}
