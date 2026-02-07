using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.WriteAdapter
{
	// Token: 0x02000021 RID: 33
	internal class EnumerableWriteAdapter : IMiniExcelWriteAdapter
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x000042B6 File Offset: 0x000024B6
		public EnumerableWriteAdapter(IEnumerable values, Configuration configuration)
		{
			this._values = values;
			this._configuration = configuration;
			this._genericType = TypeHelper.GetGenericIEnumerables(values).FirstOrDefault<Type>();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000042E0 File Offset: 0x000024E0
		public bool TryGetKnownCount(out int count)
		{
			count = 0;
			ICollection collection = this._values as ICollection;
			if (collection != null)
			{
				count = collection.Count;
				return true;
			}
			return false;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000430C File Offset: 0x0000250C
		public List<ExcelColumnInfo> GetColumns()
		{
			List<ExcelColumnInfo> result;
			if (CustomPropertyHelper.TryGetTypeColumnInfo(this._genericType, this._configuration, out result))
			{
				return result;
			}
			this._enumerator = this._values.GetEnumerator();
			if (!this._enumerator.MoveNext())
			{
				this._empty = true;
				return null;
			}
			return CustomPropertyHelper.GetColumnInfoFromValue(this._enumerator.Current, this._configuration);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000436D File Offset: 0x0000256D
		public IEnumerable<IEnumerable<CellWriteInfo>> GetRows(List<ExcelColumnInfo> props, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (this._empty)
			{
				yield break;
			}
			if (this._enumerator == null)
			{
				this._enumerator = this._values.GetEnumerator();
				if (!this._enumerator.MoveNext())
				{
					yield break;
				}
			}
			do
			{
				cancellationToken.ThrowIfCancellationRequested();
				yield return EnumerableWriteAdapter.GetRowValues(this._enumerator.Current, props);
			}
			while (this._enumerator.MoveNext());
			yield break;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000438B File Offset: 0x0000258B
		public static IEnumerable<CellWriteInfo> GetRowValues(object currentValue, List<ExcelColumnInfo> props)
		{
			int column = 1;
			foreach (ExcelColumnInfo excelColumnInfo in props)
			{
				object value;
				if (excelColumnInfo == null)
				{
					value = null;
				}
				else
				{
					IDictionary<string, object> dictionary = currentValue as IDictionary<string, object>;
					if (dictionary != null)
					{
						value = dictionary[excelColumnInfo.Key.ToString()];
					}
					else
					{
						IDictionary dictionary2 = currentValue as IDictionary;
						if (dictionary2 != null)
						{
							value = dictionary2[excelColumnInfo.Key];
						}
						else
						{
							value = excelColumnInfo.Property.GetValue(currentValue);
						}
					}
				}
				yield return new CellWriteInfo(value, column, excelColumnInfo);
				int num = column;
				column = num + 1;
			}
			List<ExcelColumnInfo>.Enumerator enumerator = default(List<ExcelColumnInfo>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04000035 RID: 53
		private readonly IEnumerable _values;

		// Token: 0x04000036 RID: 54
		private readonly Configuration _configuration;

		// Token: 0x04000037 RID: 55
		private readonly Type _genericType;

		// Token: 0x04000038 RID: 56
		private IEnumerator _enumerator;

		// Token: 0x04000039 RID: 57
		private bool _empty;
	}
}
