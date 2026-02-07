using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200003E RID: 62
	[Serializable]
	public class ReflectedFunction<TResult, T1> : ReflectedFunctionWrapper
	{
		// Token: 0x0600034C RID: 844 RVA: 0x0000966E File Offset: 0x0000786E
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.result,
				this.p1
			};
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00009688 File Offset: 0x00007888
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000969C File Offset: 0x0000789C
		public override object Call()
		{
			return this.result.value = this.call(this.p1.value);
		}

		// Token: 0x040000DA RID: 218
		private FunctionCall<T1, TResult> call;

		// Token: 0x040000DB RID: 219
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000DC RID: 220
		[BlackboardOnly]
		public BBParameter<TResult> result = new BBParameter<TResult>();
	}
}
