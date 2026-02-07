using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000014 RID: 20
	[PublicAPI]
	[Serializable]
	public class EmptySceneReferenceException : SceneReferenceException
	{
		// Token: 0x0600004B RID: 75 RVA: 0x000028E0 File Offset: 0x00000AE0
		internal EmptySceneReferenceException() : base("The SceneReference is empty (not assigned anything). To fix this, make sure the 'SceneReference' is assigned a valid scene asset.")
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000028ED File Offset: 0x00000AED
		internal EmptySceneReferenceException(Exception inner) : base("The SceneReference is empty (not assigned anything). To fix this, make sure the 'SceneReference' is assigned a valid scene asset.", inner)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000028FB File Offset: 0x00000AFB
		private protected EmptySceneReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0400002E RID: 46
		private const string ExceptionMessage = "The SceneReference is empty (not assigned anything). To fix this, make sure the 'SceneReference' is assigned a valid scene asset.";
	}
}
