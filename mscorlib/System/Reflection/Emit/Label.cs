using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000932 RID: 2354
	[ComVisible(true)]
	[Serializable]
	public readonly struct Label : IEquatable<Label>
	{
		// Token: 0x060050EC RID: 20716 RVA: 0x000FD768 File Offset: 0x000FB968
		internal Label(int val)
		{
			this.label = val;
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x000FD774 File Offset: 0x000FB974
		public override bool Equals(object obj)
		{
			bool flag = obj is Label;
			if (flag)
			{
				Label label = (Label)obj;
				flag = (this.label == label.label);
			}
			return flag;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x000FD7A5 File Offset: 0x000FB9A5
		public bool Equals(Label obj)
		{
			return this.label == obj.label;
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x000FD7B5 File Offset: 0x000FB9B5
		public static bool operator ==(Label a, Label b)
		{
			return a.Equals(b);
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x000FD7BF File Offset: 0x000FB9BF
		public static bool operator !=(Label a, Label b)
		{
			return !(a == b);
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x000FD7CB File Offset: 0x000FB9CB
		public override int GetHashCode()
		{
			return this.label.GetHashCode();
		}

		// Token: 0x040031B2 RID: 12722
		internal readonly int label;
	}
}
