using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003FB RID: 1019
	public class Revolver : MonoBehaviour
	{
		// Token: 0x06002510 RID: 9488 RVA: 0x00081B5C File Offset: 0x0007FD5C
		private void Update()
		{
			Quaternion quaternion = Quaternion.AngleAxis(Time.deltaTime * this.rPM / 60f * 360f, this.axis);
			Vector3 vector = base.transform.localPosition - this.pivot;
			Vector3 vector2 = quaternion * vector;
			Vector3 localPosition = this.pivot + vector2;
			base.transform.localPosition = localPosition;
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x00081BC4 File Offset: 0x0007FDC4
		private void OnDrawGizmosSelected()
		{
			if (base.transform.parent != null)
			{
				Gizmos.matrix = base.transform.parent.localToWorldMatrix;
			}
			Gizmos.DrawLine(this.pivot, base.transform.localPosition);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.pivot, 1f);
		}

		// Token: 0x04001929 RID: 6441
		public Vector3 pivot;

		// Token: 0x0400192A RID: 6442
		public Vector3 axis = Vector3.forward;

		// Token: 0x0400192B RID: 6443
		public float rPM;
	}
}
