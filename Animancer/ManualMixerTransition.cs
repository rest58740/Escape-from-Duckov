using System;

namespace Animancer
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	public class ManualMixerTransition : ManualMixerTransition<ManualMixerState>, ManualMixerState.ITransition, ITransition<ManualMixerState>, ITransition, IHasKey, IPolymorphic, ICopyable<ManualMixerTransition>
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		public override ManualMixerState CreateState()
		{
			base.State = new ManualMixerState();
			this.InitializeState();
			return base.State;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0000E7DD File Offset: 0x0000C9DD
		public virtual void CopyFrom(ManualMixerTransition copyFrom)
		{
			this.CopyFrom(copyFrom);
		}
	}
}
