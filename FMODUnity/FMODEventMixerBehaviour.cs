using System;
using UnityEngine;
using UnityEngine.Playables;

namespace FMODUnity
{
	// Token: 0x02000108 RID: 264
	[Serializable]
	public class FMODEventMixerBehaviour : PlayableBehaviour
	{
		// Token: 0x060006B5 RID: 1717 RVA: 0x00007D40 File Offset: 0x00005F40
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			int inputCount = playable.GetInputCount<Playable>();
			float time = (float)playable.GetGraph<Playable>().GetRootPlayable(0).GetTime<Playable>();
			for (int i = 0; i < inputCount; i++)
			{
				((ScriptPlayable<T>)playable.GetInput(i)).GetBehaviour().UpdateBehavior(time, this.volume);
			}
		}

		// Token: 0x04000587 RID: 1415
		[Range(0f, 1f)]
		public float volume = 1f;
	}
}
