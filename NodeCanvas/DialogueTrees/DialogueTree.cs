using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000F5 RID: 245
	[GraphInfo(packageName = "NodeCanvas", docsURL = "https://nodecanvas.paradoxnotion.com/documentation/", resourcesURL = "https://nodecanvas.paradoxnotion.com/downloads/", forumsURL = "https://nodecanvas.paradoxnotion.com/forums-page/")]
	[CreateAssetMenu(menuName = "ParadoxNotion/NodeCanvas/Dialogue Tree Asset")]
	public class DialogueTree : Graph
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x0001080C File Offset: 0x0000EA0C
		public override object OnDerivedDataSerialization()
		{
			return new DialogueTree.DerivedSerializationData
			{
				actorParameters = this.actorParameters
			};
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001081F File Offset: 0x0000EA1F
		public override void OnDerivedDataDeserialization(object data)
		{
			if (data is DialogueTree.DerivedSerializationData)
			{
				this.actorParameters = ((DialogueTree.DerivedSerializationData)data).actorParameters;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060004C7 RID: 1223 RVA: 0x0001083C File Offset: 0x0000EA3C
		// (remove) Token: 0x060004C8 RID: 1224 RVA: 0x00010870 File Offset: 0x0000EA70
		public static event Action<DialogueTree> OnDialogueStarted;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060004C9 RID: 1225 RVA: 0x000108A4 File Offset: 0x0000EAA4
		// (remove) Token: 0x060004CA RID: 1226 RVA: 0x000108D8 File Offset: 0x0000EAD8
		public static event Action<DialogueTree> OnDialoguePaused;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060004CB RID: 1227 RVA: 0x0001090C File Offset: 0x0000EB0C
		// (remove) Token: 0x060004CC RID: 1228 RVA: 0x00010940 File Offset: 0x0000EB40
		public static event Action<DialogueTree> OnDialogueFinished;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060004CD RID: 1229 RVA: 0x00010974 File Offset: 0x0000EB74
		// (remove) Token: 0x060004CE RID: 1230 RVA: 0x000109A8 File Offset: 0x0000EBA8
		public static event Action<SubtitlesRequestInfo> OnSubtitlesRequest;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060004CF RID: 1231 RVA: 0x000109DC File Offset: 0x0000EBDC
		// (remove) Token: 0x060004D0 RID: 1232 RVA: 0x00010A10 File Offset: 0x0000EC10
		public static event Action<MultipleChoiceRequestInfo> OnMultipleChoiceRequest;

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00010A43 File Offset: 0x0000EC43
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00010A4A File Offset: 0x0000EC4A
		public static DialogueTree currentDialogue { get; private set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00010A52 File Offset: 0x0000EC52
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00010A59 File Offset: 0x0000EC59
		public static DialogueTree previousDialogue { get; private set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00010A61 File Offset: 0x0000EC61
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00010A69 File Offset: 0x0000EC69
		public DTNode currentNode { get; private set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00010A72 File Offset: 0x0000EC72
		public override Type baseNodeType
		{
			get
			{
				return typeof(DTNode);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00010A7E File Offset: 0x0000EC7E
		public override bool requiresAgent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00010A81 File Offset: 0x0000EC81
		public override bool requiresPrimeNode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00010A84 File Offset: 0x0000EC84
		public override bool isTree
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00010A87 File Offset: 0x0000EC87
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00010A8A File Offset: 0x0000EC8A
		public sealed override bool canAcceptVariableDrops
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00010A8D File Offset: 0x0000EC8D
		public sealed override PlanarDirection flowDirection
		{
			get
			{
				return PlanarDirection.Vertical;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00010A90 File Offset: 0x0000EC90
		public List<string> definedActorParameterNames
		{
			get
			{
				List<string> list = (from r in this.actorParameters
				select r.name).ToList<string>();
				list.Insert(0, "SELF");
				return list;
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00010AD0 File Offset: 0x0000ECD0
		public DialogueTree.ActorParameter GetParameterByID(string id)
		{
			return this.actorParameters.Find((DialogueTree.ActorParameter p) => p.ID == id);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00010B04 File Offset: 0x0000ED04
		public DialogueTree.ActorParameter GetParameterByName(string paramName)
		{
			return this.actorParameters.Find((DialogueTree.ActorParameter p) => p.name == paramName);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00010B38 File Offset: 0x0000ED38
		public IDialogueActor GetActorReferenceByID(string id)
		{
			DialogueTree.ActorParameter parameterByID = this.GetParameterByID(id);
			if (parameterByID == null)
			{
				return null;
			}
			return this.GetActorReferenceByName(parameterByID.name);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00010B60 File Offset: 0x0000ED60
		public IDialogueActor GetActorReferenceByName(string paramName)
		{
			if (paramName == "SELF")
			{
				if (base.agent is IDialogueActor)
				{
					return (IDialogueActor)base.agent;
				}
				if (base.agent != null)
				{
					return new ProxyDialogueActor(base.agent.gameObject.name, base.agent.transform);
				}
				return new ProxyDialogueActor("NO ACTOR", null);
			}
			else
			{
				DialogueTree.ActorParameter actorParameter = this.actorParameters.Find((DialogueTree.ActorParameter r) => r.name == paramName);
				if (actorParameter != null && actorParameter.actor != null)
				{
					return actorParameter.actor;
				}
				return new ProxyDialogueActor(paramName, null);
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00010C18 File Offset: 0x0000EE18
		public void SetActorReference(string paramName, IDialogueActor actor)
		{
			DialogueTree.ActorParameter actorParameter = this.actorParameters.Find((DialogueTree.ActorParameter p) => p.name == paramName);
			if (actorParameter == null)
			{
				return;
			}
			actorParameter.actor = actor;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00010C58 File Offset: 0x0000EE58
		public void SetActorReferences(Dictionary<string, IDialogueActor> actors)
		{
			using (Dictionary<string, IDialogueActor>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, IDialogueActor> pair = enumerator.Current;
					DialogueTree.ActorParameter actorParameter = this.actorParameters.Find((DialogueTree.ActorParameter p) => p.name == pair.Key);
					if (actorParameter != null)
					{
						actorParameter.actor = pair.Value;
					}
				}
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00010CD8 File Offset: 0x0000EED8
		public void Continue(int index = 0)
		{
			if (index < 0 || index > this.currentNode.outConnections.Count - 1)
			{
				base.Stop(true);
				return;
			}
			this.currentNode.outConnections[index].status = Status.Success;
			this.EnterNode((DTNode)this.currentNode.outConnections[index].targetNode);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00010D3E File Offset: 0x0000EF3E
		public void EnterNode(DTNode node)
		{
			this.currentNode = node;
			this.currentNode.Reset(false);
			if (this.currentNode.Execute(base.agent, base.blackboard) == Status.Error)
			{
				base.Stop(false);
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00010D74 File Offset: 0x0000EF74
		public static void RequestSubtitles(SubtitlesRequestInfo info)
		{
			if (DialogueTree.OnSubtitlesRequest != null)
			{
				DialogueTree.OnSubtitlesRequest.Invoke(info);
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00010D88 File Offset: 0x0000EF88
		public static void RequestMultipleChoices(MultipleChoiceRequestInfo info)
		{
			if (DialogueTree.OnMultipleChoiceRequest != null)
			{
				DialogueTree.OnMultipleChoiceRequest.Invoke(info);
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00010D9C File Offset: 0x0000EF9C
		protected override void OnGraphStarted()
		{
			DialogueTree.previousDialogue = DialogueTree.currentDialogue;
			DialogueTree.currentDialogue = this;
			if (DialogueTree.OnDialogueStarted != null)
			{
				DialogueTree.OnDialogueStarted.Invoke(this);
			}
			IDialogueActor dialogueActor = base.agent as IDialogueActor;
			this.enterStartNodeFlag = true;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00010DD4 File Offset: 0x0000EFD4
		protected override void OnGraphUpdate()
		{
			if (this.enterStartNodeFlag)
			{
				this.enterStartNodeFlag = false;
				this.EnterNode((this.currentNode != null) ? this.currentNode : ((DTNode)base.primeNode));
			}
			if (this.currentNode is IUpdatable)
			{
				(this.currentNode as IUpdatable).Update();
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00010E2E File Offset: 0x0000F02E
		protected override void OnGraphStoped()
		{
			DialogueTree.currentDialogue = DialogueTree.previousDialogue;
			DialogueTree.previousDialogue = null;
			this.currentNode = null;
			if (DialogueTree.OnDialogueFinished != null)
			{
				DialogueTree.OnDialogueFinished.Invoke(this);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00010E59 File Offset: 0x0000F059
		protected override void OnGraphPaused()
		{
			if (DialogueTree.OnDialoguePaused != null)
			{
				DialogueTree.OnDialoguePaused.Invoke(this);
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00010E6D File Offset: 0x0000F06D
		protected override void OnGraphUnpaused()
		{
			this.EnterNode((this.currentNode != null) ? this.currentNode : ((DTNode)base.primeNode));
			if (DialogueTree.OnDialogueStarted != null)
			{
				DialogueTree.OnDialogueStarted.Invoke(this);
			}
		}

		// Token: 0x040002CD RID: 717
		public const string INSTIGATOR_NAME = "SELF";

		// Token: 0x040002CE RID: 718
		[SerializeField]
		public List<DialogueTree.ActorParameter> actorParameters = new List<DialogueTree.ActorParameter>();

		// Token: 0x040002CF RID: 719
		private bool enterStartNodeFlag;

		// Token: 0x02000151 RID: 337
		[Serializable]
		private class DerivedSerializationData
		{
			// Token: 0x040003D0 RID: 976
			public List<DialogueTree.ActorParameter> actorParameters;
		}

		// Token: 0x02000152 RID: 338
		[Serializable]
		public class ActorParameter
		{
			// Token: 0x170001EA RID: 490
			// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001474B File Offset: 0x0001294B
			// (set) Token: 0x060006A2 RID: 1698 RVA: 0x00014753 File Offset: 0x00012953
			public string name
			{
				get
				{
					return this._keyName;
				}
				set
				{
					this._keyName = value;
				}
			}

			// Token: 0x170001EB RID: 491
			// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0001475C File Offset: 0x0001295C
			public string ID
			{
				get
				{
					if (!string.IsNullOrEmpty(this._id))
					{
						return this._id;
					}
					return this._id = Guid.NewGuid().ToString();
				}
			}

			// Token: 0x170001EC RID: 492
			// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00014799 File Offset: 0x00012999
			// (set) Token: 0x060006A5 RID: 1701 RVA: 0x000147BA File Offset: 0x000129BA
			public IDialogueActor actor
			{
				get
				{
					if (this._actor == null)
					{
						this._actor = (this._actorObject as IDialogueActor);
					}
					return this._actor;
				}
				set
				{
					this._actor = value;
					this._actorObject = (value as Object);
				}
			}

			// Token: 0x060006A6 RID: 1702 RVA: 0x000147CF File Offset: 0x000129CF
			public ActorParameter()
			{
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x000147D7 File Offset: 0x000129D7
			public ActorParameter(string name)
			{
				this.name = name;
			}

			// Token: 0x060006A8 RID: 1704 RVA: 0x000147E6 File Offset: 0x000129E6
			public ActorParameter(string name, IDialogueActor actor)
			{
				this.name = name;
				this.actor = actor;
			}

			// Token: 0x060006A9 RID: 1705 RVA: 0x000147FC File Offset: 0x000129FC
			public override string ToString()
			{
				return this.name;
			}

			// Token: 0x040003D1 RID: 977
			[SerializeField]
			private string _keyName;

			// Token: 0x040003D2 RID: 978
			[SerializeField]
			private string _id;

			// Token: 0x040003D3 RID: 979
			[SerializeField]
			private Object _actorObject;

			// Token: 0x040003D4 RID: 980
			[NonSerialized]
			private IDialogueActor _actor;
		}
	}
}
