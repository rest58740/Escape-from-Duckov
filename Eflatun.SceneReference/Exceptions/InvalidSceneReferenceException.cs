using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000015 RID: 21
	[PublicAPI]
	[Serializable]
	public class InvalidSceneReferenceException : SceneReferenceException
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002905 File Offset: 0x00000B05
		internal InvalidSceneReferenceException() : base("The SceneReference is invalid. This can happen for these reasons:\n1. The SceneReference is assigned an invalid scene, or the assigned asset is not a scene. To fix this, make sure the 'SceneReference' is assigned a valid scene asset.\n2. The scene GUID to path map is outdated. To fix this, you can either manually run the map generator, or enable all generation triggers. It is highly recommended to keep all the generation triggers enabled.")
		{
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002912 File Offset: 0x00000B12
		internal InvalidSceneReferenceException(Exception inner) : base("The SceneReference is invalid. This can happen for these reasons:\n1. The SceneReference is assigned an invalid scene, or the assigned asset is not a scene. To fix this, make sure the 'SceneReference' is assigned a valid scene asset.\n2. The scene GUID to path map is outdated. To fix this, you can either manually run the map generator, or enable all generation triggers. It is highly recommended to keep all the generation triggers enabled.", inner)
		{
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002920 File Offset: 0x00000B20
		private protected InvalidSceneReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0400002F RID: 47
		private const string ExceptionMessage = "The SceneReference is invalid. This can happen for these reasons:\n1. The SceneReference is assigned an invalid scene, or the assigned asset is not a scene. To fix this, make sure the 'SceneReference' is assigned a valid scene asset.\n2. The scene GUID to path map is outdated. To fix this, you can either manually run the map generator, or enable all generation triggers. It is highly recommended to keep all the generation triggers enabled.";
	}
}
