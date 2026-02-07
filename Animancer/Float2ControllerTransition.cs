using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000060 RID: 96
	[Serializable]
	public class Float2ControllerTransition : ControllerTransition<Float2ControllerState>, Float2ControllerState.ITransition, ITransition<Float2ControllerState>, ITransition, IHasKey, IPolymorphic, ICopyable<Float2ControllerTransition>
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0000E45C File Offset: 0x0000C65C
		public ref string ParameterNameX
		{
			get
			{
				return ref this._ParameterNameX;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0000E464 File Offset: 0x0000C664
		public ref string ParameterNameY
		{
			get
			{
				return ref this._ParameterNameY;
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0000E46C File Offset: 0x0000C66C
		public Float2ControllerTransition()
		{
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000E474 File Offset: 0x0000C674
		public unsafe Float2ControllerTransition(RuntimeAnimatorController controller, string parameterNameX, string parameterNameY)
		{
			*base.Controller = controller;
			this._ParameterNameX = parameterNameX;
			this._ParameterNameY = parameterNameY;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0000E494 File Offset: 0x0000C694
		public unsafe override Float2ControllerState CreateState()
		{
			return base.State = new Float2ControllerState(*base.Controller, this._ParameterNameX, this._ParameterNameY, *base.ActionsOnStop);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000E4D3 File Offset: 0x0000C6D3
		public virtual void CopyFrom(Float2ControllerTransition copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._ParameterNameX = null;
				this._ParameterNameY = null;
				return;
			}
			this._ParameterNameX = copyFrom._ParameterNameX;
			this._ParameterNameY = copyFrom._ParameterNameY;
		}

		// Token: 0x040000E5 RID: 229
		[SerializeField]
		private string _ParameterNameX;

		// Token: 0x040000E6 RID: 230
		[SerializeField]
		private string _ParameterNameY;
	}
}
