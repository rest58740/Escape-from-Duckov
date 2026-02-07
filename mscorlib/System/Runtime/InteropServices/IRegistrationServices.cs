using System;
using System.Reflection;
using System.Security;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200071A RID: 1818
	[Guid("CCBD682C-73A5-4568-B8B0-C7007E11ABA2")]
	[ComVisible(true)]
	public interface IRegistrationServices
	{
		// Token: 0x060040D8 RID: 16600
		[SecurityCritical]
		bool RegisterAssembly(Assembly assembly, AssemblyRegistrationFlags flags);

		// Token: 0x060040D9 RID: 16601
		[SecurityCritical]
		bool UnregisterAssembly(Assembly assembly);

		// Token: 0x060040DA RID: 16602
		[SecurityCritical]
		Type[] GetRegistrableTypesInAssembly(Assembly assembly);

		// Token: 0x060040DB RID: 16603
		[SecurityCritical]
		string GetProgIdForType(Type type);

		// Token: 0x060040DC RID: 16604
		[SecurityCritical]
		void RegisterTypeForComClients(Type type, ref Guid g);

		// Token: 0x060040DD RID: 16605
		Guid GetManagedCategoryGuid();

		// Token: 0x060040DE RID: 16606
		[SecurityCritical]
		bool TypeRequiresRegistration(Type type);

		// Token: 0x060040DF RID: 16607
		bool TypeRepresentsComType(Type type);
	}
}
