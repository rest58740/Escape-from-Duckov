using System;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;

// Token: 0x02000003 RID: 3
[Category("✫ Blackboard")]
public class CheckEnumFlag : ConditionTask
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000004 RID: 4 RVA: 0x0000213C File Offset: 0x0000033C
	protected override string info
	{
		get
		{
			return string.Format("{0} has {1} flag", this.Variable, this.Flag);
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002154 File Offset: 0x00000354
	protected override bool OnCheck()
	{
		return ((Enum)this.Variable.value).HasFlag((Enum)this.Flag.value);
	}

	// Token: 0x04000004 RID: 4
	[BlackboardOnly]
	[RequiredField]
	public readonly BBObjectParameter Variable = new BBObjectParameter(typeof(Enum));

	// Token: 0x04000005 RID: 5
	public readonly BBObjectParameter Flag = new BBObjectParameter(typeof(Enum));
}
