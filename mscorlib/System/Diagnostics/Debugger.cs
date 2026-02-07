using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020009C0 RID: 2496
	[ComVisible(true)]
	public sealed class Debugger
	{
		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x060059C9 RID: 22985 RVA: 0x001332B1 File Offset: 0x001314B1
		public static bool IsAttached
		{
			get
			{
				return Debugger.IsAttached_internal();
			}
		}

		// Token: 0x060059CA RID: 22986
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsAttached_internal();

		// Token: 0x060059CB RID: 22987 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void Break()
		{
		}

		// Token: 0x060059CC RID: 22988
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsLogging();

		// Token: 0x060059CD RID: 22989 RVA: 0x000479FC File Offset: 0x00045BFC
		public static bool Launch()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060059CE RID: 22990
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Log_icall(int level, ref string category, ref string message);

		// Token: 0x060059CF RID: 22991 RVA: 0x001332B8 File Offset: 0x001314B8
		public static void Log(int level, string category, string message)
		{
			Debugger.Log_icall(level, ref category, ref message);
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void NotifyOfCrossThreadDependency()
		{
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x0000259F File Offset: 0x0000079F
		[Obsolete("Call the static methods directly on this type", true)]
		public Debugger()
		{
		}

		// Token: 0x0400378D RID: 14221
		public static readonly string DefaultCategory = "";
	}
}
