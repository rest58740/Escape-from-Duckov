using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace FMODUnity
{
	// Token: 0x02000106 RID: 262
	[Serializable]
	public class FMODEventPlayableBehavior : PlayableBehaviour
	{
		// Token: 0x060006A2 RID: 1698 RVA: 0x000077AA File Offset: 0x000059AA
		public FMODEventPlayableBehavior()
		{
			this.CurrentVolume = 1f;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060006A3 RID: 1699 RVA: 0x000077D4 File Offset: 0x000059D4
		// (remove) Token: 0x060006A4 RID: 1700 RVA: 0x00007808 File Offset: 0x00005A08
		public static event EventHandler<FMODEventPlayableBehavior.EventArgs> Enter;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060006A5 RID: 1701 RVA: 0x0000783C File Offset: 0x00005A3C
		// (remove) Token: 0x060006A6 RID: 1702 RVA: 0x00007870 File Offset: 0x00005A70
		public static event EventHandler<FMODEventPlayableBehavior.EventArgs> Exit;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060006A7 RID: 1703 RVA: 0x000078A4 File Offset: 0x00005AA4
		// (remove) Token: 0x060006A8 RID: 1704 RVA: 0x000078D8 File Offset: 0x00005AD8
		public static event EventHandler<FMODEventPlayableBehavior.EventArgs> GraphStop;

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0000790B File Offset: 0x00005B0B
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x00007913 File Offset: 0x00005B13
		public float ClipStartTime { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0000791C File Offset: 0x00005B1C
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x00007924 File Offset: 0x00005B24
		public float CurrentVolume { get; private set; }

		// Token: 0x060006AD RID: 1709 RVA: 0x00007930 File Offset: 0x00005B30
		protected void PlayEvent()
		{
			if (!this.EventReference.IsNull)
			{
				this.eventInstance = RuntimeManager.CreateInstance(this.EventReference);
				if (Application.isPlaying && this.TrackTargetObject)
				{
					if (this.TrackTargetObject.GetComponent<Rigidbody>())
					{
						RuntimeManager.AttachInstanceToGameObject(this.eventInstance, this.TrackTargetObject, this.TrackTargetObject.GetComponent<Rigidbody>());
					}
					else if (this.TrackTargetObject.GetComponent<Rigidbody2D>())
					{
						RuntimeManager.AttachInstanceToGameObject(this.eventInstance, this.TrackTargetObject, this.TrackTargetObject.GetComponent<Rigidbody2D>());
					}
					else
					{
						RuntimeManager.AttachInstanceToGameObject(this.eventInstance, this.TrackTargetObject, false);
					}
				}
				else
				{
					this.eventInstance.set3DAttributes(Vector3.zero.To3DAttributes());
				}
				foreach (ParamRef paramRef in this.Parameters)
				{
					this.eventInstance.setParameterByID(paramRef.ID, paramRef.Value, false);
				}
				this.eventInstance.setVolume(this.CurrentVolume);
				this.eventInstance.setTimelinePosition((int)(this.ClipStartTime * 1000f));
				this.eventInstance.start();
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00007A68 File Offset: 0x00005C68
		protected virtual void OnEnter()
		{
			if (!this.isPlayheadInside)
			{
				this.isPlayheadInside = true;
				if (Application.isPlaying)
				{
					this.PlayEvent();
					return;
				}
				FMODEventPlayableBehavior.EventArgs eventArgs = new FMODEventPlayableBehavior.EventArgs();
				FMODEventPlayableBehavior.Enter(this, eventArgs);
				this.eventInstance = eventArgs.eventInstance;
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00007AB0 File Offset: 0x00005CB0
		protected virtual void OnExit()
		{
			if (this.isPlayheadInside)
			{
				this.isPlayheadInside = false;
				if (Application.isPlaying)
				{
					if (this.eventInstance.isValid())
					{
						if (this.StopType != STOP_MODE.None)
						{
							this.eventInstance.stop((this.StopType == STOP_MODE.Immediate) ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);
						}
						this.eventInstance.release();
						this.eventInstance.clearHandle();
						return;
					}
				}
				else
				{
					FMODEventPlayableBehavior.EventArgs eventArgs = new FMODEventPlayableBehavior.EventArgs();
					eventArgs.eventInstance = this.eventInstance;
					FMODEventPlayableBehavior.Exit(this, eventArgs);
				}
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00007B38 File Offset: 0x00005D38
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (this.eventInstance.isValid())
			{
				foreach (ParameterAutomationLink parameterAutomationLink in this.ParameterLinks)
				{
					float value = this.ParameterAutomation.GetValue(parameterAutomationLink.Slot);
					this.eventInstance.setParameterByID(parameterAutomationLink.ID, value, false);
				}
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00007BB8 File Offset: 0x00005DB8
		public void UpdateBehavior(float time, float volume)
		{
			if (volume != this.CurrentVolume)
			{
				this.CurrentVolume = volume;
				if (this.eventInstance.isValid())
				{
					this.eventInstance.setVolume(volume);
				}
			}
			if ((double)time >= this.OwningClip.start && (double)time < this.OwningClip.end)
			{
				this.ClipStartTime = time - (float)this.OwningClip.start;
				this.OnEnter();
				return;
			}
			this.OnExit();
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00007C30 File Offset: 0x00005E30
		public override void OnGraphStop(Playable playable)
		{
			this.isPlayheadInside = false;
			if (Application.isPlaying)
			{
				if (this.eventInstance.isValid())
				{
					this.eventInstance.stop(STOP_MODE.IMMEDIATE);
					this.eventInstance.release();
					RuntimeManager.StudioSystem.update();
					return;
				}
			}
			else
			{
				FMODEventPlayableBehavior.EventArgs eventArgs = new FMODEventPlayableBehavior.EventArgs();
				eventArgs.eventInstance = this.eventInstance;
				FMODEventPlayableBehavior.GraphStop(this, eventArgs);
			}
		}

		// Token: 0x0400057B RID: 1403
		[FormerlySerializedAs("eventReference")]
		public EventReference EventReference;

		// Token: 0x0400057C RID: 1404
		[FormerlySerializedAs("stopType")]
		public STOP_MODE StopType;

		// Token: 0x0400057D RID: 1405
		[FormerlySerializedAs("parameters")]
		[NotKeyable]
		public ParamRef[] Parameters = new ParamRef[0];

		// Token: 0x0400057E RID: 1406
		[FormerlySerializedAs("parameterLinks")]
		public List<ParameterAutomationLink> ParameterLinks = new List<ParameterAutomationLink>();

		// Token: 0x0400057F RID: 1407
		[NonSerialized]
		public GameObject TrackTargetObject;

		// Token: 0x04000580 RID: 1408
		[NonSerialized]
		public TimelineClip OwningClip;

		// Token: 0x04000581 RID: 1409
		[FormerlySerializedAs("parameterAutomation")]
		public AutomatableSlots ParameterAutomation;

		// Token: 0x04000582 RID: 1410
		private bool isPlayheadInside;

		// Token: 0x04000583 RID: 1411
		private EventInstance eventInstance;

		// Token: 0x02000133 RID: 307
		public class EventArgs : System.EventArgs
		{
			// Token: 0x17000067 RID: 103
			// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0000C647 File Offset: 0x0000A847
			// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0000C64F File Offset: 0x0000A84F
			public EventInstance eventInstance { get; set; }
		}
	}
}
