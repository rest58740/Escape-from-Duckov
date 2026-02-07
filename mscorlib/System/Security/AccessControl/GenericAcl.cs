using System;
using System.Collections;

namespace System.Security.AccessControl
{
	// Token: 0x0200052D RID: 1325
	public abstract class GenericAcl : ICollection, IEnumerable
	{
		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600347A RID: 13434
		public abstract int BinaryLength { get; }

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600347B RID: 13435
		public abstract int Count { get; }

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x0600347C RID: 13436 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700072B RID: 1835
		public abstract GenericAce this[int index]
		{
			get;
			set;
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x0600347F RID: 13439
		public abstract byte Revision { get; }

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06003480 RID: 13440 RVA: 0x0000270D File Offset: 0x0000090D
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x000BF4E4 File Offset: 0x000BD6E4
		public void CopyTo(GenericAce[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || array.Length - index < this.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index must be non-negative integer and must not exceed array length - count");
			}
			for (int i = 0; i < this.Count; i++)
			{
				array[i + index] = this[i];
			}
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x000BF53D File Offset: 0x000BD73D
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo((GenericAce[])array, index);
		}

		// Token: 0x06003483 RID: 13443
		public abstract void GetBinaryForm(byte[] binaryForm, int offset);

		// Token: 0x06003484 RID: 13444 RVA: 0x000BF54C File Offset: 0x000BD74C
		public AceEnumerator GetEnumerator()
		{
			return new AceEnumerator(this);
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000BF554 File Offset: 0x000BD754
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06003486 RID: 13446
		internal abstract string GetSddlForm(ControlFlags sdFlags, bool isDacl);

		// Token: 0x040024A7 RID: 9383
		public static readonly byte AclRevision = 2;

		// Token: 0x040024A8 RID: 9384
		public static readonly byte AclRevisionDS = 4;

		// Token: 0x040024A9 RID: 9385
		public static readonly int MaxBinaryLength = 65536;
	}
}
