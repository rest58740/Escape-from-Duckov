using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200003B RID: 59
	[Serializable]
	public class ReflectedAction<T1, T2, T3, T4, T5> : ReflectedActionWrapper
	{
		// Token: 0x06000340 RID: 832 RVA: 0x0000942C File Offset: 0x0000762C
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.p1,
				this.p2,
				this.p3,
				this.p4,
				this.p5
			};
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00009461 File Offset: 0x00007661
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00009478 File Offset: 0x00007678
		public override void Call()
		{
			this.call(this.p1.value, this.p2.value, this.p3.value, this.p4.value, this.p5.value);
		}

		// Token: 0x040000CB RID: 203
		private ActionCall<T1, T2, T3, T4, T5> call;

		// Token: 0x040000CC RID: 204
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000CD RID: 205
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000CE RID: 206
		public BBParameter<T3> p3 = new BBParameter<T3>();

		// Token: 0x040000CF RID: 207
		public BBParameter<T4> p4 = new BBParameter<T4>();

		// Token: 0x040000D0 RID: 208
		public BBParameter<T5> p5 = new BBParameter<T5>();
	}
}
