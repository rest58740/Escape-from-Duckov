using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000093 RID: 147
	public struct DSP_DESCRIPTION
	{
		// Token: 0x040002E0 RID: 736
		public uint pluginsdkversion;

		// Token: 0x040002E1 RID: 737
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] name;

		// Token: 0x040002E2 RID: 738
		public uint version;

		// Token: 0x040002E3 RID: 739
		public int numinputbuffers;

		// Token: 0x040002E4 RID: 740
		public int numoutputbuffers;

		// Token: 0x040002E5 RID: 741
		public DSP_CREATE_CALLBACK create;

		// Token: 0x040002E6 RID: 742
		public DSP_RELEASE_CALLBACK release;

		// Token: 0x040002E7 RID: 743
		public DSP_RESET_CALLBACK reset;

		// Token: 0x040002E8 RID: 744
		public DSP_READ_CALLBACK read;

		// Token: 0x040002E9 RID: 745
		public DSP_PROCESS_CALLBACK process;

		// Token: 0x040002EA RID: 746
		public DSP_SETPOSITION_CALLBACK setposition;

		// Token: 0x040002EB RID: 747
		public int numparameters;

		// Token: 0x040002EC RID: 748
		public IntPtr paramdesc;

		// Token: 0x040002ED RID: 749
		public DSP_SETPARAM_FLOAT_CALLBACK setparameterfloat;

		// Token: 0x040002EE RID: 750
		public DSP_SETPARAM_INT_CALLBACK setparameterint;

		// Token: 0x040002EF RID: 751
		public DSP_SETPARAM_BOOL_CALLBACK setparameterbool;

		// Token: 0x040002F0 RID: 752
		public DSP_SETPARAM_DATA_CALLBACK setparameterdata;

		// Token: 0x040002F1 RID: 753
		public DSP_GETPARAM_FLOAT_CALLBACK getparameterfloat;

		// Token: 0x040002F2 RID: 754
		public DSP_GETPARAM_INT_CALLBACK getparameterint;

		// Token: 0x040002F3 RID: 755
		public DSP_GETPARAM_BOOL_CALLBACK getparameterbool;

		// Token: 0x040002F4 RID: 756
		public DSP_GETPARAM_DATA_CALLBACK getparameterdata;

		// Token: 0x040002F5 RID: 757
		public DSP_SHOULDIPROCESS_CALLBACK shouldiprocess;

		// Token: 0x040002F6 RID: 758
		public IntPtr userdata;

		// Token: 0x040002F7 RID: 759
		public DSP_SYSTEM_REGISTER_CALLBACK sys_register;

		// Token: 0x040002F8 RID: 760
		public DSP_SYSTEM_DEREGISTER_CALLBACK sys_deregister;

		// Token: 0x040002F9 RID: 761
		public DSP_SYSTEM_MIX_CALLBACK sys_mix;
	}
}
