using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000051 RID: 81
	public struct DSPConnection
	{
		// Token: 0x0600039B RID: 923 RVA: 0x000049C5 File Offset: 0x00002BC5
		public RESULT getInput(out DSP input)
		{
			return DSPConnection.FMOD5_DSPConnection_GetInput(this.handle, out input.handle);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000049D8 File Offset: 0x00002BD8
		public RESULT getOutput(out DSP output)
		{
			return DSPConnection.FMOD5_DSPConnection_GetOutput(this.handle, out output.handle);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000049EB File Offset: 0x00002BEB
		public RESULT setMix(float volume)
		{
			return DSPConnection.FMOD5_DSPConnection_SetMix(this.handle, volume);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000049F9 File Offset: 0x00002BF9
		public RESULT getMix(out float volume)
		{
			return DSPConnection.FMOD5_DSPConnection_GetMix(this.handle, out volume);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00004A07 File Offset: 0x00002C07
		public RESULT setMixMatrix(float[] matrix, int outchannels, int inchannels, int inchannel_hop = 0)
		{
			return DSPConnection.FMOD5_DSPConnection_SetMixMatrix(this.handle, matrix, outchannels, inchannels, inchannel_hop);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00004A19 File Offset: 0x00002C19
		public RESULT getMixMatrix(float[] matrix, out int outchannels, out int inchannels, int inchannel_hop = 0)
		{
			return DSPConnection.FMOD5_DSPConnection_GetMixMatrix(this.handle, matrix, out outchannels, out inchannels, inchannel_hop);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00004A2B File Offset: 0x00002C2B
		public RESULT getType(out DSPCONNECTION_TYPE type)
		{
			return DSPConnection.FMOD5_DSPConnection_GetType(this.handle, out type);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00004A39 File Offset: 0x00002C39
		public RESULT setUserData(IntPtr userdata)
		{
			return DSPConnection.FMOD5_DSPConnection_SetUserData(this.handle, userdata);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00004A47 File Offset: 0x00002C47
		public RESULT getUserData(out IntPtr userdata)
		{
			return DSPConnection.FMOD5_DSPConnection_GetUserData(this.handle, out userdata);
		}

		// Token: 0x060003A4 RID: 932
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_GetInput(IntPtr dspconnection, out IntPtr input);

		// Token: 0x060003A5 RID: 933
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_GetOutput(IntPtr dspconnection, out IntPtr output);

		// Token: 0x060003A6 RID: 934
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_SetMix(IntPtr dspconnection, float volume);

		// Token: 0x060003A7 RID: 935
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_GetMix(IntPtr dspconnection, out float volume);

		// Token: 0x060003A8 RID: 936
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_SetMixMatrix(IntPtr dspconnection, float[] matrix, int outchannels, int inchannels, int inchannel_hop);

		// Token: 0x060003A9 RID: 937
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_GetMixMatrix(IntPtr dspconnection, float[] matrix, out int outchannels, out int inchannels, int inchannel_hop);

		// Token: 0x060003AA RID: 938
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_GetType(IntPtr dspconnection, out DSPCONNECTION_TYPE type);

		// Token: 0x060003AB RID: 939
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_SetUserData(IntPtr dspconnection, IntPtr userdata);

		// Token: 0x060003AC RID: 940
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSPConnection_GetUserData(IntPtr dspconnection, out IntPtr userdata);

		// Token: 0x060003AD RID: 941 RVA: 0x00004A55 File Offset: 0x00002C55
		public DSPConnection(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00004A5E File Offset: 0x00002C5E
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00004A70 File Offset: 0x00002C70
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x04000264 RID: 612
		public IntPtr handle;
	}
}
