using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace FMODUnity
{
	// Token: 0x0200012B RID: 299
	[AddComponentMenu("FMOD Studio/FMOD Studio Event Emitter")]
	public class StudioEventEmitter : EventHandler
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0000B5B0 File Offset: 0x000097B0
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0000B5B8 File Offset: 0x000097B8
		[Obsolete("Use the EventPlayTrigger field instead")]
		public EmitterGameEvent PlayEvent
		{
			get
			{
				return this.EventPlayTrigger;
			}
			set
			{
				this.EventPlayTrigger = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0000B5C1 File Offset: 0x000097C1
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0000B5C9 File Offset: 0x000097C9
		[Obsolete("Use the EventStopTrigger field instead")]
		public EmitterGameEvent StopEvent
		{
			get
			{
				return this.EventStopTrigger;
			}
			set
			{
				this.EventStopTrigger = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0000B5D2 File Offset: 0x000097D2
		public EventDescription EventDescription
		{
			get
			{
				return this.eventDescription;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0000B5DA File Offset: 0x000097DA
		public EventInstance EventInstance
		{
			get
			{
				return this.instance;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0000B5E2 File Offset: 0x000097E2
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0000B5EA File Offset: 0x000097EA
		public bool IsActive { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0000B5F4 File Offset: 0x000097F4
		private float MaxDistance
		{
			get
			{
				if (this.OverrideAttenuation)
				{
					return this.OverrideMaxDistance;
				}
				if (!this.eventDescription.isValid())
				{
					this.Lookup();
				}
				float num;
				float result;
				this.eventDescription.getMinMaxDistance(out num, out result);
				return result;
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0000B634 File Offset: 0x00009834
		public static void UpdateActiveEmitters()
		{
			foreach (StudioEventEmitter studioEventEmitter in StudioEventEmitter.activeEmitters)
			{
				studioEventEmitter.UpdatePlayingStatus(false);
			}
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0000B684 File Offset: 0x00009884
		private static void RegisterActiveEmitter(StudioEventEmitter emitter)
		{
			if (!StudioEventEmitter.activeEmitters.Contains(emitter))
			{
				StudioEventEmitter.activeEmitters.Add(emitter);
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0000B69E File Offset: 0x0000989E
		private static void DeregisterActiveEmitter(StudioEventEmitter emitter)
		{
			StudioEventEmitter.activeEmitters.Remove(emitter);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0000B6AC File Offset: 0x000098AC
		private void UpdatePlayingStatus(bool force = false)
		{
			bool flag = StudioListener.DistanceSquaredToNearestListener(base.transform.position) <= this.MaxDistance * this.MaxDistance;
			if (force || flag != this.IsPlaying())
			{
				if (flag)
				{
					this.PlayInstance();
					return;
				}
				this.StopInstance();
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0000B6F8 File Offset: 0x000098F8
		protected override void Start()
		{
			RuntimeUtils.EnforceLibraryOrder();
			if (this.Preload)
			{
				this.Lookup();
				this.eventDescription.loadSampleData();
			}
			this.HandleGameEvent(EmitterGameEvent.ObjectStart);
			if (this.NonRigidbodyVelocity && base.GetComponent<Rigidbody>())
			{
				Debug.LogWarning(string.Format("[FMOD] Non-Rigidbody Velocity is enabled on Emitter attached to GameObject \"{0}\", which also has a Rigidbody component attached - this will be disabled in favor of velocity from Rigidbody component.", base.name));
				this.NonRigidbodyVelocity = false;
			}
			if (this.NonRigidbodyVelocity && base.GetComponent<Rigidbody2D>())
			{
				Debug.LogWarning(string.Format("[FMOD] Non-Rigidbody Velocity is enabled on Emitter attached to GameObject \"{0}\", which also has a Rigidbody2D component attached - this will be disabled in favor of velocity from Rigidbody2D component.", base.name));
				this.NonRigidbodyVelocity = false;
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0000B78D File Offset: 0x0000998D
		private void OnApplicationQuit()
		{
			this.isQuitting = true;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0000B798 File Offset: 0x00009998
		protected override void OnDestroy()
		{
			if (!this.isQuitting)
			{
				this.HandleGameEvent(EmitterGameEvent.ObjectDestroy);
				if (this.instance.isValid())
				{
					RuntimeManager.DetachInstanceFromGameObject(this.instance);
					if (this.eventDescription.isValid() && this.isOneshot)
					{
						this.instance.release();
						this.instance.clearHandle();
					}
				}
				StudioEventEmitter.DeregisterActiveEmitter(this);
				if (this.Preload)
				{
					this.eventDescription.unloadSampleData();
				}
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0000B812 File Offset: 0x00009A12
		protected override void HandleGameEvent(EmitterGameEvent gameEvent)
		{
			if (this.EventPlayTrigger == gameEvent)
			{
				this.Play();
			}
			if (this.EventStopTrigger == gameEvent)
			{
				this.Stop();
			}
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0000B834 File Offset: 0x00009A34
		private void Lookup()
		{
			this.eventDescription = RuntimeManager.GetEventDescription(this.EventReference);
			if (this.eventDescription.isValid())
			{
				for (int i = 0; i < this.Params.Length; i++)
				{
					PARAMETER_DESCRIPTION parameter_DESCRIPTION;
					this.eventDescription.getParameterDescriptionByName(this.Params[i].Name, out parameter_DESCRIPTION);
					this.Params[i].ID = parameter_DESCRIPTION.id;
				}
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0000B8A0 File Offset: 0x00009AA0
		public void Play()
		{
			if (this.TriggerOnce && this.hasTriggered)
			{
				return;
			}
			if (this.EventReference.IsNull)
			{
				return;
			}
			this.cachedParams.Clear();
			if (!this.eventDescription.isValid())
			{
				this.Lookup();
			}
			bool flag;
			this.eventDescription.isSnapshot(out flag);
			if (!flag)
			{
				this.eventDescription.isOneshot(out this.isOneshot);
			}
			bool flag2;
			this.eventDescription.is3D(out flag2);
			this.IsActive = true;
			if (flag2 && Settings.Instance.StopEventsOutsideMaxDistance)
			{
				if (!this.isOneshot)
				{
					StudioEventEmitter.RegisterActiveEmitter(this);
				}
				this.UpdatePlayingStatus(true);
				return;
			}
			this.PlayInstance();
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0000B950 File Offset: 0x00009B50
		private void PlayInstance()
		{
			if (!this.instance.isValid())
			{
				this.instance.clearHandle();
			}
			if (this.isOneshot && this.instance.isValid())
			{
				this.instance.release();
				this.instance.clearHandle();
			}
			bool flag;
			this.eventDescription.is3D(out flag);
			if (!this.instance.isValid())
			{
				this.eventDescription.createInstance(out this.instance);
				if (flag)
				{
					base.GetComponent<Transform>();
					if (base.GetComponent<Rigidbody>())
					{
						Rigidbody component = base.GetComponent<Rigidbody>();
						this.instance.set3DAttributes(RuntimeUtils.To3DAttributes(base.gameObject, component));
						RuntimeManager.AttachInstanceToGameObject(this.instance, base.gameObject, component);
					}
					else if (base.GetComponent<Rigidbody2D>())
					{
						Rigidbody2D component2 = base.GetComponent<Rigidbody2D>();
						this.instance.set3DAttributes(RuntimeUtils.To3DAttributes(base.gameObject, component2));
						RuntimeManager.AttachInstanceToGameObject(this.instance, base.gameObject, component2);
					}
					else
					{
						this.instance.set3DAttributes(base.gameObject.To3DAttributes());
						RuntimeManager.AttachInstanceToGameObject(this.instance, base.gameObject, this.NonRigidbodyVelocity);
					}
				}
			}
			foreach (ParamRef paramRef in this.Params)
			{
				this.instance.setParameterByID(paramRef.ID, paramRef.Value, false);
			}
			foreach (ParamRef paramRef2 in this.cachedParams)
			{
				this.instance.setParameterByID(paramRef2.ID, paramRef2.Value, false);
			}
			if (flag && this.OverrideAttenuation)
			{
				this.instance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, this.OverrideMinDistance);
				this.instance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, this.OverrideMaxDistance);
			}
			this.instance.start();
			this.hasTriggered = true;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0000BB68 File Offset: 0x00009D68
		public void Stop()
		{
			StudioEventEmitter.DeregisterActiveEmitter(this);
			this.IsActive = false;
			this.cachedParams.Clear();
			this.StopInstance();
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0000BB88 File Offset: 0x00009D88
		private void StopInstance()
		{
			if (this.TriggerOnce && this.hasTriggered)
			{
				StudioEventEmitter.DeregisterActiveEmitter(this);
			}
			if (this.instance.isValid())
			{
				this.instance.stop(this.AllowFadeout ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE);
				this.instance.release();
				if (!this.AllowFadeout)
				{
					this.instance.clearHandle();
				}
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0000BBF0 File Offset: 0x00009DF0
		public void SetParameter(string name, float value, bool ignoreseekspeed = false)
		{
			if (Settings.Instance.StopEventsOutsideMaxDistance && this.IsActive)
			{
				string findName = name;
				ParamRef paramRef = this.cachedParams.Find((ParamRef x) => x.Name == findName);
				if (paramRef == null)
				{
					PARAMETER_DESCRIPTION parameter_DESCRIPTION;
					this.eventDescription.getParameterDescriptionByName(name, out parameter_DESCRIPTION);
					paramRef = new ParamRef();
					paramRef.ID = parameter_DESCRIPTION.id;
					paramRef.Name = parameter_DESCRIPTION.name;
					this.cachedParams.Add(paramRef);
				}
				paramRef.Value = value;
			}
			if (this.instance.isValid())
			{
				this.instance.setParameterByName(name, value, ignoreseekspeed);
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0000BC9C File Offset: 0x00009E9C
		public void SetParameter(PARAMETER_ID id, float value, bool ignoreseekspeed = false)
		{
			if (Settings.Instance.StopEventsOutsideMaxDistance && this.IsActive)
			{
				PARAMETER_ID findId = id;
				ParamRef paramRef = this.cachedParams.Find((ParamRef x) => x.ID.Equals(findId));
				if (paramRef == null)
				{
					PARAMETER_DESCRIPTION parameter_DESCRIPTION;
					this.eventDescription.getParameterDescriptionByID(id, out parameter_DESCRIPTION);
					paramRef = new ParamRef();
					paramRef.ID = parameter_DESCRIPTION.id;
					paramRef.Name = parameter_DESCRIPTION.name;
					this.cachedParams.Add(paramRef);
				}
				paramRef.Value = value;
			}
			if (this.instance.isValid())
			{
				this.instance.setParameterByID(id, value, ignoreseekspeed);
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0000BD48 File Offset: 0x00009F48
		public bool IsPlaying()
		{
			if (this.instance.isValid())
			{
				PLAYBACK_STATE playback_STATE;
				this.instance.getPlaybackState(out playback_STATE);
				return playback_STATE != PLAYBACK_STATE.STOPPED;
			}
			return false;
		}

		// Token: 0x0400065C RID: 1628
		public EventReference EventReference;

		// Token: 0x0400065D RID: 1629
		[Obsolete("Use the EventReference field instead")]
		public string Event = "";

		// Token: 0x0400065E RID: 1630
		[FormerlySerializedAs("PlayEvent")]
		public EmitterGameEvent EventPlayTrigger;

		// Token: 0x0400065F RID: 1631
		[FormerlySerializedAs("StopEvent")]
		public EmitterGameEvent EventStopTrigger;

		// Token: 0x04000660 RID: 1632
		public bool AllowFadeout = true;

		// Token: 0x04000661 RID: 1633
		public bool TriggerOnce;

		// Token: 0x04000662 RID: 1634
		public bool Preload;

		// Token: 0x04000663 RID: 1635
		[FormerlySerializedAs("AllowNonRigidbodyDoppler")]
		public bool NonRigidbodyVelocity;

		// Token: 0x04000664 RID: 1636
		public ParamRef[] Params = new ParamRef[0];

		// Token: 0x04000665 RID: 1637
		public bool OverrideAttenuation;

		// Token: 0x04000666 RID: 1638
		public float OverrideMinDistance = -1f;

		// Token: 0x04000667 RID: 1639
		public float OverrideMaxDistance = -1f;

		// Token: 0x04000668 RID: 1640
		protected EventDescription eventDescription;

		// Token: 0x04000669 RID: 1641
		protected EventInstance instance;

		// Token: 0x0400066A RID: 1642
		private bool hasTriggered;

		// Token: 0x0400066B RID: 1643
		private bool isQuitting;

		// Token: 0x0400066C RID: 1644
		private bool isOneshot;

		// Token: 0x0400066D RID: 1645
		private List<ParamRef> cachedParams = new List<ParamRef>();

		// Token: 0x0400066E RID: 1646
		private static List<StudioEventEmitter> activeEmitters = new List<StudioEventEmitter>();

		// Token: 0x0400066F RID: 1647
		private const string SnapshotString = "snapshot";
	}
}
