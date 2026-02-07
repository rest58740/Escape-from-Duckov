using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x0200019D RID: 413
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6> : IStructuralEquatable, IStructuralComparable, IComparable, ITupleInternal, ITuple
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x000432EA File Offset: 0x000414EA
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x000432F2 File Offset: 0x000414F2
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x000432FA File Offset: 0x000414FA
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x00043302 File Offset: 0x00041502
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x0004330A File Offset: 0x0004150A
		public T5 Item5
		{
			get
			{
				return this.m_Item5;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00043312 File Offset: 0x00041512
		public T6 Item6
		{
			get
			{
				return this.m_Item6;
			}
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0004331A File Offset: 0x0004151A
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
			this.m_Item5 = item5;
			this.m_Item6 = item6;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00042711 File Offset: 0x00040911
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00043350 File Offset: 0x00041550
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4, T5, T6> tuple = other as Tuple<T1, T2, T3, T4, T5, T6>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3) && comparer.Equals(this.m_Item4, tuple.m_Item4) && comparer.Equals(this.m_Item5, tuple.m_Item5)) && comparer.Equals(this.m_Item6, tuple.m_Item6);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0004275A File Offset: 0x0004095A
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00043428 File Offset: 0x00041628
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4, T5, T6> tuple = other as Tuple<T1, T2, T3, T4, T5, T6>;
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
			num = comparer.Compare(this.m_Item5, tuple.m_Item5);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.m_Item6, tuple.m_Item6);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x000427C0 File Offset: 0x000409C0
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0004352C File Offset: 0x0004172C
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6));
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x000427E0 File Offset: 0x000409E0
		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x000435A4 File Offset: 0x000417A4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			return ((ITupleInternal)this).ToString(stringBuilder);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x000435C8 File Offset: 0x000417C8
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
			sb.Append(", ");
			sb.Append(this.m_Item6);
			sb.Append(')');
			return sb.ToString();
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x000224A7 File Offset: 0x000206A7
		int ITuple.Length
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700014F RID: 335
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
				case 5:
					return this.Item6;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x04001328 RID: 4904
		private readonly T1 m_Item1;

		// Token: 0x04001329 RID: 4905
		private readonly T2 m_Item2;

		// Token: 0x0400132A RID: 4906
		private readonly T3 m_Item3;

		// Token: 0x0400132B RID: 4907
		private readonly T4 m_Item4;

		// Token: 0x0400132C RID: 4908
		private readonly T5 m_Item5;

		// Token: 0x0400132D RID: 4909
		private readonly T6 m_Item6;
	}
}
