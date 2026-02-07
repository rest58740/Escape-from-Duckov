using System;

namespace FMOD
{
	// Token: 0x02000012 RID: 18
	[Flags]
	public enum CHANNELMASK : uint
	{
		// Token: 0x040000C9 RID: 201
		FRONT_LEFT = 1U,
		// Token: 0x040000CA RID: 202
		FRONT_RIGHT = 2U,
		// Token: 0x040000CB RID: 203
		FRONT_CENTER = 4U,
		// Token: 0x040000CC RID: 204
		LOW_FREQUENCY = 8U,
		// Token: 0x040000CD RID: 205
		SURROUND_LEFT = 16U,
		// Token: 0x040000CE RID: 206
		SURROUND_RIGHT = 32U,
		// Token: 0x040000CF RID: 207
		BACK_LEFT = 64U,
		// Token: 0x040000D0 RID: 208
		BACK_RIGHT = 128U,
		// Token: 0x040000D1 RID: 209
		BACK_CENTER = 256U,
		// Token: 0x040000D2 RID: 210
		MONO = 1U,
		// Token: 0x040000D3 RID: 211
		STEREO = 3U,
		// Token: 0x040000D4 RID: 212
		LRC = 7U,
		// Token: 0x040000D5 RID: 213
		QUAD = 51U,
		// Token: 0x040000D6 RID: 214
		SURROUND = 55U,
		// Token: 0x040000D7 RID: 215
		_5POINT1 = 63U,
		// Token: 0x040000D8 RID: 216
		_5POINT1_REARS = 207U,
		// Token: 0x040000D9 RID: 217
		_7POINT0 = 247U,
		// Token: 0x040000DA RID: 218
		_7POINT1 = 255U
	}
}
