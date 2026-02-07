using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000010 RID: 16
	public class Float3ControllerState : ControllerState
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00004230 File Offset: 0x00002430
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00004238 File Offset: 0x00002438
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00004244 File Offset: 0x00002444
		// (set) Token: 0x06000159 RID: 345 RVA: 0x0000426C File Offset: 0x0000246C
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

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00004293 File Offset: 0x00002493
		// (set) Token: 0x0600015B RID: 347 RVA: 0x0000429B File Offset: 0x0000249B
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

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000042A4 File Offset: 0x000024A4
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000042CC File Offset: 0x000024CC
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000042F3 File Offset: 0x000024F3
		// (set) Token: 0x0600015F RID: 351 RVA: 0x000042FB File Offset: 0x000024FB
		public ControllerState.ParameterID ParameterZID
		{
			get
			{
				return this._ParameterZID;
			}
			set
			{
				this._ParameterZID = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00004304 File Offset: 0x00002504
		// (set) Token: 0x06000161 RID: 353 RVA: 0x0000432C File Offset: 0x0000252C
		public float ParameterZ
		{
			get
			{
				return base.Playable.GetFloat(this._ParameterZID.Hash);
			}
			set
			{
				base.Playable.SetFloat(this._ParameterZID.Hash, value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00004353 File Offset: 0x00002553
		// (set) Token: 0x06000163 RID: 355 RVA: 0x0000436C File Offset: 0x0000256C
		public Vector3 Parameter
		{
			get
			{
				return new Vector3(this.ParameterX, this.ParameterY, this.ParameterZ);
			}
			set
			{
				this.ParameterX = value.x;
				this.ParameterY = value.y;
				this.ParameterZ = value.z;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00004392 File Offset: 0x00002592
		public Float3ControllerState(RuntimeAnimatorController controller, ControllerState.ParameterID parameterX, ControllerState.ParameterID parameterY, ControllerState.ParameterID parameterZ, params ControllerState.ActionOnStop[] actionsOnStop) : base(controller, actionsOnStop)
		{
			this._ParameterXID = parameterX;
			this._ParameterYID = parameterY;
			this._ParameterZID = parameterZ;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000043B3 File Offset: 0x000025B3
		public Float3ControllerState(RuntimeAnimatorController controller, ControllerState.ParameterID parameterX, ControllerState.ParameterID parameterY, ControllerState.ParameterID parameterZ) : this(controller, parameterX, parameterY, parameterZ, null)
		{
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000043C1 File Offset: 0x000025C1
		public override int ParameterCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000043C4 File Offset: 0x000025C4
		public override int GetParameterHash(int index)
		{
			switch (index)
			{
			case 0:
				return this._ParameterXID;
			case 1:
				return this._ParameterYID;
			case 2:
				return this._ParameterZID;
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004413 File Offset: 0x00002613
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			Float3ControllerState float3ControllerState = new Float3ControllerState(base.Controller, this._ParameterXID, this._ParameterYID, this._ParameterZID);
			float3ControllerState.SetNewCloneRoot(root);
			((ICopyable<ControllerState>)float3ControllerState).CopyFrom(this);
			return float3ControllerState;
		}

		// Token: 0x0400001C RID: 28
		private ControllerState.ParameterID _ParameterXID;

		// Token: 0x0400001D RID: 29
		private ControllerState.ParameterID _ParameterYID;

		// Token: 0x0400001E RID: 30
		private ControllerState.ParameterID _ParameterZID;

		// Token: 0x02000084 RID: 132
		public new interface ITransition : ITransition<Float3ControllerState>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
