using System;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000933 RID: 2355
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_LocalBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class LocalBuilder : LocalVariableInfo, _LocalBuilder
	{
		// Token: 0x060050F2 RID: 20722 RVA: 0x000479FC File Offset: 0x00045BFC
		void _LocalBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x000479FC File Offset: 0x00045BFC
		void _LocalBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x000479FC File Offset: 0x00045BFC
		void _LocalBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x000479FC File Offset: 0x00045BFC
		void _LocalBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x000FD7D8 File Offset: 0x000FB9D8
		internal LocalBuilder(Type t, ILGenerator ilgen)
		{
			this.type = t;
			this.ilgen = ilgen;
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x000FD7EE File Offset: 0x000FB9EE
		public void SetLocalSymInfo(string name, int startOffset, int endOffset)
		{
			this.name = name;
			this.startOffset = startOffset;
			this.endOffset = endOffset;
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x000FD805 File Offset: 0x000FBA05
		public void SetLocalSymInfo(string name)
		{
			this.SetLocalSymInfo(name, 0, 0);
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x060050F9 RID: 20729 RVA: 0x000F310D File Offset: 0x000F130D
		public override Type LocalType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x060050FA RID: 20730 RVA: 0x000F30FD File Offset: 0x000F12FD
		public override bool IsPinned
		{
			get
			{
				return this.is_pinned;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x060050FB RID: 20731 RVA: 0x000F3105 File Offset: 0x000F1305
		public override int LocalIndex
		{
			get
			{
				return (int)this.position;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x060050FC RID: 20732 RVA: 0x000FD810 File Offset: 0x000FBA10
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x060050FD RID: 20733 RVA: 0x000FD818 File Offset: 0x000FBA18
		internal int StartOffset
		{
			get
			{
				return this.startOffset;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x060050FE RID: 20734 RVA: 0x000FD820 File Offset: 0x000FBA20
		internal int EndOffset
		{
			get
			{
				return this.endOffset;
			}
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x000173AD File Offset: 0x000155AD
		internal LocalBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040031B3 RID: 12723
		private string name;

		// Token: 0x040031B4 RID: 12724
		internal ILGenerator ilgen;

		// Token: 0x040031B5 RID: 12725
		private int startOffset;

		// Token: 0x040031B6 RID: 12726
		private int endOffset;
	}
}
