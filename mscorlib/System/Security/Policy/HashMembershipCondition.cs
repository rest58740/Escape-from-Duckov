using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Mono.Security.Cryptography;

namespace System.Security.Policy
{
	// Token: 0x02000413 RID: 1043
	[ComVisible(true)]
	[Serializable]
	public sealed class HashMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IDeserializationCallback, ISerializable
	{
		// Token: 0x06002AAC RID: 10924 RVA: 0x0009A3FB File Offset: 0x000985FB
		internal HashMembershipCondition()
		{
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x0009A40C File Offset: 0x0009860C
		public HashMembershipCondition(HashAlgorithm hashAlg, byte[] value)
		{
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.hash_algorithm = hashAlg;
			this.hash_value = (byte[])value.Clone();
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06002AAE RID: 10926 RVA: 0x0009A45A File Offset: 0x0009865A
		// (set) Token: 0x06002AAF RID: 10927 RVA: 0x0009A475 File Offset: 0x00098675
		public HashAlgorithm HashAlgorithm
		{
			get
			{
				if (this.hash_algorithm == null)
				{
					this.hash_algorithm = new SHA1Managed();
				}
				return this.hash_algorithm;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("HashAlgorithm");
				}
				this.hash_algorithm = value;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06002AB0 RID: 10928 RVA: 0x0009A48C File Offset: 0x0009868C
		// (set) Token: 0x06002AB1 RID: 10929 RVA: 0x0009A4B6 File Offset: 0x000986B6
		public byte[] HashValue
		{
			get
			{
				if (this.hash_value == null)
				{
					throw new ArgumentException(Locale.GetText("No HashValue available."));
				}
				return (byte[])this.hash_value.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("HashValue");
				}
				this.hash_value = (byte[])value.Clone();
			}
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x0009A4D8 File Offset: 0x000986D8
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				Hash hash = obj as Hash;
				if (hash != null)
				{
					if (this.Compare(this.hash_value, hash.GenerateHash(this.hash_algorithm)))
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x0009A527 File Offset: 0x00098727
		public IMembershipCondition Copy()
		{
			return new HashMembershipCondition(this.hash_algorithm, this.hash_value);
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x0009A53C File Offset: 0x0009873C
		public override bool Equals(object o)
		{
			HashMembershipCondition hashMembershipCondition = o as HashMembershipCondition;
			return hashMembershipCondition != null && hashMembershipCondition.HashAlgorithm == this.hash_algorithm && this.Compare(this.hash_value, hashMembershipCondition.hash_value);
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x0009A577 File Offset: 0x00098777
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x0009A580 File Offset: 0x00098780
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = MembershipConditionHelper.Element(typeof(HashMembershipCondition), this.version);
			securityElement.AddAttribute("HashValue", CryptoConvert.ToHex(this.HashValue));
			securityElement.AddAttribute("HashAlgorithm", this.hash_algorithm.GetType().FullName);
			return securityElement;
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x0009A5D3 File Offset: 0x000987D3
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x0009A5E0 File Offset: 0x000987E0
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
			this.hash_value = CryptoConvert.FromHex(e.Attribute("HashValue"));
			string text = e.Attribute("HashAlgorithm");
			this.hash_algorithm = ((text == null) ? null : HashAlgorithm.Create(text));
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x0009A63C File Offset: 0x0009883C
		public override int GetHashCode()
		{
			int num = this.hash_algorithm.GetType().GetHashCode();
			if (this.hash_value != null)
			{
				foreach (byte b in this.hash_value)
				{
					num ^= (int)b;
				}
			}
			return num;
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x0009A680 File Offset: 0x00098880
		public override string ToString()
		{
			Type type = this.HashAlgorithm.GetType();
			return string.Format("Hash - {0} {1} = {2}", type.FullName, type.Assembly, CryptoConvert.ToHex(this.HashValue));
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x0009A6BC File Offset: 0x000988BC
		private bool Compare(byte[] expected, byte[] actual)
		{
			if (expected.Length != actual.Length)
			{
				return false;
			}
			int num = expected.Length;
			for (int i = 0; i < num; i++)
			{
				if (expected[i] != actual[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[MonoTODO("fx 2.0")]
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[MonoTODO("fx 2.0")]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x04001F9C RID: 8092
		private readonly int version = 1;

		// Token: 0x04001F9D RID: 8093
		private HashAlgorithm hash_algorithm;

		// Token: 0x04001F9E RID: 8094
		private byte[] hash_value;
	}
}
