using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200023B RID: 571
	[ComVisible(true)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public abstract class MarshalByRefObject
	{
		// Token: 0x06001A17 RID: 6679 RVA: 0x0005FEC8 File Offset: 0x0005E0C8
		internal Identity GetObjectIdentity(MarshalByRefObject obj, out bool IsClient)
		{
			IsClient = false;
			Identity objectIdentity;
			if (RemotingServices.IsTransparentProxy(obj))
			{
				objectIdentity = RemotingServices.GetRealProxy(obj).ObjectIdentity;
				IsClient = true;
			}
			else
			{
				objectIdentity = obj.ObjectIdentity;
			}
			return objectIdentity;
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001A18 RID: 6680 RVA: 0x0005FEFB File Offset: 0x0005E0FB
		// (set) Token: 0x06001A19 RID: 6681 RVA: 0x0005FF03 File Offset: 0x0005E103
		internal ServerIdentity ObjectIdentity
		{
			get
			{
				return this._identity;
			}
			set
			{
				this._identity = value;
			}
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x0005FF0C File Offset: 0x0005E10C
		[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
		public virtual ObjRef CreateObjRef(Type requestedType)
		{
			if (this._identity == null)
			{
				throw new RemotingException(Locale.GetText("No remoting information was found for the object."));
			}
			return this._identity.CreateObjRef(requestedType);
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0005FF32 File Offset: 0x0005E132
		[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
		public object GetLifetimeService()
		{
			if (this._identity == null)
			{
				return null;
			}
			return this._identity.Lease;
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0005FF49 File Offset: 0x0005E149
		[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
		public virtual object InitializeLifetimeService()
		{
			if (this._identity != null && this._identity.Lease != null)
			{
				return this._identity.Lease;
			}
			return new Lease();
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0005FF74 File Offset: 0x0005E174
		protected MarshalByRefObject MemberwiseClone(bool cloneIdentity)
		{
			MarshalByRefObject marshalByRefObject = (MarshalByRefObject)base.MemberwiseClone();
			if (!cloneIdentity)
			{
				marshalByRefObject._identity = null;
			}
			return marshalByRefObject;
		}

		// Token: 0x0400171E RID: 5918
		[NonSerialized]
		private ServerIdentity _identity;
	}
}
