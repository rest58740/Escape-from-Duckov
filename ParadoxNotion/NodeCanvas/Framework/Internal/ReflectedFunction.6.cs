using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public class ReflectedFunction<TResult, T1, T2, T3, T4, T5> : ReflectedFunctionWrapper
	{
		// Token: 0x0600035C RID: 860 RVA: 0x00009936 File Offset: 0x00007B36
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.result,
				this.p1,
				this.p2,
				this.p3,
				this.p4,
				this.p5
			};
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00009974 File Offset: 0x00007B74
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00009988 File Offset: 0x00007B88
		public override object Call()
		{
			return this.result.value = this.call(this.p1.value, this.p2.value, this.p3.value, this.p4.value, this.p5.value);
		}

		// Token: 0x040000EC RID: 236
		private FunctionCall<T1, T2, T3, T4, T5, TResult> call;

		// Token: 0x040000ED RID: 237
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000EE RID: 238
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000EF RID: 239
		public BBParameter<T3> p3 = new BBParameter<T3>();

		// Token: 0x040000F0 RID: 240
		public BBParameter<T4> p4 = new BBParameter<T4>();

		// Token: 0x040000F1 RID: 241
		public BBParameter<T5> p5 = new BBParameter<T5>();

		// Token: 0x040000F2 RID: 242
		[BlackboardOnly]
		public BBParameter<TResult> result = new BBParameter<TResult>();
	}
}
