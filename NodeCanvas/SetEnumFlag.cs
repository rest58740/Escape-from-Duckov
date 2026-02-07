using System;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;

// Token: 0x02000002 RID: 2
[Category("✫ Blackboard")]
public class SetEnumFlag : ActionTask
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	protected override string info
	{
		get
		{
			return string.Format("{0} {1} for {2} flag", this.Clear.value ? "Clear" : "Set", this.Variable, this.Flag);
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
	protected override void OnExecute()
	{
		int num = (int)this.Variable.value;
		if (this.Clear.value)
		{
			num &= ~(int)this.Flag.value;
		}
		else
		{
			num |= (int)this.Flag.value;
		}
		this.Variable.value = Enum.ToObject(this.Variable.varRef.varType, num);
		base.EndAction();
	}

	// Token: 0x04000001 RID: 1
	[BlackboardOnly]
	[RequiredField]
	public readonly BBObjectParameter Variable = new BBObjectParameter(typeof(Enum));

	// Token: 0x04000002 RID: 2
	public readonly BBObjectParameter Flag = new BBObjectParameter(typeof(Enum));

	// Token: 0x04000003 RID: 3
	public readonly BBParameter<bool> Clear = new BBParameter<bool>();
}
