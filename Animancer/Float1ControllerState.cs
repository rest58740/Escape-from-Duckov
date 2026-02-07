using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200000E RID: 14
	public class Float1ControllerState : ControllerState
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00004014 File Offset: 0x00002214
		// (set) Token: 0x0600013F RID: 319 RVA: 0x0000401C File Offset: 0x0000221C
		public new ControllerState.ParameterID ParameterID
		{
			get
			{
				return this._ParameterID;
			}
			set
			{
				this._ParameterID = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00004028 File Offset: 0x00002228
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00004050 File Offset: 0x00002250
		public float Parameter
		{
			get
			{
				return base.Playable.GetFloat(this._ParameterID.Hash);
			}
			set
			{
				base.Playable.SetFloat(this._ParameterID.Hash, value);
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00004077 File Offset: 0x00002277
		public Float1ControllerState(RuntimeAnimatorController controller, ControllerState.ParameterID parameter, params ControllerState.ActionOnStop[] actionsOnStop) : base(controller, actionsOnStop)
		{
			this._ParameterID = parameter;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00004088 File Offset: 0x00002288
		public Float1ControllerState(RuntimeAnimatorController controller, ControllerState.ParameterID parameter) : this(controller, parameter, null)
		{
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00004093 File Offset: 0x00002293
		public override int ParameterCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00004096 File Offset: 0x00002296
		public override int GetParameterHash(int index)
		{
			return this._ParameterID;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000040A3 File Offset: 0x000022A3
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			Float1ControllerState float1ControllerState = new Float1ControllerState(base.Controller, this._ParameterID);
			float1ControllerState.SetNewCloneRoot(root);
			((ICopyable<ControllerState>)float1ControllerState).CopyFrom(this);
			return float1ControllerState;
		}

		// Token: 0x04000019 RID: 25
		private ControllerState.ParameterID _ParameterID;

		// Token: 0x02000082 RID: 130
		public new interface ITransition : ITransition<Float1ControllerState>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
