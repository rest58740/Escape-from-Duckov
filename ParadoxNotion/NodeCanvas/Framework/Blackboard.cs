using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200002A RID: 42
	[SpoofAOT]
	public class Blackboard : MonoBehaviour, ISerializationCallbackReceiver, IBlackboard
	{
		// Token: 0x0600026B RID: 619 RVA: 0x000079C6 File Offset: 0x00005BC6
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.SelfSerialize();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x000079CE File Offset: 0x00005BCE
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.SelfDeserialize();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000079D8 File Offset: 0x00005BD8
		public void SelfSerialize()
		{
			if (this.haltForUndo)
			{
				return;
			}
			List<Object> list = new List<Object>();
			string text = JSONSerializer.Serialize(typeof(BlackboardSource), this._blackboard, list, false);
			if (text != this._serializedBlackboard || !list.SequenceEqual(this._objectReferences) || this._serializedVariables == null || this._serializedVariables.Length != this._blackboard.variables.Count)
			{
				this.haltForUndo = true;
				this.haltForUndo = false;
				this._serializedVariables = new SerializationPair[this._blackboard.variables.Count];
				for (int i = 0; i < this._blackboard.variables.Count; i++)
				{
					SerializationPair serializationPair = new SerializationPair();
					serializationPair._json = JSONSerializer.Serialize(typeof(Variable), this._blackboard.variables.ElementAt(i).Value, serializationPair._references, false);
					this._serializedVariables[i] = serializationPair;
				}
				this._serializedBlackboard = text;
				this._objectReferences = list;
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00007AE4 File Offset: 0x00005CE4
		public void SelfDeserialize()
		{
			this._blackboard = new BlackboardSource();
			if (!string.IsNullOrEmpty(this._serializedBlackboard))
			{
				JSONSerializer.TryDeserializeOverwrite<BlackboardSource>(this._blackboard, this._serializedBlackboard, this._objectReferences);
			}
			if (this._serializedVariables != null && this._serializedVariables.Length != 0)
			{
				this._blackboard.variables.Clear();
				for (int i = 0; i < this._serializedVariables.Length; i++)
				{
					Variable variable = JSONSerializer.Deserialize<Variable>(this._serializedVariables[i]._json, this._serializedVariables[i]._references);
					this._blackboard.variables[variable.name] = variable;
				}
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00007B8C File Offset: 0x00005D8C
		public string Serialize(List<Object> references, bool pretyJson = false)
		{
			return JSONSerializer.Serialize(typeof(BlackboardSource), this._blackboard, references, pretyJson);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00007BA8 File Offset: 0x00005DA8
		public bool Deserialize(string json, List<Object> references, bool removeMissingVariables = true)
		{
			BlackboardSource blackboardSource = JSONSerializer.Deserialize<BlackboardSource>(json, references);
			if (blackboardSource == null)
			{
				return false;
			}
			this.OverwriteFrom(blackboardSource, removeMissingVariables);
			this.InitializePropertiesBinding(((IBlackboard)this).propertiesBindTarget, true);
			return true;
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000271 RID: 625 RVA: 0x00007BD8 File Offset: 0x00005DD8
		// (remove) Token: 0x06000272 RID: 626 RVA: 0x00007C10 File Offset: 0x00005E10
		public event Action<Variable> onVariableAdded;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000273 RID: 627 RVA: 0x00007C48 File Offset: 0x00005E48
		// (remove) Token: 0x06000274 RID: 628 RVA: 0x00007C80 File Offset: 0x00005E80
		public event Action<Variable> onVariableRemoved;

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00007CB5 File Offset: 0x00005EB5
		string IBlackboard.identifier
		{
			get
			{
				return this._identifier;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00007CBD File Offset: 0x00005EBD
		// (set) Token: 0x06000277 RID: 631 RVA: 0x00007CCA File Offset: 0x00005ECA
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00007CD8 File Offset: 0x00005ED8
		Component IBlackboard.propertiesBindTarget
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00007CDB File Offset: 0x00005EDB
		Object IBlackboard.unityContextObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00007CDE File Offset: 0x00005EDE
		IBlackboard IBlackboard.parent
		{
			get
			{
				return this._parentBlackboard;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00007CE6 File Offset: 0x00005EE6
		string IBlackboard.independantVariablesFieldName
		{
			get
			{
				return "_serializedVariables";
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00007CED File Offset: 0x00005EED
		void IBlackboard.TryInvokeOnVariableAdded(Variable variable)
		{
			if (this.onVariableAdded != null)
			{
				this.onVariableAdded.Invoke(variable);
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00007D03 File Offset: 0x00005F03
		void IBlackboard.TryInvokeOnVariableRemoved(Variable variable)
		{
			if (this.onVariableRemoved != null)
			{
				this.onVariableRemoved.Invoke(variable);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00007D19 File Offset: 0x00005F19
		protected virtual void Awake()
		{
			this._identifier = base.gameObject.name;
			this.InitializePropertiesBinding(((IBlackboard)this).propertiesBindTarget, false);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00007D39 File Offset: 0x00005F39
		public Variable AddVariable(string name, Type type)
		{
			return this.AddVariable(name, type);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00007D43 File Offset: 0x00005F43
		public Variable AddVariable(string name, object value)
		{
			return this.AddVariable(name, value);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00007D4D File Offset: 0x00005F4D
		public Variable RemoveVariable(string name)
		{
			return this.RemoveVariable(name);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00007D56 File Offset: 0x00005F56
		public Variable GetVariable(string name, Type ofType = null)
		{
			return this.GetVariable(name, ofType);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00007D60 File Offset: 0x00005F60
		public Variable GetVariableByID(string ID)
		{
			return this.GetVariableByID(ID);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00007D69 File Offset: 0x00005F69
		public Variable<T> GetVariable<T>(string name)
		{
			return this.GetVariable(name);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00007D72 File Offset: 0x00005F72
		public T GetVariableValue<T>(string name)
		{
			return this.GetVariableValue(name);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00007D7B File Offset: 0x00005F7B
		public Variable SetVariableValue(string name, object value)
		{
			return this.SetVariableValue(name, value);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00007D85 File Offset: 0x00005F85
		[Obsolete("Use GetVariableValue")]
		public T GetValue<T>(string name)
		{
			return this.GetVariableValue<T>(name);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00007D8E File Offset: 0x00005F8E
		[Obsolete("Use SetVariableValue")]
		public Variable SetValue(string name, object value)
		{
			return this.SetVariableValue(name, value);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00007D98 File Offset: 0x00005F98
		[ContextMenu("Show Json")]
		private void ShowJson()
		{
			JSONSerializer.ShowData(this._serializedBlackboard, base.name);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00007DAB File Offset: 0x00005FAB
		public string Save()
		{
			return this.Save(base.name);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00007DBC File Offset: 0x00005FBC
		public string Save(string saveKey)
		{
			string text = this.Serialize(null, false);
			PlayerPrefs.SetString(saveKey, text);
			return text;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00007DDA File Offset: 0x00005FDA
		public bool Load()
		{
			return this.Load(base.name);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00007DE8 File Offset: 0x00005FE8
		public bool Load(string saveKey)
		{
			string @string = PlayerPrefs.GetString(saveKey);
			if (string.IsNullOrEmpty(@string))
			{
				Debug.Log("No data to load blackboard variables from key " + saveKey);
				return false;
			}
			return this.Deserialize(@string, null, true);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00007E1F File Offset: 0x0000601F
		protected virtual void OnValidate()
		{
			this._identifier = base.gameObject.name;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00007E32 File Offset: 0x00006032
		public override string ToString()
		{
			return this._identifier;
		}

		// Token: 0x0400008A RID: 138
		[Tooltip("An optional Parent Blackboard Asset to 'inherit' variables from.")]
		[SerializeField]
		private AssetBlackboard _parentBlackboard;

		// Token: 0x0400008B RID: 139
		[SerializeField]
		private string _serializedBlackboard;

		// Token: 0x0400008C RID: 140
		[SerializeField]
		private List<Object> _objectReferences;

		// Token: 0x0400008D RID: 141
		[SerializeField]
		private SerializationPair[] _serializedVariables;

		// Token: 0x0400008E RID: 142
		[NonSerialized]
		private BlackboardSource _blackboard = new BlackboardSource();

		// Token: 0x0400008F RID: 143
		[NonSerialized]
		private bool haltForUndo;

		// Token: 0x04000090 RID: 144
		[NonSerialized]
		private string _identifier;
	}
}
