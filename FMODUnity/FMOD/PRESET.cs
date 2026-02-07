using System;

namespace FMOD
{
	// Token: 0x0200003F RID: 63
	public class PRESET
	{
		// Token: 0x0600006A RID: 106 RVA: 0x0000251C File Offset: 0x0000071C
		public static REVERB_PROPERTIES OFF()
		{
			return new REVERB_PROPERTIES(1000f, 7f, 11f, 5000f, 100f, 100f, 100f, 250f, 0f, 20f, 96f, -80f);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000256C File Offset: 0x0000076C
		public static REVERB_PROPERTIES GENERIC()
		{
			return new REVERB_PROPERTIES(1500f, 7f, 11f, 5000f, 83f, 100f, 100f, 250f, 0f, 14500f, 96f, -8f);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000025BC File Offset: 0x000007BC
		public static REVERB_PROPERTIES PADDEDCELL()
		{
			return new REVERB_PROPERTIES(170f, 1f, 2f, 5000f, 10f, 100f, 100f, 250f, 0f, 160f, 84f, -7.8f);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000260C File Offset: 0x0000080C
		public static REVERB_PROPERTIES ROOM()
		{
			return new REVERB_PROPERTIES(400f, 2f, 3f, 5000f, 83f, 100f, 100f, 250f, 0f, 6050f, 88f, -9.4f);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000265C File Offset: 0x0000085C
		public static REVERB_PROPERTIES BATHROOM()
		{
			return new REVERB_PROPERTIES(1500f, 7f, 11f, 5000f, 54f, 100f, 60f, 250f, 0f, 2900f, 83f, 0.5f);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000026AC File Offset: 0x000008AC
		public static REVERB_PROPERTIES LIVINGROOM()
		{
			return new REVERB_PROPERTIES(500f, 3f, 4f, 5000f, 10f, 100f, 100f, 250f, 0f, 160f, 58f, -19f);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000026FC File Offset: 0x000008FC
		public static REVERB_PROPERTIES STONEROOM()
		{
			return new REVERB_PROPERTIES(2300f, 12f, 17f, 5000f, 64f, 100f, 100f, 250f, 0f, 7800f, 71f, -8.5f);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000274C File Offset: 0x0000094C
		public static REVERB_PROPERTIES AUDITORIUM()
		{
			return new REVERB_PROPERTIES(4300f, 20f, 30f, 5000f, 59f, 100f, 100f, 250f, 0f, 5850f, 64f, -11.7f);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000279C File Offset: 0x0000099C
		public static REVERB_PROPERTIES CONCERTHALL()
		{
			return new REVERB_PROPERTIES(3900f, 20f, 29f, 5000f, 70f, 100f, 100f, 250f, 0f, 5650f, 80f, -9.8f);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000027EC File Offset: 0x000009EC
		public static REVERB_PROPERTIES CAVE()
		{
			return new REVERB_PROPERTIES(2900f, 15f, 22f, 5000f, 100f, 100f, 100f, 250f, 0f, 20000f, 59f, -11.3f);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000283C File Offset: 0x00000A3C
		public static REVERB_PROPERTIES ARENA()
		{
			return new REVERB_PROPERTIES(7200f, 20f, 30f, 5000f, 33f, 100f, 100f, 250f, 0f, 4500f, 80f, -9.6f);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000288C File Offset: 0x00000A8C
		public static REVERB_PROPERTIES HANGAR()
		{
			return new REVERB_PROPERTIES(10000f, 20f, 30f, 5000f, 23f, 100f, 100f, 250f, 0f, 3400f, 72f, -7.4f);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000028DC File Offset: 0x00000ADC
		public static REVERB_PROPERTIES CARPETTEDHALLWAY()
		{
			return new REVERB_PROPERTIES(300f, 2f, 30f, 5000f, 10f, 100f, 100f, 250f, 0f, 500f, 56f, -24f);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000292C File Offset: 0x00000B2C
		public static REVERB_PROPERTIES HALLWAY()
		{
			return new REVERB_PROPERTIES(1500f, 7f, 11f, 5000f, 59f, 100f, 100f, 250f, 0f, 7800f, 87f, -5.5f);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000297C File Offset: 0x00000B7C
		public static REVERB_PROPERTIES STONECORRIDOR()
		{
			return new REVERB_PROPERTIES(270f, 13f, 20f, 5000f, 79f, 100f, 100f, 250f, 0f, 9000f, 86f, -6f);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000029CC File Offset: 0x00000BCC
		public static REVERB_PROPERTIES ALLEY()
		{
			return new REVERB_PROPERTIES(1500f, 7f, 11f, 5000f, 86f, 100f, 100f, 250f, 0f, 8300f, 80f, -9.8f);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002A1C File Offset: 0x00000C1C
		public static REVERB_PROPERTIES FOREST()
		{
			return new REVERB_PROPERTIES(1500f, 162f, 88f, 5000f, 54f, 79f, 100f, 250f, 0f, 760f, 94f, -12.3f);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002A6C File Offset: 0x00000C6C
		public static REVERB_PROPERTIES CITY()
		{
			return new REVERB_PROPERTIES(1500f, 7f, 11f, 5000f, 67f, 50f, 100f, 250f, 0f, 4050f, 66f, -26f);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002ABC File Offset: 0x00000CBC
		public static REVERB_PROPERTIES MOUNTAINS()
		{
			return new REVERB_PROPERTIES(1500f, 300f, 100f, 5000f, 21f, 27f, 100f, 250f, 0f, 1220f, 82f, -24f);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002B0C File Offset: 0x00000D0C
		public static REVERB_PROPERTIES QUARRY()
		{
			return new REVERB_PROPERTIES(1500f, 61f, 25f, 5000f, 83f, 100f, 100f, 250f, 0f, 3400f, 100f, -5f);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002B5C File Offset: 0x00000D5C
		public static REVERB_PROPERTIES PLAIN()
		{
			return new REVERB_PROPERTIES(1500f, 179f, 100f, 5000f, 50f, 21f, 100f, 250f, 0f, 1670f, 65f, -28f);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002BAC File Offset: 0x00000DAC
		public static REVERB_PROPERTIES PARKINGLOT()
		{
			return new REVERB_PROPERTIES(1700f, 8f, 12f, 5000f, 100f, 100f, 100f, 250f, 0f, 20000f, 56f, -19.5f);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002BFC File Offset: 0x00000DFC
		public static REVERB_PROPERTIES SEWERPIPE()
		{
			return new REVERB_PROPERTIES(2800f, 14f, 21f, 5000f, 14f, 80f, 60f, 250f, 0f, 3400f, 66f, 1.2f);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002C4C File Offset: 0x00000E4C
		public static REVERB_PROPERTIES UNDERWATER()
		{
			return new REVERB_PROPERTIES(1500f, 7f, 11f, 5000f, 10f, 100f, 100f, 250f, 0f, 500f, 92f, 7f);
		}
	}
}
