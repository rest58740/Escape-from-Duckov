using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200000F RID: 15
	public class Float2ControllerState : ControllerState
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000040C4 File Offset: 0x000022C4
		// (set) Token: 0x06000148 RID: 328 RVA: 0x000040CC File Offset: 0x000022CC
		public ControllerState.ParameterID ParameterXID
		{
			get
			{
				return this._ParameterXID;
			}
			set
			{
				this._ParameterXID = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000040D8 File Offset: 0x000022D8
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00004100 File Offset: 0x00002300
		public float ParameterX
		{
			get
			{
				return base.Playable.GetFloat(this._ParameterXID.Hash);
			}
			set
			{
				base.Playable.SetFloat(this._ParameterXID.Hash, value);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00004127 File Offset: 0x00002327
		// (set) Token: 0x0600014C RID: 332 RVA: 0x0000412F File Offset: 0x0000232F
		public ControllerState.ParameterID ParameterYID
		{
			get
			{
				return this._ParameterYID;
			}
			set
			{
				this._ParameterYID = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00004138 File Offset: 0x00002338
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00004160 File Offset: 0x00002360
		public float ParameterY
		{
			get
			{
				return base.Playable.GetFloat(this._ParameterYID.Hash);
			}
			set
			{
				base.Playable.SetFloat(this._ParameterYID.Hash, value);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00004187 File Offset: 0x00002387
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000419A File Offset: 0x0000239A
		public Vector2 Parameter
		{
			get
			{
				return new Vector2(this.ParameterX, this.ParameterY);
			}
			set
			{
				this.ParameterX = value.x;
				this.ParameterY = value.y;
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000041B4 File Offset: 0x000023B4
		public Float2ControllerState(RuntimeAnimatorController controller, ControllerState.ParameterID parameterX, ControllerState.ParameterID parameterY, params ControllerState.ActionOnStop[] actionsOnStop) : base(controller, actionsOnStop)
		{
			this._ParameterXID = parameterX;
			this._ParameterYID = parameterY;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000041CD File Offset: 0x000023CD
		public Float2ControllerState(RuntimeAnimatorController controller, ControllerState.ParameterID parameterX, ControllerState.ParameterID parameterY) : this(controller, parameterX, parameterY, null)
		{
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000041D9 File Offset: 0x000023D9
		public override int ParameterCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000041DC File Offset: 0x000023DC
		public override int GetParameterHash(int index)
		{
			if (index == 0)
			{
				return this._ParameterXID;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._ParameterYID;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004209 File Offset: 0x00002409
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			Float2ControllerState float2ControllerState = new Float2ControllerState(base.Controller, this._ParameterXID, this._ParameterYID);
			float2ControllerState.SetNewCloneRoot(root);
			((ICopyable<ControllerState>)float2ControllerState).CopyFrom(this);
			return float2ControllerState;
		}

		// Token: 0x0400001A RID: 26
		private ControllerState.ParameterID _ParameterXID;

		// Token: 0x0400001B RID: 27
		private ControllerState.ParameterID _ParameterYID;

		// Token: 0x02000083 RID: 131
		public new interface ITransition : ITransition<Float2ControllerState>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
