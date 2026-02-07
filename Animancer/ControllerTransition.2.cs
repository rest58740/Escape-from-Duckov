using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200005C RID: 92
	[Serializable]
	public class ControllerTransition : ControllerTransition<ControllerState>, ControllerState.ITransition, ITransition<ControllerState>, ITransition, IHasKey, IPolymorphic, ICopyable<ControllerTransition>
	{
		// Token: 0x0600053F RID: 1343 RVA: 0x0000E37C File Offset: 0x0000C57C
		public unsafe override ControllerState CreateState()
		{
			return base.State = new ControllerState(*base.Controller, *base.ActionsOnStop);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000E3A5 File Offset: 0x0000C5A5
		public ControllerTransition()
		{
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000E3AD File Offset: 0x0000C5AD
		public unsafe ControllerTransition(RuntimeAnimatorController controller)
		{
			*base.Controller = controller;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000E3BD File Offset: 0x0000C5BD
		public static implicit operator ControllerTransition(RuntimeAnimatorController controller)
		{
			return new ControllerTransition(controller);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0000E3C5 File Offset: 0x0000C5C5
		public virtual void CopyFrom(ControllerTransition copyFrom)
		{
			this.CopyFrom(copyFrom);
		}
	}
}
