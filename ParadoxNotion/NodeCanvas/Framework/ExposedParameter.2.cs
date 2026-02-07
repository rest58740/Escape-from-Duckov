using System;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200000B RID: 11
	public sealed class ExposedParameter<T> : ExposedParameter
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000025ED File Offset: 0x000007ED
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000025F5 File Offset: 0x000007F5
		public Variable<T> varRef { get; private set; }

		// Token: 0x06000031 RID: 49 RVA: 0x000025FE File Offset: 0x000007FE
		public ExposedParameter()
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002606 File Offset: 0x00000806
		public ExposedParameter(Variable target)
		{
			this._targetVariableID = target.ID;
			this._value = (T)((object)target.value);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0000262B File Offset: 0x0000082B
		public override string targetVariableID
		{
			get
			{
				return this._targetVariableID;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002633 File Offset: 0x00000833
		public override Type type
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000263F File Offset: 0x0000083F
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0000264C File Offset: 0x0000084C
		public override object valueBoxed
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = (T)((object)value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000265A File Offset: 0x0000085A
		public override Variable varRefBoxed
		{
			get
			{
				return this.varRef;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002662 File Offset: 0x00000862
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002685 File Offset: 0x00000885
		public T value
		{
			get
			{
				if (this.varRef == null || !Application.isPlaying)
				{
					return this._value;
				}
				return this.varRef.value;
			}
			set
			{
				if (this.varRef != null && Application.isPlaying)
				{
					this.varRef.value = value;
				}
				this._value = value;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000026AC File Offset: 0x000008AC
		public override void Bind(IBlackboard blackboard)
		{
			if (this.varRef != null)
			{
				this.varRef.UnBind();
			}
			this.varRef = (Variable<T>)blackboard.GetVariableByID(this.targetVariableID);
			if (this.varRef != null)
			{
				this.varRef.BindGetSet(new Func<T>(this.GetRawValue), new Action<T>(this.SetRawValue));
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000270E File Offset: 0x0000090E
		public override void UnBind()
		{
			if (this.varRef != null)
			{
				this.varRef.UnBind();
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002723 File Offset: 0x00000923
		private T GetRawValue()
		{
			return this._value;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000272B File Offset: 0x0000092B
		private void SetRawValue(T value)
		{
			this._value = value;
		}

		// Token: 0x0400001B RID: 27
		[SerializeField]
		private string _targetVariableID;

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private T _value;
	}
}
