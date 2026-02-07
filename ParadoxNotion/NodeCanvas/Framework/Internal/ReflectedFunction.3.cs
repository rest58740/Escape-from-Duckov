using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class ReflectedFunction<TResult, T1, T2> : ReflectedFunctionWrapper
	{
		// Token: 0x06000350 RID: 848 RVA: 0x000096F0 File Offset: 0x000078F0
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.result,
				this.p1,
				this.p2
			};
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00009713 File Offset: 0x00007913
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00009728 File Offset: 0x00007928
		public override object Call()
		{
			return this.result.value = this.call(this.p1.value, this.p2.value);
		}

		// Token: 0x040000DD RID: 221
		private FunctionCall<T1, T2, TResult> call;

		// Token: 0x040000DE RID: 222
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000DF RID: 223
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000E0 RID: 224
		[BlackboardOnly]
		public BBParameter<TResult> result = new BBParameter<TResult>();
	}
}
