using System;

namespace FMOD.Studio
{
	// Token: 0x020000DB RID: 219
	public struct SOUND_INFO
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x000052EC File Offset: 0x000034EC
		public string name
		{
			get
			{
				string result;
				using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
				{
					result = (((this.mode & (MODE.OPENMEMORY | MODE.OPENMEMORY_POINT)) == MODE.DEFAULT) ? freeHelper.stringFromNative(this.name_or_data) : string.Empty);
				}
				return result;
			}
		}

		// Token: 0x040004E8 RID: 1256
		public IntPtr name_or_data;

		// Token: 0x040004E9 RID: 1257
		public MODE mode;

		// Token: 0x040004EA RID: 1258
		public CREATESOUNDEXINFO exinfo;

		// Token: 0x040004EB RID: 1259
		public int subsoundindex;
	}
}
