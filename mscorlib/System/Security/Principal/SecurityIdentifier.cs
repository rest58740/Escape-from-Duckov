using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Principal
{
	// Token: 0x020004E7 RID: 1255
	[ComVisible(false)]
	public sealed class SecurityIdentifier : IdentityReference, IComparable<SecurityIdentifier>
	{
		// Token: 0x06003214 RID: 12820 RVA: 0x000B7EDB File Offset: 0x000B60DB
		public SecurityIdentifier(string sddlForm)
		{
			if (sddlForm == null)
			{
				throw new ArgumentNullException("sddlForm");
			}
			this.buffer = SecurityIdentifier.ParseSddlForm(sddlForm);
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x000B7F00 File Offset: 0x000B6100
		public unsafe SecurityIdentifier(byte[] binaryForm, int offset)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0 || offset > binaryForm.Length - 2)
			{
				throw new ArgumentException("offset");
			}
			fixed (byte[] array = binaryForm)
			{
				byte* ptr;
				if (binaryForm == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				this.CreateFromBinaryForm((IntPtr)((void*)(ptr + offset)), binaryForm.Length - offset);
			}
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x000B7F65 File Offset: 0x000B6165
		public SecurityIdentifier(IntPtr binaryForm)
		{
			this.CreateFromBinaryForm(binaryForm, int.MaxValue);
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x000B7F7C File Offset: 0x000B617C
		private void CreateFromBinaryForm(IntPtr binaryForm, int length)
		{
			int num = (int)Marshal.ReadByte(binaryForm, 0);
			int num2 = (int)Marshal.ReadByte(binaryForm, 1);
			if (num != 1 || num2 > 15)
			{
				throw new ArgumentException("Value was invalid.");
			}
			if (length < 8 + num2 * 4)
			{
				throw new ArgumentException("offset");
			}
			this.buffer = new byte[8 + num2 * 4];
			Marshal.Copy(binaryForm, this.buffer, 0, this.buffer.Length);
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000B7FE4 File Offset: 0x000B61E4
		public SecurityIdentifier(WellKnownSidType sidType, SecurityIdentifier domainSid)
		{
			WellKnownAccount wellKnownAccount = WellKnownAccount.LookupByType(sidType);
			if (wellKnownAccount == null)
			{
				throw new ArgumentException("Unable to convert SID type: " + sidType.ToString());
			}
			if (wellKnownAccount.IsAbsolute)
			{
				this.buffer = SecurityIdentifier.ParseSddlForm(wellKnownAccount.Sid);
				return;
			}
			if (domainSid == null)
			{
				throw new ArgumentNullException("domainSid");
			}
			this.buffer = SecurityIdentifier.ParseSddlForm(domainSid.Value + "-" + wellKnownAccount.Rid);
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06003219 RID: 12825 RVA: 0x000B8070 File Offset: 0x000B6270
		public SecurityIdentifier AccountDomainSid
		{
			get
			{
				if (!this.Value.StartsWith("S-1-5-21") || this.buffer[1] < 4)
				{
					return null;
				}
				byte[] array = new byte[24];
				Array.Copy(this.buffer, 0, array, 0, array.Length);
				array[1] = 4;
				return new SecurityIdentifier(array, 0);
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600321A RID: 12826 RVA: 0x000B80C0 File Offset: 0x000B62C0
		public int BinaryLength
		{
			get
			{
				return this.buffer.Length;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x0600321B RID: 12827 RVA: 0x000B80CC File Offset: 0x000B62CC
		public override string Value
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				ulong sidAuthority = this.GetSidAuthority();
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "S-1-{0}", sidAuthority);
				for (byte b = 0; b < this.GetSidSubAuthorityCount(); b += 1)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "-{0}", this.GetSidSubAuthority(b));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x000B8134 File Offset: 0x000B6334
		private ulong GetSidAuthority()
		{
			return (ulong)this.buffer[2] << 40 | (ulong)this.buffer[3] << 32 | (ulong)this.buffer[4] << 24 | (ulong)this.buffer[5] << 16 | (ulong)this.buffer[6] << 8 | (ulong)this.buffer[7];
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000B818A File Offset: 0x000B638A
		private byte GetSidSubAuthorityCount()
		{
			return this.buffer[1];
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x000B8194 File Offset: 0x000B6394
		private uint GetSidSubAuthority(byte index)
		{
			int num = (int)(8 + index * 4);
			return (uint)((int)this.buffer[num] | (int)this.buffer[num + 1] << 8 | (int)this.buffer[num + 2] << 16 | (int)this.buffer[num + 3] << 24);
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x000B81D8 File Offset: 0x000B63D8
		public int CompareTo(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			int result;
			if ((result = this.GetSidAuthority().CompareTo(sid.GetSidAuthority())) != 0)
			{
				return result;
			}
			if ((result = this.GetSidSubAuthorityCount().CompareTo(sid.GetSidSubAuthorityCount())) != 0)
			{
				return result;
			}
			for (byte b = 0; b < this.GetSidSubAuthorityCount(); b += 1)
			{
				if ((result = this.GetSidSubAuthority(b).CompareTo(sid.GetSidSubAuthority(b))) != 0)
				{
					return result;
				}
			}
			return 0;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x000B825D File Offset: 0x000B645D
		public override bool Equals(object o)
		{
			return this.Equals(o as SecurityIdentifier);
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x000B826B File Offset: 0x000B646B
		public bool Equals(SecurityIdentifier sid)
		{
			return !(sid == null) && sid.Value == this.Value;
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x000B828C File Offset: 0x000B648C
		public void GetBinaryForm(byte[] binaryForm, int offset)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0 || offset > binaryForm.Length - this.buffer.Length)
			{
				throw new ArgumentException("offset");
			}
			Array.Copy(this.buffer, 0, binaryForm, offset, this.buffer.Length);
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000B7E20 File Offset: 0x000B6020
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000B82DB File Offset: 0x000B64DB
		public bool IsAccountSid()
		{
			return this.AccountDomainSid != null;
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000B82EC File Offset: 0x000B64EC
		public bool IsEqualDomainSid(SecurityIdentifier sid)
		{
			SecurityIdentifier accountDomainSid = this.AccountDomainSid;
			return !(accountDomainSid == null) && accountDomainSid.Equals(sid.AccountDomainSid);
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x000B8317 File Offset: 0x000B6517
		public override bool IsValidTargetType(Type targetType)
		{
			return targetType == typeof(SecurityIdentifier) || targetType == typeof(NTAccount);
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x000B8344 File Offset: 0x000B6544
		public bool IsWellKnown(WellKnownSidType type)
		{
			WellKnownAccount wellKnownAccount = WellKnownAccount.LookupByType(type);
			if (wellKnownAccount == null)
			{
				return false;
			}
			string value = this.Value;
			if (wellKnownAccount.IsAbsolute)
			{
				return value == wellKnownAccount.Sid;
			}
			return value.StartsWith("S-1-5-21", StringComparison.OrdinalIgnoreCase) && value.EndsWith("-" + wellKnownAccount.Rid, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000B7E58 File Offset: 0x000B6058
		public override string ToString()
		{
			return this.Value;
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x000B83A0 File Offset: 0x000B65A0
		public override IdentityReference Translate(Type targetType)
		{
			if (targetType == typeof(SecurityIdentifier))
			{
				return this;
			}
			if (!(targetType == typeof(NTAccount)))
			{
				throw new ArgumentException("Unknown type.", "targetType");
			}
			WellKnownAccount wellKnownAccount = WellKnownAccount.LookupBySid(this.Value);
			if (wellKnownAccount == null || wellKnownAccount.Name == null)
			{
				throw new IdentityNotMappedException("Unable to map SID: " + this.Value);
			}
			return new NTAccount(wellKnownAccount.Name);
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x000B7BBC File Offset: 0x000B5DBC
		public static bool operator ==(SecurityIdentifier left, SecurityIdentifier right)
		{
			if (left == null)
			{
				return right == null;
			}
			return right != null && left.Value == right.Value;
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000B7BDC File Offset: 0x000B5DDC
		public static bool operator !=(SecurityIdentifier left, SecurityIdentifier right)
		{
			if (left == null)
			{
				return right != null;
			}
			return right == null || left.Value != right.Value;
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000B841C File Offset: 0x000B661C
		internal string GetSddlForm()
		{
			string value = this.Value;
			WellKnownAccount wellKnownAccount = WellKnownAccount.LookupBySid(value);
			if (wellKnownAccount == null || wellKnownAccount.SddlForm == null)
			{
				return value;
			}
			return wellKnownAccount.SddlForm;
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x000B844C File Offset: 0x000B664C
		internal static SecurityIdentifier ParseSddlForm(string sddlForm, ref int pos)
		{
			if (sddlForm.Length - pos < 2)
			{
				throw new ArgumentException("Invalid SDDL string.", "sddlForm");
			}
			string text = sddlForm.Substring(pos, 2).ToUpperInvariant();
			string sddlForm2;
			int num2;
			if (text == "S-")
			{
				int num = pos;
				char c = char.ToUpperInvariant(sddlForm[num]);
				while (c == 'S' || c == '-' || c == 'X' || (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F'))
				{
					num++;
					c = char.ToUpperInvariant(sddlForm[num]);
				}
				if (c == ':' && sddlForm[num - 1] == 'D')
				{
					num--;
				}
				sddlForm2 = sddlForm.Substring(pos, num - pos);
				num2 = num - pos;
			}
			else
			{
				sddlForm2 = text;
				num2 = 2;
			}
			SecurityIdentifier result = new SecurityIdentifier(sddlForm2);
			pos += num2;
			return result;
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x000B851C File Offset: 0x000B671C
		private static byte[] ParseSddlForm(string sddlForm)
		{
			string text = sddlForm;
			if (sddlForm.Length == 2)
			{
				WellKnownAccount wellKnownAccount = WellKnownAccount.LookupBySddlForm(sddlForm);
				if (wellKnownAccount == null)
				{
					throw new ArgumentException("Invalid SDDL string - unrecognized account: " + sddlForm, "sddlForm");
				}
				if (!wellKnownAccount.IsAbsolute)
				{
					throw new NotImplementedException("Mono unable to convert account to SID: " + ((wellKnownAccount.Name != null) ? wellKnownAccount.Name : sddlForm));
				}
				text = wellKnownAccount.Sid;
			}
			string[] array = text.ToUpperInvariant().Split('-', StringSplitOptions.None);
			int num = array.Length - 3;
			if (array.Length < 3 || array[0] != "S" || num > 15)
			{
				throw new ArgumentException("Value was invalid.");
			}
			if (array[1] != "1")
			{
				throw new ArgumentException("Only SIDs with revision 1 are supported");
			}
			byte[] array2 = new byte[8 + num * 4];
			array2[0] = 1;
			array2[1] = (byte)num;
			ulong num2;
			if (!SecurityIdentifier.TryParseAuthority(array[2], out num2))
			{
				throw new ArgumentException("Value was invalid.");
			}
			array2[2] = (byte)(num2 >> 40 & 255UL);
			array2[3] = (byte)(num2 >> 32 & 255UL);
			array2[4] = (byte)(num2 >> 24 & 255UL);
			array2[5] = (byte)(num2 >> 16 & 255UL);
			array2[6] = (byte)(num2 >> 8 & 255UL);
			array2[7] = (byte)(num2 & 255UL);
			for (int i = 0; i < num; i++)
			{
				uint num3;
				if (!SecurityIdentifier.TryParseSubAuthority(array[i + 3], out num3))
				{
					throw new ArgumentException("Value was invalid.");
				}
				int num4 = 8 + i * 4;
				array2[num4] = (byte)num3;
				array2[num4 + 1] = (byte)(num3 >> 8);
				array2[num4 + 2] = (byte)(num3 >> 16);
				array2[num4 + 3] = (byte)(num3 >> 24);
			}
			return array2;
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x000B86C2 File Offset: 0x000B68C2
		private static bool TryParseAuthority(string s, out ulong result)
		{
			if (s.StartsWith("0X"))
			{
				return ulong.TryParse(s.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
			}
			return ulong.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x000B86F6 File Offset: 0x000B68F6
		private static bool TryParseSubAuthority(string s, out uint result)
		{
			if (s.StartsWith("0X"))
			{
				return uint.TryParse(s.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
			}
			return uint.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}

		// Token: 0x040022C2 RID: 8898
		private byte[] buffer;

		// Token: 0x040022C3 RID: 8899
		public static readonly int MaxBinaryLength = 68;

		// Token: 0x040022C4 RID: 8900
		public static readonly int MinBinaryLength = 8;
	}
}
