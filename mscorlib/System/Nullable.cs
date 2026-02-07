using System;
using System.Runtime.Versioning;

namespace System
{
	// Token: 0x02000163 RID: 355
	[NonVersionable]
	[Serializable]
	public struct Nullable<T> where T : struct
	{
		// Token: 0x06000DE8 RID: 3560 RVA: 0x00036019 File Offset: 0x00034219
		[NonVersionable]
		public Nullable(T value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00036029 File Offset: 0x00034229
		public bool HasValue
		{
			[NonVersionable]
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00036031 File Offset: 0x00034231
		public T Value
		{
			get
			{
				if (!this.hasValue)
				{
					ThrowHelper.ThrowInvalidOperationException_InvalidOperation_NoValue();
				}
				return this.value;
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00036046 File Offset: 0x00034246
		[NonVersionable]
		public T GetValueOrDefault()
		{
			return this.value;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0003604E File Offset: 0x0003424E
		[NonVersionable]
		public T GetValueOrDefault(T defaultValue)
		{
			if (!this.hasValue)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00036060 File Offset: 0x00034260
		public override bool Equals(object other)
		{
			if (!this.hasValue)
			{
				return other == null;
			}
			return other != null && this.value.Equals(other);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00036086 File Offset: 0x00034286
		public override int GetHashCode()
		{
			if (!this.hasValue)
			{
				return 0;
			}
			return this.value.GetHashCode();
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x000360A3 File Offset: 0x000342A3
		public override string ToString()
		{
			if (!this.hasValue)
			{
				return "";
			}
			return this.value.ToString();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000360C4 File Offset: 0x000342C4
		[NonVersionable]
		public static implicit operator T?(T value)
		{
			return new T?(value);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x000360CC File Offset: 0x000342CC
		[NonVersionable]
		public static explicit operator T(T? value)
		{
			return value.Value;
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x000360D5 File Offset: 0x000342D5
		private static object Box(T? o)
		{
			if (!o.hasValue)
			{
				return null;
			}
			return o.value;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x000360EC File Offset: 0x000342EC
		private static T? Unbox(object o)
		{
			if (o == null)
			{
				return null;
			}
			return new T?((T)((object)o));
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00036114 File Offset: 0x00034314
		private static T? UnboxExact(object o)
		{
			if (o == null)
			{
				return null;
			}
			if (o.GetType() != typeof(T))
			{
				throw new InvalidCastException();
			}
			return new T?((T)((object)o));
		}

		// Token: 0x04001285 RID: 4741
		private readonly bool hasValue;

		// Token: 0x04001286 RID: 4742
		internal T value;
	}
}
