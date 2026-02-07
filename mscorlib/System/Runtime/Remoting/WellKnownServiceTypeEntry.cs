using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting
{
	// Token: 0x0200057A RID: 1402
	[ComVisible(true)]
	public class WellKnownServiceTypeEntry : TypeEntry
	{
		// Token: 0x060036FF RID: 14079 RVA: 0x000C6985 File Offset: 0x000C4B85
		public WellKnownServiceTypeEntry(Type type, string objectUri, WellKnownObjectMode mode)
		{
			base.AssemblyName = type.Assembly.FullName;
			base.TypeName = type.FullName;
			this.obj_type = type;
			this.obj_uri = objectUri;
			this.obj_mode = mode;
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x000C69C0 File Offset: 0x000C4BC0
		public WellKnownServiceTypeEntry(string typeName, string assemblyName, string objectUri, WellKnownObjectMode mode)
		{
			base.AssemblyName = assemblyName;
			base.TypeName = typeName;
			Assembly assembly = Assembly.Load(assemblyName);
			this.obj_type = assembly.GetType(typeName);
			this.obj_uri = objectUri;
			this.obj_mode = mode;
			if (this.obj_type == null)
			{
				throw new RemotingException("Type not found: " + typeName + ", " + assemblyName);
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06003701 RID: 14081 RVA: 0x0000AF5E File Offset: 0x0000915E
		// (set) Token: 0x06003702 RID: 14082 RVA: 0x00004BF9 File Offset: 0x00002DF9
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

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06003703 RID: 14083 RVA: 0x000C6A29 File Offset: 0x000C4C29
		public WellKnownObjectMode Mode
		{
			get
			{
				return this.obj_mode;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06003704 RID: 14084 RVA: 0x000C6A31 File Offset: 0x000C4C31
		public Type ObjectType
		{
			get
			{
				return this.obj_type;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06003705 RID: 14085 RVA: 0x000C6A39 File Offset: 0x000C4C39
		public string ObjectUri
		{
			get
			{
				return this.obj_uri;
			}
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x000C6A41 File Offset: 0x000C4C41
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.TypeName,
				", ",
				base.AssemblyName,
				" ",
				this.ObjectUri
			});
		}

		// Token: 0x04002573 RID: 9587
		private Type obj_type;

		// Token: 0x04002574 RID: 9588
		private string obj_uri;

		// Token: 0x04002575 RID: 9589
		private WellKnownObjectMode obj_mode;
	}
}
