using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace FMODUnityResonance
{
	// Token: 0x02000003 RID: 3
	public static class FmodResonanceAudio
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public static void UpdateAudioRoom(FmodResonanceAudioRoom room, bool roomEnabled)
		{
			if (roomEnabled)
			{
				if (!FmodResonanceAudio.enabledRooms.Contains(room))
				{
					FmodResonanceAudio.enabledRooms.Add(room);
				}
			}
			else
			{
				FmodResonanceAudio.enabledRooms.Remove(room);
			}
			if (FmodResonanceAudio.enabledRooms.Count > 0)
			{
				FmodResonanceAudio.RoomProperties roomProperties = FmodResonanceAudio.GetRoomProperties(FmodResonanceAudio.enabledRooms[FmodResonanceAudio.enabledRooms.Count - 1]);
				IntPtr intPtr = Marshal.AllocHGlobal(FmodResonanceAudio.roomPropertiesSize);
				Marshal.StructureToPtr<FmodResonanceAudio.RoomProperties>(roomProperties, intPtr, false);
				FmodResonanceAudio.ListenerPlugin.setParameterData(FmodResonanceAudio.roomPropertiesIndex, FmodResonanceAudio.GetBytes(intPtr, FmodResonanceAudio.roomPropertiesSize));
				Marshal.FreeHGlobal(intPtr);
				return;
			}
			FmodResonanceAudio.ListenerPlugin.setParameterData(FmodResonanceAudio.roomPropertiesIndex, FmodResonanceAudio.GetBytes(IntPtr.Zero, 0));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002174 File Offset: 0x00000374
		public static bool IsListenerInsideRoom(FmodResonanceAudioRoom room)
		{
			VECTOR vector;
			RuntimeManager.CoreSystem.get3DListenerAttributes(0, ref FmodResonanceAudio.listenerPositionFmod, ref vector, ref vector, ref vector);
			Vector3 point = new Vector3(FmodResonanceAudio.listenerPositionFmod.x, FmodResonanceAudio.listenerPositionFmod.y, FmodResonanceAudio.listenerPositionFmod.z) - room.transform.position;
			Quaternion rotation = Quaternion.Inverse(room.transform.rotation);
			FmodResonanceAudio.bounds.size = Vector3.Scale(room.transform.lossyScale, room.Size);
			return FmodResonanceAudio.bounds.Contains(rotation * point);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002211 File Offset: 0x00000411
		private static DSP ListenerPlugin
		{
			get
			{
				if (!FmodResonanceAudio.listenerPlugin.hasHandle())
				{
					FmodResonanceAudio.listenerPlugin = FmodResonanceAudio.Initialize();
				}
				return FmodResonanceAudio.listenerPlugin;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000222E File Offset: 0x0000042E
		private static float ConvertAmplitudeFromDb(float db)
		{
			return Mathf.Pow(10f, 0.05f * db);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002244 File Offset: 0x00000444
		private static void ConvertAudioTransformFromUnity(ref Vector3 position, ref Quaternion rotation)
		{
			Matrix4x4 rhs = Matrix4x4.TRS(position, rotation, Vector3.one);
			rhs = FmodResonanceAudio.flipZ * rhs * FmodResonanceAudio.flipZ;
			position = rhs.GetColumn(3);
			rotation = Quaternion.LookRotation(rhs.GetColumn(2), rhs.GetColumn(1));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022B8 File Offset: 0x000004B8
		private static byte[] GetBytes(IntPtr ptr, int length)
		{
			if (ptr != IntPtr.Zero)
			{
				byte[] array = new byte[length];
				Marshal.Copy(ptr, array, 0, length);
				return array;
			}
			return new byte[1];
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022EC File Offset: 0x000004EC
		private static FmodResonanceAudio.RoomProperties GetRoomProperties(FmodResonanceAudioRoom room)
		{
			Vector3 position = room.transform.position;
			Quaternion rotation = room.transform.rotation;
			Vector3 vector = Vector3.Scale(room.transform.lossyScale, room.Size);
			FmodResonanceAudio.ConvertAudioTransformFromUnity(ref position, ref rotation);
			FmodResonanceAudio.RoomProperties result;
			result.PositionX = position.x;
			result.PositionY = position.y;
			result.PositionZ = position.z;
			result.RotationX = rotation.x;
			result.RotationY = rotation.y;
			result.RotationZ = rotation.z;
			result.RotationW = rotation.w;
			result.DimensionsX = vector.x;
			result.DimensionsY = vector.y;
			result.DimensionsZ = vector.z;
			result.MaterialLeft = room.LeftWall;
			result.MaterialRight = room.RightWall;
			result.MaterialBottom = room.Floor;
			result.MaterialTop = room.Ceiling;
			result.MaterialFront = room.FrontWall;
			result.MaterialBack = room.BackWall;
			result.ReverbGain = FmodResonanceAudio.ConvertAmplitudeFromDb(room.ReverbGainDb);
			result.ReverbTime = room.ReverbTime;
			result.ReverbBrightness = room.ReverbBrightness;
			result.ReflectionScalar = room.Reflectivity;
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000243C File Offset: 0x0000063C
		private static DSP Initialize()
		{
			int num = 0;
			DSP result = default(DSP);
			Bank[] array = null;
			RuntimeManager.StudioSystem.getBankCount(ref num);
			RuntimeManager.StudioSystem.getBankList(ref array);
			for (int i = 0; i < num; i++)
			{
				int num2 = 0;
				Bus[] array2 = null;
				array[i].getBusCount(ref num2);
				array[i].getBusList(ref array2);
				for (int j = 0; j < num2; j++)
				{
					string text = null;
					array2[j].getPath(ref text);
					RuntimeManager.StudioSystem.getBus(text, ref array2[j]);
					array2[j].lockChannelGroup();
					RuntimeManager.StudioSystem.flushCommands();
					ChannelGroup channelGroup;
					array2[j].getChannelGroup(ref channelGroup);
					if (channelGroup.hasHandle())
					{
						int num3 = 0;
						channelGroup.getNumDSPs(ref num3);
						for (int k = 0; k < num3; k++)
						{
							channelGroup.getDSP(k, ref result);
							int num4 = 0;
							uint num5 = 0U;
							string text2;
							result.getInfo(ref text2, ref num5, ref num4, ref num4, ref num4);
							if (text2.ToString().Equals(FmodResonanceAudio.listenerPluginName) && result.hasHandle())
							{
								return result;
							}
						}
					}
					array2[j].unlockChannelGroup();
				}
			}
			RuntimeUtils.DebugLogError(FmodResonanceAudio.listenerPluginName + " not found in the FMOD project.");
			return result;
		}

		// Token: 0x04000001 RID: 1
		public const float MaxGainDb = 24f;

		// Token: 0x04000002 RID: 2
		public const float MinGainDb = -24f;

		// Token: 0x04000003 RID: 3
		public const float MaxReverbBrightness = 1f;

		// Token: 0x04000004 RID: 4
		public const float MinReverbBrightness = -1f;

		// Token: 0x04000005 RID: 5
		public const float MaxReverbTime = 3f;

		// Token: 0x04000006 RID: 6
		public const float MaxReflectivity = 2f;

		// Token: 0x04000007 RID: 7
		private static readonly Matrix4x4 flipZ = Matrix4x4.Scale(new Vector3(1f, 1f, -1f));

		// Token: 0x04000008 RID: 8
		private static readonly string listenerPluginName = "Resonance Audio Listener";

		// Token: 0x04000009 RID: 9
		private static readonly int roomPropertiesSize = Marshal.SizeOf<FmodResonanceAudio.RoomProperties>();

		// Token: 0x0400000A RID: 10
		private static readonly int roomPropertiesIndex = 1;

		// Token: 0x0400000B RID: 11
		private static Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

		// Token: 0x0400000C RID: 12
		private static List<FmodResonanceAudioRoom> enabledRooms = new List<FmodResonanceAudioRoom>();

		// Token: 0x0400000D RID: 13
		private static VECTOR listenerPositionFmod = default(VECTOR);

		// Token: 0x0400000E RID: 14
		private static DSP listenerPlugin;

		// Token: 0x02000007 RID: 7
		private struct RoomProperties
		{
			// Token: 0x04000021 RID: 33
			public float PositionX;

			// Token: 0x04000022 RID: 34
			public float PositionY;

			// Token: 0x04000023 RID: 35
			public float PositionZ;

			// Token: 0x04000024 RID: 36
			public float RotationX;

			// Token: 0x04000025 RID: 37
			public float RotationY;

			// Token: 0x04000026 RID: 38
			public float RotationZ;

			// Token: 0x04000027 RID: 39
			public float RotationW;

			// Token: 0x04000028 RID: 40
			public float DimensionsX;

			// Token: 0x04000029 RID: 41
			public float DimensionsY;

			// Token: 0x0400002A RID: 42
			public float DimensionsZ;

			// Token: 0x0400002B RID: 43
			public FmodResonanceAudioRoom.SurfaceMaterial MaterialLeft;

			// Token: 0x0400002C RID: 44
			public FmodResonanceAudioRoom.SurfaceMaterial MaterialRight;

			// Token: 0x0400002D RID: 45
			public FmodResonanceAudioRoom.SurfaceMaterial MaterialBottom;

			// Token: 0x0400002E RID: 46
			public FmodResonanceAudioRoom.SurfaceMaterial MaterialTop;

			// Token: 0x0400002F RID: 47
			public FmodResonanceAudioRoom.SurfaceMaterial MaterialFront;

			// Token: 0x04000030 RID: 48
			public FmodResonanceAudioRoom.SurfaceMaterial MaterialBack;

			// Token: 0x04000031 RID: 49
			public float ReflectionScalar;

			// Token: 0x04000032 RID: 50
			public float ReverbGain;

			// Token: 0x04000033 RID: 51
			public float ReverbTime;

			// Token: 0x04000034 RID: 52
			public float ReverbBrightness;
		}
	}
}
