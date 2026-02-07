using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000016 RID: 22
	[PublicAPI]
	[Serializable]
	public class SceneNotAddressableException : SceneReferenceException
	{
		// Token: 0x06000051 RID: 81 RVA: 0x0000292A File Offset: 0x00000B2A
		internal SceneNotAddressableException() : base("An addressables-specific operation is attempted on a SceneReference that is assigned a non-addressable scene.\nYou can avoid this exception by making sure the State property is Addressable.")
		{
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002937 File Offset: 0x00000B37
		internal SceneNotAddressableException(Exception inner) : base("An addressables-specific operation is attempted on a SceneReference that is assigned a non-addressable scene.\nYou can avoid this exception by making sure the State property is Addressable.", inner)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002945 File Offset: 0x00000B45
		private protected SceneNotAddressableException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04000030 RID: 48
		private const string ExceptionMessage = "An addressables-specific operation is attempted on a SceneReference that is assigned a non-addressable scene.\nYou can avoid this exception by making sure the State property is Addressable.";
	}
}
