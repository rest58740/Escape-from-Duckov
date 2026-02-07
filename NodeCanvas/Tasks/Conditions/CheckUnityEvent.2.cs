using System;
using System.Reflection;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200003F RID: 63
	[Category("✫ Reflected/Events")]
	[Description("Will subscribe to a public UnityEvent<T> and return true when that event is raised.")]
	[fsMigrateVersions(new Type[]
	{
		typeof(CheckUnityEvent_0<>)
	})]
	public class CheckUnityEvent<T> : ConditionTask, IReflectedWrapper, IMigratable<CheckUnityEvent_0<T>>, IMigratable
	{
		// Token: 0x06000102 RID: 258 RVA: 0x00005B67 File Offset: 0x00003D67
		void IMigratable<CheckUnityEvent_0<!0>>.Migrate(CheckUnityEvent_0<T> model)
		{
			Type targetType = model.targetType;
			this._eventInfo = new SerializedUnityEventInfo((targetType != null) ? targetType.RTGetField(model.eventName, false) : null);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00005B8D File Offset: 0x00003D8D
		private MemberInfo targetMember
		{
			get
			{
				if (this._eventInfo == null)
				{
					return null;
				}
				return this._eventInfo.AsMemberInfo();
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005BA4 File Offset: 0x00003DA4
		private bool isStatic
		{
			get
			{
				return this._eventInfo != null && this._eventInfo.isStatic;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00005BBB File Offset: 0x00003DBB
		private Type eventType
		{
			get
			{
				if (this._eventInfo == null)
				{
					return null;
				}
				return this._eventInfo.memberType;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005BD2 File Offset: 0x00003DD2
		private FieldInfo targetEventField
		{
			get
			{
				return this._eventInfo;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005BDF File Offset: 0x00003DDF
		private PropertyInfo targetEventProp
		{
			get
			{
				return this._eventInfo;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005BEC File Offset: 0x00003DEC
		public override Type agentType
		{
			get
			{
				if (this.targetMember == null)
				{
					return typeof(Transform);
				}
				if (!this.isStatic)
				{
					return this.targetMember.RTReflectedOrDeclaredType();
				}
				return null;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005C1C File Offset: 0x00003E1C
		protected override string info
		{
			get
			{
				if (this._eventInfo == null)
				{
					return "No Event Selected";
				}
				if (this.targetMember == null)
				{
					return this._eventInfo.AsString().FormatError();
				}
				return string.Format("'{0}' Raised", this.targetMember.Name);
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005C6B File Offset: 0x00003E6B
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this._eventInfo;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005C74 File Offset: 0x00003E74
		protected override string OnInit()
		{
			if (this._eventInfo == null)
			{
				return "No Event Selected";
			}
			if (this.targetMember == null)
			{
				return this._eventInfo.AsString();
			}
			if (this.targetEventField != null)
			{
				this.unityEvent = (UnityEvent<T>)this.targetEventField.GetValue(base.agent);
			}
			if (this.targetEventProp != null)
			{
				this.unityEvent = (UnityEvent<T>)this.targetEventProp.GetValue(base.agent);
			}
			return null;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005CFE File Offset: 0x00003EFE
		protected override void OnEnable()
		{
			if (this.unityEvent != null)
			{
				this.unityEvent.AddListener(new UnityAction<T>(this.Raised));
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005D1F File Offset: 0x00003F1F
		protected override void OnDisable()
		{
			if (this.unityEvent != null)
			{
				this.unityEvent.RemoveListener(new UnityAction<T>(this.Raised));
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005D40 File Offset: 0x00003F40
		public void Raised(T eventValue)
		{
			this.saveAs.value = eventValue;
			base.YieldReturn(true);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005D55 File Offset: 0x00003F55
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005D58 File Offset: 0x00003F58
		private void SetTargetEvent(MemberInfo newMember)
		{
			if (newMember != null)
			{
				this._eventInfo = new SerializedUnityEventInfo(newMember);
			}
		}

		// Token: 0x040000B7 RID: 183
		[SerializeField]
		private SerializedUnityEventInfo _eventInfo;

		// Token: 0x040000B8 RID: 184
		[SerializeField]
		[BlackboardOnly]
		private BBParameter<T> saveAs;

		// Token: 0x040000B9 RID: 185
		private UnityEvent<T> unityEvent;
	}
}
