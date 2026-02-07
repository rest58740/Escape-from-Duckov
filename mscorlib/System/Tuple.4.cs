using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x0200019A RID: 410
	[Serializable]
	public class Tuple<T1, T2, T3> : IStructuralEquatable, IStructuralComparable, IComparable, ITupleInternal, ITuple
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x00042A0C File Offset: 0x00040C0C
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x00042A14 File Offset: 0x00040C14
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x00042A1C File Offset: 0x00040C1C
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00042A24 File Offset: 0x00040C24
		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00042711 File Offset: 0x00040911
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00042A44 File Offset: 0x00040C44
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3> tuple = other as Tuple<T1, T2, T3>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2)) && comparer.Equals(this.m_Item3, tuple.m_Item3);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0004275A File Offset: 0x0004095A
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x00042ABC File Offset: 0x00040CBC
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3> tuple = other as Tuple<T1, T2, T3>;
			if (tuple == null)
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			int num = comparer.Compare(this.m_Item1, tuple.m_Item1);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.m_Item2, tuple.m_Item2);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.m_Item3, tuple.m_Item3);
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x000427C0 File Offset: 0x000409C0
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00042B5A File Offset: 0x00040D5A
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3));
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x000427E0 File Offset: 0x000409E0
		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00042B94 File Offset: 0x00040D94
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			return ((ITupleInternal)this).ToString(stringBuilder);
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00042BB8 File Offset: 0x00040DB8
		string ITupleInternal.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(", ");
			sb.Append(this.m_Item2);
			sb.Append(", ");
			sb.Append(this.m_Item3);
			sb.Append(')');
			return sb.ToString();
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x000221D6 File Offset: 0x000203D6
		int ITuple.Length
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700013A RID: 314
		object ITuple.this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.Item1;
				case 1:
					return this.Item2;
				case 2:
					return this.Item3;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x0400131C RID: 4892
		private readonly T1 m_Item1;

		// Token: 0x0400131D RID: 4893
		private readonly T2 m_Item2;

		// Token: 0x0400131E RID: 4894
		private readonly T3 m_Item3;
	}
}
