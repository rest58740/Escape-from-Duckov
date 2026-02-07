using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas
{
	// Token: 0x02000005 RID: 5
	[AddComponentMenu("NodeCanvas/Standalone Action List (Bonus)")]
	public class ActionListPlayer : MonoBehaviour, ITaskSystem, ISerializationCallbackReceiver
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002226 File Offset: 0x00000426
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this._objectReferences = new List<Object>();
			this._serializedList = JSONSerializer.Serialize(typeof(ActionList), this._actionList, this._objectReferences, false);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002255 File Offset: 0x00000455
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this._actionList = JSONSerializer.Deserialize<ActionList>(this._serializedList, this._objectReferences);
			if (this._actionList == null)
			{
				this._actionList = (ActionList)Task.Create(typeof(ActionList), this);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002291 File Offset: 0x00000491
		public ActionList actionList
		{
			get
			{
				return this._actionList;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002299 File Offset: 0x00000499
		public float elapsedTime
		{
			get
			{
				return Time.time - this.timeStarted;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000022A7 File Offset: 0x000004A7
		public float deltaTime
		{
			get
			{
				return Time.deltaTime;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000022AE File Offset: 0x000004AE
		Object ITaskSystem.contextObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022B1 File Offset: 0x000004B1
		Component ITaskSystem.agent
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022B4 File Offset: 0x000004B4
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000022BC File Offset: 0x000004BC
		public IBlackboard blackboard
		{
			get
			{
				return this._blackboard;
			}
			set
			{
				if (this._blackboard != value)
				{
					this._blackboard = (Blackboard)value;
					this.UpdateTasksOwner();
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022D9 File Offset: 0x000004D9
		public static ActionListPlayer Create()
		{
			return new GameObject("ActionList").AddComponent<ActionListPlayer>();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022EA File Offset: 0x000004EA
		protected void Awake()
		{
			this.UpdateTasksOwner();
			if (this.playOnAwake)
			{
				this.Play();
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002300 File Offset: 0x00000500
		public void UpdateTasksOwner()
		{
			this.actionList.SetOwnerSystem(this);
			foreach (ActionTask actionTask in this.actionList.actions)
			{
				actionTask.SetOwnerSystem(this);
				BBParameter.SetBBFields(actionTask, this.blackboard);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002370 File Offset: 0x00000570
		void ITaskSystem.SendEvent(string name, object value, object sender)
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002372 File Offset: 0x00000572
		void ITaskSystem.SendEvent<T>(string name, T value, object sender)
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002374 File Offset: 0x00000574
		[ContextMenu("Play")]
		public void Play()
		{
			this.Play(this, this.blackboard, null);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002384 File Offset: 0x00000584
		public void Play(Action<Status> OnFinish)
		{
			this.Play(this, this.blackboard, OnFinish);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002394 File Offset: 0x00000594
		public void Play(Component agent, IBlackboard blackboard, Action<Status> OnFinish)
		{
			if (Application.isPlaying)
			{
				this.timeStarted = Time.time;
				this.actionList.ExecuteIndependent(agent, blackboard, OnFinish);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023B6 File Offset: 0x000005B6
		public Status Execute()
		{
			return this.actionList.Execute(this, this.blackboard);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000023CA File Offset: 0x000005CA
		public Status Execute(Component agent)
		{
			return this.actionList.Execute(agent, this.blackboard);
		}

		// Token: 0x04000006 RID: 6
		public bool playOnAwake;

		// Token: 0x04000007 RID: 7
		[SerializeField]
		private string _serializedList;

		// Token: 0x04000008 RID: 8
		[SerializeField]
		private List<Object> _objectReferences;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		private Blackboard _blackboard;

		// Token: 0x0400000A RID: 10
		[NonSerialized]
		private ActionList _actionList;

		// Token: 0x0400000B RID: 11
		private float timeStarted;
	}
}
