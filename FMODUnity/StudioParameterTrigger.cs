using System;
using FMOD.Studio;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x0200012F RID: 303
	[AddComponentMenu("FMOD Studio/FMOD Studio Parameter Trigger")]
	public class StudioParameterTrigger : EventHandler
	{
		// Token: 0x060007DC RID: 2012 RVA: 0x0000C298 File Offset: 0x0000A498
		private void Awake()
		{
			for (int i = 0; i < this.Emitters.Length; i++)
			{
				EmitterRef emitterRef = this.Emitters[i];
				if (emitterRef.Target != null && !emitterRef.Target.EventReference.IsNull)
				{
					EventDescription eventDescription = RuntimeManager.GetEventDescription(emitterRef.Target.EventReference);
					if (eventDescription.isValid())
					{
						for (int j = 0; j < this.Emitters[i].Params.Length; j++)
						{
							PARAMETER_DESCRIPTION parameter_DESCRIPTION;
							eventDescription.getParameterDescriptionByName(emitterRef.Params[j].Name, out parameter_DESCRIPTION);
							emitterRef.Params[j].ID = parameter_DESCRIPTION.id;
						}
					}
				}
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0000C346 File Offset: 0x0000A546
		protected override void HandleGameEvent(EmitterGameEvent gameEvent)
		{
			if (this.TriggerEvent == gameEvent)
			{
				this.TriggerParameters();
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0000C358 File Offset: 0x0000A558
		public void TriggerParameters()
		{
			for (int i = 0; i < this.Emitters.Length; i++)
			{
				EmitterRef emitterRef = this.Emitters[i];
				if (emitterRef.Target != null && emitterRef.Target.EventInstance.isValid())
				{
					for (int j = 0; j < this.Emitters[i].Params.Length; j++)
					{
						emitterRef.Target.EventInstance.setParameterByID(this.Emitters[i].Params[j].ID, this.Emitters[i].Params[j].Value, false);
					}
				}
			}
		}

		// Token: 0x0400067D RID: 1661
		public EmitterRef[] Emitters;

		// Token: 0x0400067E RID: 1662
		public EmitterGameEvent TriggerEvent;
	}
}
