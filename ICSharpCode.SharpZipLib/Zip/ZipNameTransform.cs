using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000018 RID: 24
	public class ZipNameTransform : INameTransform
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00007BDC File Offset: 0x00005DDC
		public ZipNameTransform()
		{
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public ZipNameTransform(string trimPrefix)
		{
			this.TrimPrefix = trimPrefix;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007BF4 File Offset: 0x00005DF4
		static ZipNameTransform()
		{
			char[] invalidPathChars = Path.InvalidPathChars;
			int num = invalidPathChars.Length + 2;
			ZipNameTransform.InvalidEntryCharsRelaxed = new char[num];
			Array.Copy(invalidPathChars, 0, ZipNameTransform.InvalidEntryCharsRelaxed, 0, invalidPathChars.Length);
			ZipNameTransform.InvalidEntryCharsRelaxed[num - 1] = '*';
			ZipNameTransform.InvalidEntryCharsRelaxed[num - 2] = '?';
			num = invalidPathChars.Length + 4;
			ZipNameTransform.InvalidEntryChars = new char[num];
			Array.Copy(invalidPathChars, 0, ZipNameTransform.InvalidEntryChars, 0, invalidPathChars.Length);
			ZipNameTransform.InvalidEntryChars[num - 1] = ':';
			ZipNameTransform.InvalidEntryChars[num - 2] = '\\';
			ZipNameTransform.InvalidEntryChars[num - 3] = '*';
			ZipNameTransform.InvalidEntryChars[num - 4] = '?';
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007C8C File Offset: 0x00005E8C
		public string TransformDirectory(string name)
		{
			name = this.TransformFile(name);
			if (name.Length > 0)
			{
				if (!name.EndsWith("/"))
				{
					name += "/";
				}
				return name;
			}
			throw new ZipException("Cannot have an empty directory name");
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007CDC File Offset: 0x00005EDC
		public string TransformFile(string name)
		{
			if (name != null)
			{
				string text = name.ToLower();
				if (this.trimPrefix_ != null && text.IndexOf(this.trimPrefix_) == 0)
				{
					name = name.Substring(this.trimPrefix_.Length);
				}
				name = name.Replace("\\", "/");
				name = WindowsPathUtils.DropPathRoot(name);
				while (name.Length > 0 && name[0] == '/')
				{
					name = name.Remove(0, 1);
				}
				while (name.Length > 0 && name[name.Length - 1] == '/')
				{
					name = name.Remove(name.Length - 1, 1);
				}
				for (int i = name.IndexOf("//"); i >= 0; i = name.IndexOf("//"))
				{
					name = name.Remove(i, 1);
				}
				name = ZipNameTransform.MakeValidName(name, '_');
			}
			else
			{
				name = string.Empty;
			}
			return name;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00007DE4 File Offset: 0x00005FE4
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00007DEC File Offset: 0x00005FEC
		public string TrimPrefix
		{
			get
			{
				return this.trimPrefix_;
			}
			set
			{
				this.trimPrefix_ = value;
				if (this.trimPrefix_ != null)
				{
					this.trimPrefix_ = this.trimPrefix_.ToLower();
				}
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007E14 File Offset: 0x00006014
		private static string MakeValidName(string name, char replacement)
		{
			int i = name.IndexOfAny(ZipNameTransform.InvalidEntryChars);
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
						i = name.IndexOfAny(ZipNameTransform.InvalidEntryChars, i + 1);
					}
				}
				name = stringBuilder.ToString();
			}
			if (name.Length > 65535)
			{
				throw new PathTooLongException();
			}
			return name;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007E90 File Offset: 0x00006090
		public static bool IsValidName(string name, bool relaxed)
		{
			bool flag = name != null;
			if (flag)
			{
				if (relaxed)
				{
					flag = (name.IndexOfAny(ZipNameTransform.InvalidEntryCharsRelaxed) < 0);
				}
				else
				{
					flag = (name.IndexOfAny(ZipNameTransform.InvalidEntryChars) < 0 && name.IndexOf('/') != 0);
				}
			}
			return flag;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007EEC File Offset: 0x000060EC
		public static bool IsValidName(string name)
		{
			return name != null && name.IndexOfAny(ZipNameTransform.InvalidEntryChars) < 0 && name.IndexOf('/') != 0;
		}

		// Token: 0x0400011D RID: 285
		private string trimPrefix_;

		// Token: 0x0400011E RID: 286
		private static readonly char[] InvalidEntryChars;

		// Token: 0x0400011F RID: 287
		private static readonly char[] InvalidEntryCharsRelaxed;
	}
}
