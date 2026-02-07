using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000053 RID: 83
	public struct Reverb3D
	{
		// Token: 0x060003DB RID: 987 RVA: 0x00004BD5 File Offset: 0x00002DD5
		public RESULT release()
		{
			return Reverb3D.FMOD5_Reverb3D_Release(this.handle);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00004BE2 File Offset: 0x00002DE2
		public RESULT set3DAttributes(ref VECTOR position, float mindistance, float maxdistance)
		{
			return Reverb3D.FMOD5_Reverb3D_Set3DAttributes(this.handle, ref position, mindistance, maxdistance);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00004BF2 File Offset: 0x00002DF2
		public RESULT get3DAttributes(ref VECTOR position, ref float mindistance, ref float maxdistance)
		{
			return Reverb3D.FMOD5_Reverb3D_Get3DAttributes(this.handle, ref position, ref mindistance, ref maxdistance);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00004C02 File Offset: 0x00002E02
		public RESULT setProperties(ref REVERB_PROPERTIES properties)
		{
			return Reverb3D.FMOD5_Reverb3D_SetProperties(this.handle, ref properties);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00004C10 File Offset: 0x00002E10
		public RESULT getProperties(ref REVERB_PROPERTIES properties)
		{
			return Reverb3D.FMOD5_Reverb3D_GetProperties(this.handle, ref properties);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00004C1E File Offset: 0x00002E1E
		public RESULT setActive(bool active)
		{
			return Reverb3D.FMOD5_Reverb3D_SetActive(this.handle, active);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00004C2C File Offset: 0x00002E2C
		public RESULT getActive(out bool active)
		{
			return Reverb3D.FMOD5_Reverb3D_GetActive(this.handle, out active);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00004C3A File Offset: 0x00002E3A
		public RESULT setUserData(IntPtr userdata)
		{
			return Reverb3D.FMOD5_Reverb3D_SetUserData(this.handle, userdata);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00004C48 File Offset: 0x00002E48
		public RESULT getUserData(out IntPtr userdata)
		{
			return Reverb3D.FMOD5_Reverb3D_GetUserData(this.handle, out userdata);
		}

		// Token: 0x060003E4 RID: 996
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_Release(IntPtr reverb3d);

		// Token: 0x060003E5 RID: 997
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_Set3DAttributes(IntPtr reverb3d, ref VECTOR position, float mindistance, float maxdistance);

		// Token: 0x060003E6 RID: 998
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_Get3DAttributes(IntPtr reverb3d, ref VECTOR position, ref float mindistance, ref float maxdistance);

		// Token: 0x060003E7 RID: 999
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_SetProperties(IntPtr reverb3d, ref REVERB_PROPERTIES properties);

		// Token: 0x060003E8 RID: 1000
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_GetProperties(IntPtr reverb3d, ref REVERB_PROPERTIES properties);

		// Token: 0x060003E9 RID: 1001
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_SetActive(IntPtr reverb3d, bool active);

		// Token: 0x060003EA RID: 1002
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_GetActive(IntPtr reverb3d, out bool active);

		// Token: 0x060003EB RID: 1003
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_SetUserData(IntPtr reverb3d, IntPtr userdata);

		// Token: 0x060003EC RID: 1004
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Reverb3D_GetUserData(IntPtr reverb3d, out IntPtr userdata);

		// Token: 0x060003ED RID: 1005 RVA: 0x00004C56 File Offset: 0x00002E56
		public Reverb3D(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00004C5F File Offset: 0x00002E5F
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00004C71 File Offset: 0x00002E71
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x04000266 RID: 614
		public IntPtr handle;
	}
}
