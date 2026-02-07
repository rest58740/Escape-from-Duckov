using System;
using System.Reflection;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000034 RID: 52
	[Category("✫ Reflected/Events")]
	[Description("Will subscribe to a public event of Action type and return true when the event is raised.\n(eg public event System.Action [name])")]
	[fsMigrateVersions(new Type[]
	{
		typeof(CheckCSharpEvent_0)
	})]
	public class CheckCSharpEvent : ConditionTask, IReflectedWrapper, IMigratable<CheckCSharpEvent_0>, IMigratable, IMigratable<CheckStaticCSharpEvent>
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00004ABC File Offset: 0x00002CBC
		void IMigratable<CheckCSharpEvent_0>.Migrate(CheckCSharpEvent_0 model)
		{
			Type targetType = model.targetType;
			EventInfo eventInfo = (targetType != null) ? targetType.RTGetEvent(model.eventName) : null;
			if (eventInfo != null)
			{
				this.eventInfo = new SerializedEventInfo(eventInfo);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004AF8 File Offset: 0x00002CF8
		void IMigratable<CheckStaticCSharpEvent>.Migrate(CheckStaticCSharpEvent model)
		{
			Type targetType = model.targetType;
			EventInfo eventInfo = (targetType != null) ? targetType.RTGetEvent(model.eventName) : null;
			if (eventInfo != null)
			{
				this.eventInfo = new SerializedEventInfo(eventInfo);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004B33 File Offset: 0x00002D33
		private EventInfo targetEvent
		{
			get
			{
				return this.eventInfo;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004B40 File Offset: 0x00002D40
		public override Type agentType
		{
			get
			{
				if (this.targetEvent == null)
				{
					return typeof(Transform);
				}
				if (!this.targetEvent.IsStatic())
				{
					return this.targetEvent.RTReflectedOrDeclaredType();
				}
				return null;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004B78 File Offset: 0x00002D78
		protected override string info
		{
			get
			{
				if (this.eventInfo == null)
				{
					return "No Event Selected";
				}
				if (this.targetEvent == null)
				{
					return this.eventInfo.AsString().FormatError();
				}
				return string.Format("'{0}' Raised", this.targetEvent.Name);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004BC7 File Offset: 0x00002DC7
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.eventInfo;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004BD0 File Offset: 0x00002DD0
		protected override string OnInit()
		{
			if (this.eventInfo == null)
			{
				return "No Event Selected";
			}
			if (this.targetEvent == null)
			{
				return this.eventInfo.AsString().FormatError();
			}
			MethodInfo method = base.GetType().RTGetMethod("Raised");
			this.handler = method.RTCreateDelegate(this.targetEvent.EventHandlerType, this);
			return null;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004C34 File Offset: 0x00002E34
		protected override void OnEnable()
		{
			if (this.handler != null)
			{
				this.targetEvent.AddEventHandler(this.targetEvent.IsStatic() ? null : base.agent, this.handler);
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004C65 File Offset: 0x00002E65
		protected override void OnDisable()
		{
			if (this.handler != null)
			{
				this.targetEvent.RemoveEventHandler(this.targetEvent.IsStatic() ? null : base.agent, this.handler);
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004C96 File Offset: 0x00002E96
		public void Raised()
		{
			base.YieldReturn(true);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004C9F File Offset: 0x00002E9F
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004CA2 File Offset: 0x00002EA2
		private void SetTargetEvent(EventInfo info)
		{
			if (info != null)
			{
				this.eventInfo = new SerializedEventInfo(info);
			}
		}

		// Token: 0x04000098 RID: 152
		[SerializeField]
		private SerializedEventInfo eventInfo;

		// Token: 0x04000099 RID: 153
		private Delegate handler;
	}
}
