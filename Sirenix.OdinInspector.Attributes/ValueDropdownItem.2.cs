using System;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000080 RID: 128
	public struct ValueDropdownItem<T> : IValueDropdownItem
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x000040EF File Offset: 0x000022EF
		public ValueDropdownItem(string text, T value)
		{
			this.Text = text;
			this.Value = value;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000040FF File Offset: 0x000022FF
		string IValueDropdownItem.GetText()
		{
			return this.Text;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00004107 File Offset: 0x00002307
		object IValueDropdownItem.GetValue()
		{
			return this.Value;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00004114 File Offset: 0x00002314
		public override string ToString()
		{
			string result;
			if ((result = this.Text) == null)
			{
				T value = this.Value;
				result = (((value != null) ? value.ToString() : null) ?? "");
			}
			return result;
		}

		// Token: 0x0400025F RID: 607
		public string Text;

		// Token: 0x04000260 RID: 608
		public T Value;
	}
}
