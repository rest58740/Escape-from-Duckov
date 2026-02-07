using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000018 RID: 24
	[PublicAPI]
	[Serializable]
	public class SceneReferenceException : Exception
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00002983 File Offset: 0x00000B83
		internal SceneReferenceException()
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000298B File Offset: 0x00000B8B
		internal SceneReferenceException(string message) : base("[Eflatun.SceneReference] " + message)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000299E File Offset: 0x00000B9E
		internal SceneReferenceException(string message, Exception inner) : base("[Eflatun.SceneReference] " + message, inner)
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000029B2 File Offset: 0x00000BB2
		private protected SceneReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
