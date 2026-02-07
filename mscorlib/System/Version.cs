using System;
using System.Globalization;
using System.Text;

namespace System
{
	// Token: 0x020001B6 RID: 438
	[Serializable]
	public sealed class Version : ICloneable, IComparable, IComparable<Version>, IEquatable<Version>, ISpanFormattable
	{
		// Token: 0x06001300 RID: 4864 RVA: 0x0004CEC0 File Offset: 0x0004B0C0
		public Version(int major, int minor, int build, int revision)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", "Version's parameters must be greater than or equal to zero.");
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", "Version's parameters must be greater than or equal to zero.");
			}
			if (build < 0)
			{
				throw new ArgumentOutOfRangeException("build", "Version's parameters must be greater than or equal to zero.");
			}
			if (revision < 0)
			{
				throw new ArgumentOutOfRangeException("revision", "Version's parameters must be greater than or equal to zero.");
			}
			this._Major = major;
			this._Minor = minor;
			this._Build = build;
			this._Revision = revision;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0004CF50 File Offset: 0x0004B150
		public Version(int major, int minor, int build)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", "Version's parameters must be greater than or equal to zero.");
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", "Version's parameters must be greater than or equal to zero.");
			}
			if (build < 0)
			{
				throw new ArgumentOutOfRangeException("build", "Version's parameters must be greater than or equal to zero.");
			}
			this._Major = major;
			this._Minor = minor;
			this._Build = build;
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0004CFC4 File Offset: 0x0004B1C4
		public Version(int major, int minor)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", "Version's parameters must be greater than or equal to zero.");
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", "Version's parameters must be greater than or equal to zero.");
			}
			this._Major = major;
			this._Minor = minor;
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0004D01C File Offset: 0x0004B21C
		public Version(string version)
		{
			Version version2 = Version.Parse(version);
			this._Major = version2.Major;
			this._Minor = version2.Minor;
			this._Build = version2.Build;
			this._Revision = version2.Revision;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0004D074 File Offset: 0x0004B274
		public Version()
		{
			this._Major = 0;
			this._Minor = 0;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0004D098 File Offset: 0x0004B298
		private Version(Version version)
		{
			this._Major = version._Major;
			this._Minor = version._Minor;
			this._Build = version._Build;
			this._Revision = version._Revision;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0004D0E9 File Offset: 0x0004B2E9
		public object Clone()
		{
			return new Version(this);
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x0004D0F1 File Offset: 0x0004B2F1
		public int Major
		{
			get
			{
				return this._Major;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x0004D0F9 File Offset: 0x0004B2F9
		public int Minor
		{
			get
			{
				return this._Minor;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x0004D101 File Offset: 0x0004B301
		public int Build
		{
			get
			{
				return this._Build;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x0004D109 File Offset: 0x0004B309
		public int Revision
		{
			get
			{
				return this._Revision;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x0004D111 File Offset: 0x0004B311
		public short MajorRevision
		{
			get
			{
				return (short)(this._Revision >> 16);
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x0004D11D File Offset: 0x0004B31D
		public short MinorRevision
		{
			get
			{
				return (short)(this._Revision & 65535);
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0004D12C File Offset: 0x0004B32C
		public int CompareTo(object version)
		{
			if (version == null)
			{
				return 1;
			}
			Version version2 = version as Version;
			if (version2 == null)
			{
				throw new ArgumentException("Object must be of type Version.");
			}
			return this.CompareTo(version2);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0004D160 File Offset: 0x0004B360
		public int CompareTo(Version value)
		{
			if (value == this)
			{
				return 0;
			}
			if (value == null)
			{
				return 1;
			}
			if (this._Major == value._Major)
			{
				if (this._Minor == value._Minor)
				{
					if (this._Build == value._Build)
					{
						if (this._Revision == value._Revision)
						{
							return 0;
						}
						if (this._Revision <= value._Revision)
						{
							return -1;
						}
						return 1;
					}
					else
					{
						if (this._Build <= value._Build)
						{
							return -1;
						}
						return 1;
					}
				}
				else
				{
					if (this._Minor <= value._Minor)
					{
						return -1;
					}
					return 1;
				}
			}
			else
			{
				if (this._Major <= value._Major)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0004D1FF File Offset: 0x0004B3FF
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Version);
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0004D210 File Offset: 0x0004B410
		public bool Equals(Version obj)
		{
			return obj == this || (obj != null && this._Major == obj._Major && this._Minor == obj._Minor && this._Build == obj._Build && this._Revision == obj._Revision);
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0004D260 File Offset: 0x0004B460
		public override int GetHashCode()
		{
			return 0 | (this._Major & 15) << 28 | (this._Minor & 255) << 20 | (this._Build & 255) << 12 | (this._Revision & 4095);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0004D29D File Offset: 0x0004B49D
		public override string ToString()
		{
			return this.ToString(this.DefaultFormatFieldCount);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0004D2AB File Offset: 0x0004B4AB
		public string ToString(int fieldCount)
		{
			if (fieldCount == 0)
			{
				return string.Empty;
			}
			if (fieldCount != 1)
			{
				return StringBuilderCache.GetStringAndRelease(this.ToCachedStringBuilder(fieldCount));
			}
			return this._Major.ToString();
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x0004D2D2 File Offset: 0x0004B4D2
		public bool TryFormat(Span<char> destination, out int charsWritten)
		{
			return this.TryFormat(destination, this.DefaultFormatFieldCount, out charsWritten);
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0004D2E4 File Offset: 0x0004B4E4
		public bool TryFormat(Span<char> destination, int fieldCount, out int charsWritten)
		{
			if (fieldCount == 0)
			{
				charsWritten = 0;
				return true;
			}
			if (fieldCount == 1)
			{
				return this._Major.TryFormat(destination, out charsWritten, default(ReadOnlySpan<char>), null);
			}
			StringBuilder stringBuilder = this.ToCachedStringBuilder(fieldCount);
			if (stringBuilder.Length <= destination.Length)
			{
				stringBuilder.CopyTo(0, destination, stringBuilder.Length);
				StringBuilderCache.Release(stringBuilder);
				charsWritten = stringBuilder.Length;
				return true;
			}
			StringBuilderCache.Release(stringBuilder);
			charsWritten = 0;
			return false;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0004D354 File Offset: 0x0004B554
		bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
		{
			return this.TryFormat(destination, out charsWritten);
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x0004D35E File Offset: 0x0004B55E
		private int DefaultFormatFieldCount
		{
			get
			{
				if (this._Build == -1)
				{
					return 2;
				}
				if (this._Revision != -1)
				{
					return 4;
				}
				return 3;
			}
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0004D378 File Offset: 0x0004B578
		private StringBuilder ToCachedStringBuilder(int fieldCount)
		{
			if (fieldCount == 2)
			{
				StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
				stringBuilder.Append(this._Major);
				stringBuilder.Append('.');
				stringBuilder.Append(this._Minor);
				return stringBuilder;
			}
			if (this._Build == -1)
			{
				throw new ArgumentException(SR.Format("Argument must be between {0} and {1}.", "0", "2"), "fieldCount");
			}
			if (fieldCount == 3)
			{
				StringBuilder stringBuilder2 = StringBuilderCache.Acquire(16);
				stringBuilder2.Append(this._Major);
				stringBuilder2.Append('.');
				stringBuilder2.Append(this._Minor);
				stringBuilder2.Append('.');
				stringBuilder2.Append(this._Build);
				return stringBuilder2;
			}
			if (this._Revision == -1)
			{
				throw new ArgumentException(SR.Format("Argument must be between {0} and {1}.", "0", "3"), "fieldCount");
			}
			if (fieldCount == 4)
			{
				StringBuilder stringBuilder3 = StringBuilderCache.Acquire(16);
				stringBuilder3.Append(this._Major);
				stringBuilder3.Append('.');
				stringBuilder3.Append(this._Minor);
				stringBuilder3.Append('.');
				stringBuilder3.Append(this._Build);
				stringBuilder3.Append('.');
				stringBuilder3.Append(this._Revision);
				return stringBuilder3;
			}
			throw new ArgumentException(SR.Format("Argument must be between {0} and {1}.", "0", "4"), "fieldCount");
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0004D4C2 File Offset: 0x0004B6C2
		public static Version Parse(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return Version.ParseVersion(input.AsSpan(), true);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0004D4DE File Offset: 0x0004B6DE
		public static Version Parse(ReadOnlySpan<char> input)
		{
			return Version.ParseVersion(input, true);
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0004D4E8 File Offset: 0x0004B6E8
		public static bool TryParse(string input, out Version result)
		{
			if (input == null)
			{
				result = null;
				return false;
			}
			Version v;
			result = (v = Version.ParseVersion(input.AsSpan(), false));
			return v != null;
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0004D514 File Offset: 0x0004B714
		public static bool TryParse(ReadOnlySpan<char> input, out Version result)
		{
			Version v;
			result = (v = Version.ParseVersion(input, false));
			return v != null;
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0004D534 File Offset: 0x0004B734
		private static Version ParseVersion(ReadOnlySpan<char> input, bool throwOnFailure)
		{
			int num = input.IndexOf('.');
			if (num < 0)
			{
				if (throwOnFailure)
				{
					throw new ArgumentException("Version string portion was too short or too long.", "input");
				}
				return null;
			}
			else
			{
				int num2 = -1;
				int num3 = input.Slice(num + 1).IndexOf('.');
				if (num3 != -1)
				{
					num3 += num + 1;
					num2 = input.Slice(num3 + 1).IndexOf('.');
					if (num2 != -1)
					{
						num2 += num3 + 1;
						if (input.Slice(num2 + 1).IndexOf('.') != -1)
						{
							if (throwOnFailure)
							{
								throw new ArgumentException("Version string portion was too short or too long.", "input");
							}
							return null;
						}
					}
				}
				int major;
				if (!Version.TryParseComponent(input.Slice(0, num), "input", throwOnFailure, out major))
				{
					return null;
				}
				if (num3 != -1)
				{
					int minor;
					if (!Version.TryParseComponent(input.Slice(num + 1, num3 - num - 1), "input", throwOnFailure, out minor))
					{
						return null;
					}
					if (num2 != -1)
					{
						int build;
						int revision;
						if (!Version.TryParseComponent(input.Slice(num3 + 1, num2 - num3 - 1), "build", throwOnFailure, out build) || !Version.TryParseComponent(input.Slice(num2 + 1), "revision", throwOnFailure, out revision))
						{
							return null;
						}
						return new Version(major, minor, build, revision);
					}
					else
					{
						int build;
						if (!Version.TryParseComponent(input.Slice(num3 + 1), "build", throwOnFailure, out build))
						{
							return null;
						}
						return new Version(major, minor, build);
					}
				}
				else
				{
					int minor;
					if (!Version.TryParseComponent(input.Slice(num + 1), "input", throwOnFailure, out minor))
					{
						return null;
					}
					return new Version(major, minor);
				}
			}
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0004D69C File Offset: 0x0004B89C
		private static bool TryParseComponent(ReadOnlySpan<char> component, string componentName, bool throwOnFailure, out int parsedComponent)
		{
			if (!throwOnFailure)
			{
				return int.TryParse(component, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedComponent) && parsedComponent >= 0;
			}
			if ((parsedComponent = int.Parse(component, NumberStyles.Integer, CultureInfo.InvariantCulture)) < 0)
			{
				throw new ArgumentOutOfRangeException(componentName, "Version's parameters must be greater than or equal to zero.");
			}
			return true;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0004D6E7 File Offset: 0x0004B8E7
		public static bool operator ==(Version v1, Version v2)
		{
			if (v1 == null)
			{
				return v2 == null;
			}
			return v1.Equals(v2);
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0004D6F8 File Offset: 0x0004B8F8
		public static bool operator !=(Version v1, Version v2)
		{
			return !(v1 == v2);
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0004D704 File Offset: 0x0004B904
		public static bool operator <(Version v1, Version v2)
		{
			if (v1 == null)
			{
				throw new ArgumentNullException("v1");
			}
			return v1.CompareTo(v2) < 0;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0004D71E File Offset: 0x0004B91E
		public static bool operator <=(Version v1, Version v2)
		{
			if (v1 == null)
			{
				throw new ArgumentNullException("v1");
			}
			return v1.CompareTo(v2) <= 0;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0004D73B File Offset: 0x0004B93B
		public static bool operator >(Version v1, Version v2)
		{
			return v2 < v1;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0004D744 File Offset: 0x0004B944
		public static bool operator >=(Version v1, Version v2)
		{
			return v2 <= v1;
		}

		// Token: 0x0400138A RID: 5002
		private readonly int _Major;

		// Token: 0x0400138B RID: 5003
		private readonly int _Minor;

		// Token: 0x0400138C RID: 5004
		private readonly int _Build = -1;

		// Token: 0x0400138D RID: 5005
		private readonly int _Revision = -1;
	}
}
