using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x02000028 RID: 40
	public abstract class TableMapperBase<T> : MapperBase<T> where T : TableMapperBase<T>
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00005111 File Offset: 0x00003311
		protected TableMapperBase(Type type) : base(type)
		{
			this.excludes = new List<int>();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005128 File Offset: 0x00003328
		public override void Extract()
		{
			TableAttribute tableAttribute = Attribute.GetCustomAttribute(this.type, typeof(TableAttribute)) as TableAttribute;
			if (tableAttribute != null)
			{
				if (tableAttribute.Ignore != null && tableAttribute.Ignore.Length != 0)
				{
					this.Exclude(tableAttribute.Ignore);
				}
				base.SafeMode = tableAttribute.SafeMode;
			}
			base.Extract();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005184 File Offset: 0x00003384
		public T Exclude(params int[] rows)
		{
			if (rows == null || rows.Length == 0)
			{
				throw new ArgumentException("Rows must be specified");
			}
			for (int i = 0; i < rows.Length; i++)
			{
				if (rows[i] < 1)
				{
					throw new ArgumentException("One-based row index must be greater than 0");
				}
				rows[i]--;
			}
			this.excludes.AddRange(rows);
			return (T)((object)this);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000051E0 File Offset: 0x000033E0
		public T Include(params int[] rows)
		{
			if (rows == null || rows.Length == 0)
			{
				throw new ArgumentException("Rows must be specified");
			}
			for (int j = 0; j < rows.Length; j++)
			{
				if (rows[j] < 1)
				{
					throw new ArgumentException("One-based row index must be greater than 0");
				}
				rows[j]--;
			}
			this.excludes.RemoveAll((int i) => rows.Contains(i));
			return (T)((object)this);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000526E File Offset: 0x0000346E
		public T IncludeAll()
		{
			this.excludes.Clear();
			return (T)((object)this);
		}

		// Token: 0x0400001D RID: 29
		protected readonly List<int> excludes;

		// Token: 0x0400001E RID: 30
		protected int index;
	}
}
