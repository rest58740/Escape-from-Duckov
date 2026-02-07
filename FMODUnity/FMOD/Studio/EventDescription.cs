using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000F0 RID: 240
	public struct EventDescription
	{
		// Token: 0x06000528 RID: 1320 RVA: 0x00005E4E File Offset: 0x0000404E
		public RESULT getID(out GUID id)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetID(this.handle, out id);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00005E5C File Offset: 0x0000405C
		public RESULT getPath(out string path)
		{
			path = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = EventDescription.FMOD_Studio_EventDescription_GetPath(this.handle, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					intPtr = Marshal.AllocHGlobal(num);
					result = EventDescription.FMOD_Studio_EventDescription_GetPath(this.handle, intPtr, num, out num);
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

		// Token: 0x0600052A RID: 1322 RVA: 0x00005EE8 File Offset: 0x000040E8
		public RESULT getParameterDescriptionCount(out int count)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetParameterDescriptionCount(this.handle, out count);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00005EF6 File Offset: 0x000040F6
		public RESULT getParameterDescriptionByIndex(int index, out PARAMETER_DESCRIPTION parameter)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetParameterDescriptionByIndex(this.handle, index, out parameter);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00005F08 File Offset: 0x00004108
		public RESULT getParameterDescriptionByName(string name, out PARAMETER_DESCRIPTION parameter)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = EventDescription.FMOD_Studio_EventDescription_GetParameterDescriptionByName(this.handle, freeHelper.byteFromStringUTF8(name), out parameter);
			}
			return result;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00005F4C File Offset: 0x0000414C
		public RESULT getParameterDescriptionByID(PARAMETER_ID id, out PARAMETER_DESCRIPTION parameter)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetParameterDescriptionByID(this.handle, id, out parameter);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00005F5C File Offset: 0x0000415C
		public RESULT getParameterLabelByIndex(int index, int labelindex, out string label)
		{
			label = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByIndex(this.handle, index, labelindex, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByIndex(this.handle, index, labelindex, IntPtr.Zero, 0, out num);
					intPtr = Marshal.AllocHGlobal(num);
					result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByIndex(this.handle, index, labelindex, intPtr, num, out num);
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

		// Token: 0x0600052F RID: 1327 RVA: 0x00006004 File Offset: 0x00004204
		public RESULT getParameterLabelByName(string name, int labelindex, out string label)
		{
			label = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				byte[] name2 = freeHelper.byteFromStringUTF8(name);
				RESULT result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByName(this.handle, name2, labelindex, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByName(this.handle, name2, labelindex, IntPtr.Zero, 0, out num);
					intPtr = Marshal.AllocHGlobal(num);
					result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByName(this.handle, name2, labelindex, intPtr, num, out num);
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

		// Token: 0x06000530 RID: 1328 RVA: 0x000060B8 File Offset: 0x000042B8
		public RESULT getParameterLabelByID(PARAMETER_ID id, int labelindex, out string label)
		{
			label = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByID(this.handle, id, labelindex, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByID(this.handle, id, labelindex, IntPtr.Zero, 0, out num);
					intPtr = Marshal.AllocHGlobal(num);
					result = EventDescription.FMOD_Studio_EventDescription_GetParameterLabelByID(this.handle, id, labelindex, intPtr, num, out num);
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

		// Token: 0x06000531 RID: 1329 RVA: 0x00006160 File Offset: 0x00004360
		public RESULT getUserPropertyCount(out int count)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetUserPropertyCount(this.handle, out count);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000616E File Offset: 0x0000436E
		public RESULT getUserPropertyByIndex(int index, out USER_PROPERTY property)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetUserPropertyByIndex(this.handle, index, out property);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00006180 File Offset: 0x00004380
		public RESULT getUserProperty(string name, out USER_PROPERTY property)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = EventDescription.FMOD_Studio_EventDescription_GetUserProperty(this.handle, freeHelper.byteFromStringUTF8(name), out property);
			}
			return result;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000061C4 File Offset: 0x000043C4
		public RESULT getLength(out int length)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetLength(this.handle, out length);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000061D2 File Offset: 0x000043D2
		public RESULT getMinMaxDistance(out float min, out float max)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetMinMaxDistance(this.handle, out min, out max);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x000061E1 File Offset: 0x000043E1
		public RESULT getSoundSize(out float size)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetSoundSize(this.handle, out size);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x000061EF File Offset: 0x000043EF
		public RESULT isSnapshot(out bool snapshot)
		{
			return EventDescription.FMOD_Studio_EventDescription_IsSnapshot(this.handle, out snapshot);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000061FD File Offset: 0x000043FD
		public RESULT isOneshot(out bool oneshot)
		{
			return EventDescription.FMOD_Studio_EventDescription_IsOneshot(this.handle, out oneshot);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000620B File Offset: 0x0000440B
		public RESULT isStream(out bool isStream)
		{
			return EventDescription.FMOD_Studio_EventDescription_IsStream(this.handle, out isStream);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00006219 File Offset: 0x00004419
		public RESULT is3D(out bool is3D)
		{
			return EventDescription.FMOD_Studio_EventDescription_Is3D(this.handle, out is3D);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00006227 File Offset: 0x00004427
		public RESULT isDopplerEnabled(out bool doppler)
		{
			return EventDescription.FMOD_Studio_EventDescription_IsDopplerEnabled(this.handle, out doppler);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00006235 File Offset: 0x00004435
		public RESULT hasSustainPoint(out bool sustainPoint)
		{
			return EventDescription.FMOD_Studio_EventDescription_HasSustainPoint(this.handle, out sustainPoint);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00006243 File Offset: 0x00004443
		public RESULT createInstance(out EventInstance instance)
		{
			return EventDescription.FMOD_Studio_EventDescription_CreateInstance(this.handle, out instance.handle);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00006256 File Offset: 0x00004456
		public RESULT getInstanceCount(out int count)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetInstanceCount(this.handle, out count);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00006264 File Offset: 0x00004464
		public RESULT getInstanceList(out EventInstance[] array)
		{
			array = null;
			int num;
			RESULT result = EventDescription.FMOD_Studio_EventDescription_GetInstanceCount(this.handle, out num);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num == 0)
			{
				array = new EventInstance[0];
				return result;
			}
			IntPtr[] array2 = new IntPtr[num];
			int num2;
			result = EventDescription.FMOD_Studio_EventDescription_GetInstanceList(this.handle, array2, num, out num2);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num2 > num)
			{
				num2 = num;
			}
			array = new EventInstance[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i].handle = array2[i];
			}
			return RESULT.OK;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000062E1 File Offset: 0x000044E1
		public RESULT loadSampleData()
		{
			return EventDescription.FMOD_Studio_EventDescription_LoadSampleData(this.handle);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000062EE File Offset: 0x000044EE
		public RESULT unloadSampleData()
		{
			return EventDescription.FMOD_Studio_EventDescription_UnloadSampleData(this.handle);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000062FB File Offset: 0x000044FB
		public RESULT getSampleLoadingState(out LOADING_STATE state)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetSampleLoadingState(this.handle, out state);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00006309 File Offset: 0x00004509
		public RESULT releaseAllInstances()
		{
			return EventDescription.FMOD_Studio_EventDescription_ReleaseAllInstances(this.handle);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00006316 File Offset: 0x00004516
		public RESULT setCallback(EVENT_CALLBACK callback, EVENT_CALLBACK_TYPE callbackmask = EVENT_CALLBACK_TYPE.ALL)
		{
			return EventDescription.FMOD_Studio_EventDescription_SetCallback(this.handle, callback, callbackmask);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00006325 File Offset: 0x00004525
		public RESULT getUserData(out IntPtr userdata)
		{
			return EventDescription.FMOD_Studio_EventDescription_GetUserData(this.handle, out userdata);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00006333 File Offset: 0x00004533
		public RESULT setUserData(IntPtr userdata)
		{
			return EventDescription.FMOD_Studio_EventDescription_SetUserData(this.handle, userdata);
		}

		// Token: 0x06000547 RID: 1351
		[DllImport("fmodstudio")]
		private static extern bool FMOD_Studio_EventDescription_IsValid(IntPtr eventdescription);

		// Token: 0x06000548 RID: 1352
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetID(IntPtr eventdescription, out GUID id);

		// Token: 0x06000549 RID: 1353
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetPath(IntPtr eventdescription, IntPtr path, int size, out int retrieved);

		// Token: 0x0600054A RID: 1354
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetParameterDescriptionCount(IntPtr eventdescription, out int count);

		// Token: 0x0600054B RID: 1355
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetParameterDescriptionByIndex(IntPtr eventdescription, int index, out PARAMETER_DESCRIPTION parameter);

		// Token: 0x0600054C RID: 1356
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetParameterDescriptionByName(IntPtr eventdescription, byte[] name, out PARAMETER_DESCRIPTION parameter);

		// Token: 0x0600054D RID: 1357
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetParameterDescriptionByID(IntPtr eventdescription, PARAMETER_ID id, out PARAMETER_DESCRIPTION parameter);

		// Token: 0x0600054E RID: 1358
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetParameterLabelByIndex(IntPtr eventdescription, int index, int labelindex, IntPtr label, int size, out int retrieved);

		// Token: 0x0600054F RID: 1359
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetParameterLabelByName(IntPtr eventdescription, byte[] name, int labelindex, IntPtr label, int size, out int retrieved);

		// Token: 0x06000550 RID: 1360
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetParameterLabelByID(IntPtr eventdescription, PARAMETER_ID id, int labelindex, IntPtr label, int size, out int retrieved);

		// Token: 0x06000551 RID: 1361
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetUserPropertyCount(IntPtr eventdescription, out int count);

		// Token: 0x06000552 RID: 1362
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetUserPropertyByIndex(IntPtr eventdescription, int index, out USER_PROPERTY property);

		// Token: 0x06000553 RID: 1363
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetUserProperty(IntPtr eventdescription, byte[] name, out USER_PROPERTY property);

		// Token: 0x06000554 RID: 1364
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetLength(IntPtr eventdescription, out int length);

		// Token: 0x06000555 RID: 1365
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetMinMaxDistance(IntPtr eventdescription, out float min, out float max);

		// Token: 0x06000556 RID: 1366
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetSoundSize(IntPtr eventdescription, out float size);

		// Token: 0x06000557 RID: 1367
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_IsSnapshot(IntPtr eventdescription, out bool snapshot);

		// Token: 0x06000558 RID: 1368
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_IsOneshot(IntPtr eventdescription, out bool oneshot);

		// Token: 0x06000559 RID: 1369
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_IsStream(IntPtr eventdescription, out bool isStream);

		// Token: 0x0600055A RID: 1370
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_Is3D(IntPtr eventdescription, out bool is3D);

		// Token: 0x0600055B RID: 1371
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_IsDopplerEnabled(IntPtr eventdescription, out bool doppler);

		// Token: 0x0600055C RID: 1372
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_HasSustainPoint(IntPtr eventdescription, out bool sustainPoint);

		// Token: 0x0600055D RID: 1373
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_CreateInstance(IntPtr eventdescription, out IntPtr instance);

		// Token: 0x0600055E RID: 1374
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetInstanceCount(IntPtr eventdescription, out int count);

		// Token: 0x0600055F RID: 1375
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetInstanceList(IntPtr eventdescription, IntPtr[] array, int capacity, out int count);

		// Token: 0x06000560 RID: 1376
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_LoadSampleData(IntPtr eventdescription);

		// Token: 0x06000561 RID: 1377
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_UnloadSampleData(IntPtr eventdescription);

		// Token: 0x06000562 RID: 1378
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetSampleLoadingState(IntPtr eventdescription, out LOADING_STATE state);

		// Token: 0x06000563 RID: 1379
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_ReleaseAllInstances(IntPtr eventdescription);

		// Token: 0x06000564 RID: 1380
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_SetCallback(IntPtr eventdescription, EVENT_CALLBACK callback, EVENT_CALLBACK_TYPE callbackmask);

		// Token: 0x06000565 RID: 1381
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_GetUserData(IntPtr eventdescription, out IntPtr userdata);

		// Token: 0x06000566 RID: 1382
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventDescription_SetUserData(IntPtr eventdescription, IntPtr userdata);

		// Token: 0x06000567 RID: 1383 RVA: 0x00006341 File Offset: 0x00004541
		public EventDescription(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0000634A File Offset: 0x0000454A
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0000635C File Offset: 0x0000455C
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00006369 File Offset: 0x00004569
		public bool isValid()
		{
			return this.hasHandle() && EventDescription.FMOD_Studio_EventDescription_IsValid(this.handle);
		}

		// Token: 0x04000549 RID: 1353
		public IntPtr handle;
	}
}
