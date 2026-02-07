using System;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace CameraSystems
{
	// Token: 0x0200021B RID: 539
	public class CameraPropertiesControl : MonoBehaviour
	{
		// Token: 0x06001053 RID: 4179 RVA: 0x00040A00 File Offset: 0x0003EC00
		private void Awake()
		{
			this.vCam = base.GetComponent<CinemachineVirtualCamera>();
			this.volumeSettings = base.GetComponent<CinemachineVolumeSettings>();
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00040A1C File Offset: 0x0003EC1C
		private unsafe void Update()
		{
			float num = *Gamepad.current.dpad.x.value;
			if (*Gamepad.current.dpad.y.value != 0f)
			{
				float num2 = -(*Gamepad.current.dpad.y.value);
				if (*Gamepad.current.rightShoulder.value > 0f)
				{
					num2 *= 10f;
				}
				this.vCam.m_Lens.FieldOfView = Mathf.Clamp(this.vCam.m_Lens.FieldOfView + num2 * 5f * Time.deltaTime, 8f, 100f);
			}
		}

		// Token: 0x04000D2F RID: 3375
		private CinemachineVirtualCamera vCam;

		// Token: 0x04000D30 RID: 3376
		private CinemachineVolumeSettings volumeSettings;

		// Token: 0x04000D31 RID: 3377
		[SerializeField]
		private VolumeProfile volumeProfile;
	}
}
