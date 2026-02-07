using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006F4 RID: 1780
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class ComSourceInterfacesAttribute : Attribute
	{
		// Token: 0x0600407B RID: 16507 RVA: 0x000E104E File Offset: 0x000DF24E
		public ComSourceInterfacesAttribute(string sourceInterfaces)
		{
			this._val = sourceInterfaces;
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x000E105D File Offset: 0x000DF25D
		public ComSourceInterfacesAttribute(Type sourceInterface)
		{
			this._val = sourceInterface.FullName;
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x000E1071 File Offset: 0x000DF271
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2)
		{
			this._val = sourceInterface1.FullName + "\0" + sourceInterface2.FullName;
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x000E1098 File Offset: 0x000DF298
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3)
		{
			this._val = string.Concat(new string[]
			{
				sourceInterface1.FullName,
				"\0",
				sourceInterface2.FullName,
				"\0",
				sourceInterface3.FullName
			});
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x000E10E8 File Offset: 0x000DF2E8
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3, Type sourceInterface4)
		{
			this._val = string.Concat(new string[]
			{
				sourceInterface1.FullName,
				"\0",
				sourceInterface2.FullName,
				"\0",
				sourceInterface3.FullName,
				"\0",
				sourceInterface4.FullName
			});
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06004080 RID: 16512 RVA: 0x000E1149 File Offset: 0x000DF349
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A44 RID: 10820
		internal string _val;
	}
}
