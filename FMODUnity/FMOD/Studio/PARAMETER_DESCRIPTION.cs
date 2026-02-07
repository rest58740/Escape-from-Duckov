using System;

namespace FMOD.Studio
{
	// Token: 0x020000D8 RID: 216
	public struct PARAMETER_DESCRIPTION
	{
		// Token: 0x040004DB RID: 1243
		public StringWrapper name;

		// Token: 0x040004DC RID: 1244
		public PARAMETER_ID id;

		// Token: 0x040004DD RID: 1245
		public float minimum;

		// Token: 0x040004DE RID: 1246
		public float maximum;

		// Token: 0x040004DF RID: 1247
		public float defaultvalue;

		// Token: 0x040004E0 RID: 1248
		public PARAMETER_TYPE type;

		// Token: 0x040004E1 RID: 1249
		public PARAMETER_FLAGS flags;

		// Token: 0x040004E2 RID: 1250
		public GUID guid;
	}
}
