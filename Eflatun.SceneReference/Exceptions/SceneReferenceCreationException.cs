using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000017 RID: 23
	[PublicAPI]
	[Serializable]
	public class SceneReferenceCreationException : SceneReferenceException
	{
		// Token: 0x06000054 RID: 84 RVA: 0x0000294F File Offset: 0x00000B4F
		private static string PrefixMessage(string message)
		{
			return "An exception occured during the creation of a SceneReference: " + message;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000295C File Offset: 0x00000B5C
		internal SceneReferenceCreationException(string message) : base(SceneReferenceCreationException.PrefixMessage(message))
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000296A File Offset: 0x00000B6A
		internal SceneReferenceCreationException(string message, Exception inner) : base(SceneReferenceCreationException.PrefixMessage(message), inner)
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002979 File Offset: 0x00000B79
		private protected SceneReferenceCreationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
