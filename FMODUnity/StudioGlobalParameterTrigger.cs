using System;
using FMOD;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace FMODUnity
{
	// Token: 0x0200012C RID: 300
	[AddComponentMenu("FMOD Studio/FMOD Studio Global Parameter Trigger")]
	public class StudioGlobalParameterTrigger : EventHandler
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x0000BDDA File Offset: 0x00009FDA
		public PARAMETER_DESCRIPTION ParameterDescription
		{
			get
			{
				return this.parameterDescription;
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0000BDE2 File Offset: 0x00009FE2
		protected override void HandleGameEvent(EmitterGameEvent gameEvent)
		{
			if (this.TriggerEvent == gameEvent)
			{
				this.TriggerParameters();
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0000BDF4 File Offset: 0x00009FF4
		public void TriggerParameters()
		{
			if (!string.IsNullOrEmpty(this.Parameter))
			{
				RESULT result;
				if (string.IsNullOrEmpty(this.parameterDescription.name))
				{
					result = RuntimeManager.StudioSystem.getParameterDescriptionByName(this.Parameter, out this.parameterDescription);
					if (result != RESULT.OK)
					{
						RuntimeUtils.DebugLogError(string.Format("[FMOD] StudioGlobalParameterTrigger failed to lookup parameter {0} : result = {1}", this.Parameter, result));
						return;
					}
				}
				result = RuntimeManager.StudioSystem.setParameterByID(this.parameterDescription.id, this.Value, false);
				if (result != RESULT.OK)
				{
					RuntimeUtils.DebugLogError(string.Format("[FMOD] StudioGlobalParameterTrigger failed to set parameter {0} : result = {1}", this.Parameter, result));
					return;
				}
			}
		}

		// Token: 0x04000671 RID: 1649
		[ParamRef]
		[FormerlySerializedAs("parameter")]
		public string Parameter;

		// Token: 0x04000672 RID: 1650
		public EmitterGameEvent TriggerEvent;

		// Token: 0x04000673 RID: 1651
		[FormerlySerializedAs("value")]
		public float Value;

		// Token: 0x04000674 RID: 1652
		private PARAMETER_DESCRIPTION parameterDescription;
	}
}
