using System;
using System.IO;
using System.Text;
using Pathfinding.Ionic.Zip;

namespace Pathfinding.Ionic
{
	// Token: 0x0200001D RID: 29
	internal class TypeCriterion : SelectionCriterion
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002B2C File Offset: 0x00000D2C
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00002B3C File Offset: 0x00000D3C
		internal string AttributeString
		{
			get
			{
				return this.ObjectType.ToString();
			}
			set
			{
				if (value.Length != 1 || (value.get_Chars(0) != 'D' && value.get_Chars(0) != 'F'))
				{
					throw new ArgumentException("Specify a single character: either D or F");
				}
				this.ObjectType = value.get_Chars(0);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002B8C File Offset: 0x00000D8C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("type ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002BDC File Offset: 0x00000DDC
		internal override bool Evaluate(string filename)
		{
			bool flag = (this.ObjectType != 'D') ? File.Exists(filename) : Directory.Exists(filename);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002C1C File Offset: 0x00000E1C
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = (this.ObjectType != 'D') ? (!entry.IsDirectory) : entry.IsDirectory;
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x04000048 RID: 72
		private char ObjectType;

		// Token: 0x04000049 RID: 73
		internal ComparisonOperator Operator;
	}
}
