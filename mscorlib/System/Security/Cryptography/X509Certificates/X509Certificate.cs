using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Internal.Cryptography;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020004D9 RID: 1241
	[Serializable]
	public class X509Certificate : IDisposable, IDeserializationCallback, ISerializable
	{
		// Token: 0x0600316C RID: 12652 RVA: 0x000B6CE8 File Offset: 0x000B4EE8
		public virtual void Reset()
		{
			if (this.impl != null)
			{
				this.impl.Dispose();
				this.impl = null;
			}
			this.lazyCertHash = null;
			this.lazyIssuer = null;
			this.lazySubject = null;
			this.lazySerialNumber = null;
			this.lazyKeyAlgorithm = null;
			this.lazyKeyAlgorithmParameters = null;
			this.lazyPublicKey = null;
			this.lazyNotBefore = DateTime.MinValue;
			this.lazyNotAfter = DateTime.MinValue;
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x000B6D64 File Offset: 0x000B4F64
		public X509Certificate()
		{
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x000B6D82 File Offset: 0x000B4F82
		public X509Certificate(byte[] data)
		{
			if (data != null && data.Length != 0)
			{
				this.impl = X509Helper.Import(data);
			}
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x000B6DB3 File Offset: 0x000B4FB3
		public X509Certificate(byte[] rawData, string password) : this(rawData, password, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x000B6DBE File Offset: 0x000B4FBE
		[CLSCompliant(false)]
		public X509Certificate(byte[] rawData, SecureString password) : this(rawData, password, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x000B6DCC File Offset: 0x000B4FCC
		public X509Certificate(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			if (rawData == null || rawData.Length == 0)
			{
				throw new ArgumentException("Array cannot be empty or null.", "rawData");
			}
			X509Certificate.ValidateKeyStorageFlags(keyStorageFlags);
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				this.impl = X509Helper.Import(rawData, safePasswordHandle, keyStorageFlags);
			}
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x000B6E44 File Offset: 0x000B5044
		[CLSCompliant(false)]
		public X509Certificate(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			if (rawData == null || rawData.Length == 0)
			{
				throw new ArgumentException("Array cannot be empty or null.", "rawData");
			}
			X509Certificate.ValidateKeyStorageFlags(keyStorageFlags);
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				this.impl = X509Helper.Import(rawData, safePasswordHandle, keyStorageFlags);
			}
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x000B6EBC File Offset: 0x000B50BC
		public X509Certificate(IntPtr handle)
		{
			throw new PlatformNotSupportedException("Initializing `X509Certificate` from native handle is not supported.");
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x000B6EE4 File Offset: 0x000B50E4
		internal X509Certificate(X509CertificateImpl impl)
		{
			this.impl = X509Helper.InitFromCertificate(impl);
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x000B6F0E File Offset: 0x000B510E
		public X509Certificate(string fileName) : this(fileName, null, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x000B6F19 File Offset: 0x000B5119
		public X509Certificate(string fileName, string password) : this(fileName, password, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x000B6F24 File Offset: 0x000B5124
		[CLSCompliant(false)]
		public X509Certificate(string fileName, SecureString password) : this(fileName, password, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x000B6F30 File Offset: 0x000B5130
		public X509Certificate(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			X509Certificate.ValidateKeyStorageFlags(keyStorageFlags);
			byte[] rawData = File.ReadAllBytes(fileName);
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				this.impl = X509Helper.Import(rawData, safePasswordHandle, keyStorageFlags);
			}
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x000B6FA8 File Offset: 0x000B51A8
		[CLSCompliant(false)]
		public X509Certificate(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags) : this()
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			X509Certificate.ValidateKeyStorageFlags(keyStorageFlags);
			byte[] rawData = File.ReadAllBytes(fileName);
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				this.impl = X509Helper.Import(rawData, safePasswordHandle, keyStorageFlags);
			}
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x000B7008 File Offset: 0x000B5208
		public X509Certificate(X509Certificate cert)
		{
			if (cert == null)
			{
				throw new ArgumentNullException("cert");
			}
			this.impl = X509Helper.InitFromCertificate(cert);
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x000B7040 File Offset: 0x000B5240
		public X509Certificate(SerializationInfo info, StreamingContext context) : this()
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x000B704D File Offset: 0x000B524D
		public static X509Certificate CreateFromCertFile(string filename)
		{
			return new X509Certificate(filename);
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x000B704D File Offset: 0x000B524D
		public static X509Certificate CreateFromSignedFile(string filename)
		{
			return new X509Certificate(filename);
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x0001B98F File Offset: 0x00019B8F
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x0001B98F File Offset: 0x00019B8F
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000B7055 File Offset: 0x000B5255
		public IntPtr Handle
		{
			get
			{
				if (X509Helper.IsValid(this.impl))
				{
					return this.impl.Handle;
				}
				return IntPtr.Zero;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x000B7078 File Offset: 0x000B5278
		public string Issuer
		{
			get
			{
				this.ThrowIfInvalid();
				string text = this.lazyIssuer;
				if (text == null)
				{
					text = (this.lazyIssuer = this.Impl.Issuer);
				}
				return text;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000B70B0 File Offset: 0x000B52B0
		public string Subject
		{
			get
			{
				this.ThrowIfInvalid();
				string text = this.lazySubject;
				if (text == null)
				{
					text = (this.lazySubject = this.Impl.Subject);
				}
				return text;
			}
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x000B70E7 File Offset: 0x000B52E7
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x000B70F0 File Offset: 0x000B52F0
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Reset();
			}
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x000B70FC File Offset: 0x000B52FC
		public override bool Equals(object obj)
		{
			X509Certificate x509Certificate = obj as X509Certificate;
			return x509Certificate != null && this.Equals(x509Certificate);
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000B711C File Offset: 0x000B531C
		public virtual bool Equals(X509Certificate other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.Impl == null)
			{
				return other.Impl == null;
			}
			if (!this.Issuer.Equals(other.Issuer))
			{
				return false;
			}
			byte[] rawSerialNumber = this.GetRawSerialNumber();
			byte[] rawSerialNumber2 = other.GetRawSerialNumber();
			if (rawSerialNumber.Length != rawSerialNumber2.Length)
			{
				return false;
			}
			for (int i = 0; i < rawSerialNumber.Length; i++)
			{
				if (rawSerialNumber[i] != rawSerialNumber2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000B7186 File Offset: 0x000B5386
		public virtual byte[] Export(X509ContentType contentType)
		{
			return this.Export(contentType, null);
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000B7190 File Offset: 0x000B5390
		public virtual byte[] Export(X509ContentType contentType, string password)
		{
			this.VerifyContentType(contentType);
			if (this.Impl == null)
			{
				throw new CryptographicException(-2147467261);
			}
			byte[] result;
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				result = this.Impl.Export(contentType, safePasswordHandle);
			}
			return result;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000B71EC File Offset: 0x000B53EC
		[CLSCompliant(false)]
		public virtual byte[] Export(X509ContentType contentType, SecureString password)
		{
			this.VerifyContentType(contentType);
			if (this.Impl == null)
			{
				throw new CryptographicException(-2147467261);
			}
			byte[] result;
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				result = this.Impl.Export(contentType, safePasswordHandle);
			}
			return result;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x000B7248 File Offset: 0x000B5448
		public virtual string GetRawCertDataString()
		{
			this.ThrowIfInvalid();
			return this.GetRawCertData().ToHexStringUpper();
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000B725B File Offset: 0x000B545B
		public virtual byte[] GetCertHash()
		{
			this.ThrowIfInvalid();
			return this.GetRawCertHash().CloneByteArray();
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual byte[] GetCertHash(HashAlgorithmName hashAlgorithm)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual bool TryGetCertHash(HashAlgorithmName hashAlgorithm, Span<byte> destination, out int bytesWritten)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000B726E File Offset: 0x000B546E
		public virtual string GetCertHashString()
		{
			this.ThrowIfInvalid();
			return this.GetRawCertHash().ToHexStringUpper();
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x000B7281 File Offset: 0x000B5481
		public virtual string GetCertHashString(HashAlgorithmName hashAlgorithm)
		{
			this.ThrowIfInvalid();
			return this.GetCertHash(hashAlgorithm).ToHexStringUpper();
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000B7298 File Offset: 0x000B5498
		private byte[] GetRawCertHash()
		{
			byte[] result;
			if ((result = this.lazyCertHash) == null)
			{
				result = (this.lazyCertHash = this.Impl.Thumbprint);
			}
			return result;
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000B72C8 File Offset: 0x000B54C8
		public virtual string GetEffectiveDateString()
		{
			return this.GetNotBefore().ToString();
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x000B72E4 File Offset: 0x000B54E4
		public virtual string GetExpirationDateString()
		{
			return this.GetNotAfter().ToString();
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x000B72FF File Offset: 0x000B54FF
		public virtual string GetFormat()
		{
			return "X509";
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000B7306 File Offset: 0x000B5506
		public virtual string GetPublicKeyString()
		{
			return this.GetPublicKey().ToHexStringUpper();
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000B7313 File Offset: 0x000B5513
		public virtual byte[] GetRawCertData()
		{
			this.ThrowIfInvalid();
			return this.Impl.RawData.CloneByteArray();
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000B732C File Offset: 0x000B552C
		public override int GetHashCode()
		{
			if (this.Impl == null)
			{
				return 0;
			}
			byte[] rawCertHash = this.GetRawCertHash();
			int num = 0;
			int num2 = 0;
			while (num2 < rawCertHash.Length && num2 < 4)
			{
				num = (num << 8 | (int)rawCertHash[num2]);
				num2++;
			}
			return num;
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x000B7368 File Offset: 0x000B5568
		public virtual string GetKeyAlgorithm()
		{
			this.ThrowIfInvalid();
			string text = this.lazyKeyAlgorithm;
			if (text == null)
			{
				text = (this.lazyKeyAlgorithm = this.Impl.KeyAlgorithm);
			}
			return text;
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x000B73A0 File Offset: 0x000B55A0
		public virtual byte[] GetKeyAlgorithmParameters()
		{
			this.ThrowIfInvalid();
			byte[] array = this.lazyKeyAlgorithmParameters;
			if (array == null)
			{
				array = (this.lazyKeyAlgorithmParameters = this.Impl.KeyAlgorithmParameters);
			}
			return array.CloneByteArray();
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000B73DC File Offset: 0x000B55DC
		public virtual string GetKeyAlgorithmParametersString()
		{
			this.ThrowIfInvalid();
			return this.GetKeyAlgorithmParameters().ToHexStringUpper();
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x000B73F0 File Offset: 0x000B55F0
		public virtual byte[] GetPublicKey()
		{
			this.ThrowIfInvalid();
			byte[] array = this.lazyPublicKey;
			if (array == null)
			{
				array = (this.lazyPublicKey = this.Impl.PublicKeyValue);
			}
			return array.CloneByteArray();
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000B742C File Offset: 0x000B562C
		public virtual byte[] GetSerialNumber()
		{
			this.ThrowIfInvalid();
			byte[] array = this.GetRawSerialNumber().CloneByteArray();
			Array.Reverse<byte>(array);
			return array;
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x000B7445 File Offset: 0x000B5645
		public virtual string GetSerialNumberString()
		{
			this.ThrowIfInvalid();
			return this.GetRawSerialNumber().ToHexStringUpper();
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000B7458 File Offset: 0x000B5658
		private byte[] GetRawSerialNumber()
		{
			byte[] result;
			if ((result = this.lazySerialNumber) == null)
			{
				result = (this.lazySerialNumber = this.Impl.SerialNumber);
			}
			return result;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000B7487 File Offset: 0x000B5687
		[Obsolete("This method has been deprecated.  Please use the Subject property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetName()
		{
			this.ThrowIfInvalid();
			return this.Impl.LegacySubject;
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000B749A File Offset: 0x000B569A
		[Obsolete("This method has been deprecated.  Please use the Issuer property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetIssuerName()
		{
			this.ThrowIfInvalid();
			return this.Impl.LegacyIssuer;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000B74AD File Offset: 0x000B56AD
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000B74B8 File Offset: 0x000B56B8
		public virtual string ToString(bool fVerbose)
		{
			if (!fVerbose || !X509Helper.IsValid(this.impl))
			{
				return base.ToString();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[Subject]");
			stringBuilder.Append("  ");
			stringBuilder.AppendLine(this.Subject);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Issuer]");
			stringBuilder.Append("  ");
			stringBuilder.AppendLine(this.Issuer);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Serial Number]");
			stringBuilder.Append("  ");
			byte[] serialNumber = this.GetSerialNumber();
			Array.Reverse<byte>(serialNumber);
			stringBuilder.Append(serialNumber.ToHexArrayUpper());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Not Before]");
			stringBuilder.Append("  ");
			stringBuilder.AppendLine(X509Certificate.FormatDate(this.GetNotBefore()));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Not After]");
			stringBuilder.Append("  ");
			stringBuilder.AppendLine(X509Certificate.FormatDate(this.GetNotAfter()));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Thumbprint]");
			stringBuilder.Append("  ");
			stringBuilder.Append(this.GetRawCertHash().ToHexArrayUpper());
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000B7611 File Offset: 0x000B5811
		[ComVisible(false)]
		public virtual void Import(byte[] rawData)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000B7611 File Offset: 0x000B5811
		[ComVisible(false)]
		public virtual void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x000B7611 File Offset: 0x000B5811
		public virtual void Import(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x000B7611 File Offset: 0x000B5811
		[ComVisible(false)]
		public virtual void Import(string fileName)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x000B7611 File Offset: 0x000B5811
		[ComVisible(false)]
		public virtual void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x000B7611 File Offset: 0x000B5811
		public virtual void Import(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000B7620 File Offset: 0x000B5820
		internal DateTime GetNotAfter()
		{
			this.ThrowIfInvalid();
			DateTime dateTime = this.lazyNotAfter;
			if (dateTime == DateTime.MinValue)
			{
				dateTime = (this.lazyNotAfter = this.impl.NotAfter);
			}
			return dateTime;
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000B7660 File Offset: 0x000B5860
		internal DateTime GetNotBefore()
		{
			this.ThrowIfInvalid();
			DateTime dateTime = this.lazyNotBefore;
			if (dateTime == DateTime.MinValue)
			{
				dateTime = (this.lazyNotBefore = this.impl.NotBefore);
			}
			return dateTime;
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000B76A0 File Offset: 0x000B58A0
		protected static string FormatDate(DateTime date)
		{
			CultureInfo cultureInfo = CultureInfo.CurrentCulture;
			if (!cultureInfo.DateTimeFormat.Calendar.IsValidDay(date.Year, date.Month, date.Day, 0))
			{
				if (cultureInfo.DateTimeFormat.Calendar is UmAlQuraCalendar)
				{
					cultureInfo = (cultureInfo.Clone() as CultureInfo);
					cultureInfo.DateTimeFormat.Calendar = new HijriCalendar();
				}
				else
				{
					cultureInfo = CultureInfo.InvariantCulture;
				}
			}
			return date.ToString(cultureInfo);
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000B771C File Offset: 0x000B591C
		internal static void ValidateKeyStorageFlags(X509KeyStorageFlags keyStorageFlags)
		{
			if ((keyStorageFlags & ~(X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserProtected | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet)) != X509KeyStorageFlags.DefaultKeySet)
			{
				throw new ArgumentException("Value of flags is invalid.", "keyStorageFlags");
			}
			X509KeyStorageFlags x509KeyStorageFlags = keyStorageFlags & (X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet);
			if (x509KeyStorageFlags == (X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet))
			{
				throw new ArgumentException(SR.Format("The flags '{0}' may not be specified together.", x509KeyStorageFlags), "keyStorageFlags");
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000B7764 File Offset: 0x000B5964
		private void VerifyContentType(X509ContentType contentType)
		{
			if (contentType != X509ContentType.Cert && contentType != X509ContentType.SerializedCert && contentType != X509ContentType.Pfx)
			{
				throw new CryptographicException("Invalid content type.");
			}
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000B777D File Offset: 0x000B597D
		internal void ImportHandle(X509CertificateImpl impl)
		{
			this.Reset();
			this.impl = impl;
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060031AE RID: 12718 RVA: 0x000B778C File Offset: 0x000B598C
		internal X509CertificateImpl Impl
		{
			get
			{
				return this.impl;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x000B7794 File Offset: 0x000B5994
		internal bool IsValid
		{
			get
			{
				return X509Helper.IsValid(this.impl);
			}
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000B77A1 File Offset: 0x000B59A1
		internal void ThrowIfInvalid()
		{
			X509Helper.ThrowIfContextInvalid(this.impl);
		}

		// Token: 0x04002298 RID: 8856
		private X509CertificateImpl impl;

		// Token: 0x04002299 RID: 8857
		private volatile byte[] lazyCertHash;

		// Token: 0x0400229A RID: 8858
		private volatile byte[] lazySerialNumber;

		// Token: 0x0400229B RID: 8859
		private volatile string lazyIssuer;

		// Token: 0x0400229C RID: 8860
		private volatile string lazySubject;

		// Token: 0x0400229D RID: 8861
		private volatile string lazyKeyAlgorithm;

		// Token: 0x0400229E RID: 8862
		private volatile byte[] lazyKeyAlgorithmParameters;

		// Token: 0x0400229F RID: 8863
		private volatile byte[] lazyPublicKey;

		// Token: 0x040022A0 RID: 8864
		private DateTime lazyNotBefore = DateTime.MinValue;

		// Token: 0x040022A1 RID: 8865
		private DateTime lazyNotAfter = DateTime.MinValue;

		// Token: 0x040022A2 RID: 8866
		internal const X509KeyStorageFlags KeyStorageFlagsAll = X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserProtected | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet;
	}
}
