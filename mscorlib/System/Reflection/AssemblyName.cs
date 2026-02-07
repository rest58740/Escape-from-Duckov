using System;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using Mono;
using Mono.Security;
using Mono.Security.Cryptography;

namespace System.Reflection
{
	// Token: 0x020008EB RID: 2283
	[ComDefaultInterface(typeof(_AssemblyName))]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class AssemblyName : ICloneable, ISerializable, IDeserializationCallback, _AssemblyName
	{
		// Token: 0x06004C5D RID: 19549 RVA: 0x000F212F File Offset: 0x000F032F
		public AssemblyName()
		{
			this.versioncompat = AssemblyVersionCompatibility.SameMachine;
		}

		// Token: 0x06004C5E RID: 19550
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ParseAssemblyName(IntPtr name, out MonoAssemblyName aname, out bool is_version_definited, out bool is_token_defined);

		// Token: 0x06004C5F RID: 19551 RVA: 0x000F2140 File Offset: 0x000F0340
		public unsafe AssemblyName(string assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (assemblyName.Length < 1)
			{
				throw new ArgumentException("assemblyName cannot have zero length.");
			}
			using (SafeStringMarshal safeStringMarshal = RuntimeMarshal.MarshalString(assemblyName))
			{
				MonoAssemblyName monoAssemblyName;
				bool addVersion;
				bool defaultToken;
				if (!AssemblyName.ParseAssemblyName(safeStringMarshal.Value, out monoAssemblyName, out addVersion, out defaultToken))
				{
					throw new FileLoadException("The assembly name is invalid.");
				}
				try
				{
					this.FillName(&monoAssemblyName, null, addVersion, false, defaultToken, false);
				}
				finally
				{
					RuntimeMarshal.FreeAssemblyName(ref monoAssemblyName, false);
				}
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x000F21E0 File Offset: 0x000F03E0
		// (set) Token: 0x06004C61 RID: 19553 RVA: 0x000F21E8 File Offset: 0x000F03E8
		public ProcessorArchitecture ProcessorArchitecture
		{
			get
			{
				return this.processor_architecture;
			}
			set
			{
				this.processor_architecture = value;
			}
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x000F21F4 File Offset: 0x000F03F4
		internal AssemblyName(SerializationInfo si, StreamingContext sc)
		{
			this.name = si.GetString("_Name");
			this.codebase = si.GetString("_CodeBase");
			this.version = (Version)si.GetValue("_Version", typeof(Version));
			this.publicKey = (byte[])si.GetValue("_PublicKey", typeof(byte[]));
			this.keyToken = (byte[])si.GetValue("_PublicKeyToken", typeof(byte[]));
			this.hashalg = (AssemblyHashAlgorithm)si.GetValue("_HashAlgorithm", typeof(AssemblyHashAlgorithm));
			this.keypair = (StrongNameKeyPair)si.GetValue("_StrongNameKeyPair", typeof(StrongNameKeyPair));
			this.versioncompat = (AssemblyVersionCompatibility)si.GetValue("_VersionCompatibility", typeof(AssemblyVersionCompatibility));
			this.flags = (AssemblyNameFlags)si.GetValue("_Flags", typeof(AssemblyNameFlags));
			int @int = si.GetInt32("_CultureInfo");
			if (@int != -1)
			{
				this.cultureinfo = new CultureInfo(@int);
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06004C63 RID: 19555 RVA: 0x000F2325 File Offset: 0x000F0525
		// (set) Token: 0x06004C64 RID: 19556 RVA: 0x000F232D File Offset: 0x000F052D
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06004C65 RID: 19557 RVA: 0x000F2336 File Offset: 0x000F0536
		// (set) Token: 0x06004C66 RID: 19558 RVA: 0x000F233E File Offset: 0x000F053E
		public string CodeBase
		{
			get
			{
				return this.codebase;
			}
			set
			{
				this.codebase = value;
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06004C67 RID: 19559 RVA: 0x000F2347 File Offset: 0x000F0547
		public string EscapedCodeBase
		{
			get
			{
				if (this.codebase == null)
				{
					return null;
				}
				return Uri.EscapeString(this.codebase, false, true, true);
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06004C68 RID: 19560 RVA: 0x000F2361 File Offset: 0x000F0561
		// (set) Token: 0x06004C69 RID: 19561 RVA: 0x000F2369 File Offset: 0x000F0569
		public CultureInfo CultureInfo
		{
			get
			{
				return this.cultureinfo;
			}
			set
			{
				this.cultureinfo = value;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x000F2372 File Offset: 0x000F0572
		// (set) Token: 0x06004C6B RID: 19563 RVA: 0x000F237A File Offset: 0x000F057A
		public AssemblyNameFlags Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06004C6C RID: 19564 RVA: 0x000F2384 File Offset: 0x000F0584
		public string FullName
		{
			get
			{
				if (this.name == null)
				{
					return string.Empty;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (char.IsWhiteSpace(this.name[0]))
				{
					stringBuilder.Append("\"" + this.name + "\"");
				}
				else
				{
					stringBuilder.Append(this.name);
				}
				if (this.Version != null)
				{
					stringBuilder.Append(", Version=");
					stringBuilder.Append(this.Version.ToString());
				}
				if (this.cultureinfo != null)
				{
					stringBuilder.Append(", Culture=");
					if (this.cultureinfo.LCID == CultureInfo.InvariantCulture.LCID)
					{
						stringBuilder.Append("neutral");
					}
					else
					{
						stringBuilder.Append(this.cultureinfo.Name);
					}
				}
				byte[] array = this.InternalGetPublicKeyToken();
				if (array != null)
				{
					if (array.Length == 0)
					{
						stringBuilder.Append(", PublicKeyToken=null");
					}
					else
					{
						stringBuilder.Append(", PublicKeyToken=");
						for (int i = 0; i < array.Length; i++)
						{
							stringBuilder.Append(array[i].ToString("x2"));
						}
					}
				}
				if ((this.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					stringBuilder.Append(", Retargetable=Yes");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06004C6D RID: 19565 RVA: 0x000F24C8 File Offset: 0x000F06C8
		// (set) Token: 0x06004C6E RID: 19566 RVA: 0x000F24D0 File Offset: 0x000F06D0
		public AssemblyHashAlgorithm HashAlgorithm
		{
			get
			{
				return this.hashalg;
			}
			set
			{
				this.hashalg = value;
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06004C6F RID: 19567 RVA: 0x000F24D9 File Offset: 0x000F06D9
		// (set) Token: 0x06004C70 RID: 19568 RVA: 0x000F24E1 File Offset: 0x000F06E1
		public StrongNameKeyPair KeyPair
		{
			get
			{
				return this.keypair;
			}
			set
			{
				this.keypair = value;
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06004C71 RID: 19569 RVA: 0x000F24EA File Offset: 0x000F06EA
		// (set) Token: 0x06004C72 RID: 19570 RVA: 0x000F24F4 File Offset: 0x000F06F4
		public Version Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
				if (value == null)
				{
					this.major = (this.minor = (this.build = (this.revision = 0)));
					return;
				}
				this.major = value.Major;
				this.minor = value.Minor;
				this.build = value.Build;
				this.revision = value.Revision;
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06004C73 RID: 19571 RVA: 0x000F2564 File Offset: 0x000F0764
		// (set) Token: 0x06004C74 RID: 19572 RVA: 0x000F256C File Offset: 0x000F076C
		public AssemblyVersionCompatibility VersionCompatibility
		{
			get
			{
				return this.versioncompat;
			}
			set
			{
				this.versioncompat = value;
			}
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x000F2578 File Offset: 0x000F0778
		public override string ToString()
		{
			string fullName = this.FullName;
			if (fullName == null)
			{
				return base.ToString();
			}
			return fullName;
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x000F2597 File Offset: 0x000F0797
		public byte[] GetPublicKey()
		{
			return this.publicKey;
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x000F25A0 File Offset: 0x000F07A0
		public byte[] GetPublicKeyToken()
		{
			if (this.keyToken != null)
			{
				return this.keyToken;
			}
			if (this.publicKey == null)
			{
				return null;
			}
			if (this.publicKey.Length == 0)
			{
				return EmptyArray<byte>.Value;
			}
			if (!this.IsPublicKeyValid)
			{
				throw new SecurityException("The public key is not valid.");
			}
			this.keyToken = this.ComputePublicKeyToken();
			return this.keyToken;
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06004C78 RID: 19576 RVA: 0x000F25FC File Offset: 0x000F07FC
		private bool IsPublicKeyValid
		{
			get
			{
				if (this.publicKey.Length == 16)
				{
					int i = 0;
					int num = 0;
					while (i < this.publicKey.Length)
					{
						num += (int)this.publicKey[i++];
					}
					if (num == 4)
					{
						return true;
					}
				}
				byte b = this.publicKey[0];
				if (b != 0)
				{
					if (b == 6)
					{
						return CryptoConvert.TryImportCapiPublicKeyBlob(this.publicKey, 0);
					}
					if (b != 7)
					{
					}
				}
				else if (this.publicKey.Length > 12 && this.publicKey[12] == 6)
				{
					return CryptoConvert.TryImportCapiPublicKeyBlob(this.publicKey, 12);
				}
				return false;
			}
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x000F2688 File Offset: 0x000F0888
		private byte[] InternalGetPublicKeyToken()
		{
			if (this.keyToken != null)
			{
				return this.keyToken;
			}
			if (this.publicKey == null)
			{
				return null;
			}
			if (this.publicKey.Length == 0)
			{
				return EmptyArray<byte>.Value;
			}
			if (!this.IsPublicKeyValid)
			{
				throw new SecurityException("The public key is not valid.");
			}
			return this.ComputePublicKeyToken();
		}

		// Token: 0x06004C7A RID: 19578
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void get_public_token(byte* token, byte* pubkey, int len);

		// Token: 0x06004C7B RID: 19579 RVA: 0x000F26D8 File Offset: 0x000F08D8
		private unsafe byte[] ComputePublicKeyToken()
		{
			byte[] array2;
			byte[] array = array2 = new byte[8];
			byte* token;
			if (array == null || array2.Length == 0)
			{
				token = null;
			}
			else
			{
				token = &array2[0];
			}
			byte[] array3;
			byte* pubkey;
			if ((array3 = this.publicKey) == null || array3.Length == 0)
			{
				pubkey = null;
			}
			else
			{
				pubkey = &array3[0];
			}
			AssemblyName.get_public_token(token, pubkey, this.publicKey.Length);
			array3 = null;
			array2 = null;
			return array;
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x000F2733 File Offset: 0x000F0933
		public static bool ReferenceMatchesDefinition(AssemblyName reference, AssemblyName definition)
		{
			if (reference == null)
			{
				throw new ArgumentNullException("reference");
			}
			if (definition == null)
			{
				throw new ArgumentNullException("definition");
			}
			return string.Equals(reference.Name, definition.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x000F2763 File Offset: 0x000F0963
		public void SetPublicKey(byte[] publicKey)
		{
			if (publicKey == null)
			{
				this.flags ^= AssemblyNameFlags.PublicKey;
			}
			else
			{
				this.flags |= AssemblyNameFlags.PublicKey;
			}
			this.publicKey = publicKey;
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x000F278D File Offset: 0x000F098D
		public void SetPublicKeyToken(byte[] publicKeyToken)
		{
			this.keyToken = publicKeyToken;
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x000F2798 File Offset: 0x000F0998
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("_Name", this.name);
			info.AddValue("_PublicKey", this.publicKey);
			info.AddValue("_PublicKeyToken", this.keyToken);
			info.AddValue("_CultureInfo", (this.cultureinfo != null) ? this.cultureinfo.LCID : -1);
			info.AddValue("_CodeBase", this.codebase);
			info.AddValue("_Version", this.Version);
			info.AddValue("_HashAlgorithm", this.hashalg);
			info.AddValue("_HashAlgorithmForControl", AssemblyHashAlgorithm.None);
			info.AddValue("_StrongNameKeyPair", this.keypair);
			info.AddValue("_VersionCompatibility", this.versioncompat);
			info.AddValue("_Flags", this.flags);
			info.AddValue("_HashForControl", null);
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x000F289C File Offset: 0x000F0A9C
		public object Clone()
		{
			return new AssemblyName
			{
				name = this.name,
				codebase = this.codebase,
				major = this.major,
				minor = this.minor,
				build = this.build,
				revision = this.revision,
				version = this.version,
				cultureinfo = this.cultureinfo,
				flags = this.flags,
				hashalg = this.hashalg,
				keypair = this.keypair,
				publicKey = this.publicKey,
				keyToken = this.keyToken,
				versioncompat = this.versioncompat,
				processor_architecture = this.processor_architecture
			};
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x000F2962 File Offset: 0x000F0B62
		public void OnDeserialization(object sender)
		{
			this.Version = this.version;
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x000F2970 File Offset: 0x000F0B70
		public unsafe static AssemblyName GetAssemblyName(string assemblyFile)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			AssemblyName assemblyName = new AssemblyName();
			MonoAssemblyName monoAssemblyName;
			string codeBase;
			Assembly.InternalGetAssemblyName(Path.GetFullPath(assemblyFile), out monoAssemblyName, out codeBase);
			try
			{
				assemblyName.FillName(&monoAssemblyName, codeBase, true, false, true, false);
			}
			finally
			{
				RuntimeMarshal.FreeAssemblyName(ref monoAssemblyName, false);
			}
			return assemblyName;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x000479FC File Offset: 0x00045BFC
		void _AssemblyName.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x000479FC File Offset: 0x00045BFC
		void _AssemblyName.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x000479FC File Offset: 0x00045BFC
		void _AssemblyName.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x000479FC File Offset: 0x00045BFC
		void _AssemblyName.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06004C87 RID: 19591 RVA: 0x000F29CC File Offset: 0x000F0BCC
		// (set) Token: 0x06004C88 RID: 19592 RVA: 0x000F29E3 File Offset: 0x000F0BE3
		public string CultureName
		{
			get
			{
				if (this.cultureinfo != null)
				{
					return this.cultureinfo.Name;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.cultureinfo = null;
					return;
				}
				this.cultureinfo = new CultureInfo(value);
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06004C89 RID: 19593 RVA: 0x000F29FC File Offset: 0x000F0BFC
		// (set) Token: 0x06004C8A RID: 19594 RVA: 0x000F2A04 File Offset: 0x000F0C04
		[ComVisible(false)]
		public AssemblyContentType ContentType
		{
			get
			{
				return this.contentType;
			}
			set
			{
				this.contentType = value;
			}
		}

		// Token: 0x06004C8B RID: 19595
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern MonoAssemblyName* GetNativeName(IntPtr assembly_ptr);

		// Token: 0x06004C8C RID: 19596 RVA: 0x000F2A10 File Offset: 0x000F0C10
		internal unsafe void FillName(MonoAssemblyName* native, string codeBase, bool addVersion, bool addPublickey, bool defaultToken, bool assemblyRef)
		{
			this.name = RuntimeMarshal.PtrToUtf8String(native->name);
			this.major = (int)native->major;
			this.minor = (int)native->minor;
			this.build = (int)native->build;
			this.revision = (int)native->revision;
			this.flags = (AssemblyNameFlags)native->flags;
			this.hashalg = (AssemblyHashAlgorithm)native->hash_alg;
			this.versioncompat = AssemblyVersionCompatibility.SameMachine;
			this.processor_architecture = (ProcessorArchitecture)native->arch;
			if (addVersion)
			{
				this.version = new Version(this.major, this.minor, this.build, this.revision);
			}
			this.codebase = codeBase;
			if (native->culture != IntPtr.Zero)
			{
				this.cultureinfo = CultureInfo.CreateCulture(RuntimeMarshal.PtrToUtf8String(native->culture), assemblyRef);
			}
			if (native->public_key != IntPtr.Zero)
			{
				this.publicKey = RuntimeMarshal.DecodeBlobArray(native->public_key);
				this.flags |= AssemblyNameFlags.PublicKey;
			}
			else if (addPublickey)
			{
				this.publicKey = EmptyArray<byte>.Value;
				this.flags |= AssemblyNameFlags.PublicKey;
			}
			if (native->public_key_token.FixedElementField != 0)
			{
				byte[] array = new byte[8];
				int i = 0;
				int num = 0;
				while (i < 8)
				{
					array[i] = (byte)(RuntimeMarshal.AsciHexDigitValue((int)(*(ref native->public_key_token.FixedElementField + num++))) << 4);
					byte[] array2 = array;
					int num2 = i;
					array2[num2] |= (byte)RuntimeMarshal.AsciHexDigitValue((int)(*(ref native->public_key_token.FixedElementField + num++)));
					i++;
				}
				this.keyToken = array;
				return;
			}
			if (defaultToken)
			{
				this.keyToken = EmptyArray<byte>.Value;
			}
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x000F2BAC File Offset: 0x000F0DAC
		internal unsafe static AssemblyName Create(Assembly assembly, bool fillCodebase)
		{
			AssemblyName assemblyName = new AssemblyName();
			MonoAssemblyName* nativeName = AssemblyName.GetNativeName(assembly.MonoAssembly);
			assemblyName.FillName(nativeName, fillCodebase ? assembly.CodeBase : null, true, true, true, false);
			return assemblyName;
		}

		// Token: 0x04003014 RID: 12308
		private string name;

		// Token: 0x04003015 RID: 12309
		private string codebase;

		// Token: 0x04003016 RID: 12310
		private int major;

		// Token: 0x04003017 RID: 12311
		private int minor;

		// Token: 0x04003018 RID: 12312
		private int build;

		// Token: 0x04003019 RID: 12313
		private int revision;

		// Token: 0x0400301A RID: 12314
		private CultureInfo cultureinfo;

		// Token: 0x0400301B RID: 12315
		private AssemblyNameFlags flags;

		// Token: 0x0400301C RID: 12316
		private AssemblyHashAlgorithm hashalg;

		// Token: 0x0400301D RID: 12317
		private StrongNameKeyPair keypair;

		// Token: 0x0400301E RID: 12318
		private byte[] publicKey;

		// Token: 0x0400301F RID: 12319
		private byte[] keyToken;

		// Token: 0x04003020 RID: 12320
		private AssemblyVersionCompatibility versioncompat;

		// Token: 0x04003021 RID: 12321
		private Version version;

		// Token: 0x04003022 RID: 12322
		private ProcessorArchitecture processor_architecture;

		// Token: 0x04003023 RID: 12323
		private AssemblyContentType contentType;
	}
}
