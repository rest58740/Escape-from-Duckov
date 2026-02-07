using System;
using Cinemachine;
using NodeCanvas.Framework;

// Token: 0x020001BC RID: 444
public class AT_SetVirtualCamera : ActionTask
{
	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000D5E RID: 3422 RVA: 0x000382E6 File Offset: 0x000364E6
	protected override string info
	{
		get
		{
			return "Set camera :" + ((this.target.value == null) ? "Empty" : this.target.value.name);
		}
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x0003831C File Offset: 0x0003651C
	protected override void OnExecute()
	{
		base.OnExecute();
		if (AT_SetVirtualCamera.cachedVCam != null)
		{
			AT_SetVirtualCamera.cachedVCam.gameObject.SetActive(false);
		}
		if (this.target.value != null)
		{
			this.target.value.gameObject.SetActive(true);
			AT_SetVirtualCamera.cachedVCam = this.target.value;
		}
		else
		{
			AT_SetVirtualCamera.cachedVCam = null;
		}
		base.EndAction();
	}

	// Token: 0x04000B93 RID: 2963
	private static CinemachineVirtualCamera cachedVCam;

	// Token: 0x04000B94 RID: 2964
	public BBParameter<CinemachineVirtualCamera> target;
}
