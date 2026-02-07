using System;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200003D RID: 61
	[SpoofAOT]
	[Serializable]
	public class ReflectedFunction<TResult> : ReflectedFunctionWrapper
	{
		// Token: 0x06000348 RID: 840 RVA: 0x00009609 File Offset: 0x00007809
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.result
			};
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000961A File Offset: 0x0000781A
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00009630 File Offset: 0x00007830
		public override object Call()
		{
			return this.result.value = this.call();
		}

		// Token: 0x040000D8 RID: 216
		private FunctionCall<TResult> call;

		// Token: 0x040000D9 RID: 217
		[BlackboardOnly]
		public BBParameter<TResult> result = new BBParameter<TResult>();
	}
}
