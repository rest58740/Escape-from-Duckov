using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting
{
	// Token: 0x02000559 RID: 1369
	[ComVisible(true)]
	public class ActivatedServiceTypeEntry : TypeEntry
	{
		// Token: 0x060035D7 RID: 13783 RVA: 0x000C230E File Offset: 0x000C050E
		public ActivatedServiceTypeEntry(Type type)
		{
			base.AssemblyName = type.Assembly.FullName;
			base.TypeName = type.FullName;
			this.obj_type = type;
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x000C233C File Offset: 0x000C053C
		public ActivatedServiceTypeEntry(string typeName, string assemblyName)
		{
			base.AssemblyName = assemblyName;
			base.TypeName = typeName;
			Assembly assembly = Assembly.Load(assemblyName);
			this.obj_type = assembly.GetType(typeName);
			if (this.obj_type == null)
			{
				throw new RemotingException("Type not found: " + typeName + ", " + assemblyName);
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x060035D9 RID: 13785 RVA: 0x0000AF5E File Offset: 0x0000915E
		// (set) Token: 0x060035DA RID: 13786 RVA: 0x00004BF9 File Offset: 0x00002DF9
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

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x060035DB RID: 13787 RVA: 0x000C2396 File Offset: 0x000C0596
		public Type ObjectType
		{
			get
			{
				return this.obj_type;
			}
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x000C239E File Offset: 0x000C059E
		public override string ToString()
		{
			return base.AssemblyName + base.TypeName;
		}

		// Token: 0x04002515 RID: 9493
		private Type obj_type;
	}
}
