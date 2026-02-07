using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Permissions;
using Mono.Security;
using Mono.Security.Cryptography;

namespace System.Reflection
{
	// Token: 0x02000905 RID: 2309
	[ComVisible(true)]
	[Serializable]
	public class StrongNameKeyPair : ISerializable, IDeserializationCallback
	{
		// Token: 0x06004E44 RID: 20036 RVA: 0x000F5DEE File Offset: 0x000F3FEE
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public StrongNameKeyPair(byte[] keyPairArray)
		{
			if (keyPairArray == null)
			{
				throw new ArgumentNullException("keyPairArray");
			}
			this.LoadKey(keyPairArray);
			this.GetRSA();
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x000F5E14 File Offset: 0x000F4014
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public StrongNameKeyPair(FileStream keyPairFile)
		{
			if (keyPairFile == null)
			{
				throw new ArgumentNullException("keyPairFile");
			}
			byte[] array = new byte[keyPairFile.Length];
			keyPairFile.Read(array, 0, array.Length);
			this.LoadKey(array);
			this.GetRSA();
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x000F5E5C File Offset: 0x000F405C
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public StrongNameKeyPair(string keyPairContainer)
		{
			if (keyPairContainer == null)
			{
				throw new ArgumentNullException("keyPairContainer");
			}
			this._keyPairContainer = keyPairContainer;
			this.GetRSA();
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x000F5E80 File Offset: 0x000F4080
		protected StrongNameKeyPair(SerializationInfo info, StreamingContext context)
		{
			this._publicKey = (byte[])info.GetValue("_publicKey", typeof(byte[]));
			this._keyPairContainer = info.GetString("_keyPairContainer");
			this._keyPairExported = info.GetBoolean("_keyPairExported");
			this._keyPairArray = (byte[])info.GetValue("_keyPairArray", typeof(byte[]));
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x000F5EF8 File Offset: 0x000F40F8
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_publicKey", this._publicKey, typeof(byte[]));
			info.AddValue("_keyPairContainer", this._keyPairContainer);
			info.AddValue("_keyPairExported", this._keyPairExported);
			info.AddValue("_keyPairArray", this._keyPairArray, typeof(byte[]));
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x00004BF9 File Offset: 0x00002DF9
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x000F5F60 File Offset: 0x000F4160
		private RSA GetRSA()
		{
			if (this._rsa != null)
			{
				return this._rsa;
			}
			if (this._keyPairArray != null)
			{
				try
				{
					this._rsa = CryptoConvert.FromCapiKeyBlob(this._keyPairArray);
					goto IL_5A;
				}
				catch
				{
					this._keyPairArray = null;
					goto IL_5A;
				}
			}
			if (this._keyPairContainer != null)
			{
				this._rsa = new RSACryptoServiceProvider(new CspParameters
				{
					KeyContainerName = this._keyPairContainer
				});
			}
			IL_5A:
			return this._rsa;
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x000F5FE0 File Offset: 0x000F41E0
		private void LoadKey(byte[] key)
		{
			try
			{
				if (key.Length == 16)
				{
					int i = 0;
					int num = 0;
					while (i < key.Length)
					{
						num += (int)key[i++];
					}
					if (num == 4)
					{
						this._publicKey = (byte[])key.Clone();
					}
				}
				else
				{
					this._keyPairArray = key;
				}
			}
			catch
			{
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06004E4C RID: 20044 RVA: 0x000F6040 File Offset: 0x000F4240
		public byte[] PublicKey
		{
			get
			{
				if (this._publicKey == null)
				{
					RSA rsa = this.GetRSA();
					if (rsa == null)
					{
						throw new ArgumentException("invalid keypair");
					}
					byte[] array = CryptoConvert.ToCapiKeyBlob(rsa, false);
					this._publicKey = new byte[array.Length + 12];
					this._publicKey[0] = 0;
					this._publicKey[1] = 36;
					this._publicKey[2] = 0;
					this._publicKey[3] = 0;
					this._publicKey[4] = 4;
					this._publicKey[5] = 128;
					this._publicKey[6] = 0;
					this._publicKey[7] = 0;
					int num = array.Length;
					this._publicKey[8] = (byte)(num % 256);
					this._publicKey[9] = (byte)(num / 256);
					this._publicKey[10] = 0;
					this._publicKey[11] = 0;
					Buffer.BlockCopy(array, 0, this._publicKey, 12, array.Length);
				}
				return this._publicKey;
			}
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x000F6124 File Offset: 0x000F4324
		internal StrongName StrongName()
		{
			RSA rsa = this.GetRSA();
			if (rsa != null)
			{
				return new StrongName(rsa);
			}
			if (this._publicKey != null)
			{
				return new StrongName(this._publicKey);
			}
			return null;
		}

		// Token: 0x0400307E RID: 12414
		private byte[] _publicKey;

		// Token: 0x0400307F RID: 12415
		private string _keyPairContainer;

		// Token: 0x04003080 RID: 12416
		private bool _keyPairExported;

		// Token: 0x04003081 RID: 12417
		private byte[] _keyPairArray;

		// Token: 0x04003082 RID: 12418
		[NonSerialized]
		private RSA _rsa;
	}
}
