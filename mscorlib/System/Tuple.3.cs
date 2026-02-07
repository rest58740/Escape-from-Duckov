using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x02000199 RID: 409
	[Serializable]
	public class Tuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable, ITupleInternal, ITuple
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x00042848 File Offset: 0x00040A48
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x00042850 File Offset: 0x00040A50
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00042858 File Offset: 0x00040A58
		public Tuple(T1 item1, T2 item2)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00042711 File Offset: 0x00040911
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00042870 File Offset: 0x00040A70
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2> tuple = other as Tuple<T1, T2>;
			return tuple != null && comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0004275A File Offset: 0x0004095A
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x000428CC File Offset: 0x00040ACC
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2> tuple = other as Tuple<T1, T2>;
			if (tuple == null)
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			int num = comparer.Compare(this.m_Item1, tuple.m_Item1);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.m_Item2, tuple.m_Item2);
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x000427C0 File Offset: 0x000409C0
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00042948 File Offset: 0x00040B48
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2));
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x000427E0 File Offset: 0x000409E0
		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00042974 File Offset: 0x00040B74
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			return ((ITupleInternal)this).ToString(stringBuilder);
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00042998 File Offset: 0x00040B98
		string ITupleInternal.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(", ");
			sb.Append(this.m_Item2);
			sb.Append(')');
			return sb.ToString();
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x00015831 File Offset: 0x00013A31
		int ITuple.Length
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000135 RID: 309
		object ITuple.this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.Item1;
				}
				if (index != 1)
				{
					throw new IndexOutOfRangeException();
				}
				return this.Item2;
			}
		}

		// Token: 0x0400131A RID: 4890
		private readonly T1 m_Item1;

		// Token: 0x0400131B RID: 4891
		private readonly T2 m_Item2;
	}
}
