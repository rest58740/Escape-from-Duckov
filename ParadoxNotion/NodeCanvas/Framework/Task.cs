using System;
using System.Collections;
using System.Reflection;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Services;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000026 RID: 38
	[fsDeserializeOverwrite]
	[SpoofAOT]
	[Serializable]
	public abstract class Task : ISerializationCollectable, ISerializationCallbackReceiver
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x00006794 File Offset: 0x00004994
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this.agentType == null)
			{
				this._agentParameter = null;
			}
			if (this._agentParameter != null)
			{
				this._agentParameter.SetType(this.agentType);
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000067C4 File Offset: 0x000049C4
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000067C6 File Offset: 0x000049C6
		public Task()
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000067CE File Offset: 0x000049CE
		public static T Create<T>(ITaskSystem newOwnerSystem) where T : Task
		{
			return (T)((object)Task.Create(typeof(T), newOwnerSystem));
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000067E8 File Offset: 0x000049E8
		public static Task Create(Type type, ITaskSystem newOwnerSystem)
		{
			if (type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(new Type[]
				{
					type.GetFirstGenericParameterConstraintType()
				});
			}
			Task task = (Task)Activator.CreateInstance(type);
			BBParameter.SetBBFields(task, newOwnerSystem.blackboard);
			task.Validate(newOwnerSystem);
			task.OnCreate(newOwnerSystem);
			return task;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00006839 File Offset: 0x00004A39
		public virtual Task Duplicate(ITaskSystem newOwnerSystem)
		{
			Task task = JSONSerializer.Clone<Task>(this);
			BBParameter.SetBBFields(task, newOwnerSystem.blackboard);
			task.Validate(newOwnerSystem);
			return task;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00006854 File Offset: 0x00004A54
		public void Validate(ITaskSystem ownerSystem)
		{
			this.SetOwnerSystem(ownerSystem);
			this.OnValidate(ownerSystem);
			this.GetHardError();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000686B File Offset: 0x00004A6B
		public void SetOwnerSystem(ITaskSystem newOwnerSystem)
		{
			this.ownerSystem = newOwnerSystem;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00006874 File Offset: 0x00004A74
		// (set) Token: 0x060001FC RID: 508 RVA: 0x0000687C File Offset: 0x00004A7C
		public ITaskSystem ownerSystem
		{
			get
			{
				return this._ownerSystem;
			}
			private set
			{
				this._ownerSystem = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00006885 File Offset: 0x00004A85
		public Component ownerSystemAgent
		{
			get
			{
				if (this.ownerSystem == null)
				{
					return null;
				}
				return this.ownerSystem.agent;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000689C File Offset: 0x00004A9C
		public IBlackboard ownerSystemBlackboard
		{
			get
			{
				if (this.ownerSystem == null)
				{
					return null;
				}
				return this.ownerSystem.blackboard;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001FF RID: 511 RVA: 0x000068B3 File Offset: 0x00004AB3
		public float ownerSystemElapsedTime
		{
			get
			{
				if (this.ownerSystem == null)
				{
					return 0f;
				}
				return this.ownerSystem.elapsedTime;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000068CE File Offset: 0x00004ACE
		// (set) Token: 0x06000201 RID: 513 RVA: 0x000068D9 File Offset: 0x00004AD9
		public bool isUserEnabled
		{
			get
			{
				return !this._isUserDisabled;
			}
			internal set
			{
				this._isUserDisabled = !value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000202 RID: 514 RVA: 0x000068E8 File Offset: 0x00004AE8
		public string obsolete
		{
			get
			{
				if (this._obsoleteInfo == null)
				{
					ObsoleteAttribute obsoleteAttribute = base.GetType().RTGetAttribute(true);
					this._obsoleteInfo = ((obsoleteAttribute != null) ? obsoleteAttribute.Message : string.Empty);
				}
				return this._obsoleteInfo;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00006928 File Offset: 0x00004B28
		public string name
		{
			get
			{
				if (this._taskName == null)
				{
					NameAttribute nameAttribute = base.GetType().RTGetAttribute(false);
					this._taskName = ((nameAttribute != null) ? nameAttribute.name : base.GetType().FriendlyName(false).SplitCamelCase());
				}
				return this._taskName;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00006974 File Offset: 0x00004B74
		public string description
		{
			get
			{
				if (this._taskDescription == null)
				{
					DescriptionAttribute descriptionAttribute = base.GetType().RTGetAttribute(true);
					this._taskDescription = ((descriptionAttribute != null) ? descriptionAttribute.description : string.Empty);
				}
				return this._taskDescription;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000069B4 File Offset: 0x00004BB4
		public string summaryInfo
		{
			get
			{
				if (this is ActionTask)
				{
					return (this.agentIsOverride ? "* " : "") + this.info;
				}
				if (this is ConditionTask)
				{
					return (this.agentIsOverride ? "* " : "") + ((this as ConditionTask).invert ? "If <b>!</b> " : "If ") + this.info;
				}
				return this.info;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00006A30 File Offset: 0x00004C30
		protected virtual string info
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00006A38 File Offset: 0x00004C38
		public virtual Type agentType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00006A3B File Offset: 0x00004C3B
		public string agentInfo
		{
			get
			{
				if (this._agentParameter == null)
				{
					return "<b>Self</b>";
				}
				return this._agentParameter.ToString();
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00006A56 File Offset: 0x00004C56
		public string agentParameterName
		{
			get
			{
				if (this._agentParameter == null)
				{
					return null;
				}
				return this._agentParameter.name;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00006A6D File Offset: 0x00004C6D
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00006A78 File Offset: 0x00004C78
		public bool agentIsOverride
		{
			get
			{
				return this._agentParameter != null;
			}
			set
			{
				if (!value && this._agentParameter != null)
				{
					this._agentParameter = null;
				}
				if (value && this._agentParameter == null)
				{
					this._agentParameter = new TaskAgentParameter();
					this._agentParameter.bb = this.blackboard;
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00006AB4 File Offset: 0x00004CB4
		public Component agent
		{
			get
			{
				if (this._currentAgent != null)
				{
					return this._currentAgent;
				}
				return (this.agentIsOverride ? ((Component)this._agentParameter.value) : this.ownerSystemAgent).TransformToType(this.agentType);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00006B01 File Offset: 0x00004D01
		public IBlackboard blackboard
		{
			get
			{
				return this.ownerSystemBlackboard;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00006B0C File Offset: 0x00004D0C
		public EventRouter router
		{
			get
			{
				if (!(this._eventRouter != null))
				{
					return this._eventRouter = ((this.agent == null) ? null : this.agent.gameObject.GetAddComponent<EventRouter>());
				}
				return this._eventRouter;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00006B58 File Offset: 0x00004D58
		protected bool Set(Component newAgent, IBlackboard newBB)
		{
			if (this.agentIsOverride)
			{
				newAgent = (Component)this._agentParameter.value;
			}
			if (this._currentAgent != null && newAgent != null && this._currentAgent.gameObject == newAgent.gameObject)
			{
				return this._isInitSuccess;
			}
			return this._isInitSuccess = this.Initialize(newAgent);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00006BC8 File Offset: 0x00004DC8
		private bool Initialize(Component newAgent)
		{
			this._eventRouter = null;
			this._currentAgent = newAgent.TransformToType(this.agentType);
			if (this._currentAgent == null && this.agentType != null)
			{
				string text = "Failed to resolve Agent to requested type '";
				Type agentType = this.agentType;
				return this.Error(text + ((agentType != null) ? agentType.ToString() : null) + "', or new Agent is NULL. Does the Agent has the requested Component?", "Execution");
			}
			if (!this.InitializeFieldAttributes(this._currentAgent))
			{
				return false;
			}
			string text2 = this.OnInit();
			return text2 == null || this.Error(text2, "Execution");
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00006C60 File Offset: 0x00004E60
		private bool InitializeFieldAttributes(Component newAgent)
		{
			foreach (FieldInfo fieldInfo in base.GetType().RTGetFields())
			{
				if (newAgent != null && (typeof(Component).RTIsAssignableFrom(fieldInfo.FieldType) || fieldInfo.FieldType.IsInterface) && fieldInfo.RTIsDefined(true))
				{
					Component component = newAgent.GetComponent(fieldInfo.FieldType);
					fieldInfo.SetValue(this, component);
					if (component == null)
					{
						return this.Error(string.Format("GetFromAgent Attribute failed to get the required Component of type '{0}' from '{1}'. Does it exist?", fieldInfo.FieldType.Name, this.agent.gameObject.name), "Execution");
					}
				}
			}
			return true;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00006D11 File Offset: 0x00004F11
		protected bool Error(string error, string tag = "Execution")
		{
			return false;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00006D14 File Offset: 0x00004F14
		protected Coroutine StartCoroutine(IEnumerator routine)
		{
			if (!(MonoManager.current != null))
			{
				return null;
			}
			return MonoManager.current.StartCoroutine(routine);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00006D30 File Offset: 0x00004F30
		protected void StopCoroutine(Coroutine routine)
		{
			if (MonoManager.current != null)
			{
				MonoManager.current.StopCoroutine(routine);
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00006D4A File Offset: 0x00004F4A
		protected void SendEvent(string name)
		{
			if (this.ownerSystem != null)
			{
				this.ownerSystem.SendEvent(name, null, this);
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00006D62 File Offset: 0x00004F62
		protected void SendEvent<T>(string name, T value)
		{
			if (this.ownerSystem != null)
			{
				this.ownerSystem.SendEvent<T>(name, value, this);
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00006D7C File Offset: 0x00004F7C
		internal virtual string GetWarningOrError()
		{
			string hardError = this.GetHardError();
			if (hardError != null)
			{
				return "* " + hardError;
			}
			string text = this.OnErrorCheck();
			if (text != null)
			{
				return text;
			}
			if (this.obsolete != string.Empty)
			{
				return string.Format("Task is obsolete: '{0}'", this.obsolete);
			}
			if (this.agentType != null && this.agent == null && (this._agentParameter == null || (this._agentParameter.isNoneOrNull && !this._agentParameter.isDefined)))
			{
				return string.Format("* '{0}' target agent is null", this.agentType.Name);
			}
			foreach (FieldInfo fieldInfo in base.GetType().RTGetFields())
			{
				if (fieldInfo.RTIsDefined(true))
				{
					object value = fieldInfo.GetValue(this);
					if (value == null || value.Equals(null))
					{
						return string.Format("* Required field '{0}' is null", fieldInfo.Name.SplitCamelCase());
					}
					if (fieldInfo.FieldType == typeof(string) && string.IsNullOrEmpty((string)value))
					{
						return string.Format("* Required string field '{0}' is null or empty", fieldInfo.Name.SplitCamelCase());
					}
					if (typeof(BBParameter).RTIsAssignableFrom(fieldInfo.FieldType))
					{
						BBParameter bbparameter = value as BBParameter;
						if (bbparameter == null)
						{
							return string.Format("* BBParameter '{0}' is null", fieldInfo.Name.SplitCamelCase());
						}
						if (!bbparameter.isDefined && bbparameter.isNoneOrNull)
						{
							return string.Format("* Required parameter '{0}' is null", fieldInfo.Name.SplitCamelCase());
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00006F26 File Offset: 0x00005126
		protected virtual string OnErrorCheck()
		{
			return null;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00006F2C File Offset: 0x0000512C
		private string GetHardError()
		{
			if (this is IMissingRecoverable)
			{
				return string.Format("Missing Task '{0}'", (this as IMissingRecoverable).missingType);
			}
			if (this is IReflectedWrapper)
			{
				ISerializedReflectedInfo serializedInfo = (this as IReflectedWrapper).GetSerializedInfo();
				if (serializedInfo != null && serializedInfo.AsMemberInfo() == null)
				{
					return string.Format("Missing Reflected Info '{0}'", serializedInfo.AsString());
				}
			}
			return null;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00006F8E File Offset: 0x0000518E
		protected virtual string OnInit()
		{
			return null;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00006F91 File Offset: 0x00005191
		public virtual void OnCreate(ITaskSystem ownerSystem)
		{
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00006F93 File Offset: 0x00005193
		public virtual void OnValidate(ITaskSystem ownerSystem)
		{
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00006F95 File Offset: 0x00005195
		[Obsolete("Use OnDrawGizmosSelected")]
		public virtual void OnDrawGizmos()
		{
			this.OnDrawGizmosSelected();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00006F9D File Offset: 0x0000519D
		public virtual void OnDrawGizmosSelected()
		{
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006F9F File Offset: 0x0000519F
		public override string ToString()
		{
			return this.summaryInfo;
		}

		// Token: 0x04000071 RID: 113
		[fsSerializeAs("_isDisabled")]
		private bool _isUserDisabled;

		// Token: 0x04000072 RID: 114
		[fsSerializeAs("overrideAgent")]
		protected internal TaskAgentParameter _agentParameter;

		// Token: 0x04000073 RID: 115
		private ITaskSystem _ownerSystem;

		// Token: 0x04000074 RID: 116
		private Component _currentAgent;

		// Token: 0x04000075 RID: 117
		private string _taskName;

		// Token: 0x04000076 RID: 118
		private string _taskDescription;

		// Token: 0x04000077 RID: 119
		private string _obsoleteInfo;

		// Token: 0x04000078 RID: 120
		private bool _isRuntimeActive;

		// Token: 0x04000079 RID: 121
		private bool _isInitSuccess;

		// Token: 0x0400007A RID: 122
		private EventRouter _eventRouter;

		// Token: 0x02000109 RID: 265
		[AttributeUsage(256)]
		protected class GetFromAgentAttribute : Attribute
		{
		}
	}
}
