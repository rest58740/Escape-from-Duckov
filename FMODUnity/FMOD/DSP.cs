using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000050 RID: 80
	public struct DSP
	{
		// Token: 0x0600033F RID: 831 RVA: 0x00004634 File Offset: 0x00002834
		public RESULT release()
		{
			return DSP.FMOD5_DSP_Release(this.handle);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00004641 File Offset: 0x00002841
		public RESULT getSystemObject(out System system)
		{
			return DSP.FMOD5_DSP_GetSystemObject(this.handle, out system.handle);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00004654 File Offset: 0x00002854
		public RESULT addInput(DSP input)
		{
			return DSP.FMOD5_DSP_AddInput(this.handle, input.handle, IntPtr.Zero, DSPCONNECTION_TYPE.STANDARD);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000466D File Offset: 0x0000286D
		public RESULT addInput(DSP input, out DSPConnection connection, DSPCONNECTION_TYPE type = DSPCONNECTION_TYPE.STANDARD)
		{
			return DSP.FMOD5_DSP_AddInput(this.handle, input.handle, out connection.handle, type);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00004687 File Offset: 0x00002887
		public RESULT disconnectFrom(DSP target, DSPConnection connection)
		{
			return DSP.FMOD5_DSP_DisconnectFrom(this.handle, target.handle, connection.handle);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000046A0 File Offset: 0x000028A0
		public RESULT disconnectAll(bool inputs, bool outputs)
		{
			return DSP.FMOD5_DSP_DisconnectAll(this.handle, inputs, outputs);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000046AF File Offset: 0x000028AF
		public RESULT getNumInputs(out int numinputs)
		{
			return DSP.FMOD5_DSP_GetNumInputs(this.handle, out numinputs);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000046BD File Offset: 0x000028BD
		public RESULT getNumOutputs(out int numoutputs)
		{
			return DSP.FMOD5_DSP_GetNumOutputs(this.handle, out numoutputs);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000046CB File Offset: 0x000028CB
		public RESULT getInput(int index, out DSP input, out DSPConnection inputconnection)
		{
			return DSP.FMOD5_DSP_GetInput(this.handle, index, out input.handle, out inputconnection.handle);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000046E5 File Offset: 0x000028E5
		public RESULT getOutput(int index, out DSP output, out DSPConnection outputconnection)
		{
			return DSP.FMOD5_DSP_GetOutput(this.handle, index, out output.handle, out outputconnection.handle);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000046FF File Offset: 0x000028FF
		public RESULT setActive(bool active)
		{
			return DSP.FMOD5_DSP_SetActive(this.handle, active);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000470D File Offset: 0x0000290D
		public RESULT getActive(out bool active)
		{
			return DSP.FMOD5_DSP_GetActive(this.handle, out active);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000471B File Offset: 0x0000291B
		public RESULT setBypass(bool bypass)
		{
			return DSP.FMOD5_DSP_SetBypass(this.handle, bypass);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00004729 File Offset: 0x00002929
		public RESULT getBypass(out bool bypass)
		{
			return DSP.FMOD5_DSP_GetBypass(this.handle, out bypass);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00004737 File Offset: 0x00002937
		public RESULT setWetDryMix(float prewet, float postwet, float dry)
		{
			return DSP.FMOD5_DSP_SetWetDryMix(this.handle, prewet, postwet, dry);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00004747 File Offset: 0x00002947
		public RESULT getWetDryMix(out float prewet, out float postwet, out float dry)
		{
			return DSP.FMOD5_DSP_GetWetDryMix(this.handle, out prewet, out postwet, out dry);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00004757 File Offset: 0x00002957
		public RESULT setChannelFormat(CHANNELMASK channelmask, int numchannels, SPEAKERMODE source_speakermode)
		{
			return DSP.FMOD5_DSP_SetChannelFormat(this.handle, channelmask, numchannels, source_speakermode);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00004767 File Offset: 0x00002967
		public RESULT getChannelFormat(out CHANNELMASK channelmask, out int numchannels, out SPEAKERMODE source_speakermode)
		{
			return DSP.FMOD5_DSP_GetChannelFormat(this.handle, out channelmask, out numchannels, out source_speakermode);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00004777 File Offset: 0x00002977
		public RESULT getOutputChannelFormat(CHANNELMASK inmask, int inchannels, SPEAKERMODE inspeakermode, out CHANNELMASK outmask, out int outchannels, out SPEAKERMODE outspeakermode)
		{
			return DSP.FMOD5_DSP_GetOutputChannelFormat(this.handle, inmask, inchannels, inspeakermode, out outmask, out outchannels, out outspeakermode);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000478D File Offset: 0x0000298D
		public RESULT reset()
		{
			return DSP.FMOD5_DSP_Reset(this.handle);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000479A File Offset: 0x0000299A
		public RESULT setCallback(DSP_CALLBACK callback)
		{
			return DSP.FMOD5_DSP_SetCallback(this.handle, callback);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000047A8 File Offset: 0x000029A8
		public RESULT setParameterFloat(int index, float value)
		{
			return DSP.FMOD5_DSP_SetParameterFloat(this.handle, index, value);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000047B7 File Offset: 0x000029B7
		public RESULT setParameterInt(int index, int value)
		{
			return DSP.FMOD5_DSP_SetParameterInt(this.handle, index, value);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000047C6 File Offset: 0x000029C6
		public RESULT setParameterBool(int index, bool value)
		{
			return DSP.FMOD5_DSP_SetParameterBool(this.handle, index, value);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000047D5 File Offset: 0x000029D5
		public RESULT setParameterData(int index, byte[] data)
		{
			return DSP.FMOD5_DSP_SetParameterData(this.handle, index, data, (uint)((data == null) ? 0 : data.Length));
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000047ED File Offset: 0x000029ED
		public RESULT getParameterFloat(int index, out float value)
		{
			return DSP.FMOD5_DSP_GetParameterFloat(this.handle, index, out value, IntPtr.Zero, 0);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00004802 File Offset: 0x00002A02
		public RESULT getParameterInt(int index, out int value)
		{
			return DSP.FMOD5_DSP_GetParameterInt(this.handle, index, out value, IntPtr.Zero, 0);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00004817 File Offset: 0x00002A17
		public RESULT getParameterBool(int index, out bool value)
		{
			return DSP.FMOD5_DSP_GetParameterBool(this.handle, index, out value, IntPtr.Zero, 0);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000482C File Offset: 0x00002A2C
		public RESULT getParameterData(int index, out IntPtr data, out uint length)
		{
			return DSP.FMOD5_DSP_GetParameterData(this.handle, index, out data, out length, IntPtr.Zero, 0);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00004842 File Offset: 0x00002A42
		public RESULT getNumParameters(out int numparams)
		{
			return DSP.FMOD5_DSP_GetNumParameters(this.handle, out numparams);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00004850 File Offset: 0x00002A50
		public RESULT getParameterInfo(int index, out DSP_PARAMETER_DESC desc)
		{
			IntPtr ptr;
			RESULT result = DSP.FMOD5_DSP_GetParameterInfo(this.handle, index, out ptr);
			desc = Marshal.PtrToStructure<DSP_PARAMETER_DESC>(ptr);
			return result;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00004877 File Offset: 0x00002A77
		public RESULT getDataParameterIndex(int datatype, out int index)
		{
			return DSP.FMOD5_DSP_GetDataParameterIndex(this.handle, datatype, out index);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00004886 File Offset: 0x00002A86
		public RESULT showConfigDialog(IntPtr hwnd, bool show)
		{
			return DSP.FMOD5_DSP_ShowConfigDialog(this.handle, hwnd, show);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00004898 File Offset: 0x00002A98
		public RESULT getInfo(out string name, out uint version, out int channels, out int configwidth, out int configheight)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(32);
			RESULT result = DSP.FMOD5_DSP_GetInfo(this.handle, intPtr, out version, out channels, out configwidth, out configheight);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				name = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000048F4 File Offset: 0x00002AF4
		public RESULT getInfo(out uint version, out int channels, out int configwidth, out int configheight)
		{
			return DSP.FMOD5_DSP_GetInfo(this.handle, IntPtr.Zero, out version, out channels, out configwidth, out configheight);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000490B File Offset: 0x00002B0B
		public RESULT getType(out DSP_TYPE type)
		{
			return DSP.FMOD5_DSP_GetType(this.handle, out type);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00004919 File Offset: 0x00002B19
		public RESULT getIdle(out bool idle)
		{
			return DSP.FMOD5_DSP_GetIdle(this.handle, out idle);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00004927 File Offset: 0x00002B27
		public RESULT setUserData(IntPtr userdata)
		{
			return DSP.FMOD5_DSP_SetUserData(this.handle, userdata);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00004935 File Offset: 0x00002B35
		public RESULT getUserData(out IntPtr userdata)
		{
			return DSP.FMOD5_DSP_GetUserData(this.handle, out userdata);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00004943 File Offset: 0x00002B43
		public RESULT setMeteringEnabled(bool inputEnabled, bool outputEnabled)
		{
			return DSP.FMOD5_DSP_SetMeteringEnabled(this.handle, inputEnabled, outputEnabled);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00004952 File Offset: 0x00002B52
		public RESULT getMeteringEnabled(out bool inputEnabled, out bool outputEnabled)
		{
			return DSP.FMOD5_DSP_GetMeteringEnabled(this.handle, out inputEnabled, out outputEnabled);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00004961 File Offset: 0x00002B61
		public RESULT getMeteringInfo(IntPtr zero, out DSP_METERING_INFO outputInfo)
		{
			return DSP.FMOD5_DSP_GetMeteringInfo(this.handle, zero, out outputInfo);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00004970 File Offset: 0x00002B70
		public RESULT getMeteringInfo(out DSP_METERING_INFO inputInfo, IntPtr zero)
		{
			return DSP.FMOD5_DSP_GetMeteringInfo(this.handle, out inputInfo, zero);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000497F File Offset: 0x00002B7F
		public RESULT getMeteringInfo(out DSP_METERING_INFO inputInfo, out DSP_METERING_INFO outputInfo)
		{
			return DSP.FMOD5_DSP_GetMeteringInfo(this.handle, out inputInfo, out outputInfo);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000498E File Offset: 0x00002B8E
		public RESULT getCPUUsage(out uint exclusive, out uint inclusive)
		{
			return DSP.FMOD5_DSP_GetCPUUsage(this.handle, out exclusive, out inclusive);
		}

		// Token: 0x0600036C RID: 876
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_Release(IntPtr dsp);

		// Token: 0x0600036D RID: 877
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetSystemObject(IntPtr dsp, out IntPtr system);

		// Token: 0x0600036E RID: 878
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_AddInput(IntPtr dsp, IntPtr input, IntPtr zero, DSPCONNECTION_TYPE type);

		// Token: 0x0600036F RID: 879
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_AddInput(IntPtr dsp, IntPtr input, out IntPtr connection, DSPCONNECTION_TYPE type);

		// Token: 0x06000370 RID: 880
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_DisconnectFrom(IntPtr dsp, IntPtr target, IntPtr connection);

		// Token: 0x06000371 RID: 881
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_DisconnectAll(IntPtr dsp, bool inputs, bool outputs);

		// Token: 0x06000372 RID: 882
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetNumInputs(IntPtr dsp, out int numinputs);

		// Token: 0x06000373 RID: 883
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetNumOutputs(IntPtr dsp, out int numoutputs);

		// Token: 0x06000374 RID: 884
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetInput(IntPtr dsp, int index, out IntPtr input, out IntPtr inputconnection);

		// Token: 0x06000375 RID: 885
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetOutput(IntPtr dsp, int index, out IntPtr output, out IntPtr outputconnection);

		// Token: 0x06000376 RID: 886
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetActive(IntPtr dsp, bool active);

		// Token: 0x06000377 RID: 887
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetActive(IntPtr dsp, out bool active);

		// Token: 0x06000378 RID: 888
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetBypass(IntPtr dsp, bool bypass);

		// Token: 0x06000379 RID: 889
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetBypass(IntPtr dsp, out bool bypass);

		// Token: 0x0600037A RID: 890
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetWetDryMix(IntPtr dsp, float prewet, float postwet, float dry);

		// Token: 0x0600037B RID: 891
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetWetDryMix(IntPtr dsp, out float prewet, out float postwet, out float dry);

		// Token: 0x0600037C RID: 892
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetChannelFormat(IntPtr dsp, CHANNELMASK channelmask, int numchannels, SPEAKERMODE source_speakermode);

		// Token: 0x0600037D RID: 893
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetChannelFormat(IntPtr dsp, out CHANNELMASK channelmask, out int numchannels, out SPEAKERMODE source_speakermode);

		// Token: 0x0600037E RID: 894
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetOutputChannelFormat(IntPtr dsp, CHANNELMASK inmask, int inchannels, SPEAKERMODE inspeakermode, out CHANNELMASK outmask, out int outchannels, out SPEAKERMODE outspeakermode);

		// Token: 0x0600037F RID: 895
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_Reset(IntPtr dsp);

		// Token: 0x06000380 RID: 896
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetCallback(IntPtr dsp, DSP_CALLBACK callback);

		// Token: 0x06000381 RID: 897
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetParameterFloat(IntPtr dsp, int index, float value);

		// Token: 0x06000382 RID: 898
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetParameterInt(IntPtr dsp, int index, int value);

		// Token: 0x06000383 RID: 899
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetParameterBool(IntPtr dsp, int index, bool value);

		// Token: 0x06000384 RID: 900
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetParameterData(IntPtr dsp, int index, byte[] data, uint length);

		// Token: 0x06000385 RID: 901
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetParameterFloat(IntPtr dsp, int index, out float value, IntPtr valuestr, int valuestrlen);

		// Token: 0x06000386 RID: 902
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetParameterInt(IntPtr dsp, int index, out int value, IntPtr valuestr, int valuestrlen);

		// Token: 0x06000387 RID: 903
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetParameterBool(IntPtr dsp, int index, out bool value, IntPtr valuestr, int valuestrlen);

		// Token: 0x06000388 RID: 904
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetParameterData(IntPtr dsp, int index, out IntPtr data, out uint length, IntPtr valuestr, int valuestrlen);

		// Token: 0x06000389 RID: 905
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetNumParameters(IntPtr dsp, out int numparams);

		// Token: 0x0600038A RID: 906
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetParameterInfo(IntPtr dsp, int index, out IntPtr desc);

		// Token: 0x0600038B RID: 907
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetDataParameterIndex(IntPtr dsp, int datatype, out int index);

		// Token: 0x0600038C RID: 908
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_ShowConfigDialog(IntPtr dsp, IntPtr hwnd, bool show);

		// Token: 0x0600038D RID: 909
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetInfo(IntPtr dsp, IntPtr name, out uint version, out int channels, out int configwidth, out int configheight);

		// Token: 0x0600038E RID: 910
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetType(IntPtr dsp, out DSP_TYPE type);

		// Token: 0x0600038F RID: 911
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetIdle(IntPtr dsp, out bool idle);

		// Token: 0x06000390 RID: 912
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_SetUserData(IntPtr dsp, IntPtr userdata);

		// Token: 0x06000391 RID: 913
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_DSP_GetUserData(IntPtr dsp, out IntPtr userdata);

		// Token: 0x06000392 RID: 914
		[DllImport("fmodstudio")]
		public static extern RESULT FMOD5_DSP_SetMeteringEnabled(IntPtr dsp, bool inputEnabled, bool outputEnabled);

		// Token: 0x06000393 RID: 915
		[DllImport("fmodstudio")]
		public static extern RESULT FMOD5_DSP_GetMeteringEnabled(IntPtr dsp, out bool inputEnabled, out bool outputEnabled);

		// Token: 0x06000394 RID: 916
		[DllImport("fmodstudio")]
		public static extern RESULT FMOD5_DSP_GetMeteringInfo(IntPtr dsp, IntPtr zero, out DSP_METERING_INFO outputInfo);

		// Token: 0x06000395 RID: 917
		[DllImport("fmodstudio")]
		public static extern RESULT FMOD5_DSP_GetMeteringInfo(IntPtr dsp, out DSP_METERING_INFO inputInfo, IntPtr zero);

		// Token: 0x06000396 RID: 918
		[DllImport("fmodstudio")]
		public static extern RESULT FMOD5_DSP_GetMeteringInfo(IntPtr dsp, out DSP_METERING_INFO inputInfo, out DSP_METERING_INFO outputInfo);

		// Token: 0x06000397 RID: 919
		[DllImport("fmodstudio")]
		public static extern RESULT FMOD5_DSP_GetCPUUsage(IntPtr dsp, out uint exclusive, out uint inclusive);

		// Token: 0x06000398 RID: 920 RVA: 0x0000499D File Offset: 0x00002B9D
		public DSP(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000049A6 File Offset: 0x00002BA6
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000049B8 File Offset: 0x00002BB8
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x04000263 RID: 611
		public IntPtr handle;
	}
}
