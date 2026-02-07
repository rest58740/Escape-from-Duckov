using System;
using System.Collections.Generic;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x0200000F RID: 15
	[ExecuteAlways]
	[HelpURL("https://github.com/SymmetryBreakStudio/TastyGrassShader/wiki/Quick-Start")]
	[AddComponentMenu("Symmetry Break Studio/Tasty Grass Shader/Collider")]
	public class TgsCollider : MonoBehaviour
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002241 File Offset: 0x00000441
		private void OnEnable()
		{
			if (TgsCollider._activeColliders == null)
			{
				TgsCollider._activeColliders = new List<TgsCollider>(16);
			}
			TgsCollider._activeColliders.Add(this);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002261 File Offset: 0x00000461
		private void OnDisable()
		{
			if (TgsCollider._activeColliders == null)
			{
				TgsCollider._activeColliders = new List<TgsCollider>(16);
			}
			TgsCollider._activeColliders.Remove(this);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002284 File Offset: 0x00000484
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			float num = base.transform.lossyScale.magnitude * this.radius;
			Gizmos.DrawWireSphere(base.transform.position, num);
		}

		// Token: 0x04000025 RID: 37
		public static List<TgsCollider> _activeColliders = new List<TgsCollider>(16);

		// Token: 0x04000026 RID: 38
		[Tooltip("The radius of the collider.")]
		[Min(0f)]
		public float radius = 1f;
	}
}
