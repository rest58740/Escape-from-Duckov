using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200053D RID: 1341
	public abstract class ObjectAccessRule : AccessRule
	{
		// Token: 0x060034EF RID: 13551 RVA: 0x000C006D File Offset: 0x000BE26D
		protected ObjectAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AccessControlType type) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
			this.object_type = objectType;
			this.inherited_object_type = inheritedObjectType;
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060034F0 RID: 13552 RVA: 0x000C008E File Offset: 0x000BE28E
		public Guid InheritedObjectType
		{
			get
			{
				return this.inherited_object_type;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060034F1 RID: 13553 RVA: 0x000C0098 File Offset: 0x000BE298
		public ObjectAceFlags ObjectFlags
		{
			get
			{
				ObjectAceFlags objectAceFlags = ObjectAceFlags.None;
				if (this.object_type != Guid.Empty)
				{
					objectAceFlags |= ObjectAceFlags.ObjectAceTypePresent;
				}
				if (this.inherited_object_type != Guid.Empty)
				{
					objectAceFlags |= ObjectAceFlags.InheritedObjectAceTypePresent;
				}
				return objectAceFlags;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060034F2 RID: 13554 RVA: 0x000C00D4 File Offset: 0x000BE2D4
		public Guid ObjectType
		{
			get
			{
				return this.object_type;
			}
		}

		// Token: 0x040024C2 RID: 9410
		private Guid object_type;

		// Token: 0x040024C3 RID: 9411
		private Guid inherited_object_type;
	}
}
