using System;
using System.Reflection;
using ParadoxNotion;
using ParadoxNotion.Services;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000030 RID: 48
	public class Variable<T> : Variable
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060002E1 RID: 737 RVA: 0x00008900 File Offset: 0x00006B00
		// (remove) Token: 0x060002E2 RID: 738 RVA: 0x00008938 File Offset: 0x00006B38
		private event Func<T> getter;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060002E3 RID: 739 RVA: 0x00008970 File Offset: 0x00006B70
		// (remove) Token: 0x060002E4 RID: 740 RVA: 0x000089A8 File Offset: 0x00006BA8
		private event Action<T> setter;

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x000089DD File Offset: 0x00006BDD
		public override Type varType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x000089E9 File Offset: 0x00006BE9
		public override bool isDataBound
		{
			get
			{
				return this.getter != null || this.setter != null;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x000089FE File Offset: 0x00006BFE
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x00008A06 File Offset: 0x00006C06
		public override string propertyPath
		{
			get
			{
				return this._propertyPath;
			}
			set
			{
				this._propertyPath = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x00008A0F File Offset: 0x00006C0F
		// (set) Token: 0x060002EA RID: 746 RVA: 0x00008A2C File Offset: 0x00006C2C
		public new T value
		{
			get
			{
				if (this.getter == null)
				{
					return this._value;
				}
				return this.getter.Invoke();
			}
			set
			{
				if (base.HasValueChangeEvent())
				{
					object obj = value;
					if (!ObjectUtils.AnyEquals(this._value, obj))
					{
						this._value = value;
						if (this.setter != null)
						{
							this.setter.Invoke(value);
						}
						base.TryInvokeValueChangeEvent(obj);
					}
					return;
				}
				this._value = value;
				if (this.setter != null)
				{
					this.setter.Invoke(value);
				}
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00008A99 File Offset: 0x00006C99
		public Variable()
		{
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00008AA1 File Offset: 0x00006CA1
		public Variable(string name, string ID) : base(name, ID)
		{
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00008AAB File Offset: 0x00006CAB
		public override object GetValueBoxed()
		{
			return this.value;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00008AB8 File Offset: 0x00006CB8
		public override void SetValueBoxed(object newValue)
		{
			this.value = (T)((object)newValue);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00008AC6 File Offset: 0x00006CC6
		public T GetValue()
		{
			return this.value;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00008ACE File Offset: 0x00006CCE
		public void SetValue(T newValue)
		{
			this.value = newValue;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00008AD8 File Offset: 0x00006CD8
		public override void BindProperty(MemberInfo prop, GameObject target = null)
		{
			if (prop is PropertyInfo || prop is FieldInfo)
			{
				this._propertyPath = string.Format("{0}.{1}", prop.RTReflectedOrDeclaredType().FullName, prop.Name);
				if (target != null)
				{
					this.InitializePropertyBinding(target, false);
				}
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00008B27 File Offset: 0x00006D27
		public void BindGetSet(Func<T> _get, Action<T> _set)
		{
			this.getter = _get;
			this.setter = _set;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00008B37 File Offset: 0x00006D37
		public override void UnBind()
		{
			this._propertyPath = null;
			this.getter = null;
			this.setter = null;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00008B50 File Offset: 0x00006D50
		public override void InitializePropertyBinding(GameObject go, bool callSetter = false)
		{
			if (!base.isPropertyBound || !Threader.applicationIsPlaying)
			{
				return;
			}
			this.getter = null;
			this.setter = null;
			int num = this._propertyPath.LastIndexOf('.');
			string typeFullName = this._propertyPath.Substring(0, num);
			string name = this._propertyPath.Substring(num + 1);
			Type type = ReflectionTools.GetType(typeFullName, true, typeof(Component));
			if (type == null)
			{
				return;
			}
			MemberInfo memberInfo = type.RTGetFieldOrProp(name);
			if (memberInfo is FieldInfo)
			{
				FieldInfo field = (FieldInfo)memberInfo;
				Component instance = field.IsStatic ? null : go.GetComponent(type);
				if (instance == null && !field.IsStatic)
				{
					return;
				}
				if (field.IsConstant())
				{
					T value = (T)((object)field.GetValue(instance));
					this.getter = (() => value);
					return;
				}
				this.getter = (() => (T)((object)field.GetValue(instance)));
				this.setter = delegate(T o)
				{
					field.SetValue(instance, o);
				};
				return;
			}
			else
			{
				if (!(memberInfo is PropertyInfo))
				{
					return;
				}
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				MethodInfo getMethod = propertyInfo.RTGetGetMethod();
				MethodInfo setMethod = propertyInfo.RTGetSetMethod();
				bool flag = (getMethod != null && getMethod.IsStatic) || (setMethod != null && setMethod.IsStatic);
				Component instance = flag ? null : go.GetComponent(type);
				if (instance == null && !flag)
				{
					return;
				}
				if (propertyInfo.CanRead && getMethod != null)
				{
					try
					{
						this.getter = getMethod.RTCreateDelegate(instance);
						goto IL_243;
					}
					catch
					{
						this.getter = (() => (T)((object)getMethod.Invoke(instance, null)));
						goto IL_243;
					}
				}
				this.getter = (() => default(T));
				IL_243:
				if (propertyInfo.CanWrite && setMethod != null)
				{
					try
					{
						this.setter = setMethod.RTCreateDelegate(instance);
					}
					catch
					{
						this.setter = delegate(T o)
						{
							setMethod.Invoke(instance, ReflectionTools.SingleTempArgsArray(o));
						};
					}
					if (callSetter)
					{
						this.setter.Invoke(this._value);
						return;
					}
				}
				else
				{
					this.setter = delegate(T o)
					{
					};
				}
				return;
			}
		}

		// Token: 0x0400009F RID: 159
		[SerializeField]
		private T _value;

		// Token: 0x040000A0 RID: 160
		[SerializeField]
		private string _propertyPath;
	}
}
