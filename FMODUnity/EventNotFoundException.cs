using System;
using FMOD;

namespace FMODUnity
{
	// Token: 0x02000114 RID: 276
	public class EventNotFoundException : Exception
	{
		// Token: 0x0600075B RID: 1883 RVA: 0x0000A3E4 File Offset: 0x000085E4
		public EventNotFoundException(string path) : base("[FMOD] Event not found: '" + path + "'")
		{
			this.Path = path;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0000A404 File Offset: 0x00008604
		public EventNotFoundException(GUID guid)
		{
			string str = "[FMOD] Event not found: ";
			GUID guid2 = guid;
			base..ctor(str + guid2.ToString());
			this.Guid = guid;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0000A437 File Offset: 0x00008637
		public EventNotFoundException(EventReference eventReference) : base("[FMOD] Event not found: " + eventReference.ToString())
		{
			this.Guid = eventReference.Guid;
		}

		// Token: 0x040005B5 RID: 1461
		public GUID Guid;

		// Token: 0x040005B6 RID: 1462
		public string Path;
	}
}
