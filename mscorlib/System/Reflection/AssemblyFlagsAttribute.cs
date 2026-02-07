using System;

namespace System.Reflection
{
	// Token: 0x02000887 RID: 2183
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyFlagsAttribute : Attribute
	{
		// Token: 0x06004861 RID: 18529 RVA: 0x000EE0C2 File Offset: 0x000EC2C2
		[Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[CLSCompliant(false)]
		public AssemblyFlagsAttribute(uint flags)
		{
			this._flags = (AssemblyNameFlags)flags;
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06004862 RID: 18530 RVA: 0x000EE0D1 File Offset: 0x000EC2D1
		[CLSCompliant(false)]
		[Obsolete("This property has been deprecated. Please use AssemblyFlags instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public uint Flags
		{
			get
			{
				return (uint)this._flags;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06004863 RID: 18531 RVA: 0x000EE0D1 File Offset: 0x000EC2D1
		public int AssemblyFlags
		{
			get
			{
				return (int)this._flags;
			}
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x000EE0C2 File Offset: 0x000EC2C2
		[Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyFlagsAttribute(int assemblyFlags)
		{
			this._flags = (AssemblyNameFlags)assemblyFlags;
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x000EE0C2 File Offset: 0x000EC2C2
		public AssemblyFlagsAttribute(AssemblyNameFlags assemblyFlags)
		{
			this._flags = assemblyFlags;
		}

		// Token: 0x04002E55 RID: 11861
		private AssemblyNameFlags _flags;
	}
}
