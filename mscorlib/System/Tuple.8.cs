using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x0200019E RID: 414
	[Serializable]
	public class Tuple<T1, T2, T3, T4, T5, T6, T7> : IStructuralEquatable, IStructuralComparable, IComparable, ITupleInternal, ITuple
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x00043706 File Offset: 0x00041906
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x0004370E File Offset: 0x0004190E
		public T2 Item2
		{
			get
			{
				return this.m_Item2;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060010A3 RID: 4259 RVA: 0x00043716 File Offset: 0x00041916
		public T3 Item3
		{
			get
			{
				return this.m_Item3;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0004371E File Offset: 0x0004191E
		public T4 Item4
		{
			get
			{
				return this.m_Item4;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00043726 File Offset: 0x00041926
		public T5 Item5
		{
			get
			{
				return this.m_Item5;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0004372E File Offset: 0x0004192E
		public T6 Item6
		{
			get
			{
				return this.m_Item6;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x00043736 File Offset: 0x00041936
		public T7 Item7
		{
			get
			{
				return this.m_Item7;
			}
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0004373E File Offset: 0x0004193E
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			this.m_Item1 = item1;
			this.m_Item2 = item2;
			this.m_Item3 = item3;
			this.m_Item4 = item4;
			this.m_Item5 = item5;
			this.m_Item6 = item6;
			this.m_Item7 = item7;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00042711 File Offset: 0x00040911
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0004377C File Offset: 0x0004197C
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1, T2, T3, T4, T5, T6, T7> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7>;
			return tuple != null && (comparer.Equals(this.m_Item1, tuple.m_Item1) && comparer.Equals(this.m_Item2, tuple.m_Item2) && comparer.Equals(this.m_Item3, tuple.m_Item3) && comparer.Equals(this.m_Item4, tuple.m_Item4) && comparer.Equals(this.m_Item5, tuple.m_Item5) && comparer.Equals(this.m_Item6, tuple.m_Item6)) && comparer.Equals(this.m_Item7, tuple.m_Item7);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0004275A File Offset: 0x0004095A
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00043874 File Offset: 0x00041A74
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1, T2, T3, T4, T5, T6, T7> tuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7>;
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
			num = comparer.Compare(this.m_Item6, tuple.m_Item6);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.m_Item7, tuple.m_Item7);
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x000427C0 File Offset: 0x000409C0
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0004399C File Offset: 0x00041B9C
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(this.m_Item1), comparer.GetHashCode(this.m_Item2), comparer.GetHashCode(this.m_Item3), comparer.GetHashCode(this.m_Item4), comparer.GetHashCode(this.m_Item5), comparer.GetHashCode(this.m_Item6), comparer.GetHashCode(this.m_Item7));
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x000427E0 File Offset: 0x000409E0
		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00043A28 File Offset: 0x00041C28
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			return ((ITupleInternal)this).ToString(stringBuilder);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00043A4C File Offset: 0x00041C4C
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
			sb.Append(", ");
			sb.Append(this.m_Item7);
			sb.Append(')');
			return sb.ToString();
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x00032282 File Offset: 0x00030482
		int ITuple.Length
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17000158 RID: 344
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
				case 6:
					return this.Item7;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x0400132E RID: 4910
		private readonly T1 m_Item1;

		// Token: 0x0400132F RID: 4911
		private readonly T2 m_Item2;

		// Token: 0x04001330 RID: 4912
		private readonly T3 m_Item3;

		// Token: 0x04001331 RID: 4913
		private readonly T4 m_Item4;

		// Token: 0x04001332 RID: 4914
		private readonly T5 m_Item5;

		// Token: 0x04001333 RID: 4915
		private readonly T6 m_Item6;

		// Token: 0x04001334 RID: 4916
		private readonly T7 m_Item7;
	}
}
