using System;

namespace EPOOutline
{
	// Token: 0x02000023 RID: 35
	public class SerializedPassInfoAttribute : Attribute
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00006C41 File Offset: 0x00004E41
		public SerializedPassInfoAttribute(string title, string shadersFolder)
		{
			this.Title = title;
			this.ShadersFolder = shadersFolder;
		}

		// Token: 0x040000C9 RID: 201
		public readonly string Title;

		// Token: 0x040000CA RID: 202
		public readonly string ShadersFolder;
	}
}
