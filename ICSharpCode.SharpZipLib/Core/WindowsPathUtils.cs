using System;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x02000068 RID: 104
	public abstract class WindowsPathUtils
	{
		// Token: 0x0600047C RID: 1148 RVA: 0x00018C9C File Offset: 0x00016E9C
		internal WindowsPathUtils()
		{
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00018CA4 File Offset: 0x00016EA4
		public static string DropPathRoot(string path)
		{
			string text = path;
			if (path != null && path.Length > 0)
			{
				if (path[0] == '\\' || path[0] == '/')
				{
					if (path.Length > 1 && (path[1] == '\\' || path[1] == '/'))
					{
						int num = 2;
						int num2 = 2;
						while (num <= path.Length && ((path[num] != '\\' && path[num] != '/') || --num2 > 0))
						{
							num++;
						}
						num++;
						if (num < path.Length)
						{
							text = path.Substring(num);
						}
						else
						{
							text = string.Empty;
						}
					}
				}
				else if (path.Length > 1 && path[1] == ':')
				{
					int count = 2;
					if (path.Length > 2 && (path[2] == '\\' || path[2] == '/'))
					{
						count = 3;
					}
					text = text.Remove(0, count);
				}
			}
			return text;
		}
	}
}
