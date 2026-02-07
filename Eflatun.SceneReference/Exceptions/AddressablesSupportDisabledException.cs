using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000011 RID: 17
	[PublicAPI]
	[Serializable]
	public class AddressablesSupportDisabledException : SceneReferenceException
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002849 File Offset: 0x00000A49
		internal AddressablesSupportDisabledException() : base("An operation that requires addressables support is attempted while addressables support is disabled. To fix it, make sure addressables support is enabled.")
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002856 File Offset: 0x00000A56
		internal AddressablesSupportDisabledException(Exception inner) : base("An operation that requires addressables support is attempted while addressables support is disabled. To fix it, make sure addressables support is enabled.", inner)
		{
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002864 File Offset: 0x00000A64
		private protected AddressablesSupportDisabledException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0400002D RID: 45
		private const string ExceptionMessage = "An operation that requires addressables support is attempted while addressables support is disabled. To fix it, make sure addressables support is enabled.";
	}
}
