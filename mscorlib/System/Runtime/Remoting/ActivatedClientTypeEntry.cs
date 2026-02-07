using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting
{
	// Token: 0x02000558 RID: 1368
	[ComVisible(true)]
	public class ActivatedClientTypeEntry : TypeEntry
	{
		// Token: 0x060035D0 RID: 13776 RVA: 0x000C2251 File Offset: 0x000C0451
		public ActivatedClientTypeEntry(Type type, string appUrl)
		{
			base.AssemblyName = type.Assembly.FullName;
			base.TypeName = type.FullName;
			this.applicationUrl = appUrl;
			this.obj_type = type;
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x000C2284 File Offset: 0x000C0484
		public ActivatedClientTypeEntry(string typeName, string assemblyName, string appUrl)
		{
			base.AssemblyName = assemblyName;
			base.TypeName = typeName;
			this.applicationUrl = appUrl;
			Assembly assembly = Assembly.Load(assemblyName);
			this.obj_type = assembly.GetType(typeName);
			if (this.obj_type == null)
			{
				throw new RemotingException("Type not found: " + typeName + ", " + assemblyName);
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060035D2 RID: 13778 RVA: 0x000C22E5 File Offset: 0x000C04E5
		public string ApplicationUrl
		{
			get
			{
				return this.applicationUrl;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060035D3 RID: 13779 RVA: 0x0000AF5E File Offset: 0x0000915E
		// (set) Token: 0x060035D4 RID: 13780 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public IContextAttribute[] ContextAttributes
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x000C22ED File Offset: 0x000C04ED
		public Type ObjectType
		{
			get
			{
				return this.obj_type;
			}
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x000C22F5 File Offset: 0x000C04F5
		public override string ToString()
		{
			return base.TypeName + base.AssemblyName + this.ApplicationUrl;
		}

		// Token: 0x04002513 RID: 9491
		private string applicationUrl;

		// Token: 0x04002514 RID: 9492
		private Type obj_type;
	}
}
