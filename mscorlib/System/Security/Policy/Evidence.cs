using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using Unity;

namespace System.Security.Policy
{
	// Token: 0x0200040B RID: 1035
	[MonoTODO("Serialization format not compatible with .NET")]
	[ComVisible(true)]
	[Serializable]
	public sealed class Evidence : ICollection, IEnumerable
	{
		// Token: 0x06002A54 RID: 10836 RVA: 0x0000259F File Offset: 0x0000079F
		public Evidence()
		{
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000996AE File Offset: 0x000978AE
		public Evidence(Evidence evidence)
		{
			if (evidence != null)
			{
				this.Merge(evidence);
			}
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000996C0 File Offset: 0x000978C0
		public Evidence(EvidenceBase[] hostEvidence, EvidenceBase[] assemblyEvidence)
		{
			if (hostEvidence != null)
			{
				this.HostEvidenceList.AddRange(hostEvidence);
			}
			if (assemblyEvidence != null)
			{
				this.AssemblyEvidenceList.AddRange(assemblyEvidence);
			}
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000996C0 File Offset: 0x000978C0
		[Obsolete]
		public Evidence(object[] hostEvidence, object[] assemblyEvidence)
		{
			if (hostEvidence != null)
			{
				this.HostEvidenceList.AddRange(hostEvidence);
			}
			if (assemblyEvidence != null)
			{
				this.AssemblyEvidenceList.AddRange(assemblyEvidence);
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06002A58 RID: 10840 RVA: 0x000996E8 File Offset: 0x000978E8
		[Obsolete]
		public int Count
		{
			get
			{
				int num = 0;
				if (this.hostEvidenceList != null)
				{
					num += this.hostEvidenceList.Count;
				}
				if (this.assemblyEvidenceList != null)
				{
					num += this.assemblyEvidenceList.Count;
				}
				return num;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06002A59 RID: 10841 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06002A5A RID: 10842 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06002A5B RID: 10843 RVA: 0x00099724 File Offset: 0x00097924
		// (set) Token: 0x06002A5C RID: 10844 RVA: 0x0009972C File Offset: 0x0009792C
		public bool Locked
		{
			get
			{
				return this._locked;
			}
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			set
			{
				this._locked = value;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06002A5D RID: 10845 RVA: 0x0000270D File Offset: 0x0000090D
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06002A5E RID: 10846 RVA: 0x00099735 File Offset: 0x00097935
		internal ArrayList HostEvidenceList
		{
			get
			{
				if (this.hostEvidenceList == null)
				{
					this.hostEvidenceList = ArrayList.Synchronized(new ArrayList());
				}
				return this.hostEvidenceList;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06002A5F RID: 10847 RVA: 0x00099755 File Offset: 0x00097955
		internal ArrayList AssemblyEvidenceList
		{
			get
			{
				if (this.assemblyEvidenceList == null)
				{
					this.assemblyEvidenceList = ArrayList.Synchronized(new ArrayList());
				}
				return this.assemblyEvidenceList;
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x00099775 File Offset: 0x00097975
		[Obsolete]
		public void AddAssembly(object id)
		{
			this.AssemblyEvidenceList.Add(id);
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x00099784 File Offset: 0x00097984
		[Obsolete]
		public void AddHost(object id)
		{
			if (this._locked && SecurityManager.SecurityEnabled)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
			}
			this.HostEvidenceList.Add(id);
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000997AE File Offset: 0x000979AE
		[ComVisible(false)]
		public void Clear()
		{
			if (this.hostEvidenceList != null)
			{
				this.hostEvidenceList.Clear();
			}
			if (this.assemblyEvidenceList != null)
			{
				this.assemblyEvidenceList.Clear();
			}
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000997D6 File Offset: 0x000979D6
		[ComVisible(false)]
		public Evidence Clone()
		{
			return new Evidence(this);
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000997E0 File Offset: 0x000979E0
		[Obsolete]
		public void CopyTo(Array array, int index)
		{
			int num = 0;
			if (this.hostEvidenceList != null)
			{
				num = this.hostEvidenceList.Count;
				if (num > 0)
				{
					this.hostEvidenceList.CopyTo(array, index);
				}
			}
			if (this.assemblyEvidenceList != null && this.assemblyEvidenceList.Count > 0)
			{
				this.assemblyEvidenceList.CopyTo(array, index + num);
			}
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x0009983C File Offset: 0x00097A3C
		[Obsolete]
		public IEnumerator GetEnumerator()
		{
			IEnumerator hostenum = null;
			if (this.hostEvidenceList != null)
			{
				hostenum = this.hostEvidenceList.GetEnumerator();
			}
			IEnumerator assemblyenum = null;
			if (this.assemblyEvidenceList != null)
			{
				assemblyenum = this.assemblyEvidenceList.GetEnumerator();
			}
			return new Evidence.EvidenceEnumerator(hostenum, assemblyenum);
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x0009987C File Offset: 0x00097A7C
		public IEnumerator GetAssemblyEnumerator()
		{
			return this.AssemblyEvidenceList.GetEnumerator();
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x00099889 File Offset: 0x00097A89
		public IEnumerator GetHostEnumerator()
		{
			return this.HostEvidenceList.GetEnumerator();
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x00099898 File Offset: 0x00097A98
		public void Merge(Evidence evidence)
		{
			if (evidence != null && evidence.Count > 0)
			{
				if (evidence.hostEvidenceList != null)
				{
					foreach (object id in evidence.hostEvidenceList)
					{
						this.AddHost(id);
					}
				}
				if (evidence.assemblyEvidenceList != null)
				{
					foreach (object id2 in evidence.assemblyEvidenceList)
					{
						this.AddAssembly(id2);
					}
				}
			}
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x00099950 File Offset: 0x00097B50
		[ComVisible(false)]
		public void RemoveType(Type t)
		{
			for (int i = this.hostEvidenceList.Count; i >= 0; i--)
			{
				if (this.hostEvidenceList.GetType() == t)
				{
					this.hostEvidenceList.RemoveAt(i);
				}
			}
			for (int j = this.assemblyEvidenceList.Count; j >= 0; j--)
			{
				if (this.assemblyEvidenceList.GetType() == t)
				{
					this.assemblyEvidenceList.RemoveAt(j);
				}
			}
		}

		// Token: 0x06002A6A RID: 10858
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsAuthenticodePresent(Assembly a);

		// Token: 0x06002A6B RID: 10859 RVA: 0x000999C8 File Offset: 0x00097BC8
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static Evidence GetDefaultHostEvidence(Assembly a)
		{
			Evidence evidence = new Evidence();
			string escapedCodeBase = a.EscapedCodeBase;
			evidence.AddHost(Zone.CreateFromUrl(escapedCodeBase));
			evidence.AddHost(new Url(escapedCodeBase));
			evidence.AddHost(new Hash(a));
			if (string.Compare("FILE://", 0, escapedCodeBase, 0, 7, true, CultureInfo.InvariantCulture) != 0)
			{
				evidence.AddHost(Site.CreateFromUrl(escapedCodeBase));
			}
			AssemblyName name = a.GetName();
			byte[] publicKey = name.GetPublicKey();
			if (publicKey != null && publicKey.Length != 0)
			{
				StrongNamePublicKeyBlob blob = new StrongNamePublicKeyBlob(publicKey);
				evidence.AddHost(new StrongName(blob, name.Name, name.Version));
			}
			if (Evidence.IsAuthenticodePresent(a))
			{
				try
				{
					X509Certificate cert = X509Certificate.CreateFromSignedFile(a.Location);
					evidence.AddHost(new Publisher(cert));
				}
				catch (CryptographicException)
				{
				}
			}
			if (a.GlobalAssemblyCache)
			{
				evidence.AddHost(new GacInstalled());
			}
			AppDomainManager domainManager = AppDomain.CurrentDomain.DomainManager;
			if (domainManager != null && (domainManager.HostSecurityManager.Flags & HostSecurityManagerOptions.HostAssemblyEvidence) == HostSecurityManagerOptions.HostAssemblyEvidence)
			{
				evidence = domainManager.HostSecurityManager.ProvideAssemblyEvidence(a, evidence);
			}
			return evidence;
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000173AD File Offset: 0x000155AD
		[ComVisible(false)]
		public void AddAssemblyEvidence<T>(T evidence)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000173AD File Offset: 0x000155AD
		[ComVisible(false)]
		public void AddHostEvidence<T>(T evidence)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x00099ADC File Offset: 0x00097CDC
		[ComVisible(false)]
		public T GetAssemblyEvidence<T>()
		{
			ThrowStub.ThrowNotSupportedException();
			return default(T);
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x00099AF8 File Offset: 0x00097CF8
		[ComVisible(false)]
		public T GetHostEvidence<T>()
		{
			ThrowStub.ThrowNotSupportedException();
			return default(T);
		}

		// Token: 0x04001F90 RID: 8080
		private bool _locked;

		// Token: 0x04001F91 RID: 8081
		private ArrayList hostEvidenceList;

		// Token: 0x04001F92 RID: 8082
		private ArrayList assemblyEvidenceList;

		// Token: 0x0200040C RID: 1036
		private class EvidenceEnumerator : IEnumerator
		{
			// Token: 0x06002A70 RID: 10864 RVA: 0x00099B13 File Offset: 0x00097D13
			public EvidenceEnumerator(IEnumerator hostenum, IEnumerator assemblyenum)
			{
				this.hostEnum = hostenum;
				this.assemblyEnum = assemblyenum;
				this.currentEnum = this.hostEnum;
			}

			// Token: 0x06002A71 RID: 10865 RVA: 0x00099B38 File Offset: 0x00097D38
			public bool MoveNext()
			{
				if (this.currentEnum == null)
				{
					return false;
				}
				bool flag = this.currentEnum.MoveNext();
				if (!flag && this.hostEnum == this.currentEnum && this.assemblyEnum != null)
				{
					this.currentEnum = this.assemblyEnum;
					flag = this.assemblyEnum.MoveNext();
				}
				return flag;
			}

			// Token: 0x06002A72 RID: 10866 RVA: 0x00099B90 File Offset: 0x00097D90
			public void Reset()
			{
				if (this.hostEnum != null)
				{
					this.hostEnum.Reset();
					this.currentEnum = this.hostEnum;
				}
				else
				{
					this.currentEnum = this.assemblyEnum;
				}
				if (this.assemblyEnum != null)
				{
					this.assemblyEnum.Reset();
				}
			}

			// Token: 0x1700054B RID: 1355
			// (get) Token: 0x06002A73 RID: 10867 RVA: 0x00099BDD File Offset: 0x00097DDD
			public object Current
			{
				get
				{
					return this.currentEnum.Current;
				}
			}

			// Token: 0x04001F93 RID: 8083
			private IEnumerator currentEnum;

			// Token: 0x04001F94 RID: 8084
			private IEnumerator hostEnum;

			// Token: 0x04001F95 RID: 8085
			private IEnumerator assemblyEnum;
		}
	}
}
