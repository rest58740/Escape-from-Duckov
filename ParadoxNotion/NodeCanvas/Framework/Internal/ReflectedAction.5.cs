using System;
using ParadoxNotion;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public class ReflectedAction<T1, T2, T3, T4> : ReflectedActionWrapper
	{
		// Token: 0x0600033C RID: 828 RVA: 0x0000937F File Offset: 0x0000757F
		public override BBParameter[] GetVariables()
		{
			return new BBParameter[]
			{
				this.p1,
				this.p2,
				this.p3,
				this.p4
			};
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000093AB File Offset: 0x000075AB
		public override void Init(object instance)
		{
			this.call = base.GetMethod().RTCreateDelegate(instance);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000093BF File Offset: 0x000075BF
		public override void Call()
		{
			this.call(this.p1.value, this.p2.value, this.p3.value, this.p4.value);
		}

		// Token: 0x040000C6 RID: 198
		private ActionCall<T1, T2, T3, T4> call;

		// Token: 0x040000C7 RID: 199
		public BBParameter<T1> p1 = new BBParameter<T1>();

		// Token: 0x040000C8 RID: 200
		public BBParameter<T2> p2 = new BBParameter<T2>();

		// Token: 0x040000C9 RID: 201
		public BBParameter<T3> p3 = new BBParameter<T3>();

		// Token: 0x040000CA RID: 202
		public BBParameter<T4> p4 = new BBParameter<T4>();
	}
}
