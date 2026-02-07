using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class ReflectedFunction<TResult, T1, T2, T3> : ReflectedFunctionWrapper
	{
		// Token: 0x06000354 RID: 852 RVA: 0x00009792 File Offset: 0x00007992
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.result,
				this.p1,
				this.p2,
				this.p3
			};
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000097BE File Offset: 0x000079BE
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000097D4 File Offset: 0x000079D4
		public override object Call()
		{
			return this.result.value = this.call(this.p1.value, this.p2.value, this.p3.value);
		}

		// Token: 0x040000E1 RID: 225
		private FunctionCall<T1, T2, T3, TResult> call;

		// Token: 0x040000E2 RID: 226
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000E3 RID: 227
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000E4 RID: 228
		public BBParameter<T3> p3 = new BBParameter<T3>();

		// Token: 0x040000E5 RID: 229
		[BlackboardOnly]
		public BBParameter<TResult> result = new BBParameter<TResult>();
	}
}
