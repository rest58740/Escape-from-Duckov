using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000F1 RID: 241
	public struct EventInstance
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x00006380 File Offset: 0x00004580
		public RESULT getDescription(out EventDescription description)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetDescription(this.handle, out description.handle);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00006393 File Offset: 0x00004593
		public RESULT getSystem(out System system)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetSystem(this.handle, out system.handle);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000063A6 File Offset: 0x000045A6
		public RESULT getVolume(out float volume)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetVolume(this.handle, out volume, IntPtr.Zero);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000063B9 File Offset: 0x000045B9
		public RESULT getVolume(out float volume, out float finalvolume)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetVolume(this.handle, out volume, out finalvolume);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000063C8 File Offset: 0x000045C8
		public RESULT setVolume(float volume)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetVolume(this.handle, volume);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x000063D6 File Offset: 0x000045D6
		public RESULT getPitch(out float pitch)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetPitch(this.handle, out pitch, IntPtr.Zero);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x000063E9 File Offset: 0x000045E9
		public RESULT getPitch(out float pitch, out float finalpitch)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetPitch(this.handle, out pitch, out finalpitch);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000063F8 File Offset: 0x000045F8
		public RESULT setPitch(float pitch)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetPitch(this.handle, pitch);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00006406 File Offset: 0x00004606
		public RESULT get3DAttributes(out ATTRIBUTES_3D attributes)
		{
			return EventInstance.FMOD_Studio_EventInstance_Get3DAttributes(this.handle, out attributes);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00006414 File Offset: 0x00004614
		public RESULT set3DAttributes(ATTRIBUTES_3D attributes)
		{
			return EventInstance.FMOD_Studio_EventInstance_Set3DAttributes(this.handle, ref attributes);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00006423 File Offset: 0x00004623
		public RESULT getListenerMask(out uint mask)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetListenerMask(this.handle, out mask);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00006431 File Offset: 0x00004631
		public RESULT setListenerMask(uint mask)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetListenerMask(this.handle, mask);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0000643F File Offset: 0x0000463F
		public RESULT getProperty(EVENT_PROPERTY index, out float value)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetProperty(this.handle, index, out value);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0000644E File Offset: 0x0000464E
		public RESULT setProperty(EVENT_PROPERTY index, float value)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetProperty(this.handle, index, value);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0000645D File Offset: 0x0000465D
		public RESULT getReverbLevel(int index, out float level)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetReverbLevel(this.handle, index, out level);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0000646C File Offset: 0x0000466C
		public RESULT setReverbLevel(int index, float level)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetReverbLevel(this.handle, index, level);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0000647B File Offset: 0x0000467B
		public RESULT getPaused(out bool paused)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetPaused(this.handle, out paused);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00006489 File Offset: 0x00004689
		public RESULT setPaused(bool paused)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetPaused(this.handle, paused);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00006497 File Offset: 0x00004697
		public RESULT start()
		{
			return EventInstance.FMOD_Studio_EventInstance_Start(this.handle);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x000064A4 File Offset: 0x000046A4
		public RESULT stop(STOP_MODE mode)
		{
			return EventInstance.FMOD_Studio_EventInstance_Stop(this.handle, mode);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x000064B2 File Offset: 0x000046B2
		public RESULT getTimelinePosition(out int position)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetTimelinePosition(this.handle, out position);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000064C0 File Offset: 0x000046C0
		public RESULT setTimelinePosition(int position)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetTimelinePosition(this.handle, position);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000064CE File Offset: 0x000046CE
		public RESULT getPlaybackState(out PLAYBACK_STATE state)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetPlaybackState(this.handle, out state);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000064DC File Offset: 0x000046DC
		public RESULT getChannelGroup(out ChannelGroup group)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetChannelGroup(this.handle, out group.handle);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000064EF File Offset: 0x000046EF
		public RESULT getMinMaxDistance(out float min, out float max)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetMinMaxDistance(this.handle, out min, out max);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000064FE File Offset: 0x000046FE
		public RESULT release()
		{
			return EventInstance.FMOD_Studio_EventInstance_Release(this.handle);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0000650B File Offset: 0x0000470B
		public RESULT isVirtual(out bool virtualstate)
		{
			return EventInstance.FMOD_Studio_EventInstance_IsVirtual(this.handle, out virtualstate);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000651C File Offset: 0x0000471C
		public RESULT getParameterByID(PARAMETER_ID id, out float value)
		{
			float num;
			return this.getParameterByID(id, out value, out num);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00006533 File Offset: 0x00004733
		public RESULT getParameterByID(PARAMETER_ID id, out float value, out float finalvalue)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetParameterByID(this.handle, id, out value, out finalvalue);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00006543 File Offset: 0x00004743
		public RESULT setParameterByID(PARAMETER_ID id, float value, bool ignoreseekspeed = false)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetParameterByID(this.handle, id, value, ignoreseekspeed);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00006554 File Offset: 0x00004754
		public RESULT setParameterByIDWithLabel(PARAMETER_ID id, string label, bool ignoreseekspeed = false)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = EventInstance.FMOD_Studio_EventInstance_SetParameterByIDWithLabel(this.handle, id, freeHelper.byteFromStringUTF8(label), ignoreseekspeed);
			}
			return result;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000659C File Offset: 0x0000479C
		public RESULT setParametersByIDs(PARAMETER_ID[] ids, float[] values, int count, bool ignoreseekspeed = false)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetParametersByIDs(this.handle, ids, values, count, ignoreseekspeed);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000065B0 File Offset: 0x000047B0
		public RESULT getParameterByName(string name, out float value)
		{
			float num;
			return this.getParameterByName(name, out value, out num);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000065C8 File Offset: 0x000047C8
		public RESULT getParameterByName(string name, out float value, out float finalvalue)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = EventInstance.FMOD_Studio_EventInstance_GetParameterByName(this.handle, freeHelper.byteFromStringUTF8(name), out value, out finalvalue);
			}
			return result;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00006610 File Offset: 0x00004810
		public RESULT setParameterByName(string name, float value, bool ignoreseekspeed = false)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = EventInstance.FMOD_Studio_EventInstance_SetParameterByName(this.handle, freeHelper.byteFromStringUTF8(name), value, ignoreseekspeed);
			}
			return result;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00006658 File Offset: 0x00004858
		public RESULT setParameterByNameWithLabel(string name, string label, bool ignoreseekspeed = false)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				using (StringHelper.ThreadSafeEncoding freeHelper2 = StringHelper.GetFreeHelper())
				{
					result = EventInstance.FMOD_Studio_EventInstance_SetParameterByNameWithLabel(this.handle, freeHelper.byteFromStringUTF8(name), freeHelper2.byteFromStringUTF8(label), ignoreseekspeed);
				}
			}
			return result;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000066C0 File Offset: 0x000048C0
		public RESULT keyOff()
		{
			return EventInstance.FMOD_Studio_EventInstance_KeyOff(this.handle);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000066CD File Offset: 0x000048CD
		public RESULT setCallback(EVENT_CALLBACK callback, EVENT_CALLBACK_TYPE callbackmask = EVENT_CALLBACK_TYPE.ALL)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetCallback(this.handle, callback, callbackmask);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x000066DC File Offset: 0x000048DC
		public RESULT getUserData(out IntPtr userdata)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetUserData(this.handle, out userdata);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000066EA File Offset: 0x000048EA
		public RESULT setUserData(IntPtr userdata)
		{
			return EventInstance.FMOD_Studio_EventInstance_SetUserData(this.handle, userdata);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000066F8 File Offset: 0x000048F8
		public RESULT getCPUUsage(out uint exclusive, out uint inclusive)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetCPUUsage(this.handle, out exclusive, out inclusive);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00006707 File Offset: 0x00004907
		public RESULT getMemoryUsage(out MEMORY_USAGE memoryusage)
		{
			return EventInstance.FMOD_Studio_EventInstance_GetMemoryUsage(this.handle, out memoryusage);
		}

		// Token: 0x06000595 RID: 1429
		[DllImport("fmodstudio")]
		private static extern bool FMOD_Studio_EventInstance_IsValid(IntPtr _event);

		// Token: 0x06000596 RID: 1430
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetDescription(IntPtr _event, out IntPtr description);

		// Token: 0x06000597 RID: 1431
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetSystem(IntPtr _event, out IntPtr system);

		// Token: 0x06000598 RID: 1432
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetVolume(IntPtr _event, out float volume, IntPtr zero);

		// Token: 0x06000599 RID: 1433
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetVolume(IntPtr _event, out float volume, out float finalvolume);

		// Token: 0x0600059A RID: 1434
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetVolume(IntPtr _event, float volume);

		// Token: 0x0600059B RID: 1435
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetPitch(IntPtr _event, out float pitch, IntPtr zero);

		// Token: 0x0600059C RID: 1436
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetPitch(IntPtr _event, out float pitch, out float finalpitch);

		// Token: 0x0600059D RID: 1437
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetPitch(IntPtr _event, float pitch);

		// Token: 0x0600059E RID: 1438
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_Get3DAttributes(IntPtr _event, out ATTRIBUTES_3D attributes);

		// Token: 0x0600059F RID: 1439
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_Set3DAttributes(IntPtr _event, ref ATTRIBUTES_3D attributes);

		// Token: 0x060005A0 RID: 1440
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetListenerMask(IntPtr _event, out uint mask);

		// Token: 0x060005A1 RID: 1441
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetListenerMask(IntPtr _event, uint mask);

		// Token: 0x060005A2 RID: 1442
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetProperty(IntPtr _event, EVENT_PROPERTY index, out float value);

		// Token: 0x060005A3 RID: 1443
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetProperty(IntPtr _event, EVENT_PROPERTY index, float value);

		// Token: 0x060005A4 RID: 1444
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetReverbLevel(IntPtr _event, int index, out float level);

		// Token: 0x060005A5 RID: 1445
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetReverbLevel(IntPtr _event, int index, float level);

		// Token: 0x060005A6 RID: 1446
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetPaused(IntPtr _event, out bool paused);

		// Token: 0x060005A7 RID: 1447
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetPaused(IntPtr _event, bool paused);

		// Token: 0x060005A8 RID: 1448
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_Start(IntPtr _event);

		// Token: 0x060005A9 RID: 1449
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_Stop(IntPtr _event, STOP_MODE mode);

		// Token: 0x060005AA RID: 1450
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetTimelinePosition(IntPtr _event, out int position);

		// Token: 0x060005AB RID: 1451
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetTimelinePosition(IntPtr _event, int position);

		// Token: 0x060005AC RID: 1452
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetPlaybackState(IntPtr _event, out PLAYBACK_STATE state);

		// Token: 0x060005AD RID: 1453
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetChannelGroup(IntPtr _event, out IntPtr group);

		// Token: 0x060005AE RID: 1454
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetMinMaxDistance(IntPtr _event, out float min, out float max);

		// Token: 0x060005AF RID: 1455
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_Release(IntPtr _event);

		// Token: 0x060005B0 RID: 1456
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_IsVirtual(IntPtr _event, out bool virtualstate);

		// Token: 0x060005B1 RID: 1457
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetParameterByName(IntPtr _event, byte[] name, out float value, out float finalvalue);

		// Token: 0x060005B2 RID: 1458
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetParameterByName(IntPtr _event, byte[] name, float value, bool ignoreseekspeed);

		// Token: 0x060005B3 RID: 1459
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetParameterByNameWithLabel(IntPtr _event, byte[] name, byte[] label, bool ignoreseekspeed);

		// Token: 0x060005B4 RID: 1460
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetParameterByID(IntPtr _event, PARAMETER_ID id, out float value, out float finalvalue);

		// Token: 0x060005B5 RID: 1461
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetParameterByID(IntPtr _event, PARAMETER_ID id, float value, bool ignoreseekspeed);

		// Token: 0x060005B6 RID: 1462
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetParameterByIDWithLabel(IntPtr _event, PARAMETER_ID id, byte[] label, bool ignoreseekspeed);

		// Token: 0x060005B7 RID: 1463
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetParametersByIDs(IntPtr _event, PARAMETER_ID[] ids, float[] values, int count, bool ignoreseekspeed);

		// Token: 0x060005B8 RID: 1464
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_KeyOff(IntPtr _event);

		// Token: 0x060005B9 RID: 1465
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetCallback(IntPtr _event, EVENT_CALLBACK callback, EVENT_CALLBACK_TYPE callbackmask);

		// Token: 0x060005BA RID: 1466
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetUserData(IntPtr _event, out IntPtr userdata);

		// Token: 0x060005BB RID: 1467
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_SetUserData(IntPtr _event, IntPtr userdata);

		// Token: 0x060005BC RID: 1468
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetCPUUsage(IntPtr _event, out uint exclusive, out uint inclusive);

		// Token: 0x060005BD RID: 1469
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_EventInstance_GetMemoryUsage(IntPtr _event, out MEMORY_USAGE memoryusage);

		// Token: 0x060005BE RID: 1470 RVA: 0x00006715 File Offset: 0x00004915
		public EventInstance(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0000671E File Offset: 0x0000491E
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00006730 File Offset: 0x00004930
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000673D File Offset: 0x0000493D
		public bool isValid()
		{
			return this.hasHandle() && EventInstance.FMOD_Studio_EventInstance_IsValid(this.handle);
		}

		// Token: 0x0400054A RID: 1354
		public IntPtr handle;
	}
}
