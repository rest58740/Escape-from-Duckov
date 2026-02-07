using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000058 RID: 88
	public static class ArchitectureInfo
	{
		// Token: 0x0600031D RID: 797 RVA: 0x00016AA7 File Offset: 0x00014CA7
		static ArchitectureInfo()
		{
			Debug.Log("Odin Serializer ArchitectureInfo initialization with defaults (all unaligned read/writes disabled).");
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00016AC0 File Offset: 0x00014CC0
		internal unsafe static void SetRuntimePlatform(RuntimePlatform platform)
		{
			if (platform <= RuntimePlatform.MetroPlayerX64)
			{
				if (platform - RuntimePlatform.OSXPlayer > 1)
				{
					switch (platform)
					{
					case RuntimePlatform.PS3:
					case RuntimePlatform.XBOX360:
					case RuntimePlatform.LinuxPlayer:
					case RuntimePlatform.WebGLPlayer:
					case RuntimePlatform.MetroPlayerX86:
					case RuntimePlatform.MetroPlayerX64:
						break;
					case RuntimePlatform.Android:
					case RuntimePlatform.NaCl:
					case (RuntimePlatform)14:
					case RuntimePlatform.FlashPlayer:
					case RuntimePlatform.LinuxEditor:
						goto IL_F0;
					default:
						goto IL_F0;
					}
				}
			}
			else if (platform != RuntimePlatform.PS4 && platform != RuntimePlatform.XboxOne && platform != RuntimePlatform.WiiU)
			{
				goto IL_F0;
			}
			try
			{
				byte[] array = new byte[8];
				try
				{
					byte[] array2;
					byte* ptr;
					if ((array2 = array) == null || array2.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array2[0];
					}
					for (int i = 0; i < 4; i++)
					{
						float num = *(float*)(ptr + i);
					}
					ArchitectureInfo.Architecture_Supports_Unaligned_Float32_Reads = true;
				}
				finally
				{
					byte[] array2 = null;
				}
			}
			catch (NullReferenceException)
			{
				ArchitectureInfo.Architecture_Supports_Unaligned_Float32_Reads = false;
			}
			if (ArchitectureInfo.Architecture_Supports_Unaligned_Float32_Reads)
			{
				Debug.Log("Odin Serializer detected whitelisted runtime platform " + platform.ToString() + " and memory read test succeeded; enabling all unaligned memory read/writes.");
				ArchitectureInfo.Architecture_Supports_All_Unaligned_ReadWrites = true;
				return;
			}
			Debug.Log("Odin Serializer detected whitelisted runtime platform " + platform.ToString() + " and memory read test failed; disabling all unaligned memory read/writes.");
			return;
			IL_F0:
			ArchitectureInfo.Architecture_Supports_Unaligned_Float32_Reads = false;
			ArchitectureInfo.Architecture_Supports_All_Unaligned_ReadWrites = false;
			Debug.Log("Odin Serializer detected non-white-listed runtime platform " + platform.ToString() + "; disabling all unaligned memory read/writes.");
		}

		// Token: 0x040000F8 RID: 248
		public static bool Architecture_Supports_Unaligned_Float32_Reads = false;

		// Token: 0x040000F9 RID: 249
		public static bool Architecture_Supports_All_Unaligned_ReadWrites = false;
	}
}
