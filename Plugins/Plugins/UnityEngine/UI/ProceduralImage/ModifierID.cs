using System;

namespace UnityEngine.UI.ProceduralImage
{
	// Token: 0x0200000E RID: 14
	[AttributeUsage(4)]
	public class ModifierID : Attribute
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002C62 File Offset: 0x00000E62
		public ModifierID(string name)
		{
			this.name = name;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002C71 File Offset: 0x00000E71
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x04000015 RID: 21
		private string name;
	}
}
