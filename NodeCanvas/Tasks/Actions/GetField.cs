using System;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000BB RID: 187
	[Category("✫ Reflected")]
	[Description("Get a variable of a script and save it to the blackboard")]
	[Name("Get Field", 6)]
	[fsMigrateVersions(new Type[]
	{
		typeof(GetField_0)
	})]
	public class GetField : ActionTask, IReflectedWrapper, IMigratable<GetField_0>, IMigratable
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000C727 File Offset: 0x0000A927
		void IMigratable<GetField_0>.Migrate(GetField_0 model)
		{
			Type targetType = model.targetType;
			this.field = new SerializedFieldInfo((targetType != null) ? targetType.RTGetField(model.fieldName, false) : null);
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000C74D File Offset: 0x0000A94D
		private FieldInfo targetField
		{
			get
			{
				return this.field;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000C75A File Offset: 0x0000A95A
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000C790 File Offset: 0x0000A990
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
				return string.Format("{0} = {1}.{2}", this.saveAs.ToString(), text, this.targetField.Name);
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000C812 File Offset: 0x0000AA12
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.field;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000C81A File Offset: 0x0000AA1A
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

		// Token: 0x06000317 RID: 791 RVA: 0x0000C84A File Offset: 0x0000AA4A
		protected override void OnExecute()
		{
			this.saveAs.value = this.targetField.GetValue(this.targetField.IsStatic ? null : base.agent);
			base.EndAction();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000C87E File Offset: 0x0000AA7E
		private void SetTargetField(FieldInfo newField)
		{
			if (newField != null)
			{
				this.field = new SerializedFieldInfo(newField);
				this.saveAs.SetType(newField.FieldType);
			}
		}

		// Token: 0x0400023E RID: 574
		[SerializeField]
		protected SerializedFieldInfo field;

		// Token: 0x0400023F RID: 575
		[SerializeField]
		[BlackboardOnly]
		protected BBObjectParameter saveAs;
	}
}
