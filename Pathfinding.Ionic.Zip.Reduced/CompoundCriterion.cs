using System;
using System.Text;
using Pathfinding.Ionic.Zip;

namespace Pathfinding.Ionic
{
	// Token: 0x0200001E RID: 30
	internal class CompoundCriterion : SelectionCriterion
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002C68 File Offset: 0x00000E68
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00002C70 File Offset: 0x00000E70
		internal SelectionCriterion Right
		{
			get
			{
				return this._Right;
			}
			set
			{
				this._Right = value;
				if (value == null)
				{
					this.Conjunction = LogicalConjunction.NONE;
				}
				else if (this.Conjunction == LogicalConjunction.NONE)
				{
					this.Conjunction = LogicalConjunction.AND;
				}
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002CA0 File Offset: 0x00000EA0
		internal override bool Evaluate(string filename)
		{
			bool flag = this.Left.Evaluate(filename);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(filename);
				break;
			default:
				throw new ArgumentException("Conjunction");
			}
			return flag;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002D2C File Offset: 0x00000F2C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(").Append((this.Left == null) ? "null" : this.Left.ToString()).Append(" ").Append(this.Conjunction.ToString()).Append(" ").Append((this.Right == null) ? "null" : this.Right.ToString()).Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002DD0 File Offset: 0x00000FD0
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = this.Left.Evaluate(entry);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(entry);
				break;
			}
			return flag;
		}

		// Token: 0x0400004A RID: 74
		internal LogicalConjunction Conjunction;

		// Token: 0x0400004B RID: 75
		internal SelectionCriterion Left;

		// Token: 0x0400004C RID: 76
		private SelectionCriterion _Right;
	}
}
