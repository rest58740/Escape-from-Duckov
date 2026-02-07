using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x02000A02 RID: 2562
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	[Conditional("CODE_ANALYSIS")]
	public sealed class SuppressMessageAttribute : Attribute
	{
		// Token: 0x06005B4E RID: 23374 RVA: 0x00134935 File Offset: 0x00132B35
		public SuppressMessageAttribute(string category, string checkId)
		{
			this.Category = category;
			this.CheckId = checkId;
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06005B4F RID: 23375 RVA: 0x0013494B File Offset: 0x00132B4B
		public string Category { get; }

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06005B50 RID: 23376 RVA: 0x00134953 File Offset: 0x00132B53
		public string CheckId { get; }

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06005B51 RID: 23377 RVA: 0x0013495B File Offset: 0x00132B5B
		// (set) Token: 0x06005B52 RID: 23378 RVA: 0x00134963 File Offset: 0x00132B63
		public string Scope { get; set; }

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06005B53 RID: 23379 RVA: 0x0013496C File Offset: 0x00132B6C
		// (set) Token: 0x06005B54 RID: 23380 RVA: 0x00134974 File Offset: 0x00132B74
		public string Target { get; set; }

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06005B55 RID: 23381 RVA: 0x0013497D File Offset: 0x00132B7D
		// (set) Token: 0x06005B56 RID: 23382 RVA: 0x00134985 File Offset: 0x00132B85
		public string MessageId { get; set; }

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06005B57 RID: 23383 RVA: 0x0013498E File Offset: 0x00132B8E
		// (set) Token: 0x06005B58 RID: 23384 RVA: 0x00134996 File Offset: 0x00132B96
		public string Justification { get; set; }
	}
}
