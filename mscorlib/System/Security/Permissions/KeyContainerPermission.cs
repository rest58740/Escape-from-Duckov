using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000445 RID: 1093
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
	{
		// Token: 0x06002C4A RID: 11338 RVA: 0x0009F5FE File Offset: 0x0009D7FE
		public KeyContainerPermission(PermissionState state)
		{
			if (CodeAccessPermission.CheckPermissionState(state, true) == PermissionState.Unrestricted)
			{
				this._flags = KeyContainerPermissionFlags.AllFlags;
			}
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x0009F61B File Offset: 0x0009D81B
		public KeyContainerPermission(KeyContainerPermissionFlags flags)
		{
			this.SetFlags(flags);
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x0009F62C File Offset: 0x0009D82C
		public KeyContainerPermission(KeyContainerPermissionFlags flags, KeyContainerPermissionAccessEntry[] accessList)
		{
			this.SetFlags(flags);
			if (accessList != null)
			{
				this._accessEntries = new KeyContainerPermissionAccessEntryCollection();
				foreach (KeyContainerPermissionAccessEntry accessEntry in accessList)
				{
					this._accessEntries.Add(accessEntry);
				}
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06002C4D RID: 11341 RVA: 0x0009F675 File Offset: 0x0009D875
		public KeyContainerPermissionAccessEntryCollection AccessEntries
		{
			get
			{
				return this._accessEntries;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002C4E RID: 11342 RVA: 0x0009F67D File Offset: 0x0009D87D
		public KeyContainerPermissionFlags Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x0009F688 File Offset: 0x0009D888
		public override IPermission Copy()
		{
			if (this._accessEntries.Count == 0)
			{
				return new KeyContainerPermission(this._flags);
			}
			KeyContainerPermissionAccessEntry[] array = new KeyContainerPermissionAccessEntry[this._accessEntries.Count];
			this._accessEntries.CopyTo(array, 0);
			return new KeyContainerPermission(this._flags, array);
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x0009F6D8 File Offset: 0x0009D8D8
		[MonoTODO("(2.0) missing support for AccessEntries")]
		public override void FromXml(SecurityElement securityElement)
		{
			CodeAccessPermission.CheckSecurityElement(securityElement, "securityElement", 1, 1);
			if (CodeAccessPermission.IsUnrestricted(securityElement))
			{
				this._flags = KeyContainerPermissionFlags.AllFlags;
				return;
			}
			this._flags = (KeyContainerPermissionFlags)Enum.Parse(typeof(KeyContainerPermissionFlags), securityElement.Attribute("Flags"));
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x0000AF5E File Offset: 0x0000915E
		[MonoTODO("(2.0)")]
		public override IPermission Intersect(IPermission target)
		{
			return null;
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("(2.0)")]
		public override bool IsSubsetOf(IPermission target)
		{
			return false;
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x0009F72C File Offset: 0x0009D92C
		public bool IsUnrestricted()
		{
			return this._flags == KeyContainerPermissionFlags.AllFlags;
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x0009F73C File Offset: 0x0009D93C
		[MonoTODO("(2.0) missing support for AccessEntries")]
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x0009F76C File Offset: 0x0009D96C
		public override IPermission Union(IPermission target)
		{
			KeyContainerPermission keyContainerPermission = this.Cast(target);
			if (keyContainerPermission == null)
			{
				return this.Copy();
			}
			KeyContainerPermissionAccessEntryCollection keyContainerPermissionAccessEntryCollection = new KeyContainerPermissionAccessEntryCollection();
			foreach (KeyContainerPermissionAccessEntry accessEntry in this._accessEntries)
			{
				keyContainerPermissionAccessEntryCollection.Add(accessEntry);
			}
			foreach (KeyContainerPermissionAccessEntry accessEntry2 in keyContainerPermission._accessEntries)
			{
				if (this._accessEntries.IndexOf(accessEntry2) == -1)
				{
					keyContainerPermissionAccessEntryCollection.Add(accessEntry2);
				}
			}
			if (keyContainerPermissionAccessEntryCollection.Count == 0)
			{
				return new KeyContainerPermission(this._flags | keyContainerPermission._flags);
			}
			KeyContainerPermissionAccessEntry[] array = new KeyContainerPermissionAccessEntry[keyContainerPermissionAccessEntryCollection.Count];
			keyContainerPermissionAccessEntryCollection.CopyTo(array, 0);
			return new KeyContainerPermission(this._flags | keyContainerPermission._flags, array);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x00026EE5 File Offset: 0x000250E5
		int IBuiltInPermission.GetTokenIndex()
		{
			return 16;
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x0009F831 File Offset: 0x0009DA31
		private void SetFlags(KeyContainerPermissionFlags flags)
		{
			if ((flags & KeyContainerPermissionFlags.AllFlags) == KeyContainerPermissionFlags.NoFlags)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), flags), "KeyContainerPermissionFlags");
			}
			this._flags = flags;
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x0009F863 File Offset: 0x0009DA63
		private KeyContainerPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			KeyContainerPermission keyContainerPermission = target as KeyContainerPermission;
			if (keyContainerPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(KeyContainerPermission));
			}
			return keyContainerPermission;
		}

		// Token: 0x0400204A RID: 8266
		private KeyContainerPermissionAccessEntryCollection _accessEntries;

		// Token: 0x0400204B RID: 8267
		private KeyContainerPermissionFlags _flags;

		// Token: 0x0400204C RID: 8268
		private const int version = 1;
	}
}
