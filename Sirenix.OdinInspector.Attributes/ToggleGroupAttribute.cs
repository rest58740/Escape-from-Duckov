using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000072 RID: 114
	[AttributeUsage(32767, AllowMultiple = true, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class ToggleGroupAttribute : PropertyGroupAttribute
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00003CD3 File Offset: 0x00001ED3
		public ToggleGroupAttribute(string toggleMemberName, float order = 0f, string groupTitle = null) : base(toggleMemberName, order)
		{
			this.ToggleGroupTitle = groupTitle;
			this.CollapseOthersOnExpand = true;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00003CEB File Offset: 0x00001EEB
		public ToggleGroupAttribute(string toggleMemberName, string groupTitle) : this(toggleMemberName, 0f, groupTitle)
		{
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00003CD3 File Offset: 0x00001ED3
		[Obsolete("Use [ToggleGroup(\"toggleMemberName\", groupTitle: \"$titleStringMemberName\")] instead")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ToggleGroupAttribute(string toggleMemberName, float order, string groupTitle, string titleStringMemberName) : base(toggleMemberName, order)
		{
			this.ToggleGroupTitle = groupTitle;
			this.CollapseOthersOnExpand = true;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00003CFA File Offset: 0x00001EFA
		public string ToggleMemberName
		{
			get
			{
				return this.GroupName;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00003D02 File Offset: 0x00001F02
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00003D0A File Offset: 0x00001F0A
		[Obsolete("Add a $ infront of group title instead, i.e: \"$MyStringMember\".")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string TitleStringMemberName { get; set; }

		// Token: 0x0600017F RID: 383 RVA: 0x00003D14 File Offset: 0x00001F14
		protected override void CombineValuesWith(PropertyGroupAttribute other)
		{
			ToggleGroupAttribute toggleGroupAttribute = other as ToggleGroupAttribute;
			if (this.ToggleGroupTitle == null)
			{
				this.ToggleGroupTitle = toggleGroupAttribute.ToggleGroupTitle;
			}
			else if (toggleGroupAttribute.ToggleGroupTitle == null)
			{
				toggleGroupAttribute.ToggleGroupTitle = this.ToggleGroupTitle;
			}
			this.CollapseOthersOnExpand = (this.CollapseOthersOnExpand && toggleGroupAttribute.CollapseOthersOnExpand);
			toggleGroupAttribute.CollapseOthersOnExpand = this.CollapseOthersOnExpand;
		}

		// Token: 0x04000146 RID: 326
		public string ToggleGroupTitle;

		// Token: 0x04000147 RID: 327
		public bool CollapseOthersOnExpand;
	}
}
