using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000013 RID: 19
	[PublicAPI]
	[Serializable]
	public class AddressNotUniqueException : SceneReferenceException
	{
		// Token: 0x06000047 RID: 71 RVA: 0x000028A7 File Offset: 0x00000AA7
		private static string MakeExceptionMessage(string address)
		{
			return "The address matches multiple scenes in the Scene GUID to Address Map. Address: " + address + ".\nThrown if a given address matches multiple entries in the Scene GUID to Address Map. This can happen for these reasons:\n1. There are multiple addressable scenes with the same given address. To fix this, make sure there is only one addressable scene with the given address.\n2. The Scene GUID to Address Map is outdated. To fix this, you can either manually run the generator, or enable generation triggers. It is highly recommended to keep all the generation triggers enabled.";
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000028B9 File Offset: 0x00000AB9
		internal AddressNotUniqueException(string address) : base(AddressNotUniqueException.MakeExceptionMessage(address))
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000028C7 File Offset: 0x00000AC7
		internal AddressNotUniqueException(string address, Exception inner) : base(AddressNotUniqueException.MakeExceptionMessage(address), inner)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000028D6 File Offset: 0x00000AD6
		private protected AddressNotUniqueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
