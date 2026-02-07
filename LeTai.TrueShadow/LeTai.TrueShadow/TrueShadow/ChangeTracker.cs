using System;
using System.Collections.Generic;

namespace LeTai.TrueShadow
{
	// Token: 0x02000019 RID: 25
	internal class ChangeTracker<T> : IChangeTracker
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00006440 File Offset: 0x00004640
		public ChangeTracker(Func<T> getValue, Func<T, T> onChange, Func<T, T, bool> compare = null)
		{
			this.getValue = getValue;
			this.onChange = onChange;
			this.compare = (compare ?? new Func<T, T, bool>(EqualityComparer<T>.Default.Equals));
			this.previousValue = this.getValue();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000648E File Offset: 0x0000468E
		public void Forget()
		{
			this.previousValue = this.getValue();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000064A4 File Offset: 0x000046A4
		public void Check()
		{
			T t = this.getValue();
			if (!this.compare(this.previousValue, t))
			{
				this.previousValue = this.onChange(t);
			}
		}

		// Token: 0x040000AB RID: 171
		private T previousValue;

		// Token: 0x040000AC RID: 172
		private readonly Func<T> getValue;

		// Token: 0x040000AD RID: 173
		private readonly Func<T, T> onChange;

		// Token: 0x040000AE RID: 174
		private readonly Func<T, T, bool> compare;
	}
}
