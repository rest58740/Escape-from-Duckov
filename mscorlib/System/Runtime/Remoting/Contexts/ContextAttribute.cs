using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Security;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200058F RID: 1423
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class)]
	[Serializable]
	public class ContextAttribute : Attribute, IContextAttribute, IContextProperty
	{
		// Token: 0x060037B7 RID: 14263 RVA: 0x000C8A48 File Offset: 0x000C6C48
		public ContextAttribute(string name)
		{
			this.AttributeName = name;
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060037B8 RID: 14264 RVA: 0x000C8A57 File Offset: 0x000C6C57
		public virtual string Name
		{
			[SecurityCritical]
			get
			{
				return this.AttributeName;
			}
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x000C8A5F File Offset: 0x000C6C5F
		public override bool Equals(object o)
		{
			return o != null && o is ContextAttribute && !(((ContextAttribute)o).AttributeName != this.AttributeName);
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[SecurityCritical]
		public virtual void Freeze(Context newContext)
		{
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000C8A8B File Offset: 0x000C6C8B
		public override int GetHashCode()
		{
			if (this.AttributeName == null)
			{
				return 0;
			}
			return this.AttributeName.GetHashCode();
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x000C8AA2 File Offset: 0x000C6CA2
		[SecurityCritical]
		public virtual void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null)
			{
				throw new ArgumentNullException("ctorMsg");
			}
			ctorMsg.ContextProperties.Add(this);
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x000C8AC0 File Offset: 0x000C6CC0
		[SecurityCritical]
		public virtual bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null)
			{
				throw new ArgumentNullException("ctorMsg");
			}
			if (ctx == null)
			{
				throw new ArgumentNullException("ctx");
			}
			if (!ctorMsg.ActivationType.IsContextful)
			{
				return true;
			}
			IContextProperty property = ctx.GetProperty(this.AttributeName);
			return property != null && this == property;
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x000040F7 File Offset: 0x000022F7
		[SecurityCritical]
		public virtual bool IsNewContextOK(Context newCtx)
		{
			return true;
		}

		// Token: 0x040025B1 RID: 9649
		protected string AttributeName;
	}
}
