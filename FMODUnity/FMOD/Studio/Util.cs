using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000EE RID: 238
	public struct Util
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x000053AC File Offset: 0x000035AC
		public static RESULT parseID(string idString, out GUID id)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = Util.FMOD_Studio_ParseID(freeHelper.byteFromStringUTF8(idString), out id);
			}
			return result;
		}

		// Token: 0x060004AD RID: 1197
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_ParseID(byte[] idString, out GUID id);
	}
}
