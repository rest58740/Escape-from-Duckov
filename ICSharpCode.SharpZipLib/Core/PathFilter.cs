using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x0200004F RID: 79
	public class PathFilter : IScanFilter
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x00015E88 File Offset: 0x00014088
		public PathFilter(string filter)
		{
			this.nameFilter_ = new NameFilter(filter);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00015E9C File Offset: 0x0001409C
		public virtual bool IsMatch(string name)
		{
			bool result = false;
			if (name != null)
			{
				string name2 = (name.Length <= 0) ? string.Empty : Path.GetFullPath(name);
				result = this.nameFilter_.IsMatch(name2);
			}
			return result;
		}

		// Token: 0x040002AB RID: 683
		private NameFilter nameFilter_;
	}
}
