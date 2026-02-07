using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x0200019B RID: 411
	[Serializable]
	public class Tuple<T1, T2, T3, T4> : IStructuralEquatable, IStructuralComparable, IComparable, ITupleInternal, ITuple
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x00042C61 File Offset: 0x00040E61
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x00042C69 File Offset: 0x00040E69
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x00042C71 File Offset: 0x00040E71
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x00042C79 File Offset: 0x00040E79
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00042C81 File Offset: 0x00040E81
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00042711 File Offset: 0x00040911
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00042CA8 File Offset: 0x00040EA8
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4> tuple = other as Tuple<T1, T2, T3, T4>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3)) && comparer.Equals(this.m_Item4, tuple.m_Item4);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0004275A File Offset: 0x0004095A
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00042D40 File Offset: 0x00040F40
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4> tuple = other as Tuple<T1, T2, T3, T4>;
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
			return comparer.Compare(this.m_Item4, tuple.m_Item4);
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x000427C0 File Offset: 0x000409C0
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00042E00 File Offset: 0x00041000
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4));
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x000427E0 File Offset: 0x000409E0
		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00042E58 File Offset: 0x00041058
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			return ((ITupleInternal)this).ToString(stringBuilder);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00042E7C File Offset: 0x0004107C
		string ITupleInternal.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(", ");
			sb.Append(this.m_Item2);
			sb.Append(", ");
			sb.Append(this.m_Item3);
			sb.Append(", ");
			sb.Append(this.m_Item4);
			sb.Append(')');
			return sb.ToString();
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x0002280B File Offset: 0x00020A0B
		int ITuple.Length
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000140 RID: 320
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
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x0400131F RID: 4895
		private readonly T1 m_Item1;

		// Token: 0x04001320 RID: 4896
		private readonly T2 m_Item2;

		// Token: 0x04001321 RID: 4897
		private readonly T3 m_Item3;

		// Token: 0x04001322 RID: 4898
		private readonly T4 m_Item4;
	}
}
