using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000061 RID: 97
	[DontApplyToListElements]
	[AttributeUsage(32767, AllowMultiple = true, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class ShowIfAttribute : Attribute
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000378A File Offset: 0x0000198A
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00003792 File Offset: 0x00001992
		[Obsolete("Use the Condition member instead.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string MemberName
		{
			get
			{
				return this.Condition;
			}
			set
			{
				this.Condition = value;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000379B File Offset: 0x0000199B
		public ShowIfAttribute(string condition, bool animate = true)
		{
			this.Condition = condition;
			this.Animate = animate;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000037B1 File Offset: 0x000019B1
		public ShowIfAttribute(string condition, object optionalValue, bool animate = true)
		{
			this.Condition = condition;
			this.Value = optionalValue;
			this.Animate = animate;
		}

		// Token: 0x04000109 RID: 265
		public string Condition;

		// Token: 0x0400010A RID: 266
		public object Value;

		// Token: 0x0400010B RID: 267
		public bool Animate;
	}
}
