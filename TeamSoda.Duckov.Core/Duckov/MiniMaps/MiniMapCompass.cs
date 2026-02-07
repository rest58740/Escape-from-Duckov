using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000286 RID: 646
	public class MiniMapCompass : MonoBehaviour
	{
		// Token: 0x060014AD RID: 5293 RVA: 0x0004CE64 File Offset: 0x0004B064
		private void SetupRotation()
		{
			Vector3 vector = UnityVectorExtensions.ProjectOntoPlane(LevelManager.Instance.GameCamera.mainVCam.transform.up, Vector3.up);
			Vector3 forward = Vector3.forward;
			float num = Vector3.SignedAngle(vector, forward, Vector3.up);
			this.arrow.localRotation = Quaternion.Euler(0f, 0f, -num);
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0004CEC2 File Offset: 0x0004B0C2
		private void Update()
		{
			this.SetupRotation();
		}

		// Token: 0x04000F3A RID: 3898
		[SerializeField]
		private Transform arrow;
	}
}
