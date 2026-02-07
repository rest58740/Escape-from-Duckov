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
	// Token: 0x02000035 RID: 53
	[Category("✫ Reflected/Events")]
	[Description("Will subscribe to a public event of Action<T> type and return true when the event is raised.\n(eg public event System.Action<T> [name])")]
	[fsMigrateVersions(new Type[]
	{
		typeof(CheckCSharpEvent_0<>)
	})]
	public class CheckCSharpEvent<T> : ConditionTask, IReflectedWrapper, IMigratable<CheckCSharpEvent_0<T>>, IMigratable, IMigratable<CheckStaticCSharpEvent<T>>
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00004CC1 File Offset: 0x00002EC1
		void IMigratable<CheckCSharpEvent_0<!0>>.Migrate(CheckCSharpEvent_0<T> model)
		{
			Type targetType = model.targetType;
			this.SetTargetEvent((targetType != null) ? targetType.RTGetEvent(model.eventName) : null);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004CE1 File Offset: 0x00002EE1
		void IMigratable<CheckStaticCSharpEvent<!0>>.Migrate(CheckStaticCSharpEvent<T> model)
		{
			Type targetType = model.targetType;
			this.SetTargetEvent((targetType != null) ? targetType.RTGetEvent(model.eventName) : null);
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004D01 File Offset: 0x00002F01
		private EventInfo targetEvent
		{
			get
			{
				return this.eventInfo;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004D0E File Offset: 0x00002F0E
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004D44 File Offset: 0x00002F44
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

		// Token: 0x060000BF RID: 191 RVA: 0x00004D93 File Offset: 0x00002F93
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.eventInfo;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004D9C File Offset: 0x00002F9C
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

		// Token: 0x060000C1 RID: 193 RVA: 0x00004E00 File Offset: 0x00003000
		protected override void OnEnable()
		{
			if (this.handler != null)
			{
				this.targetEvent.AddEventHandler(this.targetEvent.IsStatic() ? null : base.agent, this.handler);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004E31 File Offset: 0x00003031
		protected override void OnDisable()
		{
			if (this.handler != null)
			{
				this.targetEvent.RemoveEventHandler(this.targetEvent.IsStatic() ? null : base.agent, this.handler);
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004E62 File Offset: 0x00003062
		public void Raised(T eventValue)
		{
			this.saveAs.value = eventValue;
			base.YieldReturn(true);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004E77 File Offset: 0x00003077
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004E7A File Offset: 0x0000307A
		private void SetTargetEvent(EventInfo info)
		{
			if (info != null)
			{
				this.eventInfo = new SerializedEventInfo(info);
			}
		}

		// Token: 0x0400009A RID: 154
		[SerializeField]
		private SerializedEventInfo eventInfo;

		// Token: 0x0400009B RID: 155
		[SerializeField]
		[BlackboardOnly]
		private BBParameter<T> saveAs;

		// Token: 0x0400009C RID: 156
		private Delegate handler;
	}
}
