using System;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000038 RID: 56
	[Name("Check Field", 8)]
	[Category("✫ Reflected")]
	[Description("Check a field on a script and return if it's equal or not to a value")]
	[fsMigrateVersions(new Type[]
	{
		typeof(CheckField_0)
	})]
	public class CheckField : ConditionTask, IReflectedWrapper, IMigratable<CheckField_0>, IMigratable
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00005074 File Offset: 0x00003274
		void IMigratable<CheckField_0>.Migrate(CheckField_0 model)
		{
			try
			{
				Type targetType = model.targetType;
				this.field = new SerializedFieldInfo((targetType != null) ? targetType.RTGetField(model.fieldName, false) : null);
			}
			finally
			{
				this.checkValue = new BBObjectParameter(model.checkValue);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000050CC File Offset: 0x000032CC
		private FieldInfo targetField
		{
			get
			{
				return this.field;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000050D9 File Offset: 0x000032D9
		public override Type agentType
		{
			get
			{
				if (this.targetField == null)
				{
					return typeof(Transform);
				}
				if (!this.targetField.IsStatic)
				{
					return this.targetField.RTReflectedOrDeclaredType();
				}
				return null;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005110 File Offset: 0x00003310
		protected override string info
		{
			get
			{
				if (this.field == null)
				{
					return "No Field Selected";
				}
				if (this.targetField == null)
				{
					return this.field.AsString().FormatError();
				}
				string text = this.targetField.IsStatic ? this.targetField.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				return string.Format("{0}.{1}{2}{3}", new object[]
				{
					text,
					this.targetField.Name,
					OperationTools.GetCompareString(this.comparison),
					this.checkValue
				});
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000051AA File Offset: 0x000033AA
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.field;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000051B2 File Offset: 0x000033B2
		protected override string OnInit()
		{
			if (this.field == null)
			{
				return "No Field Selected";
			}
			if (this.targetField == null)
			{
				return this.field.AsString().FormatError();
			}
			return null;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000051E4 File Offset: 0x000033E4
		protected override bool OnCheck()
		{
			if (this.checkValue.varType == typeof(float))
			{
				return OperationTools.Compare((float)this.targetField.GetValue(base.agent), (float)this.checkValue.value, this.comparison, 0.05f);
			}
			if (this.checkValue.varType == typeof(int))
			{
				return OperationTools.Compare((int)this.targetField.GetValue(base.agent), (int)this.checkValue.value, this.comparison);
			}
			return ObjectUtils.AnyEquals(this.targetField.GetValue(base.agent), this.checkValue.value);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000052B3 File Offset: 0x000034B3
		private void SetTargetField(FieldInfo newField)
		{
			if (newField != null)
			{
				this.field = new SerializedFieldInfo(newField);
				this.checkValue.SetType(newField.FieldType);
				this.comparison = CompareMethod.EqualTo;
			}
		}

		// Token: 0x040000A3 RID: 163
		[SerializeField]
		protected BBObjectParameter checkValue;

		// Token: 0x040000A4 RID: 164
		[SerializeField]
		protected CompareMethod comparison;

		// Token: 0x040000A5 RID: 165
		[SerializeField]
		protected SerializedFieldInfo field;
	}
}
