using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x02000049 RID: 73
	[RequireComponent(typeof(Camera))]
	public class CameraToggleBeamVisibility : MonoBehaviour
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0000B794 File Offset: 0x00009994
		private void Update()
		{
			if (Input.GetKeyDown(this.m_KeyCode))
			{
				Camera component = base.GetComponent<Camera>();
				int geometryLayerID = Config.Instance.geometryLayerID;
				int num = 1 << geometryLayerID;
				if ((component.cullingMask & num) == num)
				{
					component.cullingMask &= ~num;
					return;
				}
				component.cullingMask |= num;
			}
		}

		// Token: 0x040001B7 RID: 439
		[SerializeField]
		private KeyCode m_KeyCode = KeyCode.Space;
	}
}
