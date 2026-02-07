using System;
using System.Collections;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000028 RID: 40
	[SpoofAOT]
	[fsAutoInstance(true)]
	[fsUninitialized]
	[Serializable]
	public abstract class BBParameter : ISerializationCollectable, ISerializationCallbackReceiver
	{
		// Token: 0x06000237 RID: 567 RVA: 0x000071BF File Offset: 0x000053BF
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.useBlackboard)
			{
				this.SetDefaultValue();
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000071CF File Offset: 0x000053CF
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000239 RID: 569 RVA: 0x000071D4 File Offset: 0x000053D4
		// (remove) Token: 0x0600023A RID: 570 RVA: 0x0000720C File Offset: 0x0000540C
		public event Action<Variable> onVariableReferenceChanged;

		// Token: 0x0600023B RID: 571 RVA: 0x00007241 File Offset: 0x00005441
		public BBParameter()
		{
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00007249 File Offset: 0x00005449
		public static BBParameter CreateInstance(Type t, IBlackboard bb)
		{
			if (t == null)
			{
				return null;
			}
			BBParameter bbparameter = (BBParameter)Activator.CreateInstance(typeof(BBParameter<>).RTMakeGenericType(new Type[]
			{
				t
			}));
			bbparameter.bb = bb;
			return bbparameter;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00007280 File Offset: 0x00005480
		public static void SetBBFields(object target, IBlackboard bb)
		{
			if (target == null)
			{
				return;
			}
			JSONSerializer.SerializeAndExecuteNoCycles(target.GetType(), target, delegate(object o, fsData d)
			{
				if (o is BBParameter)
				{
					(o as BBParameter).bb = bb;
				}
			});
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600023E RID: 574 RVA: 0x000072B6 File Offset: 0x000054B6
		// (set) Token: 0x0600023F RID: 575 RVA: 0x000072BE File Offset: 0x000054BE
		public string targetVariableID
		{
			get
			{
				return this._targetVariableID;
			}
			protected set
			{
				this._targetVariableID = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000240 RID: 576 RVA: 0x000072C7 File Offset: 0x000054C7
		// (set) Token: 0x06000241 RID: 577 RVA: 0x000072CF File Offset: 0x000054CF
		public Variable varRef
		{
			get
			{
				return this._varRef;
			}
			protected set
			{
				if (this._varRef != value)
				{
					this._varRef = value;
					this.Bind(value);
					if (this.onVariableReferenceChanged != null)
					{
						this.onVariableReferenceChanged.Invoke(value);
					}
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000072FC File Offset: 0x000054FC
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00007304 File Offset: 0x00005504
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
					if (string.IsNullOrEmpty(value))
					{
						this.varRef = null;
						this.targetVariableID = null;
						return;
					}
					this.varRef = ((value != null) ? this.ResolveReference(this.bb, false) : null);
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00007356 File Offset: 0x00005556
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000735E File Offset: 0x0000555E
		public IBlackboard bb
		{
			get
			{
				return this._bb;
			}
			set
			{
				if (this._bb != value)
				{
					this._bb = value;
				}
				this.varRef = ((value != null) ? this.ResolveReference(this._bb, true) : null);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00007389 File Offset: 0x00005589
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00007394 File Offset: 0x00005594
		public bool useBlackboard
		{
			get
			{
				return this.name != null;
			}
			set
			{
				if (!value)
				{
					this.name = null;
					this.targetVariableID = null;
					this.varRef = null;
				}
				if (value && this.name == null)
				{
					this.name = string.Empty;
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000073C4 File Offset: 0x000055C4
		public bool isPresumedDynamic
		{
			get
			{
				return this.name != null && this.name.StartsWith("_");
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000073E0 File Offset: 0x000055E0
		public bool isNone
		{
			get
			{
				return this.name == string.Empty;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000073F2 File Offset: 0x000055F2
		public bool isNull
		{
			get
			{
				return ObjectUtils.AnyEquals(this.value, null);
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00007400 File Offset: 0x00005600
		public bool isNoneOrNull
		{
			get
			{
				return this.isNone || this.isNull;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00007412 File Offset: 0x00005612
		public bool isDefined
		{
			get
			{
				return !string.IsNullOrEmpty(this.name);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00007422 File Offset: 0x00005622
		public Type refType
		{
			get
			{
				if (this.varRef == null)
				{
					return null;
				}
				return this.varRef.varType;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00007439 File Offset: 0x00005639
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00007441 File Offset: 0x00005641
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000250 RID: 592
		public abstract Type varType { get; }

		// Token: 0x06000251 RID: 593
		protected abstract void SetDefaultValue();

		// Token: 0x06000252 RID: 594
		protected abstract void Bind(Variable data);

		// Token: 0x06000253 RID: 595
		public abstract object GetValueBoxed();

		// Token: 0x06000254 RID: 596
		public abstract void SetValueBoxed(object value);

		// Token: 0x06000255 RID: 597 RVA: 0x0000744C File Offset: 0x0000564C
		public void SetTargetVariable(IBlackboard targetBB, Variable targetVariable)
		{
			if (targetVariable != null)
			{
				this._targetVariableID = targetVariable.ID;
				this._name = ((targetBB is GlobalBlackboard) ? string.Format("{0}/{1}", targetBB.identifier, targetVariable.name) : targetVariable.name);
				this.varRef = this.ResolveReference(this.bb, true);
				return;
			}
			this.targetVariableID = null;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000074B0 File Offset: 0x000056B0
		private Variable ResolveReference(IBlackboard targetBlackboard, bool useID)
		{
			if (string.IsNullOrEmpty(this.name) && string.IsNullOrEmpty(this.targetVariableID))
			{
				return null;
			}
			string text = this.name;
			if (text != null && text.Contains("/"))
			{
				string[] array = text.Split('/', 0);
				targetBlackboard = GlobalBlackboard.Find(array[0]);
				text = array[1];
			}
			Variable variable = null;
			if (targetBlackboard == null)
			{
				return null;
			}
			if (useID && this.targetVariableID != null)
			{
				variable = targetBlackboard.GetVariableByID(this.targetVariableID);
			}
			if (variable == null && !string.IsNullOrEmpty(text))
			{
				variable = targetBlackboard.GetVariable(text, this.varType);
			}
			return variable;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00007540 File Offset: 0x00005740
		public Variable PromoteToVariable(IBlackboard targetBB)
		{
			if (string.IsNullOrEmpty(this.name))
			{
				this.varRef = null;
				return null;
			}
			string varName = this.name;
			string name = string.Empty;
			if (this.name.Contains("/"))
			{
				string[] array = this.name.Split('/', 0);
				name = array[0];
				varName = array[1];
				targetBB = GlobalBlackboard.Find(name);
			}
			if (targetBB == null)
			{
				this.varRef = null;
				return null;
			}
			this.varRef = targetBB.AddVariable(varName, this.varType);
			Variable varRef = this.varRef;
			return this.varRef;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000075CC File Offset: 0x000057CC
		public sealed override string ToString()
		{
			if (this.isNone)
			{
				return "<b>NONE</b>";
			}
			if (this.useBlackboard)
			{
				return string.Format("<b>${0}</b>", this.name);
			}
			if (this.isNull)
			{
				return "<b>NULL</b>";
			}
			if (this.value is IList || this.value is IDictionary)
			{
				return string.Format("<b>{0}</b>", this.varType.FriendlyName(false));
			}
			return string.Format("<b>{0}</b>", this.value.ToStringAdvanced());
		}

		// Token: 0x04000082 RID: 130
		[SerializeField]
		private string _name;

		// Token: 0x04000083 RID: 131
		[SerializeField]
		private string _targetVariableID;

		// Token: 0x04000084 RID: 132
		private IBlackboard _bb;

		// Token: 0x04000085 RID: 133
		private Variable _varRef;
	}
}
