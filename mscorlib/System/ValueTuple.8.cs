using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020001B4 RID: 436
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7> : IEquatable<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>, IValueTupleInternal, ITuple
	{
		// Token: 0x060012E2 RID: 4834 RVA: 0x0004AA52 File Offset: 0x00048C52
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0004AA89 File Offset: 0x00048C89
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1, T2, T3, T4, T5, T6, T7> && this.Equals((ValueTuple<T1, T2, T3, T4, T5, T6, T7>)obj);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0004AAA4 File Offset: 0x00048CA4
		public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6, T7> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4) && EqualityComparer<T5>.Default.Equals(this.Item5, other.Item5) && EqualityComparer<T6>.Default.Equals(this.Item6, other.Item6) && EqualityComparer<T7>.Default.Equals(this.Item7, other.Item7);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0004AB5C File Offset: 0x00048D5C
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null || !(other is ValueTuple<T1, T2, T3, T4, T5, T6, T7>))
			{
				return false;
			}
			ValueTuple<T1, T2, T3, T4, T5, T6, T7> valueTuple = (ValueTuple<T1, T2, T3, T4, T5, T6, T7>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3) && comparer.Equals(this.Item4, valueTuple.Item4) && comparer.Equals(this.Item5, valueTuple.Item5) && comparer.Equals(this.Item6, valueTuple.Item6) && comparer.Equals(this.Item7, valueTuple.Item7);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0004AC58 File Offset: 0x00048E58
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2, T3, T4, T5, T6, T7>))
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			return this.CompareTo((ValueTuple<T1, T2, T3, T4, T5, T6, T7>)other);
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0004ACA8 File Offset: 0x00048EA8
		public int CompareTo(ValueTuple<T1, T2, T3, T4, T5, T6, T7> other)
		{
			int num = Comparer<T1>.Default.Compare(this.Item1, other.Item1);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T2>.Default.Compare(this.Item2, other.Item2);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T3>.Default.Compare(this.Item3, other.Item3);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T4>.Default.Compare(this.Item4, other.Item4);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T5>.Default.Compare(this.Item5, other.Item5);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T6>.Default.Compare(this.Item6, other.Item6);
			if (num != 0)
			{
				return num;
			}
			return Comparer<T7>.Default.Compare(this.Item7, other.Item7);
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0004AD74 File Offset: 0x00048F74
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2, T3, T4, T5, T6, T7>))
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			ValueTuple<T1, T2, T3, T4, T5, T6, T7> valueTuple = (ValueTuple<T1, T2, T3, T4, T5, T6, T7>)other;
			int num = comparer.Compare(this.Item1, valueTuple.Item1);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item2, valueTuple.Item2);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item3, valueTuple.Item3);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item4, valueTuple.Item4);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item5, valueTuple.Item5);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item6, valueTuple.Item6);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Item7, valueTuple.Item7);
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0004AEA8 File Offset: 0x000490A8
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
			ref T3 ptr3 = ref this.Item3;
			T3 t3 = default(T3);
			int h3;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					h3 = 0;
					goto IL_9F;
				}
			}
			h3 = ptr3.GetHashCode();
			IL_9F:
			ref T4 ptr4 = ref this.Item4;
			T4 t4 = default(T4);
			int h4;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					h4 = 0;
					goto IL_D4;
				}
			}
			h4 = ptr4.GetHashCode();
			IL_D4:
			ref T5 ptr5 = ref this.Item5;
			T5 t5 = default(T5);
			int h5;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					h5 = 0;
					goto IL_10C;
				}
			}
			h5 = ptr5.GetHashCode();
			IL_10C:
			ref T6 ptr6 = ref this.Item6;
			T6 t6 = default(T6);
			int h6;
			if (t6 == null)
			{
				t6 = this.Item6;
				ptr6 = ref t6;
				if (t6 == null)
				{
					h6 = 0;
					goto IL_144;
				}
			}
			h6 = ptr6.GetHashCode();
			IL_144:
			ref T7 ptr7 = ref this.Item7;
			T7 t7 = default(T7);
			int h7;
			if (t7 == null)
			{
				t7 = this.Item7;
				ptr7 = ref t7;
				if (t7 == null)
				{
					h7 = 0;
					goto IL_17C;
				}
			}
			h7 = ptr7.GetHashCode();
			IL_17C:
			return ValueTuple.CombineHashCodes(h, h2, h3, h4, h5, h6, h7);
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0004B036 File Offset: 0x00049236
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0004B040 File Offset: 0x00049240
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7));
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0004B036 File Offset: 0x00049236
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0004B0CC File Offset: 0x000492CC
		public override string ToString()
		{
			string[] array = new string[15];
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
					goto IL_46;
				}
			}
			text = ptr.ToString();
			IL_46:
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
					goto IL_86;
				}
			}
			text2 = ptr2.ToString();
			IL_86:
			array[num2] = text2;
			array[4] = ", ";
			int num3 = 5;
			ref T3 ptr3 = ref this.Item3;
			T3 t3 = default(T3);
			string text3;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					text3 = null;
					goto IL_C6;
				}
			}
			text3 = ptr3.ToString();
			IL_C6:
			array[num3] = text3;
			array[6] = ", ";
			int num4 = 7;
			ref T4 ptr4 = ref this.Item4;
			T4 t4 = default(T4);
			string text4;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					text4 = null;
					goto IL_106;
				}
			}
			text4 = ptr4.ToString();
			IL_106:
			array[num4] = text4;
			array[8] = ", ";
			int num5 = 9;
			ref T5 ptr5 = ref this.Item5;
			T5 t5 = default(T5);
			string text5;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					text5 = null;
					goto IL_14A;
				}
			}
			text5 = ptr5.ToString();
			IL_14A:
			array[num5] = text5;
			array[10] = ", ";
			int num6 = 11;
			ref T6 ptr6 = ref this.Item6;
			T6 t6 = default(T6);
			string text6;
			if (t6 == null)
			{
				t6 = this.Item6;
				ptr6 = ref t6;
				if (t6 == null)
				{
					text6 = null;
					goto IL_18F;
				}
			}
			text6 = ptr6.ToString();
			IL_18F:
			array[num6] = text6;
			array[12] = ", ";
			int num7 = 13;
			ref T7 ptr7 = ref this.Item7;
			T7 t7 = default(T7);
			string text7;
			if (t7 == null)
			{
				t7 = this.Item7;
				ptr7 = ref t7;
				if (t7 == null)
				{
					text7 = null;
					goto IL_1D4;
				}
			}
			text7 = ptr7.ToString();
			IL_1D4:
			array[num7] = text7;
			array[14] = ")";
			return string.Concat(array);
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0004B2BC File Offset: 0x000494BC
		string IValueTupleInternal.ToStringEnd()
		{
			string[] array = new string[14];
			int num = 0;
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
					goto IL_3E;
				}
			}
			text = ptr.ToString();
			IL_3E:
			array[num] = text;
			array[1] = ", ";
			int num2 = 2;
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
					goto IL_7E;
				}
			}
			text2 = ptr2.ToString();
			IL_7E:
			array[num2] = text2;
			array[3] = ", ";
			int num3 = 4;
			ref T3 ptr3 = ref this.Item3;
			T3 t3 = default(T3);
			string text3;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					text3 = null;
					goto IL_BE;
				}
			}
			text3 = ptr3.ToString();
			IL_BE:
			array[num3] = text3;
			array[5] = ", ";
			int num4 = 6;
			ref T4 ptr4 = ref this.Item4;
			T4 t4 = default(T4);
			string text4;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					text4 = null;
					goto IL_FE;
				}
			}
			text4 = ptr4.ToString();
			IL_FE:
			array[num4] = text4;
			array[7] = ", ";
			int num5 = 8;
			ref T5 ptr5 = ref this.Item5;
			T5 t5 = default(T5);
			string text5;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					text5 = null;
					goto IL_141;
				}
			}
			text5 = ptr5.ToString();
			IL_141:
			array[num5] = text5;
			array[9] = ", ";
			int num6 = 10;
			ref T6 ptr6 = ref this.Item6;
			T6 t6 = default(T6);
			string text6;
			if (t6 == null)
			{
				t6 = this.Item6;
				ptr6 = ref t6;
				if (t6 == null)
				{
					text6 = null;
					goto IL_186;
				}
			}
			text6 = ptr6.ToString();
			IL_186:
			array[num6] = text6;
			array[11] = ", ";
			int num7 = 12;
			ref T7 ptr7 = ref this.Item7;
			T7 t7 = default(T7);
			string text7;
			if (t7 == null)
			{
				t7 = this.Item7;
				ptr7 = ref t7;
				if (t7 == null)
				{
					text7 = null;
					goto IL_1CB;
				}
			}
			text7 = ptr7.ToString();
			IL_1CB:
			array[num7] = text7;
			array[13] = ")";
			return string.Concat(array);
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x00032282 File Offset: 0x00030482
		int ITuple.Length
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x170001C1 RID: 449
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

		// Token: 0x0400137B RID: 4987
		public T1 Item1;

		// Token: 0x0400137C RID: 4988
		public T2 Item2;

		// Token: 0x0400137D RID: 4989
		public T3 Item3;

		// Token: 0x0400137E RID: 4990
		public T4 Item4;

		// Token: 0x0400137F RID: 4991
		public T5 Item5;

		// Token: 0x04001380 RID: 4992
		public T6 Item6;

		// Token: 0x04001381 RID: 4993
		public T7 Item7;
	}
}
