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
	// Token: 0x0200003E RID: 62
	[Category("✫ Reflected/Events")]
	[Description("Will subscribe to a public UnityEvent and return true when that event is raised.")]
	[fsMigrateVersions(new Type[]
	{
		typeof(CheckUnityEvent_0)
	})]
	public class CheckUnityEvent : ConditionTask, IReflectedWrapper, IMigratable<CheckUnityEvent_0>, IMigratable
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00005961 File Offset: 0x00003B61
		void IMigratable<CheckUnityEvent_0>.Migrate(CheckUnityEvent_0 model)
		{
			Type targetType = model.targetType;
			this._eventInfo = new SerializedUnityEventInfo((targetType != null) ? targetType.RTGetField(model.eventName, false) : null);
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005987 File Offset: 0x00003B87
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000599E File Offset: 0x00003B9E
		private bool isStatic
		{
			get
			{
				return this._eventInfo != null && this._eventInfo.isStatic;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000059B5 File Offset: 0x00003BB5
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000059CC File Offset: 0x00003BCC
		private FieldInfo targetEventField
		{
			get
			{
				return this._eventInfo;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000059D9 File Offset: 0x00003BD9
		private PropertyInfo targetEventProp
		{
			get
			{
				return this._eventInfo;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000059E6 File Offset: 0x00003BE6
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00005A18 File Offset: 0x00003C18
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

		// Token: 0x060000FA RID: 250 RVA: 0x00005A67 File Offset: 0x00003C67
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this._eventInfo;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005A70 File Offset: 0x00003C70
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
				this.unityEvent = (UnityEvent)this.targetEventField.GetValue(base.agent);
			}
			if (this.targetEventProp != null)
			{
				this.unityEvent = (UnityEvent)this.targetEventProp.GetValue(base.agent);
			}
			return null;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005AFA File Offset: 0x00003CFA
		protected override void OnEnable()
		{
			if (this.unityEvent != null)
			{
				this.unityEvent.AddListener(new UnityAction(this.Raised));
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005B1B File Offset: 0x00003D1B
		protected override void OnDisable()
		{
			if (this.unityEvent != null)
			{
				this.unityEvent.RemoveListener(new UnityAction(this.Raised));
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005B3C File Offset: 0x00003D3C
		public void Raised()
		{
			base.YieldReturn(true);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005B45 File Offset: 0x00003D45
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005B48 File Offset: 0x00003D48
		private void SetTargetEvent(MemberInfo newMember)
		{
			if (newMember != null)
			{
				this._eventInfo = new SerializedUnityEventInfo(newMember);
			}
		}

		// Token: 0x040000B5 RID: 181
		[SerializeField]
		private SerializedUnityEventInfo _eventInfo;

		// Token: 0x040000B6 RID: 182
		private UnityEvent unityEvent;
	}
}
