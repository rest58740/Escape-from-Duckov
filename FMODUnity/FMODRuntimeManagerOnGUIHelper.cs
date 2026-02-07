using System;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x02000109 RID: 265
	public class FMODRuntimeManagerOnGUIHelper : MonoBehaviour
	{
		// Token: 0x060006B7 RID: 1719 RVA: 0x00007DAA File Offset: 0x00005FAA
		private void OnGUI()
		{
			if (this.TargetRuntimeManager)
			{
				this.TargetRuntimeManager.ExecuteOnGUI();
			}
		}

		// Token: 0x04000588 RID: 1416
		public RuntimeManager TargetRuntimeManager;
	}
}
