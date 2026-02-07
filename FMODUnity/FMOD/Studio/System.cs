using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000EF RID: 239
	public struct System
	{
		// Token: 0x060004AE RID: 1198 RVA: 0x000053EC File Offset: 0x000035EC
		public static RESULT create(out System system)
		{
			return System.FMOD_Studio_System_Create(out system.handle, 131848U);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000053FE File Offset: 0x000035FE
		public RESULT setAdvancedSettings(ADVANCEDSETTINGS settings)
		{
			settings.cbsize = Marshal.SizeOf<ADVANCEDSETTINGS>();
			return System.FMOD_Studio_System_SetAdvancedSettings(this.handle, ref settings);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000541C File Offset: 0x0000361C
		public RESULT setAdvancedSettings(ADVANCEDSETTINGS settings, string encryptionKey)
		{
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr encryptionkey = settings.encryptionkey;
				settings.encryptionkey = freeHelper.intptrFromStringUTF8(encryptionKey);
				RESULT result = this.setAdvancedSettings(settings);
				settings.encryptionkey = encryptionkey;
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00005474 File Offset: 0x00003674
		public RESULT getAdvancedSettings(out ADVANCEDSETTINGS settings)
		{
			settings.cbsize = Marshal.SizeOf<ADVANCEDSETTINGS>();
			return System.FMOD_Studio_System_GetAdvancedSettings(this.handle, out settings);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000548D File Offset: 0x0000368D
		public RESULT initialize(int maxchannels, INITFLAGS studioflags, INITFLAGS flags, IntPtr extradriverdata)
		{
			return System.FMOD_Studio_System_Initialize(this.handle, maxchannels, studioflags, flags, extradriverdata);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000549F File Offset: 0x0000369F
		public RESULT release()
		{
			return System.FMOD_Studio_System_Release(this.handle);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000054AC File Offset: 0x000036AC
		public RESULT update()
		{
			return System.FMOD_Studio_System_Update(this.handle);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000054B9 File Offset: 0x000036B9
		public RESULT getCoreSystem(out System coresystem)
		{
			return System.FMOD_Studio_System_GetCoreSystem(this.handle, out coresystem.handle);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000054CC File Offset: 0x000036CC
		public RESULT getEvent(string path, out EventDescription _event)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_GetEvent(this.handle, freeHelper.byteFromStringUTF8(path), out _event.handle);
			}
			return result;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00005518 File Offset: 0x00003718
		public RESULT getBus(string path, out Bus bus)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_GetBus(this.handle, freeHelper.byteFromStringUTF8(path), out bus.handle);
			}
			return result;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00005564 File Offset: 0x00003764
		public RESULT getVCA(string path, out VCA vca)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_GetVCA(this.handle, freeHelper.byteFromStringUTF8(path), out vca.handle);
			}
			return result;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000055B0 File Offset: 0x000037B0
		public RESULT getBank(string path, out Bank bank)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_GetBank(this.handle, freeHelper.byteFromStringUTF8(path), out bank.handle);
			}
			return result;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000055FC File Offset: 0x000037FC
		public RESULT getEventByID(GUID id, out EventDescription _event)
		{
			return System.FMOD_Studio_System_GetEventByID(this.handle, ref id, out _event.handle);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00005611 File Offset: 0x00003811
		public RESULT getBusByID(GUID id, out Bus bus)
		{
			return System.FMOD_Studio_System_GetBusByID(this.handle, ref id, out bus.handle);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00005626 File Offset: 0x00003826
		public RESULT getVCAByID(GUID id, out VCA vca)
		{
			return System.FMOD_Studio_System_GetVCAByID(this.handle, ref id, out vca.handle);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000563B File Offset: 0x0000383B
		public RESULT getBankByID(GUID id, out Bank bank)
		{
			return System.FMOD_Studio_System_GetBankByID(this.handle, ref id, out bank.handle);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00005650 File Offset: 0x00003850
		public RESULT getSoundInfo(string key, out SOUND_INFO info)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_GetSoundInfo(this.handle, freeHelper.byteFromStringUTF8(key), out info);
			}
			return result;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00005694 File Offset: 0x00003894
		public RESULT getParameterDescriptionByName(string name, out PARAMETER_DESCRIPTION parameter)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_GetParameterDescriptionByName(this.handle, freeHelper.byteFromStringUTF8(name), out parameter);
			}
			return result;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000056D8 File Offset: 0x000038D8
		public RESULT getParameterDescriptionByID(PARAMETER_ID id, out PARAMETER_DESCRIPTION parameter)
		{
			return System.FMOD_Studio_System_GetParameterDescriptionByID(this.handle, id, out parameter);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000056E8 File Offset: 0x000038E8
		public RESULT getParameterLabelByName(string name, int labelindex, out string label)
		{
			label = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				byte[] name2 = freeHelper.byteFromStringUTF8(name);
				RESULT result = System.FMOD_Studio_System_GetParameterLabelByName(this.handle, name2, labelindex, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					result = System.FMOD_Studio_System_GetParameterLabelByName(this.handle, name2, labelindex, IntPtr.Zero, 0, out num);
					intPtr = Marshal.AllocHGlobal(num);
					result = System.FMOD_Studio_System_GetParameterLabelByName(this.handle, name2, labelindex, intPtr, num, out num);
				}
				if (result == RESULT.OK)
				{
					label = freeHelper.stringFromNative(intPtr);
				}
				Marshal.FreeHGlobal(intPtr);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000579C File Offset: 0x0000399C
		public RESULT getParameterLabelByID(PARAMETER_ID id, int labelindex, out string label)
		{
			label = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = System.FMOD_Studio_System_GetParameterLabelByID(this.handle, id, labelindex, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					result = System.FMOD_Studio_System_GetParameterLabelByID(this.handle, id, labelindex, IntPtr.Zero, 0, out num);
					intPtr = Marshal.AllocHGlobal(num);
					result = System.FMOD_Studio_System_GetParameterLabelByID(this.handle, id, labelindex, intPtr, num, out num);
				}
				if (result == RESULT.OK)
				{
					label = freeHelper.stringFromNative(intPtr);
				}
				Marshal.FreeHGlobal(intPtr);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00005844 File Offset: 0x00003A44
		public RESULT getParameterByID(PARAMETER_ID id, out float value)
		{
			float num;
			return this.getParameterByID(id, out value, out num);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000585B File Offset: 0x00003A5B
		public RESULT getParameterByID(PARAMETER_ID id, out float value, out float finalvalue)
		{
			return System.FMOD_Studio_System_GetParameterByID(this.handle, id, out value, out finalvalue);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000586B File Offset: 0x00003A6B
		public RESULT setParameterByID(PARAMETER_ID id, float value, bool ignoreseekspeed = false)
		{
			return System.FMOD_Studio_System_SetParameterByID(this.handle, id, value, ignoreseekspeed);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000587C File Offset: 0x00003A7C
		public RESULT setParameterByIDWithLabel(PARAMETER_ID id, string label, bool ignoreseekspeed = false)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_SetParameterByIDWithLabel(this.handle, id, freeHelper.byteFromStringUTF8(label), ignoreseekspeed);
			}
			return result;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000058C4 File Offset: 0x00003AC4
		public RESULT setParametersByIDs(PARAMETER_ID[] ids, float[] values, int count, bool ignoreseekspeed = false)
		{
			return System.FMOD_Studio_System_SetParametersByIDs(this.handle, ids, values, count, ignoreseekspeed);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000058D8 File Offset: 0x00003AD8
		public RESULT getParameterByName(string name, out float value)
		{
			float num;
			return this.getParameterByName(name, out value, out num);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000058F0 File Offset: 0x00003AF0
		public RESULT getParameterByName(string name, out float value, out float finalvalue)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_GetParameterByName(this.handle, freeHelper.byteFromStringUTF8(name), out value, out finalvalue);
			}
			return result;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00005938 File Offset: 0x00003B38
		public RESULT setParameterByName(string name, float value, bool ignoreseekspeed = false)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_SetParameterByName(this.handle, freeHelper.byteFromStringUTF8(name), value, ignoreseekspeed);
			}
			return result;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00005980 File Offset: 0x00003B80
		public RESULT setParameterByNameWithLabel(string name, string label, bool ignoreseekspeed = false)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				using (StringHelper.ThreadSafeEncoding freeHelper2 = StringHelper.GetFreeHelper())
				{
					result = System.FMOD_Studio_System_SetParameterByNameWithLabel(this.handle, freeHelper.byteFromStringUTF8(name), freeHelper2.byteFromStringUTF8(label), ignoreseekspeed);
				}
			}
			return result;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000059E8 File Offset: 0x00003BE8
		public RESULT lookupID(string path, out GUID id)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_LookupID(this.handle, freeHelper.byteFromStringUTF8(path), out id);
			}
			return result;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00005A2C File Offset: 0x00003C2C
		public RESULT lookupPath(GUID id, out string path)
		{
			path = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = System.FMOD_Studio_System_LookupPath(this.handle, ref id, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					intPtr = Marshal.AllocHGlobal(num);
					result = System.FMOD_Studio_System_LookupPath(this.handle, ref id, intPtr, num, out num);
				}
				if (result == RESULT.OK)
				{
					path = freeHelper.stringFromNative(intPtr);
				}
				Marshal.FreeHGlobal(intPtr);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00005ABC File Offset: 0x00003CBC
		public RESULT getNumListeners(out int numlisteners)
		{
			return System.FMOD_Studio_System_GetNumListeners(this.handle, out numlisteners);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00005ACA File Offset: 0x00003CCA
		public RESULT setNumListeners(int numlisteners)
		{
			return System.FMOD_Studio_System_SetNumListeners(this.handle, numlisteners);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00005AD8 File Offset: 0x00003CD8
		public RESULT getListenerAttributes(int listener, out ATTRIBUTES_3D attributes)
		{
			return System.FMOD_Studio_System_GetListenerAttributes(this.handle, listener, out attributes, IntPtr.Zero);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00005AEC File Offset: 0x00003CEC
		public RESULT getListenerAttributes(int listener, out ATTRIBUTES_3D attributes, out VECTOR attenuationposition)
		{
			return System.FMOD_Studio_System_GetListenerAttributes(this.handle, listener, out attributes, out attenuationposition);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00005AFC File Offset: 0x00003CFC
		public RESULT setListenerAttributes(int listener, ATTRIBUTES_3D attributes)
		{
			return System.FMOD_Studio_System_SetListenerAttributes(this.handle, listener, ref attributes, IntPtr.Zero);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00005B11 File Offset: 0x00003D11
		public RESULT setListenerAttributes(int listener, ATTRIBUTES_3D attributes, VECTOR attenuationposition)
		{
			return System.FMOD_Studio_System_SetListenerAttributes(this.handle, listener, ref attributes, ref attenuationposition);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00005B23 File Offset: 0x00003D23
		public RESULT getListenerWeight(int listener, out float weight)
		{
			return System.FMOD_Studio_System_GetListenerWeight(this.handle, listener, out weight);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00005B32 File Offset: 0x00003D32
		public RESULT setListenerWeight(int listener, float weight)
		{
			return System.FMOD_Studio_System_SetListenerWeight(this.handle, listener, weight);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00005B44 File Offset: 0x00003D44
		public RESULT loadBankFile(string filename, LOAD_BANK_FLAGS flags, out Bank bank)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_LoadBankFile(this.handle, freeHelper.byteFromStringUTF8(filename), flags, out bank.handle);
			}
			return result;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00005B90 File Offset: 0x00003D90
		public RESULT loadBankMemory(byte[] buffer, LOAD_BANK_FLAGS flags, out Bank bank)
		{
			GCHandle gchandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			IntPtr buffer2 = gchandle.AddrOfPinnedObject();
			RESULT result = System.FMOD_Studio_System_LoadBankMemory(this.handle, buffer2, buffer.Length, LOAD_MEMORY_MODE.LOAD_MEMORY, flags, out bank.handle);
			gchandle.Free();
			return result;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00005BCB File Offset: 0x00003DCB
		public RESULT loadBankCustom(BANK_INFO info, LOAD_BANK_FLAGS flags, out Bank bank)
		{
			info.size = Marshal.SizeOf<BANK_INFO>();
			return System.FMOD_Studio_System_LoadBankCustom(this.handle, ref info, flags, out bank.handle);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00005BED File Offset: 0x00003DED
		public RESULT unloadAll()
		{
			return System.FMOD_Studio_System_UnloadAll(this.handle);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00005BFA File Offset: 0x00003DFA
		public RESULT flushCommands()
		{
			return System.FMOD_Studio_System_FlushCommands(this.handle);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00005C07 File Offset: 0x00003E07
		public RESULT flushSampleLoading()
		{
			return System.FMOD_Studio_System_FlushSampleLoading(this.handle);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00005C14 File Offset: 0x00003E14
		public RESULT startCommandCapture(string filename, COMMANDCAPTURE_FLAGS flags)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_StartCommandCapture(this.handle, freeHelper.byteFromStringUTF8(filename), flags);
			}
			return result;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00005C58 File Offset: 0x00003E58
		public RESULT stopCommandCapture()
		{
			return System.FMOD_Studio_System_StopCommandCapture(this.handle);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00005C68 File Offset: 0x00003E68
		public RESULT loadCommandReplay(string filename, COMMANDREPLAY_FLAGS flags, out CommandReplay replay)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = System.FMOD_Studio_System_LoadCommandReplay(this.handle, freeHelper.byteFromStringUTF8(filename), flags, out replay.handle);
			}
			return result;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00005CB4 File Offset: 0x00003EB4
		public RESULT getBankCount(out int count)
		{
			return System.FMOD_Studio_System_GetBankCount(this.handle, out count);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public RESULT getBankList(out Bank[] array)
		{
			array = null;
			int num;
			RESULT result = System.FMOD_Studio_System_GetBankCount(this.handle, out num);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num == 0)
			{
				array = new Bank[0];
				return result;
			}
			IntPtr[] array2 = new IntPtr[num];
			int num2;
			result = System.FMOD_Studio_System_GetBankList(this.handle, array2, num, out num2);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num2 > num)
			{
				num2 = num;
			}
			array = new Bank[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i].handle = array2[i];
			}
			return RESULT.OK;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00005D41 File Offset: 0x00003F41
		public RESULT getParameterDescriptionCount(out int count)
		{
			return System.FMOD_Studio_System_GetParameterDescriptionCount(this.handle, out count);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00005D50 File Offset: 0x00003F50
		public RESULT getParameterDescriptionList(out PARAMETER_DESCRIPTION[] array)
		{
			array = null;
			int num;
			RESULT result = System.FMOD_Studio_System_GetParameterDescriptionCount(this.handle, out num);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num == 0)
			{
				array = new PARAMETER_DESCRIPTION[0];
				return RESULT.OK;
			}
			PARAMETER_DESCRIPTION[] array2 = new PARAMETER_DESCRIPTION[num];
			int num2;
			result = System.FMOD_Studio_System_GetParameterDescriptionList(this.handle, array2, num, out num2);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num2 != num)
			{
				Array.Resize<PARAMETER_DESCRIPTION>(ref array2, num2);
			}
			array = array2;
			return RESULT.OK;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00005DAC File Offset: 0x00003FAC
		public RESULT getCPUUsage(out CPU_USAGE usage, out CPU_USAGE usage_core)
		{
			return System.FMOD_Studio_System_GetCPUUsage(this.handle, out usage, out usage_core);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00005DBB File Offset: 0x00003FBB
		public RESULT getBufferUsage(out BUFFER_USAGE usage)
		{
			return System.FMOD_Studio_System_GetBufferUsage(this.handle, out usage);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00005DC9 File Offset: 0x00003FC9
		public RESULT resetBufferUsage()
		{
			return System.FMOD_Studio_System_ResetBufferUsage(this.handle);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00005DD6 File Offset: 0x00003FD6
		public RESULT setCallback(SYSTEM_CALLBACK callback, SYSTEM_CALLBACK_TYPE callbackmask = SYSTEM_CALLBACK_TYPE.ALL)
		{
			return System.FMOD_Studio_System_SetCallback(this.handle, callback, callbackmask);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00005DE5 File Offset: 0x00003FE5
		public RESULT getUserData(out IntPtr userdata)
		{
			return System.FMOD_Studio_System_GetUserData(this.handle, out userdata);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00005DF3 File Offset: 0x00003FF3
		public RESULT setUserData(IntPtr userdata)
		{
			return System.FMOD_Studio_System_SetUserData(this.handle, userdata);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00005E01 File Offset: 0x00004001
		public RESULT getMemoryUsage(out MEMORY_USAGE memoryusage)
		{
			return System.FMOD_Studio_System_GetMemoryUsage(this.handle, out memoryusage);
		}

		// Token: 0x060004EA RID: 1258
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_Create(out IntPtr system, uint headerversion);

		// Token: 0x060004EB RID: 1259
		[DllImport("fmodstudio")]
		private static extern bool FMOD_Studio_System_IsValid(IntPtr system);

		// Token: 0x060004EC RID: 1260
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetAdvancedSettings(IntPtr system, ref ADVANCEDSETTINGS settings);

		// Token: 0x060004ED RID: 1261
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetAdvancedSettings(IntPtr system, out ADVANCEDSETTINGS settings);

		// Token: 0x060004EE RID: 1262
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_Initialize(IntPtr system, int maxchannels, INITFLAGS studioflags, INITFLAGS flags, IntPtr extradriverdata);

		// Token: 0x060004EF RID: 1263
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_Release(IntPtr system);

		// Token: 0x060004F0 RID: 1264
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_Update(IntPtr system);

		// Token: 0x060004F1 RID: 1265
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetCoreSystem(IntPtr system, out IntPtr coresystem);

		// Token: 0x060004F2 RID: 1266
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetEvent(IntPtr system, byte[] path, out IntPtr _event);

		// Token: 0x060004F3 RID: 1267
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetBus(IntPtr system, byte[] path, out IntPtr bus);

		// Token: 0x060004F4 RID: 1268
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetVCA(IntPtr system, byte[] path, out IntPtr vca);

		// Token: 0x060004F5 RID: 1269
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetBank(IntPtr system, byte[] path, out IntPtr bank);

		// Token: 0x060004F6 RID: 1270
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetEventByID(IntPtr system, ref GUID id, out IntPtr _event);

		// Token: 0x060004F7 RID: 1271
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetBusByID(IntPtr system, ref GUID id, out IntPtr bus);

		// Token: 0x060004F8 RID: 1272
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetVCAByID(IntPtr system, ref GUID id, out IntPtr vca);

		// Token: 0x060004F9 RID: 1273
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetBankByID(IntPtr system, ref GUID id, out IntPtr bank);

		// Token: 0x060004FA RID: 1274
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetSoundInfo(IntPtr system, byte[] key, out SOUND_INFO info);

		// Token: 0x060004FB RID: 1275
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetParameterDescriptionByName(IntPtr system, byte[] name, out PARAMETER_DESCRIPTION parameter);

		// Token: 0x060004FC RID: 1276
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetParameterDescriptionByID(IntPtr system, PARAMETER_ID id, out PARAMETER_DESCRIPTION parameter);

		// Token: 0x060004FD RID: 1277
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetParameterLabelByName(IntPtr system, byte[] name, int labelindex, IntPtr label, int size, out int retrieved);

		// Token: 0x060004FE RID: 1278
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetParameterLabelByID(IntPtr system, PARAMETER_ID id, int labelindex, IntPtr label, int size, out int retrieved);

		// Token: 0x060004FF RID: 1279
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetParameterByID(IntPtr system, PARAMETER_ID id, out float value, out float finalvalue);

		// Token: 0x06000500 RID: 1280
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetParameterByID(IntPtr system, PARAMETER_ID id, float value, bool ignoreseekspeed);

		// Token: 0x06000501 RID: 1281
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetParameterByIDWithLabel(IntPtr system, PARAMETER_ID id, byte[] label, bool ignoreseekspeed);

		// Token: 0x06000502 RID: 1282
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetParametersByIDs(IntPtr system, PARAMETER_ID[] ids, float[] values, int count, bool ignoreseekspeed);

		// Token: 0x06000503 RID: 1283
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetParameterByName(IntPtr system, byte[] name, out float value, out float finalvalue);

		// Token: 0x06000504 RID: 1284
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetParameterByName(IntPtr system, byte[] name, float value, bool ignoreseekspeed);

		// Token: 0x06000505 RID: 1285
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetParameterByNameWithLabel(IntPtr system, byte[] name, byte[] label, bool ignoreseekspeed);

		// Token: 0x06000506 RID: 1286
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_LookupID(IntPtr system, byte[] path, out GUID id);

		// Token: 0x06000507 RID: 1287
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_LookupPath(IntPtr system, ref GUID id, IntPtr path, int size, out int retrieved);

		// Token: 0x06000508 RID: 1288
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetNumListeners(IntPtr system, out int numlisteners);

		// Token: 0x06000509 RID: 1289
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetNumListeners(IntPtr system, int numlisteners);

		// Token: 0x0600050A RID: 1290
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetListenerAttributes(IntPtr system, int listener, out ATTRIBUTES_3D attributes, IntPtr zero);

		// Token: 0x0600050B RID: 1291
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetListenerAttributes(IntPtr system, int listener, out ATTRIBUTES_3D attributes, out VECTOR attenuationposition);

		// Token: 0x0600050C RID: 1292
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetListenerAttributes(IntPtr system, int listener, ref ATTRIBUTES_3D attributes, IntPtr zero);

		// Token: 0x0600050D RID: 1293
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetListenerAttributes(IntPtr system, int listener, ref ATTRIBUTES_3D attributes, ref VECTOR attenuationposition);

		// Token: 0x0600050E RID: 1294
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetListenerWeight(IntPtr system, int listener, out float weight);

		// Token: 0x0600050F RID: 1295
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetListenerWeight(IntPtr system, int listener, float weight);

		// Token: 0x06000510 RID: 1296
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_LoadBankFile(IntPtr system, byte[] filename, LOAD_BANK_FLAGS flags, out IntPtr bank);

		// Token: 0x06000511 RID: 1297
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_LoadBankMemory(IntPtr system, IntPtr buffer, int length, LOAD_MEMORY_MODE mode, LOAD_BANK_FLAGS flags, out IntPtr bank);

		// Token: 0x06000512 RID: 1298
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_LoadBankCustom(IntPtr system, ref BANK_INFO info, LOAD_BANK_FLAGS flags, out IntPtr bank);

		// Token: 0x06000513 RID: 1299
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_UnloadAll(IntPtr system);

		// Token: 0x06000514 RID: 1300
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_FlushCommands(IntPtr system);

		// Token: 0x06000515 RID: 1301
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_FlushSampleLoading(IntPtr system);

		// Token: 0x06000516 RID: 1302
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_StartCommandCapture(IntPtr system, byte[] filename, COMMANDCAPTURE_FLAGS flags);

		// Token: 0x06000517 RID: 1303
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_StopCommandCapture(IntPtr system);

		// Token: 0x06000518 RID: 1304
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_LoadCommandReplay(IntPtr system, byte[] filename, COMMANDREPLAY_FLAGS flags, out IntPtr replay);

		// Token: 0x06000519 RID: 1305
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetBankCount(IntPtr system, out int count);

		// Token: 0x0600051A RID: 1306
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetBankList(IntPtr system, IntPtr[] array, int capacity, out int count);

		// Token: 0x0600051B RID: 1307
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetParameterDescriptionCount(IntPtr system, out int count);

		// Token: 0x0600051C RID: 1308
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetParameterDescriptionList(IntPtr system, [Out] PARAMETER_DESCRIPTION[] array, int capacity, out int count);

		// Token: 0x0600051D RID: 1309
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetCPUUsage(IntPtr system, out CPU_USAGE usage, out CPU_USAGE usage_core);

		// Token: 0x0600051E RID: 1310
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetBufferUsage(IntPtr system, out BUFFER_USAGE usage);

		// Token: 0x0600051F RID: 1311
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_ResetBufferUsage(IntPtr system);

		// Token: 0x06000520 RID: 1312
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetCallback(IntPtr system, SYSTEM_CALLBACK callback, SYSTEM_CALLBACK_TYPE callbackmask);

		// Token: 0x06000521 RID: 1313
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetUserData(IntPtr system, out IntPtr userdata);

		// Token: 0x06000522 RID: 1314
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_SetUserData(IntPtr system, IntPtr userdata);

		// Token: 0x06000523 RID: 1315
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_System_GetMemoryUsage(IntPtr system, out MEMORY_USAGE memoryusage);

		// Token: 0x06000524 RID: 1316 RVA: 0x00005E0F File Offset: 0x0000400F
		public System(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00005E18 File Offset: 0x00004018
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00005E2A File Offset: 0x0000402A
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00005E37 File Offset: 0x00004037
		public bool isValid()
		{
			return this.hasHandle() && System.FMOD_Studio_System_IsValid(this.handle);
		}

		// Token: 0x04000548 RID: 1352
		public IntPtr handle;
	}
}
