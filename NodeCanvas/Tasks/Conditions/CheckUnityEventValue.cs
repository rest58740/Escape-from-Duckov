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
	// Token: 0x02000040 RID: 64
	[Category("✫ Reflected/Events")]
	[Description("Will subscribe to a public UnityEvent<T> and return true when that event is raised and it's value is equal to provided value as well.")]
	[fsMigrateVersions(new Type[]
	{
		typeof(CheckUnityEventValue_0<>)
	})]
	public class CheckUnityEventValue<T> : ConditionTask, IReflectedWrapper, IMigratable<CheckUnityEventValue_0<T>>, IMigratable
	{
		// Token: 0x06000112 RID: 274 RVA: 0x00005D77 File Offset: 0x00003F77
		void IMigratable<CheckUnityEventValue_0<!0>>.Migrate(CheckUnityEventValue_0<T> model)
		{
			Type targetType = model.targetType;
			this._eventInfo = new SerializedUnityEventInfo((targetType != null) ? targetType.RTGetField(model.eventName, false) : null);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00005D9D File Offset: 0x00003F9D
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005DB4 File Offset: 0x00003FB4
		private bool isStatic
		{
			get
			{
				return this._eventInfo != null && this._eventInfo.isStatic;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00005DCB File Offset: 0x00003FCB
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

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00005DE2 File Offset: 0x00003FE2
		private FieldInfo targetEventField
		{
			get
			{
				return this._eventInfo;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005DEF File Offset: 0x00003FEF
		private PropertyInfo targetEventProp
		{
			get
			{
				return this._eventInfo;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00005DFC File Offset: 0x00003FFC
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

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00005E2C File Offset: 0x0000402C
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
				return string.Format("'{0}' Raised && Value == {1}", this.targetMember.Name, this.checkValue);
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005E81 File Offset: 0x00004081
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this._eventInfo;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005E8C File Offset: 0x0000408C
		protected override string OnInit()
		{
			if (this._eventInfo == null)
			{
				return "No Event Selected";
			}
			if (this.targetEventField == null)
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

		// Token: 0x0600011C RID: 284 RVA: 0x00005F16 File Offset: 0x00004116
		protected override void OnEnable()
		{
			if (this.unityEvent != null)
			{
				this.unityEvent.AddListener(new UnityAction<T>(this.Raised));
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005F37 File Offset: 0x00004137
		protected override void OnDisable()
		{
			if (this.unityEvent != null)
			{
				this.unityEvent.RemoveListener(new UnityAction<T>(this.Raised));
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005F58 File Offset: 0x00004158
		public void Raised(T eventValue)
		{
			if (ObjectUtils.AnyEquals(this.checkValue.value, eventValue))
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005F7E File Offset: 0x0000417E
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005F81 File Offset: 0x00004181
		private void SetTargetEvent(MemberInfo newMember)
		{
			if (newMember != null)
			{
				this._eventInfo = new SerializedUnityEventInfo(newMember);
			}
		}

		// Token: 0x040000BA RID: 186
		[SerializeField]
		private SerializedUnityEventInfo _eventInfo;

		// Token: 0x040000BB RID: 187
		[SerializeField]
		private BBParameter<T> checkValue;

		// Token: 0x040000BC RID: 188
		private UnityEvent<T> unityEvent;
	}
}
