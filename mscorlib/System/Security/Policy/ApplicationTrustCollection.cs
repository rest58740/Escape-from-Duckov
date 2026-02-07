using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000404 RID: 1028
	[ComVisible(true)]
	public sealed class ApplicationTrustCollection : ICollection, IEnumerable
	{
		// Token: 0x06002A01 RID: 10753 RVA: 0x000985D1 File Offset: 0x000967D1
		internal ApplicationTrustCollection()
		{
			this._list = new ArrayList();
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06002A02 RID: 10754 RVA: 0x000985E4 File Offset: 0x000967E4
		public int Count
		{
			[SecuritySafeCritical]
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06002A03 RID: 10755 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsSynchronized
		{
			[SecuritySafeCritical]
			get
			{
				return false;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002A04 RID: 10756 RVA: 0x0000270D File Offset: 0x0000090D
		public object SyncRoot
		{
			[SecuritySafeCritical]
			get
			{
				return this;
			}
		}

		// Token: 0x1700052F RID: 1327
		public ApplicationTrust this[int index]
		{
			get
			{
				return (ApplicationTrust)this._list[index];
			}
		}

		// Token: 0x17000530 RID: 1328
		public ApplicationTrust this[string appFullName]
		{
			get
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					ApplicationTrust applicationTrust = this._list[i] as ApplicationTrust;
					if (applicationTrust.ApplicationIdentity.FullName == appFullName)
					{
						return applicationTrust;
					}
				}
				return null;
			}
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x0009864F File Offset: 0x0009684F
		public int Add(ApplicationTrust trust)
		{
			if (trust == null)
			{
				throw new ArgumentNullException("trust");
			}
			if (trust.ApplicationIdentity == null)
			{
				throw new ArgumentException(Locale.GetText("ApplicationTrust.ApplicationIdentity can't be null."), "trust");
			}
			return this._list.Add(trust);
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x00098688 File Offset: 0x00096888
		public void AddRange(ApplicationTrust[] trusts)
		{
			if (trusts == null)
			{
				throw new ArgumentNullException("trusts");
			}
			foreach (ApplicationTrust applicationTrust in trusts)
			{
				if (applicationTrust.ApplicationIdentity == null)
				{
					throw new ArgumentException(Locale.GetText("ApplicationTrust.ApplicationIdentity can't be null."), "trust");
				}
				this._list.Add(applicationTrust);
			}
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000986E4 File Offset: 0x000968E4
		public void AddRange(ApplicationTrustCollection trusts)
		{
			if (trusts == null)
			{
				throw new ArgumentNullException("trusts");
			}
			foreach (ApplicationTrust applicationTrust in trusts)
			{
				if (applicationTrust.ApplicationIdentity == null)
				{
					throw new ArgumentException(Locale.GetText("ApplicationTrust.ApplicationIdentity can't be null."), "trust");
				}
				this._list.Add(applicationTrust);
			}
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x00098741 File Offset: 0x00096941
		public void Clear()
		{
			this._list.Clear();
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x0009874E File Offset: 0x0009694E
		public void CopyTo(ApplicationTrust[] array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x0009874E File Offset: 0x0009694E
		void ICollection.CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x00098760 File Offset: 0x00096960
		public ApplicationTrustCollection Find(ApplicationIdentity applicationIdentity, ApplicationVersionMatch versionMatch)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			string text = applicationIdentity.FullName;
			if (versionMatch != ApplicationVersionMatch.MatchExactVersion)
			{
				if (versionMatch != ApplicationVersionMatch.MatchAllVersions)
				{
					throw new ArgumentException("versionMatch");
				}
				int num = text.IndexOf(", Version=");
				if (num >= 0)
				{
					text = text.Substring(0, num);
				}
			}
			ApplicationTrustCollection applicationTrustCollection = new ApplicationTrustCollection();
			foreach (object obj in this._list)
			{
				ApplicationTrust applicationTrust = (ApplicationTrust)obj;
				if (applicationTrust.ApplicationIdentity.FullName.StartsWith(text))
				{
					applicationTrustCollection.Add(applicationTrust);
				}
			}
			return applicationTrustCollection;
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x0009881C File Offset: 0x00096A1C
		public ApplicationTrustEnumerator GetEnumerator()
		{
			return new ApplicationTrustEnumerator(this);
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x0009881C File Offset: 0x00096A1C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ApplicationTrustEnumerator(this);
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x00098824 File Offset: 0x00096A24
		public void Remove(ApplicationTrust trust)
		{
			if (trust == null)
			{
				throw new ArgumentNullException("trust");
			}
			if (trust.ApplicationIdentity == null)
			{
				throw new ArgumentException(Locale.GetText("ApplicationTrust.ApplicationIdentity can't be null."), "trust");
			}
			this.RemoveAllInstances(trust);
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x00098858 File Offset: 0x00096A58
		public void Remove(ApplicationIdentity applicationIdentity, ApplicationVersionMatch versionMatch)
		{
			foreach (ApplicationTrust trust in this.Find(applicationIdentity, versionMatch))
			{
				this.RemoveAllInstances(trust);
			}
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x0009888C File Offset: 0x00096A8C
		public void RemoveRange(ApplicationTrust[] trusts)
		{
			if (trusts == null)
			{
				throw new ArgumentNullException("trusts");
			}
			foreach (ApplicationTrust trust in trusts)
			{
				this.RemoveAllInstances(trust);
			}
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000988C4 File Offset: 0x00096AC4
		public void RemoveRange(ApplicationTrustCollection trusts)
		{
			if (trusts == null)
			{
				throw new ArgumentNullException("trusts");
			}
			foreach (ApplicationTrust trust in trusts)
			{
				this.RemoveAllInstances(trust);
			}
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x00098900 File Offset: 0x00096B00
		internal void RemoveAllInstances(ApplicationTrust trust)
		{
			for (int i = this._list.Count - 1; i >= 0; i--)
			{
				if (trust.Equals(this._list[i]))
				{
					this._list.RemoveAt(i);
				}
			}
		}

		// Token: 0x04001F61 RID: 8033
		private ArrayList _list;
	}
}
