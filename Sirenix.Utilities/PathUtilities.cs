using System;
using System.IO;
using System.Text;

namespace Sirenix.Utilities
{
	// Token: 0x0200000B RID: 11
	public static class PathUtilities
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00003312 File Offset: 0x00001512
		public static string GetDirectoryName(string x)
		{
			if (x == null)
			{
				return null;
			}
			return Path.GetDirectoryName(x).Replace("\\", "/");
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003330 File Offset: 0x00001530
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

		// Token: 0x0600004C RID: 76 RVA: 0x0000338C File Offset: 0x0000158C
		public static DirectoryInfo FindParentDirectoryWithName(this DirectoryInfo dir, string folderName)
		{
			if (dir.Parent == null)
			{
				return null;
			}
			if (string.Equals(dir.Name, folderName, 3))
			{
				return dir;
			}
			return dir.Parent.FindParentDirectoryWithName(folderName);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000033B8 File Offset: 0x000015B8
		public static bool CanMakeRelative(string absoluteParentPath, string absolutePath)
		{
			if (absoluteParentPath == null)
			{
				throw new ArgumentNullException("absoluteParentPath");
			}
			if (absolutePath == null)
			{
				throw new ArgumentNullException("absoluteParentPath");
			}
			absoluteParentPath = absoluteParentPath.Replace('\\', '/').Trim(new char[]
			{
				'/'
			});
			absolutePath = absolutePath.Replace('\\', '/').Trim(new char[]
			{
				'/'
			});
			return Path.GetPathRoot(absoluteParentPath).Equals(Path.GetPathRoot(absolutePath), 1);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000342C File Offset: 0x0000162C
		public static string MakeRelative(string absoluteParentPath, string absolutePath)
		{
			absoluteParentPath = absoluteParentPath.TrimEnd(new char[]
			{
				'\\',
				'/'
			});
			absolutePath = absolutePath.TrimEnd(new char[]
			{
				'\\',
				'/'
			});
			string[] array = absoluteParentPath.Split(new char[]
			{
				'/',
				'\\'
			});
			string[] array2 = absolutePath.Split(new char[]
			{
				'/',
				'\\'
			});
			int num = -1;
			int num2 = 0;
			while (num2 < array.Length && num2 < array2.Length && array[num2].Equals(array2[num2], 1))
			{
				num = num2;
				num2++;
			}
			if (num == -1)
			{
				throw new InvalidOperationException("No common directory found.");
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (num + 1 < array.Length)
			{
				for (int i = num + 1; i < array.Length; i++)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('/');
					}
					stringBuilder.Append("..");
				}
			}
			for (int j = num + 1; j < array2.Length; j++)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append('/');
				}
				stringBuilder.Append(array2[j]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000354B File Offset: 0x0000174B
		public static bool TryMakeRelative(string absoluteParentPath, string absolutePath, out string relativePath)
		{
			if (PathUtilities.CanMakeRelative(absoluteParentPath, absolutePath))
			{
				relativePath = PathUtilities.MakeRelative(absoluteParentPath, absolutePath);
				return true;
			}
			relativePath = null;
			return false;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003568 File Offset: 0x00001768
		public static string Combine(string a, string b)
		{
			a = a.Replace("\\", "/").TrimEnd(new char[]
			{
				'/'
			});
			b = b.Replace("\\", "/").TrimStart(new char[]
			{
				'/'
			});
			return a + "/" + b;
		}
	}
}
