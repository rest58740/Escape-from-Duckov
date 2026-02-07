using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FlexFramework.Excel
{
	// Token: 0x02000023 RID: 35
	public abstract class MapperBase<T> where T : MapperBase<T>
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00004911 File Offset: 0x00002B11
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00004919 File Offset: 0x00002B19
		public bool SafeMode { get; set; }

		// Token: 0x060000DF RID: 223 RVA: 0x00004924 File Offset: 0x00002B24
		protected MapperBase(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.type = type;
			MemberInfo[] members = this.GetMembers();
			this.mappings = (from m in members
			select new Mapping(m)).ToArray<Mapping>();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004989 File Offset: 0x00002B89
		protected virtual MemberInfo[] GetMembers()
		{
			return FormatterServices.GetSerializableMembers(this.type);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004996 File Offset: 0x00002B96
		protected virtual object CreateInstance()
		{
			return FormatterServices.GetUninitializedObject(this.type);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000049A3 File Offset: 0x00002BA3
		protected virtual void Assign(object obj, MemberInfo[] members, object[] data)
		{
			FormatterServices.PopulateObjectMembers(obj, members, data);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000049B0 File Offset: 0x00002BB0
		public virtual void Extract()
		{
			foreach (Mapping mapping in this.mappings)
			{
				ColumnAttribute columnAttribute = Attribute.GetCustomAttribute(mapping.Member, typeof(ColumnAttribute)) as ColumnAttribute;
				if (columnAttribute == null)
				{
					mapping.Column = 0;
				}
				else
				{
					if (columnAttribute.Column < 1)
					{
						throw new ArgumentException("One-based column index must be greater than 0");
					}
					mapping.Column = columnAttribute.Column;
					mapping.Default = columnAttribute.Default;
					mapping.Fallback = columnAttribute.Fallback;
				}
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004A35 File Offset: 0x00002C35
		protected IEnumerable<object> Cast(Row row)
		{
			foreach (Mapping mapping in from m in this.mappings
			where m.Column > 0
			select m)
			{
				int num = mapping.Column - 1;
				if (num >= row.Count)
				{
					throw new InvalidOperationException(string.Format("Column '{0}' index '{1}' out of range '{2}' on type '{3}'", new object[]
					{
						mapping.Member.Name,
						num,
						row.Count,
						this.type.FullName
					}));
				}
				object obj = null;
				try
				{
					obj = row[num].Convert(mapping.Type);
				}
				catch (FormatException)
				{
					if (mapping.Fallback)
					{
						obj = mapping.Default;
					}
					else
					{
						if (!this.SafeMode)
						{
							throw new FormatException(string.Format("Input cell at {0} was not in the correct format for type {1}", row[num].Address, mapping.Type));
						}
						mapping.Failed = true;
					}
				}
				if (!mapping.Failed)
				{
					yield return obj;
				}
			}
			IEnumerator<Mapping> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004A4C File Offset: 0x00002C4C
		public T Map(string member, int column)
		{
			Mapping mapping = Array.Find<Mapping>(this.mappings, (Mapping m) => m.Member.Name == member);
			if (mapping == null)
			{
				throw new InvalidOperationException(string.Format("Member '{0}' not found in type '{1}'", member, this.type.FullName));
			}
			if (mapping.Column > 0)
			{
				throw new InvalidOperationException(string.Format("Member '{0}' for type '{1}' already mapped to Column '{2}'", member, this.type.FullName, mapping.Column));
			}
			if (column < 1)
			{
				throw new ArgumentException("One-based column index must be greater than 0");
			}
			mapping.Column = column;
			return (T)((object)this);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004AF4 File Offset: 0x00002CF4
		public T Map(string member, int column, object @default)
		{
			Mapping mapping = Array.Find<Mapping>(this.mappings, (Mapping m) => m.Member.Name == member);
			if (mapping == null)
			{
				throw new InvalidOperationException(string.Format("Member '{0}' not found in type '{1}'", member, this.type.FullName));
			}
			if (mapping.Column > 0)
			{
				throw new InvalidOperationException(string.Format("Member '{0}' for type '{1}' already mapped to Column '{2}'", member, this.type.FullName, mapping.Column));
			}
			if (column < 1)
			{
				throw new ArgumentException("One-based column index must be greater than 0");
			}
			mapping.Default = @default;
			mapping.Column = column;
			mapping.Fallback = true;
			return (T)((object)this);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004BA9 File Offset: 0x00002DA9
		public T Map(string member, string column)
		{
			return this.Map(member, Address.ParseColumn(column));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004BB8 File Offset: 0x00002DB8
		public T Map(string member, string column, object @default)
		{
			return this.Map(member, Address.ParseColumn(column), @default);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004BC8 File Offset: 0x00002DC8
		public T Map(string expression)
		{
			if (!Regex.IsMatch(expression, "^(\\w+\\s*:(\\d+|[A-Z]+)\\s*)(,\\s*\\w+\\s*:(\\d+|[A-Z]+)\\s*)*$"))
			{
				throw new FormatException("Invalid mapping expression");
			}
			string[] array = expression.Split(new string[]
			{
				","
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new string[]
				{
					":"
				}, StringSplitOptions.RemoveEmptyEntries);
				string text = array2[1].Trim();
				if (Regex.IsMatch(text, "^\\d+$"))
				{
					this.Map(array2[0].Trim(), int.Parse(text));
				}
				else
				{
					this.Map(array2[0].Trim(), text);
				}
			}
			return (T)((object)this);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004C6C File Offset: 0x00002E6C
		public T Map(IEnumerable<Cell> row)
		{
			foreach (Cell cell in row)
			{
				this.Map(cell.Text, cell.Address.Column);
			}
			return (T)((object)this);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public T Remove(string member)
		{
			Mapping mapping = Array.Find<Mapping>(this.mappings, (Mapping m) => m.Member.Name == member);
			if (mapping == null)
			{
				throw new InvalidOperationException(string.Format("Member '{0}' not found in type '{1}'", member, this.type.FullName));
			}
			mapping.Column = 0;
			return (T)((object)this);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004D31 File Offset: 0x00002F31
		public T Clear()
		{
			Array.ForEach<Mapping>(this.mappings, delegate(Mapping m)
			{
				m.Column = 0;
			});
			return (T)((object)this);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004D64 File Offset: 0x00002F64
		public T Copy(T mapping)
		{
			Array.ForEach<Mapping>(this.mappings, delegate(Mapping m)
			{
				m.Column = 0;
			});
			Mapping[] array = mapping.mappings;
			for (int i = 0; i < array.Length; i++)
			{
				Mapping source = array[i];
				Mapping mapping2 = Array.Find<Mapping>(this.mappings, (Mapping m) => m.Member.Name == source.Member.Name && m.Member.DeclaringType == source.Member.DeclaringType);
				if (mapping2 != null)
				{
					mapping2.Column = source.Column;
					mapping2.Default = source.Default;
					mapping2.Fallback = source.Fallback;
				}
			}
			return (T)((object)this);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004E18 File Offset: 0x00003018
		protected object Instantiate(Row row)
		{
			object obj = this.CreateInstance();
			object[] data = this.Cast(row).ToArray<object>();
			MemberInfo[] members = (from m in this.mappings
			where m.Column > 0 && !m.Failed
			select m.Member).ToArray<MemberInfo>();
			Array.ForEach<Mapping>(this.mappings, delegate(Mapping m)
			{
				m.Failed = false;
			});
			this.Assign(obj, members, data);
			return obj;
		}

		// Token: 0x04000012 RID: 18
		protected readonly Mapping[] mappings;

		// Token: 0x04000013 RID: 19
		protected readonly Type type;
	}
}
