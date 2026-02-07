using System;
using System.Collections.Generic;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000027 RID: 39
	[CreateAssetMenu(menuName = "ParadoxNotion/CanvasCore/Blackboard Asset")]
	public class AssetBlackboard : ScriptableObject, ISerializationCallbackReceiver, IGlobalBlackboard, IBlackboard
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000220 RID: 544 RVA: 0x00006FA8 File Offset: 0x000051A8
		// (remove) Token: 0x06000221 RID: 545 RVA: 0x00006FE0 File Offset: 0x000051E0
		public event Action<Variable> onVariableAdded;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000222 RID: 546 RVA: 0x00007018 File Offset: 0x00005218
		// (remove) Token: 0x06000223 RID: 547 RVA: 0x00007050 File Offset: 0x00005250
		public event Action<Variable> onVariableRemoved;

		// Token: 0x06000224 RID: 548 RVA: 0x00007085 File Offset: 0x00005285
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.SelfSerialize();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000708D File Offset: 0x0000528D
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.SelfDeserialize();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00007095 File Offset: 0x00005295
		private void SelfSerialize()
		{
			this._objectReferences = new List<Object>();
			this._serializedBlackboard = JSONSerializer.Serialize(typeof(BlackboardSource), this._blackboard, this._objectReferences, false);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000070C4 File Offset: 0x000052C4
		private void SelfDeserialize()
		{
			this._blackboard = JSONSerializer.Deserialize<BlackboardSource>(this._serializedBlackboard, this._objectReferences);
			if (this._blackboard == null)
			{
				this._blackboard = new BlackboardSource();
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000070F0 File Offset: 0x000052F0
		// (set) Token: 0x06000229 RID: 553 RVA: 0x000070FD File Offset: 0x000052FD
		Dictionary<string, Variable> IBlackboard.variables
		{
			get
			{
				return this._blackboard.variables;
			}
			set
			{
				this._blackboard.variables = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000710B File Offset: 0x0000530B
		Object IBlackboard.unityContextObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000710E File Offset: 0x0000530E
		IBlackboard IBlackboard.parent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00007111 File Offset: 0x00005311
		Component IBlackboard.propertiesBindTarget
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00007114 File Offset: 0x00005314
		string IBlackboard.independantVariablesFieldName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00007117 File Offset: 0x00005317
		void IBlackboard.TryInvokeOnVariableAdded(Variable variable)
		{
			if (this.onVariableAdded != null)
			{
				this.onVariableAdded.Invoke(variable);
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000712D File Offset: 0x0000532D
		void IBlackboard.TryInvokeOnVariableRemoved(Variable variable)
		{
			if (this.onVariableRemoved != null)
			{
				this.onVariableRemoved.Invoke(variable);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00007143 File Offset: 0x00005343
		public string identifier
		{
			get
			{
				return this._identifier;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000714B File Offset: 0x0000534B
		public string UID
		{
			get
			{
				return this._UID;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00007153 File Offset: 0x00005353
		[ContextMenu("Show Json")]
		private void ShowJson()
		{
			JSONSerializer.ShowData(this._serializedBlackboard, base.name);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00007166 File Offset: 0x00005366
		public override string ToString()
		{
			return this.identifier;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000716E File Offset: 0x0000536E
		private void OnValidate()
		{
			this._identifier = base.name;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000717C File Offset: 0x0000537C
		private void OnEnable()
		{
			this.InitializePropertiesBinding(null, false);
		}

		// Token: 0x0400007D RID: 125
		[SerializeField]
		private string _serializedBlackboard;

		// Token: 0x0400007E RID: 126
		[SerializeField]
		private List<Object> _objectReferences;

		// Token: 0x0400007F RID: 127
		[SerializeField]
		private string _UID = Guid.NewGuid().ToString();

		// Token: 0x04000080 RID: 128
		[NonSerialized]
		private string _identifier;

		// Token: 0x04000081 RID: 129
		[NonSerialized]
		private BlackboardSource _blackboard = new BlackboardSource();
	}
}
