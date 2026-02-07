using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public class ReflectedAction<T1, T2, T3, T4, T5, T6> : ReflectedActionWrapper
	{
		// Token: 0x06000344 RID: 836 RVA: 0x00009506 File Offset: 0x00007706
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.p1,
				this.p2,
				this.p3,
				this.p4,
				this.p5,
				this.p6
			};
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00009544 File Offset: 0x00007744
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00009558 File Offset: 0x00007758
		public override void Call()
		{
			this.call(this.p1.value, this.p2.value, this.p3.value, this.p4.value, this.p5.value, this.p6.value);
		}

		// Token: 0x040000D1 RID: 209
		private ActionCall<T1, T2, T3, T4, T5, T6> call;

		// Token: 0x040000D2 RID: 210
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000D3 RID: 211
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000D4 RID: 212
		public BBParameter<T3> p3 = new BBParameter<T3>();

		// Token: 0x040000D5 RID: 213
		public BBParameter<T4> p4 = new BBParameter<T4>();

		// Token: 0x040000D6 RID: 214
		public BBParameter<T5> p5 = new BBParameter<T5>();

		// Token: 0x040000D7 RID: 215
		public BBParameter<T6> p6 = new BBParameter<T6>();
	}
}
