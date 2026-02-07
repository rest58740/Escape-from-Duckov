using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Unity;

namespace System
{
	// Token: 0x02000223 RID: 547
	[ComVisible(false)]
	[Serializable]
	public sealed class ActivationContext : IDisposable, ISerializable
	{
		// Token: 0x06001871 RID: 6257 RVA: 0x0005D860 File Offset: 0x0005BA60
		private ActivationContext(ApplicationIdentity identity)
		{
			this._appid = identity;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0005D870 File Offset: 0x0005BA70
		~ActivationContext()
		{
			this.Dispose(false);
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x0005D8A0 File Offset: 0x0005BAA0
		public ActivationContext.ContextForm Form
		{
			get
			{
				return this._form;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x0005D8A8 File Offset: 0x0005BAA8
		public ApplicationIdentity Identity
		{
			get
			{
				return this._appid;
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0005D8B0 File Offset: 0x0005BAB0
		[MonoTODO("Missing validation")]
		public static ActivationContext CreatePartialActivationContext(ApplicationIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			return new ActivationContext(identity);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0005D8C6 File Offset: 0x0005BAC6
		[MonoTODO("Missing validation")]
		public static ActivationContext CreatePartialActivationContext(ApplicationIdentity identity, string[] manifestPaths)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			if (manifestPaths == null)
			{
				throw new ArgumentNullException("manifestPaths");
			}
			return new ActivationContext(identity);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0005D8EA File Offset: 0x0005BAEA
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0005D8F9 File Offset: 0x0005BAF9
		private void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				this._disposed = true;
			}
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0005D90C File Offset: 0x0005BB0C
		[MonoTODO("Missing serialization support")]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x000173AD File Offset: 0x000155AD
		internal ActivationContext()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600187B RID: 6267 RVA: 0x00052959 File Offset: 0x00050B59
		public byte[] ApplicationManifestBytes
		{
			get
			{
				ThrowStub.ThrowNotSupportedException();
				return null;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600187C RID: 6268 RVA: 0x00052959 File Offset: 0x00050B59
		public byte[] DeploymentManifestBytes
		{
			get
			{
				ThrowStub.ThrowNotSupportedException();
				return null;
			}
		}

		// Token: 0x040016B3 RID: 5811
		private ApplicationIdentity _appid;

		// Token: 0x040016B4 RID: 5812
		private ActivationContext.ContextForm _form;

		// Token: 0x040016B5 RID: 5813
		private bool _disposed;

		// Token: 0x02000224 RID: 548
		public enum ContextForm
		{
			// Token: 0x040016B7 RID: 5815
			Loose,
			// Token: 0x040016B8 RID: 5816
			StoreBounded
		}
	}
}
