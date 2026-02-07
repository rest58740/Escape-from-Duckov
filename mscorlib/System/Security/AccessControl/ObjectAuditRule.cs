using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000540 RID: 1344
	public abstract class ObjectAuditRule : AuditRule
	{
		// Token: 0x06003505 RID: 13573 RVA: 0x000C052E File Offset: 0x000BE72E
		protected ObjectAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AuditFlags auditFlags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, auditFlags)
		{
			this.object_type = objectType;
			this.inherited_object_type = inheritedObjectType;
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06003506 RID: 13574 RVA: 0x000C054F File Offset: 0x000BE74F
		public Guid InheritedObjectType
		{
			get
			{
				return this.inherited_object_type;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06003507 RID: 13575 RVA: 0x000C0558 File Offset: 0x000BE758
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

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06003508 RID: 13576 RVA: 0x000C0594 File Offset: 0x000BE794
		public Guid ObjectType
		{
			get
			{
				return this.object_type;
			}
		}

		// Token: 0x040024CB RID: 9419
		private Guid inherited_object_type;

		// Token: 0x040024CC RID: 9420
		private Guid object_type;
	}
}
