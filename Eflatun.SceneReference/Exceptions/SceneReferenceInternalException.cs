using System;
using System.Runtime.Serialization;

namespace Eflatun.SceneReference.Exceptions
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	internal class SceneReferenceInternalException : SceneReferenceException
	{
		// Token: 0x0600005C RID: 92 RVA: 0x000029BC File Offset: 0x00000BBC
		public static SceneReferenceInternalException InvalidGuid(string location, string guid)
		{
			return new SceneReferenceInternalException(location, "GUID is invalid. GUID: \"" + guid + "\"");
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000029D4 File Offset: 0x00000BD4
		public static SceneReferenceInternalException ExceptionImpossible<TException>(string location, TException exception) where TException : Exception
		{
			return new SceneReferenceInternalException(location, "Exception impossible. Exception: \n" + exception.ToString());
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000029F1 File Offset: 0x00000BF1
		public static Exception EditorCode(string location, string description)
		{
			return new SceneReferenceInternalException(location, "Editor code. " + description);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002A04 File Offset: 0x00000C04
		private SceneReferenceInternalException(string location, string info) : base("If you are seeing this, something has gone wrong internally. Please open an issue on Github (https://github.com/starikcetin/Eflatun.SceneReference/issues) and include the following information:\nLocation: " + location + "\nInfo: " + info)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002A1D File Offset: 0x00000C1D
		private protected SceneReferenceInternalException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
