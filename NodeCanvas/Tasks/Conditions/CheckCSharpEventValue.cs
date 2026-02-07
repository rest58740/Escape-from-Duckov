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
	// Token: 0x02000036 RID: 54
	[Category("✫ Reflected/Events")]
	[Description("Will subscribe to a public event of Action<T> type and return true when the event is raised and it's value is equal to provided value as well.\n(eg public event System.Action<T> [name])")]
	[fsMigrateVersions(new Type[]
	{
		typeof(CheckCSharpEventValue_0<>)
	})]
	public class CheckCSharpEventValue<T> : ConditionTask, IReflectedWrapper, IMigratable<CheckCSharpEventValue_0<T>>, IMigratable
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x00004E99 File Offset: 0x00003099
		void IMigratable<CheckCSharpEventValue_0<!0>>.Migrate(CheckCSharpEventValue_0<T> model)
		{
			Type targetType = model.targetType;
			this.SetTargetEvent((targetType != null) ? targetType.RTGetEvent(model.eventName) : null);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004EB9 File Offset: 0x000030B9
		private EventInfo targetEvent
		{
			get
			{
				return this.eventInfo;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004EC6 File Offset: 0x000030C6
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004EFC File Offset: 0x000030FC
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
				return string.Format("'{0}' Raised && Value == {1}", this.targetEvent.Name, this.checkValue);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004F51 File Offset: 0x00003151
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.eventInfo;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004F5C File Offset: 0x0000315C
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

		// Token: 0x060000CD RID: 205 RVA: 0x00004FC0 File Offset: 0x000031C0
		protected override void OnEnable()
		{
			if (this.handler != null)
			{
				this.targetEvent.AddEventHandler(this.targetEvent.IsStatic() ? null : base.agent, this.handler);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004FF1 File Offset: 0x000031F1
		protected override void OnDisable()
		{
			if (this.handler != null)
			{
				this.targetEvent.RemoveEventHandler(this.targetEvent.IsStatic() ? null : base.agent, this.handler);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005022 File Offset: 0x00003222
		public void Raised(T eventValue)
		{
			if (ObjectUtils.AnyEquals(this.checkValue.value, eventValue))
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005048 File Offset: 0x00003248
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000504B File Offset: 0x0000324B
		private void SetTargetEvent(EventInfo info)
		{
			if (info != null)
			{
				this.eventInfo = new SerializedEventInfo(info);
			}
		}

		// Token: 0x0400009D RID: 157
		[SerializeField]
		private SerializedEventInfo eventInfo;

		// Token: 0x0400009E RID: 158
		[SerializeField]
		private BBParameter<T> checkValue;

		// Token: 0x0400009F RID: 159
		private Delegate handler;
	}
}
