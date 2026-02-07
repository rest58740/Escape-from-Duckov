using System;
using System.Reflection;

namespace FlexFramework.Excel
{
	// Token: 0x02000024 RID: 36
	public class Mapping
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004EC1 File Offset: 0x000030C1
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004EC9 File Offset: 0x000030C9
		public MemberInfo Member { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004ED2 File Offset: 0x000030D2
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004EDA File Offset: 0x000030DA
		public Type Type { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004EE3 File Offset: 0x000030E3
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00004EEB File Offset: 0x000030EB
		public int Column
		{
			get
			{
				return this._column;
			}
			set
			{
				this._column = Math.Max(0, value);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004EFA File Offset: 0x000030FA
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004F02 File Offset: 0x00003102
		public object Default
		{
			get
			{
				return this._default;
			}
			set
			{
				if (value != null && !Validator.CanCast(value.GetType(), this.Type))
				{
					throw new ArgumentException(string.Format("Incompatible value '{0}' for type '{1}'", value, this.Type.FullName), "default");
				}
				this._default = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004F42 File Offset: 0x00003142
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004F4A File Offset: 0x0000314A
		public bool Fallback { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004F53 File Offset: 0x00003153
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004F5B File Offset: 0x0000315B
		public bool Failed { get; set; }

		// Token: 0x060000FB RID: 251 RVA: 0x00004F64 File Offset: 0x00003164
		public Mapping(MemberInfo member)
		{
			this.Member = member;
			MemberTypes memberType = member.MemberType;
			if (memberType == MemberTypes.Field)
			{
				this.Type = ((FieldInfo)member).FieldType;
				return;
			}
			if (memberType != MemberTypes.Property)
			{
				throw new NotSupportedException();
			}
			this.Type = ((PropertyInfo)member).PropertyType;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004FB9 File Offset: 0x000031B9
		public Mapping(MemberInfo member, int column) : this(member)
		{
			this.Column = column;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004FCC File Offset: 0x000031CC
		public Mapping(MemberInfo member, int column, object @default) : this(member, column)
		{
			if (@default == null && this.Type.IsValueType)
			{
				throw new ArgumentException(string.Format("null cannot be assigned to value type '{0}'", this.Type.FullName), "default");
			}
			this.Default = @default;
			this.Fallback = true;
		}

		// Token: 0x04000015 RID: 21
		private object _default;

		// Token: 0x04000016 RID: 22
		private int _column;
	}
}
