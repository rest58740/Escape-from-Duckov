using System;
using ParadoxNotion.Services;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000029 RID: 41
	[Serializable]
	public class BBParameter<T> : BBParameter
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000259 RID: 601 RVA: 0x00007654 File Offset: 0x00005854
		// (remove) Token: 0x0600025A RID: 602 RVA: 0x0000768C File Offset: 0x0000588C
		private event Func<T> getter;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600025B RID: 603 RVA: 0x000076C4 File Offset: 0x000058C4
		// (remove) Token: 0x0600025C RID: 604 RVA: 0x000076FC File Offset: 0x000058FC
		private event Action<T> setter;

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00007734 File Offset: 0x00005934
		// (set) Token: 0x0600025E RID: 606 RVA: 0x000077C0 File Offset: 0x000059C0
		public new T value
		{
			get
			{
				if (this.getter != null)
				{
					return this.getter.Invoke();
				}
				if (!Threader.applicationIsPlaying || base.varRef != null || base.bb == null || string.IsNullOrEmpty(base.name))
				{
					return this._value;
				}
				base.varRef = base.bb.GetVariable(base.name, typeof(T));
				if (this.getter == null)
				{
					return default(T);
				}
				return this.getter.Invoke();
			}
			set
			{
				if (this.setter != null)
				{
					this.setter.Invoke(value);
					return;
				}
				if (base.isNone)
				{
					return;
				}
				if (base.varRef == null && base.bb != null && !string.IsNullOrEmpty(base.name))
				{
					if (base.isPresumedDynamic)
					{
						base.varRef = base.PromoteToVariable(base.bb);
						if (this.setter != null)
						{
							this.setter.Invoke(value);
						}
					}
					return;
				}
				this._value = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000783E File Offset: 0x00005A3E
		public override Type varType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000784A File Offset: 0x00005A4A
		public BBParameter()
		{
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00007852 File Offset: 0x00005A52
		public BBParameter(T value)
		{
			this._value = value;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00007861 File Offset: 0x00005A61
		public override object GetValueBoxed()
		{
			return this.value;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000786E File Offset: 0x00005A6E
		public override void SetValueBoxed(object newValue)
		{
			this.value = (T)((object)newValue);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000787C File Offset: 0x00005A7C
		public T GetValue()
		{
			return this.value;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00007884 File Offset: 0x00005A84
		public void SetValue(T value)
		{
			this.value = value;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000788D File Offset: 0x00005A8D
		protected override void SetDefaultValue()
		{
			this._value = default(T);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000789B File Offset: 0x00005A9B
		protected override void Bind(Variable variable)
		{
			this._value = default(T);
			if (variable == null)
			{
				this.getter = null;
				this.setter = null;
				return;
			}
			this.BindGetter(variable);
			this.BindSetter(variable);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000078CC File Offset: 0x00005ACC
		private bool BindGetter(Variable variable)
		{
			if (variable is Variable<T>)
			{
				this.getter = new Func<T>((variable as Variable<T>).GetValue);
				return true;
			}
			Func<object> convertFunc = variable.GetGetConverter(this.varType);
			if (convertFunc != null)
			{
				this.getter = (() => (T)((object)convertFunc.Invoke()));
				return true;
			}
			return false;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00007930 File Offset: 0x00005B30
		private bool BindSetter(Variable variable)
		{
			if (variable is Variable<T>)
			{
				this.setter = new Action<T>((variable as Variable<T>).SetValue);
				return true;
			}
			Action<object> convertFunc = variable.GetSetConverter(this.varType);
			if (convertFunc != null)
			{
				this.setter = delegate(T value)
				{
					convertFunc.Invoke(value);
				};
				return true;
			}
			this.setter = delegate(T value)
			{
			};
			return false;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000079B8 File Offset: 0x00005BB8
		public static implicit operator BBParameter<T>(T value)
		{
			return new BBParameter<T>
			{
				value = value
			};
		}

		// Token: 0x04000087 RID: 135
		[SerializeField]
		protected T _value;
	}
}
