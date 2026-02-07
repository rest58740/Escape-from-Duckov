using System;
using System.IO;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000BC RID: 188
	internal static class PathUtilities
	{
		// Token: 0x06000529 RID: 1321 RVA: 0x00023F8C File Offset: 0x0002218C
		public static bool HasSubDirectory(this DirectoryInfo parentDir, DirectoryInfo subDir)
		{
			string text = parentDir.FullName.TrimEnd(new char[]
			{
				'\\',
				'/'
			});
			while (subDir != null)
			{
				if (subDir.FullName.TrimEnd(new char[]
				{
					'\\',
					'/'
				}) == text)
				{
					return true;
				}
				subDir = subDir.Parent;
			}
			return false;
		}
	}
}
