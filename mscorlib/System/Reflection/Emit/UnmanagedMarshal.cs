using System;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x0200094A RID: 2378
	[ComVisible(true)]
	[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead.")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class UnmanagedMarshal
	{
		// Token: 0x0600535B RID: 21339 RVA: 0x00105C92 File Offset: 0x00103E92
		private UnmanagedMarshal(UnmanagedType maint, int cnt)
		{
			this.count = cnt;
			this.t = maint;
			this.tbase = maint;
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x00105CAF File Offset: 0x00103EAF
		private UnmanagedMarshal(UnmanagedType maint, UnmanagedType elemt)
		{
			this.count = 0;
			this.t = maint;
			this.tbase = elemt;
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x0600535D RID: 21341 RVA: 0x00105CCC File Offset: 0x00103ECC
		public UnmanagedType BaseType
		{
			get
			{
				if (this.t == UnmanagedType.LPArray)
				{
					throw new ArgumentException();
				}
				if (this.t == UnmanagedType.SafeArray)
				{
					throw new ArgumentException();
				}
				return this.tbase;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x0600535E RID: 21342 RVA: 0x00105CF4 File Offset: 0x00103EF4
		public int ElementCount
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x0600535F RID: 21343 RVA: 0x00105CFC File Offset: 0x00103EFC
		public UnmanagedType GetUnmanagedType
		{
			get
			{
				return this.t;
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06005360 RID: 21344 RVA: 0x00105D04 File Offset: 0x00103F04
		public Guid IIDGuid
		{
			get
			{
				return new Guid(this.guid);
			}
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x00105D11 File Offset: 0x00103F11
		public static UnmanagedMarshal DefineByValArray(int elemCount)
		{
			return new UnmanagedMarshal(UnmanagedType.ByValArray, elemCount);
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x00105D1B File Offset: 0x00103F1B
		public static UnmanagedMarshal DefineByValTStr(int elemCount)
		{
			return new UnmanagedMarshal(UnmanagedType.ByValTStr, elemCount);
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x00105D25 File Offset: 0x00103F25
		public static UnmanagedMarshal DefineLPArray(UnmanagedType elemType)
		{
			return new UnmanagedMarshal(UnmanagedType.LPArray, elemType);
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x00105D2F File Offset: 0x00103F2F
		public static UnmanagedMarshal DefineSafeArray(UnmanagedType elemType)
		{
			return new UnmanagedMarshal(UnmanagedType.SafeArray, elemType);
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x00105D39 File Offset: 0x00103F39
		public static UnmanagedMarshal DefineUnmanagedMarshal(UnmanagedType unmanagedType)
		{
			return new UnmanagedMarshal(unmanagedType, unmanagedType);
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x00105D44 File Offset: 0x00103F44
		internal static UnmanagedMarshal DefineCustom(Type typeref, string cookie, string mtype, Guid id)
		{
			UnmanagedMarshal unmanagedMarshal = new UnmanagedMarshal(UnmanagedType.CustomMarshaler, UnmanagedType.CustomMarshaler);
			unmanagedMarshal.mcookie = cookie;
			unmanagedMarshal.marshaltype = mtype;
			unmanagedMarshal.marshaltyperef = typeref;
			if (id == Guid.Empty)
			{
				unmanagedMarshal.guid = string.Empty;
			}
			else
			{
				unmanagedMarshal.guid = id.ToString();
			}
			return unmanagedMarshal;
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x00105D9E File Offset: 0x00103F9E
		internal static UnmanagedMarshal DefineLPArrayInternal(UnmanagedType elemType, int sizeConst, int sizeParamIndex)
		{
			return new UnmanagedMarshal(UnmanagedType.LPArray, elemType)
			{
				count = sizeConst,
				param_num = sizeParamIndex,
				has_size = true
			};
		}

		// Token: 0x06005368 RID: 21352 RVA: 0x000173AD File Offset: 0x000155AD
		internal UnmanagedMarshal()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400333F RID: 13119
		private int count;

		// Token: 0x04003340 RID: 13120
		private UnmanagedType t;

		// Token: 0x04003341 RID: 13121
		private UnmanagedType tbase;

		// Token: 0x04003342 RID: 13122
		private string guid;

		// Token: 0x04003343 RID: 13123
		private string mcookie;

		// Token: 0x04003344 RID: 13124
		private string marshaltype;

		// Token: 0x04003345 RID: 13125
		internal Type marshaltyperef;

		// Token: 0x04003346 RID: 13126
		private int param_num;

		// Token: 0x04003347 RID: 13127
		private bool has_size;
	}
}
