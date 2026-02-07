using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Pathfinding.Ionic.Zip;

namespace Pathfinding.Ionic
{
	// Token: 0x0200001C RID: 28
	internal class NameCriterion : SelectionCriterion
	{
		// Token: 0x1700000E RID: 14
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000299C File Offset: 0x00000B9C
		internal virtual string MatchingFileSpec
		{
			set
			{
				if (Directory.Exists(value))
				{
					this._MatchingFileSpec = ".\\" + value + "\\*.*";
				}
				else
				{
					this._MatchingFileSpec = value;
				}
				this._regexString = "^" + Regex.Escape(this._MatchingFileSpec).Replace("\\\\\\*\\.\\*", "\\\\([^\\.]+|.*\\.[^\\\\\\.]*)").Replace("\\.\\*", "\\.[^\\\\\\.]*").Replace("\\*", ".*").Replace("\\?", "[^\\\\\\.]") + "$";
				this._re = new Regex(this._regexString, RegexOptions.IgnoreCase);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002A44 File Offset: 0x00000C44
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("name ").Append(EnumUtil.GetDescription(this.Operator)).Append(" '").Append(this._MatchingFileSpec).Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002AA0 File Offset: 0x00000CA0
		internal override bool Evaluate(string filename)
		{
			return this._Evaluate(filename);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002AAC File Offset: 0x00000CAC
		private bool _Evaluate(string fullpath)
		{
			string input = (this._MatchingFileSpec.IndexOf('\\') != -1) ? fullpath : Path.GetFileName(fullpath);
			bool flag = this._re.IsMatch(input);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002AF8 File Offset: 0x00000CF8
		internal override bool Evaluate(ZipEntry entry)
		{
			string fullpath = entry.FileName.Replace("/", "\\");
			return this._Evaluate(fullpath);
		}

		// Token: 0x04000044 RID: 68
		private Regex _re;

		// Token: 0x04000045 RID: 69
		private string _regexString;

		// Token: 0x04000046 RID: 70
		internal ComparisonOperator Operator;

		// Token: 0x04000047 RID: 71
		private string _MatchingFileSpec;
	}
}
