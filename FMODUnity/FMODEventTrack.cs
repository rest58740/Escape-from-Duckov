using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace FMODUnity
{
	// Token: 0x02000107 RID: 263
	[TrackColor(0.066f, 0.134f, 0.244f)]
	[TrackClipType(typeof(FMODEventPlayable))]
	[TrackBindingType(typeof(GameObject))]
	[DisplayName("FMOD/Event Track")]
	public class FMODEventTrack : TrackAsset
	{
		// Token: 0x060006B3 RID: 1715 RVA: 0x00007CA0 File Offset: 0x00005EA0
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			GameObject trackTargetObject = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
			foreach (TimelineClip timelineClip in base.GetClips())
			{
				FMODEventPlayable fmodeventPlayable = timelineClip.asset as FMODEventPlayable;
				if (fmodeventPlayable)
				{
					fmodeventPlayable.TrackTargetObject = trackTargetObject;
					fmodeventPlayable.OwningClip = timelineClip;
				}
			}
			return ScriptPlayable<FMODEventMixerBehaviour>.Create(graph, this.template, inputCount);
		}

		// Token: 0x04000586 RID: 1414
		public FMODEventMixerBehaviour template = new FMODEventMixerBehaviour();
	}
}
