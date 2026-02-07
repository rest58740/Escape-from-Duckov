using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class Float3ControllerTransition : ControllerTransition<Float3ControllerState>, Float3ControllerState.ITransition, ITransition<Float3ControllerState>, ITransition, IHasKey, IPolymorphic, ICopyable<Float3ControllerTransition>
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0000E50E File Offset: 0x0000C70E
		public ref string ParameterNameX
		{
			get
			{
				return ref this._ParameterNameX;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0000E516 File Offset: 0x0000C716
		public ref string ParameterNameY
		{
			get
			{
				return ref this._ParameterNameY;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0000E51E File Offset: 0x0000C71E
		public ref string ParameterNameZ
		{
			get
			{
				return ref this._ParameterNameZ;
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0000E526 File Offset: 0x0000C726
		public Float3ControllerTransition()
		{
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0000E52E File Offset: 0x0000C72E
		public unsafe Float3ControllerTransition(RuntimeAnimatorController controller, string parameterNameX, string parameterNameY, string parameterNameZ)
		{
			*base.Controller = controller;
			this._ParameterNameX = parameterNameX;
			this._ParameterNameY = parameterNameY;
			this._ParameterNameZ = parameterNameZ;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0000E554 File Offset: 0x0000C754
		public unsafe override Float3ControllerState CreateState()
		{
			return base.State = new Float3ControllerState(*base.Controller, this._ParameterNameX, this._ParameterNameY, this._ParameterNameZ, *base.ActionsOnStop);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public virtual void CopyFrom(Float3ControllerTransition copyFrom)
		{
			this.CopyFrom(copyFrom);
			if (copyFrom == null)
			{
				this._ParameterNameX = null;
				this._ParameterNameY = null;
				this._ParameterNameZ = null;
				return;
			}
			this._ParameterNameX = copyFrom._ParameterNameX;
			this._ParameterNameY = copyFrom._ParameterNameY;
			this._ParameterNameZ = copyFrom._ParameterNameZ;
		}

		// Token: 0x040000E7 RID: 231
		[SerializeField]
		private string _ParameterNameX;

		// Token: 0x040000E8 RID: 232
		[SerializeField]
		private string _ParameterNameY;

		// Token: 0x040000E9 RID: 233
		[SerializeField]
		private string _ParameterNameZ;
	}
}
