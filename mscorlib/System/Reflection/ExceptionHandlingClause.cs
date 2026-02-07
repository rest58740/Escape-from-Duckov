using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020008EE RID: 2286
	[ComVisible(true)]
	[StructLayout(LayoutKind.Sequential)]
	public class ExceptionHandlingClause
	{
		// Token: 0x06004CA2 RID: 19618 RVA: 0x0000259F File Offset: 0x0000079F
		protected ExceptionHandlingClause()
		{
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06004CA3 RID: 19619 RVA: 0x000F301F File Offset: 0x000F121F
		public virtual Type CatchType
		{
			get
			{
				return this.catch_type;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x000F3027 File Offset: 0x000F1227
		public virtual int FilterOffset
		{
			get
			{
				return this.filter_offset;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06004CA5 RID: 19621 RVA: 0x000F302F File Offset: 0x000F122F
		public virtual ExceptionHandlingClauseOptions Flags
		{
			get
			{
				return this.flags;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06004CA6 RID: 19622 RVA: 0x000F3037 File Offset: 0x000F1237
		public virtual int HandlerLength
		{
			get
			{
				return this.handler_length;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06004CA7 RID: 19623 RVA: 0x000F303F File Offset: 0x000F123F
		public virtual int HandlerOffset
		{
			get
			{
				return this.handler_offset;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06004CA8 RID: 19624 RVA: 0x000F3047 File Offset: 0x000F1247
		public virtual int TryLength
		{
			get
			{
				return this.try_length;
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x000F304F File Offset: 0x000F124F
		public virtual int TryOffset
		{
			get
			{
				return this.try_offset;
			}
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x000F3058 File Offset: 0x000F1258
		public override string ToString()
		{
			string text = string.Format("Flags={0}, TryOffset={1}, TryLength={2}, HandlerOffset={3}, HandlerLength={4}", new object[]
			{
				this.flags,
				this.try_offset,
				this.try_length,
				this.handler_offset,
				this.handler_length
			});
			if (this.catch_type != null)
			{
				text = string.Format("{0}, CatchType={1}", text, this.catch_type);
			}
			if (this.flags == ExceptionHandlingClauseOptions.Filter)
			{
				text = string.Format("{0}, FilterOffset={1}", text, this.filter_offset);
			}
			return text;
		}

		// Token: 0x0400302B RID: 12331
		internal Type catch_type;

		// Token: 0x0400302C RID: 12332
		internal int filter_offset;

		// Token: 0x0400302D RID: 12333
		internal ExceptionHandlingClauseOptions flags;

		// Token: 0x0400302E RID: 12334
		internal int try_offset;

		// Token: 0x0400302F RID: 12335
		internal int try_length;

		// Token: 0x04003030 RID: 12336
		internal int handler_offset;

		// Token: 0x04003031 RID: 12337
		internal int handler_length;
	}
}
