using System;
using System.Globalization;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200050E RID: 1294
	public sealed class CommonAce : QualifiedAce
	{
		// Token: 0x0600335D RID: 13149 RVA: 0x000BCA8C File Offset: 0x000BAC8C
		public CommonAce(AceFlags flags, AceQualifier qualifier, int accessMask, SecurityIdentifier sid, bool isCallback, byte[] opaque) : base(CommonAce.ConvertType(qualifier, isCallback), flags, opaque)
		{
			base.AccessMask = accessMask;
			base.SecurityIdentifier = sid;
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000BCAAE File Offset: 0x000BACAE
		internal CommonAce(AceType type, AceFlags flags, int accessMask, SecurityIdentifier sid, byte[] opaque) : base(type, flags, opaque)
		{
			base.AccessMask = accessMask;
			base.SecurityIdentifier = sid;
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000BCACC File Offset: 0x000BACCC
		internal CommonAce(byte[] binaryForm, int offset) : base(binaryForm, offset)
		{
			int num = (int)GenericAce.ReadUShort(binaryForm, offset + 2);
			if (offset > binaryForm.Length - num)
			{
				throw new ArgumentException("Invalid ACE - truncated", "binaryForm");
			}
			if (num < 8 + SecurityIdentifier.MinBinaryLength)
			{
				throw new ArgumentException("Invalid ACE", "binaryForm");
			}
			base.AccessMask = GenericAce.ReadInt(binaryForm, offset + 4);
			base.SecurityIdentifier = new SecurityIdentifier(binaryForm, offset + 8);
			int num2 = num - (8 + base.SecurityIdentifier.BinaryLength);
			if (num2 > 0)
			{
				byte[] array = new byte[num2];
				Array.Copy(binaryForm, offset + 8 + base.SecurityIdentifier.BinaryLength, array, 0, num2);
				base.SetOpaque(array);
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06003360 RID: 13152 RVA: 0x000BCB75 File Offset: 0x000BAD75
		public override int BinaryLength
		{
			get
			{
				return 8 + base.SecurityIdentifier.BinaryLength + base.OpaqueLength;
			}
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000BCB8C File Offset: 0x000BAD8C
		public override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			ushort binaryLength = (ushort)this.BinaryLength;
			binaryForm[offset] = (byte)base.AceType;
			binaryForm[offset + 1] = (byte)base.AceFlags;
			GenericAce.WriteUShort(binaryLength, binaryForm, offset + 2);
			GenericAce.WriteInt(base.AccessMask, binaryForm, offset + 4);
			base.SecurityIdentifier.GetBinaryForm(binaryForm, offset + 8);
			byte[] opaque = base.GetOpaque();
			if (opaque != null)
			{
				Array.Copy(opaque, 0, binaryForm, offset + 8 + base.SecurityIdentifier.BinaryLength, opaque.Length);
			}
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000BCBFF File Offset: 0x000BADFF
		public static int MaxOpaqueLength(bool isCallback)
		{
			return 65459;
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x000BCC08 File Offset: 0x000BAE08
		internal override string GetSddlForm()
		{
			if (base.OpaqueLength != 0)
			{
				throw new NotImplementedException("Unable to convert conditional ACEs to SDDL");
			}
			return string.Format(CultureInfo.InvariantCulture, "({0};{1};{2};;;{3})", new object[]
			{
				GenericAce.GetSddlAceType(base.AceType),
				GenericAce.GetSddlAceFlags(base.AceFlags),
				KnownAce.GetSddlAccessRights(base.AccessMask),
				base.SecurityIdentifier.GetSddlForm()
			});
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x000BCC78 File Offset: 0x000BAE78
		private static AceType ConvertType(AceQualifier qualifier, bool isCallback)
		{
			switch (qualifier)
			{
			case AceQualifier.AccessAllowed:
				if (isCallback)
				{
					return AceType.AccessAllowedCallback;
				}
				return AceType.AccessAllowed;
			case AceQualifier.AccessDenied:
				if (isCallback)
				{
					return AceType.AccessDeniedCallback;
				}
				return AceType.AccessDenied;
			case AceQualifier.SystemAudit:
				if (isCallback)
				{
					return AceType.SystemAuditCallback;
				}
				return AceType.SystemAudit;
			case AceQualifier.SystemAlarm:
				if (isCallback)
				{
					return AceType.SystemAlarmCallback;
				}
				return AceType.SystemAlarm;
			default:
				throw new ArgumentException("Unrecognized ACE qualifier: " + qualifier.ToString(), "qualifier");
			}
		}
	}
}
