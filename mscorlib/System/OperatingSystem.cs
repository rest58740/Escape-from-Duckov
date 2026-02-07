using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020001CC RID: 460
	[Serializable]
	public sealed class OperatingSystem : ISerializable, ICloneable
	{
		// Token: 0x060013BC RID: 5052 RVA: 0x0004E838 File Offset: 0x0004CA38
		public OperatingSystem(PlatformID platform, Version version) : this(platform, version, null)
		{
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0004E844 File Offset: 0x0004CA44
		internal OperatingSystem(PlatformID platform, Version version, string servicePack)
		{
			if (platform < PlatformID.Win32S || platform > PlatformID.MacOSX)
			{
				throw new ArgumentOutOfRangeException("platform", platform, SR.Format("Illegal enum value: {0}.", platform));
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._platform = platform;
			this._version = version;
			this._servicePack = servicePack;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0001B98F File Offset: 0x00019B8F
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x0004E8A9 File Offset: 0x0004CAA9
		public PlatformID Platform
		{
			get
			{
				return this._platform;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0004E8B1 File Offset: 0x0004CAB1
		public string ServicePack
		{
			get
			{
				return this._servicePack ?? string.Empty;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x0004E8C2 File Offset: 0x0004CAC2
		public Version Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0004E8CA File Offset: 0x0004CACA
		public object Clone()
		{
			return new OperatingSystem(this._platform, this._version, this._servicePack);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0004E8E3 File Offset: 0x0004CAE3
		public override string ToString()
		{
			return this.VersionString;
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0004E8EC File Offset: 0x0004CAEC
		public string VersionString
		{
			get
			{
				if (this._versionString == null)
				{
					string str;
					switch (this._platform)
					{
					case PlatformID.Win32S:
						str = "Microsoft Win32S ";
						break;
					case PlatformID.Win32Windows:
						str = ((this._version.Major > 4 || (this._version.Major == 4 && this._version.Minor > 0)) ? "Microsoft Windows 98 " : "Microsoft Windows 95 ");
						break;
					case PlatformID.Win32NT:
						str = "Microsoft Windows NT ";
						break;
					case PlatformID.WinCE:
						str = "Microsoft Windows CE ";
						break;
					case PlatformID.Unix:
						str = "Unix ";
						break;
					case PlatformID.Xbox:
						str = "Xbox ";
						break;
					case PlatformID.MacOSX:
						str = "Mac OS X ";
						break;
					default:
						str = "<unknown> ";
						break;
					}
					this._versionString = (string.IsNullOrEmpty(this._servicePack) ? (str + this._version.ToString()) : (str + this._version.ToString(3) + " " + this._servicePack));
				}
				return this._versionString;
			}
		}

		// Token: 0x04001455 RID: 5205
		private readonly Version _version;

		// Token: 0x04001456 RID: 5206
		private readonly PlatformID _platform;

		// Token: 0x04001457 RID: 5207
		private readonly string _servicePack;

		// Token: 0x04001458 RID: 5208
		private string _versionString;
	}
}
