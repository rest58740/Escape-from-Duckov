using System;
using System.Reflection;
using System.Security;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200074E RID: 1870
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("475e398f-8afa-43a7-a3be-f4ef8d6787c9")]
	public class RegistrationServices : IRegistrationServices
	{
		// Token: 0x06004239 RID: 16953 RVA: 0x000E3A9E File Offset: 0x000E1C9E
		public virtual Guid GetManagedCategoryGuid()
		{
			return RegistrationServices.guidManagedCategory;
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x000E3AA5 File Offset: 0x000E1CA5
		[SecurityCritical]
		public virtual string GetProgIdForType(Type type)
		{
			return Marshal.GenerateProgIdForType(type);
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("implement")]
		[SecurityCritical]
		public virtual Type[] GetRegistrableTypesInAssembly(Assembly assembly)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecurityCritical]
		[MonoTODO("implement")]
		public virtual bool RegisterAssembly(Assembly assembly, AssemblyRegistrationFlags flags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecurityCritical]
		[MonoTODO("implement")]
		public virtual void RegisterTypeForComClients(Type type, ref Guid g)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("implement")]
		[SecuritySafeCritical]
		public virtual bool TypeRepresentsComType(Type type)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("implement")]
		[SecurityCritical]
		public virtual bool TypeRequiresRegistration(Type type)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("implement")]
		[SecurityCritical]
		public virtual bool UnregisterAssembly(Assembly assembly)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x000479FC File Offset: 0x00045BFC
		[ComVisible(false)]
		[MonoTODO("implement")]
		public virtual int RegisterTypeForComClients(Type type, RegistrationClassContext classContext, RegistrationConnectionType flags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x000479FC File Offset: 0x00045BFC
		[ComVisible(false)]
		[MonoTODO("implement")]
		public virtual void UnregisterTypeForComClients(int cookie)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002BF1 RID: 11249
		private static Guid guidManagedCategory = new Guid("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
	}
}
