using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200004A RID: 74
	public struct System
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00002D1B File Offset: 0x00000F1B
		public RESULT release()
		{
			return System.FMOD5_System_Release(this.handle);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002D28 File Offset: 0x00000F28
		public RESULT setOutput(OUTPUTTYPE output)
		{
			return System.FMOD5_System_SetOutput(this.handle, output);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002D36 File Offset: 0x00000F36
		public RESULT getOutput(out OUTPUTTYPE output)
		{
			return System.FMOD5_System_GetOutput(this.handle, out output);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002D44 File Offset: 0x00000F44
		public RESULT getNumDrivers(out int numdrivers)
		{
			return System.FMOD5_System_GetNumDrivers(this.handle, out numdrivers);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002D54 File Offset: 0x00000F54
		public RESULT getDriverInfo(int id, out string name, int namelen, out Guid guid, out int systemrate, out SPEAKERMODE speakermode, out int speakermodechannels)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(namelen);
			RESULT result = System.FMOD5_System_GetDriverInfo(this.handle, id, intPtr, namelen, out guid, out systemrate, out speakermode, out speakermodechannels);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				name = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public RESULT getDriverInfo(int id, out Guid guid, out int systemrate, out SPEAKERMODE speakermode, out int speakermodechannels)
		{
			return System.FMOD5_System_GetDriverInfo(this.handle, id, IntPtr.Zero, 0, out guid, out systemrate, out speakermode, out speakermodechannels);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00002DCE File Offset: 0x00000FCE
		public RESULT setDriver(int driver)
		{
			return System.FMOD5_System_SetDriver(this.handle, driver);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002DDC File Offset: 0x00000FDC
		public RESULT getDriver(out int driver)
		{
			return System.FMOD5_System_GetDriver(this.handle, out driver);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002DEA File Offset: 0x00000FEA
		public RESULT setSoftwareChannels(int numsoftwarechannels)
		{
			return System.FMOD5_System_SetSoftwareChannels(this.handle, numsoftwarechannels);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public RESULT getSoftwareChannels(out int numsoftwarechannels)
		{
			return System.FMOD5_System_GetSoftwareChannels(this.handle, out numsoftwarechannels);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002E06 File Offset: 0x00001006
		public RESULT setSoftwareFormat(int samplerate, SPEAKERMODE speakermode, int numrawspeakers)
		{
			return System.FMOD5_System_SetSoftwareFormat(this.handle, samplerate, speakermode, numrawspeakers);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002E16 File Offset: 0x00001016
		public RESULT getSoftwareFormat(out int samplerate, out SPEAKERMODE speakermode, out int numrawspeakers)
		{
			return System.FMOD5_System_GetSoftwareFormat(this.handle, out samplerate, out speakermode, out numrawspeakers);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002E26 File Offset: 0x00001026
		public RESULT setDSPBufferSize(uint bufferlength, int numbuffers)
		{
			return System.FMOD5_System_SetDSPBufferSize(this.handle, bufferlength, numbuffers);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002E35 File Offset: 0x00001035
		public RESULT getDSPBufferSize(out uint bufferlength, out int numbuffers)
		{
			return System.FMOD5_System_GetDSPBufferSize(this.handle, out bufferlength, out numbuffers);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002E44 File Offset: 0x00001044
		public RESULT setFileSystem(FILE_OPEN_CALLBACK useropen, FILE_CLOSE_CALLBACK userclose, FILE_READ_CALLBACK userread, FILE_SEEK_CALLBACK userseek, FILE_ASYNCREAD_CALLBACK userasyncread, FILE_ASYNCCANCEL_CALLBACK userasynccancel, int blockalign)
		{
			return System.FMOD5_System_SetFileSystem(this.handle, useropen, userclose, userread, userseek, userasyncread, userasynccancel, blockalign);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002E5C File Offset: 0x0000105C
		public RESULT attachFileSystem(FILE_OPEN_CALLBACK useropen, FILE_CLOSE_CALLBACK userclose, FILE_READ_CALLBACK userread, FILE_SEEK_CALLBACK userseek)
		{
			return System.FMOD5_System_AttachFileSystem(this.handle, useropen, userclose, userread, userseek);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002E6E File Offset: 0x0000106E
		public RESULT setAdvancedSettings(ref ADVANCEDSETTINGS settings)
		{
			settings.cbSize = Marshal.SizeOf<ADVANCEDSETTINGS>();
			return System.FMOD5_System_SetAdvancedSettings(this.handle, ref settings);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002E87 File Offset: 0x00001087
		public RESULT getAdvancedSettings(ref ADVANCEDSETTINGS settings)
		{
			settings.cbSize = Marshal.SizeOf<ADVANCEDSETTINGS>();
			return System.FMOD5_System_GetAdvancedSettings(this.handle, ref settings);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002EA0 File Offset: 0x000010A0
		public RESULT setCallback(SYSTEM_CALLBACK callback, SYSTEM_CALLBACK_TYPE callbackmask = SYSTEM_CALLBACK_TYPE.ALL)
		{
			return System.FMOD5_System_SetCallback(this.handle, callback, callbackmask);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002EB0 File Offset: 0x000010B0
		public RESULT setPluginPath(string path)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD5_System_SetPluginPath(this.handle, freeHelper.byteFromStringUTF8(path));
			}
			return result;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002EF4 File Offset: 0x000010F4
		public RESULT loadPlugin(string filename, out uint handle, uint priority = 0U)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD5_System_LoadPlugin(this.handle, freeHelper.byteFromStringUTF8(filename), out handle, priority);
			}
			return result;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002F3C File Offset: 0x0000113C
		public RESULT unloadPlugin(uint handle)
		{
			return System.FMOD5_System_UnloadPlugin(this.handle, handle);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002F4A File Offset: 0x0000114A
		public RESULT getNumNestedPlugins(uint handle, out int count)
		{
			return System.FMOD5_System_GetNumNestedPlugins(this.handle, handle, out count);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00002F59 File Offset: 0x00001159
		public RESULT getNestedPlugin(uint handle, int index, out uint nestedhandle)
		{
			return System.FMOD5_System_GetNestedPlugin(this.handle, handle, index, out nestedhandle);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00002F69 File Offset: 0x00001169
		public RESULT getNumPlugins(PLUGINTYPE plugintype, out int numplugins)
		{
			return System.FMOD5_System_GetNumPlugins(this.handle, plugintype, out numplugins);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00002F78 File Offset: 0x00001178
		public RESULT getPluginHandle(PLUGINTYPE plugintype, int index, out uint handle)
		{
			return System.FMOD5_System_GetPluginHandle(this.handle, plugintype, index, out handle);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00002F88 File Offset: 0x00001188
		public RESULT getPluginInfo(uint handle, out PLUGINTYPE plugintype, out string name, int namelen, out uint version)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(namelen);
			RESULT result = System.FMOD5_System_GetPluginInfo(this.handle, handle, out plugintype, intPtr, namelen, out version);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				name = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002FE4 File Offset: 0x000011E4
		public RESULT getPluginInfo(uint handle, out PLUGINTYPE plugintype, out uint version)
		{
			return System.FMOD5_System_GetPluginInfo(this.handle, handle, out plugintype, IntPtr.Zero, 0, out version);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00002FFA File Offset: 0x000011FA
		public RESULT setOutputByPlugin(uint handle)
		{
			return System.FMOD5_System_SetOutputByPlugin(this.handle, handle);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003008 File Offset: 0x00001208
		public RESULT getOutputByPlugin(out uint handle)
		{
			return System.FMOD5_System_GetOutputByPlugin(this.handle, out handle);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003016 File Offset: 0x00001216
		public RESULT createDSPByPlugin(uint handle, out DSP dsp)
		{
			return System.FMOD5_System_CreateDSPByPlugin(this.handle, handle, out dsp.handle);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000302A File Offset: 0x0000122A
		public RESULT getDSPInfoByPlugin(uint handle, out IntPtr description)
		{
			return System.FMOD5_System_GetDSPInfoByPlugin(this.handle, handle, out description);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003039 File Offset: 0x00001239
		public RESULT registerDSP(ref DSP_DESCRIPTION description, out uint handle)
		{
			return System.FMOD5_System_RegisterDSP(this.handle, ref description, out handle);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003048 File Offset: 0x00001248
		public RESULT init(int maxchannels, INITFLAGS flags, IntPtr extradriverdata)
		{
			return System.FMOD5_System_Init(this.handle, maxchannels, flags, extradriverdata);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003058 File Offset: 0x00001258
		public RESULT close()
		{
			return System.FMOD5_System_Close(this.handle);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003065 File Offset: 0x00001265
		public RESULT update()
		{
			return System.FMOD5_System_Update(this.handle);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003072 File Offset: 0x00001272
		public RESULT setSpeakerPosition(SPEAKER speaker, float x, float y, bool active)
		{
			return System.FMOD5_System_SetSpeakerPosition(this.handle, speaker, x, y, active);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003084 File Offset: 0x00001284
		public RESULT getSpeakerPosition(SPEAKER speaker, out float x, out float y, out bool active)
		{
			return System.FMOD5_System_GetSpeakerPosition(this.handle, speaker, out x, out y, out active);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003096 File Offset: 0x00001296
		public RESULT setStreamBufferSize(uint filebuffersize, TIMEUNIT filebuffersizetype)
		{
			return System.FMOD5_System_SetStreamBufferSize(this.handle, filebuffersize, filebuffersizetype);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000030A5 File Offset: 0x000012A5
		public RESULT getStreamBufferSize(out uint filebuffersize, out TIMEUNIT filebuffersizetype)
		{
			return System.FMOD5_System_GetStreamBufferSize(this.handle, out filebuffersize, out filebuffersizetype);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000030B4 File Offset: 0x000012B4
		public RESULT set3DSettings(float dopplerscale, float distancefactor, float rolloffscale)
		{
			return System.FMOD5_System_Set3DSettings(this.handle, dopplerscale, distancefactor, rolloffscale);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000030C4 File Offset: 0x000012C4
		public RESULT get3DSettings(out float dopplerscale, out float distancefactor, out float rolloffscale)
		{
			return System.FMOD5_System_Get3DSettings(this.handle, out dopplerscale, out distancefactor, out rolloffscale);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000030D4 File Offset: 0x000012D4
		public RESULT set3DNumListeners(int numlisteners)
		{
			return System.FMOD5_System_Set3DNumListeners(this.handle, numlisteners);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000030E2 File Offset: 0x000012E2
		public RESULT get3DNumListeners(out int numlisteners)
		{
			return System.FMOD5_System_Get3DNumListeners(this.handle, out numlisteners);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000030F0 File Offset: 0x000012F0
		public RESULT set3DListenerAttributes(int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up)
		{
			return System.FMOD5_System_Set3DListenerAttributes(this.handle, listener, ref pos, ref vel, ref forward, ref up);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003104 File Offset: 0x00001304
		public RESULT get3DListenerAttributes(int listener, out VECTOR pos, out VECTOR vel, out VECTOR forward, out VECTOR up)
		{
			return System.FMOD5_System_Get3DListenerAttributes(this.handle, listener, out pos, out vel, out forward, out up);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003118 File Offset: 0x00001318
		public RESULT set3DRolloffCallback(CB_3D_ROLLOFF_CALLBACK callback)
		{
			return System.FMOD5_System_Set3DRolloffCallback(this.handle, callback);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003126 File Offset: 0x00001326
		public RESULT mixerSuspend()
		{
			return System.FMOD5_System_MixerSuspend(this.handle);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003133 File Offset: 0x00001333
		public RESULT mixerResume()
		{
			return System.FMOD5_System_MixerResume(this.handle);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003140 File Offset: 0x00001340
		public RESULT getDefaultMixMatrix(SPEAKERMODE sourcespeakermode, SPEAKERMODE targetspeakermode, float[] matrix, int matrixhop)
		{
			return System.FMOD5_System_GetDefaultMixMatrix(this.handle, sourcespeakermode, targetspeakermode, matrix, matrixhop);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003152 File Offset: 0x00001352
		public RESULT getSpeakerModeChannels(SPEAKERMODE mode, out int channels)
		{
			return System.FMOD5_System_GetSpeakerModeChannels(this.handle, mode, out channels);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003164 File Offset: 0x00001364
		public RESULT getVersion(out uint version)
		{
			uint num;
			return this.getVersion(out version, out num);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000317A File Offset: 0x0000137A
		public RESULT getVersion(out uint version, out uint buildnumber)
		{
			return System.FMOD5_System_GetVersion(this.handle, out version, out buildnumber);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003189 File Offset: 0x00001389
		public RESULT getOutputHandle(out IntPtr handle)
		{
			return System.FMOD5_System_GetOutputHandle(this.handle, out handle);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003197 File Offset: 0x00001397
		public RESULT getChannelsPlaying(out int channels)
		{
			return System.FMOD5_System_GetChannelsPlaying(this.handle, out channels, IntPtr.Zero);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000031AA File Offset: 0x000013AA
		public RESULT getChannelsPlaying(out int channels, out int realchannels)
		{
			return System.FMOD5_System_GetChannelsPlaying(this.handle, out channels, out realchannels);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000031B9 File Offset: 0x000013B9
		public RESULT getCPUUsage(out CPU_USAGE usage)
		{
			return System.FMOD5_System_GetCPUUsage(this.handle, out usage);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000031C7 File Offset: 0x000013C7
		public RESULT getFileUsage(out long sampleBytesRead, out long streamBytesRead, out long otherBytesRead)
		{
			return System.FMOD5_System_GetFileUsage(this.handle, out sampleBytesRead, out streamBytesRead, out otherBytesRead);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000031D8 File Offset: 0x000013D8
		public RESULT createSound(string name, MODE mode, ref CREATESOUNDEXINFO exinfo, out Sound sound)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD5_System_CreateSound(this.handle, freeHelper.byteFromStringUTF8(name), mode, ref exinfo, out sound.handle);
			}
			return result;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003224 File Offset: 0x00001424
		public RESULT createSound(byte[] data, MODE mode, ref CREATESOUNDEXINFO exinfo, out Sound sound)
		{
			return System.FMOD5_System_CreateSound(this.handle, data, mode, ref exinfo, out sound.handle);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000323B File Offset: 0x0000143B
		public RESULT createSound(IntPtr name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, out Sound sound)
		{
			return System.FMOD5_System_CreateSound(this.handle, name_or_data, mode, ref exinfo, out sound.handle);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003254 File Offset: 0x00001454
		public RESULT createSound(string name, MODE mode, out Sound sound)
		{
			CREATESOUNDEXINFO createsoundexinfo = default(CREATESOUNDEXINFO);
			createsoundexinfo.cbsize = Marshal.SizeOf<CREATESOUNDEXINFO>();
			return this.createSound(name, mode, ref createsoundexinfo, out sound);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003280 File Offset: 0x00001480
		public RESULT createStream(string name, MODE mode, ref CREATESOUNDEXINFO exinfo, out Sound sound)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD5_System_CreateStream(this.handle, freeHelper.byteFromStringUTF8(name), mode, ref exinfo, out sound.handle);
			}
			return result;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000032CC File Offset: 0x000014CC
		public RESULT createStream(byte[] data, MODE mode, ref CREATESOUNDEXINFO exinfo, out Sound sound)
		{
			return System.FMOD5_System_CreateStream(this.handle, data, mode, ref exinfo, out sound.handle);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000032E3 File Offset: 0x000014E3
		public RESULT createStream(IntPtr name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, out Sound sound)
		{
			return System.FMOD5_System_CreateStream(this.handle, name_or_data, mode, ref exinfo, out sound.handle);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000032FC File Offset: 0x000014FC
		public RESULT createStream(string name, MODE mode, out Sound sound)
		{
			CREATESOUNDEXINFO createsoundexinfo = default(CREATESOUNDEXINFO);
			createsoundexinfo.cbsize = Marshal.SizeOf<CREATESOUNDEXINFO>();
			return this.createStream(name, mode, ref createsoundexinfo, out sound);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003328 File Offset: 0x00001528
		public RESULT createDSP(ref DSP_DESCRIPTION description, out DSP dsp)
		{
			return System.FMOD5_System_CreateDSP(this.handle, ref description, out dsp.handle);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000333C File Offset: 0x0000153C
		public RESULT createDSPByType(DSP_TYPE type, out DSP dsp)
		{
			return System.FMOD5_System_CreateDSPByType(this.handle, type, out dsp.handle);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003350 File Offset: 0x00001550
		public RESULT createChannelGroup(string name, out ChannelGroup channelgroup)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD5_System_CreateChannelGroup(this.handle, freeHelper.byteFromStringUTF8(name), out channelgroup.handle);
			}
			return result;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000339C File Offset: 0x0000159C
		public RESULT createSoundGroup(string name, out SoundGroup soundgroup)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD5_System_CreateSoundGroup(this.handle, freeHelper.byteFromStringUTF8(name), out soundgroup.handle);
			}
			return result;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000033E8 File Offset: 0x000015E8
		public RESULT createReverb3D(out Reverb3D reverb)
		{
			return System.FMOD5_System_CreateReverb3D(this.handle, out reverb.handle);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000033FB File Offset: 0x000015FB
		public RESULT playSound(Sound sound, ChannelGroup channelgroup, bool paused, out Channel channel)
		{
			return System.FMOD5_System_PlaySound(this.handle, sound.handle, channelgroup.handle, paused, out channel.handle);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000341C File Offset: 0x0000161C
		public RESULT playDSP(DSP dsp, ChannelGroup channelgroup, bool paused, out Channel channel)
		{
			return System.FMOD5_System_PlayDSP(this.handle, dsp.handle, channelgroup.handle, paused, out channel.handle);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000343D File Offset: 0x0000163D
		public RESULT getChannel(int channelid, out Channel channel)
		{
			return System.FMOD5_System_GetChannel(this.handle, channelid, out channel.handle);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003451 File Offset: 0x00001651
		public RESULT getDSPInfoByType(DSP_TYPE type, out IntPtr description)
		{
			return System.FMOD5_System_GetDSPInfoByType(this.handle, type, out description);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003460 File Offset: 0x00001660
		public RESULT getMasterChannelGroup(out ChannelGroup channelgroup)
		{
			return System.FMOD5_System_GetMasterChannelGroup(this.handle, out channelgroup.handle);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003473 File Offset: 0x00001673
		public RESULT getMasterSoundGroup(out SoundGroup soundgroup)
		{
			return System.FMOD5_System_GetMasterSoundGroup(this.handle, out soundgroup.handle);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003486 File Offset: 0x00001686
		public RESULT attachChannelGroupToPort(PORT_TYPE portType, ulong portIndex, ChannelGroup channelgroup, bool passThru = false)
		{
			return System.FMOD5_System_AttachChannelGroupToPort(this.handle, portType, portIndex, channelgroup.handle, passThru);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000349D File Offset: 0x0000169D
		public RESULT detachChannelGroupFromPort(ChannelGroup channelgroup)
		{
			return System.FMOD5_System_DetachChannelGroupFromPort(this.handle, channelgroup.handle);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000034B0 File Offset: 0x000016B0
		public RESULT setReverbProperties(int instance, ref REVERB_PROPERTIES prop)
		{
			return System.FMOD5_System_SetReverbProperties(this.handle, instance, ref prop);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000034BF File Offset: 0x000016BF
		public RESULT getReverbProperties(int instance, out REVERB_PROPERTIES prop)
		{
			return System.FMOD5_System_GetReverbProperties(this.handle, instance, out prop);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000034CE File Offset: 0x000016CE
		public RESULT lockDSP()
		{
			return System.FMOD5_System_LockDSP(this.handle);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000034DB File Offset: 0x000016DB
		public RESULT unlockDSP()
		{
			return System.FMOD5_System_UnlockDSP(this.handle);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000034E8 File Offset: 0x000016E8
		public RESULT getRecordNumDrivers(out int numdrivers, out int numconnected)
		{
			return System.FMOD5_System_GetRecordNumDrivers(this.handle, out numdrivers, out numconnected);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000034F8 File Offset: 0x000016F8
		public RESULT getRecordDriverInfo(int id, out string name, int namelen, out Guid guid, out int systemrate, out SPEAKERMODE speakermode, out int speakermodechannels, out DRIVER_STATE state)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(namelen);
			RESULT result = System.FMOD5_System_GetRecordDriverInfo(this.handle, id, intPtr, namelen, out guid, out systemrate, out speakermode, out speakermodechannels, out state);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				name = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003558 File Offset: 0x00001758
		public RESULT getRecordDriverInfo(int id, out Guid guid, out int systemrate, out SPEAKERMODE speakermode, out int speakermodechannels, out DRIVER_STATE state)
		{
			return System.FMOD5_System_GetRecordDriverInfo(this.handle, id, IntPtr.Zero, 0, out guid, out systemrate, out speakermode, out speakermodechannels, out state);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000357F File Offset: 0x0000177F
		public RESULT getRecordPosition(int id, out uint position)
		{
			return System.FMOD5_System_GetRecordPosition(this.handle, id, out position);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000358E File Offset: 0x0000178E
		public RESULT recordStart(int id, Sound sound, bool loop)
		{
			return System.FMOD5_System_RecordStart(this.handle, id, sound.handle, loop);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000035A3 File Offset: 0x000017A3
		public RESULT recordStop(int id)
		{
			return System.FMOD5_System_RecordStop(this.handle, id);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000035B1 File Offset: 0x000017B1
		public RESULT isRecording(int id, out bool recording)
		{
			return System.FMOD5_System_IsRecording(this.handle, id, out recording);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000035C0 File Offset: 0x000017C0
		public RESULT createGeometry(int maxpolygons, int maxvertices, out Geometry geometry)
		{
			return System.FMOD5_System_CreateGeometry(this.handle, maxpolygons, maxvertices, out geometry.handle);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000035D5 File Offset: 0x000017D5
		public RESULT setGeometrySettings(float maxworldsize)
		{
			return System.FMOD5_System_SetGeometrySettings(this.handle, maxworldsize);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000035E3 File Offset: 0x000017E3
		public RESULT getGeometrySettings(out float maxworldsize)
		{
			return System.FMOD5_System_GetGeometrySettings(this.handle, out maxworldsize);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000035F1 File Offset: 0x000017F1
		public RESULT loadGeometry(IntPtr data, int datasize, out Geometry geometry)
		{
			return System.FMOD5_System_LoadGeometry(this.handle, data, datasize, out geometry.handle);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003606 File Offset: 0x00001806
		public RESULT getGeometryOcclusion(ref VECTOR listener, ref VECTOR source, out float direct, out float reverb)
		{
			return System.FMOD5_System_GetGeometryOcclusion(this.handle, ref listener, ref source, out direct, out reverb);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003618 File Offset: 0x00001818
		public RESULT setNetworkProxy(string proxy)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD5_System_SetNetworkProxy(this.handle, freeHelper.byteFromStringUTF8(proxy));
			}
			return result;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000365C File Offset: 0x0000185C
		public RESULT getNetworkProxy(out string proxy, int proxylen)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(proxylen);
			RESULT result = System.FMOD5_System_GetNetworkProxy(this.handle, intPtr, proxylen);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				proxy = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000036B0 File Offset: 0x000018B0
		public RESULT setNetworkTimeout(int timeout)
		{
			return System.FMOD5_System_SetNetworkTimeout(this.handle, timeout);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000036BE File Offset: 0x000018BE
		public RESULT getNetworkTimeout(out int timeout)
		{
			return System.FMOD5_System_GetNetworkTimeout(this.handle, out timeout);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000036CC File Offset: 0x000018CC
		public RESULT setUserData(IntPtr userdata)
		{
			return System.FMOD5_System_SetUserData(this.handle, userdata);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000036DA File Offset: 0x000018DA
		public RESULT getUserData(out IntPtr userdata)
		{
			return System.FMOD5_System_GetUserData(this.handle, out userdata);
		}

		// Token: 0x060000F2 RID: 242
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Release(IntPtr system);

		// Token: 0x060000F3 RID: 243
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetOutput(IntPtr system, OUTPUTTYPE output);

		// Token: 0x060000F4 RID: 244
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetOutput(IntPtr system, out OUTPUTTYPE output);

		// Token: 0x060000F5 RID: 245
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetNumDrivers(IntPtr system, out int numdrivers);

		// Token: 0x060000F6 RID: 246
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetDriverInfo(IntPtr system, int id, IntPtr name, int namelen, out Guid guid, out int systemrate, out SPEAKERMODE speakermode, out int speakermodechannels);

		// Token: 0x060000F7 RID: 247
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetDriver(IntPtr system, int driver);

		// Token: 0x060000F8 RID: 248
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetDriver(IntPtr system, out int driver);

		// Token: 0x060000F9 RID: 249
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetSoftwareChannels(IntPtr system, int numsoftwarechannels);

		// Token: 0x060000FA RID: 250
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetSoftwareChannels(IntPtr system, out int numsoftwarechannels);

		// Token: 0x060000FB RID: 251
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetSoftwareFormat(IntPtr system, int samplerate, SPEAKERMODE speakermode, int numrawspeakers);

		// Token: 0x060000FC RID: 252
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetSoftwareFormat(IntPtr system, out int samplerate, out SPEAKERMODE speakermode, out int numrawspeakers);

		// Token: 0x060000FD RID: 253
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetDSPBufferSize(IntPtr system, uint bufferlength, int numbuffers);

		// Token: 0x060000FE RID: 254
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetDSPBufferSize(IntPtr system, out uint bufferlength, out int numbuffers);

		// Token: 0x060000FF RID: 255
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetFileSystem(IntPtr system, FILE_OPEN_CALLBACK useropen, FILE_CLOSE_CALLBACK userclose, FILE_READ_CALLBACK userread, FILE_SEEK_CALLBACK userseek, FILE_ASYNCREAD_CALLBACK userasyncread, FILE_ASYNCCANCEL_CALLBACK userasynccancel, int blockalign);

		// Token: 0x06000100 RID: 256
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_AttachFileSystem(IntPtr system, FILE_OPEN_CALLBACK useropen, FILE_CLOSE_CALLBACK userclose, FILE_READ_CALLBACK userread, FILE_SEEK_CALLBACK userseek);

		// Token: 0x06000101 RID: 257
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetAdvancedSettings(IntPtr system, ref ADVANCEDSETTINGS settings);

		// Token: 0x06000102 RID: 258
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetAdvancedSettings(IntPtr system, ref ADVANCEDSETTINGS settings);

		// Token: 0x06000103 RID: 259
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetCallback(IntPtr system, SYSTEM_CALLBACK callback, SYSTEM_CALLBACK_TYPE callbackmask);

		// Token: 0x06000104 RID: 260
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetPluginPath(IntPtr system, byte[] path);

		// Token: 0x06000105 RID: 261
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_LoadPlugin(IntPtr system, byte[] filename, out uint handle, uint priority);

		// Token: 0x06000106 RID: 262
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_UnloadPlugin(IntPtr system, uint handle);

		// Token: 0x06000107 RID: 263
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetNumNestedPlugins(IntPtr system, uint handle, out int count);

		// Token: 0x06000108 RID: 264
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetNestedPlugin(IntPtr system, uint handle, int index, out uint nestedhandle);

		// Token: 0x06000109 RID: 265
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetNumPlugins(IntPtr system, PLUGINTYPE plugintype, out int numplugins);

		// Token: 0x0600010A RID: 266
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetPluginHandle(IntPtr system, PLUGINTYPE plugintype, int index, out uint handle);

		// Token: 0x0600010B RID: 267
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetPluginInfo(IntPtr system, uint handle, out PLUGINTYPE plugintype, IntPtr name, int namelen, out uint version);

		// Token: 0x0600010C RID: 268
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetOutputByPlugin(IntPtr system, uint handle);

		// Token: 0x0600010D RID: 269
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetOutputByPlugin(IntPtr system, out uint handle);

		// Token: 0x0600010E RID: 270
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateDSPByPlugin(IntPtr system, uint handle, out IntPtr dsp);

		// Token: 0x0600010F RID: 271
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetDSPInfoByPlugin(IntPtr system, uint handle, out IntPtr description);

		// Token: 0x06000110 RID: 272
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_RegisterDSP(IntPtr system, ref DSP_DESCRIPTION description, out uint handle);

		// Token: 0x06000111 RID: 273
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Init(IntPtr system, int maxchannels, INITFLAGS flags, IntPtr extradriverdata);

		// Token: 0x06000112 RID: 274
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Close(IntPtr system);

		// Token: 0x06000113 RID: 275
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Update(IntPtr system);

		// Token: 0x06000114 RID: 276
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetSpeakerPosition(IntPtr system, SPEAKER speaker, float x, float y, bool active);

		// Token: 0x06000115 RID: 277
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetSpeakerPosition(IntPtr system, SPEAKER speaker, out float x, out float y, out bool active);

		// Token: 0x06000116 RID: 278
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetStreamBufferSize(IntPtr system, uint filebuffersize, TIMEUNIT filebuffersizetype);

		// Token: 0x06000117 RID: 279
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetStreamBufferSize(IntPtr system, out uint filebuffersize, out TIMEUNIT filebuffersizetype);

		// Token: 0x06000118 RID: 280
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Set3DSettings(IntPtr system, float dopplerscale, float distancefactor, float rolloffscale);

		// Token: 0x06000119 RID: 281
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Get3DSettings(IntPtr system, out float dopplerscale, out float distancefactor, out float rolloffscale);

		// Token: 0x0600011A RID: 282
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Set3DNumListeners(IntPtr system, int numlisteners);

		// Token: 0x0600011B RID: 283
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Get3DNumListeners(IntPtr system, out int numlisteners);

		// Token: 0x0600011C RID: 284
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Set3DListenerAttributes(IntPtr system, int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up);

		// Token: 0x0600011D RID: 285
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Get3DListenerAttributes(IntPtr system, int listener, out VECTOR pos, out VECTOR vel, out VECTOR forward, out VECTOR up);

		// Token: 0x0600011E RID: 286
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Set3DRolloffCallback(IntPtr system, CB_3D_ROLLOFF_CALLBACK callback);

		// Token: 0x0600011F RID: 287
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_MixerSuspend(IntPtr system);

		// Token: 0x06000120 RID: 288
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_MixerResume(IntPtr system);

		// Token: 0x06000121 RID: 289
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetDefaultMixMatrix(IntPtr system, SPEAKERMODE sourcespeakermode, SPEAKERMODE targetspeakermode, float[] matrix, int matrixhop);

		// Token: 0x06000122 RID: 290
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetSpeakerModeChannels(IntPtr system, SPEAKERMODE mode, out int channels);

		// Token: 0x06000123 RID: 291
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetVersion(IntPtr system, out uint version, out uint buildnumber);

		// Token: 0x06000124 RID: 292
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetOutputHandle(IntPtr system, out IntPtr handle);

		// Token: 0x06000125 RID: 293
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetChannelsPlaying(IntPtr system, out int channels, IntPtr zero);

		// Token: 0x06000126 RID: 294
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetChannelsPlaying(IntPtr system, out int channels, out int realchannels);

		// Token: 0x06000127 RID: 295
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetCPUUsage(IntPtr system, out CPU_USAGE usage);

		// Token: 0x06000128 RID: 296
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetFileUsage(IntPtr system, out long sampleBytesRead, out long streamBytesRead, out long otherBytesRead);

		// Token: 0x06000129 RID: 297
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateSound(IntPtr system, byte[] name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, out IntPtr sound);

		// Token: 0x0600012A RID: 298
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateSound(IntPtr system, IntPtr name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, out IntPtr sound);

		// Token: 0x0600012B RID: 299
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateStream(IntPtr system, byte[] name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, out IntPtr sound);

		// Token: 0x0600012C RID: 300
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateStream(IntPtr system, IntPtr name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, out IntPtr sound);

		// Token: 0x0600012D RID: 301
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateDSP(IntPtr system, ref DSP_DESCRIPTION description, out IntPtr dsp);

		// Token: 0x0600012E RID: 302
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateDSPByType(IntPtr system, DSP_TYPE type, out IntPtr dsp);

		// Token: 0x0600012F RID: 303
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateChannelGroup(IntPtr system, byte[] name, out IntPtr channelgroup);

		// Token: 0x06000130 RID: 304
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateSoundGroup(IntPtr system, byte[] name, out IntPtr soundgroup);

		// Token: 0x06000131 RID: 305
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateReverb3D(IntPtr system, out IntPtr reverb);

		// Token: 0x06000132 RID: 306
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_PlaySound(IntPtr system, IntPtr sound, IntPtr channelgroup, bool paused, out IntPtr channel);

		// Token: 0x06000133 RID: 307
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_PlayDSP(IntPtr system, IntPtr dsp, IntPtr channelgroup, bool paused, out IntPtr channel);

		// Token: 0x06000134 RID: 308
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetChannel(IntPtr system, int channelid, out IntPtr channel);

		// Token: 0x06000135 RID: 309
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetDSPInfoByType(IntPtr system, DSP_TYPE type, out IntPtr description);

		// Token: 0x06000136 RID: 310
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetMasterChannelGroup(IntPtr system, out IntPtr channelgroup);

		// Token: 0x06000137 RID: 311
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetMasterSoundGroup(IntPtr system, out IntPtr soundgroup);

		// Token: 0x06000138 RID: 312
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_AttachChannelGroupToPort(IntPtr system, PORT_TYPE portType, ulong portIndex, IntPtr channelgroup, bool passThru);

		// Token: 0x06000139 RID: 313
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_DetachChannelGroupFromPort(IntPtr system, IntPtr channelgroup);

		// Token: 0x0600013A RID: 314
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetReverbProperties(IntPtr system, int instance, ref REVERB_PROPERTIES prop);

		// Token: 0x0600013B RID: 315
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetReverbProperties(IntPtr system, int instance, out REVERB_PROPERTIES prop);

		// Token: 0x0600013C RID: 316
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_LockDSP(IntPtr system);

		// Token: 0x0600013D RID: 317
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_UnlockDSP(IntPtr system);

		// Token: 0x0600013E RID: 318
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetRecordNumDrivers(IntPtr system, out int numdrivers, out int numconnected);

		// Token: 0x0600013F RID: 319
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetRecordDriverInfo(IntPtr system, int id, IntPtr name, int namelen, out Guid guid, out int systemrate, out SPEAKERMODE speakermode, out int speakermodechannels, out DRIVER_STATE state);

		// Token: 0x06000140 RID: 320
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetRecordPosition(IntPtr system, int id, out uint position);

		// Token: 0x06000141 RID: 321
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_RecordStart(IntPtr system, int id, IntPtr sound, bool loop);

		// Token: 0x06000142 RID: 322
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_RecordStop(IntPtr system, int id);

		// Token: 0x06000143 RID: 323
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_IsRecording(IntPtr system, int id, out bool recording);

		// Token: 0x06000144 RID: 324
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_CreateGeometry(IntPtr system, int maxpolygons, int maxvertices, out IntPtr geometry);

		// Token: 0x06000145 RID: 325
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetGeometrySettings(IntPtr system, float maxworldsize);

		// Token: 0x06000146 RID: 326
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetGeometrySettings(IntPtr system, out float maxworldsize);

		// Token: 0x06000147 RID: 327
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_LoadGeometry(IntPtr system, IntPtr data, int datasize, out IntPtr geometry);

		// Token: 0x06000148 RID: 328
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetGeometryOcclusion(IntPtr system, ref VECTOR listener, ref VECTOR source, out float direct, out float reverb);

		// Token: 0x06000149 RID: 329
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetNetworkProxy(IntPtr system, byte[] proxy);

		// Token: 0x0600014A RID: 330
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetNetworkProxy(IntPtr system, IntPtr proxy, int proxylen);

		// Token: 0x0600014B RID: 331
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetNetworkTimeout(IntPtr system, int timeout);

		// Token: 0x0600014C RID: 332
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetNetworkTimeout(IntPtr system, out int timeout);

		// Token: 0x0600014D RID: 333
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_SetUserData(IntPtr system, IntPtr userdata);

		// Token: 0x0600014E RID: 334
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_GetUserData(IntPtr system, out IntPtr userdata);

		// Token: 0x0600014F RID: 335 RVA: 0x000036E8 File Offset: 0x000018E8
		public System(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000036F1 File Offset: 0x000018F1
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00003703 File Offset: 0x00001903
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x0400025E RID: 606
		public IntPtr handle;
	}
}
