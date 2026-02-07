using System;
using System.Collections;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020001AD RID: 429
	[Serializable]
	public struct ValueTuple : IEquatable<ValueTuple>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple>, IValueTupleInternal, ITuple
	{
		// Token: 0x0600126C RID: 4716 RVA: 0x0004851F File Offset: 0x0004671F
		public override bool Equals(object obj)
		{
			return obj is ValueTuple;
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool Equals(ValueTuple other)
		{
			return true;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0004851F File Offset: 0x0004671F
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			return other is ValueTuple;
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0004852A File Offset: 0x0004672A
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple))
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			return 0;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public int CompareTo(ValueTuple other)
		{
			return 0;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0004852A File Offset: 0x0004672A
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple))
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			return 0;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return 0;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return 0;
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00048564 File Offset: 0x00046764
		public override string ToString()
		{
			return "()";
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0004856B File Offset: 0x0004676B
		string IValueTupleInternal.ToStringEnd()
		{
			return ")";
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		int ITuple.Length
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170001B3 RID: 435
		object ITuple.this[int index]
		{
			get
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0004857C File Offset: 0x0004677C
		public static ValueTuple Create()
		{
			return default(ValueTuple);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00048592 File Offset: 0x00046792
		public static ValueTuple<T1> Create<T1>(T1 item1)
		{
			return new ValueTuple<T1>(item1);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0004859A File Offset: 0x0004679A
		public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new ValueTuple<T1, T2>(item1, item2);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000485A3 File Offset: 0x000467A3
		public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new ValueTuple<T1, T2, T3>(item1, item2, item3);
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x000485AD File Offset: 0x000467AD
		public static ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new ValueTuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000485B8 File Offset: 0x000467B8
		public static ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			return new ValueTuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x000485C5 File Offset: 0x000467C5
		public static ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			return new ValueTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x000485D4 File Offset: 0x000467D4
		public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			return new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x000485E5 File Offset: 0x000467E5
		public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			return new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(item1, item2, item3, item4, item5, item6, item7, ValueTuple.Create<T8>(item8));
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x000485FD File Offset: 0x000467FD
		internal static int CombineHashCodes(int h1, int h2)
		{
			return System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.RandomSeed, h1), h2);
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00048610 File Offset: 0x00046810
		internal static int CombineHashCodes(int h1, int h2, int h3)
		{
			return System.Numerics.Hashing.HashHelpers.Combine(ValueTuple.CombineHashCodes(h1, h2), h3);
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0004861F File Offset: 0x0004681F
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return System.Numerics.Hashing.HashHelpers.Combine(ValueTuple.CombineHashCodes(h1, h2, h3), h4);
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0004862F File Offset: 0x0004682F
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return System.Numerics.Hashing.HashHelpers.Combine(ValueTuple.CombineHashCodes(h1, h2, h3, h4), h5);
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00048641 File Offset: 0x00046841
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return System.Numerics.Hashing.HashHelpers.Combine(ValueTuple.CombineHashCodes(h1, h2, h3, h4, h5), h6);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00048655 File Offset: 0x00046855
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return System.Numerics.Hashing.HashHelpers.Combine(ValueTuple.CombineHashCodes(h1, h2, h3, h4, h5, h6), h7);
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0004866B File Offset: 0x0004686B
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return System.Numerics.Hashing.HashHelpers.Combine(ValueTuple.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7), h8);
		}
	}
}
