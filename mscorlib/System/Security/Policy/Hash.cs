using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using Unity;

namespace System.Security.Policy
{
	// Token: 0x02000412 RID: 1042
	[ComVisible(true)]
	[Serializable]
	public sealed class Hash : EvidenceBase, ISerializable, IBuiltInEvidence
	{
		// Token: 0x06002A9C RID: 10908 RVA: 0x0009A181 File Offset: 0x00098381
		public Hash(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.assembly = assembly;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x00097E2E File Offset: 0x0009602E
		internal Hash()
		{
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x0009A1A4 File Offset: 0x000983A4
		internal Hash(SerializationInfo info, StreamingContext context)
		{
			this.data = (byte[])info.GetValue("RawData", typeof(byte[]));
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06002A9F RID: 10911 RVA: 0x0009A1CC File Offset: 0x000983CC
		public byte[] MD5
		{
			get
			{
				if (this._md5 != null)
				{
					return this._md5;
				}
				if (this.assembly == null && this._sha1 != null)
				{
					throw new SecurityException(Locale.GetText("No assembly data. This instance was initialized with an MSHA1 digest value."));
				}
				HashAlgorithm hashAlg = System.Security.Cryptography.MD5.Create();
				this._md5 = this.GenerateHash(hashAlg);
				return this._md5;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x0009A228 File Offset: 0x00098428
		public byte[] SHA1
		{
			get
			{
				if (this._sha1 != null)
				{
					return this._sha1;
				}
				if (this.assembly == null && this._md5 != null)
				{
					throw new SecurityException(Locale.GetText("No assembly data. This instance was initialized with an MD5 digest value."));
				}
				HashAlgorithm hashAlg = System.Security.Cryptography.SHA1.Create();
				this._sha1 = this.GenerateHash(hashAlg);
				return this._sha1;
			}
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x0009A283 File Offset: 0x00098483
		public byte[] GenerateHash(HashAlgorithm hashAlg)
		{
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			return hashAlg.ComputeHash(this.GetData());
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x0009A29F File Offset: 0x0009849F
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("RawData", this.GetData());
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x0009A2C0 File Offset: 0x000984C0
		public override string ToString()
		{
			SecurityElement securityElement = new SecurityElement(base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = this.GetData();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			securityElement.AddChild(new SecurityElement("RawData", stringBuilder.ToString()));
			return securityElement.ToString();
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x0009A340 File Offset: 0x00098540
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		private byte[] GetData()
		{
			if (this.assembly == null && this.data == null)
			{
				throw new SecurityException(Locale.GetText("No assembly data."));
			}
			if (this.data == null)
			{
				FileStream fileStream = new FileStream(this.assembly.Location, FileMode.Open, FileAccess.Read);
				this.data = new byte[fileStream.Length];
				fileStream.Read(this.data, 0, (int)fileStream.Length);
			}
			return this.data;
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x0009A3BB File Offset: 0x000985BB
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			if (!verbose)
			{
				return 0;
			}
			return 5;
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return 0;
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			return 0;
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x0009A3C3 File Offset: 0x000985C3
		public static Hash CreateMD5(byte[] md5)
		{
			if (md5 == null)
			{
				throw new ArgumentNullException("md5");
			}
			return new Hash
			{
				_md5 = md5
			};
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x0009A3DF File Offset: 0x000985DF
		public static Hash CreateSHA1(byte[] sha1)
		{
			if (sha1 == null)
			{
				throw new ArgumentNullException("sha1");
			}
			return new Hash
			{
				_sha1 = sha1
			};
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06002AAA RID: 10922 RVA: 0x00052959 File Offset: 0x00050B59
		public byte[] SHA256
		{
			get
			{
				ThrowStub.ThrowNotSupportedException();
				return null;
			}
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x00052959 File Offset: 0x00050B59
		public static Hash CreateSHA256(byte[] sha256)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x04001F98 RID: 8088
		private Assembly assembly;

		// Token: 0x04001F99 RID: 8089
		private byte[] data;

		// Token: 0x04001F9A RID: 8090
		internal byte[] _md5;

		// Token: 0x04001F9B RID: 8091
		internal byte[] _sha1;
	}
}
