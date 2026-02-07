using System;
using FMOD;
using FMOD.Studio;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x0200010C RID: 268
	public class PlatformCallbackHandler : ScriptableObject
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x00007DDC File Offset: 0x00005FDC
		public virtual void PreInitialize(FMOD.Studio.System system, Action<RESULT, string> reportResult)
		{
		}
	}
}
