using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x020003E1 RID: 993
	[ComVisible(true)]
	[Serializable]
	public sealed class NamedPermissionSet : PermissionSet
	{
		// Token: 0x060028B6 RID: 10422 RVA: 0x0009340A File Offset: 0x0009160A
		internal NamedPermissionSet()
		{
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x00093412 File Offset: 0x00091612
		public NamedPermissionSet(string name, PermissionSet permSet) : base(permSet)
		{
			this.Name = name;
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x00093422 File Offset: 0x00091622
		public NamedPermissionSet(string name, PermissionState state) : base(state)
		{
			this.Name = name;
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x00093432 File Offset: 0x00091632
		public NamedPermissionSet(NamedPermissionSet permSet) : base(permSet)
		{
			this.name = permSet.name;
			this.description = permSet.description;
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x00093453 File Offset: 0x00091653
		public NamedPermissionSet(string name) : this(name, PermissionState.Unrestricted)
		{
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060028BB RID: 10427 RVA: 0x0009345D File Offset: 0x0009165D
		// (set) Token: 0x060028BC RID: 10428 RVA: 0x00093465 File Offset: 0x00091665
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x0009346E File Offset: 0x0009166E
		// (set) Token: 0x060028BE RID: 10430 RVA: 0x00093476 File Offset: 0x00091676
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					throw new ArgumentException(Locale.GetText("invalid name"));
				}
				this.name = value;
			}
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x0009349F File Offset: 0x0009169F
		public override PermissionSet Copy()
		{
			return new NamedPermissionSet(this);
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000934A7 File Offset: 0x000916A7
		public NamedPermissionSet Copy(string name)
		{
			return new NamedPermissionSet(this)
			{
				Name = name
			};
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000934B6 File Offset: 0x000916B6
		public override void FromXml(SecurityElement et)
		{
			base.FromXml(et);
			this.name = et.Attribute("Name");
			this.description = et.Attribute("Description");
			if (this.description == null)
			{
				this.description = string.Empty;
			}
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x000934F4 File Offset: 0x000916F4
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.ToXml();
			if (this.name != null)
			{
				securityElement.AddAttribute("Name", this.name);
			}
			if (this.description != null)
			{
				securityElement.AddAttribute("Description", this.description);
			}
			return securityElement;
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x0009353C File Offset: 0x0009173C
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			NamedPermissionSet namedPermissionSet = obj as NamedPermissionSet;
			return namedPermissionSet != null && this.name == namedPermissionSet.Name && base.Equals(obj);
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x00093578 File Offset: 0x00091778
		[ComVisible(false)]
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.name != null)
			{
				num ^= this.name.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001EBF RID: 7871
		private string name;

		// Token: 0x04001EC0 RID: 7872
		private string description;
	}
}
