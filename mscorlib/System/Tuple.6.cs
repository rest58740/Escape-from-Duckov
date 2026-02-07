using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x0200019C RID: 412
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5> : IStructuralEquatable, IStructuralComparable, IComparable, ITupleInternal, ITuple
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x00042F5E File Offset: 0x0004115E
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x00042F66 File Offset: 0x00041166
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x00042F6E File Offset: 0x0004116E
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x00042F76 File Offset: 0x00041176
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x00042F7E File Offset: 0x0004117E
		public T5 Item5
		{
			get
			{
				return this.m_Item5;
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00042F86 File Offset: 0x00041186
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
			this.m_Item5 = item5;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00042711 File Offset: 0x00040911
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00042FB4 File Offset: 0x000411B4
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4, T5> tuple = other as Tuple<T1, T2, T3, T4, T5>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3) && comparer.Equals(this.m_Item4, tuple.m_Item4)) && comparer.Equals(this.m_Item5, tuple.m_Item5);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0004275A File Offset: 0x0004095A
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00043068 File Offset: 0x00041268
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4, T5> tuple = other as Tuple<T1, T2, T3, T4, T5>;
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
			num = comparer.Compare(this.m_Item3, tuple.m_Item3);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.m_Item4, tuple.m_Item4);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.m_Item5, tuple.m_Item5);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x000427C0 File Offset: 0x000409C0
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0004314C File Offset: 0x0004134C
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5));
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x000427E0 File Offset: 0x000409E0
		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x000431B4 File Offset: 0x000413B4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			return ((ITupleInternal)this).ToString(stringBuilder);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x000431D8 File Offset: 0x000413D8
		string ITupleInternal.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(", ");
			sb.Append(this.m_Item2);
			sb.Append(", ");
			sb.Append(this.m_Item3);
			sb.Append(", ");
			sb.Append(this.m_Item4);
			sb.Append(", ");
			sb.Append(this.m_Item5);
			sb.Append(')');
			return sb.ToString();
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x0003CDA4 File Offset: 0x0003AFA4
		int ITuple.Length
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000147 RID: 327
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
				case 3:
					return this.Item4;
				case 4:
					return this.Item5;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x04001323 RID: 4899
		private readonly T1 m_Item1;

		// Token: 0x04001324 RID: 4900
		private readonly T2 m_Item2;

		// Token: 0x04001325 RID: 4901
		private readonly T3 m_Item3;

		// Token: 0x04001326 RID: 4902
		private readonly T4 m_Item4;

		// Token: 0x04001327 RID: 4903
		private readonly T5 m_Item5;
	}
}
