using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200005E RID: 94
	[Serializable]
	public class Float1ControllerTransition : ControllerTransition<Float1ControllerState>, Float1ControllerState.ITransition, ITransition<Float1ControllerState>, ITransition, IHasKey, IPolymorphic, ICopyable<Float1ControllerTransition>
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0000E3D6 File Offset: 0x0000C5D6
		public ref string ParameterName
		{
			get
			{
				return ref this._ParameterName;
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000E3DE File Offset: 0x0000C5DE
		public Float1ControllerTransition()
		{
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000E3E6 File Offset: 0x0000C5E6
		public unsafe Float1ControllerTransition(RuntimeAnimatorController controller, string parameterName)
		{
			*base.Controller = controller;
			this._ParameterName = parameterName;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000E400 File Offset: 0x0000C600
		public unsafe override Float1ControllerState CreateState()
		{
			return base.State = new Float1ControllerState(*base.Controller, this._ParameterName, *base.ActionsOnStop);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0000E434 File Offset: 0x0000C634
		public virtual void CopyFrom(Float1ControllerTransition copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._ParameterName = null;
				return;
			}
			this._ParameterName = copyFrom._ParameterName;
		}

		// Token: 0x040000E4 RID: 228
		[SerializeField]
		private string _ParameterName;
	}
}
