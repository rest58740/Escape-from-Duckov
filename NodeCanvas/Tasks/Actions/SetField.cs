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
	// Token: 0x020000C2 RID: 194
	[Category("✫ Reflected")]
	[Description("Set a variable on a script")]
	[Name("Set Field", 5)]
	[fsMigrateVersions(new Type[]
	{
		typeof(SetField_0)
	})]
	public class SetField : ActionTask, IReflectedWrapper, IMigratable<SetField_0>, IMigratable
	{
		// Token: 0x06000337 RID: 823 RVA: 0x0000CE23 File Offset: 0x0000B023
		void IMigratable<SetField_0>.Migrate(SetField_0 model)
		{
			Type targetType = model.targetType;
			this.field = new SerializedFieldInfo((targetType != null) ? targetType.RTGetField(model.fieldName, false) : null);
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000CE49 File Offset: 0x0000B049
		private FieldInfo targetField
		{
			get
			{
				return this.field;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000CE56 File Offset: 0x0000B056
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000CE8C File Offset: 0x0000B08C
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
				return string.Format("{0}.{1} = {2}", text, this.targetField.Name, this.setValue);
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000CF09 File Offset: 0x0000B109
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.field;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000CF11 File Offset: 0x0000B111
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

		// Token: 0x0600033D RID: 829 RVA: 0x0000CF41 File Offset: 0x0000B141
		protected override void OnExecute()
		{
			this.targetField.SetValue(this.targetField.IsStatic ? null : base.agent, this.setValue.value);
			base.EndAction();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000CF75 File Offset: 0x0000B175
		private void SetTargetField(FieldInfo newField)
		{
			if (newField != null)
			{
				this.field = new SerializedFieldInfo(newField);
				this.setValue.SetType(newField.FieldType);
			}
		}

		// Token: 0x0400024D RID: 589
		[SerializeField]
		protected SerializedFieldInfo field;

		// Token: 0x0400024E RID: 590
		[SerializeField]
		protected BBObjectParameter setValue;
	}
}
