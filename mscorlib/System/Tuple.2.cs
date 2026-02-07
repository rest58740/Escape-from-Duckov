using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
	// Token: 0x02000198 RID: 408
	[Serializable]
	public class Tuple<T1> : IStructuralEquatable, IStructuralComparable, IComparable, ITupleInternal, ITuple
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x000426FA File Offset: 0x000408FA
		public T1 Item1
		{
			get
			{
				return this.m_Item1;
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00042702 File Offset: 0x00040902
		public Tuple(T1 item1)
		{
			this.m_Item1 = item1;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00042711 File Offset: 0x00040911
		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00042720 File Offset: 0x00040920
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
			{
				return false;
			}
			Tuple<T1> tuple = other as Tuple<T1>;
			return tuple != null && comparer.Equals(this.m_Item1, tuple.m_Item1);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0004275A File Offset: 0x0004095A
		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00042768 File Offset: 0x00040968
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			Tuple<T1> tuple = other as Tuple<T1>;
			if (tuple == null)
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			return comparer.Compare(this.m_Item1, tuple.m_Item1);
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x000427C0 File Offset: 0x000409C0
		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000427CD File Offset: 0x000409CD
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(this.m_Item1);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000427E0 File Offset: 0x000409E0
		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000427EC File Offset: 0x000409EC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			return ((ITupleInternal)this).ToString(stringBuilder);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0004280F File Offset: 0x00040A0F
		string ITupleInternal.ToString(StringBuilder sb)
		{
			sb.Append(this.m_Item1);
			sb.Append(')');
			return sb.ToString();
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x000040F7 File Offset: 0x000022F7
		int ITuple.Length
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000131 RID: 305
		object ITuple.this[int index]
		{
			get
			{
				if (index != 0)
				{
					throw new IndexOutOfRangeException();
				}
				return this.Item1;
			}
		}

		// Token: 0x04001319 RID: 4889
		private readonly T1 m_Item1;
	}
}
