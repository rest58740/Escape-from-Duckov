using System;

namespace System.Reflection
{
	// Token: 0x020008DB RID: 2267
	public class AssemblyNameProxy : MarshalByRefObject
	{
		// Token: 0x06004B82 RID: 19330 RVA: 0x000F08F1 File Offset: 0x000EEAF1
		public AssemblyName GetAssemblyName(string assemblyFile)
		{
			return AssemblyName.GetAssemblyName(assemblyFile);
		}
	}
}
