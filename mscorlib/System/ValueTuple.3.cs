using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020001AF RID: 431
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2> : IEquatable<ValueTuple<T1, T2>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple<T1, T2>>, IValueTupleInternal, ITuple
	{
		// Token: 0x06001297 RID: 4759 RVA: 0x000488EA File Offset: 0x00046AEA
		public ValueTuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000488FA File Offset: 0x00046AFA
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1, T2> && this.Equals((ValueTuple<T1, T2>)obj);
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00048912 File Offset: 0x00046B12
		public bool Equals(ValueTuple<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00048944 File Offset: 0x00046B44
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null || !(other is ValueTuple<T1, T2>))
			{
				return false;
			}
			ValueTuple<T1, T2> valueTuple = (ValueTuple<T1, T2>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x000489A4 File Offset: 0x00046BA4
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2>))
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			return this.CompareTo((ValueTuple<T1, T2>)other);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x000489F4 File Offset: 0x00046BF4
		public int CompareTo(ValueTuple<T1, T2> other)
		{
			int num = Comparer<T1>.Default.Compare(this.Item1, other.Item1);
			if (num != 0)
			{
				return num;
			}
			return Comparer<T2>.Default.Compare(this.Item2, other.Item2);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00048A34 File Offset: 0x00046C34
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2>))
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			ValueTuple<T1, T2> valueTuple = (ValueTuple<T1, T2>)other;
			int num = comparer.Compare(this.Item1, valueTuple.Item1);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Item2, valueTuple.Item2);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00048AC0 File Offset: 0x00046CC0
		public override int GetHashCode()
		{
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			int h;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					h = 0;
					goto IL_35;
				}
			}
			h = ptr.GetHashCode();
			IL_35:
			ref T2 ptr2 = ref this.Item2;
			T2 t2 = default(T2);
			int h2;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					h2 = 0;
					goto IL_6A;
				}
			}
			h2 = ptr2.GetHashCode();
			IL_6A:
			return ValueTuple.CombineHashCodes(h, h2);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00048B3C File Offset: 0x00046D3C
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00048B45 File Offset: 0x00046D45
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2));
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00048B3C File Offset: 0x00046D3C
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00048B70 File Offset: 0x00046D70
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			string text;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					text = null;
					goto IL_45;
				}
			}
			text = ptr.ToString();
			IL_45:
			array[num] = text;
			array[2] = ", ";
			int num2 = 3;
			ref T2 ptr2 = ref this.Item2;
			T2 t2 = default(T2);
			string text2;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					text2 = null;
					goto IL_85;
				}
			}
			text2 = ptr2.ToString();
			IL_85:
			array[num2] = text2;
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00048C10 File Offset: 0x00046E10
		string IValueTupleInternal.ToStringEnd()
		{
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			string str;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					str = null;
					goto IL_35;
				}
			}
			str = ptr.ToString();
			IL_35:
			string str2 = ", ";
			ref T2 ptr2 = ref this.Item2;
			T2 t2 = default(T2);
			string str3;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					str3 = null;
					goto IL_6F;
				}
			}
			str3 = ptr2.ToString();
			IL_6F:
			return str + str2 + str3 + ")";
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x00015831 File Offset: 0x00013A31
		int ITuple.Length
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170001B7 RID: 439
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

		// Token: 0x04001367 RID: 4967
		public T1 Item1;

		// Token: 0x04001368 RID: 4968
		public T2 Item2;
	}
}
