using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000012 RID: 18
	[PublicAPI]
	[Serializable]
	public class AddressNotFoundException : SceneReferenceException
	{
		// Token: 0x06000043 RID: 67 RVA: 0x0000286E File Offset: 0x00000A6E
		private static string MakeExceptionMessage(string address)
		{
			return "The address is not found in the Scene GUID to Address Map. Address: " + address + ".\nThis can happen for these reasons:\n1. The asset with the given address either doesn't exist or is not a scene. To fix this, make sure you provide the address of a valid addressable scene.\n2. The Scene GUID to Address Map is outdated. To fix this, you can either manually run the generator, or enable generation triggers. It is highly recommended to keep all the generation triggers enabled.";
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002880 File Offset: 0x00000A80
		internal AddressNotFoundException(string address) : base(AddressNotFoundException.MakeExceptionMessage(address))
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000288E File Offset: 0x00000A8E
		internal AddressNotFoundException(string address, Exception inner) : base(AddressNotFoundException.MakeExceptionMessage(address), inner)
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000289D File Offset: 0x00000A9D
		private protected AddressNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
