using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public class ReflectedFunction<TResult, T1, T2, T3, T4, T5, T6> : ReflectedFunctionWrapper
	{
		// Token: 0x06000360 RID: 864 RVA: 0x00009A44 File Offset: 0x00007C44
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.result,
				this.p1,
				this.p2,
				this.p3,
				this.p4,
				this.p5,
				this.p6
			};
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00009A96 File Offset: 0x00007C96
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00009AAC File Offset: 0x00007CAC
		public override object Call()
		{
			return this.result.value = this.call(this.p1.value, this.p2.value, this.p3.value, this.p4.value, this.p5.value, this.p6.value);
		}

		// Token: 0x040000F3 RID: 243
		private FunctionCall<T1, T2, T3, T4, T5, T6, TResult> call;

		// Token: 0x040000F4 RID: 244
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000F5 RID: 245
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000F6 RID: 246
		public BBParameter<T3> p3 = new BBParameter<T3>();

		// Token: 0x040000F7 RID: 247
		public BBParameter<T4> p4 = new BBParameter<T4>();

		// Token: 0x040000F8 RID: 248
		public BBParameter<T5> p5 = new BBParameter<T5>();

		// Token: 0x040000F9 RID: 249
		public BBParameter<T6> p6 = new BBParameter<T6>();

		// Token: 0x040000FA RID: 250
		[BlackboardOnly]
		public BBParameter<TResult> result = new BBParameter<TResult>();
	}
}
