using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200008E RID: 142
	public struct DSP_PARAMETER_FFT
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00004EA8 File Offset: 0x000030A8
		public float[][] spectrum
		{
			get
			{
				float[][] array = new float[this.numchannels][];
				for (int i = 0; i < this.numchannels; i++)
				{
					array[i] = new float[this.length];
					Marshal.Copy(this.spectrum_internal[i], array[i], 0, this.length);
				}
				return array;
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00004EF8 File Offset: 0x000030F8
		public void getSpectrum(ref float[][] buffer)
		{
			int num = Math.Min(buffer.Length, this.numchannels);
			for (int i = 0; i < num; i++)
			{
				this.getSpectrum(i, ref buffer[i]);
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00004F30 File Offset: 0x00003130
		public void getSpectrum(int channel, ref float[] buffer)
		{
			int num = Math.Min(buffer.Length, this.length);
			Marshal.Copy(this.spectrum_internal[channel], buffer, 0, num);
		}

		// Token: 0x040002D0 RID: 720
		public int length;

		// Token: 0x040002D1 RID: 721
		public int numchannels;

		// Token: 0x040002D2 RID: 722
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private IntPtr[] spectrum_internal;
	}
}
