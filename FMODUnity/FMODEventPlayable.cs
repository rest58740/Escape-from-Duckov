using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace FMODUnity
{
	// Token: 0x02000103 RID: 259
	[Serializable]
	public class FMODEventPlayable : PlayableAsset, ITimelineClipAsset
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000696 RID: 1686 RVA: 0x00007574 File Offset: 0x00005774
		// (remove) Token: 0x06000697 RID: 1687 RVA: 0x000075A8 File Offset: 0x000057A8
		public static event EventHandler<EventArgs> OnCreatePlayable;

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x000075DB File Offset: 0x000057DB
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x000075E3 File Offset: 0x000057E3
		public GameObject TrackTargetObject { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x000075EC File Offset: 0x000057EC
		public override double duration
		{
			get
			{
				if (this.EventReference.IsNull)
				{
					return base.duration;
				}
				return (double)this.EventLength;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00007609 File Offset: 0x00005809
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.None;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0000760C File Offset: 0x0000580C
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x00007614 File Offset: 0x00005814
		public TimelineClip OwningClip { get; set; }

		// Token: 0x0600069E RID: 1694 RVA: 0x00007620 File Offset: 0x00005820
		public void LinkParameters(EventDescription eventDescription)
		{
			if (!this.CachedParameters && !this.EventReference.IsNull)
			{
				for (int i = 0; i < this.Parameters.Length; i++)
				{
					PARAMETER_DESCRIPTION parameter_DESCRIPTION;
					eventDescription.getParameterDescriptionByName(this.Parameters[i].Name, out parameter_DESCRIPTION);
					this.Parameters[i].ID = parameter_DESCRIPTION.id;
				}
				List<ParameterAutomationLink> parameterLinks = this.Template.ParameterLinks;
				for (int j = 0; j < parameterLinks.Count; j++)
				{
					PARAMETER_DESCRIPTION parameter_DESCRIPTION2;
					eventDescription.getParameterDescriptionByName(parameterLinks[j].Name, out parameter_DESCRIPTION2);
					parameterLinks[j].ID = parameter_DESCRIPTION2.id;
				}
				this.CachedParameters = true;
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x000076D4 File Offset: 0x000058D4
		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			if (Application.isPlaying)
			{
				this.LinkParameters(RuntimeManager.GetEventDescription(this.EventReference));
			}
			else
			{
				EventArgs e = new EventArgs();
				FMODEventPlayable.OnCreatePlayable(this, e);
			}
			ScriptPlayable<FMODEventPlayableBehavior> playable = ScriptPlayable<FMODEventPlayableBehavior>.Create(graph, this.Template, 0);
			this.behavior = playable.GetBehaviour();
			this.behavior.TrackTargetObject = this.TrackTargetObject;
			this.behavior.EventReference = this.EventReference;
			this.behavior.StopType = this.StopType;
			this.behavior.Parameters = this.Parameters;
			this.behavior.OwningClip = this.OwningClip;
			return playable;
		}

		// Token: 0x04000566 RID: 1382
		[FormerlySerializedAs("template")]
		public FMODEventPlayableBehavior Template = new FMODEventPlayableBehavior();

		// Token: 0x04000567 RID: 1383
		[FormerlySerializedAs("eventLength")]
		public float EventLength;

		// Token: 0x04000568 RID: 1384
		[Obsolete("Use the eventReference field instead")]
		[SerializeField]
		public string eventName;

		// Token: 0x04000569 RID: 1385
		[FormerlySerializedAs("eventReference")]
		[SerializeField]
		public EventReference EventReference;

		// Token: 0x0400056A RID: 1386
		[FormerlySerializedAs("stopType")]
		[SerializeField]
		public STOP_MODE StopType;

		// Token: 0x0400056B RID: 1387
		[FormerlySerializedAs("parameters")]
		[SerializeField]
		public ParamRef[] Parameters = new ParamRef[0];

		// Token: 0x0400056C RID: 1388
		[NonSerialized]
		public bool CachedParameters;

		// Token: 0x0400056E RID: 1390
		private FMODEventPlayableBehavior behavior;
	}
}
