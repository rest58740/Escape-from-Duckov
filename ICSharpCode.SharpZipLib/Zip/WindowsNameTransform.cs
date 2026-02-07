using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200005A RID: 90
	public class WindowsNameTransform : INameTransform
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x00016E18 File Offset: 0x00015018
		public WindowsNameTransform(string baseDirectory)
		{
			if (baseDirectory == null)
			{
				throw new ArgumentNullException("baseDirectory", "Directory name is invalid");
			}
			this.BaseDirectory = baseDirectory;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00016E48 File Offset: 0x00015048
		public WindowsNameTransform()
		{
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00016E58 File Offset: 0x00015058
		static WindowsNameTransform()
		{
			char[] invalidPathChars = Path.InvalidPathChars;
			int num = invalidPathChars.Length + 3;
			WindowsNameTransform.InvalidEntryChars = new char[num];
			Array.Copy(invalidPathChars, 0, WindowsNameTransform.InvalidEntryChars, 0, invalidPathChars.Length);
			WindowsNameTransform.InvalidEntryChars[num - 1] = '*';
			WindowsNameTransform.InvalidEntryChars[num - 2] = '?';
			WindowsNameTransform.InvalidEntryChars[num - 3] = ':';
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00016EB0 File Offset: 0x000150B0
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00016EB8 File Offset: 0x000150B8
		public string BaseDirectory
		{
			get
			{
				return this._baseDirectory;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._baseDirectory = Path.GetFullPath(value);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00016ED8 File Offset: 0x000150D8
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x00016EE0 File Offset: 0x000150E0
		public bool TrimIncomingPaths
		{
			get
			{
				return this._trimIncomingPaths;
			}
			set
			{
				this._trimIncomingPaths = value;
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00016EEC File Offset: 0x000150EC
		public string TransformDirectory(string name)
		{
			name = this.TransformFile(name);
			if (name.Length > 0)
			{
				while (name.EndsWith("\\"))
				{
					name = name.Remove(name.Length - 1, 1);
				}
				return name;
			}
			throw new ZipException("Cannot have an empty directory name");
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00016F48 File Offset: 0x00015148
		public string TransformFile(string name)
		{
			if (name != null)
			{
				name = WindowsNameTransform.MakeValidName(name, this._replacementChar);
				if (this._trimIncomingPaths)
				{
					name = Path.GetFileName(name);
				}
				if (this._baseDirectory != null)
				{
					name = Path.Combine(this._baseDirectory, name);
				}
			}
			else
			{
				name = string.Empty;
			}
			return name;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00016FA4 File Offset: 0x000151A4
		public static bool IsValidName(string name)
		{
			return name != null && name.Length <= 260 && string.Compare(name, WindowsNameTransform.MakeValidName(name, '_')) == 0;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00016FE0 File Offset: 0x000151E0
		public static string MakeValidName(string name, char replacement)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			name = WindowsPathUtils.DropPathRoot(name.Replace("/", "\\"));
			while (name.Length > 0 && name[0] == '\\')
			{
				name = name.Remove(0, 1);
			}
			while (name.Length > 0 && name[name.Length - 1] == '\\')
			{
				name = name.Remove(name.Length - 1, 1);
			}
			int i;
			for (i = name.IndexOf("\\\\"); i >= 0; i = name.IndexOf("\\\\"))
			{
				name = name.Remove(i, 1);
			}
			i = name.IndexOfAny(WindowsNameTransform.InvalidEntryChars);
			if (i >= 0)
			{
				StringBuilder stringBuilder = new StringBuilder(name);
				while (i >= 0)
				{
					stringBuilder[i] = replacement;
					if (i >= name.Length)
					{
						i = -1;
					}
					else
					{
						i = name.IndexOfAny(WindowsNameTransform.InvalidEntryChars, i + 1);
					}
				}
				name = stringBuilder.ToString();
			}
			if (name.Length > 260)
			{
				throw new PathTooLongException();
			}
			return name;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00017114 File Offset: 0x00015314
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0001711C File Offset: 0x0001531C
		public char Replacement
		{
			get
			{
				return this._replacementChar;
			}
			set
			{
				for (int i = 0; i < WindowsNameTransform.InvalidEntryChars.Length; i++)
				{
					if (WindowsNameTransform.InvalidEntryChars[i] == value)
					{
						throw new ArgumentException("invalid path character");
					}
				}
				if (value == '\\' || value == '/')
				{
					throw new ArgumentException("invalid replacement character");
				}
				this._replacementChar = value;
			}
		}

		// Token: 0x040002C4 RID: 708
		private const int MaxPath = 260;

		// Token: 0x040002C5 RID: 709
		private string _baseDirectory;

		// Token: 0x040002C6 RID: 710
		private bool _trimIncomingPaths;

		// Token: 0x040002C7 RID: 711
		private char _replacementChar = '_';

		// Token: 0x040002C8 RID: 712
		private static readonly char[] InvalidEntryChars;
	}
}
