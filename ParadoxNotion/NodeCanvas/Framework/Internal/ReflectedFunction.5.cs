using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000041 RID: 65
	[Serializable]
	public class ReflectedFunction<TResult, T1, T2, T3, T4> : ReflectedFunctionWrapper
	{
		// Token: 0x06000358 RID: 856 RVA: 0x00009854 File Offset: 0x00007A54
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.result,
				this.p1,
				this.p2,
				this.p3,
				this.p4
			};
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00009889 File Offset: 0x00007A89
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000098A0 File Offset: 0x00007AA0
		public override object Call()
		{
			return this.result.value = this.call(this.p1.value, this.p2.value, this.p3.value, this.p4.value);
		}

		// Token: 0x040000E6 RID: 230
		private FunctionCall<T1, T2, T3, T4, TResult> call;

		// Token: 0x040000E7 RID: 231
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000E8 RID: 232
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000E9 RID: 233
		public BBParameter<T3> p3 = new BBParameter<T3>();

		// Token: 0x040000EA RID: 234
		public BBParameter<T4> p4 = new BBParameter<T4>();

		// Token: 0x040000EB RID: 235
		[BlackboardOnly]
		public BBParameter<TResult> result = new BBParameter<TResult>();
	}
}
