using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB_Samples
{
	// Token: 0x0200004E RID: 78
	public class Rotater : MonoBehaviour
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000BE84 File Offset: 0x0000A084
		private void Update()
		{
			Vector3 vector = base.transform.rotation.eulerAngles;
			vector += this.EulerSpeed * Time.deltaTime;
			base.transform.rotation = Quaternion.Euler(vector);
		}

		// Token: 0x040001C9 RID: 457
		[FormerlySerializedAs("m_EulerSpeed")]
		public Vector3 EulerSpeed = Vector3.zero;
	}
}
