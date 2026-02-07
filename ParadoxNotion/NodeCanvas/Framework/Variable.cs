using System;
using System.Reflection;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200002F RID: 47
	[fsUninitialized]
	[SpoofAOT]
	[Serializable]
	public abstract class Variable
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060002BD RID: 701 RVA: 0x00008530 File Offset: 0x00006730
		// (remove) Token: 0x060002BE RID: 702 RVA: 0x00008568 File Offset: 0x00006768
		public event Action<string> onNameChanged;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060002BF RID: 703 RVA: 0x000085A0 File Offset: 0x000067A0
		// (remove) Token: 0x060002C0 RID: 704 RVA: 0x000085D8 File Offset: 0x000067D8
		public event Action<object> onValueChanged;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060002C1 RID: 705 RVA: 0x00008610 File Offset: 0x00006810
		// (remove) Token: 0x060002C2 RID: 706 RVA: 0x00008648 File Offset: 0x00006848
		public event Action onDestroy;

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000867D File Offset: 0x0000687D
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x00008685 File Offset: 0x00006885
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (this._name != value)
				{
					this._name = value;
					if (this.onNameChanged != null)
					{
						this.onNameChanged.Invoke(value);
					}
				}
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x000086B0 File Offset: 0x000068B0
		public string ID
		{
			get
			{
				if (!string.IsNullOrEmpty(this._id))
				{
					return this._id;
				}
				return this._id = Guid.NewGuid().ToString();
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x000086ED File Offset: 0x000068ED
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x000086F5 File Offset: 0x000068F5
		public object value
		{
			get
			{
				return this.GetValueBoxed();
			}
			set
			{
				this.SetValueBoxed(value);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x000086FE File Offset: 0x000068FE
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x00008713 File Offset: 0x00006913
		public bool isExposedPublic
		{
			get
			{
				return this._isPublic && !this.isPropertyBound;
			}
			set
			{
				this._isPublic = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000871C File Offset: 0x0000691C
		// (set) Token: 0x060002CB RID: 715 RVA: 0x00008724 File Offset: 0x00006924
		public bool debugBoundValue
		{
			get
			{
				return this._debugBoundValue;
			}
			set
			{
				this._debugBoundValue = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000872D File Offset: 0x0000692D
		public bool isPropertyBound
		{
			get
			{
				return !string.IsNullOrEmpty(this.propertyPath);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002CD RID: 717
		public abstract bool isDataBound { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002CE RID: 718
		public abstract Type varType { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002CF RID: 719
		// (set) Token: 0x060002D0 RID: 720
		public abstract string propertyPath { get; set; }

		// Token: 0x060002D1 RID: 721
		public abstract void BindProperty(MemberInfo prop, GameObject target = null);

		// Token: 0x060002D2 RID: 722
		public abstract void UnBind();

		// Token: 0x060002D3 RID: 723
		public abstract void InitializePropertyBinding(GameObject go, bool callSetter = false);

		// Token: 0x060002D4 RID: 724
		public abstract object GetValueBoxed();

		// Token: 0x060002D5 RID: 725
		public abstract void SetValueBoxed(object value);

		// Token: 0x060002D6 RID: 726 RVA: 0x00008740 File Offset: 0x00006940
		public Variable()
		{
			this._id = Guid.NewGuid().ToString();
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000876C File Offset: 0x0000696C
		public Variable(string name, string ID)
		{
			this._name = name;
			this._id = ID;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00008782 File Offset: 0x00006982
		internal void OnDestroy()
		{
			if (this.onDestroy != null)
			{
				this.onDestroy.Invoke();
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00008798 File Offset: 0x00006998
		public Variable Duplicate(IBlackboard targetBB)
		{
			string text = this.name;
			while (targetBB.variables.ContainsKey(text))
			{
				text += ".";
			}
			Variable variable = targetBB.AddVariable(text, this.varType);
			if (variable != null)
			{
				variable.value = this.value;
				variable.propertyPath = this.propertyPath;
				variable.isExposedPublic = this.isExposedPublic;
			}
			return variable;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000087FE File Offset: 0x000069FE
		protected bool HasValueChangeEvent()
		{
			return this.onValueChanged != null;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00008809 File Offset: 0x00006A09
		protected void TryInvokeValueChangeEvent(object value)
		{
			if (this.onValueChanged != null)
			{
				this.onValueChanged.Invoke(value);
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000881F File Offset: 0x00006A1F
		public bool CanConvertTo(Type toType)
		{
			return this.GetGetConverter(toType) != null;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000882C File Offset: 0x00006A2C
		public Func<object> GetGetConverter(Type toType)
		{
			if (toType.RTIsAssignableFrom(this.varType))
			{
				return () => this.value;
			}
			Func<object, object> converter = TypeConverter.Get(this.varType, toType);
			if (converter != null)
			{
				return () => converter.Invoke(this.value);
			}
			return null;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00008889 File Offset: 0x00006A89
		public bool CanConvertFrom(Type fromType)
		{
			return this.GetSetConverter(fromType) != null;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00008898 File Offset: 0x00006A98
		public Action<object> GetSetConverter(Type fromType)
		{
			if (this.varType.RTIsAssignableFrom(fromType))
			{
				return delegate(object x)
				{
					this.value = x;
				};
			}
			Func<object, object> converter = TypeConverter.Get(fromType, this.varType);
			if (converter != null)
			{
				return delegate(object x)
				{
					this.value = converter.Invoke(x);
				};
			}
			return null;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000088F5 File Offset: 0x00006AF5
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000098 RID: 152
		[SerializeField]
		private string _name;

		// Token: 0x04000099 RID: 153
		[SerializeField]
		private string _id;

		// Token: 0x0400009A RID: 154
		[SerializeField]
		private bool _isPublic;

		// Token: 0x0400009B RID: 155
		[SerializeField]
		[fsIgnoreInBuild]
		private bool _debugBoundValue;
	}
}
