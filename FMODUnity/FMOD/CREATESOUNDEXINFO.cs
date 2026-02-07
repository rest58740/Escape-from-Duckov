using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200003D RID: 61
	public struct CREATESOUNDEXINFO
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000022C5 File Offset: 0x000004C5
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000022AD File Offset: 0x000004AD
		public SOUND_PCMREAD_CALLBACK pcmreadcallback
		{
			get
			{
				if (!(this.pcmreadcallback_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<SOUND_PCMREAD_CALLBACK>(this.pcmreadcallback_internal);
				}
				return null;
			}
			set
			{
				this.pcmreadcallback_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<SOUND_PCMREAD_CALLBACK>(value));
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000022FE File Offset: 0x000004FE
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000022E6 File Offset: 0x000004E6
		public SOUND_PCMSETPOS_CALLBACK pcmsetposcallback
		{
			get
			{
				if (!(this.pcmsetposcallback_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<SOUND_PCMSETPOS_CALLBACK>(this.pcmsetposcallback_internal);
				}
				return null;
			}
			set
			{
				this.pcmsetposcallback_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<SOUND_PCMSETPOS_CALLBACK>(value));
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002337 File Offset: 0x00000537
		// (set) Token: 0x0600005B RID: 91 RVA: 0x0000231F File Offset: 0x0000051F
		public SOUND_NONBLOCK_CALLBACK nonblockcallback
		{
			get
			{
				if (!(this.nonblockcallback_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<SOUND_NONBLOCK_CALLBACK>(this.nonblockcallback_internal);
				}
				return null;
			}
			set
			{
				this.nonblockcallback_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<SOUND_NONBLOCK_CALLBACK>(value));
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002370 File Offset: 0x00000570
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002358 File Offset: 0x00000558
		public FILE_OPEN_CALLBACK fileuseropen
		{
			get
			{
				if (!(this.fileuseropen_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<FILE_OPEN_CALLBACK>(this.fileuseropen_internal);
				}
				return null;
			}
			set
			{
				this.fileuseropen_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_OPEN_CALLBACK>(value));
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000023A9 File Offset: 0x000005A9
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002391 File Offset: 0x00000591
		public FILE_CLOSE_CALLBACK fileuserclose
		{
			get
			{
				if (!(this.fileuserclose_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<FILE_CLOSE_CALLBACK>(this.fileuserclose_internal);
				}
				return null;
			}
			set
			{
				this.fileuserclose_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_CLOSE_CALLBACK>(value));
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000023E2 File Offset: 0x000005E2
		// (set) Token: 0x06000061 RID: 97 RVA: 0x000023CA File Offset: 0x000005CA
		public FILE_READ_CALLBACK fileuserread
		{
			get
			{
				if (!(this.fileuserread_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<FILE_READ_CALLBACK>(this.fileuserread_internal);
				}
				return null;
			}
			set
			{
				this.fileuserread_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_READ_CALLBACK>(value));
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000241B File Offset: 0x0000061B
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002403 File Offset: 0x00000603
		public FILE_SEEK_CALLBACK fileuserseek
		{
			get
			{
				if (!(this.fileuserseek_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<FILE_SEEK_CALLBACK>(this.fileuserseek_internal);
				}
				return null;
			}
			set
			{
				this.fileuserseek_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_SEEK_CALLBACK>(value));
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002454 File Offset: 0x00000654
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000243C File Offset: 0x0000063C
		public FILE_ASYNCREAD_CALLBACK fileuserasyncread
		{
			get
			{
				if (!(this.fileuserasyncread_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<FILE_ASYNCREAD_CALLBACK>(this.fileuserasyncread_internal);
				}
				return null;
			}
			set
			{
				this.fileuserasyncread_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_ASYNCREAD_CALLBACK>(value));
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000248D File Offset: 0x0000068D
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002475 File Offset: 0x00000675
		public FILE_ASYNCCANCEL_CALLBACK fileuserasynccancel
		{
			get
			{
				if (!(this.fileuserasynccancel_internal == IntPtr.Zero))
				{
					return Marshal.GetDelegateForFunctionPointer<FILE_ASYNCCANCEL_CALLBACK>(this.fileuserasynccancel_internal);
				}
				return null;
			}
			set
			{
				this.fileuserasynccancel_internal = ((value == null) ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_ASYNCCANCEL_CALLBACK>(value));
			}
		}

		// Token: 0x040001BD RID: 445
		public int cbsize;

		// Token: 0x040001BE RID: 446
		public uint length;

		// Token: 0x040001BF RID: 447
		public uint fileoffset;

		// Token: 0x040001C0 RID: 448
		public int numchannels;

		// Token: 0x040001C1 RID: 449
		public int defaultfrequency;

		// Token: 0x040001C2 RID: 450
		public SOUND_FORMAT format;

		// Token: 0x040001C3 RID: 451
		public uint decodebuffersize;

		// Token: 0x040001C4 RID: 452
		public int initialsubsound;

		// Token: 0x040001C5 RID: 453
		public int numsubsounds;

		// Token: 0x040001C6 RID: 454
		public IntPtr inclusionlist;

		// Token: 0x040001C7 RID: 455
		public int inclusionlistnum;

		// Token: 0x040001C8 RID: 456
		public IntPtr pcmreadcallback_internal;

		// Token: 0x040001C9 RID: 457
		public IntPtr pcmsetposcallback_internal;

		// Token: 0x040001CA RID: 458
		public IntPtr nonblockcallback_internal;

		// Token: 0x040001CB RID: 459
		public IntPtr dlsname;

		// Token: 0x040001CC RID: 460
		public IntPtr encryptionkey;

		// Token: 0x040001CD RID: 461
		public int maxpolyphony;

		// Token: 0x040001CE RID: 462
		public IntPtr userdata;

		// Token: 0x040001CF RID: 463
		public SOUND_TYPE suggestedsoundtype;

		// Token: 0x040001D0 RID: 464
		public IntPtr fileuseropen_internal;

		// Token: 0x040001D1 RID: 465
		public IntPtr fileuserclose_internal;

		// Token: 0x040001D2 RID: 466
		public IntPtr fileuserread_internal;

		// Token: 0x040001D3 RID: 467
		public IntPtr fileuserseek_internal;

		// Token: 0x040001D4 RID: 468
		public IntPtr fileuserasyncread_internal;

		// Token: 0x040001D5 RID: 469
		public IntPtr fileuserasynccancel_internal;

		// Token: 0x040001D6 RID: 470
		public IntPtr fileuserdata;

		// Token: 0x040001D7 RID: 471
		public int filebuffersize;

		// Token: 0x040001D8 RID: 472
		public CHANNELORDER channelorder;

		// Token: 0x040001D9 RID: 473
		public IntPtr initialsoundgroup;

		// Token: 0x040001DA RID: 474
		public uint initialseekposition;

		// Token: 0x040001DB RID: 475
		public TIMEUNIT initialseekpostype;

		// Token: 0x040001DC RID: 476
		public int ignoresetfilesystem;

		// Token: 0x040001DD RID: 477
		public uint audioqueuepolicy;

		// Token: 0x040001DE RID: 478
		public uint minmidigranularity;

		// Token: 0x040001DF RID: 479
		public int nonblockthreadid;

		// Token: 0x040001E0 RID: 480
		public IntPtr fsbguid;
	}
}
